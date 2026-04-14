using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.Queries
{
    public class PaymentMethodsQuery : DatabaseClass
    {
        #region Constructor
        public PaymentMethodsQuery(PotplantsEntities potplantsEntities) : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<PaymentMethods> GetPaymentMethods()
        {
            return potplantsEntities.PaymentMethods.Where(x => x.IsActive == true);
        }
        public List<KeyAndValue> GetPaymentMethodsForCombobox()
        {
            return GetPaymentMethods().Select(pm => new KeyAndValue
                {
                    Key = pm.PaymentMethodId,
                    Value = pm.Name
                }).ToList();
        }
        #endregion
    }
}
