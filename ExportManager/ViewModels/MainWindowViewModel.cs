using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ExportManager.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.ShowAllViewModels;
using ExportManager.ViewModels.ReportViewModels;
using System.Windows;
using ExportManager.ViewModels.Windows;
using ExportManager.Views.Windows;

namespace ExportManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ReadOnlyCollection<CommandViewModel> _GlobalCommands;
        private ReadOnlyCollection<CommandViewModel> _DatabaseDictionaryCommands;
        private CommandViewModel _AddressCommand;
        private CommandViewModel _ProductsCommand;
        private ReadOnlyCollection<CommandViewModel> _EntityCommands;
        private ReadOnlyCollection<CommandViewModel> _ReportCommands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
            }
        }
        private int _LoadingCounter = 0;
        public int LoadingCounter
        {
            get { return _LoadingCounter; }
            set
            {
                _LoadingCounter = value;
            }
        }
        #endregion
        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        public ReadOnlyCollection<CommandViewModel> GlobalCommands
        {
            get
            {
                if (_GlobalCommands == null)
                {
                    _GlobalCommands = new ReadOnlyCollection<CommandViewModel>(
                        this.CreateGlobalCommands());
                }
                return _GlobalCommands;
            }
        }
        public ReadOnlyCollection<CommandViewModel> DatabaseDictionaryCommands
        {
            get
            {
                if (_DatabaseDictionaryCommands == null)
                {
                    _DatabaseDictionaryCommands = new ReadOnlyCollection<CommandViewModel>(
                        this.CreateDatabaseDictionaryCommands());
                }
                return _DatabaseDictionaryCommands;
            }
        }
        public ReadOnlyCollection<CommandViewModel> EntityCommands
        {
            get
            {
                if (_EntityCommands == null)
                {
                    _EntityCommands = new ReadOnlyCollection<CommandViewModel>(
                        this.CreateEntityCommands());
                }
                return _EntityCommands;
            }
        }
        public ReadOnlyCollection<CommandViewModel> ReportCommands
        {
            get
            {
                if (_ReportCommands == null)
                {
                    _ReportCommands = new ReadOnlyCollection<CommandViewModel>(
                        this.CreateReportCommands());
                }
                return _ReportCommands;
            }
        }
        public CommandViewModel AddressCommand
        {
            get
            {
                if (_AddressCommand == null)
                {
                    _AddressCommand = new CommandViewModel(
                        "Manage Addresses",
                        new BaseCommand(() => this.ShowAll<AllAddressesViewModel>())
                    );
                }
                return _AddressCommand;
            }
        }
        public CommandViewModel ProductsCommand
        {
            get
            {
                if (_ProductsCommand == null)
                {
                    _ProductsCommand = new CommandViewModel(
                        "Manage Products",
                        new BaseCommand(() => this.ShowAll<AllProductsViewModel>())
                    );
                }
                return _AddressCommand;
            }
        }
        //Bylo w szkielecie
        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                //new CommandViewModel(
                //    "Towary",
                //    new BaseCommand(() => this.ShowAll<>())),

                //new CommandViewModel(
                //    "Towar",
                //    new BaseCommand(() => this.CreateTowar()))
            };
        }
        //Komendy w StackPanel
        private List<CommandViewModel> CreateGlobalCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Catalog", new BaseCommand(() => this.ShowAll<AllProductsViewModel>())),
                new CommandViewModel("Stock", new BaseCommand(() => this.ShowAll<AllInStockViewModel>())),
                new CommandViewModel("Batches", new BaseCommand(() => this.ShowAll<AllBatchesViewModel>())),
                new CommandViewModel("Orders", new BaseCommand(() => this.ShowAll<AllOrdersViewModel>())),
                new CommandViewModel("Order items", new BaseCommand(() => this.ShowAll<AllOrderItemsViewModel>())),
                new CommandViewModel("Invoices", new BaseCommand(() => this.ShowAll<AllInvoicesViewModel>()))
            };
        }
        //Komendy w menu u gory
        private List<CommandViewModel> CreateDatabaseDictionaryCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Carrier types", new BaseCommand(() => this.ShowAll<AllCarrierTypesViewModel>())),
                new CommandViewModel("Categories", new BaseCommand(() => this.ShowAll<AllCategoriesViewModel>())),
                new CommandViewModel("Colors", new BaseCommand(() => this.ShowAll<AllColorsViewModel>())),
                new CommandViewModel("Countries", new BaseCommand(() => this.ShowAll<AllCountriesViewModel>())),
                new CommandViewModel("Cultivations", new BaseCommand(() => this.ShowAll<AllCultivationsViewModel>())),
                new CommandViewModel("Payment methods", new BaseCommand(() => this.ShowAll<AllPaymentMethodsViewModel>())),
                new CommandViewModel("Quality types", new BaseCommand(() => this.ShowAll<AllQualitiesViewModel>())),
                new CommandViewModel("Tray types", new BaseCommand(() => this.ShowAll<AllTrayTypesViewModel>()))
            };
        }
        private List<CommandViewModel> CreateEntityCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Clients", new BaseCommand(() => this.ShowAll<AllClientsViewModel>())),
                new CommandViewModel("Growers", new BaseCommand(() => this.ShowAll<AllGrowersViewModel>()))
            };
        }
        private List<CommandViewModel> CreateReportCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Volume per client per period", new BaseCommand(() => this.ShowAll<VolumeReportViewModel>())),
                new CommandViewModel("Weight per order", new BaseCommand(() => this.ShowAll<OrderWeightReportViewModel>())),
                new CommandViewModel("Invoice summary", new BaseCommand(() => this.ShowAll<InvoiceReportViewModel>()))
            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += this.OnWorkspaceRequestClose;
                    workspace.RequestOpen += OnWorkspaceRequestOpen;
                    workspace.LoadingStarted += OnWorkspaceLoadingStarted;
                    workspace.LoadingFinished += OnWorkspaceLoadingEnded;
                    workspace.RequestImageWindow += OnWorkspaceWindowRequest;
                }
            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
                    workspace.RequestOpen -= OnWorkspaceRequestOpen;
                    workspace.LoadingStarted -= OnWorkspaceLoadingStarted;
                    workspace.LoadingFinished -= OnWorkspaceLoadingEnded;
                    workspace.RequestImageWindow -= OnWorkspaceWindowRequest;
                }      
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }
        private void OnWorkspaceRequestOpen(object sender, WorkspaceViewModel workspace)
        {
            this.CreateView(workspace);
        } 
        private void OnWorkspaceLoadingStarted(object sender, EventArgs e)
        {
            LoadingCounter++;
            this.IsLoading = true;
            OnPropertyChanged(() => IsLoading);
            Console.WriteLine(IsLoading);
        }
        private void OnWorkspaceLoadingEnded(object sender, EventArgs e)
        {
            LoadingCounter--;
            if (LoadingCounter <= 0)
            {
                LoadingCounter = 0;
                this.IsLoading = false;
                OnPropertyChanged(() => IsLoading);
                Console.WriteLine(IsLoading);
            }
        }
        private void OnWorkspaceWindowRequest(object sender, ImageWindowEventArgs e)
        {
            var window = new ImageWindowViewModel(e.Title, e.ImageData);
            new ImageWindowView(window).Show();
        }
        #endregion // Workspaces

        #region Private Helpers
        public void ShowAll<T>() where T:WorkspaceViewModel, new()
        {
            T workspace =
                this.Workspaces.FirstOrDefault(vm => vm is T)
                as T;
            if (workspace == null)
            {
                workspace = new T();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
        public void CreateView(WorkspaceViewModel workspace) //jedna uniwersalna funkcja do tworzenia widokow
        {
            this.Workspaces.Add(workspace); //dodajemy zakladke do kolekcji zakladek
            this.SetActiveWorkspace(workspace); //  aktywujemy zakladke(zeby byla wlaczona)
        }
        //private void ShowAllTowar()
        //{
        //    WszystkieTowaryViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is WszystkieTowaryViewModel)
        //        as WszystkieTowaryViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new WszystkieTowaryViewModel();
        //        this.Workspaces.Add(workspace);
        //    }

        //    this.SetActiveWorkspace(workspace);
        //}
        //private void ShowAllProducts()
        //{
        //    AllProductsViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is AllProductsViewModel)
        //        as AllProductsViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new AllProductsViewModel();
        //        this.Workspaces.Add(workspace);
        //    }
        //    this.SetActiveWorkspace(workspace);
        //}
        //private void ShowAllOrderItems()
        //{
        //    AllOrderItemsViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is AllOrderItemsViewModel)
        //        as AllOrderItemsViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new AllOrderItemsViewModel();
        //        this.Workspaces.Add(workspace);
        //    }

        //    this.SetActiveWorkspace(workspace);
        //}
        //private void ShowAllCarrierTypes()
        //{
        //    AllCarrierTypesViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is AllCarrierTypesViewModel)
        //        as AllCarrierTypesViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new AllCarrierTypesViewModel();
        //        this.Workspaces.Add(workspace);
        //    }
        //    this.SetActiveWorkspace(workspace);
        //}
        //private void ShowAllCategories()
        //{
        //    AllCategoriesViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is AllCategoriesViewModel)
        //        as AllCategoriesViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new AllCategoriesViewModel();
        //        this.Workspaces.Add(workspace);
        //    }
        //    this.SetActiveWorkspace(workspace);
        //}
        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }
}
