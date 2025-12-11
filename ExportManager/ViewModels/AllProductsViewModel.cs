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

namespace ExportManager.ViewModels
{
    public class AllProductsViewModel : AllViewModel<dynamic>
    {
        private BaseCommand _TestCommand;
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(
                from product in potplantsEntities.Products
                where product.IsActive == true
                select new ProductsListView
                {
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
            :base()
        {
            base.DisplayName = "Products";

        }
        #endregion
        #region Commands
        public ICommand TestCommand
        {
            get
            {
                if (_TestCommand == null)
                    _TestCommand = new BaseCommand(Test);
                return _TestCommand;
            }
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            AddNew<NewProductViewModel>();
        }
        public override void OnEdit()
        {
            return;
        }
        public override void OnRemove()
        {
            return;
        }
        public void Test()
        {
            if(SelectedItem != null && SelectedItem is ProductsListView)
                MessageBox.Show(SelectedItem.Name);
        }
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            List<CommandViewModel> TestList = new List<CommandViewModel>();
            for(int i = 0; i < 4; i++)
            {
                TestList.Add(new CommandViewModel("Test button " + (i+1).ToString(), TestCommand));
            }
            return TestList;
            //return new List<CommandViewModel>
            //{
            //    new CommandViewModel("Test", TestCommand)
            //};
        }
        #endregion

    }
}