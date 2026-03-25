using ExportManager.Helper;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExportManager.Models.BusinessLogic.ListViewsForUI;

namespace ExportManager.ViewModels.ReportViewModels
{
    public class InvoiceReportViewModel: WorkspaceViewModel
    {
        #region Fields
        public DateTime _Date;
        private decimal? _NetTotal;
        private decimal? _TaxTotal;
        private decimal? _GrossTotal;
        private ObservableCollection<KeyAndValue> _InvoiceComboBoxItems;
        private KeyAndValue _SelectedInvoice;
        private BaseCommand _CalculateInvoice;
        private ObservableCollection<InvoiceReportItemsListView> _InvoiceItemsList;
        #endregion
        #region Database
        public PotplantsEntities potplantsEntities;
        #endregion
        #region Constructor
        public InvoiceReportViewModel()
        {
            base.DisplayName = "Invoice report";
            potplantsEntities = new PotplantsEntities();
            Date = DateTime.Now;
        }
        #endregion
        #region Properties
        public ObservableCollection<InvoiceReportItemsListView> InvoiceItemsList
        {
            get
            {
                if (_InvoiceItemsList == null)
                    LoadInvoiceItems();
                return _InvoiceItemsList;
            }
            set
            {
                if (_InvoiceItemsList != value)
                {
                    _InvoiceItemsList = value;
                    OnPropertyChanged(() => InvoiceItemsList);
                }

            }
        }
        public ObservableCollection<KeyAndValue> InvoiceComboBoxItems
        {
            get { return _InvoiceComboBoxItems; }
            set
            {
                if (_InvoiceComboBoxItems != value)
                {
                    _InvoiceComboBoxItems = value;
                    OnPropertyChanged(() => InvoiceComboBoxItems);
                }
            }
        }

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    _InvoiceComboBoxItems = new InvoicesQuery(potplantsEntities).GetInvoicesListItemsPerDate(Date);
                    OnPropertyChanged(() => Date);
                    OnPropertyChanged(() => InvoiceComboBoxItems);
                }
            }
        }
        public decimal? NetTotal
        {
            get { return _NetTotal; }
            set
            {
                if (_NetTotal != value)
                {
                    _NetTotal = value;
                    OnPropertyChanged(() => NetTotal);
                }
            }
        }
        public decimal? TaxTotal
        {
            get { return _TaxTotal; }
            set
            {
                if (_TaxTotal != value)
                {
                    _TaxTotal = value;
                    OnPropertyChanged(() => TaxTotal);
                }
            }
        }
        public decimal? GrossTotal
        {
            get { return _GrossTotal; }
            set
            {
                if (_GrossTotal != value)
                {
                    _GrossTotal = value;
                    OnPropertyChanged(() => GrossTotal);
                }
            }
        }
        public KeyAndValue SelectedInvoice
        {
            get { return _SelectedInvoice; }
            set
            {
                if (_SelectedInvoice != value)
                {
                    _SelectedInvoice = value;
                    OnPropertyChanged(() => SelectedInvoice);
                }
            }
        }
        public ICommand CalculateInvoice
        {
            get
            {
                if (_CalculateInvoice == null)
                    _CalculateInvoice = new BaseCommand(calculateInvoiceClick);
                return _CalculateInvoice;
            }
        }
        #endregion
        #region Functions
        public void LoadInvoiceItems()
        {
            if (SelectedInvoice == null)
                return;
            try
            {
                InvoiceItemsList = new ObservableCollection<InvoiceReportItemsListView>(
                new InvoiceCalculator(potplantsEntities).InvoiceItemsQuery(SelectedInvoice.Key, Date).ToList());
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }
        private void calculateInvoiceClick()
        {
            if (SelectedInvoice == null)
            {
                NetTotal = null;
                TaxTotal = null;
                GrossTotal = null;
                InvoiceItemsList = null;
                return;
            }
                
            NetTotal = new InvoiceCalculator(potplantsEntities).CalculateNetTotal(SelectedInvoice.Key, Date);
            TaxTotal = new InvoiceCalculator(potplantsEntities).CalculateTaxTotal(SelectedInvoice.Key, Date);
            GrossTotal = new InvoiceCalculator(potplantsEntities).CalculateGrossTotal(SelectedInvoice.Key, Date);
            LoadInvoiceItems();
        }
        #endregion
    }
}
