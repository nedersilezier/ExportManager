using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;

namespace ExportManager.ViewModels
{
    public class AllQualitiesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.Qualities.Where(t => t.IsActive == true));
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
            AddNew<NewQualityTypeViewModel>();
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
