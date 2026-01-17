using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllGrowersViewModel : AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                var growers = shortLivedPotplantsEntities.Growers.Where(t => t.IsActive == true).ToList();
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
        }
        #endregion
        #region Constructor
        public AllGrowersViewModel()
            : base()
        {
            base.DisplayName = "Growers";
        }
        #endregion
        #region Commands

        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewGrowerViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewGrowerViewModel(SelectedItem.GrowerId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Growers>(SelectedItem.GrowerId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "City", "Country", "Registration number" };
        }
        public override void Sort()
        {
            switch (SortField)
            {
                case "Name":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.ClientName));
                    break;
                case "City":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.City));
                    break;
                case "Country":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Country));
                    break;
                case "Registration number":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.RegistrationNumber));
                    break;
            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Name", "City", "Country", "Cultivations", "Tax ID" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Name":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.ClientName != null && t.ClientName.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "City":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.City != null && t.City.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Country":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.Country != null && t.Country.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Cultivations":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.Cultivations != null && t.Cultivations.ToLower().Contains(FindTextBox.ToLower())));
                    break;
                case "Tax ID":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.TaxId != null && t.TaxId.StartsWith(FindTextBox)));
                    break;
            }
        }
        #endregion
    }
}
