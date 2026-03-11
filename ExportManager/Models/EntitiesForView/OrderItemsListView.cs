using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class OrderItemsListView
    {
        public int OrderItemId { get; set; }
        public int StockItemId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProtuctHeight { get; set; }
        public decimal? ProductPotsize { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal StorageCost { get; set; }
        public decimal TransportCost { get; set; }
        public decimal Discount { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Remarks { get; set; }
        public int InternalNo { get; set; }
        public string ProductInternalNo { get; set; }
        public string GrowerName { get; set; }
        public string TrayType { get; set; }
        public string Quality { get; set; }

        public DateTime? OrderedDate { get; set; }
        public bool IsScanned { get; set; }
        public string IsScannedString
        {
            get
            {
                return IsScanned ? "Yes" : "No";
            }
        }
    }
}
