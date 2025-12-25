using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class CultivationsForGrowers: DatabaseClass
    {
        #region Constructor
        public CultivationsForGrowers(PotplantsEntities potplantsEntities)
            :base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        //public IQueryable<KeyAndValue> GetCultivationsKeyAndValueItems()
        //{
        //    return
        //        (
        //        from cultivation in potplantsEntities.Cultivations
        //        where cultivation.IsActive == true
        //        select new KeyAndValue
        //        {
        //            Key = cultivation.CultivationId, Value = cultivation.Name
        //        }
        //        ).ToList().AsQueryable();
        //}
        public ObservableCollection<KeyAndValue> GetCultivationsKeyAndValueItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Cultivations
                .Where(t => t.IsActive==true)
                .Select(t => new KeyAndValue
                {
                    Key = t.CultivationId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
