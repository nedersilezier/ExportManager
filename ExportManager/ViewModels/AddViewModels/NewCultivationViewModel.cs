using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;

namespace ExportManager.ViewModels.AddViewModels
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
        public NewCultivationViewModel(int cultivationId)
            : base()
        {
            base.DisplayName = "Edit cultivation type";
            _IsEditMode = true;
            item = potplantsEntities.Cultivations.FirstOrDefault(t => t.CultivationId == cultivationId);
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
