using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ITServiceApp.UI.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd.MM.yyyy");
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParse(dateString, out DateTime result))
                {
                    return result;
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
