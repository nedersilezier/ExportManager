using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.ShowAllViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewGrowerViewModel: NewItemViewModel<Growers>
    {
        #region Item picker
        private BaseCommand _SelectCommand;
        public ICommand SelectCommand
        {
            get
            {
                if (_SelectCommand == null)
                {
                    _SelectCommand = new BaseCommand(openSelectTab);
                }
                return _SelectCommand;
            }
        }
        private KeyAndValue _SelectedAddress;
        public KeyAndValue SelectedAddress
        {
            get
            {
                if (_SelectedAddress == null)
                {
                    _SelectedAddress = new KeyAndValue();
                }
                return _SelectedAddress;
            }
            set
            {
                if (_SelectedAddress != value)
                {
                    _SelectedAddress = value;
                    OnPropertyChanged(() => SelectedAddress);
                }
            }
        }
        public void setAddress(SelectedItemEventArgs e)
        {
            SelectedAddress.Key = e.ItemId;
            SelectedAddress.Value = e.DisplayName;
        }
        private void openSelectTab()
        {
            OpenNewTab(() => new AllAddressesViewModel(setAddress));
        }
        #endregion
        #region Constructor
        public NewGrowerViewModel()
            : base()
        {
            base.DisplayName = "New grower";
            item = new Growers();
            newGrowerAddress = new Addresses();
            //Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
            IsNotAddressesNeeded = true;
        }
        public NewGrowerViewModel(int growerId)
            : base()
        {
            base.DisplayName = "Edit grower";
            _IsEditMode = true;
            item = potplantsEntities.Growers.FirstOrDefault(t => t.GrowerId == growerId);
            newGrowerAddress = potplantsEntities.Addresses.FirstOrDefault(t => t.AddressId == item.AddressId);
            //Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
            SelectedCountry = Countries.FirstOrDefault(t => t.Key == item.Addresses.CountryId);
            //SelectedAddress = Addresses.FirstOrDefault(t => t.Key == item.AddressId);
            var selectedCultivationsIds = item.Cultivations.Select(t => t.CultivationId).ToList();
            SelectedCultivations = new ObservableCollection<KeyAndValue>
            (
                AllCultivations.Where(t => selectedCultivationsIds.Contains(t.Key)).ToList()
            );
            AllCultivations = new ObservableCollection<KeyAndValue>
            (
                AllCultivations.Where(t => !selectedCultivationsIds.Contains(t.Key)).ToList()
            );
            IsNotAddressesNeeded = true;
        }
        #endregion
        #region Properties and Fields
        //Fields
        private BaseCommand _AddCultivationCommand;
        private BaseCommand _RemoveCultivationCommand;
        private BaseCommand _NewCountryCommand;
        private KeyAndValue _SelectedCountry;
        //private KeyAndValue _SelectedAddress;
        public ObservableCollection<KeyAndValue> _Countries;
        //public ObservableCollection<KeyAndValue> _Addresses;
        private ObservableCollection<KeyAndValue> _AllCultivations;
        private ObservableCollection<KeyAndValue> _SelectedCultivations;
        private Addresses newGrowerAddress;
        public bool _IsAddressesNeeded;
        public bool _IsNotAddressesNeeded;
        private int _SelectedTabIndex;
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
                        //if (Addresses == null)
                        //{
                        //    Addresses = new AddressesForEntities(potplantsEntities).GetAddressesListItems();
                        //    OnPropertyChanged(() => Addresses);
                        //}
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
        public string Street
        {
            get { return newGrowerAddress.Street; }
            set
            {
                if (newGrowerAddress.Street != value)
                {
                    newGrowerAddress.Street = value;
                }
                OnPropertyChanged(() => Street);
            }
        }
        public string HouseNumber
        {
            get { return newGrowerAddress.HouseNumber; }
            set
            {
                if (newGrowerAddress.HouseNumber != value)
                    newGrowerAddress.HouseNumber = value;
                OnPropertyChanged(() => HouseNumber);
            }
        }
        public string ApartmentNumber
        {
            get { return newGrowerAddress.ApartmentNumber; }
            set
            {
                if (newGrowerAddress.ApartmentNumber != value)
                    newGrowerAddress.ApartmentNumber = value;
                OnPropertyChanged(() => ApartmentNumber);
            }
        }
        public string ZipCode
        {
            get { return newGrowerAddress.ZipCode; }
            set
            {
                if (newGrowerAddress.ZipCode != value)
                    newGrowerAddress.ZipCode = value;
                OnPropertyChanged(() => ZipCode);
            }
        }
        public string City
        {
            get { return newGrowerAddress.City; }
            set
            {
                if (newGrowerAddress.City != value)
                    newGrowerAddress.City = value;
                OnPropertyChanged(() => City);
            }
        }
        public ObservableCollection<KeyAndValue> Countries
        {
            get
            {
                if(_Countries == null)
                    _Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
                //_Countries = new ObservableCollection<KeyAndValue>();
                return _Countries;
            }
            set
            {
                if(_Countries != value)
                {
                    _Countries = value;
                    OnPropertyChanged(() => Countries);
                }
            }
        }
        //public ObservableCollection<KeyAndValue> Addresses
        //{
        //    get
        //    {
        //        //if (_Addresses == null)
        //        //    _Addresses = new ObservableCollection<KeyAndValue>();
        //        if(_Addresses == null)
        //            _Addresses = new AddressesForEntities(potplantsEntities).GetAddressesListItems();
        //        return _Addresses;
        //    }
        //    set
        //    {
        //        if (_Addresses != value)
        //        {
        //            _Addresses = value;
        //            OnPropertyChanged(() => Addresses);
        //        }
        //    }
        //}
        public KeyAndValue SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                if (_SelectedCountry != value)
                {
                    _SelectedCountry = value;
                    OnPropertyChanged(() => SelectedCountry); //sprawdz czy to w ogole potrzebne....
                }
            }
        }

        //Grower related
        public string GrowerName
        {
            get { return item.Name; }
            set
            {
                if (item.Name != value)
                {
                    item.Name = value;
                    OnPropertyChanged(() => GrowerName);
                }
            }
        }
        public ObservableCollection<KeyAndValue> AllCultivations
        {
            get
            {
                if(_AllCultivations == null)
                    _AllCultivations = new CultivationsForGrowers(potplantsEntities).GetCultivationsKeyAndValueItems();
                return _AllCultivations;
            }
            set
            {
                if (_AllCultivations != value)
                {
                    _AllCultivations = value;
                    OnPropertyChanged(() => _AllCultivations);
                }
            }
        }
        public ObservableCollection<KeyAndValue> SelectedCultivations
        {
            get 
            { 
                if(_SelectedCultivations == null)
                    _SelectedCultivations = new ObservableCollection<KeyAndValue>();
                return _SelectedCultivations; 
            }
            set
            {
                if (_SelectedCultivations != value)
                {
                    _SelectedCultivations = value;
                    OnPropertyChanged(() => _SelectedCultivations);
                }
            }
        }
        public KeyAndValue AllCultivationsSelectedItem { get; set; }
        public KeyAndValue SelectedCultivationsSelectedItem { get; set; }
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
        //Tab control related
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
        #region Commands
        public override void Save()
        {
            if (IsAddressesNeeded)
            {
                if (SelectedAddress == null)
                    throw new Exception("No address selected");
                item.AddressId = SelectedAddress.Key;
            }
            else
            {
                if (SelectedCountry == null)
                    throw new Exception("No country selected");
                newGrowerAddress.CountryId = SelectedCountry.Key;
                if(!_IsEditMode)
                {
                    newGrowerAddress.IsActive = true;
                    potplantsEntities.Addresses.Add(newGrowerAddress);
                }
                potplantsEntities.SaveChanges();
                item.AddressId = newGrowerAddress.AddressId;
            }
            item.Cultivations.Clear();
            var selectedCultivationsIds = SelectedCultivations.Select(t => t.Key);
            var cultivations = potplantsEntities.Cultivations.Where(t => selectedCultivationsIds.Contains(t.CultivationId));
            foreach (var cultivation in cultivations)
                item.Cultivations.Add(cultivation);
            if (!_IsEditMode)
            {
                item.IsActive = true;
                potplantsEntities.Growers.Add(item);
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
        public ICommand AddCultivationCommand
        {
            get
            {
                if (_AddCultivationCommand == null)
                {
                    _AddCultivationCommand = new BaseCommand(onAddCultivation);
                }
                return _AddCultivationCommand;
            }
        }
        public ICommand RemoveCultivationCommand
        {
            get
            {
                if (_RemoveCultivationCommand == null)
                {
                    _RemoveCultivationCommand = new BaseCommand(onRemoveCultivation);
                }
                return _RemoveCultivationCommand;
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
        private void onAddCultivation()
        {
            if (AllCultivationsSelectedItem == null)
                return;
            SelectedCultivations.Add(AllCultivationsSelectedItem);
            AllCultivations.Remove(AllCultivationsSelectedItem);
        }
        private void onRemoveCultivation()
        {
            if (SelectedCultivationsSelectedItem == null)
                return;
            AllCultivations.Add(SelectedCultivationsSelectedItem);
            SelectedCultivations.Remove(SelectedCultivationsSelectedItem);
        }
        #endregion
    }
}
