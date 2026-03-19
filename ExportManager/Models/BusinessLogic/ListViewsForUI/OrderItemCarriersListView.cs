using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class OrderItemCarriersListView
    {
        public int CarrierId { get; set; }
        public int OrderId { get; set; }
        public List<int> OrderItemIds { get; set; }
        public string CarrierType { get; set; }
        public int AmountOfShelves { get; set; }
        public int AmountOfExtensions { get; set; }
        public int AmountOfBatches 
        {
            get
            {
                return OrderItemIds.Count();
            }
        }
        public int AmountOfPlants { get; set; }
    }
}
