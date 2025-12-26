using ExportManager.Models;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewCategoryViewModel: NewItemViewModel<Categories>
    {
        #region Constructor
        public NewCategoryViewModel()
            : base()
        {
            base.DisplayName = "New category";
            item = new Categories();
        }
        public NewCategoryViewModel(int categoryId)
            : base()
        {
            base.DisplayName = "Edit category";
            _IsEditMode = true;
            item = potplantsEntities.Categories.FirstOrDefault(t => t.CategoryId == categoryId);
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return item.Name; }
            set
            {
                if (item.Name != value)
                {
                    item.Name = value;
                }
                OnPropertyChanged(() => Name);
            }
        }
        public string Remarks
        {
            get { return item.Remarks; }
            set
            {
                if (item.Remarks != value)
                {
                    item.Remarks = value;
                }
                OnPropertyChanged(() => Name);
            }
        }
        #endregion
        #region Commands
        //public override void Save()
        //{
        //    item.IsActive = true;
        //    potplantsEntities.Categories.Add(item);
        //    potplantsEntities.SaveChanges();
        //    //CategoryAdded?.Invoke();
        //}
        #endregion
    }
}
