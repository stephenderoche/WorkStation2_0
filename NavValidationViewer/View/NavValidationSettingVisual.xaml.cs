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
using NavValidationViewer.Client.ViewModel;
using NavValidationViewer.Client.View;

using NavValidationViewer.Client;
using NavValidationViewer.Client.Model;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
namespace NavValidationViewer.Client.View

{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class NavValidationSettingVisual : Window
    {

        NavValidationViewerSettingsViewModel _viewModel;
        NavValidationViewerVisual _view;

        public NavValidationSettingVisual(NavValidationViewerSettingsViewModel ViewModel, NavValidationViewerVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

        //private void Window_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    //_view.XML = this.txtXml.Text;
        //    //_viewModel._ViewerModel.Parameters.XML = this.txtXml.Text;
        //}

        private void cbTheme_EditValueChanged(object send, EditValueChangedEventArgs e)
        {

            if (this.cbTheme.SelectedItem != null)
            {
                ApplicationThemeHelper.ApplicationThemeName = (this.cbTheme.SelectedItem as Theme).Name;
                _view._view.Parameters.DefaultTheme = cbTheme.Text;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbTheme.Text = _view._view.Parameters.DefaultTheme;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view._view.Parameters.DefaultTheme = cbTheme.Text;
        }


    }
}
