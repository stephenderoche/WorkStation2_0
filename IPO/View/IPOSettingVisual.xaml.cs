
using System.Windows;

using IPO.Client.Model;

namespace IPO.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class IPOSettingVisual : Window
    {

        IPOSettingsViewModel _viewModel;
        IPOVisual _view;

        public IPOSettingVisual(IPOSettingsViewModel ViewModel, IPOVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.txtXml.Text = _viewModel._ViewerModel.Parameters.XML;

          
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view.XML = this.txtXml.Text;
            _viewModel._ViewerModel.Parameters.XML = this.txtXml.Text;
        }

       
    }
}
