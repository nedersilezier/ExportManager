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
using ExportManager.Models.Extensions;
using System.Security.Policy;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewOrderViewModel : NewItemViewModel<Orders>
    {
        #region Constructor
        public NewOrderViewModel()
            : base(new[] { "" })
        {
            base.DisplayName = "New order";
            item = new Orders();
            //customAddress = new Addresses();
            IsNotAddressesNeeded = true;
            OrderDate = DateTime.Now.Date;
            PreparationDate = DateTime.Now.Date;
            ShipmentDate = DateTime.Now.Date;
            DeliveryDate = DateTime.Now.Date;

        }
        public NewOrderViewModel(int orderId)
            : base(new[] {""} )
        {
            item = potplantsEntities.Orders.FirstOrDefault(o => o.OrderId == orderId);
            base.DisplayName = "Edit order";
            IsEditMode = true;
            SelectedDeliveryAddress = new OrderDetailsQuery(potplantsEntities).GetOrdersDeliveryAddress(orderId).FirstOrDefault();
            SelectedClient = new ClientDetailsQuery(potplantsEntities).GetKeyAndValueById(item.ClientId).FirstOrDefault();
            SelectedCountry = Countries.FirstOrDefault(c => c.Key == item.Addresses.Countries.CountryId);
            IsNotAddressesNeeded = true;
        }
        #endregion
        #region Fields
        //logic
        private int _SelectedTabIndex;
        //address related
        private bool _IsAddressesNeeded;
        private bool _IsNotAddressesNeeded;
        private KeyAndValue _SelectedCountry;
        private ObservableCollection<KeyAndValue> _Countries;
        private Addresses customAddress;
        private BaseCommand _NewCountryCommand;
        private string _Street;
        private string _HouseNumber;
        private string _ApartmentNumber;
        private string _City;
        private string _ZipCode;
        //other
        //Edit mode
        private AddressesListView _SelectedDeliveryAddress;
        #endregion
        #region Properties
        //logic
        public int SelectedTabIndex
        {
            get
            {
                return _SelectedTabIndex;
            }
            set
            {
                if (_SelectedTabIndex != value)
                {
                    _SelectedTabIndex = value;
                    OnPropertyChanged(() => SelectedTabIndex);
                }
            }
        }
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set
            {
                if (_IsEditMode != value)
                {
                    _IsEditMode = value;
                    OnPropertyChanged(() => IsEditMode);
                }
            }
        }
        //address related
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
        public string Street
        {
            get { return _Street; }
            set
            {
                if (_Street != value)
                {
                    _Street = value;
                    OnPropertyChanged(() => Street);
                }
            }
        }
        public string HouseNumber
        {
            get { return _HouseNumber; }
            set
            {
                if (_HouseNumber != value)
                {
                    _HouseNumber = value;
                    OnPropertyChanged(() => HouseNumber);
                }
            }
        }
        public string ApartmentNumber
        {
            get { return _ApartmentNumber; }
            set
            {
                if (_ApartmentNumber != value)
                {
                    _ApartmentNumber = value;
                    OnPropertyChanged(() => ApartmentNumber);
                }
            }
        }
        public string City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged(() => City);
                }
            }
        }
        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                if (_ZipCode != value)
                {
                    _ZipCode = value;
                    OnPropertyChanged(() => ZipCode);
                }
            }
        }
        //other
        public string SalesPerson
        {
            get { return item.SalesPerson; }
            set
            {
                if (item.SalesPerson != value)
                {
                    item.SalesPerson = value;
                    OnPropertyChanged(() => SalesPerson);
                }
            }
        }
        public string Remarks
        {
            get { return item.Remarks; }
            set
            {
                if (item.Remarks != value)
                {
                    item.Remarks = value;
                    OnPropertyChanged(() => Remarks);
                }
            }
        }
        //dates
        public DateTime OrderDate
        {
            get { return item.OrderDate; }
            set
            {
                if (item.OrderDate != value)
                {
                    item.OrderDate = value;
                    OnPropertyChanged(() => OrderDate);
                    if (item.PreparationDate < item.OrderDate)
                    {
                        PreparationDate = item.OrderDate;
                    }
                }
            }
        }
        public DateTime PreparationDate
        {
            get { return item.PreparationDate; }
            set
            {
                if (item.PreparationDate != value)
                {
                    if (value >= item.OrderDate)
                    {
                        item.PreparationDate = value;
                        OnPropertyChanged(() => PreparationDate);
                        if (item.ShipmentDate < item.PreparationDate)
                        {
                            ShipmentDate = item.PreparationDate;
                        }
                    }
                    else
                        ShowMessageBox("Preparation date cannot be before order date.");
                }
            }
        }
        public DateTime ShipmentDate
        {
            get { return item.ShipmentDate; }
            set
            {
                if (item.ShipmentDate != value)
                {
                    if (value >= item.PreparationDate)
                    {
                        item.ShipmentDate = value;
                        OnPropertyChanged(() => ShipmentDate);
                        if (item.DeliveryDate < item.ShipmentDate)
                        {
                            DeliveryDate = item.ShipmentDate;
                        }
                    }
                    else
                        ShowMessageBox("Shipment date cannot be before preparation date.");
                }
            }
        }
        public DateTime DeliveryDate
        {
            get { return item.DeliveryDate; }
            set
            {
                if (item.DeliveryDate != value)
                {
                    if (value >= item.ShipmentDate)
                    {
                        item.DeliveryDate = value;
                        OnPropertyChanged(() => DeliveryDate);
                    }
                    else
                        ShowMessageBox("Delivery date cannot be before shipment date.");
                }
            }
        }
        //Edit mode
        public AddressesListView SelectedDeliveryAddress
        {
            get
            {
                if (_SelectedDeliveryAddress == null)
                {
                    _SelectedDeliveryAddress = new AddressesListView();
                }
                return _SelectedDeliveryAddress;
            }
            set
            {
                if (_SelectedDeliveryAddress != value)
                {
                    _SelectedDeliveryAddress = value;
                    OnPropertyChanged(() => SelectedDeliveryAddress);
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
                if (_SelectedClient == null)
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
                if (_SelectedClientDetailed == null)
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
                if (_SelectClientCommand == null)
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
            SelectedClientDetailed = new ClientDetailsQuery(potplantsEntities).GetClientAddressDetailsById(e.ItemId).FirstOrDefault();
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
        public override void Save()
        {
            if (IsEditMode)
                UpdateOrder();
            else
                CreateOrder();
            potplantsEntities.SaveChanges();
        }
        private void CreateOrder()
        {
            if (SelectedClient.Key == 0)
                throw new Exception("No client selected.");
            bool orderExists = potplantsEntities.Orders.Any(
               o => o.ClientId == SelectedClient.Key && 
               o.IsActive && 
               o.OrderDate == OrderDate &&
               o.PreparationDate == PreparationDate &&
               o.ShipmentDate == ShipmentDate &&
               o.DeliveryDate == DeliveryDate);
            if (orderExists)
                throw new Exception("An order for this client with the selected dates already exists.");
            item.ClientId = SelectedClient.Key;
            if (IsNotAddressesNeeded)
                item.DeliveryAddressId = new AddressesForEntities(potplantsEntities).GetAddressIdByClientId(SelectedClient.Key);
            else
            {
                if (SelectedCountry.Key == 0)
                    throw new Exception("No country selected.");

                customAddress = new Addresses
                {
                    Street = Street,
                    HouseNumber = HouseNumber,
                    ApartmentNumber = ApartmentNumber,
                    City = City,
                    ZipCode = ZipCode,
                    CountryId = SelectedCountry.Key,
                    IsActive = true
                };
                potplantsEntities.Addresses.Add(customAddress);
                potplantsEntities.SaveChanges();
                item.DeliveryAddressId = customAddress.AddressId;
            }
            item.IsActive = true;
            item.Status = OrderStatuses.Open;
            potplantsEntities.Orders.Add(item);
        }
        private void UpdateOrder()
        {
            var address = potplantsEntities.Addresses.FirstOrDefault(a => a.AddressId == item.DeliveryAddressId);
            if (address != null)
            {
                address.Street = SelectedDeliveryAddress.Street;
                address.HouseNumber = SelectedDeliveryAddress.HouseNumber;
                address.ApartmentNumber = SelectedDeliveryAddress.ApartmentNumber;
                address.City = SelectedDeliveryAddress.City;
                address.ZipCode = SelectedDeliveryAddress.ZipCode;
                address.CountryId = SelectedCountry.Key;
            }
        }
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
