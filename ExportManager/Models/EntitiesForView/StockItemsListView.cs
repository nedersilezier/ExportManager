using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class StockItemsListView
    {
        public int StockItemId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductHeight { get; set; }
        public decimal? Potsize { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityLeft { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public int TrayAmount { get; set; }
        public string GrowerName { get; set; }
        public string CountryName { get; set; }
        public string TrayTypeName { get; set; }
        public string QualityName { get; set; }
        public decimal? CostPrice { get; set; }
        public string InternalNo { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsInside { get; set; }
        public string Remarks { get; set; }

    }
}
