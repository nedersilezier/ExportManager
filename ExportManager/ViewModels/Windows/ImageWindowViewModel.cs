using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.Windows
{
    public class ImageWindowViewModel : BaseViewModel, IParameterReceiver<ImageWindowParameter>
    {
        #region Fields
        private byte[] _ImageData;
        #endregion
        #region Properties
        public byte[] ImageData
        {
            get { return _ImageData; }
            set
            {
                if (_ImageData != value)
                {
                    _ImageData = value;
                    OnPropertyChanged(() => ImageData);
                }
            }
        }
        #endregion
        #region Constructor
        //public ImageWindowViewModel()
        //    : base()
        //{
        //    base.DisplayName = "Image window";
        //}
        //public ImageWindowViewModel(string productName, byte[] imageData)
        //    : base()
        //{
        //    base.DisplayName = productName;
        //    _ImageData = imageData;
        //}
        #endregion

        public void SetParameter(ImageWindowParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }

            DisplayName = parameter.Title;
            ImageData = parameter.ImageData;
            OnPropertyChanged(() => DisplayName);
        }
    }
}
