
using System.Windows;

using NavContingentDashBoard.Client.Model;

namespace NavContingentDashBoard.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class ModelHelperSettingVisual : Window
    {

        NavContingentDashBoardSettingsViewModel _viewModel;
        NavContingentDashBoardVisual _view;

        public ModelHelperSettingVisual(NavContingentDashBoardSettingsViewModel ViewModel, NavContingentDashBoardVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.txtXml.Text = _viewModel._ViewerModel.Parameters.XML;

          
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
           
        }

       
    }
}
