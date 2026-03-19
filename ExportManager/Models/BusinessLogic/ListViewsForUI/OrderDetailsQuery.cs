using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class OrderDetailsQuery:DatabaseClass
    {
        #region Constructor
        public OrderDetailsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<Orders> GetOrderById(int orderId)
        {
                       return potplantsEntities.Orders.Where(o => o.IsActive && o.OrderId == orderId);
        }
        public IQueryable<AddressesListView> GetOrdersDeliveryAddress(int orderId)
        {
            return GetOrderById(orderId).Select(o => new AddressesListView
            {
                AddressId = o.Addresses.AddressId,
                Country = o.Addresses.Countries.Name,
                City = o.Addresses.City,
                Street = o.Addresses.Street,
                HouseNumber = o.Addresses.HouseNumber,
                ApartmentNumber = o.Addresses.ApartmentNumber,
                ZipCode = o.Addresses.ZipCode
            });
        }
        public string GetOrderDisplayName(int orderId)
        {
            return GetOrderById(orderId)
                .Select(o => o.Clients.ClientCode).FirstOrDefault();
        }
        public string GetOrderFullDisplayName(int orderId)
        {
            return GetOrderById(orderId)
                .Select(o => new
                {
                    o.Clients.ClientCode,
                    o.OrderDate
                })
                .AsEnumerable()
                .Select(o => $"{o.ClientCode} {o.OrderDate:dd-MM-yyyy}").FirstOrDefault();
        }
        public bool OrderContainsActiveItem(int orderId, int stockItemId)
        {
            return GetOrderById(orderId).SelectMany(o => o.OrderItems).Any(
                oi => oi.StockItemId == stockItemId
                && oi.IsActive == true 
                && oi.Quantity > 0);
        }
        #endregion
    }
}
