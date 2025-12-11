using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models
{
    partial class Addresses
    {
        public string FullHouseNumber
        {
            get { return HouseNumber + (ApartmentNumber == null || ApartmentNumber == string.Empty ? string.Empty : "/" + ApartmentNumber); }
        }
        public string FullAddress
        {
            get { return Street + " " + FullHouseNumber + ", " + ZipCode + " " + City + "; " + Countries.Name; }
        }
    }
}
