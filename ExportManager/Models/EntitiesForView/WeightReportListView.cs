using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class WeightReportProductListView
    {
        public string ProductName {  get; set; }
        public decimal? ProductTotalWeight { get; set; }
        public int ProductTotalAmount { get; set; }
    }
    public class WeightReportCarrierListView
    {
        public string CarrierName { get; set; }
        public int CarrierTotalAmount { get; set; }
        public int CarrierTotalShelfsAmount { get; set; }
        public decimal CarriersTotalWeight { get; set; }
    }
}
