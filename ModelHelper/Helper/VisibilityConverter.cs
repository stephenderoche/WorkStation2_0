using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.GroupRowLayout;

namespace ModelHelper.Client
{


    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var data = value as GroupRowData;

            if (value.GetType() == typeof(Int32))
            {
                if ((Int32)value > 1)
                {
                    return true;
                }
            }
            else if (value.GetType() == typeof(string))
            {
                var str = value.ToString();
                var info = str.IndexOf("=");
                int val;
                Int32.TryParse(str.Substring(info + 1), out val);
                if (val > 1)
                {
                    return true;
                }
            }

            else if (data != null)
            {
                var str = data.GroupSummaryData[0].Text.ToString();
                var info = str.IndexOf("=");
                int val;
                Int32.TryParse(str.Substring(info + 1), out val);
                if (val > 1)
                {
                    return Brushes.Green;
                }
                else return Brushes.LightBlue;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class VisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visible = (bool)value;
            return visible ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visible = (Visibility)value;
            return visible == Visibility.Visible;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }


    public class ReverseVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visible = (Visibility)value;
            return visible == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
