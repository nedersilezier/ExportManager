using ExportManager.Helper;
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
using System.Windows;
using System.Windows.Input;

namespace ExportManager.ViewModels.Windows
{
    public class EditInvoiceViewModel : BaseViewModel, IParameterReceiver<InvoiceParameter>
    {
        #region Fields
        private int _SelectedIndexTab;
        private InvoiceItemsListView _SelectedInvoiceItem;
        private PotplantsEntities potplantsEntities;
        private InvoicesListView _Invoice;
        private ObservableCollection<KeyAndValue> _PaymentMethods;
        private DateTime _PaymentDate;
        private InvoicePartyListView _Buyer;
        private InvoicePartyListView _Seller;
        private ObservableCollection<InvoiceItemsListView> _InvoiceItems;
        private ObservableCollection<InvoiceItemsListView> _InvoiceItemsToChange;
        private KeyAndValue _SelectedPaymentMethod;
        private decimal _TransportCost;
        private decimal _StorageCost;
        private decimal _TotalAmount;
        private Action InvoiceEdited;
        private bool _IsClosing = false;
        private bool _IsDeleteEnabled;
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
                    if (SelectedIndexTab == 2)
                    {
                        IsDeleteEnabled = true;
                    }
                    else
                    {
                        IsDeleteEnabled = false;
                    }
                    OnPropertyChanged(() => SelectedIndexTab);
                }
            }
        }
        public InvoiceItemsListView SelectedInvoiceItem
        {
            get { return _SelectedInvoiceItem; }
            set
            {
                if (_SelectedInvoiceItem != value)
                {
                    _SelectedInvoiceItem = value;
                    OnPropertyChanged(() => SelectedInvoiceItem);
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
        public ObservableCollection<InvoiceItemsListView> InvoiceItemsToChange
        {
            get
            {
                if (_InvoiceItemsToChange == null)
                    _InvoiceItemsToChange = new ObservableCollection<InvoiceItemsListView>();
                return _InvoiceItemsToChange;
            }
            set
            {
                if (_InvoiceItemsToChange != value)
                {
                    _InvoiceItemsToChange = value;
                    OnPropertyChanged(() => InvoiceItemsToChange);
                }
            }
        }
        public DateTime PaymentDate
        {
            get { return _PaymentDate; }
            set
            {
                if (_PaymentDate != value)
                {
                    if (Invoice != null && value.Date < Invoice.InvoiceDate.Date)
                    {
                        MessageBox.Show("Payment date cannot be earlier than invoice date.", "Invalid Date", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        return;
                    }
                    _PaymentDate = value;
                    OnPropertyChanged(() => PaymentDate);
                }
            }
        }
        public decimal TransportCost
        {
            get { return _TransportCost; }
            set
            {
                if (_TransportCost != value)
                {
                    _TransportCost = value;
                    if (Invoice != null)
                    {
                        Invoice.TotalTransportCost = value;
                        Invoice.TotalAmount = Invoice.TotalNet + Invoice.TotalTax + Invoice.TotalStorageCost + Invoice.TotalTransportCost;
                        TotalAmount = Invoice.TotalNet + Invoice.TotalTax + Invoice.TotalStorageCost + Invoice.TotalTransportCost;
                    }
                        
                    var transportItem = InvoiceItems?.FirstOrDefault(ii => ii.Name == "Transport");
                    if (transportItem != null)
                    {
                        if (InvoiceItemsToChange.Where(ii => ii.Name == "Transport").Count() == 0)
                            InvoiceItemsToChange.Add(transportItem);
                        transportItem.NetAmount = value;
                        transportItem.UnitPrice = value;
                        transportItem.GrossAmount = value;
                        InvoiceItems = new ObservableCollection<InvoiceItemsListView>(InvoiceItems);
                    }
                    OnPropertyChanged(() => TransportCost);
                    
                }
            }
        }
        public decimal StorageCost
        {
            get { return _StorageCost; }
            set
            {
                if (_StorageCost != value)
                {
                    _StorageCost = value;
                    if (Invoice != null)
                    {
                        Invoice.TotalStorageCost = value;
                        Invoice.TotalAmount = Invoice.TotalNet + Invoice.TotalTax + Invoice.TotalStorageCost + Invoice.TotalTransportCost;
                        TotalAmount = Invoice.TotalNet + Invoice.TotalTax + Invoice.TotalStorageCost + Invoice.TotalTransportCost;
                    }
                    var storageItem = InvoiceItems?.FirstOrDefault(ii => ii.Name == "Storage");
                    if (storageItem != null)
                    {
                        if (!InvoiceItemsToChange.Contains(storageItem))
                            InvoiceItemsToChange.Add(storageItem);
                        storageItem.NetAmount = value;
                        storageItem.UnitPrice = value;
                        storageItem.GrossAmount = value;
                        InvoiceItems = new ObservableCollection<InvoiceItemsListView>(InvoiceItems);
                    }
                    OnPropertyChanged(() => StorageCost);
                    OnPropertyChanged(() => InvoiceItems);
                }
            }
        }
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged(() => TotalAmount);
                }
            }
        }
        public bool IsClosing
        {
            get { return _IsClosing; }
            set
            {
                if (_IsClosing != value)
                {
                    _IsClosing = value;
                    OnPropertyChanged(() => IsClosing);
                }
            }
        }
        public bool IsDeleteEnabled
        {
            get { return _IsDeleteEnabled; }
            set
            {
                if (_IsDeleteEnabled != value)
                {
                    _IsDeleteEnabled = value;
                    OnPropertyChanged(() => IsDeleteEnabled);
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
            PaymentDate = Invoice.PaymentDate ?? DateTime.Now;
            InvoiceItems = new ObservableCollection<InvoiceItemsListView>(new InvoiceItemsQuery(potplantsEntities).GetInvoiceItemsDTO(Invoice.InvoiceId).ToList());
            TransportCost = Invoice.TotalTransportCost;
            StorageCost = Invoice.TotalStorageCost;
            TotalAmount = Invoice.TotalAmount;
            InvoiceItemsToChange = new ObservableCollection<InvoiceItemsListView>();
            InvoiceEdited += parameter.RefreshEvent;
            OnPropertyChanged(() => DisplayName);
        }
        #endregion
        #region Commands
        private BaseCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new BaseCommand(() => SaveAndClose());
                }
                return _SaveCommand;
            }
        }
        private BaseCommand _DeleteInvoiceItemCommand;
        public ICommand DeleteInvoiceItemCommand
        {
            get
            {
                if (_DeleteInvoiceItemCommand == null)
                {
                    _DeleteInvoiceItemCommand = new BaseCommand(() => DeleteInvoiceItem());
                }
                return _DeleteInvoiceItemCommand;
            }
        }
        private BaseCommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new BaseCommand(() => IsClosing = true);
                }
                return _CloseCommand;
            }
        }
        private void DeleteInvoiceItem()
        {
            if (SelectedInvoiceItem == null)
            {
                MessageBox.Show("Please select an invoice item to delete.", "No Item Selected", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            if (SelectedInvoiceItem.Name == "Transport" || SelectedInvoiceItem.Name == "Storage")
            {
                MessageBox.Show("Transport and Storage cost items cannot be deleted. You can set their amounts to 0 if you don't want them to be included in the invoice.", "Invalid Item", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show("Are you sure you want to delete the selected invoice item? This will move the corresponding order item back to stock", "Confirm Deletion", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            Invoice.TotalNet -= SelectedInvoiceItem.NetAmount ?? 0m;
            Invoice.TotalGross -= SelectedInvoiceItem.GrossAmount ?? 0m;
            Invoice.TotalTax -= SelectedInvoiceItem.TaxAmount ?? 0m;
            Invoice.TotalAmount -= SelectedInvoiceItem.GrossAmount ?? 0m;
            TotalAmount -= SelectedInvoiceItem.GrossAmount ?? 0m;
            OnPropertyChanged(() => Invoice);
            InvoiceItemsToChange.Add(SelectedInvoiceItem);
            InvoiceItems.Remove(SelectedInvoiceItem);
            OnPropertyChanged(() => InvoiceItems);
        }
        private void SaveAndClose()
        {
            var invoiceToEdit = potplantsEntities.Invoices
                .Include("InvoiceParties")
                .FirstOrDefault(i => i.InvoiceId == Invoice.InvoiceId);
            if (invoiceToEdit == null)
                return;

            invoiceToEdit.InvoiceDate = Invoice.InvoiceDate;
            invoiceToEdit.PaymentDate = PaymentDate;
            invoiceToEdit.TotalNet = Invoice.TotalNet;
            invoiceToEdit.TotalGross = Invoice.TotalGross;
            invoiceToEdit.TotalTax = Invoice.TotalTax;
            invoiceToEdit.TotalAmount = Invoice.TotalAmount;
            invoiceToEdit.TotalStorageCost = Invoice.TotalStorageCost;
            invoiceToEdit.TotalTransportCost = Invoice.TotalTransportCost;

            if (SelectedPaymentMethod != null)
                invoiceToEdit.PaymentMethodId = SelectedPaymentMethod.Key;

            // Update buyer fields
            var clientToEdit = invoiceToEdit.InvoiceParties.FirstOrDefault(ip => ip.Role == InvoicePartyRoles.Buyer);
            if (clientToEdit != null && Buyer != null)
            {
                clientToEdit.Name = Buyer.Name;
                clientToEdit.TaxId = Buyer.TaxId;
                clientToEdit.Street = Buyer.Street;
                clientToEdit.City = Buyer.City;
                clientToEdit.PostalCode = Buyer.PostalCode;
                clientToEdit.CountryCode = Buyer.CountryCode;
                clientToEdit.FullHouseNo = Buyer.FullHouseNumber;
                clientToEdit.Country = Buyer.Country;
            }

            if (InvoiceItemsToChange != null && InvoiceItemsToChange.Count > 0)
            {
                // build id sets for single-DB queries
                var invoiceItemIdsToChange = new HashSet<int>(InvoiceItemsToChange.Select(iitc => iitc.InvoiceItemId));

                var invoiceItemsToChange = potplantsEntities.InvoiceItems
                    .Where(ii => invoiceItemIdsToChange.Contains(ii.InvoiceItemId))
                    .ToList();
                //update transport and storage separately
                var transportCostItem = invoiceItemsToChange.FirstOrDefault(ii => ii.NameSnapshot == "Transport");
                if (transportCostItem != null)
                {
                    transportCostItem.NetAmount = TransportCost;
                    transportCostItem.UnitPriceSnapshot = TransportCost;
                    transportCostItem.GrossAmount = TransportCost;
                }
                invoiceItemsToChange.Remove(transportCostItem);

                var storageCostItem = invoiceItemsToChange.FirstOrDefault(ii => ii.NameSnapshot == "Storage");
                if (storageCostItem != null)
                {
                    storageCostItem.NetAmount = StorageCost;
                    storageCostItem.UnitPriceSnapshot = StorageCost;
                    storageCostItem.GrossAmount = StorageCost;
                }
                invoiceItemsToChange.Remove(storageCostItem);
                var orderItemIdsToChange = invoiceItemsToChange
                    .Where(ii => ii.SourceOrderItemId.HasValue)
                    .Select(ii => ii.SourceOrderItemId.Value)
                    .ToList();

                List<OrderItems> orderItemsToChange = new List<OrderItems>();
                Dictionary<int, OrderItems> orderItemsDict = null;
                Dictionary<int, StockItems> stockItemsDict = null;

                if (orderItemIdsToChange.Count > 0)
                {
                    orderItemsToChange = potplantsEntities.OrderItems
                        .Where(oi => orderItemIdsToChange.Contains(oi.OrderItemId))
                        .ToList();
                    orderItemsDict = orderItemsToChange.ToDictionary(oi => oi.OrderItemId);
                    var stockItemIdsToChange = orderItemsToChange
                        .Select(oi => oi.StockItemId)
                        .ToList();
                    var stockItemsToChange = potplantsEntities.StockItems
                        .Where(si => stockItemIdsToChange.Contains(si.StockItemId))
                        .ToList();
                    stockItemsDict = stockItemsToChange.ToDictionary(si => si.StockItemId);
                }
                // Temporarily disable AutoDetectChanges for bulk in-memory updates
                var cfg = potplantsEntities.Configuration;
                var originalAutoDetect = cfg.AutoDetectChangesEnabled;
                try
                {
                    cfg.AutoDetectChangesEnabled = false;

                    foreach (var item in invoiceItemsToChange)
                    {
                        item.IsActive = false;
                        item.DeletedAt = DateTime.Now;
                        item.DeletedBy = Environment.UserName;

                        if (item.SourceOrderItemId.HasValue && orderItemsDict != null)
                        {
                            if (orderItemsDict.TryGetValue(item.SourceOrderItemId.Value, out var orderItem))
                            {
                                orderItem.IsActive = false;
                                orderItem.DeletedAt = DateTime.Now;
                                orderItem.DeletedBy = Environment.UserName;

                                if (stockItemsDict != null && stockItemsDict.TryGetValue(orderItem.StockItemId, out var stockItem))
                                {
                                    stockItem.QuantityLeft += orderItem.Quantity;
                                }
                            }
                        }
                    }
                    // Ensure EF sees the batched changes before saving
                    potplantsEntities.ChangeTracker.DetectChanges();
                    potplantsEntities.SaveChanges();
                }
                finally
                {
                    cfg.AutoDetectChangesEnabled = originalAutoDetect;
                }
            }
            else
            {
                // No invoice items to change, just save other edits
                potplantsEntities.SaveChanges();
            }
            InvoiceEdited?.Invoke();
            IsClosing = true;
        }
        #endregion
    }
}
