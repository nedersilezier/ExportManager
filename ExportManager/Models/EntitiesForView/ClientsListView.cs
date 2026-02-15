using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class ClientsListView
    {
        public int ClientId {  get; set; }
        public string ClientName { get; set; }
        public string ClientCode {  get; set; }
        public string ContactPerson {  get; set; }
        public string PhoneNumber {  get; set; }
        public string EmailAddress { get; set; }
        public string City {  get; set; }
        public string Street {  get; set; }
        public string HouseNumber {  get; set; }
        public string ApartmentNumber {  get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string TaxId {  get; set; }
        public string RegistrationNumber {  get; set; }
        public string Remarks {  get; set; }
        public string DisplayName
        {
            get
            {
                return ClientCode + ", " + Country;
            }
        }
        public string FullHouseNumber
        {
            get { return HouseNumber + (ApartmentNumber == null || ApartmentNumber == string.Empty ? string.Empty : "/" + ApartmentNumber); }
        }
    }
}
