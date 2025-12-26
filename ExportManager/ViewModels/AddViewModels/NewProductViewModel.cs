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
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using Microsoft.Win32;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewProductViewModel: NewItemViewModel<Products>
    {
        private BaseCommand _NewCategoryCommand;
        private BaseCommand _NewColorCommand;
        private BaseCommand _SelectImageCommand;
        private BaseCommand _RemoveImageCommand;
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
        }
        public NewProductViewModel(int productId)
            :base()
        {
            base.DisplayName = "Edit product";
            _IsEditMode = true;
            item = potplantsEntities.Products.FirstOrDefault(p => p.ProductId == productId);
            SelectedCategory = Categories.FirstOrDefault(c => c.Key == item.CategoryId);
            SelectedColor = Colors.FirstOrDefault(c => c.Key == item.ColorId);
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
                if(_Categories!=value)
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
                    OnPropertyChanged(() => Name);
                }
            }
        }
        public decimal? Potsize {
            get { return item.Potsize; } 
            set
            {
                if(item.Potsize != value)
                {
                    item.Potsize = value;
                    OnPropertyChanged(() => Potsize);
                }
            }
        }
        public decimal? Height {
            get { return item.Height; }
            set
            {
                if (item.Height != value)
                {
                    item.Height = value;
                    OnPropertyChanged(() => Height);
                }
            }
        }
        public decimal? Weight {
            get { return item.Weight; }
            set
            {
                if(item.Weight != value)
                {
                    item.Weight = value;
                    OnPropertyChanged(() => Weight);
                }
            }
        }
        public bool Cites {
            get { return item.IsCites; }
            set
            {
                if (item.IsCites != value)
                {
                    item.IsCites = value;
                    OnPropertyChanged(() => Cites);
                }
            }
        }
        public string Remarks {
            get { return item.Remarks; } 
            set
            {
                if(item.Remarks != value)
                {
                    item.Remarks = value;
                    OnPropertyChanged(() => Remarks);
                }
            }
        }
        public byte[] Image
        {
            get { return item.Image; }
            set
            {
                if (item.Image != value)
                {
                    item.Image = value;
                    OnPropertyChanged(() => Image);
                }
            }
        }
        #endregion
        #region Commands
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
        public ICommand SelectImageCommand
        {
            get
            {
                if(_SelectImageCommand == null)
                    _SelectImageCommand = new BaseCommand(SelectImageDialog);
                return _SelectImageCommand;
            }
        }
        public ICommand RemoveImageCommand
        {
            get
            {
                if (_RemoveImageCommand == null)
                    _RemoveImageCommand = new BaseCommand(RemoveImage);
                return _RemoveImageCommand;
            }
        }
        #endregion
        #region Functions
        public override void Save()
        {
            if (SelectedCategory == null || SelectedColor == null)
                throw new Exception("No category or color selected.");
            item.ColorId = SelectedColor.Key;
            item.CategoryId = SelectedCategory.Key;
            if (!_IsEditMode)
            {
                item.IsActive = true;
                potplantsEntities.Products.Add(item);
            }
            potplantsEntities.SaveChanges();
        }
        private void OpenNewCategoryTab()
        {
            OpenNewTab(() => new NewCategoryViewModel(), RefreshCategories);
        }
        private void OpenNewColorTab()
        {
            OpenNewTab(() => new NewColorViewModel(), RefreshColors);
        }
        private void RefreshCategories()
        {
            Categories = new CategoriesForProducts(potplantsEntities).GetCategoriesListItems();
        }
        private void RefreshColors()
        {
            Colors = new ColorsForProducts(potplantsEntities).GetColorsListItems();
        }
        private void SelectImageDialog()
        {
            var dialogWindow = new OpenFileDialog { Filter = "Image files (*jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"};
            if(dialogWindow.ShowDialog() == true)
            {
                string selectedFileName = dialogWindow.FileName;
                Image = System.IO.File.ReadAllBytes(selectedFileName);
            }
        }
        private void RemoveImage()
        {
            Image = null;
        }
        #endregion
    }
}
