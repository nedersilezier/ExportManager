using ExportManager.Helper;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion
        #region Constructor
        public NewInvoicePerOrderViewModel()
        {
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
        //invoice related
        public string InvoiceNo { get; private set; }
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
