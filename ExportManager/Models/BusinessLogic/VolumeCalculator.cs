using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class VolumeCalculator: DatabaseClass
    {
        #region Constructor
        public VolumeCalculator(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public decimal? CalculateVolumePerClientPerPeriod(int clientId, DateTime dateFrom, DateTime dateTo, int carrierTypeId)
        {
            DateTime dateFromIncluded = dateFrom.Date;
            DateTime dateToIncluded = dateTo.AddDays(1);
            var carrier = potplantsEntities.CarrierTypes.Where(t => t.IsActive == true && t.CarrierTypeId == carrierTypeId)
                .Select(t => new{ t.Width, t.Length, t.MaxHeight}).FirstOrDefault();
            if (carrier == null)
                return null;
            if (carrier.Length == null || carrier.Width == null || carrier.MaxHeight == null)
                return null;
            decimal? carrierVolume = carrier.Width * carrier.Length * carrier.MaxHeight;
            //Console.WriteLine("Carrier volume: " +  carrierVolume);
            var products = (
                from order in potplantsEntities.Orders
                join orderitem in potplantsEntities.OrderItems on order.OrderId equals orderitem.OrderId
                join stockitem in potplantsEntities.StockItems on orderitem.StockItemId equals stockitem.StockItemId
                join traytype in potplantsEntities.TrayTypes on stockitem.TrayTypeId equals traytype.TrayTypeId
                join product in potplantsEntities.Products on stockitem.ProductId equals product.ProductId
                where order.IsActive == true
                      && order.ClientId == clientId
                      && order.OrderDate >= dateFromIncluded
                      && order.OrderDate <= dateToIncluded
                group orderitem by new
                {
                    traytype.Width,
                    traytype.Length,
                    traytype.QtyPerTray,
                    product.Height
                }
                into grouped
                select new
                {
                    TrayWidth = grouped.Key.Width,
                    TrayLength = grouped.Key.Length,
                    grouped.Key.QtyPerTray,
                    grouped.Key.Height,
                    TotalQty = grouped.Sum(t => t.Quantity)
                }
                ).ToList();
            decimal? productsVolume = 0m;
            foreach(var product in products)
            {
                var traysNeeded = (int)Math.Ceiling(product.TotalQty / (decimal)product.QtyPerTray);
                //Console.WriteLine("Trays needed: " + traysNeeded);
                var trayVolume = product.TrayLength * product.TrayWidth * product.Height;
                //Console.WriteLine("Length: " + product.TrayLength);
                //Console.WriteLine("Width: " + product.TrayWidth);
                //Console.WriteLine("Height: " + product.Height);
                //Console.WriteLine("Tray volume: " + trayVolume);
                productsVolume += trayVolume * traysNeeded;
                //Console.WriteLine("Products volume: " + productsVolume);
            }
            //Console.WriteLine("Products volume: " + productsVolume);
            decimal result = (decimal)(productsVolume / carrierVolume);
            return Math.Round(result, 2, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
