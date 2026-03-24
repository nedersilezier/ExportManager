using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
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
        public override void Load()
        {
            List = new ObservableCollection<OrdersListView>(
                from order in potplantsEntities.Orders
                where order.IsActive == true
                select new OrdersListView
                {
                    OrderId = order.OrderId,
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
        }
        #endregion
        #region Constructor
        public AllOrdersViewModel()
            : base()
        {
            base.DisplayName = "Orders";
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
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Order items", ShowDetailsCommand),
                new CommandViewModel("Carriers", ShowCarriersCommand)
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
            OpenNewTab(() => new NewOrderViewModel(SelectedItem.OrderId), Load);
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
                        var carriers = potplantsEntities.Carriers.Where(c => c.OrderId ==  SelectedItem.OrderId).ToList();
                        carriers.ForEach(c =>
                        { 
                            c.IsActive = false;
                            c.DeletedAt = DateTime.Now;
                            });
                        var orderItems = potplantsEntities.OrderItems.Where(oi => oi.IsActive == true && oi.OrderId == SelectedItem.OrderId).ToList();
                        var stockItemIdsDict = orderItems.ToDictionary(oi => oi.StockItemId, oi => oi.Quantity);
                        var stockItems = potplantsEntities.StockItems.Where(si => stockItemIdsDict.Keys.Contains(si.StockItemId)).ToList();
                        foreach (var stockItem in stockItems)
                        {
                            stockItem.QuantityLeft += stockItemIdsDict[stockItem.StockItemId];
                        }
                        orderItems.ForEach(oi => oi.IsActive = false);
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
            OpenNewTab(() => new AllOrderItemsViewModel(SelectedItem.OrderId));
        }
        private void OnShowCarriers()
        {
            if (SelectedItem == null)
            {
                ShowMessageBox("No order selected.");
                return;
            }
            OpenNewTab(() => new AllOrderItemCarriersViewModel(SelectedItem.OrderId));
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
