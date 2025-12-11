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

namespace ExportManager.ViewModels
{
    public class NewClientViewModel: NewItemViewModel<Clients>
    {
        private BaseCommand _NewCountryCommand;
        private Countries _SelectedCountry;
        private BaseCommand _NewAddressCommand;
        private Addresses _SelectedAddress;
        public ObservableCollection<Countries> Countries { get; set; }
        public ObservableCollection<Addresses> Addresses { get; set; }
        private Addresses newClientAddress;
        public bool _IsAddressesNeeded;
        public bool _IsNotAddressesNeeded;
        #region Constructor
        public NewClientViewModel(  )
            :base()
        {
            base.DisplayName = "New client";
            item = new Clients();
            newClientAddress = new Addresses();
            Countries = new ObservableCollection<Countries>(potplantsEntities.Countries.Where(t => t.IsActive==true).ToList());
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
                if( _IsAddressesNeeded != value )
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
                if( _IsNotAddressesNeeded != value )
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
        public Countries SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                if(_SelectedCountry != value)
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
                if(_SelectedAddress != value)
                {
                    _SelectedAddress = value;
                    OnPropertyChanged(() => SelectedAddress);
                }
            }
        }

        //Client related
        public string ClientName {
            get { return item.Name; }
            set
            {
                if(item.Name != value)
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
                if(item.ClientCode != value)
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
                if( item.Email != value)
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
            if(IsAddressesNeeded)
            {
                if (SelectedAddress == null)
                    throw new Exception("No address selected");
                item.AddressId = SelectedAddress.AddressId;
            }
            else
            {
                if (SelectedCountry == null)
                    throw new Exception("No country selected");
                newClientAddress.CountryId = SelectedCountry.CountryId;
                newClientAddress.IsActive = true;
                potplantsEntities.Addresses.Add(newClientAddress);
                potplantsEntities.SaveChanges();
                item.AddressId = newClientAddress.AddressId;
            }
            item.IsActive = true;
            potplantsEntities.Clients.Add(item);
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
        #endregion
    }
}
