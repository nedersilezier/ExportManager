using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class GrowersListView
    {
        public int GrowerId { get; set; }
        public string GrowerName { get; set; }
        public string Cultivations {  get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string TaxId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Remarks { get; set; }
    }
}
