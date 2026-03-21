using ExportManager.Models;
using ExportManager.ViewModels;
using ExportManager.ViewModels.ShowAllViewModels;
using ExportManager.ViewModels.Windows;
using ExportManager.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace ExportManager
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CultureInfo culture = new CultureInfo("en-GB");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var services = new ServiceCollection();

            services.AddSingleton<IWindowService, WindowService>();

            services.AddSingleton<PotplantsEntities>();

            // Main window view model
            services.AddTransient<MainWindowViewModel>();

            // Window view models
            services.AddTransient<ImageWindowViewModel>();
            services.AddTransient<NewOrderItemCarrierViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
            MainWindow window = new MainWindow();
            var viewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            window.DataContext = viewModel;
            window.Show();
        }
    }

}
