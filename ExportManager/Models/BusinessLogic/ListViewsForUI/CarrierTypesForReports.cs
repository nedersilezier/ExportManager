using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class CarrierTypesForReports: DatabaseClass
    {
        #region Constructor
        public CarrierTypesForReports(PotplantsEntities potplantsEntities)
            :base(potplantsEntities)
        {

        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetCarrierTypesListItems()
        {
            return new ObservableCollection<KeyAndValue>(potplantsEntities.CarrierTypes.Where(t => t.IsActive == true).Select(t =>
                new KeyAndValue
                {
                    Key = t.CarrierTypeId,
                    Value = t.Name
                }
                ));
        }
        #endregion
    }
}
