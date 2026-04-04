using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class VolumeCalculator: DatabaseClass
    {
        #region Fields
        private const decimal CarrierHeightClearance = 20m;
        private const decimal ShelfClearance = 5m;
        #endregion
        #region Constructor
        public VolumeCalculator(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        private IQueryable<Orders> GetOrdersQuery(int clientId, DateTime dateFrom, DateTime dateTo)
        {
            DateTime dateFromIncluded = dateFrom.Date;
            DateTime dateToIncluded = dateTo.AddDays(1);
            return potplantsEntities.Orders.Where(
                t => t.IsActive && t.ClientId == clientId && t.OrderDate >= dateFromIncluded && t.OrderDate <= dateToIncluded
                );
        }
        private IQueryable<dynamic> GetProductsQuery(int clientId, DateTime dateFrom, DateTime dateTo)
        {
            return GetOrdersQuery(clientId, dateFrom, dateTo).SelectMany(o => o.OrderItems.Select(oi => new
            {
                oi.StockItems.TrayTypes.Width,
                oi.StockItems.TrayTypes.Length,
                oi.StockItems.Products.Height,
                oi.StockItems.TrayTypes.QtyPerTray,
                oi.Quantity
            }
                        )
                ).GroupBy(t => new
                {
                    t.Width,
                    t.Length,
                    t.Height,
                    t.QtyPerTray
                }).Select(t => new
                {
                    t.Key.Width,
                    t.Key.Length,
                    t.Key.Height,
                    t.Key.QtyPerTray,
                    TotalQty = t.Sum(x => x.Quantity)
                });
        }
        public decimal? CalculateVolumePerClientPerPeriod(int clientId, DateTime dateFrom, DateTime dateTo, int carrierTypeId)
        {
            
            var carrier = potplantsEntities.CarrierTypes.Where(t => t.IsActive == true && t.CarrierTypeId == carrierTypeId)
                .Select(t => new{ t.Area, t.MaxHeight}).FirstOrDefault();
            if (carrier == null)
                return null;
            if (carrier.Area == null || carrier.MaxHeight == null)
                return null;
            decimal? carrierVolume = carrier.Area * (carrier.MaxHeight - CarrierHeightClearance);
            var products = GetProductsQuery(clientId, dateFrom, dateTo).ToList();
            decimal? productsVolume = 0m;
            foreach(var product in products)
            {
                var traysNeeded = Math.Ceiling(product.TotalQty / (decimal)product.QtyPerTray);
                var trayVolume = product.Length * product.Width * (product.Height + ShelfClearance);
                productsVolume += trayVolume * traysNeeded;
            }
            decimal result = (decimal)(productsVolume / carrierVolume);
            return Math.Round(result, 2, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
