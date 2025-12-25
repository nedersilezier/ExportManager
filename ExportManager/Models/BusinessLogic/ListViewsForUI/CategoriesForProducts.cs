using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class CategoriesForProducts: DatabaseClass
    {
        #region Constructor
        public CategoriesForProducts(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetCategoriesListItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Categories
                .Where(t => t.IsActive == true)
                .Select(t => new KeyAndValue
                {
                    Key = t.CategoryId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
