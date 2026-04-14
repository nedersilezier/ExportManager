using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic;
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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewInvoicePerOrderViewModel : NewInvoiceViewModel
    {
        #region Fields
        private int _SelectedTabIndex;
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
        private ObservableCollection<InvoiceItemsListView> _OrderItemsList;
        private decimal _TaxRate;
        private decimal _TransportCost;
        private decimal _StorageCost;
        private decimal _NetAmount;
        private decimal _TaxAmount;
        private decimal _GrossAmount;
        #endregion
        #region Constructor
        public NewInvoicePerOrderViewModel()
        {
            InvoiceDate = DateTime.Now;
            Status = InvoiceStatuses.Draft;
        }
        #endregion
        #region Properties
        public int SelectedTabIndex
        {
            get { return _SelectedTabIndex; }
            set
            {
                if (_SelectedTabIndex != value)
                {
                    _SelectedTabIndex = value;
                    OnPropertyChanged(() => SelectedTabIndex);
                }
            }
        }
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
                    if (value?.Date < InvoiceDate?.Date)
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
        //invoice items related
        public ObservableCollection<InvoiceItemsListView> OrderItemsList
        {
            get { return _OrderItemsList; }
            set
            {
                if (_OrderItemsList != value)
                {
                    _OrderItemsList = value;
                    OnPropertyChanged(() => OrderItemsList);
                }
            }
        }
        public decimal TaxRate
        {
            get { return _TaxRate; }
            set
            {
                if (_TaxRate != value)
                {
                    _TaxRate = value;
                    //refresh invoice items preview when margin changes
                    if (OrderId > 0)
                    {
                        GrossAmount = _NetAmount * (1 + value / 100);
                        TaxAmount = _NetAmount * value / 100;
                        UpdateInvoiceItemsPreview();
                        OnPropertyChanged(() => GrossAmount);
                        OnPropertyChanged(() => TaxAmount);
                        OnPropertyChanged(() => OrderItemsList);
                    }
                    OnPropertyChanged(() => TaxRate);
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
                    OnPropertyChanged(() => TransportCost);
                    UpdateInvoiceItemsPreview();
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
                    OnPropertyChanged(() => StorageCost);
                    UpdateInvoiceItemsPreview();
                }
            }
        }
        public decimal NetAmount
        {
            get { return _NetAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged(() => NetAmount);
                }
            }
        }
        public decimal TaxAmount
        {
            get { return _TaxAmount; }
            set
            {
                if (_TaxAmount != value)
                {
                    _TaxAmount = value;
                    OnPropertyChanged(() => TaxAmount);
                }
            }
        }
        public decimal GrossAmount
        {
            get { return _GrossAmount; }
            set
            {
                if (_GrossAmount != value)
                {
                    _GrossAmount = value;
                    OnPropertyChanged(() => GrossAmount);
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
            OrderItemsList = new ObservableCollection<InvoiceItemsListView>(
                new OrderItemsQuery(potplantsEntities).GetInvoiceItemsPreview(orderSelection.OrderId, TaxRate));
            _TransportCost = OrderItemsList.FirstOrDefault(i => i.Name == "Transport")?.NetAmount ?? 0;
            _StorageCost = OrderItemsList.FirstOrDefault(i => i.Name == "Storage")?.NetAmount ?? 0;
            OnPropertyChanged(() => TransportCost);
            OnPropertyChanged(() => StorageCost);
            UpdateInvoiceItemsPreview();
            NetAmount = OrderItemsList.Where(oi => oi.Name != "Transport" && oi.Name != "Storage").Sum(oi => oi.NetAmount) ?? 0;
            GrossAmount = NetAmount * (1 + TaxRate / 100);
        }
        private string GenerateInvoiceNo()
        {
            int year = DateTime.Now.Year;
            int invoiceCountForYear = new InvoicesQuery(potplantsEntities).CountInvoicesPerClientPerYear(ClientId, year);
            int nextInvoiceNumber = invoiceCountForYear + 1;
            return $"{ClientCode}-{year}-{nextInvoiceNumber:D4}";
        }
        private void UpdateInvoiceItemsPreview()
        {
            foreach (var item in OrderItemsList)
            {
                if (item.Name == "Transport")
                {
                    item.UnitPrice = TransportCost;
                    item.NetAmount = TransportCost;
                    continue;
                }
                if (item.Name == "Storage")
                {
                    item.UnitPrice = StorageCost;
                    item.NetAmount = StorageCost;
                    continue;
                }
                var temporaryNetAmount = item.NetAmount;
                item.GrossAmount = temporaryNetAmount * (1 + TaxRate / 100);
                var temporaryGrossAmount = item.GrossAmount;
                item.TaxAmount = temporaryGrossAmount - temporaryNetAmount;
                OrderItemsList = new ObservableCollection<InvoiceItemsListView>(OrderItemsList);
            }
        }
        public override void Save()
        {
            if (OrderId <= 0)
            {
                throw new Exception("Please select an order before saving the invoice.");
            }
            if (PaymentDate == null)
            {
                throw new Exception("Please select a payment date before saving the invoice.");
            }
            if (SelectedPaymentMethod == null || SelectedPaymentMethod.Key <= 0)
            {
                throw new Exception("Please select a payment method before saving the invoice.");
            }
            using (var transaction = potplantsEntities.Database.BeginTransaction())
            {
                try
                {
                    CreateInvoice();
                    potplantsEntities.Invoices.Add(item);
                    new OrdersCommand(potplantsEntities).UpdateOrderStatus(OrderId, OrderStatuses.Invoiced);
                    potplantsEntities.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"An error occurred while saving the invoice: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateInvoice()
        {
            var now = DateTime.Now;
            var user = Environment.UserName;

            item.InvoiceNo = InvoiceNo;
            item.InvoiceDate = InvoiceDate ?? DateTime.Now.Date;
            item.PaymentDate = PaymentDate;
            item.PaymentMethodId = SelectedPaymentMethod.Key;
            item.Status = Status;
            item.Remarks = Remarks;
            item.TotalNet = NetAmount;
            item.TotalTax = TaxAmount;
            item.TotalGross = GrossAmount;
            item.TotalTransportCost = TransportCost;
            item.TotalStorageCost = StorageCost;
            item.TotalAmount = GrossAmount + TransportCost + StorageCost;
            item.IsApproved = false;
            item.IsActive = true;
            item.CreatedAt = now;
            item.CreatedBy = user;
            item.UpdatedAt = now;
            item.UpdatedBy = user;
            var transportItem = OrderItemsList.FirstOrDefault(i => i.Name == "Transport");
            var storageItem = OrderItemsList.FirstOrDefault(i => i.Name == "Storage");
            var transportInvoiceItem = new InvoiceItems
            {
                ItemNo = transportItem.ItemNo,
                NameSnapshot = transportItem?.Name,
                UnitPriceSnapshot = transportItem?.UnitPrice ?? 0,
                Quantity = transportItem?.Quantity ?? 0,
                NetAmount = transportItem?.NetAmount ?? 0,
                TaxRateSnapshot = 0, //assuming transport cost is not taxed
                IsActive = true,
                CreatedAt = now,
                CreatedBy = user,
                UpdatedAt = now,
                UpdatedBy = user,
            };
            potplantsEntities.InvoiceItems.Add(transportInvoiceItem);
            var storageInvoiceItem = new InvoiceItems
            {
                ItemNo = storageItem.ItemNo,
                NameSnapshot = storageItem?.Name,
                UnitPriceSnapshot = storageItem?.UnitPrice ?? 0,
                Quantity = storageItem?.Quantity ?? 0,
                NetAmount = storageItem?.NetAmount ?? 0,
                TaxRateSnapshot = 0, //assuming storage cost is not taxed
                IsActive = true,
                CreatedAt = now,
                CreatedBy = user,
                UpdatedAt = now,
                UpdatedBy = user,
            };
            potplantsEntities.InvoiceItems.Add(storageInvoiceItem);
            foreach (var orderItem in OrderItemsList.Skip(2))
            {
                var invoiceItem = new InvoiceItems
                {
                    SourceOrderItemId = orderItem.SourceOrderItemId,
                    ItemNo = orderItem.ItemNo,
                    NameSnapshot = orderItem.Name,
                    PotSizeSnapshot = orderItem.Potsize,
                    HeightSnapshot = orderItem.Height,
                    Quantity = orderItem.Quantity,
                    UnitPriceSnapshot = orderItem.UnitPrice,
                    TaxRateSnapshot = TaxRate,
                    NetAmount = orderItem.NetAmount,
                    TaxAmount = orderItem.TaxAmount,
                    GrossAmount = orderItem.GrossAmount,
                    IsActive = true,
                    CreatedAt = now,
                    CreatedBy = user,
                    UpdatedAt = now,
                    UpdatedBy = user,
                };
                item.InvoiceItems.Add(invoiceItem);
            }
            item.InvoiceParties.Add(new InvoiceParties
            {
                Name = Client.Name,
                TaxId = Client.TaxId,
                Street = Client.Street,
                FullHouseNo = Client.FullHouseNumber,
                City = Client.City,
                PostalCode = Client.PostalCode,
                CountryCode = Client.CountryCode,
                Country = Client.Country,
                Role = InvoicePartyRoles.Buyer,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = user,
                UpdatedAt = now,
                UpdatedBy = user,
            });
            item.InvoiceParties.Add(new InvoiceParties
            {
                Name = "Your company name",
                TaxId = "123456789",
                Street = "YourStreet",
                FullHouseNo = "1/1",
                City = "YourCity",
                PostalCode = "1234QW",
                CountryCode = "YC",
                Country = "YourCountry",
                Role = InvoicePartyRoles.Seller,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = user,
                UpdatedAt = now,
                UpdatedBy = user,
            });
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
