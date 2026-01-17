using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllOrderItemsViewModel : AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(potplantsEntities.OrderItems.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Constructor
        public AllOrderItemsViewModel()
            :base()
        {
            base.DisplayName = "Order items";
        }
        #endregion

        #region Functions
        public override void OnAdd()
        {
            return;
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Not implemented yet" };
        }
        public override void Sort()
        {
            return;
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Not implemented yet" };
        }
        public override void Find()
        {
            return;
        }
        #endregion
    }
}
