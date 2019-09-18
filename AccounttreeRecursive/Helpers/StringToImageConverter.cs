using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace AccountTreeCashViewer.View
{
    public class StringToImageConverter : MarkupExtension, IValueConverter {
        public StringToImageConverter() { }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) 
        {
            string iconGroup = "pack://application:,,,/AccountTreeCashViewer.Client;component/Images/Folder_16.png";
            string iconAccount = "pack://application:,,,/AccountTreeCashViewer.Client;component/Images/account.ico";
            if (value.ToString() == "Account Group")
                return new BitmapImage(new Uri(String.Format("{0}", iconGroup), UriKind.RelativeOrAbsolute));
            else
                return new BitmapImage(new Uri(String.Format("{0}", iconAccount), UriKind.RelativeOrAbsolute));
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
        
    }
}
