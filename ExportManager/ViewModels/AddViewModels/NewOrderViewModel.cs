using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;
using System.Collections.ObjectModel;
using ExportManager.Models.EntitiesForView;
using ExportManager.Helper;
using System.Windows.Input;
using ExportManager.ViewModels.Events;
using ExportManager.ViewModels.ShowAllViewModels;
using System.Security.Cryptography;
using ExportManager.Models.BusinessLogic.ListViewsForUI;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewOrderViewModel: NewItemViewModel<Orders>
    {
        #region Constructor
        public NewOrderViewModel()
            : base(new[] {""})
        {
            base.DisplayName = "New order";
        }
        public NewOrderViewModel(int orderId)
            : base(new[] { "" })
        {
            base.DisplayName = "Edit order";
            _IsEditMode = true;
            item = potplantsEntities.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
        }
        #endregion
        #region Fields
        #endregion
        #region Properties
        #endregion
        #region Item pickers
        private KeyAndValue _SelectedClient;
        private ClientsListView _SelectedClientDetailed;
        public KeyAndValue SelectedClient
        {
            get
            {
                if(_SelectedClient == null)
                {
                    _SelectedClient = new KeyAndValue();
                }
                return _SelectedClient;
            }
            set
            {
                if (_SelectedClient != value)
                {
                    _SelectedClient = value;
                    OnPropertyChanged(() => SelectedClient);
                }
            }
        }
        public ClientsListView SelectedClientDetailed
        {
            get
            {
                if(_SelectedClientDetailed == null)
                {
                    _SelectedClientDetailed = new ClientsListView();
                }
                return _SelectedClientDetailed;
            }
            set
            {
                if (_SelectedClientDetailed != value)
                {
                    _SelectedClientDetailed = value;
                    OnPropertyChanged(() => SelectedClientDetailed);
                }
            }
        }
        private BaseCommand _SelectClientCommand;
        public ICommand SelectClientCommand
        {
            get
            {
                if(_SelectClientCommand == null)
                {
                    _SelectClientCommand = new BaseCommand(openSelectClientTab);
                }
                return _SelectClientCommand;
            }
        }
        private void setClient(SelectedItemEventArgs e)
        {
            SelectedClient = new KeyAndValue
            {
                Key = e.ItemId,
                Value = e.DisplayName
            };
            SelectedClientDetailed = new ClientDetailsQuery(potplantsEntities).getClientAddressDetailsById(e.ItemId).FirstOrDefault();
        }
        private void openSelectClientTab()
        {
            OpenNewTab(() => new AllClientsViewModel(setClient));
        }
        #endregion
    }
}
