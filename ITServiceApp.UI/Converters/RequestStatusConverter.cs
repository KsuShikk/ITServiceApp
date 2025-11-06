using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.UI.Converters
{
    public class RequestStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RequestStatus status)
            {
                switch (status)
                {
                    case RequestStatus.Created:
                        return "Создана";
                    case RequestStatus.InProgress:
                        return "В процессе";
                    case RequestStatus.WaitingForParts:
                        return "Ожидание запчастей";
                    case RequestStatus.Completed:
                        return "Завершена";
                    default:
                        return status.ToString();
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
