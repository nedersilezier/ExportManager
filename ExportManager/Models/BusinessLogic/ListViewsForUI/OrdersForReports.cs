using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class OrdersForReports: DatabaseClass
    {
        #region Constructor
        public OrdersForReports(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {

        }
        #endregion
        #region Functions
        public ObservableCollection<KeyAndValue> GetOrdersListItems()
        {
            return new ObservableCollection<KeyAndValue>(
                potplantsEntities.Orders.Where(t => t.IsActive == true).Select(t => new KeyAndValue
                {
                    Key = t.OrderId,
                    Value = t.Clients.ClientCode + " | " + t.Clients.Name
                }));
        }
        public ObservableCollection<KeyAndValue> GetOrdersListItemsPerDate(DateTime date)
        {
            DateTime dateStart = date.Date;
            DateTime dateEnd = dateStart.Date.AddDays(1);
            return new ObservableCollection<KeyAndValue>(
                potplantsEntities.Orders.Where(t => t.IsActive == true && t.PreparationDate >= dateStart && t.PreparationDate <= dateEnd).Select(t => new KeyAndValue
                {
                    Key = t.OrderId,
                    Value = t.Clients.ClientCode + " | " + t.Clients.Name
                }));
        }
        #endregion
    }
}
