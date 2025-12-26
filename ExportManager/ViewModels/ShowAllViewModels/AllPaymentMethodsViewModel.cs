using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    internal class AllPaymentMethodsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using(var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<dynamic>(shortLivedPotplantsEntities.PaymentMethods.Where(t => t.IsActive == true).ToList());
            }
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
            OpenNewTab(() => new NewPaymentMethodViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewPaymentMethodViewModel(SelectedItem.PaymentMethodId), Load);
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
    }
}
