using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.ViewModels.Abstract;

namespace ExportManager.ViewModels
{
    public class AllBatchesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.StockItems.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllBatchesViewModel()
            : base()
        {
            base.DisplayName = "Batches";
        }
        #endregion

        #region Functions
        public override void OnAdd()
        {
            return;
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
