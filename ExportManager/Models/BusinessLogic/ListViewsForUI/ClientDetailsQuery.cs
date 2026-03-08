using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.ListViewsForUI
{
    public class ClientDetailsQuery: DatabaseClass
    {
        #region Constructor
        public ClientDetailsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<Clients> GetClientById(int id)
        {
            return potplantsEntities.Clients.Where(c => c.IsActive && c.ClientId == id);
        }
        public IQueryable<ClientsListView> GetClientFullDetailsById(int id)
        {
            return GetClientById(id).Select(client => new ClientsListView
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
            });
        }
        public IQueryable<ClientsListView> GetClientAddressDetailsById(int id)
        {
            return GetClientById(id).Select(client => new ClientsListView
            {
                City = client.Addresses.City,
                Street = client.Addresses.Street,
                HouseNumber = client.Addresses.HouseNumber,
                ApartmentNumber = client.Addresses.ApartmentNumber,
                ZipCode = client.Addresses.ZipCode,
                Country = client.Addresses.Countries.Name
            });
        }
        public IQueryable<KeyAndValue> GetKeyAndValueById(int clientId)
        {
            return GetClientById(clientId).Select(c => new KeyAndValue
            {
                Key = c.ClientId,
                Value = c.Name + "(" + c.ClientCode + ")"
            });
        }
        #endregion
    }
}
