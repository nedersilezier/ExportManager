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
    internal class AllPaymentMethodsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.PaymentMethods.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllPaymentMethodsViewModel()
            : base()
        {
            base.DisplayName = "Payment methods";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            AddNew<NewPaymentMethodViewModel>();
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
