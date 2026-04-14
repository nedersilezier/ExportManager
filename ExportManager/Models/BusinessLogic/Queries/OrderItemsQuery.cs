using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class OrderItemsQuery : DatabaseClass
    {
        #region Constructor
        public OrderItemsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<OrderItems> GetOrderItems(int orderId)
        {
            return potplantsEntities.OrderItems.Where(oi => oi.IsActive == true && oi.OrderId == orderId);
        }
        public List<InvoiceItemsListView> GetInvoiceItemsPreview(int orderId, decimal margin)
        {
            var orderitems = GetOrderItems(orderId).ToList();
            var TransportSum = orderitems.Sum(oi => oi.TransportCost);
            var StorageSum = orderitems.Sum(oi => oi.StorageCost);
            var list = new List<InvoiceItemsListView>
            {
                new InvoiceItemsListView
                {
                    SourceOrderItemId = null,
                    ItemNo = 1,
                    Name = "Transport",
                    Potsize = null,
                    Height = null,
                    Quantity = 1,
                    UnitPrice = TransportSum ?? 0,
                    NetAmount = TransportSum,
                    GrossAmount = 0,
                    TaxAmount = 0
                },
                new InvoiceItemsListView
                {
                    SourceOrderItemId = null,
                    ItemNo = 2,
                    Name = "Storage",
                    Potsize = null,
                    Height = null,
                    Quantity = 1,
                    UnitPrice = StorageSum ?? 0,
                    NetAmount = StorageSum,
                    GrossAmount = 0,
                    TaxAmount = 0
                }
            };
            int nextItemNo = 3;
            list.AddRange(orderitems.Select(oi => new InvoiceItemsListView
            {
                SourceOrderItemId = oi.OrderItemId,
                ItemNo = nextItemNo++,
                Name = oi.StockItems.Products.Name,
                Potsize = oi.StockItems.Products.Potsize,
                Height = oi.StockItems.Products.Height,
                Quantity = oi.Quantity,
                Discount = oi.Discount ?? 0,
                UnitPrice = oi.UnitPrice ?? 0,
                NetAmount = oi.TotalPrice,
                TaxAmount = oi.TotalPrice * margin / 100,
                GrossAmount = margin > 0 ? oi.TotalPrice * (1 + margin / 100) : 0
            }));
            return list;
        }
        #endregion
    }
}
