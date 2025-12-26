using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewBatchViewModel: NewItemViewModel<StockItems>
    {
        #region Fields
        private BaseCommand _NewProductCommand;
        private BaseCommand _NewGrowerCommand;
        private BaseCommand _NewQualityCommand;
        private BaseCommand _NewTrayTypeCommand;
        private KeyAndValue _SelectedProduct;
        private KeyAndValue _SelectedGrower;
        private KeyAndValue _SelectedQuality;
        private KeyAndValue _SelectedTrayType;
        public ObservableCollection<KeyAndValue> _Products;
        public ObservableCollection<KeyAndValue> _Growers;
        public ObservableCollection<KeyAndValue> _Qualities;
        public ObservableCollection<KeyAndValue> _TrayTypes;
        #endregion
        #region Constructor
        public NewBatchViewModel()
            :base()
        {
            base.DisplayName = "New batch";
            item = new StockItems();
            ExpiryDate = DateTime.Now;
            IsBlocked = false;
        }
        public NewBatchViewModel(int batchId)
            : base()
        {
            base.DisplayName = "Edit batch";
            _IsEditMode = true;
            item = potplantsEntities.StockItems.FirstOrDefault(t => t.StockItemId == batchId);
            SelectedGrower = Growers.FirstOrDefault(g => g.Key == item.GrowerId);
            SelectedProduct = Products.FirstOrDefault(p => p.Key == item.ProductId);
            SelectedQuality = Qualities.FirstOrDefault(q => q.Key == item.QualityId);
            SelectedTrayType = TrayTypes.FirstOrDefault(t => t.Key == item.TrayTypeId);
        }
        #endregion
        #region Properties
        public ObservableCollection<KeyAndValue> Products
        {
            get
            {
                if (_Products == null)
                    _Products = new ProductsForStockItems(potplantsEntities).GetProductsListItems();
                return _Products;
            }
            set
            {
                if (_Products != value)
                {
                    _Products = value;
                    OnPropertyChanged(() => Products);
                }
            }
        }
        public ObservableCollection<KeyAndValue> Growers
        {
            get
            {
                if (_Growers == null)
                    _Growers = new GrowersForStockItems(potplantsEntities).GetGrowersListItems();
                return _Growers;
            }
            set
            {
                if (_Growers != value)
                {
                    _Growers = value;
                    OnPropertyChanged(() => Growers);
                }
            }
        }
        public ObservableCollection<KeyAndValue> Qualities
        {
            get
            {
                if (_Qualities == null)
                    _Qualities = new QualitiesForStockItems(potplantsEntities).GetQualitiesListItems();
                return _Qualities;
            }
            set
            {
                if (_Qualities != value)
                {
                    _Qualities = value;
                    OnPropertyChanged(() => Qualities);
                }
            }
        }
        public ObservableCollection<KeyAndValue> TrayTypes
        {
            get
            {
                if (_TrayTypes == null)
                    _TrayTypes = new TrayTypesForStockItems(potplantsEntities).GetTrayTypesListItems();
                return _TrayTypes;
            }
            set
            {
                if (_TrayTypes != value)
                {
                    _TrayTypes = value;
                    OnPropertyChanged(() => TrayTypes);
                }
            }
        }
        public KeyAndValue SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                if (_SelectedProduct != value)
                {
                    _SelectedProduct = value;
                    TrayTypes = new TrayTypesForStockItems(potplantsEntities).GetCompatibleTrayTypes(SelectedProduct.Key);
                    OnPropertyChanged(() => TrayTypes);
                    OnPropertyChanged(() => SelectedProduct);
                }
            }
        }
        public KeyAndValue SelectedGrower
        {
            get { return _SelectedGrower; }
            set
            {
                if (_SelectedGrower != value)
                {
                    _SelectedGrower = value;
                    OnPropertyChanged(() => SelectedGrower);
                }
            }
        }
        public KeyAndValue SelectedQuality
        {
            get { return _SelectedQuality; }
            set
            {
                if (_SelectedQuality != value)
                {
                    _SelectedQuality = value;
                    OnPropertyChanged(() => SelectedQuality);
                }
            }
        }
        public KeyAndValue SelectedTrayType
        {
            get { return _SelectedTrayType; }
            set
            {
                if (_SelectedTrayType != value)
                {
                    _SelectedTrayType = value;
                    OnPropertyChanged(() => SelectedTrayType);
                }
            }
        }
        public int Quantity
        {
            get { return item.Quantity ?? 0; }
            set
            {
                if (item.Quantity != value)
                {
                    item.Quantity = value;
                    OnPropertyChanged(() => Quantity);
                }
            }
        }
        public decimal CostPrice
        {
            get { return item.CostPrice ?? 0; }
            set
            {
                if (item.CostPrice != value)
                {
                    item.CostPrice = value;
                    OnPropertyChanged(() => CostPrice);
                }
            }
        }
        public string InternalNo
        {
            get { return item.InternalNo; }
            set
            {
                if (item.InternalNo != value)
                {
                    item.InternalNo = value;
                    OnPropertyChanged(() => InternalNo);
                }
            }
        }
        public bool IsBlocked
        {
            get { return item.IsBlocked ?? false; }
            set
            {
                if (item.IsBlocked != value)
                {
                    item.IsBlocked = value;
                    OnPropertyChanged(() => IsBlocked);
                }
            }
        }
        public DateTime? ExpiryDate
        {
            get { return item.ExpiryDate;  }
            set
            {
                if(item.ExpiryDate != value)
                {
                    item.ExpiryDate = value;
                    OnPropertyChanged(() => ExpiryDate);
                }
            }
        }
        public string Remarks
        {
            get { return item.Remarks; }
            set
            {
                if (item.Remarks != value)
                {
                    item.Remarks = value;
                    OnPropertyChanged(() => Remarks);
                }
            }
        }
        #endregion
        #region Commands
        public ICommand NewProductCommand
        {
            get
            {
                if (_NewProductCommand == null)
                {
                    _NewProductCommand = new BaseCommand(OpenNewProductTab);
                }
                return _NewProductCommand;
            }
        }
        public ICommand NewGrowerCommand
        {
            get
            {
                if (_NewGrowerCommand == null)
                {
                    _NewGrowerCommand = new BaseCommand(OpenNewGrowerTab);
                }
                return _NewGrowerCommand;
            }
        }
        public ICommand NewQualityCommand
        {
            get
            {
                if (_NewQualityCommand == null)
                {
                    _NewQualityCommand = new BaseCommand(openNewQualityTab);
                }
                return _NewQualityCommand;
            }
        }
        public ICommand NewTrayTypeCommand
        {
            get
            {
                if (_NewTrayTypeCommand == null)
                {
                    _NewTrayTypeCommand = new BaseCommand(OpenNewTrayTypeTab);
                }
                return _NewTrayTypeCommand;
            }
        }
        #endregion
        #region Functions
        public override void Save()
        {
            if (SelectedProduct == null || SelectedGrower == null || SelectedQuality == null || SelectedTrayType == null)
                throw new Exception("Please select all data.");
            item.ProductId = SelectedProduct.Key;
            item.GrowerId = SelectedGrower.Key;
            item.QualityId = SelectedQuality.Key;
            item.TrayTypeId = SelectedTrayType.Key;
            if(!_IsEditMode)
            {
                item.QuantityLeft = item.Quantity;
                item.IsActive = true;
                var counter = potplantsEntities.Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.Seq_InternalNo").Single();
                item.InternalNo = counter.ToString("D7");
                potplantsEntities.StockItems.Add(item);
            } 
            potplantsEntities.SaveChanges();
        }
        public void OpenNewProductTab()
        {
            OpenNewTab(() => new NewProductViewModel(), RefreshProducts);
        }

        public void OpenNewGrowerTab()
        {
            OpenNewTab(() => new NewGrowerViewModel(), RefreshGrowers);
        }
        public void openNewQualityTab()
        {
            OpenNewTab(() => new NewQualityTypeViewModel(), RefreshQualities);
        }
        public void OpenNewTrayTypeTab()
        {
            OpenNewTab(() => new NewTrayTypeViewModel(), RefreshTrayTypes);
        }
        private void RefreshProducts()
        {
            Products = new ProductsForStockItems(potplantsEntities).GetProductsListItems();
        }
        private void RefreshGrowers()
        {
            Growers = new GrowersForStockItems(potplantsEntities).GetGrowersListItems();
        }
        private void RefreshQualities()
        {
            Qualities = new QualitiesForStockItems(potplantsEntities).GetQualitiesListItems();
        }
        private void RefreshTrayTypes()
        {
            if(SelectedProduct == null)
                TrayTypes = new TrayTypesForStockItems(potplantsEntities).GetTrayTypesListItems();
            else
                TrayTypes = new TrayTypesForStockItems(potplantsEntities).GetCompatibleTrayTypes(SelectedProduct.Key);
        }
        #endregion
    }
}
