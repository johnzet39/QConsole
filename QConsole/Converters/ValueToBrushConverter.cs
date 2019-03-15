using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace QConsole.Converters
{
    class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool input;
            try
            {
                DataGridCell dgc = (DataGridCell)value;
                System.Data.DataRowView rowView = (System.Data.DataRowView)dgc.DataContext;
                input = (bool)rowView.Row.ItemArray[dgc.Column.DisplayIndex];
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }
            switch (input)
            {
                case true: return "#FFE7FFE2";
                case false: return "#FFFFE8E9";
                default: return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
