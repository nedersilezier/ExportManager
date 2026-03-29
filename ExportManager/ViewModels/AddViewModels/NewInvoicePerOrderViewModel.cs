using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.Commands;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
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
using System.Windows;
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewInvoicePerOrderViewModel: NewInvoiceViewModel
    {
        #region Fields
        private int _OrderId;
        private int _ClientId;
        private string _ClientCode;
        private string _ClientName;
        private DateTime? _OrderDate;
        private DateTime? _DeliveryDate;
        private DateTime? _InvoiceDate;
        private DateTime? _PaymentDate;
        private ObservableCollection<KeyAndValue> _PaymentMethods;
        private KeyAndValue _SelectedPaymentMethod;
        private string _Status;
        private string _Remarks;
        private InvoicePartyListView _Client;
        #endregion
        #region Constructor
        public NewInvoicePerOrderViewModel()
        {
            InvoiceDate = DateTime.Now.Date;
            Status = InvoiceStatuses.Draft;
        }
        #endregion
        #region Properties
        public NewInvoiceViewModel Owner { get; set; }
        //order related
        public int OrderId
        {
            get { return _OrderId; }
            set
            {
                if (_OrderId != value)
                {
                    _OrderId = value;
                    OnPropertyChanged(() => OrderId);
                }
            }
        }
        public int ClientId
        {
            get { return _ClientId; }
            set
            {
                if (_ClientId != value)
                {
                    _ClientId = value;
                    OnPropertyChanged(() => ClientId);
                }
            }
        }
        public string ClientCode
        {
            get { return _ClientCode; }
            set
            {
                if (_ClientCode != value)
                {
                    _ClientCode = value;
                    OnPropertyChanged(() => ClientCode);
                }
            }
        }
        public string ClientName
        {
            get { return _ClientName; }
            set
            {
                if (_ClientName != value)
                {
                    _ClientName = value;
                    OnPropertyChanged(() => ClientName);
                }
            }
        }
        public DateTime? OrderDate
        {
            get { return _OrderDate; }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged(() => OrderDate);
                }
            }
        }
        public DateTime? DeliveryDate
        {
            get { return _DeliveryDate; }
            set
            {
                if (_DeliveryDate != value)
                {
                    _DeliveryDate = value;
                    OnPropertyChanged(() => DeliveryDate);
                }
            }
        }
        public string ClientDisplayName
        {
            get 
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(ClientName))
                    parts.Add(ClientName);
                if (!string.IsNullOrEmpty(ClientCode))
                    parts.Add($"({ClientCode})");

                return parts.Count > 0 ? string.Join(" ", parts) : "Select order";
            }
        }
        public InvoicePartyListView Client
        {
            get { return _Client; }
            set
            {
                if (_Client != value)
                {
                    _Client = value;
                    OnPropertyChanged(() => Client);
                }
            }
        }
        //invoice related
        public string InvoiceNo { get; private set; }
        public DateTime? InvoiceDate
        {
            get { return _InvoiceDate; }
            set
            {
                if (_InvoiceDate != value)
                {
                    _InvoiceDate = value;
                    OnPropertyChanged(() => InvoiceDate);
                }
            }
        }
        public DateTime? PaymentDate
        {
            get { return _PaymentDate; }
            set
            {
                if (_PaymentDate != value)
                {
                    if(value < InvoiceDate)
                    {
                        MessageBox.Show("Payment date cannot be before the invoice date.", "Invalid Payment Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _PaymentDate = value;
                    OnPropertyChanged(() => PaymentDate);
                }
            }
        }
        public ObservableCollection<KeyAndValue> PaymentMethods
        {
            get
            {
                if(_PaymentMethods == null)
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
        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged(() => Status);
                }
            }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged(() => Remarks);
                }
            }
        }
        #endregion
        #region Functions
        public void setSelectedOrder(OrderSelectionResult orderSelection)
        {
            OrderId = orderSelection.OrderId;
            Console.WriteLine($"Selected order id: {OrderId}");
            ClientId = orderSelection.ClientId;
            ClientCode = orderSelection.ClientCode;
            ClientName = orderSelection.ClientName;
            OrderDate = orderSelection.OrderDate;
            DeliveryDate = orderSelection.DeliveryDate;
            InvoiceNo = GenerateInvoiceNo();
            Client = new InvoicePartiesQuery(potplantsEntities).GetFromClient(orderSelection.ClientId); 
        }
        private string GenerateInvoiceNo()
        {
            int year = DateTime.Now.Year;
            int invoiceCountForYear = new InvoicesQuery(potplantsEntities).CountInvoicesPerClientPerYear(ClientId, year);
            int nextInvoiceNumber = invoiceCountForYear + 1;
            return $"{ClientCode}-{year}-{nextInvoiceNumber:D4}";
        }
        #endregion
        #region Commands
        private BaseCommand _SelectPerOrder;
        public ICommand SelectOrderCommand
        {
            get
            {
                if (_SelectPerOrder == null)
                    _SelectPerOrder = new BaseCommand(() => Owner?.openSelectOrderTab(setSelectedOrder));
                return _SelectPerOrder;
            }
        }
        #endregion
    }
}
