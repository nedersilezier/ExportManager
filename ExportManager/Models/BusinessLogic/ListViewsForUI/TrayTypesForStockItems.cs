using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI    
{
    public class TrayTypesForStockItems:DatabaseClass
    {
        #region Constructor
        public TrayTypesForStockItems(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        private IQueryable<TrayTypes> GetTrayTypesQuery()
        {
            return potplantsEntities.TrayTypes.Where(t => t.IsActive == true);
        }
        public ObservableCollection<KeyAndValue> GetCompatibleTrayTypes(int productId)
        {
            var potsize = potplantsEntities.Products
                .Where(p => p.ProductId == productId)
                .Select(p => p.Potsize)
                .FirstOrDefault();
            return new ObservableCollection<KeyAndValue>(
                GetTrayTypesQuery().Where(t => t.FittingPotSize >= potsize).Select(t => new KeyAndValue
                {
                    Key = t.TrayTypeId,
                    Value = t.Name
                }
                ).ToList());
        }
        public ObservableCollection<KeyAndValue> GetTrayTypesListItems()
        {
            return new ObservableCollection<KeyAndValue>(
                from traytype in potplantsEntities.TrayTypes
                where traytype.IsActive == true
                select new KeyAndValue
                {
                    Key = traytype.TrayTypeId,
                    Value = traytype.Name
                }
                );
        }
        #endregion
    }
}
