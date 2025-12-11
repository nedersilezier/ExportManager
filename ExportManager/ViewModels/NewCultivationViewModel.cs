using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;

namespace ExportManager.ViewModels
{
    public class NewCultivationViewModel: NewItemViewModel<Cultivations>
    {
        #region Constructor
        public NewCultivationViewModel()
            :base()
        {
            base.DisplayName = "New cultivation type";
            item = new Cultivations();
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return item.Name; }
            set
            {
                if(item.Name != value)
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
        //    potplantsEntities.Cultivations.Add(item);
        //    potplantsEntities.SaveChanges();
        //    RaiseAdded();
        //}
        #endregion
    }
}
