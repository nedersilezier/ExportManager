using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.BusinessLogic;
using ExportManager.Models.BusinessLogic.ListViewsForUI;
using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExportManager.ViewModels.ReportViewModels
{
    public class VolumeReportViewModel : WorkspaceViewModel
    {
        #region Fields
        public DateTime _FromDate;
        public DateTime _ToDate;
        private decimal? _Volume;
        private int _CarrierTypeId;
        private KeyAndValue _SelectedClient;
        private KeyAndValue _SelectedCarrierType;
        private BaseCommand _CalculateVolume;
        #endregion
        #region Database
        public PotplantsEntities potplantsEntities;
        #endregion
        #region Constructor
        public VolumeReportViewModel()
        {
            base.DisplayName = "Volume report";
            potplantsEntities = new PotplantsEntities();
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }
        #endregion
        #region Properties
        public DateTime FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged(() => FromDate);
                }
            }
        }
        public DateTime ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged(() => ToDate);
                }
            }
        }
        public decimal? Volume
        {
            get { return _Volume; }
            set
            {
                if (_Volume != value)
                {
                    _Volume = value;
                    OnPropertyChanged(() => Volume);
                }
            }
        }
        public int CarrierTypeId
        {
            get { return _CarrierTypeId; }
            set
            {
                if( _CarrierTypeId != value )
                {
                    _CarrierTypeId = value;
                    OnPropertyChanged(() =>  CarrierTypeId);
                }
            }
        }
        public ObservableCollection<KeyAndValue> ClientsItems
        {
            get { return new ClientsQuery(potplantsEntities).GetClientsListItems(); }
        }
        public ObservableCollection<KeyAndValue> CarrierTypesItems
        {
            get { return new CarrierTypesQuery(potplantsEntities).GetCarrierTypesListItems(); }
        }
        public KeyAndValue SelectedClient
        {
            get { return _SelectedClient; }
            set
            {
                if (_SelectedClient != value)
                {
                    _SelectedClient = value;
                    OnPropertyChanged(() => SelectedClient);
                }
            }
        }
        public KeyAndValue SelectedCarrierType
        {
            get { return _SelectedCarrierType; }
            set
            {
                if( _SelectedCarrierType != value )
                {
                    _SelectedCarrierType = value;
                    OnPropertyChanged(() => SelectedCarrierType);
                }
            }
        }
        public ICommand CalculateVolume
        {
            get
            {
                if (_CalculateVolume == null)
                    _CalculateVolume = new BaseCommand(calculateVolumeClick);
                return _CalculateVolume;
            }
        }
        #endregion
        #region Functions
        private void calculateVolumeClick()
        {
            if (SelectedClient == null || SelectedCarrierType == null)
                return;
            Volume = new VolumeCalculator(potplantsEntities).CalculateVolumePerClientPerPeriod(SelectedClient.Key, FromDate, ToDate, SelectedCarrierType.Key);
            Console.WriteLine(Volume);
        }
        #endregion
    }
}
