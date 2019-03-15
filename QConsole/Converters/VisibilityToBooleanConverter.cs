using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace QConsole.Converters
{
    // converter boolean to visibility
    public class VisibilityToBooleanConverter : IValueConverter
    {

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Boolean))
            {
                var visible = System.Convert.ToBoolean(value, culture);
                visible = !visible;
                return visible;
            }
            throw new InvalidOperationException("Converter can only convert to value of type Boolean.");
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Converter cannot convert back.");
        }
    }
}
