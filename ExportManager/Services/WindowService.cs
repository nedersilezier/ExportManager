using ExportManager.Views.Windows;
using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExportManager.ViewModels.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show(Type viewModelType, object parameter = null)
        {
            var window = BuildWindow(viewModelType, parameter);
            window.Show();
        }

        public bool? ShowDialog(Type viewModelType, object parameter = null)
        {
            var window = BuildWindow(viewModelType, parameter);
            return window.ShowDialog();
        }

        public void Show<TViewModel>() where TViewModel : class
        {
            Show<TViewModel>(null);
        }

        public void Show<TViewModel>(object parameter) where TViewModel : class
        {
            Show(typeof(TViewModel), parameter);
        }

        public bool? ShowDialog<TViewModel>() where TViewModel : class
        {
            return ShowDialog<TViewModel>(null);
        }

        public bool? ShowDialog<TViewModel>(object parameter) where TViewModel : class
        {
            return ShowDialog(typeof(TViewModel), parameter);
        }

        private Window BuildWindow<TViewModel>(object parameter) where TViewModel : class
        {
            return BuildWindow(typeof(TViewModel), parameter);
        }

        private Window BuildWindow(Type viewModelType, object parameter)
        {
            var viewModel = _serviceProvider.GetRequiredService(viewModelType);
            SetParameterIfSupported(viewModel, parameter);

            var windowType = MapViewModelToWindow(viewModelType);
            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = viewModel;
            return window;
        }

        private static void SetParameterIfSupported(object viewModel, object parameter)
        {
            if (parameter == null || viewModel == null)
            {
                return;
            }

            var receiverInterface = viewModel.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IParameterReceiver<>));

            if (receiverInterface == null)
            {
                return;
            }

            var expectedType = receiverInterface.GetGenericArguments()[0];
            if (!expectedType.IsAssignableFrom(parameter.GetType()))
            {
                throw new InvalidOperationException(
                    $"Cannot pass parameter type {parameter.GetType().Name} to {viewModel.GetType().Name}. Expected {expectedType.Name}.");
            }

            MethodInfo method = receiverInterface.GetMethod("SetParameter");
            method.Invoke(viewModel, new[] { parameter });
        }

        private Type MapViewModelToWindow(Type viewModelType)
        {
            switch (viewModelType)
            {
                case Type t when t == typeof(ImageWindowViewModel):
                    return typeof(ImageWindowView);
                case Type t when t == typeof(NewOrderItemCarrierViewModel):
                    return typeof(NewOrderItemCarrierView);
            }
                //if (viewModelType == typeof(ImageWindowViewModel))
                //return typeof(ImageWindowView);

            throw new Exception($"No window mapped for {viewModelType.Name}");
        }
    }
}
