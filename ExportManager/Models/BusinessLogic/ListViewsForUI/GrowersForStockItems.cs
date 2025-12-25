using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class GrowersForStockItems: DatabaseClass
    {
        #region Constructor
        public GrowersForStockItems(PotplantsEntities potplantsEntities)
            :base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetGrowersListItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Growers
                .Where(t => t.IsActive == true)
                .Select(t => new KeyAndValue
                {
                    Key = t.GrowerId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
