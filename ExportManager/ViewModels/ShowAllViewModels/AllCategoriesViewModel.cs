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
            return;
        }
        #endregion
    }
}
