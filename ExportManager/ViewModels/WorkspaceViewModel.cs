using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportManager.Helper;
using System.Windows.Input;
using ExportManager.ViewModels.Abstract;

namespace ExportManager.ViewModels
{
    public abstract class WorkspaceViewModel : BaseViewModel
    {
        #region Fields
        private BaseCommand _CloseCommand;
        #endregion 
        
        #region Constructor
        public WorkspaceViewModel()
        {
        }
        #endregion 

        #region CloseCommand
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(() => this.OnRequestClose());
                return _CloseCommand;
            }
        }
        #endregion
        #region Events
        #region RequestClose
        public event EventHandler RequestClose;
        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion
        #region RequestOpen
        public event EventHandler<WorkspaceViewModel> OpenWorkspaceRequested;
        protected void OnRequestOpen(WorkspaceViewModel workspace)
        {
            OpenWorkspaceRequested?.Invoke(this, workspace);
        }
        #endregion
        #endregion
        #region Functions
        protected void OpenNewTab<T>(Action refreshEvent) where T : WorkspaceViewModel, new()
        {
            var viewModel = new T();
            if (viewModel is NewItemViewModelBase newItemViewModel)
            {
                void handler()
                {
                    refreshEvent();
                    newItemViewModel.Added -= handler;
                }
                newItemViewModel.Added += handler;
            }
            OnRequestOpen(viewModel);
        }
        #endregion

    }
}
