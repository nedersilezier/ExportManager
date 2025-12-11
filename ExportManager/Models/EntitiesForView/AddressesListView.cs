using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class AddressesListView
    {
        public string Country {  get; set; }
        public string City {  get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber {  get; set; }
        public string ZipCode {  get; set; }
        public string Remarks {  get; set; }
        public string CountryISO2Code { get; set; }
        public string CountryPhoneCode { get; set; }
        public string Continent {  get; set; }
    }
}
