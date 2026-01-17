using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.ViewModels.ShowAllViewModels;
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
    public abstract class AllViewModel<T> : WorkspaceViewModel, IAllViewable
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
                if (!Equals(_SelectedItem, value))
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
                    _EditCommand = new BaseCommand(Edit);
                return _EditCommand;
            }
        }
        public ICommand RemoveCommand
        {
            get
            {
                if (_RemoveCommand == null)
                    _RemoveCommand = new BaseCommand(Remove);
                return _RemoveCommand;
            }
        }
        public ReadOnlyCollection<CommandViewModel> ExtraCommands
        {
            get
            {
                if (_ExtraCommands != null)
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
        public void Edit()
        {
            if (SelectedItem != null)
            {
                OnEdit();
            }
        }
        public abstract void OnRemove();
        public void Remove()
        {
            if (SelectedItem != null)
            {
                OnRemove();
            }
        }
        protected void SoftDelete<T>(int itemId) where T : class, IHasIsActive
        {
            var item = potplantsEntities.Set<T>().Find(itemId);
            if (item == null)
                return;
            item.IsActive = false;
            item.DeletedAt = DateTime.Now;
            potplantsEntities.SaveChanges();
            Load();
        }

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
            IsCRUDMode = true;
        }
        public AllViewModel(Action<SelectedItemEventArgs> itemSetter, Func<T, SelectedItemEventArgs> generateArgsFromSelection)
        {
            potplantsEntities = new PotplantsEntities();
            IsSelectMode = true;
            _ItemSelected = itemSetter;
            _GenerateArgsFromSelection = generateArgsFromSelection;
        }
        //public AllViewModel(Action<SelectedItemEventArgs<T>> itemSetter)
        //{
        //    potplantsEntities = new PotplantsEntities();
        //    IsSelectMode = true;
        //    _ItemSelected = itemSetter;
        //}
        #endregion
        #region Select mode
        private bool _isCRUDMode;
        private bool _isSelectMode;
        protected event Action<SelectedItemEventArgs> _ItemSelected;
        protected Func<T, SelectedItemEventArgs> _GenerateArgsFromSelection;
        //protected event Action<SelectedItemEventArgs<T>> _ItemSelected;
        //protected event Action<T> _ItemSelected;
        public bool IsCRUDMode
        {
            get { return _isCRUDMode; }
            set
            {
                if (_isCRUDMode != value)
                {
                    _isCRUDMode = value;
                    _isSelectMode = !value;
                    OnPropertyChanged(() => IsCRUDMode);
                    OnPropertyChanged(() => IsSelectMode);
                }
            }
        }
        public bool IsSelectMode
        {
            get { return _isSelectMode; }
            set
            {
                if (_isSelectMode != value)
                {
                    _isSelectMode = value;
                    _isCRUDMode = !value;
                    OnPropertyChanged(() => IsSelectMode);
                    OnPropertyChanged(() => IsCRUDMode);
                }
            }
        }

        private BaseCommand _SelectCommand;
        public ICommand SelectCommand
        {
            get
            {
                if (_SelectCommand == null)
                    _SelectCommand = new BaseCommand(selectAndClose);
                return _SelectCommand;

            }
        }
        public void selectAndClose()
        {
            //_ItemSelected?.Invoke(SelectedItem);
            if(SelectedItem == null)
            {
                throw new InvalidOperationException("No item is selected.");
            }
            if(_GenerateArgsFromSelection == null)
            {
                throw new InvalidOperationException("GenerateArgsFromSelection function is not set.");
            }

            _ItemSelected?.Invoke(_GenerateArgsFromSelection(SelectedItem));
            OnRequestClose();
        }
        //public virtual SelectedItemEventArgs GenerateArgsFromSelection()
        //{
        //    if(_GenerateArgsFromSelection == null)
        //    {
        //       throw new InvalidOperationException("GenerateArgsFromSelection function is not set.");
        //    }
        //    return _GenerateArgsFromSelection(SelectedItem);
        //}
        //public virtual SelectedItemEventArgs<T> GenerateArgsFromSelection()
        //{
        //    return new SelectedItemEventArgs<T>(SelectedItem);
        //}
        #endregion
        #region Sorting and searching
        private BaseCommand _SortCommand;
        public ICommand SortCommand
        {
            get
            {
                if (_SortCommand == null)
                {
                    _SortCommand = new BaseCommand(Sort);
                }
                return _SortCommand;
            }
        }
        public string SortField { get; set; }
        private BaseCommand _FindCommand;
        public ICommand FindCommand
        {
            get
            {
                if (_FindCommand == null)
                    _FindCommand = new BaseCommand(Find);
                return _FindCommand;
            }
        }
        public string FindField { get; set; }
        public string FindTextBox { get; set; }
        public List<string> SortComboBoxItems
        {
            get { return getComboBoxSortList(); }
        }
        public List<string> FindComboBoxItems
        {
            get { return getComboBoxFindList(); }
        }
        public abstract void Sort();
        public abstract void Find();
        public abstract List<string> getComboBoxSortList();
        public abstract List<string> getComboBoxFindList();
        #endregion
    }
}
