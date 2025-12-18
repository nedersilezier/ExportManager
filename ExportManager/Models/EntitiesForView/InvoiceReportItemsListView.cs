using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class InvoiceReportItemsListView
    {
        public int ItemNo {  get; set; }
        public string Name {  get; set; }
        public decimal? Potsize { get; set; }
        public decimal? Height { get; set; }
        public int Quantity {  get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public string FullName
        {
            get
            {
                return Name 
                    + (Potsize != null ? " d" + Potsize.Value.ToString("G29") + " " : string.Empty) 
                    + (Height != null ? " h" + Height.Value.ToString("G29") : string.Empty);
            }
        }
    }
}
