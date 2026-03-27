using ExportManager.Models;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewInvoiceViewModel : NewItemViewModel<Invoices>
    {
        #region Fields
        private bool _IsPerOrder;
        private bool _IsPerPeriod;
        private NewInvoiceViewModel _CurrentViewModel;
        #endregion
        #region Properties
        public bool IsPerOrder
        {
            get { return _IsPerOrder; }
            set
            {
                if(_IsPerOrder != value )
                {
                    _IsPerOrder = value;
                    IsPerPeriod = !value;
                    OnPropertyChanged(() => IsPerOrder);
                    OnPropertyChanged(() => CurrentViewModel);
                }
            }
        }
        public bool IsPerPeriod
        {
            get { return _IsPerPeriod; }
            set
            {
                if (_IsPerPeriod != value)
                {
                    _IsPerPeriod = value;
                    IsPerOrder = !value;
                    OnPropertyChanged(() => IsPerPeriod);
                    OnPropertyChanged(() => CurrentViewModel);
                }
            }
        }
        public NewInvoicePerOrderViewModel PerOrderVM
        {
            get { return new NewInvoicePerOrderViewModel(); }
        }
        public NewInvoicePerPeriodViewModel PerPeriodVM
        {
            get { return new NewInvoicePerPeriodViewModel(); }
        }
        public NewInvoiceViewModel CurrentViewModel
        {
            get
            {
                if (_IsPerOrder)
                    return PerOrderVM;
                else
                    return PerPeriodVM;
            }
        }
        #endregion
        #region Constructor
        public NewInvoiceViewModel()
            : base(new[] { "" })
        {
            base.DisplayName = "New invoice";
            IsPerOrder = true;
        }
        #endregion
        #region Functions

        #endregion
    }
}
