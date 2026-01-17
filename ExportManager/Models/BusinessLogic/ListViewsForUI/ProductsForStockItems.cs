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
        public string GetProductDisplayNamePerId(int? id)
        {
            return potplantsEntities.Products.Where(t => t.IsActive == true && t.ProductId == id).Select(t => 
            new ProductsListView { 
                Name = t.Name, 
                Potsize = t.Potsize, 
                Height = t.Height 
            }).FirstOrDefault().ProductDisplayName;
        }
        #endregion
    }
}
