using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class QualitiesQuery: DatabaseClass
    {
        #region Constructor
        public QualitiesQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetQualitiesListItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Qualities
                .Where(t => t.IsActive == true)
                .Select(t => new KeyAndValue
                {
                    Key = t.QualityId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
