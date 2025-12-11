using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels
{
    public class AllOrdersViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.Orders.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllOrdersViewModel()
            : base()
        {
            base.DisplayName = "Orders";
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
