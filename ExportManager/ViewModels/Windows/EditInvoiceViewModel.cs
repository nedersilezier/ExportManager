using ExportManager.Models;
using ExportManager.Models.BusinessLogic.Queries;
using ExportManager.Models.DTO;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Extensions;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.Windows
{
    public class EditInvoiceViewModel : BaseViewModel, IParameterReceiver<InvoiceParameter>
    {
        #region Fields
        private int _SelectedIndexTab;
        private PotplantsEntities potplantsEntities;
        private InvoicesListView _Invoice;
        private ObservableCollection<KeyAndValue> _PaymentMethods;
        private InvoicePartyListView _Buyer;
        private InvoicePartyListView _Seller;
        private ObservableCollection<InvoiceItemsListView> _InvoiceItems;
        private KeyAndValue _SelectedPaymentMethod;
        private Action InvoiceEdited;
        #endregion
        #region Constructor
        public EditInvoiceViewModel()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
        #region Properties
        public int SelectedIndexTab
        {
            get { return _SelectedIndexTab; }
            set
            {
                if (_SelectedIndexTab != value)
                {
                    _SelectedIndexTab = value;
                    OnPropertyChanged(() => SelectedIndexTab);
                }
            }
        }
        public KeyAndValue SelectedPaymentMethod
        {
            get { return _SelectedPaymentMethod; }
            set
            {
                if (_SelectedPaymentMethod != value)
                {
                    _SelectedPaymentMethod = value;
                    OnPropertyChanged(() => SelectedPaymentMethod);
                }
            }
        }
        public ObservableCollection<KeyAndValue> PaymentMethods
        {
            get
            {
                if (_PaymentMethods == null)
                {
                    _PaymentMethods = new ObservableCollection<KeyAndValue>(new PaymentMethodsQuery(potplantsEntities).GetPaymentMethodsForCombobox());
                }
                return _PaymentMethods;
            }
            set
            {
                if (_PaymentMethods != value)
                {
                    _PaymentMethods = value;
                    OnPropertyChanged(() => PaymentMethods);
                }
            }
        }
        public InvoicesListView Invoice
        {
            get { return _Invoice; }
            set
            {
                if (_Invoice != value)
                {
                    _Invoice = value;
                    OnPropertyChanged(() => Invoice);
                }
            }
        }
        public int InvoiceId
        {
            get { return _InvoiceId; }
            set
            {
                if (_InvoiceId != value)
                {
                    _InvoiceId = value;
                    OnPropertyChanged(() => InvoiceId);
                }
            }
        }
        public InvoicePartyListView Buyer
        {
            get { return _Buyer; }
            set
            {
                if (_Buyer != value)
                {
                    _Buyer = value;
                    OnPropertyChanged(() => Buyer);
                }
            }
        }
        public InvoicePartyListView Seller
        {
            get { return _Seller; }
            set
            {
                if (_Seller != value)
                {
                    _Seller = value;
                    OnPropertyChanged(() => Seller);
                }
            }
        }
        public ObservableCollection<InvoiceItemsListView> InvoiceItems
        {
            get { return _InvoiceItems; }
            set
            {
                if (_InvoiceItems != value)
                {
                    _InvoiceItems = value;
                    OnPropertyChanged(() => InvoiceItems);
                }
            }
        }
        #endregion
        #region Functions
        public void SetParameter(InvoiceParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }
            DisplayName = parameter.Title;
            Invoice = parameter.Invoice;
            Buyer = Invoice.Buyer;
            Seller = Invoice.Seller;
            SelectedPaymentMethod = PaymentMethods.FirstOrDefault(pm => pm.Value == Invoice.PaymentMethod);
            InvoiceItems = new ObservableCollection<InvoiceItemsListView>(new InvoiceItemsQuery(potplantsEntities).GetInvoiceItemsDTO(Invoice.InvoiceId).ToList());
            InvoiceEdited += parameter.RefreshEvent;
            OnPropertyChanged(() => DisplayName);
        }
        #endregion
    }
}
