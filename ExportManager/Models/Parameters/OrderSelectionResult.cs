using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Parameters
{
    public class OrderSelectionResult
    {
        public int OrderId { get; private set; }
        public int ClientId { get; private set; }
        public string ClientCode { get; private set; }
        public string ClientName { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public OrderSelectionResult(int orderId, int clientId, string clientCode, string clientName, DateTime orderDate, DateTime deliveryDate)
        {
            OrderId = orderId;
            ClientId = clientId;
            ClientCode = clientCode;
            ClientName = clientName;
            OrderDate = orderDate;
            DeliveryDate = deliveryDate;
        }
    }
}
