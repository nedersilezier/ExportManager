using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class OrderWeightCalculator: DatabaseClass
    {
        #region Fields
        #endregion
        #region Constructor
        public OrderWeightCalculator(PotplantsEntities potplantsEntities)
            : base(potplantsEntities){
        }
        #endregion
        #region Functions
        //public decimal? CalculateOrderWeight(int orderId, DateTime date)
        //{
        //    DateTime startDate = date.Date;
        //    DateTime endDate = startDate.Date.AddDays(1);
        //    decimal? carriersWeight = 0;
        //    var carriers = potplantsEntities.Carriers.Where(t => t.IsActive == true && t.OrderId == orderId)
        //        .Select(t => new { t.CarrierTypes.Weight, t.CarrierTypes.ShelfWeight, t.AmountOfShelfs });
        //    if (carriers != null)
        //    {
        //        foreach (var carrier in carriers)
        //        {
        //            Console.WriteLine("Carrier 1: " + carrier.Weight + " " + carrier.ShelfWeight + " " + carrier.AmountOfShelfs);
        //            carriersWeight += carrier.Weight + carrier.ShelfWeight * carrier.AmountOfShelfs;
        //        }
        //    }
        //    Console.WriteLine("Carriers weight: " + carriersWeight);
        //    var products = (
        //        from order in potplantsEntities.Orders
        //        join orderitem in potplantsEntities.OrderItems on order.OrderId equals orderitem.OrderId
        //        join product in potplantsEntities.Products on orderitem.StockItems.Products.ProductId equals product.ProductId
        //        where order.IsActive == true
        //        && order.PreparationDate >= startDate
        //        && order.PreparationDate <= endDate
        //        && orderitem.IsActive == true
        //        group orderitem by new
        //        {
        //            ProductWeight = product.Weight
        //        }
        //        into grouped
        //        select new
        //        {
        //            grouped.Key.ProductWeight,
        //            TotalQty = grouped.Sum(t => t.Quantity)
        //        }
        //        ).ToList();
        //    decimal? totalWeight = carriersWeight;
        //    foreach (var product in products)
        //    {
        //        Console.WriteLine("Product amount: " + product.TotalQty);
        //        Console.WriteLine("Product weight: " + product.ProductWeight);
        //        totalWeight += product.TotalQty * product.ProductWeight;
        //    }
        //    return totalWeight;
        //}
        public decimal? CalculateOrderWeight(int orderId, DateTime date)
        {
            var carriersWeight = CarriersQuery(orderId, date).Sum(t => (decimal?)t.CarriersTotalWeight);
            var productsWeight = ProductsQuery(orderId, date).Sum(t => (decimal?)t.ProductTotalWeight);
            return carriersWeight + productsWeight;
        }
        private IQueryable<Orders> OrdersQuery(int orderId, DateTime date)
        {
            DateTime startDate = date.Date;
            DateTime endDate = startDate.AddDays(1);
            return potplantsEntities.Orders.Where(
                t => t.IsActive 
                && t.OrderId == orderId 
                && t.PreparationDate >= startDate 
                && t.PreparationDate < endDate);
        }
        public IQueryable<WeightReportProductListView> ProductsQuery(int orderId, DateTime date)
        {
            return OrdersQuery(orderId, date).SelectMany(order => order.OrderItems)
                .Where(orderitem => orderitem.IsActive)
                .GroupBy(orderitem => orderitem.StockItems.Products.Name)
                .Select(t => new WeightReportProductListView
                {
                    ProductName = t.Key,
                    ProductTotalAmount = t.Sum(orderitem => orderitem.Quantity),
                    ProductTotalWeight = t.Sum(orderitem => orderitem.StockItems.Products.Weight * orderitem.Quantity)
                });
        }
        public IQueryable<WeightReportCarrierListView> CarriersQuery(int orderId, DateTime date)
        {
            return OrdersQuery(orderId, date).SelectMany(order => order.Carriers)
                .Where(carrier => carrier.IsActive)
                .GroupBy(carrier => new { carrier.CarrierTypes.CarrierTypeId, carrier.CarrierTypes.Name})
                .Select(t => new WeightReportCarrierListView
                {
                    CarrierName = t.Key.Name,
                    CarrierTotalAmount = t.Count(),
                    CarrierTotalShelfsAmount = t.Sum(carrier => carrier.AmountOfShelfs),
                    CarriersTotalWeight = 
                                    t.Count() * t.Select(u => u.CarrierTypes.Weight).FirstOrDefault() 
                                    + t.Sum(carrier => carrier.AmountOfShelfs) * t.Select(u => u.CarrierTypes.ShelfWeight).FirstOrDefault()
                });
        }
        #endregion
    }
}
