using ExportManager.Helper;
using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.Abstract
{
    public abstract class AllViewModel<T>: WorkspaceViewModel
    {
        private T _SelectedItem;
        private ReadOnlyCollection<CommandViewModel> _ExtraCommands;
        #region Database
        protected PotplantsEntities potplantsEntities;
        #endregion
        #region Properties
        public T SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if(!Equals(_SelectedItem, value))
                    _SelectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        #endregion
        #region Command
        private BaseCommand _LoadCommand;
        private BaseCommand _AddCommand;
        private BaseCommand _EditCommand;
        private BaseCommand _RemoveCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(Load);
                return _LoadCommand;
            }
        }
        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                    _AddCommand = new BaseCommand(OnAdd);
                return _AddCommand;
            }
        }
        public ICommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                    _EditCommand = new BaseCommand(OnEdit);
                return _EditCommand;
            }
        }
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new BaseCommand(OnRemove);
                return _RemoveCommand;
            }
        }
        public ReadOnlyCollection<CommandViewModel> ExtraCommands
        {
            get
            {
                if(_ExtraCommands != null)
                    return _ExtraCommands;
                _ExtraCommands = new ReadOnlyCollection<CommandViewModel>(CreateExtraCommands());
                return _ExtraCommands;
            }
        }
        
        #endregion
        #region Functions
        //protected void AddNew<T2>() where T2: NewItemViewModelBase, new()
        //{
        //    var viewModel = new T2();
        //    if(viewModel is NewItemViewModelBase newitemViewModelBase)
        //    {
        //        void handler()
        //        {
        //            Load();
        //            newitemViewModelBase.Added -= handler;
        //        }
        //        newitemViewModelBase.Added += handler;
        //    }
        //    OnRequestOpen(viewModel);
        //}
        public abstract void Load();
        public abstract void OnAdd();
        public abstract void OnEdit();
        public abstract void OnRemove();
        public virtual IList<CommandViewModel> CreateExtraCommands()
        {
            return new List<CommandViewModel>();
        }
        #endregion
        #region List
        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                {
                    OnLoadingStarted();
                    Load();
                    OnLoadingFinished();
                }
                return _List;
            }
            set
            {
                if (_List != value)
                {
                    _List = value;
                    OnPropertyChanged(() => List);
                }

            }
        }
        #endregion
        #region Constructor
        public AllViewModel()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
    }
}
