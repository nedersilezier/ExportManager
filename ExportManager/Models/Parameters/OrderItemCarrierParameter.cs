using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Parameters
{
    public class OrderItemCarrierParameter
    {
        public int CarrierId { get; private set; }
        public string Title { get; private set; }
        public int Shelves { get; private set; }
        public int Extensions { get; private set; }
        public Action RefreshEvent { get; private set; }
        public OrderItemCarrierParameter(int orderId, string title, int shelves, int extensions, Action refreshEvent)
        {
            CarrierId = orderId;
            Title = title;
            Shelves = shelves;
            Extensions = extensions;
            RefreshEvent = refreshEvent;
        }
    }
}
