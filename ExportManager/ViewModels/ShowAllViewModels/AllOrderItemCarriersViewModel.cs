using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Parameters;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllOrderItemCarriersViewModel : AllViewModel<OrderItemCarriersListView>
    {
        #region Fields
        private readonly int _orderId;
        private ObservableCollection<OrderItemsListView> _AssignedOrderItems;
        private ObservableCollection<OrderItemsListView> _UnassignedOrderItems;
        private OrderItemsListView _SelectedAssignedOrderItem;
        #endregion
        #region Constructor
        public AllOrderItemCarriersViewModel(int orderId)
            : base()
        {
            _orderId = orderId;
            base.DisplayName = new OrderDetailsQuery(potplantsEntities).GetOrderDisplayName(orderId) + " carriers";
            base.FullDisplayName = new OrderDetailsQuery(potplantsEntities).GetOrderFullDisplayName(orderId) + " carriers";
        }
        #endregion
        #region Properties
        public ObservableCollection<OrderItemsListView> AssignedOrderItems
        {
            get
            {
                return _AssignedOrderItems;
            }
            set
            {
                if (_AssignedOrderItems != value)
                {
                    _AssignedOrderItems = value;
                    OnPropertyChanged(() => AssignedOrderItems);
                }

            }
        }
        public OrderItemsListView SelectedAssignedOrderItem
        {
            get
            {
                return _SelectedAssignedOrderItem;
            }
            set
            {
                if (_SelectedAssignedOrderItem != value)
                {
                    _SelectedAssignedOrderItem = value;
                    OnPropertyChanged(() => SelectedAssignedOrderItem);
                }
            }
        }
        #endregion
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                var carriers = shortLivedPotplantsEntities.Carriers
                .Where(c => c.OrderId == _orderId)
                .Select(c => new
                {
                    c.CarrierId,
                    c.OrderId,
                    CarrierType = c.CarrierTypes.Name,
                    c.AmountOfExtensions,
                    c.AmountOfShelfs,
                    OrderItemIds = c.OrderItems.Select(oi => oi.OrderItemId).ToList(),
                    AmountOfPlants = c.OrderItems.Sum(oi => (int?)oi.Quantity) ?? 0
                })
            .ToList();
                List = new ObservableCollection<OrderItemCarriersListView>(
                from carrier in carriers
                where carrier.OrderId == _orderId
                select new OrderItemCarriersListView
                {
                    CarrierId = carrier.CarrierId,
                    OrderId = carrier.OrderId,
                    CarrierType = carrier.CarrierType,
                    AmountOfExtensions = carrier.AmountOfExtensions,
                    AmountOfShelves = carrier.AmountOfShelfs,
                    OrderItemIds = carrier.OrderItemIds,
                    AmountOfPlants = carrier.AmountOfPlants
                });
            }
        }
        #endregion
        #region Commands
        private BaseCommand _LoadAssignedOrderItems;
        public ICommand LoadAssignedOrderItemsCommand
        {
            get
            {
                if (_LoadAssignedOrderItems == null)
                    _LoadAssignedOrderItems = new BaseCommand(LoadAssignedOrderItems);
                return _LoadAssignedOrderItems;
            }
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OnRequestWindow<NewOrderItemCarrierViewModel>(new NewOrderItemCarrierWindowParameter(
                _orderId, 
                new OrderDetailsQuery(potplantsEntities).GetOrderFullDisplayName(_orderId) + " - New carrier",
                Load
                ));
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        public void LoadAssignedOrderItems()
        {
            if (SelectedItem == null)
                return;
            AssignedOrderItems.Clear();
            AssignedOrderItems = new ObservableCollection<OrderItemsListView>(
                potplantsEntities.OrderItems.Where(oi => SelectedItem.OrderItemIds.Contains(oi.OrderItemId)).Select(oi => new OrderItemsListView
                {
                    OrderItemId = oi.OrderItemId,
                    StockItemId = oi.StockItemId,
                    ProductName = oi.StockItems.Products.Name,
                    ProductHeight = oi.StockItems.Products.Height,
                    ProductPotsize = oi.StockItems.Products.Potsize,
                    Quantity = oi.Quantity,
                    InternalNo = oi.InternalNo,
                    ProductInternalNo = oi.StockItems.InternalNo,
                    TrayType = oi.StockItems.TrayTypes.Name,
                    Quality = oi.StockItems.Qualities.Name
                }));
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
