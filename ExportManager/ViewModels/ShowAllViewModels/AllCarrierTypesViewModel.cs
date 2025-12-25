using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using ExportManager.ViewModels.AddViewModels;
namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllCarrierTypesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.CarrierTypes.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllCarrierTypesViewModel()
        : base()
        {
            base.DisplayName = "Carrier types";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            AddNew<NewCarrierTypeViewModel>();
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
