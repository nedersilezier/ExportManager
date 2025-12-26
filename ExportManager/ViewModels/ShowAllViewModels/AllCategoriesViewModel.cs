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
namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllCategoriesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.Categories.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllCategoriesViewModel() 
        :base()
        {
            base.DisplayName = "Categories";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab<NewCategoryViewModel>(Load);
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
