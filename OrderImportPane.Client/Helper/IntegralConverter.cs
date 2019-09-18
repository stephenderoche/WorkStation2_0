using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace OrderImportPane.Client.Helper
{
    [ValueConversion(typeof(long), typeof(string))]
    public class IntegralConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value != null) && (!object.Equals(string.Empty, value)))
            {
                try
                {
                    // Convert the string value provided by an editor to a long before formatting.
                    long tempLong = System.Convert.ToInt64(value, CultureInfo.CurrentCulture);
                    return string.Format(CultureInfo.CurrentCulture, "{0:#,##0}", tempLong);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
            }

            return string.Format(CultureInfo.CurrentCulture, "{0}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return long.Parse(value as string, NumberStyles.Integer, CultureInfo.CurrentCulture);
        }
    }
}