using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class CountriesListView
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string ISO2Code { get; set; }
        public string PhoneCode { get; set; }
        public bool IsEUMember { get; set; }
        public string IsEUMemberToText
        {
            get { return IsEUMember ? "Yes" : "No"; }
        }
        public string Remarks { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
