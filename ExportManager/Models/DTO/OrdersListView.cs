using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class OrdersListView
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PreparationDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Country { get; set; }
        public string SalesPerson { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

    }
}
