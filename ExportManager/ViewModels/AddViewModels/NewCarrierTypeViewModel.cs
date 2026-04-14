using ExportManager.Models;
using ExportManager.Models.Validators;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewCarrierTypeViewModel: NewItemViewModel<CarrierTypes>
    {
        #region Constructor
        public NewCarrierTypeViewModel()
        : base(new[] { "MaxHeight", "Width", "Length", "Weight", "ShelfWeight" })
        {
            base.DisplayName = "New carrier type";
            item = new CarrierTypes();
        }
        public NewCarrierTypeViewModel(int carrierTypeId)
        : base(new[] { "MaxHeight", "Width", "Length", "Weight", "ShelfWeight" })
        {
            base.DisplayName = "Edit carrier type";
            _IsEditMode = true;
            item = potplantsEntities.CarrierTypes.FirstOrDefault(t => t.CarrierTypeId == carrierTypeId);
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
                if (item.Length != value)
                    item.Length = value;
                OnPropertyChanged(() => Length);
            }
        }
        public decimal? MaxHeight
        {
            get { return item.MaxHeight; }
            set
            {
                if (item.MaxHeight != value)
                    item.MaxHeight = value;
                OnPropertyChanged(() => MaxHeight);
            }
        }
        public decimal Weight
        {
            get { return item.Weight; }
            set
            {
                if(item.Weight != value)
                    item.Weight = value;
                OnPropertyChanged(() => Weight);
            }
        }
        public decimal ShelfWeight
        {
            get { return item.ShelfWeight; }
            set
            {
                if (item.ShelfWeight != value)
                    item.ShelfWeight = value;
                OnPropertyChanged(() => ShelfWeight);
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
        #region Commands
        //public override void Save()
        //{
        //    item.IsActive = true;
        //    potplantsEntities.CarrierTypes.Add(item);
        //    potplantsEntities.SaveChanges();
        //    RaiseAdded();
        //}
        #endregion
        #region  Validation 
        public override string this[string name]
        {
            get
            {
                string message = null;
                switch (name)
                {
                    //accounting for the assumption that carriers surface is at 20cm above the floor
                    case "MaxHeight":
                        message = NumberValidator.IsGreaterThan(this.MaxHeight, 20);
                        break;
                    case "Width":
                        message = NumberValidator.IsPositive(this.Width);
                        break;
                    case "Length":
                        message = NumberValidator.IsPositive(this.Length);
                        break;
                    case "Weight":
                        message = NumberValidator.IsPositive(this.Weight);
                        break;
                    case "ShelfWeight":
                        message = NumberValidator.IsPositive(this.ShelfWeight);
                        break;
                }

                return message;
            }
        }
        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(this["MaxHeight"]) && string.IsNullOrEmpty(this["Width"]) && string.IsNullOrEmpty(this["Length"]) && string.IsNullOrEmpty(this["Weight"]) && string.IsNullOrEmpty(this["ShelfWeight"]))
                return true;
            return false;
        }
        #endregion
    }
}
