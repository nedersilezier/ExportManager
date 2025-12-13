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
using System.Runtime.Remoting.Contexts;

namespace ExportManager.ViewModels
{
    public class NewGrowerViewModel: NewItemViewModel<Growers>
    {
        private BaseCommand _AddCultivationCommand;
        private BaseCommand _RemoveCultivationCommand;
        private BaseCommand _NewCountryCommand;
        private Countries _SelectedCountry;
        private BaseCommand _NewAddressCommand;
        private Addresses _SelectedAddress;
        public ObservableCollection<Countries> Countries { get; set; }
        public ObservableCollection<Addresses> Addresses { get; set; }
        private ObservableCollection<KeyAndValue> _AllCultivations;
        private ObservableCollection<KeyAndValue> _SelectedCultivations;
        private Addresses newGrowerAddress;
        public bool _IsAddressesNeeded;
        public bool _IsNotAddressesNeeded;
        #region Constructor
        public NewGrowerViewModel()
            : base()
        {
            base.DisplayName = "New grower";
            item = new Growers();
            newGrowerAddress = new Addresses();
            Countries = new ObservableCollection<Countries>(potplantsEntities.Countries.Where(t => t.IsActive == true).ToList());
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
                            Addresses = new ObservableCollection<Addresses>(potplantsEntities.Addresses.Where(t => t.IsActive == true).ToList());
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
        public Countries SelectedCountry
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
        public Addresses SelectedAddress
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
        #endregion
        #region Commands
        public override void Save()
        {
            if (IsAddressesNeeded)
            {
                if (SelectedAddress == null)
                    throw new Exception("No address selected");
                item.AddressId = SelectedAddress.AddressId;
            }
            else
            {
                if (SelectedCountry == null)
                    throw new Exception("No country selected");
                newGrowerAddress.CountryId = SelectedCountry.CountryId;
                newGrowerAddress.IsActive = true;
                potplantsEntities.Addresses.Add(newGrowerAddress);
                potplantsEntities.SaveChanges();
                item.AddressId = newGrowerAddress.AddressId;
            }
            item.IsActive = true;
            var selectedCultivationIds = SelectedCultivations.Select(t => t.Key).ToList();
            var cultivations = potplantsEntities.Cultivations.Where(t => selectedCultivationIds.Contains(t.CultivationId)).ToList();
            foreach(var cultivation in cultivations)
                item.Cultivations.Add(cultivation);
            potplantsEntities.Growers.Add(item);
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
            var viewModel = new NewCountryViewModel();
            viewModel.Added += RefreshCountries;
            var mainViewModel = (MainWindowViewModel)App.Current.MainWindow.DataContext;
            mainViewModel.CreateView(viewModel);
        }
        private void RefreshCountries()
        {
            Countries = new ObservableCollection<Countries>(potplantsEntities.Countries.Where(t => t.IsActive == true).ToList());
            OnPropertyChanged(() => Countries);
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
