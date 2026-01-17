using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;
using ExportManager.Helper;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.ViewModels.ShowAllViewModels;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewClientViewModel : NewItemViewModel<Clients>
    {
        #region Select test
        private BaseCommand _SelectTestCommand;
        public ICommand SelectTestCommand
        {
            get
            {
                if (_SelectTestCommand == null)
                {
                    _SelectTestCommand = new BaseCommand(openSelectTestTab);
                }
                return _SelectTestCommand;
            }
        }
        private KeyAndValue _TestAddress;
        public KeyAndValue TestAddress
        {
            get
            {
                if (_TestAddress == null)
                {
                    _TestAddress = new KeyAndValue();
                }
                return _TestAddress;
            }
            set
            {
                if (_TestAddress != value)
                {
                    _TestAddress = value;
                    OnPropertyChanged(() => TestAddress);
                }
            }
        }
        public void setTestAddress(SelectedItemEventArgs<dynamic> e)
        {
            string FullHouseNumber = e.SelectedItem.HouseNumber + (e.SelectedItem.ApartmentNumber == null || e.SelectedItem.ApartmentNumber == string.Empty ?
                    string.Empty :
                    "/" + e.SelectedItem.ApartmentNumber);
            TestAddress.Key = e.SelectedItem.AddressId;
            TestAddress.Value = e.SelectedItem.Street + " " + FullHouseNumber + ", " + e.SelectedItem.ZipCode + " " + e.SelectedItem.City + "; " + e.SelectedItem.Country;
        }
        //public void setTestAddress(AddressesListView address)
        //{
        //    string FullHouseNumber = address.HouseNumber + (address.ApartmentNumber == null || address.ApartmentNumber == string.Empty ?
        //            string.Empty :
        //            "/" + address.ApartmentNumber);
        //    TestAddress.Key = address.AddressId;
        //    TestAddress.Value = address.Street + " " + FullHouseNumber + ", " + address.ZipCode + " " + address.City + "; " + address.Country;
        //}
        private void openSelectTestTab()
        {
            OpenNewTab(() => new AllAddressesViewModel(setTestAddress), () => { });
        }
        private int _SelectedTabIndex;
        public int SelectedTabIndex
        {
            get => _SelectedTabIndex;
            set
            {
                if (_SelectedTabIndex != value)
                {
                    _SelectedTabIndex = value;
                    OnPropertyChanged(() => SelectedTabIndex);
                }
            }
        }
        #endregion
        private BaseCommand _NewCountryCommand;
        private KeyAndValue _SelectedCountry;
        private KeyAndValue _SelectedAddress;
        public ObservableCollection<KeyAndValue> _Countries;
        public ObservableCollection<KeyAndValue> _Addresses;
        private Addresses newClientAddress;
        public bool _IsAddressesNeeded;
        public bool _IsNotAddressesNeeded;
        #region Constructor
        public NewClientViewModel()
            : base()
        {
            base.DisplayName = "New client";
            item = new Clients();
            newClientAddress = new Addresses();
            //Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
            IsNotAddressesNeeded = true;
        }
        public NewClientViewModel(int clientId)
            : base()
        {
            base.DisplayName = "Edit client";
            _IsEditMode = true;
            item = potplantsEntities.Clients.FirstOrDefault(t => t.ClientId == clientId);
            newClientAddress = potplantsEntities.Addresses.FirstOrDefault(t => t.AddressId == item.AddressId);
            SelectedCountry = Countries.FirstOrDefault(t => t.Key == item.Addresses.CountryId);
            SelectedAddress = Addresses.FirstOrDefault(t => t.Key == item.AddressId);
            IsNotAddressesNeeded = true;
        }
        #endregion
        #region Properties
        //Address related
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
                        if (Addresses == null)
                        {
                            Addresses = new AddressesForEntities(potplantsEntities).GetAddressesListItems();
                            OnPropertyChanged(() => Addresses);
                            //foreach(Addresses address  in Addresses)
                            //    Console.WriteLine(address.FullAddress + "\n");
                        }
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
        public ObservableCollection<KeyAndValue> Addresses
        {
            get
            {
                if (_Addresses == null)
                    _Addresses = new AddressesForEntities(potplantsEntities).GetAddressesListItems();
                return _Addresses;
            }
            set
            {
                if (_Addresses != value)
                {
                    _Addresses = value;
                    OnPropertyChanged(() => Addresses);
                }
            }
        }
        public string Street
        {
            get { return newClientAddress.Street; }
            set
            {
                if (newClientAddress.Street != value)
                {
                    newClientAddress.Street = value;
                }
                OnPropertyChanged(() => Street);
            }
        }
        public string HouseNumber
        {
            get { return newClientAddress.HouseNumber; }
            set
            {
                if (newClientAddress.HouseNumber != value)
                    newClientAddress.HouseNumber = value;
                OnPropertyChanged(() => HouseNumber);
            }
        }
        public string ApartmentNumber
        {
            get { return newClientAddress.ApartmentNumber; }
            set
            {
                if (newClientAddress.ApartmentNumber != value)
                    newClientAddress.ApartmentNumber = value;
                OnPropertyChanged(() => ApartmentNumber);
            }
        }
        public string ZipCode
        {
            get { return newClientAddress.ZipCode; }
            set
            {
                if (newClientAddress.ZipCode != value)
                    newClientAddress.ZipCode = value;
                OnPropertyChanged(() => ZipCode);
            }
        }
        public string City
        {
            get { return newClientAddress.City; }
            set
            {
                if (newClientAddress.City != value)
                    newClientAddress.City = value;
                OnPropertyChanged(() => City);
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
        public KeyAndValue SelectedAddress
        {
            get { return _SelectedAddress; }
            set
            {
                if (_SelectedAddress != value)
                {
                    _SelectedAddress = value;
                    OnPropertyChanged(() => SelectedAddress);
                }
            }
        }

        //Client related
        public string ClientName
        {
            get { return item.Name; }
            set
            {
                if (item.Name != value)
                {
                    item.Name = value;
                    OnPropertyChanged(() => ClientName);
                }
            }
        }
        public string ClientCode
        {
            get { return item.ClientCode; }
            set
            {
                if (item.ClientCode != value)
                {
                    item.ClientCode = value;
                    OnPropertyChanged(() => ClientCode);
                }
            }
        }
        public string ContactPerson
        {
            get { return item.ContactPerson; }
            set
            {
                if (item.ContactPerson != value)
                {
                    item.ContactPerson = value;
                    OnPropertyChanged(() => ContactPerson);
                }
            }
        }
        public string Phone
        {
            get { return item.Phone; }
            set
            {
                if (item.Phone != value)
                {
                    item.Phone = value;
                    OnPropertyChanged(() => Phone);
                }
            }
        }
        public string Email
        {
            get { return item.Email; }
            set
            {
                if (item.Email != value)
                {
                    item.Email = value;
                    OnPropertyChanged(() => Email);
                }
            }
        }
        public int AddressId
        {
            get { return item.AddressId; }
            set
            {
                if (item.AddressId != value)
                {
                    item.AddressId = value;
                    OnPropertyChanged(() => AddressId);
                }
            }
        }
        public string TaxId
        {
            get { return item.TaxId; }
            set
            {
                if (item.TaxId != value)
                {
                    item.TaxId = value;
                    OnPropertyChanged(() => TaxId);
                }
            }
        }
        public string RegistrationNo
        {
            get { return item.RegistrationNo; }
            set
            {
                if (item.RegistrationNo != value)
                {
                    item.RegistrationNo = value;
                    OnPropertyChanged(() => RegistrationNo);
                }
            }
        }
        public bool IsActive
        {
            get { return item.IsActive; }
            set
            {
                if (item.IsActive != value)
                {
                    item.IsActive = value;
                    OnPropertyChanged(() => IsActive);
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
        #endregion
        #region Commands
        public override void Save()
        {
            if (IsAddressesNeeded)
            {
                //if (SelectedAddress == null)
                if (TestAddress == null)
                    throw new Exception("No address selected");
                //item.AddressId = SelectedAddress.Key;
                item.AddressId = TestAddress.Key;
            }
            else
            {
                if (SelectedCountry == null)
                    throw new Exception("No country selected");
                newClientAddress.CountryId = SelectedCountry.Key;
                if (!_IsEditMode)
                {
                    newClientAddress.IsActive = true;
                    potplantsEntities.Addresses.Add(newClientAddress);
                }
                potplantsEntities.SaveChanges();
                item.AddressId = newClientAddress.AddressId;
            }
            if (!_IsEditMode)
            {
                item.IsActive = true;
                potplantsEntities.Clients.Add(item);
            }
            potplantsEntities.SaveChanges();
        }
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
