using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;
using ExportManager.Models;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models.BusinessLogic.ListViewsForUI;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllAddressesViewModel : AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(
                from address in potplantsEntities.Addresses
                where address.IsActive == true
                select new AddressesListView
                {
                    AddressId = address.AddressId,
                    Country = address.Countries.Name,
                    City = address.City,
                    Street = address.Street,
                    HouseNumber = address.HouseNumber,
                    ApartmentNumber = address.ApartmentNumber,
                    ZipCode = address.ZipCode,
                    Remarks = address.Remarks,
                    CountryISO2Code = address.Countries.ISO2Code,
                    CountryPhoneCode = address.Countries.PhoneCode,
                    Continent = address.Countries.Continent
                });
        }
        #endregion
        #region Constructor
        public AllAddressesViewModel()
            : base()
        {
            base.DisplayName = "Addresses";
        }
        public AllAddressesViewModel(Action<SelectedItemEventArgs> itemSetter)
            : base(itemSetter,
                  generateArgsFromSelection:
                  selectedItem => new SelectedItemEventArgs(selectedItem.AddressId, selectedItem.FullAddress))
        {
            base.DisplayName = "Select the address";
        }
        //public AllAddressesViewModel(Action<SelectedItemEventArgs<dynamic>> itemSetter)
        //    : base(itemSetter)
        //{
        //    base.DisplayName = "Select the address";
        //}
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewAddressViewModel(), Load);
        }
        public override void OnEdit()
        {
            if (SelectedItem == null)
                return;
            OpenNewTab(() => new NewAddressViewModel(SelectedItem.AddressId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Addresses>(SelectedItem.AddressId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "city", "country" };
        }
        public override void Sort()
        {
            if (SortField == "city")
                List = new ObservableCollection<dynamic>(List.OrderBy(t => t.City));
            if (SortField == "country")
                List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Country));
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "city", "country" };
        }
        public override void Find()
        {
            switch(FindField)
            {
                case "city":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.City != null && t.City.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "country":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.Country != null && t.Country.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
            }
            //if (FindField == "city")
            //{
            //    Load();
            //    List = new ObservableCollection<dynamic>(List.Where(t => t.City != null && t.City.ToLower().StartsWith(FindTextBox.ToLower())));
            //}

        }
        #endregion
    }
}
