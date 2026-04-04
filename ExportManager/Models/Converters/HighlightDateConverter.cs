using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ExportManager.Models.Converters
{

    public class HighlightDateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2
                && values[0] is DateTime date
                && values[1] is HashSet<DateTime> highlightedDates)
            {
                return highlightedDates.Contains(date.Date)
                    ? Brushes.LightGreen
                    : Brushes.Transparent;
            }
            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
