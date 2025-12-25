using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllTrayTypesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.TrayTypes.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllTrayTypesViewModel()
            : base()
        {
            base.DisplayName = "Tray types";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            AddNew<NewTrayTypeViewModel>();
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
