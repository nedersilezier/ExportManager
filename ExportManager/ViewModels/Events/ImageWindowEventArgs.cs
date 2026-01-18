using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExportManager.ViewModels.Events
{
    public class ImageWindowEventArgs: EventArgs
    {
        public byte[] ImageData { get; private set; }
        public string Title { get; private set; }
        public ImageWindowEventArgs(byte[] imageData, string title)
        {
            ImageData = imageData;
            Title = title;
        }
    }
}
