
using System.Windows;

using FIDashBoard.Client.Model;

namespace FIDashBoard.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class FIDashBoardSettingVisual : Window
    {

        FIDashBoardSettingsViewModel _viewModel;
        FIDashBoardVisual _view;

        public FIDashBoardSettingVisual(FIDashBoardSettingsViewModel ViewModel, FIDashBoardVisual View)
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
