using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.Parameters
{
    public class ImageWindowParameter
    {
        public byte[] ImageData { get; private set; }
        public string Title { get; private set; }
        public ImageWindowParameter(byte[] imageData, string title)
        {
            ImageData = imageData;
            Title = title;
        }
    }
}
