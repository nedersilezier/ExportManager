using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewTrayTypeViewModel : NewItemViewModel<TrayTypes>
    {
        #region Constructor
        public NewTrayTypeViewModel()
            : base()
        {
            base.DisplayName = "New tray type";
            item = new TrayTypes();
        }
        #endregion
        #region Properties
        public string Name
        {
            get {  return item.Name; }
            set
            {
                if(item.Name != value)
                    item.Name = value;
                OnPropertyChanged(() => Name);
            }
        }
        public decimal? Width
        {
            get { return item.Width; }
            set
            {
                if (item.Width != value)
                    item.Width = value;
                OnPropertyChanged(() => Width);
            }
        }
        public decimal? Length
        {
            get { return item.Length; }
            set
            {
                if(item.Length != value)
                    item.Length = value;
                OnPropertyChanged(() => Length);
            }
        }
        public decimal? FittingPotSize
        {
            get { return item.FittingPotSize; }
            set
            {
                if (item.FittingPotSize != value)
                    item.FittingPotSize = value;
                OnPropertyChanged(() => FittingPotSize);
            }

        }
        public int? QtyPerTray
        {
            get { return item.QtyPerTray; }
            set
            {
                if (item.QtyPerTray != value)
                    item.QtyPerTray = value;
                OnPropertyChanged(() => QtyPerTray);
            }
        }
        public string Remarks
        {
            get { return item.Remarks; }
            set
            {
                if(item.Remarks != value)
                    item.Remarks = value;
                OnPropertyChanged(() => Remarks);
            }
        }
        #endregion
    }
}
