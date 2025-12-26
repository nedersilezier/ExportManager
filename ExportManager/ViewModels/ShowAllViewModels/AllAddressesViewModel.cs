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

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllAddressesViewModel: AllViewModel<dynamic>
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
            :base()
        {
            base.DisplayName = "Addresses";
        }
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
            return;
        }
        #endregion
    }
}
