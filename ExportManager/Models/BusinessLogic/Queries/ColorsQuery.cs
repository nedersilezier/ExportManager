using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class ColorsQuery: DatabaseClass
    {
        #region Constructor
        public ColorsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetColorsListItems()
        {
            return new ObservableCollection<KeyAndValue>
            (
                potplantsEntities.Colors
                .Where(t => t.IsActive == true)
                .Select(t => new KeyAndValue
                {
                    Key = t.ColorId,
                    Value = t.Name
                }).ToList()
            );
        }
        #endregion
    }
}
