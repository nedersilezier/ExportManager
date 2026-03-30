using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class InvoiceItemsListView
    {
        public int ItemNo {  get; set; }
        public string Name {  get; set; }
        public decimal? Potsize { get; set; }
        public decimal? Height { get; set; }
        public int Quantity {  get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public int? SourceOrderItemId { get; set; }
        public string FullName
        {
            get
            {
                var details = new List<string>();

                if (Potsize != null)
                    details.Add($"Pot: {Potsize.Value} cm");

                if (Height != null)
                    details.Add($"H: {Height.Value} cm");

                return details.Count > 0 ? $"{Name} ({string.Join(", ", details)})" : Name;
            }
        }
    }
}
