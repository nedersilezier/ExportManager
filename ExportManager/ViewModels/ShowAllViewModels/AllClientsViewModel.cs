using ExportManager.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ExportManager.Models.EntitiesForView;
using ExportManager.ViewModels.AddViewModels;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllClientsViewModel: AllViewModel<dynamic>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<dynamic>(
                from client in potplantsEntities.Clients
                where client.IsActive == true
                select new ClientsListView
                {
                    ClientId = client.ClientId,
                    ClientName = client.Name,
                    ClientCode = client.ClientCode,
                    ContactPerson = client.ContactPerson,
                    PhoneNumber = client.Phone,
                    EmailAddress = client.Email,
                    City = client.Addresses.City,
                    Street = client.Addresses.Street,
                    HouseNumber = client.Addresses.HouseNumber,
                    ApartmentNumber = client.Addresses.ApartmentNumber,
                    ZipCode = client.Addresses.ZipCode,
                    Country = client.Addresses.Countries.Name,
                    TaxId = client.TaxId,
                    RegistrationNumber = client.RegistrationNo,
                    Remarks = client.Remarks
                }
                );
        }
        #endregion
        #region Constructor
        public AllClientsViewModel()
            : base()
        {
            base.DisplayName = "Clients";
        }
        #endregion
        #region Commands
        //to be filled
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewClientViewModel(), Load);
        }
        public override void OnEdit()
        {
            OpenNewTab(() => new NewClientViewModel(SelectedItem.ClientId), Load);
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
    }
}
