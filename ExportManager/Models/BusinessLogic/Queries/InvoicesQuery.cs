using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class InvoicesQuery : DatabaseClass
    {
        #region Constructor
        public InvoicesQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {

        }
        #endregion
        #region Functions
        public IQueryable<Invoices> GetInvoices()
        {
            return potplantsEntities.Invoices;
        }
        public IQueryable<Invoices> GetActiveInvoices()
        {
            return potplantsEntities.Invoices.Where(i => i.IsActive);
        }
        public IQueryable<Invoices> GetInvoicesPerClient(int clientId)
        {
            return GetInvoices().Where(i => i.InvoiceItems.Any(ii => ii.OrderItems.Orders.Clients.ClientId == clientId));
        }
        public int CountInvoicesPerClientPerYear(int clientId, int year)
        {
            return GetInvoicesPerClient(clientId).Count(i => i.InvoiceDate.Year == year);
        }
        public ObservableCollection<KeyAndValue> GetInvoicesListItemsPerDate(DateTime date)
        {
            DateTime dateStart = date.Date;
            DateTime dateEnd = dateStart.Date.AddDays(1);
            var query = potplantsEntities.Invoices
                        .Where(i => i.IsActive && i.InvoiceDate >= dateStart && i.InvoiceDate < dateEnd)
                        .Select(i => new
                        {
                            i.InvoiceId,
                            Client = i.InvoiceItems.Where(ii => ii.SourceOrderItemId.HasValue).Select(ii => ii.OrderItems.Orders.Clients).FirstOrDefault()
                        }).Where(x => x.Client != null).Select(x => new KeyAndValue
                        {
                            Key = x.InvoiceId,
                            Value = x.Client.ClientCode + " | " + x.Client.Name
                        });

            return new ObservableCollection<KeyAndValue>(query);
        }
        public HashSet<DateTime> GetInvoiceDates()
        {
            return GetActiveInvoices().Select(i =>  i.InvoiceDate).ToList().Select(d => d.Date).Distinct().ToHashSet();
        }
        #endregion
    }
}
