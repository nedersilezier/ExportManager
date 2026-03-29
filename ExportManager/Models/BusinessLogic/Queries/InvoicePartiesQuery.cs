using ExportManager.Models.DTO;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.Commands
{
    public class InvoicePartiesQuery:DatabaseClass
    {
        #region Constructor
        public InvoicePartiesQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public InvoicePartyListView GetFromClient(int clientId)
        {
            return potplantsEntities.Clients.Where(c => c.ClientId == clientId).Select(c => new
            {
                c.Name,
                c.TaxId,
                c.Addresses
            }).AsEnumerable().Select(c => new InvoicePartyListView
            {
                Name = c.Name,
                TaxId = c.TaxId,
                City = c.Addresses.City,
                Street = c.Addresses.Street,
                FullHouseNumber = c.Addresses.FullHouseNumber,
                PostalCode = c.Addresses.ZipCode,
                Country = c.Addresses.Countries.Name,
                CountryCode = c.Addresses.Countries.ISO2Code,
                Role = InvoicePartyRoles.Buyer
            }).FirstOrDefault();
        }
        #endregion
    }
}
