
using System.Windows;

using DataDashBoard.Client.Model;

namespace DataDashBoard.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class DataDashBoardSettingVisual : Window
    {

        DataDashBoardSettingsViewModel _viewModel;
        DataDashBoardVisual _view;

        public DataDashBoardSettingVisual(DataDashBoardSettingsViewModel ViewModel, DataDashBoardVisual View)
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
