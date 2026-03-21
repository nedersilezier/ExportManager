using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.Windows
{
    public interface IParameterReceiver<T>
    {
        void SetParameter(T parameter);
    }
}
