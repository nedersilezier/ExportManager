using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class GrowerDetailsQuery: DatabaseClass
    {
        #region Constructor
        public GrowerDetailsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<Growers> GetGrowerById(int growerId)
        {
            return potplantsEntities.Growers.Where(g => g.IsActive && g.GrowerId == growerId);
        }
        public string GetGrowerDisplayNameById(int growerId)
        {
            return GetGrowerById(growerId).Select(g => new GrowersListView
            {
                GrowerName = g.Name,
                Country = g.Addresses.Countries.Name
            }).FirstOrDefault().DisplayName;
        }
        #endregion
    }
}
