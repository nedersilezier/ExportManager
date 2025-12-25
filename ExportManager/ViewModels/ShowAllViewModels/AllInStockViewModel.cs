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

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllInStockViewModel : AllViewModel<dynamic>
    {
        #region Fields
        #endregion
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(
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
    }
}
