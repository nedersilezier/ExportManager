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
    public class AllQualitiesViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using (var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<dynamic>(shortLivedPotplantsEntities.Qualities.Where(t => t.IsActive == true));
            }
        }
        #endregion
        #region Constructor
        public AllQualitiesViewModel()
            :base()
        {
            base.DisplayName = "Quality types";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewQualityTypeViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewQualityTypeViewModel(SelectedItem.QualityId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Qualities>(SelectedItem.QualityId);
        }
        #endregion
    }
}
