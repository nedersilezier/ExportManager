using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class OrdersQuery:DatabaseClass
    {
        #region Constructor
        public OrdersQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<Orders> GetActiveOrders()
        {
            return potplantsEntities.Orders.Where(o => o.IsActive == true);
        }
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
        public ObservableCollection<KeyAndValue> GetOrdersListItems()
        {
            return new ObservableCollection<KeyAndValue>(
                potplantsEntities.Orders.Where(t => t.IsActive == true).Select(t => new KeyAndValue
                {
                    Key = t.OrderId,
                    Value = t.Clients.ClientCode + " | " + t.Clients.Name
                }));
        }
        public ObservableCollection<KeyAndValue> GetOrdersListItemsPerDate(DateTime date)
        {
            DateTime dateStart = date.Date;
            DateTime dateEnd = dateStart.Date.AddDays(1);
            return new ObservableCollection<KeyAndValue>(
                potplantsEntities.Orders.Where(t => t.IsActive == true && t.PreparationDate >= dateStart && t.PreparationDate <= dateEnd).Select(t => new KeyAndValue
                {
                    Key = t.OrderId,
                    Value = t.Clients.ClientCode + " | " + t.Clients.Name
                }));
        }
        public HashSet<DateTime> GetOrderDates()
        {
            return GetActiveOrders().Select(o => o.OrderDate).Distinct().ToHashSet();
        }
        public HashSet<DateTime> GetOrderDatesPerClient(int clientId)
        {
            return GetActiveOrders().Where(o => o.ClientId == clientId).Select(o => o.OrderDate).ToList().Select(d => d.Date).Distinct().ToHashSet();
        }
        #endregion
    }
}
