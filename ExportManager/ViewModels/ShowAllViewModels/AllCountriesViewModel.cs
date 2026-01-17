using ExportManager.Models;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllCountriesViewModel : AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            //List = new ObservableCollection<dynamic>(potplantsEntities.Countries.Where(t => t.IsActive == true).ToList());
            List = new ObservableCollection<dynamic>
                (potplantsEntities.Countries.Where(t => t.IsActive == true).Select(country => new CountriesListView
                {
                    CountryId = country.CountryId,
                    Name = country.Name,
                    ISO2Code = country.ISO2Code,
                    PhoneCode = country.PhoneCode,
                    IsEUMember = (bool)country.IsEUMember,
                    Remarks = country.Remarks,
                    UpdatedAt = country.UpdatedAt
                }).ToList());
        }
        #endregion
        #region Constructor
        public AllCountriesViewModel()
            : base()
        {
            base.DisplayName = "Countries";

        }
        #endregion
        #region Properties
        //public int CountryId { get; set; }
        //public string Name { get; set; }
        //public string ISO2Code { get; set; }
        //public string PhoneCode { get; set; }
        //public bool IsEUMember { get; set; }
        //public string IsEUMemberToText
        //{
        //    get { return IsEUMember ? "Yes" : "No"; }
        //}
        //public string Remarks { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewCountryViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewCountryViewModel(SelectedItem.CountryId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Countries>(SelectedItem.CountryId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "ISO2 Code", "Phone code", "Continent" };
        }
        public override void Sort()
        {
            switch (SortField)
            {
                case "Name":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Name));
                    break;
                case "ISO2 Code":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.ISO2Code));
                    break;
                case "Phone code":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.PhoneCode));
                    break;
                case "Continent":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Continent));
                    break;

            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Name", "ISO2 Code", "Phone code", "Continent" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Name":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.Name != null && t.Name.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "ISO2 Code":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.ISO2Code != null && t.ISO2Code.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Phone code":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.PhoneCode != null && t.PhoneCode.StartsWith(FindTextBox)));
                    break;
                case "Continent":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.Continent != null && t.Continent.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
            }

        }
        #endregion
    }
}
