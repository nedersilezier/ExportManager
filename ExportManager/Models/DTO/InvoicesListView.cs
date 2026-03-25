using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.DTO
{
    public class InvoicesListView
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Status { get; set; }
        public bool IsApproved { get; set; }
        public string IsApprovedString
        {
            get { return IsApproved ? "Yes" : "No"; }
        }
        public decimal TotalAmount { get; set; }
        public decimal TotalGross { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalStorageCost { get; set; }
        public decimal TotalTransportCost { get; set; }
        public string PaymentMethod { get; set; }

        public string ClientName { get; set; }
        public string ClientTaxId { get; set; }
        public string ClientCity { get; set; }
        public string ClientStreet { get; set; }
        public string ClientPostalCode { get; set; }
        public string ClientCountry { get; set; }
        public string ClientCountryCode { get; set; }

        public int TotalQuantity { get; set; }
        public decimal TotalNetAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public string Remarks { get; set; }
    }
}
