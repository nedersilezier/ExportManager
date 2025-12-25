using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class ClientsForReports: DatabaseClass
    {
        #region Constructor
        public ClientsForReports(PotplantsEntities potplantsEntities)
            :base(potplantsEntities)
        {

        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetClientsListItems()
        {
            return new ObservableCollection<KeyAndValue>(
                potplantsEntities.Clients.Where(t => t.IsActive == true).Select(t => new KeyAndValue
                {
                    Key = t.ClientId,
                    Value = t.ClientCode + " | " + t.Name
                }));
        }
        #endregion
    }
}
