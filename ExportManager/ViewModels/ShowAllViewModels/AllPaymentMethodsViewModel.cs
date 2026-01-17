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
    internal class AllPaymentMethodsViewModel: AllViewModel<PaymentMethods>
    {
        #region List
        public override void Load()
        {
            using(var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<PaymentMethods>(shortLivedPotplantsEntities.PaymentMethods.Where(t => t.IsActive == true).ToList());
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
            SoftDelete<PaymentMethods>(SelectedItem.PaymentMethodId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "Description" };
        }
        public override void Sort()
        {
            switch (SortField)
            {
                case "Name":
                    List = new ObservableCollection<PaymentMethods>(List.OrderBy(t => t.Name));
                    break;
                case "Description":
                    List = new ObservableCollection<PaymentMethods>(List.OrderBy(t => t.Description));
                    break;
            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Name", "Description" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Name":
                    Load();
                    List = new ObservableCollection<PaymentMethods>(List.Where(t => t.Name != null && t.Name.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Description":
                    Load();
                    List = new ObservableCollection<PaymentMethods>(List.Where(t => t.Description != null && t.Description.ToLower().Contains(FindTextBox.ToLower())));
                    break;
            }

        }
        #endregion
    }
}
