using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.Windows
{
    public class EditCarrierAddonsViewModel: BaseViewModel, IParameterReceiver<OrderItemCarrierParameter>
    {
        #region Fields
        private int _carrierId;
        private int _AmountOfShelves;
        private int _AmountOfExtensions;
        private PotplantsEntities potplantsEntities;
        public event Action CarrierEdited;
        private bool _IsClosing;
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
        public int AmountOfShelves
        {
            get { return _AmountOfShelves;  }
            set
            {
                if (_AmountOfShelves != value)
                {
                    _AmountOfShelves = value;
                    OnPropertyChanged(() => AmountOfShelves);
                }
            }
        }
        public int AmountOfExtensions
        {
            get { return _AmountOfExtensions; }
            set
            {
                if(_AmountOfExtensions != value)
                {
                    _AmountOfExtensions = value;
                    OnPropertyChanged(() => AmountOfExtensions);
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
        public EditCarrierAddonsViewModel()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
        #region Commands
        private BaseCommand _EditCarrierCommand;
        public ICommand EditCarrierCommand
        {
            get
            {
                if (_EditCarrierCommand == null)
                    _EditCarrierCommand = new BaseCommand(OnEditCarrier);
                return _EditCarrierCommand;
            }
        }
        #endregion
        #region Functions
        public void SetParameter(OrderItemCarrierParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }

            DisplayName = parameter.Title;
            CarrierId = parameter.CarrierId;
            AmountOfShelves = parameter.Shelves;
            AmountOfExtensions = parameter.Extensions;
            CarrierEdited += parameter.RefreshEvent;
            OnPropertyChanged(() => DisplayName);
        }
        private void OnEditCarrier()
        {
            var carrier = potplantsEntities.Carriers.FirstOrDefault(c => c.CarrierId == CarrierId);
            carrier.AmountOfShelfs = AmountOfShelves;
            carrier.AmountOfExtensions = AmountOfExtensions;
            potplantsEntities.SaveChanges();
            CarrierEdited?.Invoke();
            IsClosing = true;
        }
        #endregion
    }
}
