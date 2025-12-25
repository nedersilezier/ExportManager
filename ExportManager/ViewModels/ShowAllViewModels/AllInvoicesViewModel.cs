using ExportManager.Models;
using ExportManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllInvoicesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.Invoices.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllInvoicesViewModel()
            : base()
        {
            base.DisplayName = "Invoices";
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
