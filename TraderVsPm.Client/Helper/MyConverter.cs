using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TraderVsPM.Client.Helper
{
    public class MyConverter : IValueConverter
    {
        public double MaxHeaderHeight
        {
            get;
            set;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value > MaxHeaderHeight)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
