using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class StockItemDetailsQuery : DatabaseClass
    {
        #region Constructor
        public StockItemDetailsQuery(PotplantsEntities potplantsEntities)
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
        #endregion
    }
}
