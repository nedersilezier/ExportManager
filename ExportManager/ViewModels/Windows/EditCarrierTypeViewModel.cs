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
    public class EditCarrierTypeViewModel : BaseViewModel, IParameterReceiver<CarrierTypeParameter>
    {
        #region Fields
        private int _carrierId;
        private PotplantsEntities potplantsEntities;
        private Carriers carrier;
        private ObservableCollection<KeyAndValue> _CarrierTypes;
        private KeyAndValue _SelectedCarrierType;
        private bool _IsClosing;
        public event Action CarrierEdited;
        #endregion
        #region Properties
        public int CarrierId
        {
            get { return _carrierId; }
            set
            {
                if (_carrierId != value)
                {
                    _carrierId = value;
                    OnPropertyChanged(() => CarrierId);
                }
            }
        }
        public ObservableCollection<KeyAndValue> CarrierTypes
        {
            get
            {
                if (_CarrierTypes == null)
                {
                    _CarrierTypes = new CarrierTypesForReports(potplantsEntities).GetCarrierTypesListItems();
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
            get
            {
                return _SelectedCarrierType;
            }
            set
            {
                if (_SelectedCarrierType != value)
                {
                    _SelectedCarrierType = value;
                    OnPropertyChanged(() => SelectedCarrierType);
                }
            }
        }
        public bool IsClosing
        {
            get { return _IsClosing; }
            set
            {
                if (_IsClosing != value)
                {
                    _IsClosing = value;
                    OnPropertyChanged(() => IsClosing);
                }
            }
        }
        #endregion
        #region Constructor
        public EditCarrierTypeViewModel()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
        #region Commands
        private BaseCommand _EditCarrierTypeCommand;
        public ICommand EditCarrierTypeCommand
        {
            get
            {
                if (_EditCarrierTypeCommand == null)
                    _EditCarrierTypeCommand = new BaseCommand(OnEditCarrier);
                return _EditCarrierTypeCommand;
            }
        }
        #endregion
        #region Functions
        public void SetParameter(CarrierTypeParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }

            DisplayName = parameter.Title;
            CarrierId = parameter.CarrierId;
            SelectedCarrierType = CarrierTypes.FirstOrDefault(ct => ct.Value == parameter.CarrierTypeName);
            CarrierEdited += parameter.RefreshEvent;
            OnPropertyChanged(() => DisplayName);
        }
        private void OnEditCarrier()
        {
            if (SelectedCarrierType == null)
            {
                ShowMessageBox("Please select a carrier type.");
                return;
            }
            carrier = potplantsEntities.Carriers.FirstOrDefault(c => c.CarrierId == CarrierId);
            carrier.CarrierTypeId = SelectedCarrierType.Key;
            potplantsEntities.SaveChanges();
            CarrierEdited?.Invoke();
            IsClosing = true;
        }
        #endregion
    }
}
