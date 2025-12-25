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
using ExportManager.ViewModels.AddViewModels;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllBatchesViewModel: AllViewModel<dynamic>
    {
        #region Fields
        private BaseCommand _UnblockCommand;
        private BaseCommand _BlockCommand;
        #endregion
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(
                from stockitem in potplantsEntities.StockItems
                where stockitem.IsActive == true
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
        public AllBatchesViewModel()
            : base()
        {
            base.DisplayName = "Batches";
        }
        #endregion
        #region Commands
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Unblock", UnblockCommand),
                new CommandViewModel("Block", BlockCommand)
            };
        }
        public ICommand UnblockCommand
        {
            get
            {
                if (_UnblockCommand == null)
                    _UnblockCommand = new BaseCommand(Unblock);
                return _UnblockCommand;
            }
        }
        public ICommand BlockCommand
        {
            get
            {
                if (_BlockCommand == null)
                    _BlockCommand = new BaseCommand(Block);
                return _BlockCommand;
            }
        }
        #endregion

        #region Functions
        public override void OnAdd()
        {
            AddNew<NewBatchViewModel>();
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        public void Unblock()
        {
            if (SelectedItem != null && SelectedItem is StockItemsListView)
            {
                int stockItemId = SelectedItem.StockItemId;
                var stockItem = potplantsEntities.StockItems.FirstOrDefault(t => t.StockItemId == stockItemId);
                stockItem.IsBlocked = false;
                potplantsEntities.SaveChanges();
                Load();
            }
        }
        public void Block()
        {
            if (SelectedItem != null && SelectedItem is StockItemsListView)
            {
                int stockItemId = SelectedItem.StockItemId;
                var stockItem = potplantsEntities.StockItems.FirstOrDefault(t => t.StockItemId == stockItemId);
                stockItem.IsBlocked = true;
                potplantsEntities.SaveChanges();
                Load();
            }
        }
        #endregion
    }
}
