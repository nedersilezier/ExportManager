using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExportManager.ViewModels.Windows
{
    public class NewOrderItemCarrierViewModel : BaseViewModel, IParameterReceiver<NewOrderItemCarrierWindowParameter>
    {
        #region Fields
        private int _orderId;
        private PotplantsEntities potplantsEntities;
        private ObservableCollection<KeyAndValue> _CarrierTypes;
        private KeyAndValue _SelectedCarrierType;
        private int _Quantity;
        private bool _IsClosing;
        public event Action CarrierAdded;
        #endregion
        #region Properties
        public int OrderId
        {
            get { return _orderId; }
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;
                    OnPropertyChanged(() => OrderId);
                }
            }
        }
        public ObservableCollection<KeyAndValue> CarrierTypes
        {
            get
            {
                if (_CarrierTypes == null)
                {
                    _CarrierTypes = new CarrierTypesQuery(potplantsEntities).GetCarrierTypesListItems();
                }
                return _CarrierTypes;
            }
            set
            {
                if (_CarrierTypes != value)
                {
                    _CarrierTypes = value;
                    OnPropertyChanged(() => CarrierTypes);
                }
            }
        }
        public KeyAndValue SelectedCarrierType
        {
            get { return _SelectedCarrierType; }
            set
            {
                if (_SelectedCarrierType != value)
                {
                    _SelectedCarrierType = value;
                    OnPropertyChanged(() => SelectedCarrierType);
                }
            }
        }
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged(() => Quantity);
                }
            }
        }
        public bool IsClosing
        {
            get { return _IsClosing; }
            set
            {
                if(_IsClosing != value)
                {
                    _IsClosing = value;
                    OnPropertyChanged(() => IsClosing);
                }
            }
        }
        #endregion
        #region Constructor
        public NewOrderItemCarrierViewModel()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
        #region Commands
        private BaseCommand _AddCarrierCommand;
        public ICommand AddCarrierCommand
        {
            get
            {
                if (_AddCarrierCommand == null)
                    _AddCarrierCommand = new BaseCommand(OnAddCarrier);
                return _AddCarrierCommand;
            }
        }
        #endregion
        #region Functions
        public void SetParameter(NewOrderItemCarrierWindowParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }

            DisplayName = parameter.Title;
            OrderId = parameter.OrderId;
            CarrierAdded += parameter.RefreshEvent;
            OnPropertyChanged(() => DisplayName);
        }
        private void OnAddCarrier()
        {
            if (SelectedCarrierType == null)
            {
                ShowMessageBox("Please select a carrier type.");
                return;
            }
            for(int i = 0; i < Quantity; i++)
            {
                var carrier = new Carriers
                {
                    OrderId = OrderId,
                    CarrierTypeId = SelectedCarrierType.Key,
                    IsActive = true
                };
                potplantsEntities.Carriers.Add(carrier);
            }
            potplantsEntities.SaveChanges();
            CarrierAdded?.Invoke();
            IsClosing = true;
        }
        #endregion
    }
}
