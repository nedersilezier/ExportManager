using ExportManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.ViewModels.Abstract;
using ExportManager.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ExportManager.Helper;
using System.Windows.Input;

namespace ExportManager.ViewModels
{
    public class NewProductViewModel: NewItemViewModel<Products>
    {
        private BaseCommand _NewCategoryCommand;
        private BaseCommand _NewColorCommand;
        private Categories _SelectedCategory;
        private Colors _SelectedColor;
        public ObservableCollection<Categories> Categories { get; set; }
        public ObservableCollection<Colors> Colors { get; set; }
        #region Constructor
        public NewProductViewModel()
            : base()
        {
            base.DisplayName = "New product";
            item = new Products();
            Categories = new ObservableCollection<Categories>(potplantsEntities.Categories.Where(t => t.IsActive == true).ToList());
            Colors = new ObservableCollection<Colors>(potplantsEntities.Colors.Where(t => t.IsActive == true).ToList());
        }
        #endregion
        #region Properties
        
        public Categories SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                if (value != _SelectedCategory)
                {
                    _SelectedCategory = value;
                }
                OnPropertyChanged(() => SelectedCategory);
            }
        }
        public Colors SelectedColor
        {
            get { return _SelectedColor; }
            set
            {
                if (value != _SelectedColor)
                {
                    _SelectedColor = value;
                }
                OnPropertyChanged(() => SelectedColor);
            }
        }
        public string Name
        {
            get { return item.Name; }
            set
            {
                if (item.Name != value)
                {
                    item.Name = value;
                }
                OnPropertyChanged(() => Name);
            }
        }
        public decimal? Potsize {
            get { return item.Potsize; } 
            set
            {
                if(item.Potsize != value)
                    item.Potsize = value;
                OnPropertyChanged(() => Potsize);
            }
        }
        public decimal? Height {
            get { return item.Height; }
            set
            {
                if (item.Height != value)
                    item.Height = value;
                OnPropertyChanged(() => Height);
            }
        }
        public decimal? Weight {
            get { return item.Weight; }
            set
            {
                if(item.Weight != value)
                    item.Weight = value;
                OnPropertyChanged(() => Weight);
            }
        }
        public bool? Cites {
            get { return item.IsCites; }
            set
            {
                if (item.IsCites != value)
                    item.IsCites = value;
                OnPropertyChanged(() => Cites);
            }
        }
        public string Remarks {
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
        public override void Save()
        {
            if(SelectedCategory == null || SelectedColor == null) 
                throw new Exception("No category or color selected.");
            item.CategoryId = SelectedCategory.CategoryId;
            item.ColorId = SelectedColor.ColorId;
            item.IsActive = true;
            potplantsEntities.Products.Add(item);
            potplantsEntities.SaveChanges();
        }
        public ICommand NewCategoryCommand
        {
            get
            {
                if(_NewCategoryCommand == null)
                {
                    _NewCategoryCommand = new BaseCommand(OpenNewCategoryTab);
                }
                return _NewCategoryCommand;
            }
        }
        public ICommand NewColorCommand
        {
            get
            {
                if (_NewColorCommand == null)
                    _NewColorCommand = new BaseCommand(OpenNewColorTab);
                return _NewColorCommand;
            }
        }
        #endregion
        #region Functions
        //fuj. Powtarzajacy sie kod! W wolnym czasie przemyslec.
        private void OpenNewCategoryTab()
        {
            var viewModel = new NewCategoryViewModel();
            viewModel.Added += RefreshCategories;
            var mainViewModel = (MainWindowViewModel)App.Current.MainWindow.DataContext;
            mainViewModel.CreateView(viewModel);
        }
        private void OpenNewColorTab()
        {
            var viewModel = new NewColorViewModel();
            viewModel.Added += RefreshColors;
            var mainViewModel = (MainWindowViewModel)App.Current.MainWindow.DataContext;
            mainViewModel.CreateView(viewModel);
        }
        private void RefreshCategories()
        {
            Categories = new ObservableCollection<Categories>(potplantsEntities.Categories.Where(t => t.IsActive == true).ToList());
            OnPropertyChanged(() => Categories);
        }
        private void RefreshColors()
        {
            Colors = new ObservableCollection<Colors>(potplantsEntities.Colors.Where(t => t.IsActive == true).ToList());
            OnPropertyChanged(() => Colors);
        }
        #endregion
    }
}
