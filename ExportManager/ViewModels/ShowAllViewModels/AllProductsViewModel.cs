using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ExportManager.ViewModels.Abstract;
using ExportManager.Models.EntitiesForView;
using ExportManager.Helper;
using System.Windows.Input;
using System.Windows;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.Models;
using ExportManager.ViewModels.Windows;
using ExportManager.Views.Windows;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllProductsViewModel : AllViewModel<dynamic>
    {
        #region Fields
        private BaseCommand _ShowPictureCommand;
        #endregion
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(
                from product in potplantsEntities.Products
                where product.IsActive == true
                select new ProductsListView
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    ColorName = product.Colors.Name,
                    ColorRemarks = product.Colors.Remarks,
                    CategoryName = product.Categories.Name,
                    CategoryRemarks = product.Categories.Remarks,
                    Potsize = product.Potsize,
                    Height = product.Height,
                    Weight = product.Weight,
                    IsCites = product.IsCites,
                    Remarks = product.Remarks
                }
                );
        }
        #endregion
        #region Constructor
        public AllProductsViewModel()
            : base()
        {
            base.DisplayName = "Products";

        }
        #endregion
        #region Commands
        public ICommand ShowPictureCommand
        {
            get
            {
                if (_ShowPictureCommand == null)
                    _ShowPictureCommand = new BaseCommand(OnShowPicture);
                return _ShowPictureCommand;
            }
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewProductViewModel(), Load);
        }
        public override void OnEdit()
        {
            if (SelectedItem == null)
                return;
            OpenNewTab(() => new NewProductViewModel(SelectedItem.ProductId), Load);
        }
        public override void OnRemove()
        {
            SoftDelete<Products>(SelectedItem.ProductId);
        }
        private void OnShowPicture()
        {
            if (SelectedItem != null && SelectedItem is ProductsListView)
            {
                int productId = SelectedItem.ProductId;
                var image = potplantsEntities.Products.Where(t => t.ProductId == productId).Select(t => t.Image).FirstOrDefault();
                if (image != null && image.Length > 0)
                {
                    OnRequestImageWindow(new ImageWindowEventArgs(image, SelectedItem.Name));
                }
                else
                {
                    MessageBox.Show("No image available for the selected product.", "Image Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            //List<CommandViewModel> TestList = new List<CommandViewModel>();
            //for(int i = 0; i < 4; i++)
            //{
            //    TestList.Add(new CommandViewModel("Test button " + (i+1).ToString(), TestCommand));
            //}
            //return TestList;
            return new List<CommandViewModel>
            {
                new CommandViewModel("Picture", ShowPictureCommand)
            };
        }
        #endregion

    }
}