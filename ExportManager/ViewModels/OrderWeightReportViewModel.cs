using ExportManager.Helper;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels
{
    public class OrderWeightReportViewModel: WorkspaceViewModel
    {
        #region Fields
        public DateTime _Date;
        private decimal? _Weight;
        private ObservableCollection<KeyAndValue> _OrderItems;
        private KeyAndValue _SelectedOrder;
        private BaseCommand _CalculateWeight;
        private ObservableCollection<WeightReportProductListView> _ProductList;
        private ObservableCollection<WeightReportCarrierListView> _CarrierList;
        #endregion
        #region Database
        public PotplantsEntities potplantsEntities;
        #endregion
        #region Constructor
        public OrderWeightReportViewModel()
        {
            base.DisplayName = "Weight report";
            potplantsEntities = new PotplantsEntities();
            Date = DateTime.Now;
            //OrderItems = new OrdersForReports(potplantsEntities).GetOrdersListItemsPerDate(Date);

        }
        #endregion
        #region Properties
        public ObservableCollection<WeightReportProductListView> ProductList
        {
            get
            {
                if (_ProductList == null)
                    LoadProducts();
                return _ProductList;
            }
            set
            {
                if (_ProductList != value)
                {
                    _ProductList = value;
                    OnPropertyChanged(() => ProductList);
                }

            }
        }
        public ObservableCollection<WeightReportCarrierListView> CarrierList
        {
            get
            {
                if (_CarrierList == null)
                    LoadCarriers();
                return _CarrierList;
            }
            set
            {
                if (_CarrierList != value)
                {
                    _CarrierList = value;
                    OnPropertyChanged(() => CarrierList);
                }

            }
        }
        public ObservableCollection<KeyAndValue> OrderItems
        {
            get { return _OrderItems; }
            set
            {
                if(_OrderItems != value)
                {
                    _OrderItems = value;
                    OnPropertyChanged(() => OrderItems);
                }
            }
        }

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    _OrderItems = new OrdersForReports(potplantsEntities).GetOrdersListItemsPerDate(Date);
                    OnPropertyChanged(() => Date);
                    OnPropertyChanged(() => OrderItems);
                }
            }
        }
        public decimal? Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged(() => Weight);
                }
            }
        }
        public KeyAndValue SelectedOrder
        {
            get { return _SelectedOrder; }
            set
            {
                if (_SelectedOrder != value)
                {
                    _SelectedOrder = value;
                    OnPropertyChanged(() => SelectedOrder);
                }
            }
        }
        public ICommand CalculateWeight
        {
            get
            {
                if (_CalculateWeight == null)
                    _CalculateWeight = new BaseCommand(calculateWeightClick);
                return _CalculateWeight;
            }
        }
        #endregion
        #region Functions
        public void LoadProducts()
        {
            if (SelectedOrder == null)
                return;
            try
            {
                ProductList = new ObservableCollection<WeightReportProductListView>(
                new OrderWeightCalculator(potplantsEntities).ProductsQuery(SelectedOrder.Key, Date).ToList());
            }
            catch(Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }
        public void LoadCarriers()
        {
            if(SelectedOrder == null)
                return;
            CarrierList = new ObservableCollection<WeightReportCarrierListView>(
                new OrderWeightCalculator(potplantsEntities).CarriersQuery(SelectedOrder.Key, Date).ToList());
        }
        private void calculateWeightClick()
        {
            if (SelectedOrder == null)
                return;
            Weight = new OrderWeightCalculator(potplantsEntities).CalculateOrderWeight(SelectedOrder.Key, Date);
            LoadProducts();
            LoadCarriers();
            Console.WriteLine("OrderId: " + SelectedOrder.Key);
            Console.WriteLine("Weight: " + Weight);
        }
        #endregion
    }
}
