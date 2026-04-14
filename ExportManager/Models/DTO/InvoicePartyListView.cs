using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.DTO
{
    public class InvoicePartyListView
    {
        public int InvoicePartyId { get; set; }
        public int InvoiceId { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string City { get; set; }
        public string FullHouseNumber { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Role { get; set; }
    }
}
