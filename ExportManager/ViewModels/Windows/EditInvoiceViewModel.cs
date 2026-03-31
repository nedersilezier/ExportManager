using ExportManager.Models;
using ExportManager.Models.BusinessLogic.Queries;
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
        private int _InvoiceId;
        private int _SelectedIndexTab;
        private PotplantsEntities potplantsEntities;
        private Invoices _Invoice;
        private string _InvoiceNo;
        private DateTime _InvoiceDate;
        private DateTime? _PaymentDate;
        private ObservableCollection<KeyAndValue> _PaymentMethods;
        private InvoiceParties _Buyer;
        private InvoiceParties _Seller;
        private ObservableCollection<InvoiceItems> _InvoiceItems;
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
        public Invoices Invoice
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
        public InvoiceParties Buyer
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
        public InvoiceParties Seller
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
        public ObservableCollection<InvoiceItems> InvoiceItems
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
            InvoiceId = parameter.InvoiceId;
            Invoice = potplantsEntities.Invoices.FirstOrDefault(i => i.InvoiceId == InvoiceId);
            Buyer = Invoice.InvoiceParties.FirstOrDefault(ip => ip.IsActive == true && ip.Role == InvoicePartyRoles.Buyer);
            Seller = Invoice.InvoiceParties.FirstOrDefault(ip => ip.IsActive == true && ip.Role == InvoicePartyRoles.Seller);
            SelectedPaymentMethod = PaymentMethods.FirstOrDefault(pm => pm.Key == Invoice.PaymentMethodId);
            InvoiceItems = new ObservableCollection<InvoiceItems>(Invoice.InvoiceItems.Where(ii => ii.IsActive == true).ToList());
            InvoiceEdited += parameter.RefreshEvent;
            OnPropertyChanged(() => DisplayName);
        }
        #endregion
    }
}
