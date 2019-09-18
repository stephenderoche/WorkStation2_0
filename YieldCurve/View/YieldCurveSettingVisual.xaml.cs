
using System.Windows;

using YieldCurve.Client.Model;

namespace YieldCurve.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class YieldCurveSettingVisual : Window
    {

        YieldCurveSettingsViewModel _viewModel;
        YieldCurveVisual _view;

        public YieldCurveSettingVisual(YieldCurveSettingsViewModel ViewModel, YieldCurveVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.txtXml.Text = _viewModel._ViewerModel.Parameters.XML;

          
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
           
            _viewModel._ViewerModel.Parameters.XML = this.txtXml.Text;
        }

       
    }
}
