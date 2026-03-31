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
        public int InvoiceId { get; private set; }
        public string Title { get; private set; }
        public Action RefreshEvent { get; private set; }
        public InvoiceParameter(int invoiceId, Action refreshEvent)
        {
            InvoiceId = invoiceId;
            Title = $"Invoice Details - ID: {invoiceId}";
            RefreshEvent = refreshEvent;
        }
    }
}
