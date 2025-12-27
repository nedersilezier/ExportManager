using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models
{
    public interface IHasIsActive
    {
        bool IsActive{ get; set;}
        DateTime? DeletedAt { get; set; }
    }
}
