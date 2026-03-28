using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Extensions;
using ExportManager.Models.Parameters;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllOrdersViewModel : AllViewModel<OrdersListView>
    {
        #region List
        private Func<IQueryable<OrdersListView>, IQueryable<OrdersListView>> _ordersFilter = null;

        private IQueryable<OrdersListView> GetOrders()
        {
            var query = potplantsEntities.Orders.Where(o => o.IsActive == true)
                .Select(order => new OrdersListView
                {
                    OrderId = order.OrderId,
                    ClientId = order.Clients.ClientId,
                    ClientCode = order.Clients.ClientCode,
                    ClientName = order.Clients.Name,
                    OrderDate = order.OrderDate,
                    PreparationDate = order.PreparationDate,
                    ShipmentDate = order.ShipmentDate,
                    DeliveryDate = order.DeliveryDate,
                    Country = order.Clients.Addresses.Countries.Name,
                    SalesPerson = order.SalesPerson,
                    Status = order.Status,
                    Remarks = order.Remarks
                });

            if (_ordersFilter != null)
                query = _ordersFilter(query);

            return query;
        }
        public override void Load()
        {
            //List = new ObservableCollection<OrdersListView>(
            //    from order in potplantsEntities.Orders
            //    where order.IsActive == true
            //    select new OrdersListView
            //    {
            //        OrderId = order.OrderId,
            //        ClientId = order.Clients.ClientId,
            //        ClientCode = order.Clients.ClientCode,
            //        ClientName = order.Clients.Name,
            //        OrderDate = order.OrderDate,
            //        PreparationDate = order.PreparationDate,
            //        ShipmentDate = order.ShipmentDate,
            //        DeliveryDate = order.DeliveryDate,
            //        Country = order.Clients.Addresses.Countries.Name,
            //        SalesPerson = order.SalesPerson,
            //        Status = order.Status,
            //        Remarks = order.Remarks
            //    });
            List = new ObservableCollection<OrdersListView>(GetOrders().ToList());
        }
        #endregion
        #region Constructor
        public AllOrdersViewModel()
            : base()
        {
            base.DisplayName = "Orders";
        }
        public AllOrdersViewModel(Action<OrderSelectionResult> itemSetter)
            : base(itemSetter,
                 generateArgsFromSelection:
                 selectedItem => (object)new OrderSelectionResult(
                     selectedItem.OrderId,
                     selectedItem.ClientId,
                     selectedItem.ClientCode,
                     selectedItem.ClientName,
                     selectedItem.OrderDate,
                     selectedItem.DeliveryDate))
        {
            _ordersFilter = q => q.Where(o => o.Status == OrderStatuses.Closed);
            base.DisplayName = "Select the order";
        }
        #endregion
        #region Commands
        private BaseCommand _ShowDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                if (_ShowDetailsCommand == null)
                    _ShowDetailsCommand = new BaseCommand(OnShowDetails);
                return _ShowDetailsCommand;
            }
        }
        private BaseCommand _ShowCarriersCommand;
        public ICommand ShowCarriersCommand
        {
            get
            {
                if (_ShowCarriersCommand == null)
                    _ShowCarriersCommand = new BaseCommand(OnShowCarriers);
                return _ShowCarriersCommand;
            }
        }
        private BaseCommand _CloseOrderCommand;
        public ICommand CloseOrderCommand
        {
            get
            {
                if (_CloseOrderCommand == null)
                    _CloseOrderCommand = new BaseCommand(OnCloseOrder);
                return _CloseOrderCommand;
            }
        }
        private BaseCommand _CancelOrderCommand;
        public ICommand CancelOrderCommand
        {
            get
            {
                if (_CancelOrderCommand == null)
                    _CancelOrderCommand = new BaseCommand(OnCancelOrder);
                return _CancelOrderCommand;
            }
        }

        public override IList<CommandViewModel> CreateExtraCommands()
        {
            if (IsSelectMode)
                return base.CreateExtraCommands();
            return new List<CommandViewModel>
            {
                new CommandViewModel("Order items", ShowDetailsCommand),
                new CommandViewModel("Carriers", ShowCarriersCommand),
                new CommandViewModel("Close order", CloseOrderCommand),
                new CommandViewModel("Cancel order", CancelOrderCommand),
            };
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewOrderViewModel(), Load);
        }
        public override void OnEdit()
        {
            if (SelectedItem.Status == OrderStatuses.Open)
                OpenNewTab(() => new NewOrderViewModel(SelectedItem.OrderId), Load);
            else
                MessageBox.Show("This order cannot be modified anymore.");
        }
        public override void Remove()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No order selected.");
                return;
            }
            OnRemove();
        }
        public override void OnRemove()
        {
            var result = MessageBox.Show(
                        "Delete this order? All asigned orderitems will return to stock",
                        $"{SelectedItem.ClientName}",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var transaction = potplantsEntities.Database.BeginTransaction())
                {
                    try
                    {
                        OrderCleanup(potplantsEntities);
                        SoftDelete<Orders>(SelectedItem.OrderId);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }
        private void OnShowDetails()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("No order selected.");
                return;
            }
            if (SelectedItem.Status != OrderStatuses.Open)
            {
                ShowMessageBox("Not available.");
                return;
            }
            OpenNewTab(() => new AllOrderItemsViewModel(SelectedItem.OrderId));
        }
        private void OnShowCarriers()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("No order selected.");
                return;
            }
            if (SelectedItem.Status != OrderStatuses.Open)
            {
                ShowMessageBox("Not available.");
                return;
            }
            OpenNewTab(() => new AllOrderItemCarriersViewModel(SelectedItem.OrderId));
        }
        private void OnCancelOrder()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No order selected.");
                return;
            }
            if (SelectedItem.Status != OrderStatuses.Open)
            {
                ShowMessageBox("Not available.");
                return;
            }
            var result = MessageBox.Show(
                        "Cancel this order? All asigned orderitems will return to stock",
                        $"{SelectedItem.ClientName}",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var shortLivedPotplantsEntities = new PotplantsEntities())
                {
                    var order = shortLivedPotplantsEntities.Orders.Where(o => o.OrderId == SelectedItem.OrderId).FirstOrDefault();
                    if (order != null)
                    {
                        using (var transaction = shortLivedPotplantsEntities.Database.BeginTransaction())
                        {
                            try
                            {
                                order.Status = OrderStatuses.Canceled;
                                OrderCleanup(shortLivedPotplantsEntities);
                                shortLivedPotplantsEntities.SaveChanges();
                                transaction.Commit();
                                SelectedItem.Status = OrderStatuses.Canceled;
                                OnPropertyChanged(() => List);
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
        }
        private void OnCloseOrder()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No order selected.");
                return;
            }
            if (SelectedItem.Status != OrderStatuses.Open)
            {
                ShowMessageBox("Not available.");
                return;
            }
            var result = MessageBox.Show(
                        "Close this order?",
                        $"{SelectedItem.ClientName}",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var shortLivedPotplantsEntities = new PotplantsEntities())
                {
                    var order = shortLivedPotplantsEntities.Orders.Where(o => o.OrderId == SelectedItem.OrderId).FirstOrDefault();
                    if (order == null)
                    {
                        MessageBox.Show("Order not found.");
                        return;
                    }
                    var orderitems = order.OrderItems.Where(oi => oi.IsActive == true).ToList();
                    if (!orderitems.Any())
                    {
                        MessageBox.Show("There are no items in this order.");
                        return;
                    }
                    var isAllScanned = orderitems.All(oi => oi.IsScanned == true);
                    if (!isAllScanned)
                    {
                        MessageBox.Show("Not all order items are scanned.");
                        return;
                    }
                    try
                    {
                        order.Status = OrderStatuses.Closed;
                        shortLivedPotplantsEntities.SaveChanges();
                        SelectedItem.Status = OrderStatuses.Closed;
                        OnPropertyChanged(() => List);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while closing the order.");
                    }
                }
            }
        }
        private void OrderCleanup(PotplantsEntities potplantsentities)
        {
            var carriers = potplantsentities.Carriers.Where(c => c.OrderId == SelectedItem.OrderId).ToList();
            carriers.ForEach(c =>
            {
                c.IsActive = false;
                c.DeletedAt = DateTime.Now;
            });
            var orderItems = potplantsentities.OrderItems.Where(oi => oi.IsActive == true && oi.OrderId == SelectedItem.OrderId).ToList();
            var stockItemIdsDict = orderItems.ToDictionary(oi => oi.StockItemId, oi => oi.Quantity);
            var stockItems = potplantsentities.StockItems.Where(si => stockItemIdsDict.Keys.Contains(si.StockItemId)).ToList();
            foreach (var stockItem in stockItems)
            {
                stockItem.QuantityLeft += stockItemIdsDict[stockItem.StockItemId];
            }
            orderItems.ForEach(oi => {
                oi.IsActive = false;
                oi.DeletedAt = DateTime.Now;
                });
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
