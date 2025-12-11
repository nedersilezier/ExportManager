using ExportManager.Models;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels
{
    public class NewColorViewModel: NewItemViewModel<Colors>
    {
        #region Constructor
        public NewColorViewModel()
            : base()
        {
            base.DisplayName = "New color";
            item = new Colors();
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return item.Name; }
            set
            {
                if(item.Name != value)
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
        //    potplantsEntities.Colors.Add(item);
        //    potplantsEntities.SaveChanges();
        //}
        #endregion
    }
}
