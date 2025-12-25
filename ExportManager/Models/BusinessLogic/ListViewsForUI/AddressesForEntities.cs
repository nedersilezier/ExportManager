using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class AddressesForEntities: DatabaseClass
    {
        #region Constructor
        public AddressesForEntities(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetAddressesListItems()
        {
            var addresses = new ObservableCollection<Addresses>(potplantsEntities.Addresses.Where(t=>t.IsActive==true).ToList());
            return new ObservableCollection<KeyAndValue>
                (
                    from adress in addresses
                    select new KeyAndValue
                    {
                        Key = adress.AddressId,
                        Value = adress.FullAddress
                    }
                );
            //return new ObservableCollection<KeyAndValue>
            //(
            //    potplantsEntities.Addresses
            //    .Where(t => t.IsActive == true)
            //    .Select(t => new KeyAndValue
            //    {
            //        Key = t.AddressId,
            //        Value = t.Name
            //    }).ToList()
            //);
        }
        #endregion
    }
}
