using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class StockItemCommand : DatabaseClass
    {
        #region Constructor
        public StockItemCommand(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public void UpdateStockItemQuantity(int stockItemId, int quantity)
        {
            var stockItem = potplantsEntities.StockItems.FirstOrDefault(s => s.StockItemId == stockItemId);
            if (stockItem == null)
                throw new Exception($"Stock item with ID {stockItemId} not found.");
            if (quantity > stockItem.QuantityLeft)
                throw new Exception($"Insufficient stock quantity. Available: {stockItem.QuantityLeft}, Requested: {quantity}.");
            stockItem.QuantityLeft -= quantity;
        }
        #endregion
    }
}
