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

namespace ExportManager.ViewModels.Abstract
{
    public abstract class NewItemViewModelBase: WorkspaceViewModel
    {
        public event Action Added;
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
                MessageBox.Show(ex.Message);
            }
        }
        protected void RaiseAdded()
        {
            Added?.Invoke();
        }
        #endregion
    }
}
