using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.Windows
{
    public interface IWindowService
    {
        void Show(Type viewModelType, object parameter = null);
        bool? ShowDialog(Type viewModelType, object parameter = null);
        void Show<TViewModel>() where TViewModel : class;
        void Show<TViewModel>(object parameter) where TViewModel : class;
        bool? ShowDialog<TViewModel>() where TViewModel : class;
        bool? ShowDialog<TViewModel>(object parameter) where TViewModel : class;
    }
}
