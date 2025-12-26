using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllTrayTypesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using(var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<dynamic>(shortLivedPotplantsEntities.TrayTypes.Where(t => t.IsActive == true).ToList());
            }
        }
        #endregion
        #region Constructor
        public AllTrayTypesViewModel()
            : base()
        {
            base.DisplayName = "Tray types";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewTrayTypeViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewTrayTypeViewModel(SelectedItem.TrayTypeId), Load);
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
    }
}
