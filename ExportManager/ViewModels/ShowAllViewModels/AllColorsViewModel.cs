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
    public class AllColorsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            using(var shortLivedPotplantsEntities = new PotplantsEntities())
            {
                List = new ObservableCollection<dynamic>(shortLivedPotplantsEntities.Colors.Where(t => t.IsActive == true).ToList());
            }
        }
        #endregion
        #region Constructor
        public AllColorsViewModel()
            :base()
        {
            base.DisplayName = "Colors";
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewColorViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewColorViewModel(SelectedItem.ColorId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Colors>(SelectedItem.ColorId);
        }
        #endregion
    }
}
