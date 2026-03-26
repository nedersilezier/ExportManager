using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class CountriesQuery: DatabaseClass
    {
        #region Constructor
        public CountriesQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetCountriesListItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Countries
                .Where(t => t.IsActive == true)
                .Select(t => new KeyAndValue
                {
                    Key = t.CountryId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
