using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TraderVsPM.Client.ViewModel;

namespace TraderVsPM.Client
{
    class ButtonLabelConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            bool actualValue = (bool) value;

            Visibility result = (parameter == null)
                ? (actualValue ? Visibility.Visible : Visibility.Hidden)
                : (actualValue ? Visibility.Hidden : Visibility.Visible);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class ButtonLabelConverterReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            bool actualValue = (bool)value;

            Visibility result = (parameter == null)
                ? (actualValue ? Visibility.Hidden : Visibility.Visible)
                : (actualValue ? Visibility.Visible : Visibility.Hidden);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
