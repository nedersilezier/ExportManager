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
    internal class AllCultivationsViewModel: AllViewModel<Cultivations>
    {
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<Cultivations>(shortLivedPotplantsEntities.Cultivations.Where(t => t.IsActive == true).ToList());
            }
        }
        #endregion
        #region Constructor
        public AllCultivationsViewModel()
            :base()
        {
            base.DisplayName = "Cultivations";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewCultivationViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewCultivationViewModel(SelectedItem.CultivationId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Cultivations>(SelectedItem.CultivationId);
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
                    List = new ObservableCollection<Cultivations>(List.OrderBy(t => t.Name));
                    break;
                case "Description":
                    List = new ObservableCollection<Cultivations>(List.OrderBy(t => t.Description));
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
                    List = new ObservableCollection<Cultivations>(List.Where(t => t.Name != null && t.Name.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Description":
                    Load();
                    List = new ObservableCollection<Cultivations>(List.Where(t => t.Description != null && t.Description.ToLower().Contains(FindTextBox.ToLower())));
                    break;
            }

        }
        #endregion
    }
}
