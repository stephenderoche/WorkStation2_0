namespace Linedata.Framework.Client.TestWebBrowserWidget.View
{
    using System.Windows;
    using System.Windows.Controls;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.Client.TestWebBrowserWidget.Helpers;
    using Linedata.Framework.Client.TestWebBrowserWidget.Plugin;

    /// <summary>
    /// Interaction logic for PluginSettingsDialog.xaml
    /// </summary>
    public partial class PluginSettingsDialog : Window
    {
        private WebBrowserParameters parameters;

        public PluginSettingsDialog(WebBrowserParameters parameters)
        {
            ExceptionsHelper.CheckIsNull(parameters, "parameters");

            this.Parameters = parameters;            
            this.InitializeComponent();
            this.titleTextBox.Text = this.Parameters.Title;
            this.urlTextBox.Text = this.Parameters.URL;
            this.DataContext = this.parameters;
        }

        public WebBrowserParameters Parameters
        {
            get
            {
                return this.parameters;
            }

            private set
            {
                ExceptionsHelper.CheckIsNull(value, "value");
                this.parameters = value;
            }
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.Parameters.Title = this.titleTextBox.Text;

       

            this.Parameters.URL = WebBrowserMvvmHelper.NormalizeUrl(this.urlTextBox.Text);

            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.titleTextBox.Focus();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.titleTextBox.Text) && !string.IsNullOrWhiteSpace(this.urlTextBox.Text))
            {
                this.okButton.IsEnabled = WebBrowserMvvmHelper.VerifyUrlFormat(this.urlTextBox.Text);
            }
            else
            {
                // either both or neither textbox is empty
                this.okButton.IsEnabled = string.IsNullOrWhiteSpace(this.titleTextBox.Text) && string.IsNullOrWhiteSpace(this.urlTextBox.Text);
            }
        }
    }
}
