using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace ExportManager.Models.EntitiesForView
{
    public class OrderItemsListView
    {
        public int OrderItemId { get; set; }
        public int StockItemId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductHeight { get; set; }
        public decimal? ProductPotsize { get; set; }
        public string FullProductName
        {
            get
            {
                var details = new List<string>();

                if (ProductPotsize != null)
                    details.Add($"Pot: {ProductPotsize.Value} cm");

                if (ProductHeight != null)
                    details.Add($"H: {ProductHeight.Value} cm");

                return details.Count > 0 ? $"{ProductName} ({string.Join(", ", details)})" : ProductName;
            }
        }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TransportCost { get; set; }
        public decimal? StorageCost { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TotalPriceString
        {
            get
            {
                return TotalPrice.HasValue ? TotalPrice.Value.ToString("C") : "N/A";
            }
        }
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
