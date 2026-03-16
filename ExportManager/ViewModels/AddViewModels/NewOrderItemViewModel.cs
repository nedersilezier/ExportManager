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
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewOrderItemViewModel : NewItemViewModel<OrderItems>
    {
        #region Fields
        private readonly int _OrderId;
        private string _GrowerDisplayName;
        private decimal? _CostPrice;
        private string _UnitPriceString;
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
                return item.Quantity;
            }
            set
            {
                if (item.Quantity != value)
                {
                    item.Quantity = value;
                    OnPropertyChanged(() => Quantity);
                }
            }
        }
        public decimal UnitPrice
        {
            get
            {
                return item.UnitPrice;
            }
            set
            {
                if (item.UnitPrice != value)
                {
                    item.UnitPrice = value;
                    OnPropertyChanged(() => UnitPrice);
                }
            }
        }
        public decimal TransportCost
        {
            get
            {
                return item.TransportCost;
            }
            set
            {
                if (item.TransportCost != value)
                {
                    item.TransportCost = value;
                    OnPropertyChanged(() => TransportCost);
                }
            }
        }
        public decimal StorageCost
        {
            get
            {
                return item.StorageCost;
            }
            set
            {
                if (item.StorageCost != value)
                {
                    item.StorageCost = value;
                    OnPropertyChanged(() => StorageCost);
                }
            }
        }
        public decimal Discount
        {
            get
            {
                return item.Discount;
            }
            set
            {
                if (item.Discount != value)
                {
                    item.Discount = value;
                    OnPropertyChanged(() => Discount);
                }
            }
        }
        public decimal TotalUnitCost
        {
            get
            {
                return Quantity * UnitPrice * (1 - Discount / 100);
            }
        }
        public decimal TotalCost
        {
            get
            {
                return TotalUnitCost + TransportCost + StorageCost;
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
        #endregion
        #region Constructor
        public NewOrderItemViewModel(int orderId)
            : base(new[] { "UnitPrice" })
        {
            _OrderId = orderId;
            base.DisplayName = "New order item";
            base.FullDisplayName = new OrderDetailsQuery(potplantsEntities).GetOrderFullDisplayName(orderId);
            item = new OrderItems
            {
                OrderId = _OrderId,
                IsScanned = false,
                IsActive = true,
                OrderedDate = DateTime.Now.Date
            };
        }
        #endregion
        #region Functions
        public override void Save()
        {   
            if(SelectedStockItem.Key == 0)
            {
                throw new Exception("Please select a stock item before saving.");
            }
            if (!_IsEditMode)
            {
                item.StockItemId = SelectedStockItem.Key;
                potplantsEntities.OrderItems.Add(item);
            }
            potplantsEntities.SaveChanges();
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
            SelectedStockItem = new KeyAndValue
            {
                Key = e.ItemId,
                Value = e.DisplayName
            };
            GrowerDisplayName = new StockItemDetailsQuery(potplantsEntities).GetGrowerDisplayNameByStockItemId(e.ItemId);
            CostPrice = new StockItemDetailsQuery(potplantsEntities).GetStockItemCostPriceById(e.ItemId);
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
                    case "UnitPrice":
                        message = NumberValidator.IsPositive(this.UnitPrice);
                        break;
                    case "StorageCost":
                        message = NumberValidator.IsPositive(this.UnitPrice);
                        break;
                    case "TransportCost":
                        message = NumberValidator.IsPositive(this.UnitPrice);
                        break;
                    case "Discount":
                        message = NumberValidator.IsPercentage(this.UnitPrice);
                        break;
                }

                return message;
            }
        }
        public override bool IsValid()
        {
            if (this["UnitPrice"] == null)
                return true;
            return false;
        }
        #endregion 
    }
}
