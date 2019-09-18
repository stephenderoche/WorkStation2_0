
using System.Windows;

using PMDashBoard.Client.Model;

namespace PMDashBoard.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class PMDashBoardSettingVisual : Window
    {

        PMDashBoardSettingsViewModel _viewModel;
        PMDashBoardVisual _view;

        public PMDashBoardSettingVisual(PMDashBoardSettingsViewModel ViewModel, PMDashBoardVisual View)
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
