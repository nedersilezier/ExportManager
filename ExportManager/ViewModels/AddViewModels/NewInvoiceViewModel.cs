using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.Parameters;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.ViewModels.ShowAllViewModels;
using ExportManager.Views.ShowAllViews;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewInvoiceViewModel : NewItemViewModel<Invoices>
    {
        #region Fields
        private bool _IsPerOrder;
        private bool _IsPerPeriod;
        private NewInvoicePerOrderViewModel _perOrderVM;
        private NewInvoicePerPeriodViewModel _perPeriodVM;
        #endregion
        #region Properties
        public bool IsPerOrder
        {
            get { return _IsPerOrder; }
            set
            {
                if (_IsPerOrder != value)
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
            get
            {
                if (_perOrderVM == null)
                {
                    _perOrderVM = new NewInvoicePerOrderViewModel();
                    _perOrderVM.Owner = this;
                }
                return _perOrderVM;
            }
        }
        public NewInvoicePerPeriodViewModel PerPeriodVM
        {
            get
            {
                if (_perPeriodVM == null)
                    _perPeriodVM = new NewInvoicePerPeriodViewModel();
                return _perPeriodVM;
            }
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
        public override void Save()
        {
            CurrentViewModel.Save();
        }
        #endregion
        #region ItemPicker
        public void openSelectOrderTab(Action<OrderSelectionResult> itemSetter)
        {
            OpenNewTab(() => new AllOrdersViewModel(itemSetter));
        }
        #endregion
    }
}
