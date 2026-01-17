using ExportManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;
namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllCategoriesViewModel : AllViewModel<Categories>
    {
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<Categories>(shortLivedPotplantsEntities.Categories.Where(t => t.IsActive == true).ToList());
            }
        }
        #endregion
        #region Constructor
        public AllCategoriesViewModel()
        : base()
        {
            base.DisplayName = "Categories";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewCategoryViewModel(), Load);
        }
        public override void OnEdit()
        {
            if (SelectedItem == null)
                return;
            OpenNewTab(() => new NewCategoryViewModel(SelectedItem.CategoryId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Categories>(SelectedItem.CategoryId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "Remarks" };
        }
        public override void Sort()
        {
            switch(SortField)
            {
                case "Name":
                    List = new ObservableCollection<Categories>(List.OrderBy(t => t.Name));
                    break;
                case "Remarks":
                    List = new ObservableCollection<Categories>(List.OrderBy(t => t.Remarks));
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
                    List = new ObservableCollection<Categories>(List.Where(t => t.Name != null && t.Name.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Remarks":
                    Load();
                    List = new ObservableCollection<Categories>(List.Where(t => t.Remarks != null && t.Remarks.ToLower().Contains(FindTextBox.ToLower())));
                    break;
            }

        }
        #endregion
    }
}
