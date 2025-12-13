using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.EntitiesForView
{
    public class ProductsListView
    {
        public string Name { get; set; }
        public string ColorName{ get; set; }
        public string ColorRemarks { get; set; }
        public decimal? Potsize { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public bool IsCites { get; set; }
        public string Cites 
        { 
            get
            {
                return IsCites == true ? "Yes" : "No";    
            }
        }
        public string CategoryName { get; set; }
        public string CategoryRemarks { get; set; }
        public string Remarks { get; set; }

    }
}
