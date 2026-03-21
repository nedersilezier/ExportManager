using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.Windows
{
    public class NewOrderItemCarrierViewModel : BaseViewModel, IParameterReceiver<NewOrderItemCarrierWindowParameter>
    {
        #region Fields
        private int _orderId;
        #endregion
        #region Properties
        public int OrderId
        {
            get { return _orderId; }
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;
                    OnPropertyChanged(() => OrderId);
                }
            }
        }
        #endregion
        #region Constructor
        #endregion

        public void SetParameter(NewOrderItemCarrierWindowParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }

            DisplayName = parameter.Title;
            OrderId = parameter.OrderId;
            OnPropertyChanged(() => DisplayName);
        }
    }
}
