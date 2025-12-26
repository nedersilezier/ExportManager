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

namespace ExportManager.ViewModels.Abstract
{
    public abstract class NewItemViewModelBase: WorkspaceViewModel
    {
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
            try
            {
                Save();
                RaiseAdded();
                OnRequestClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
    }
}
