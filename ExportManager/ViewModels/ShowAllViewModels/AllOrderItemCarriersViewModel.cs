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
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
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
        private OrderItemCarriersListView _SelectedCarrier;
        private OrderItemsListView _SelectedAssignedOrderItem;
        private OrderItemsListView _SelectedUnassignedOrderItem;
        private bool _IsOrderItemsChanged;
        #endregion
        #region Constructor
        public AllOrderItemCarriersViewModel(int orderId)
            : base()
        {
            _orderId = orderId;
            _IsOrderItemsChanged = false;
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
        public ObservableCollection<OrderItemsListView> UnassignedOrderItems
        {
            get { return _UnassignedOrderItems; }
            set
            {
                if (_UnassignedOrderItems != value)
                {
                    _UnassignedOrderItems = value;
                    OnPropertyChanged(() => UnassignedOrderItems);
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
        public OrderItemsListView SelectedUnassignedOrderItem
        {
            get
            {
                return _SelectedUnassignedOrderItem;
            }
            set
            {
                if (_SelectedUnassignedOrderItem != value)
                {
                    _SelectedUnassignedOrderItem = value;
                    OnPropertyChanged(() => SelectedUnassignedOrderItem);
                }
            }
        }
        public OrderItemCarriersListView SelectedCarrier
        {
            get { return _SelectedCarrier; }
            set
            {
                if (_SelectedCarrier != value)
                {
                    if (_IsOrderItemsChanged)
                        UpdateDatabase();
                    _SelectedCarrier = value;
                    OnPropertyChanged(() => SelectedCarrier);
                    LoadAssignedOrderItems();
                }
            }
        }
        public bool IsChanged
        {
            get { return _IsOrderItemsChanged; }
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
            LoadUnAssignedOrderItems();
            //if (_IsOrderItemsChanged)
            //    UpdateDatabase();
        }
        #endregion
        #region Commands
        //private BaseCommand _LoadAssignedOrderItems;
        //public ICommand LoadAssignedOrderItemsCommand
        //{
        //    get
        //    {
        //        if (_LoadAssignedOrderItems == null)
        //            _LoadAssignedOrderItems = new BaseCommand(LoadAssignedOrderItems);
        //        return _LoadAssignedOrderItems;
        //    }
        //}
        private BaseCommand _AssignOrderItemCommand;
        public ICommand AssignOrderItemCommand
        {
            get
            {
                if (_AssignOrderItemCommand == null)
                    _AssignOrderItemCommand = new BaseCommand(AssignOrderItem);
                return _AssignOrderItemCommand;
            }
        }
        private BaseCommand _UnassignOrderItemCommand;
        public ICommand UnassignOrderItemCommand
        {
            get
            {
                if (_UnassignOrderItemCommand == null)
                    _UnassignOrderItemCommand = new BaseCommand(UnassignOrderItem);
                return _UnassignOrderItemCommand;
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
            if (SelectedCarrier == null)
                return;
            AssignedOrderItems = new ObservableCollection<OrderItemsListView>(
                potplantsEntities.OrderItems.Where(oi => SelectedCarrier.OrderItemIds.Contains(oi.OrderItemId)).Select(oi => new OrderItemsListView
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
        public void LoadUnAssignedOrderItems()
        {
            UnassignedOrderItems = new ObservableCollection<OrderItemsListView>(
                potplantsEntities.OrderItems.Where(oi => oi.OrderId == _orderId
                && oi.IsScanned == false
                && oi.IsActive == true).Select(oi => new OrderItemsListView
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
        private void AssignOrderItem()
        {
            if (SelectedCarrier == null)
            {
                MessageBox.Show("No carrier selected.");
                return;
            }

            if (SelectedUnassignedOrderItem == null)
            {
                MessageBox.Show("No order item selected to assign.");
                return;
            }
            int carrierindex = List.IndexOf(SelectedCarrier);
            int itemindex = UnassignedOrderItems.IndexOf(SelectedUnassignedOrderItem);
            AssignedOrderItems.Add(SelectedUnassignedOrderItem);
            SelectedCarrier.OrderItemIds.Add(SelectedUnassignedOrderItem.OrderItemId);
            SelectedCarrier.AmountOfPlants += SelectedUnassignedOrderItem.Quantity;
            OnPropertyChanged(() => List);
            SelectedCarrier = List[carrierindex];
            UnassignedOrderItems.Remove(SelectedUnassignedOrderItem);
            if (UnassignedOrderItems.Count > 0)
            {
                if (itemindex >= UnassignedOrderItems.Count)
                    itemindex = UnassignedOrderItems.Count - 1;
                SelectedUnassignedOrderItem = UnassignedOrderItems[itemindex];
            }
            _IsOrderItemsChanged = true;
        }
        private void UnassignOrderItem()
        {
            if (SelectedCarrier == null)
            {
                MessageBox.Show("No carrier selected.");
                return;
            }
            if (SelectedAssignedOrderItem == null)
            {
                MessageBox.Show("No order item selected to remove from the carrier.");
                return;
            }
            int carrierindex = List.IndexOf(SelectedCarrier);
            int itemindex = AssignedOrderItems.IndexOf(SelectedAssignedOrderItem);
            UnassignedOrderItems.Add(SelectedAssignedOrderItem);
            SelectedCarrier.OrderItemIds.Remove(SelectedAssignedOrderItem.OrderItemId);
            SelectedCarrier.AmountOfPlants -= SelectedAssignedOrderItem.Quantity;
            OnPropertyChanged(() => List);
            SelectedCarrier = List[carrierindex];
            AssignedOrderItems.Remove(SelectedAssignedOrderItem);
            if (AssignedOrderItems.Count > 0)
            {
                if (itemindex >= AssignedOrderItems.Count)
                    itemindex = AssignedOrderItems.Count - 1;
                SelectedAssignedOrderItem = AssignedOrderItems[itemindex];
            }
            _IsOrderItemsChanged = true;
        }
        private void UpdateDatabase()
        {
            var carrierIds = List.Select(c => c.CarrierId).ToList();
            var orderItemIds = List.SelectMany(c => c.OrderItemIds).Distinct().ToList();

            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                var carriersEF = shortLivedPotplantsEntities.Carriers.Include(c => c.OrderItems).Where(c => carrierIds.Contains(c.CarrierId)).ToList();
                var carriersDict = carriersEF.ToDictionary(c => c.CarrierId);
                var orderItemsEF = shortLivedPotplantsEntities.OrderItems.Where(oi => orderItemIds.Contains(oi.OrderItemId)).ToList();
                var orderItemsDict = orderItemsEF.ToDictionary(oi => oi.OrderItemId);
                foreach (var carrier in List)
                {
                    var carrierEF = carriersDict[carrier.CarrierId];
                    carrierEF.AmountOfShelfs = carrier.AmountOfShelves;
                    carrierEF.AmountOfExtensions = carrier.AmountOfExtensions;
                    var currentOrderItemIds = carrierEF.OrderItems.Select(oi => oi.OrderItemId).ToHashSet();
                    var newOrderItemIds = carrier.OrderItemIds.ToHashSet();
                    var toRemove = carrierEF.OrderItems.Where(oi => !newOrderItemIds.Contains(oi.OrderItemId)).ToList();
                    foreach (var itemToRemove in toRemove)
                    {
                        itemToRemove.IsScanned = false;
                        carrierEF.OrderItems.Remove(itemToRemove);
                    }
                    var idsToAdd = newOrderItemIds.Except(currentOrderItemIds);
                    foreach (var idToAdd in idsToAdd)
                    {
                        if (orderItemsDict.TryGetValue(idToAdd, out var orderitem))
                        {
                            orderitem.IsScanned = true;
                            carrierEF.OrderItems.Add(orderitem);
                        }
                    }

                }
                shortLivedPotplantsEntities.SaveChanges();
            }
            _IsOrderItemsChanged = false;
        }
        //private void UpdateDatabase()
        //{
        //    using (var shortLivedPotplantsEntities = new PotplantsEntities())
        //    {
        //        foreach (var carrier in List)
        //        {
        //            var carrierEF = shortLivedPotplantsEntities.Carriers.Include(c => c.OrderItems).First(c => c.CarrierId == carrier.CarrierId);
        //            var toRemove = carrierEF.OrderItems.Where(oi => !carrier.OrderItemIds.Contains(oi.OrderItemId)).ToList();
        //            foreach (var itemToRemove in toRemove)
        //            {
        //                itemToRemove.IsScanned = false;
        //                carrierEF.OrderItems.Remove(itemToRemove);
        //            }
        //            var oldIDset = carrierEF.OrderItems.Select(oi => oi.OrderItemId).ToList();
        //            var idsToAdd = carrier.OrderItemIds.Except(oldIDset).ToList();
        //            foreach (var idToAdd in idsToAdd)
        //            {
        //                var orderitem = shortLivedPotplantsEntities.OrderItems.Find(idToAdd);
        //                if (orderitem != null)
        //                {
        //                    orderitem.IsScanned = true;
        //                    carrierEF.OrderItems.Add(orderitem);
        //                }

        //            }

        //        }
        //        shortLivedPotplantsEntities.SaveChanges();
        //    }
        //    _IsOrderItemsChanged = false;
        //}
        public void SaveChangesExternal()
        {
            if (_IsOrderItemsChanged)
                UpdateDatabase();
        }
        protected override void OnRequestClose()
        {
            if (_IsOrderItemsChanged)
                UpdateDatabase();
            base.OnRequestClose();
        }
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Edit shelves/extensions", new BaseCommand(OnEditShelvesExtensions))
                };
        }
        private void OnEditShelvesExtensions()
        {
            if(SelectedCarrier == null)
            {
                MessageBox.Show("No carrier selected.");
                return;
            }    
            OnRequestWindow<EditCarrierAddonsViewModel>(new OrderItemCarrierParameter(
                SelectedCarrier.CarrierId,
                SelectedCarrier.CarrierType,
                SelectedCarrier.AmountOfShelves,
                SelectedCarrier.AmountOfExtensions,
                Load
                ));
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
