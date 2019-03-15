using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace QConsole.Converters
{
    // converter boolean to visibility
    public class BooleanToBrushConverter : IValueConverter
    {

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush))
            {
                return (bool)value ? new SolidColorBrush(Color.FromRgb(55, 255, 55)) : new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            throw new InvalidOperationException("Converter can only convert to value of type Brush.");
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Converter cannot convert back.");
        }
    }
}
