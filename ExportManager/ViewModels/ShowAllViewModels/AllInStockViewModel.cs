using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Helper;
using System.Windows.Input;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.Abstract;
using static System.Net.Mime.MediaTypeNames;
using ExportManager.ViewModels.Events;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllInStockViewModel : AllViewModel<StockItemsListView>
    {
        #region Fields
        #endregion
        #region List
        public override void Load()
        {
            List = new ObservableCollection<StockItemsListView>(
                from stockitem in potplantsEntities.StockItems
                where stockitem.IsActive == true
                && stockitem.IsBlocked == false
                && stockitem.QuantityLeft > 0
                select new StockItemsListView
                {
                    StockItemId = stockitem.StockItemId,
                    ProductName = stockitem.Products.Name,
                    ProductHeight = stockitem.Products.Height,
                    Potsize = stockitem.Products.Potsize,
                    Quantity = stockitem.Quantity,
                    QuantityLeft = stockitem.QuantityLeft,
                    ExpiryDate = stockitem.ExpiryDate,
                    ReceivedAt = stockitem.ReceivedAt,
                    TrayAmount = (int)Math.Ceiling((decimal)(stockitem.Quantity / stockitem.TrayTypes.QtyPerTray)),
                    GrowerName = stockitem.Growers.Name,
                    CountryName = stockitem.Growers.Addresses.Countries.Name,
                    TrayTypeName = stockitem.TrayTypes.Name,
                    QualityName = stockitem.Qualities.Name,
                    CostPrice = stockitem.CostPrice,
                    InternalNo = stockitem.InternalNo,
                    IsBlocked = stockitem.IsBlocked,
                    IsInside = stockitem.IsInside,
                    Remarks = stockitem.Remarks
                }
                );
        }
        #endregion
        #region Constructor
        public AllInStockViewModel()
            : base()
        {
            base.DisplayName = "Stock items";
        }
        public AllInStockViewModel(Action<SelectedItemEventArgs> itemSetter)
            : base(itemSetter,
                 generateArgsFromSelection:
                 selectedItem => new SelectedItemEventArgs(selectedItem.StockItemId, selectedItem.DisplayName))
        {
            base.DisplayName = "Select the stock item";
        }
        #endregion
        #region Commands
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            return new List<CommandViewModel>
            {
            };
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            return;
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Product name", "Expiry date" };
        }
        public override void Sort()
        {
            switch (SortField)
            {
                case "Product name":
                    List = new ObservableCollection<StockItemsListView>(List.OrderBy(t => t.ProductName));
                    break;
                case "Expiry date":
                    List = new ObservableCollection<StockItemsListView>(List.OrderBy(t => t.ExpiryDate));
                    break;
            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Product name", "Grower", "Pot size" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Product name":
                    Load();
                    List = new ObservableCollection<StockItemsListView>(List.Where(t => t.ProductName != null && t.ProductName.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Grower":
                    Load();
                    List = new ObservableCollection<StockItemsListView>(List.Where(t => t.GrowerName != null && t.GrowerName.ToLower().StartsWith(FindTextBox.ToLower())));
                    break;
                case "Pot size":
                    Load();
                    int potsize;
                    if (int.TryParse(FindTextBox, out potsize))
                        List = new ObservableCollection<StockItemsListView>(List.Where(t => t.Potsize == potsize));
                    break;
            }
        }
        #endregion
    }
}
