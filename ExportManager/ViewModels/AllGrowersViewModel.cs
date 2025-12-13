using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.Models.EntitiesForView;

namespace ExportManager.ViewModels
{
    public class AllGrowersViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            var growers = potplantsEntities.Growers.Where(t => t.IsActive==true).ToList();
            List = new ObservableCollection<dynamic>(
                growers.Select(grower => new GrowersListView
                {
                    GrowerId = grower.GrowerId,
                    GrowerName = grower.Name,
                    ContactPerson = grower.ContactPerson,
                    PhoneNumber = grower.Phone,
                    EmailAddress = grower.Email,
                    Cultivations = string.Join(", ", grower.Cultivations.Select(c => c.Name)),
                    City = grower.Addresses.City,
                    Street = grower.Addresses.Street,
                    HouseNumber = grower.Addresses.HouseNumber,
                    ApartmentNumber = grower.Addresses.ApartmentNumber,
                    ZipCode = grower.Addresses.ZipCode,
                    Country = grower.Addresses.Countries.Name,
                    TaxId = grower.TaxId,
                    RegistrationNumber = grower.RegistrationNo,
                    Remarks = grower.Remarks
                }
                ));
        }
        #endregion
        #region Constructor
        public AllGrowersViewModel()
            :base()
        {
            base.DisplayName = "Growers";
        }
        #endregion
        #region Commands

        #endregion
        #region Functions
        public override void OnAdd()
        {
            AddNew<NewGrowerViewModel>();
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
