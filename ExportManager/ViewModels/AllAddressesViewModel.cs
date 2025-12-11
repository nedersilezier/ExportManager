using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;
using ExportManager.Models;
using ExportManager.Models.EntitiesForView;

namespace ExportManager.ViewModels
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
            AddNew<NewAddressViewModel>();
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
    }
}
