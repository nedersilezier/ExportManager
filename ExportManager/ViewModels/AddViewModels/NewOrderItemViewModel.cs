using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Validators;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.Events;
using ExportManager.ViewModels.ShowAllViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewOrderItemViewModel : NewItemViewModel<OrderItems>
    {
        #region Fields
        private readonly int _OrderId;
        private decimal? _UnitPrice;
        private int _Quantity;
        private decimal? _StorageCost;
        private decimal? _TransportCost;
        private decimal? _Discount;
        private string _GrowerDisplayName;
        private decimal? _CostPrice;
        private int? _AvailableStock;
        private readonly int _OriginalQuantity;

        #endregion
        #region Properties
        public string GrowerDisplayName
        {
            get
            {
                return _GrowerDisplayName;
            }
            set
            {
                if (_GrowerDisplayName != value)
                    _GrowerDisplayName = value;
                OnPropertyChanged(() => GrowerDisplayName);
            }
        }
        public int? AvailableStock
        {
            get
            {
                return _AvailableStock;
            }
            set
            {
                if (_AvailableStock != value)
                    _AvailableStock = value;
                OnPropertyChanged(() => AvailableStock);
            }
        }
        public decimal? CostPrice
        {
            get
            {
                return _CostPrice;
            }
            set
            {
                if (_CostPrice != value)
                    _CostPrice = value;
                OnPropertyChanged(() => CostPrice);
            }
        }
        public int Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged(() => Quantity);
                    OnPropertyChanged(() => QuantityValidationMessage);
                    OnPropertyChanged(() => TotalProductCost);
                    OnPropertyChanged(() => TotalCost);
                    OnPropertyChanged(() => Profit);
                }
            }
        }
        public decimal? UnitPrice
        {
            get
            {
                return _UnitPrice;
            }
            set
            {
                if (_UnitPrice != value)
                {
                    _UnitPrice = value;
                    OnPropertyChanged(() => UnitPrice);
                    OnPropertyChanged(() => TotalProductCost);
                    OnPropertyChanged(() => TotalCost);
                    OnPropertyChanged(() => Profit);
                }
            }
        }
        public decimal? TransportCost
        {
            get
            {
                return _TransportCost;
            }
            set
            {
                if (_TransportCost != value)
                {
                    _TransportCost = value;
                    OnPropertyChanged(() => TransportCost);
                    OnPropertyChanged(() => TotalCost);
                }
            }
        }
        public decimal? StorageCost
        {
            get
            {
                return _StorageCost;
            }
            set
            {
                if (_StorageCost != value)
                {
                    _StorageCost = value;
                    OnPropertyChanged(() => StorageCost);
                    OnPropertyChanged(() => TotalCost);
                }
            }
        }
        public decimal? Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                if (_Discount != value)
                {
                    _Discount = value;
                    OnPropertyChanged(() => Discount);
                    OnPropertyChanged(() => TotalProductCost);
                    OnPropertyChanged(() => TotalCost);
                    OnPropertyChanged(() => Profit);
                }
            }
        }
        public decimal? TotalProductCost
        {
            get
            {
                return UnitPrice * Quantity * (1 - Discount / 100);
            }
        }
        public decimal? TotalCost
        {
            get
            {
                return TotalProductCost + TransportCost + StorageCost;
            }
        }
        public decimal? Profit
        {
            get
            {
                return TotalProductCost - (CostPrice * Quantity);
            }
        }
        public string Remarks
        {
            get
            {
                return item.Remarks;
            }
            set
            {
                if (item.Remarks != value)
                {
                    item.Remarks = value;
                    OnPropertyChanged(() => Remarks);
                }
            }
        }
        public string QuantityValidationMessage
        {
            get
            {
                if (Quantity < 1)
                    return "Quantity must be at least 1";
                if (Quantity > AvailableStock)
                    return $"Available stock: {AvailableStock}";
                return null;
            }
        }
        #endregion
        #region Constructor
        public NewOrderItemViewModel(int orderId)
            : base(new[] { "UnitPrice", "StorageCost", "TransportCost", "Discount", "Quantity" })
        {
            _OrderId = orderId;
            base.DisplayName = "New order item";
            base.FullDisplayName = new OrderDetailsQuery(potplantsEntities).GetOrderFullDisplayName(orderId);
            _OriginalQuantity = 0;
            item = new OrderItems
            {
                OrderId = _OrderId,
                IsScanned = false,
                IsActive = true,
                OrderedDate = DateTime.Now.Date
            };
        }
        public NewOrderItemViewModel(int orderId, int orderItemId)
            : base(new[] { "UnitPrice", "StorageCost", "TransportCost", "Discount", "Quantity" })
        {
            IsEditMode = true;
            _OrderId = orderId;
            item = potplantsEntities.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
            Quantity = item.Quantity;
            UnitPrice = item.UnitPrice;
            TransportCost = item.TransportCost;
            StorageCost = item.StorageCost;
            Discount = item.Discount;
            base.DisplayName = "Edit order item";
            base.FullDisplayName = new OrderDetailsQuery(potplantsEntities).GetOrderFullDisplayName(orderId);
            var stockItemDetails = new StockItemDetailsQuery(potplantsEntities).GetStockItemDetailsForNewOrderItem(item.StockItemId).FirstOrDefault();
            SelectedStockItem = new KeyAndValue
            {
                Key = item.StockItemId,
                Value = stockItemDetails.DisplayName
            };
            GrowerDisplayName = stockItemDetails.GrowerName;
            CostPrice = stockItemDetails.CostPrice;
            AvailableStock = stockItemDetails.QuantityLeft;
            _OriginalQuantity = item.Quantity;
        }
        #endregion
        #region Functions
        public override void Save()
        {
            if (SelectedStockItem.Key == 0)
            {
                throw new Exception("Please select a stock item before saving.");
            }
            int quantityDifference = Quantity - _OriginalQuantity;
            if (quantityDifference > AvailableStock)
            {
                throw new Exception($"Quantity cannot exceed available stock ({AvailableStock}).");
            }
            using (var transaction = potplantsEntities.Database.BeginTransaction())

            {
                try
                {
                    item.Quantity = Quantity;
                    item.UnitPrice = UnitPrice;
                    item.TransportCost = TransportCost;
                    item.StorageCost = StorageCost;
                    item.Discount = Discount;
                    if (!_IsEditMode)
                    {
                        item.StockItemId = SelectedStockItem.Key;
                        potplantsEntities.OrderItems.Add(item);
                    }
                    new StockItemCommand(potplantsEntities).UpdateStockItemQuantity(SelectedStockItem.Key, quantityDifference);

                    potplantsEntities.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        #endregion
        #region Item picker
        private BaseCommand _SelectStockItemCommand;
        public ICommand SelectStockItemCommand
        {
            get
            {
                if (_SelectStockItemCommand == null)
                    _SelectStockItemCommand = new BaseCommand(openSelectStockItemTab);
                return _SelectStockItemCommand;
            }

        }
        private KeyAndValue _SelectedStockItem;
        public KeyAndValue SelectedStockItem
        {
            get
            {
                if (_SelectedStockItem == null)
                    _SelectedStockItem = new KeyAndValue();
                return _SelectedStockItem;
            }
            set
            {
                if (_SelectedStockItem != value)
                    _SelectedStockItem = value;
                OnPropertyChanged(() => SelectedStockItem);
            }
        }
        public void setStockItem(SelectedItemEventArgs e)
        {
            bool stockItemExists = new OrderDetailsQuery(potplantsEntities).OrderContainsActiveItem(_OrderId, e.ItemId);
            if (stockItemExists)
                throw new Exception("Stock item already exists in this order.");
            SelectedStockItem = new KeyAndValue
            {
                Key = e.ItemId,
                Value = e.DisplayName
            };
            var stockItemDetails = new StockItemDetailsQuery(potplantsEntities).GetStockItemDetailsForNewOrderItem(e.ItemId).FirstOrDefault();
            GrowerDisplayName = stockItemDetails.GrowerName;
            CostPrice = stockItemDetails.CostPrice;
            AvailableStock = stockItemDetails.QuantityLeft;
        }
        public void openSelectStockItemTab()
        {
            OpenNewTab(() => new AllInStockViewModel(setStockItem));
        }
        #endregion
        #region  Validation 
        public override string this[string name]
        {
            get
            {
                string message = null;
                switch (name)
                {
                    case nameof(UnitPrice):
                        message = NumberValidator.IsPositive(this.UnitPrice);
                        break;
                    case nameof(StorageCost):
                        message = NumberValidator.IsPositive(this.StorageCost);
                        break;
                    case nameof(TransportCost):
                        message = NumberValidator.IsPositive(this.TransportCost);
                        break;
                    case nameof(Discount):
                        message = NumberValidator.IsPercentage(this.Discount);
                        break;
                    case nameof(Quantity):
                        message = NumberValidator.IsQuantity(this.Quantity);
                        break;
                }

                return message;
            }
        }
        public override bool IsValid()
        {
            foreach (var property in ValidatedFields)
            {
                if (this[property] != null)
                    return false;
            }
            return true;
        }
        #endregion 
    }
}
