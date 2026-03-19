using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllOrdersViewModel: AllViewModel<OrdersListView>
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
                if(_ShowDetailsCommand == null)
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
        public override void OnRemove()
        {
            SoftDelete<Orders>(SelectedItem.OrderId);
        }
        private void OnShowDetails()
        {
            if(SelectedItem == null)
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
