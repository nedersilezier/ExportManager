using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ExportManager.Models.Converters
{
    public class ProfitToFontColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal profit)
            {
                if (profit < 0)
                {
                    return Brushes.Red;
                }
                else if (profit > 0)
                {
                    return Brushes.Green;
                }
            }
            return Brushes.Black;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
