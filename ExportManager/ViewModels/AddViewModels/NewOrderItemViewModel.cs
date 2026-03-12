using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
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
    public class NewOrderItemViewModel: NewItemViewModel<OrderItems>
    {
        #region Fields
        private readonly int _OrderId;
        #endregion
        #region Properties
        #endregion
        #region Constructor
        public NewOrderItemViewModel(int orderId)
            : base(new[] {""} )
        {
            _OrderId = orderId;
            base.DisplayName = "New order item";
            base.FullDisplayName = new OrderDetailsQuery(potplantsEntities).GetOrderFullDisplayName(orderId);
            item = new OrderItems
            {
                OrderId = _OrderId,
                IsScanned = false
            };
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
                if(_SelectedStockItem != value)
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
        }
        public void openSelectStockItemTab()
        {
            OpenNewTab(() => new AllInStockViewModel(setStockItem));
        }
        #endregion
    }
}
