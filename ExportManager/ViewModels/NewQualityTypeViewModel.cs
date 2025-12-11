using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;
using ExportManager.ViewModels.Abstract;

namespace ExportManager.ViewModels
{
    public class NewQualityTypeViewModel: NewItemViewModel<Qualities>
    {
        #region Constructor
        public NewQualityTypeViewModel()
            :base()
        {
            base.DisplayName = "New quality type";
            item = new Qualities();
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return item.Name; }
            set
            {
                if (item.Name != value)
                    item.Name = value;
                OnPropertyChanged(() => Name);
            }
        }
        public string Description
        {
            get { return item.Description; }
            set
            {
                if (item.Description != value)
                    item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }
        #endregion
        #region Commands
        //public override void Save()
        //{
        //    item.IsActive = true;
        //    potplantsEntities.Qualities.Add(item);
        //    potplantsEntities.SaveChanges();
        //    RaiseAdded();
        //}
        #endregion
    }
}
