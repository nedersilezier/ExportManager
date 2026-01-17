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
            SoftDelete<CarrierTypes>(SelectedItem.CarrierTypeId);
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Name", "Width", "Length" };
        }
        public override void Sort()
        {
            switch(SortField)
            {
                case "Name":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Name));
                    break;
                case "Width":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Width));
                    break;
                case "Length":
                    List = new ObservableCollection<dynamic>(List.OrderBy(t => t.Length));
                    break;
            }
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Name", "Width", "Length" };
        }
        public override void Find()
        {
            switch (FindField)
            {
                case "Name":
                    Load();
                    List = new ObservableCollection<dynamic>(List.Where(t => t.Name != null && t.Name.ToLower().Contains(FindTextBox.ToLower())));
                    break;
                case "Width":
                    decimal width;
                    bool isDecimal = decimal.TryParse(FindTextBox, out width);
                    Load();
                    if (isDecimal)
                    {
                        List = new ObservableCollection<dynamic>(List.Where(t => t.Width == width));
                    }
                    break;
                case "Length":
                    decimal length;
                    bool isDecimalLength = decimal.TryParse(FindTextBox, out length);
                    Load();
                    if (isDecimalLength)
                    {
                        List = new ObservableCollection<dynamic>(List.Where(t => t.Length == length));
                    }
                    break;
            }
        }
        #endregion
    }
}
