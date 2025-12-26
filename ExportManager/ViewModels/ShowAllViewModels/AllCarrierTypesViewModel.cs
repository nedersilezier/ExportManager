using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;
namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllCarrierTypesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using(var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<dynamic>(shortLivedPotplantsEntities.CarrierTypes.Where(t => t.IsActive == true).ToList());
            }    
        }
        #endregion
        #region Constructor
        public AllCarrierTypesViewModel()
        : base()
        {
            base.DisplayName = "Carrier types";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewCarrierTypeViewModel(), Load);
        }
        public override void OnEdit()
        {
            if (SelectedItem == null)
                return;
            OpenNewTab(() => new NewCarrierTypeViewModel(SelectedItem.CarrierTypeId), Load);
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
    }
}
