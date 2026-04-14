using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExportManager.Models.BusinessLogic.Commands
{
    public class OrdersCommand : DatabaseClass
    {
        #region Constructor
        public OrdersCommand(PotplantsEntities potplantsEntities) : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = potplantsEntities.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if(order == null)
            {
                MessageBox.Show($"Order with ID {orderId} not found.", "Error");
                return;
            }
            order.Status = status;
        }
        #endregion
    }
}
