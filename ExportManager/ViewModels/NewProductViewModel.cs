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
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.BusinessLogic;

namespace ExportManager.ViewModels
{
    public class NewProductViewModel: NewItemViewModel<Products>
    {
        private BaseCommand _NewCategoryCommand;
        private BaseCommand _NewColorCommand;
        private KeyAndValue _SelectedCategory;
        private KeyAndValue _SelectedColor;
        public ObservableCollection<KeyAndValue> _Categories;
        public ObservableCollection<KeyAndValue> _Colors;
        #region Constructor
        public NewProductViewModel()
            : base()
        {
            base.DisplayName = "New product";
            item = new Products();
            //Categories = new CategoriesForProducts(potplantsEntities).GetCategoriesListItems();
            //Colors = new ColorsForProducts(potplantsEntities).GetColorsListItems();
        }
        #endregion
        #region Properties
        public ObservableCollection<KeyAndValue> Categories
        {
            get 
            { 
                if(_Categories == null)
                    _Categories = new CategoriesForProducts(potplantsEntities).GetCategoriesListItems();
                return _Categories; 
            }
            set
            {
                if(_Categories!=null)
                {
                    _Categories = value;
                    OnPropertyChanged(()=>Categories);
                }
            }
        }
        public ObservableCollection<KeyAndValue> Colors
        {
            get
            {
                if (_Colors == null)
                    _Colors = new ColorsForProducts(potplantsEntities).GetColorsListItems();
                return _Colors;
            }
            set
            {
                if( _Colors!=value)
                {
                    _Colors = value;
                    OnPropertyChanged(()=>Colors);
                }
            }
        }
        
        public KeyAndValue SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                if (value != _SelectedCategory)
                {
                    _SelectedCategory = value;
                    OnPropertyChanged(() => SelectedCategory);
                }
                
            }
        }
        public KeyAndValue SelectedColor
        {
            get { return _SelectedColor; }
            set
            {
                if (value != _SelectedColor)
                {
                    _SelectedColor = value;
                    OnPropertyChanged(() => SelectedColor);
                }
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
        public bool Cites {
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
            var selectedColor = potplantsEntities.Colors.FirstOrDefault(c => c.ColorId == SelectedColor.Key);
            item.ColorId = selectedColor.ColorId;
            var selectedCategory = potplantsEntities.Categories.FirstOrDefault(c => c.CategoryId == SelectedCategory.Key);
            item.CategoryId = selectedCategory.CategoryId;
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
            Categories = new CategoriesForProducts(potplantsEntities).GetCategoriesListItems();
            OnPropertyChanged(() => Categories);
        }
        private void RefreshColors()
        {
            Colors = new ColorsForProducts(potplantsEntities).GetColorsListItems();
            OnPropertyChanged(() => Colors);
        }
        #endregion
    }
}
