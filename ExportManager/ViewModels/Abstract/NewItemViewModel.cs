using ExportManager.Helper;
using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExportManager.ViewModels;
using System.Data.Entity.Validation;
using System.Windows;
using System.Collections.ObjectModel;

namespace ExportManager.ViewModels.Abstract
{
    public abstract class NewItemViewModel<T> : NewItemViewModelBase where T : class, IHasIsActive, new()
    {
        #region Fields
        protected bool _IsEditMode = false;
        protected string errorMessage = null;
        #endregion
        #region Properties
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set
            {
                if (_IsEditMode != value)
                {
                    _IsEditMode = value;
                    OnPropertyChanged(() => IsEditMode);
                }
            }
        }
        #endregion
        #region Database
        protected T item;
        #endregion
        #region Constructor
        public NewItemViewModel(string[] validatedFields)
            : base()
        {
            item = new T();
            ValidatedFields = validatedFields ?? Array.Empty<string>();
        }
        #endregion
        #region Commands
        //save method for viewmodels that use pure EF entities
        public override void Save()
        {
            if (IsValid() == true)
            {
                if (!_IsEditMode)
                {
                    item.IsActive = true;
                    potplantsEntities.Set<T>().Add(item);
                }
                try
                {
                    potplantsEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show(errorMessage + ". Please correct the errors before saving.");
        }
        #endregion
        #region Functions
        #endregion
    }
}
