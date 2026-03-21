using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.Events
{
    public class WindowRequestEventArgs : EventArgs
    {
        public Type ViewModelType { get; private set; }
        public object Parameter { get; private set; }
        public Action RefreshRequest;

        public WindowRequestEventArgs(Type viewModelType, object parameter = null)
        {
            if (viewModelType == null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            ViewModelType = viewModelType;
            Parameter = parameter;
        }
    }
}
