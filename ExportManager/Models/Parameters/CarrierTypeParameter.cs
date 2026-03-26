using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Parameters
{
    public class CarrierTypeParameter
    {
        public int CarrierId { get; private set; }
        public string CarrierTypeName { get; private set; }
        public string Title { get; private set; }
        public Action RefreshEvent { get; private set; }
        public CarrierTypeParameter(int carrierId, string carrierTypeName, string title, Action refreshEvent)
        {
            CarrierId = carrierId;
            CarrierTypeName = carrierTypeName;
            Title = title;
            RefreshEvent = refreshEvent;
        }
    }
}
