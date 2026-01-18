using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;

namespace ExportManager.ViewModels.AddViewModels
{
    internal class NewPaymentMethodViewModel: NewItemViewModel<PaymentMethods>
    {
        #region Constructor
        public NewPaymentMethodViewModel()
            :base(new[] { "" })
        {
            base.DisplayName = "New payment method";
            item = new PaymentMethods();
        }
        public NewPaymentMethodViewModel(int paymentMethodId)
            : base(new[] { "" })
        {
            base.DisplayName = "Edit payment method";
            _IsEditMode = true;
            item = potplantsEntities.PaymentMethods.FirstOrDefault(t => t.PaymentMethodId == paymentMethodId);
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return item.Name; }
            set
            {
                if(item.Name != value) 
                    item.Name = value;
                OnPropertyChanged(()=> Name);
            }
        }
        public string Description
        {
            get { return item.Description; }
            set
            {
                if (item.Description != value)
                    item.Description = value;
                OnPropertyChanged(()=> Description);
            }
        }
        #endregion
        #region Commands
        //public override void Save()
        //{
        //    item.IsActive = true;
        //    potplantsEntities.PaymentMethods.Add(item);
        //    potplantsEntities.SaveChanges();
        //    RaiseAdded();
        //}
        #endregion
    }
}
