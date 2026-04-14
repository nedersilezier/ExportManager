using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Extensions
{
    public class InvoiceStatuses
    {
        public const string Draft = "Draft";
        public const string Issued = "Issued";
        public const string Paid = "Paid";
        public const string Canceled = "Canceled";
        public const string Overdue = "Overdue";
    }
}
