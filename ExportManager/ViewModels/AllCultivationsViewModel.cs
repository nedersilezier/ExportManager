using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels
{
    internal class AllCultivationsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.Cultivations.Where(t => t.IsActive == true).ToList());
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
            AddNew<NewCultivationViewModel>();
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
