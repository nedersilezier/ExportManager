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
    public class AllColorsViewModel: AllViewModel<Colors>
    {
        #region List
        public override void Load()
        {
            using(var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<Colors>(shortLivedPotplantsEntities.Colors.Where(t => t.IsActive == true).ToList());
            }
        }
        #endregion
        #region Constructor
        public AllColorsViewModel()
            :base()
        {
            base.DisplayName = "Colors";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewColorViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewColorViewModel(SelectedItem.ColorId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Colors>(SelectedItem.ColorId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "Remarks" };
        }
        public override void Sort()
        {
            switch (SortField)
            {
                case "Name":
                    List = new ObservableCollection<Colors>(List.OrderBy(t => t.Name));
                    break;
                case "Remarks":
                    List = new ObservableCollection<Colors>(List.OrderBy(t => t.Remarks));
                    break;
            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Name", "Remarks" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Name":
                    Load();
                    List = new ObservableCollection<Colors>(List.Where(t => t.Name != null && t.Name.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Remarks":
                    Load();
                    List = new ObservableCollection<Colors>(List.Where(t => t.Remarks != null && t.Remarks.ToLower().Contains(FindTextBox.ToLower())));
                    break;
            }

        }
        #endregion
    }
}
