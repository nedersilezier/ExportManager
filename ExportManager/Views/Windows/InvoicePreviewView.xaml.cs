using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExportManager.Views.Windows
{
    /// <summary>
    /// Interaction logic for InvoicePreviewView.xaml
    /// </summary>
    public partial class InvoicePreviewView : Window
    {
        private const double MinZoom = 10.0;
        private const double MaxZoom = 500.0;
        private const double ZoomIncrement = 2.5;
        public InvoicePreviewView()
        {
            InitializeComponent();
        }
        private void DocViewer_Loaded(object sender, RoutedEventArgs e)
        {
            // Fit to height
            if (sender is DocumentViewer viewer)
            {
                viewer.FitToMaxPagesAcross(1);
                viewer.GoToPage(1);
            }
        }
        // Zoom commands
        private void IncreaseZoom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (docViewer.Zoom + ZoomIncrement <= MaxZoom)
            {
                docViewer.Zoom += ZoomIncrement;
            }
            e.Handled = true;
        }

        private void DecreaseZoom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (docViewer.Zoom - ZoomIncrement >= MinZoom)
            {
                docViewer.Zoom -= ZoomIncrement;
            }
            e.Handled = true;
        }
    }
}
