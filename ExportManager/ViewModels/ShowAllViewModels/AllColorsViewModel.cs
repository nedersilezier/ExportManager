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
    public class AllColorsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.Colors.Where(t => t.IsActive == true).ToList());
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
            AddNew<NewColorViewModel>();
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
