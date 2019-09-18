using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace RebalanceInfoSend.Client
{
    class BooleanToVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool actualValue = (bool)value;

            Visibility result = (parameter == null || parameter.ToString() != "inverse")
                ? (actualValue ? Visibility.Visible : Visibility.Hidden)
                : (actualValue ? Visibility.Hidden : Visibility.Visible);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
