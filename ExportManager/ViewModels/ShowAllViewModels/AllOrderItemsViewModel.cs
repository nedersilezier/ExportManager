using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.Models.EntitiesForView;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllOrderItemsViewModel : AllViewModel<OrderItemsListView>
    {
        #region Fields
        private int _OrderId;
        #endregion
        #region List
        public override void Load()
        {
            //List = new ObservableCollection<dynamic>(potplantsEntities.OrderItems.Where(t => t.IsActive == true && t.OrderId == _OrderId).ToList());
            List = new ObservableCollection<OrderItemsListView>(
                from orderitem in potplantsEntities.OrderItems
                where orderitem.IsActive == true && orderitem.OrderId == _OrderId
                select new OrderItemsListView
                {
                    OrderItemId = orderitem.OrderItemId,
                    StockItemId = orderitem.StockItemId,
                    ProductName = orderitem.StockItems.Products.Name,
                    ProtuctHeight = orderitem.StockItems.Products.Height,
                    ProductPotsize = orderitem.StockItems.Products.Potsize,
                    Quantity = orderitem.Quantity,
                    UnitPrice = orderitem.UnitPrice,
                    StorageCost = orderitem.StorageCost,
                    TransportCost = orderitem.TransportCost,
                    Discount = orderitem.Discount,
                    TotalPrice = orderitem.TotalPrice,
                    Remarks = orderitem.Remarks,
                    InternalNo = orderitem.InternalNo,
                    ProductInternalNo = orderitem.StockItems.InternalNo,
                    GrowerName = orderitem.StockItems.Growers.Name,
                    TrayType = orderitem.StockItems.TrayTypes.Name,
                    Quality = orderitem.StockItems.Qualities.Name,
                    OrderedDate = orderitem.OrderedDate,
                    IsScanned = orderitem.IsScanned,
                });
        }
        #endregion
        #region Constructor
        public AllOrderItemsViewModel(int orderId)
            :base()
        {
                _OrderId = orderId;
            base.DisplayName = "Order items";
        }
        #endregion

        #region Functions
        public override void OnAdd()
        {
            return;
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Not implemented yet" };
        }
        public override void Sort()
        {
            return;
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Not implemented yet" };
        }
        public override void Find()
        {
            return;
        }
        #endregion
    }
}
