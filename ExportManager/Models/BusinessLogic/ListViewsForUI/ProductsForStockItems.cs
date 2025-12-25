using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class ProductsForStockItems: DatabaseClass
    {
        #region Constructor
        public ProductsForStockItems(PotplantsEntities potplantsEntities)
            :base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetProductsListItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Products
                .Where(t => t.IsActive == true)
                .Select(t => new KeyAndValue
                {
                    Key = t.ProductId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
