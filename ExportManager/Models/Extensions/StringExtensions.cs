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
    partial class InvoiceItems
    {
        public string FullName
        {
            get
            {
                var details = new List<string>();
                if (PotSizeSnapshot != null)
                    details.Add($"Pot: {PotSizeSnapshot.Value} cm");
                if (HeightSnapshot != null)
                    details.Add($"H: {HeightSnapshot.Value} cm");
                return details.Count > 0 ? $"{NameSnapshot} ({string.Join(", ", details)})" : NameSnapshot;
            }
        }
    }
}
