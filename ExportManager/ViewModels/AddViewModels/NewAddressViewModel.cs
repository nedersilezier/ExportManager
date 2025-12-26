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
using System.Windows;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.BusinessLogic.ListViewsForUI;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewAddressViewModel: NewItemViewModel<Addresses>
    {
        private BaseCommand _NewCountryCommand;
        private KeyAndValue _SelectedCountry;
        public ObservableCollection<KeyAndValue> _Countries;
        #region Constructor
        public NewAddressViewModel()
            : base()
        {
            base.DisplayName = "New address";
            item = new Addresses();
            
        }
        public NewAddressViewModel(int addressId)
            : base()
        {
            base.DisplayName = "Edit address";
            _IsEditMode = true;
            item = potplantsEntities.Addresses.FirstOrDefault(t => t.AddressId == addressId);
            SelectedCountry = Countries.FirstOrDefault(t => t.Key == item.CountryId);
        }
        #endregion
        #region Properties
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
        public KeyAndValue SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                if (value != _SelectedCountry)
                {
                    _SelectedCountry = value;
                }
                OnPropertyChanged(() => SelectedCountry);
            }
        }
        public string Street
        {
            get { return item.Street; }
            set
            {
                if (item.Street != value)
                {
                    item.Street = value;
                }
                OnPropertyChanged(() => Street);
            }
        }
        public string HouseNumber
        {
            get { return item.HouseNumber; }
            set
            {
                if (item.HouseNumber != value)
                    item.HouseNumber = value;
                OnPropertyChanged(() => HouseNumber);
            }
        }
        public string ApartmentNumber
        {
            get { return item.ApartmentNumber; }
            set
            {
                if (item.ApartmentNumber != value)
                    item.ApartmentNumber = value;
                OnPropertyChanged(() => ApartmentNumber);
            }
        }
        public string ZipCode
        {
            get { return item.ZipCode; }
            set
            {
                if (item.ZipCode != value)
                    item.ZipCode = value;
                OnPropertyChanged(() => ZipCode);
            }
        }
        public string City
        {
            get { return item.City; }
            set
            {
                if (item.City != value)
                    item.City = value;
                OnPropertyChanged(() => City);
            }
        }
        public string Remarks
        {
            get { return item.Remarks; }
            set
            {
                if (item.Remarks != value)
                    item.Remarks = value;
                OnPropertyChanged(() => Remarks);
            }
        }
        #endregion
        #region Commands
        public override void Save()
        {
            if (SelectedCountry == null)
            {
                throw new Exception("No country chosen.");
            }
            var selectedCountry = potplantsEntities.Countries.FirstOrDefault(t=>t.CountryId==SelectedCountry.Key);
            item.CountryId = selectedCountry.CountryId;
            if(!_IsEditMode)
            {
                item.IsActive = true;
                potplantsEntities.Addresses.Add(item);
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
            OpenNewTab(()=> new NewCountryViewModel(), RefreshCountries);
        }
        private void RefreshCountries()
        {
            Countries = new CountriesForEntities(potplantsEntities).GetCountriesListItems();
        }
        #endregion
    }
}
