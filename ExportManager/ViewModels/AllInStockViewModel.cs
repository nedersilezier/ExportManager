using ExportManager.Models;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ExportManager.ViewModels
{
    public class AllInStockViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.StockItems.Where(t => t.IsActive == true && t.IsBlocked == false).ToList());
        }
        #endregion
        #region Constructor
        public AllInStockViewModel()
            : base()
        {
            base.DisplayName = "Stock";
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
