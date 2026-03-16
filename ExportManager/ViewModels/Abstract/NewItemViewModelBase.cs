using ExportManager.Helper;
using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExportManager.ViewModels;
using System.Windows;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using ExportManager.ViewModels.AddViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ExportManager.ViewModels.Abstract
{
    public abstract class NewItemViewModelBase : WorkspaceViewModel
    {
        #region Fields
        protected bool _IsCreationMode = true;
        #endregion
        #region Properties
        #endregion
        #region Events
        public event Action Added;
        #endregion
        #region Database
        protected PotplantsEntities potplantsEntities;
        #endregion
        #region Constructor
        public NewItemViewModelBase()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
        #region Commands
        private BaseCommand _SaveAndCloseCommand;
        public ICommand SaveAndCloseCommand
        {
            get
            {
                if (_SaveAndCloseCommand == null)
                    _SaveAndCloseCommand = new BaseCommand(saveAndClose);
                return _SaveAndCloseCommand;
            }
        }

        #endregion
        #region Functions
        public abstract void Save();
        private void saveAndClose()
        {
            if (IsValid())
                try
                {
                    Save();
                    RaiseAdded();
                    OnRequestClose();
                }
                catch (Exception ex)
                {
                    //Developer friendly
                    //MessageBox.Show(ex.ToString());
                    //User friendly
                    MessageBox.Show(ex.Message);
                }
            else
            {
                var message = string.Join(Environment.NewLine, this.GetValidationErrors());
                MessageBox.Show(message + "\nPlease correct the errors before saving.");
            }
                
        }
        protected void RaiseAdded()
        {
            Added?.Invoke();
        }
        //protected void OpenNewTab<T>(Action refreshEvent) where T: WorkspaceViewModel, new()
        //{
        //    var viewModel = new T();
        //    if(viewModel is NewItemViewModelBase newItemViewModel)
        //    {
        //        void handler()
        //        {
        //            refreshEvent();
        //            newItemViewModel.Added -= handler;
        //        }
        //        newItemViewModel.Added += handler;
        //    }
        //    OnRequestOpen(viewModel);
        //}
        #endregion
        #region  Validation 
        protected string[] ValidatedFields;
        public virtual string this[string name] { get { return null; } }
        public virtual bool IsValid()
        {
            return true;
        }
        protected IEnumerable<string> GetValidationErrors()
        {
            foreach(var field in ValidatedFields)
            {
                var error = this[field];
                if (!string.IsNullOrEmpty(error))
                {
                    yield return field + ": " + error;
                }
            }
        }
        #endregion
    }
}
