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
            item = new Orders();
            customAddress = new Addresses();
            IsNotAddressesNeeded = true;
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
        private int _SelectedTabIndex;
        private bool _IsAddressesNeeded;
        private bool _IsNotAddressesNeeded;
        private KeyAndValue _SelectedCountry;
        private ObservableCollection<KeyAndValue> _Countries;
        private Addresses customAddress;
        private BaseCommand _NewCountryCommand;
        private string _SalesPerson;
        private string _Remarks;
        #endregion
        #region Properties
        public int SelectedTabIndex
        {
            get
            {
                return _SelectedTabIndex;
            }
            set
            {
                if(_SelectedTabIndex != value)
                {
                    _SelectedTabIndex = value;
                    OnPropertyChanged(() => SelectedTabIndex);
                }
            }
        }
        public bool IsAddressesNeeded
        {
            get { return _IsAddressesNeeded; }
            set
            {
                if (_IsAddressesNeeded != value)
                {
                    _IsAddressesNeeded = value;
                    if (value == true)
                    {
                        IsNotAddressesNeeded = false;
                    }
                    OnPropertyChanged(() => IsAddressesNeeded);
                }
            }
        }
        public bool IsNotAddressesNeeded
        {
            get { return _IsNotAddressesNeeded; }
            set
            {
                if (_IsNotAddressesNeeded != value)
                {
                    _IsNotAddressesNeeded = value;
                    if (value == true)
                        IsAddressesNeeded = false;
                    OnPropertyChanged(() => IsNotAddressesNeeded);
                }
            }
        }
        public KeyAndValue SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                if (_SelectedCountry != value)
                {
                    _SelectedCountry = value;
                    OnPropertyChanged(() => SelectedCountry);
                }
            }
        }
        public ObservableCollection<KeyAndValue> Countries
        {
            get
            {
                if (_Countries == null)
                    _Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
                return _Countries;
            }
            set
            {
                if (_Countries != value)
                {
                    _Countries = value;
                    OnPropertyChanged(() => Countries);
                }
            }
        }
        public string SalesPerson
        {
            get { return _SalesPerson; }
            set
            {
                if (_SalesPerson != value)
                {
                    _SalesPerson = value;
                    OnPropertyChanged(() => SalesPerson);
                }
            }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged(() => Remarks);
                }
            }
        }
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
        #region Commands
        public ICommand NewCountryCommand
        {
            get
            {
                if (_NewCountryCommand == null)
                {
                    _NewCountryCommand = new BaseCommand(OpenNewCountryTab);
                }
                return _NewCountryCommand;
            }
        }
        #endregion
        #region Functions
        private void OpenNewCountryTab()
        {
            OpenNewTab(() => new NewCountryViewModel(), RefreshCountries);
        }
        private void RefreshCountries()
        {
            Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
        }
        #endregion
    }
}
