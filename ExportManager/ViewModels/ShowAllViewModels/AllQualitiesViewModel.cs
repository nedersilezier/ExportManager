using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllQualitiesViewModel: AllViewModel<Qualities>
    {
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<Qualities>(shortLivedPotplantsEntities.Qualities.Where(t => t.IsActive == true));
            }
        }
        #endregion
        #region Constructor
        public AllQualitiesViewModel()
            :base()
        {
            base.DisplayName = "Quality types";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewQualityTypeViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewQualityTypeViewModel(SelectedItem.QualityId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Qualities>(SelectedItem.QualityId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "Description" };
        }
        public override void Sort()
        {
            switch (SortField)
            {
                case "Name":
                    List = new ObservableCollection<Qualities>(List.OrderBy(t => t.Name));
                    break;
                case "Description":
                    List = new ObservableCollection<Qualities>(List.OrderBy(t => t.Description));
                    break;
            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Name", "Description" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Name":
                    Load();
                    List = new ObservableCollection<Qualities>(List.Where(t => t.Name != null && t.Name.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Description":
                    Load();
                    List = new ObservableCollection<Qualities>(List.Where(t => t.Description != null && t.Description.ToLower().Contains(FindTextBox.ToLower())));
                    break;
            }

        }
        #endregion
    }
}
