using ExportManager.Models.DTO;
using ExportManager.Models.EntitiesForView;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Parameters
{
    public class InvoiceParameter
    {
        //public int InvoiceId { get; private set; }
        public InvoicesListView Invoice { get; private set; }
        public List<InvoiceItemsListView> InvoiceItems { get; private set; }
        public string Title { get; private set; }
        public Action RefreshEvent { get; private set; }
        public InvoiceParameter(InvoicesListView invoice, Action refreshEvent, List<InvoiceItemsListView> invoiceItems = null)
        {
            //InvoiceId = invoiceId;
            Invoice = invoice;
            InvoiceItems = invoiceItems;
            Title = $"Invoice Details - Client: {Invoice.Buyer.Name}";
            RefreshEvent = refreshEvent;
        }
    }
}
