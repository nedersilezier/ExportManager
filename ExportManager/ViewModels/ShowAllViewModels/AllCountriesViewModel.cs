using ExportManager.Models;
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
                (potplantsEntities.Countries.Where(t => t.IsActive == true).Select(country => new AllCountriesViewModel
                {
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
        public string Name { get; set; }
        public string ISO2Code { get; set; }
        public string PhoneCode { get; set; }
        public bool IsEUMember { get; set; }
        public string IsEUMemberToText
        {
            get { return IsEUMember ? "Yes" : "No"; }
        }
        public string Remarks { get; set; }
        public DateTime? UpdatedAt { get; set; }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab<NewCountryViewModel>(Load);
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
