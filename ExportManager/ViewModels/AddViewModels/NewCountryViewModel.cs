using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportManager.Models;

using System.Windows.Media.Imaging;

namespace ExportManager.ViewModels.AddViewModels
{
    public class NewCountryViewModel : NewItemViewModel<Countries>
    {
        #region Constructor
        public NewCountryViewModel()
        : base(new[] { "" })
        {
            base.DisplayName = "New country";
            item = new Countries();
        }
        public NewCountryViewModel(int countryId)
        : base(new[] { "" })
        {
            base.DisplayName = "Edit country";
            _IsEditMode = true;
            item = potplantsEntities.Countries.FirstOrDefault(t => t.CountryId == countryId);
        }
        #endregion
        #region Properties
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
        public string ISO2Code
        {
            get { return item.ISO2Code; }
            set
            {
                if (item.ISO2Code != value)
                {
                    item.ISO2Code = value;
                }
                OnPropertyChanged(() => ISO2Code);
            }
        }
        public string PhoneCode
        {
            get { return item.PhoneCode; }
            set
            {
                if (item.PhoneCode != value)
                    item.PhoneCode = value;
                OnPropertyChanged(() => PhoneCode);
            }
        }
        public string Continent
        {
            get { return item.Continent; }
            set
            {
                if(item.Continent != value)
                    item.Continent = value;
                OnPropertyChanged(() => Continent);
            }
        }
        public bool IsEUMember
        {
            get { return item.IsEUMember; }
            set
            {
                if (item.IsEUMember != value)
                    item.IsEUMember = value;
                OnPropertyChanged(() => IsEUMember);
            }
        }
        public string Remarks
        {
            get { return item.Remarks; }
            set
            {
                if (item.Remarks != value)
                    item.Remarks = value;
                OnPropertyChanged(() => Remarks);
            }
        }
        #endregion
        #region Commands
        //public override void Save()
        //{
        //    item.IsActive = true;
        //    potplantsEntities.Countries.Add(item);
        //    potplantsEntities.SaveChanges();
        //    RaiseAdded();
        //}
        #endregion
    }
}
