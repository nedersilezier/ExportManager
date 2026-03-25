using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class StockItemsQuery : DatabaseClass
    {
        #region Constructor
        public StockItemsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<StockItems> GetStockItemById(int stockItemId)
        {
            return potplantsEntities.StockItems.Where(s => s.IsActive && s.StockItemId == stockItemId);
        }
        public string GetGrowerDisplayNameByStockItemId(int stockItemId)
        {
            return GetStockItemById(stockItemId).Select(s => new GrowersListView
            {
                GrowerName = s.Growers.Name,
                Country = s.Growers.Addresses.Countries.Name
            }).FirstOrDefault()?.DisplayName;
        }
        public decimal? GetStockItemCostPriceById(int stockItemId)
        {
                       return GetStockItemById(stockItemId).Select(s => s.CostPrice).FirstOrDefault();

        }
        public IQueryable<StockItemsListView> GetStockItemDetailsForNewOrderItem(int stockItemId)
        {
            return GetStockItemById(stockItemId).Select(s => new StockItemsListView
            {
                StockItemId = s.StockItemId,
                ProductName = s.Products.Name,
                Potsize = s.Products.Potsize,
                ProductHeight = s.Products.Height,
                GrowerName = s.Growers.Name,
                CostPrice = s.CostPrice,
                QuantityLeft = s.QuantityLeft
            });
        }
        #endregion
    }
}
