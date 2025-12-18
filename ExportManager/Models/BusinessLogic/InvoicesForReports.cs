using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class InvoicesForReports: DatabaseClass
    {
        #region Constructor
        public InvoicesForReports(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {

        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetInvoicesListItemsPerDate(DateTime date)
        {
            DateTime dateStart = date.Date;
            DateTime dateEnd = dateStart.Date.AddDays(1);
            return new ObservableCollection<KeyAndValue>(
                potplantsEntities.Invoices.Where(t => t.IsActive == true && t.InvoiceDate >= dateStart && t.InvoiceDate <= dateEnd)
                .Select(t => new KeyAndValue
                {
                    Key = t.InvoiceId,
                    Value = t.Orders.Clients.ClientCode + " | " + t.Orders.Clients.Name
                }));
        }
        #endregion
    }
}
