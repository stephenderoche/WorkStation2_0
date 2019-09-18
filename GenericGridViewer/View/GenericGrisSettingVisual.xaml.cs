using System;
using System.Collections.Generic;
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
using GenericGrid.Client.ViewModel;
using GenericGrid.Client.View;

using GenericGrid.Client;
using GenericGrid.Client.Model;

namespace GenericGrid.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class GenericGridSettingVisual : Window
    {

        GenericGridViewerSettingsViewModel _viewModel;
        GenericGridViewerVisual _view;

        public GenericGridSettingVisual(GenericGridViewerSettingsViewModel ViewModel, GenericGridViewerVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.txtXml.Text = _viewModel._genericGridViewerModel.Parameters.XML;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view.XML = this.txtXml.Text;
            _viewModel._genericGridViewerModel.Parameters.XML = this.txtXml.Text;
        }
    }
}
