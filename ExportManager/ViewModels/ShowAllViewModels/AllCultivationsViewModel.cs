using ExportManager.Models;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    internal class AllCultivationsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<dynamic>(shortLivedPotplantsEntities.Cultivations.Where(t => t.IsActive == true).ToList());
            }
        }
        #endregion
        #region Constructor
        public AllCultivationsViewModel()
            :base()
        {
            base.DisplayName = "Cultivations";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewCultivationViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewCultivationViewModel(SelectedItem.CultivationId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Cultivations>(SelectedItem.CultivationId);
        }
        #endregion
    }
}
