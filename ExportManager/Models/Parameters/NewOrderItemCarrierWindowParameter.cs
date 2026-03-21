using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Parameters
{
    public class NewOrderItemCarrierWindowParameter
    {
        public int OrderId { get; private set; }
        public string Title { get; private set; }
        public NewOrderItemCarrierWindowParameter(int orderId, string title)
        {
            OrderId = orderId;
            Title = title;
        }
    }
}
