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
using Reporting.Client.ViewModel;
using Reporting.Client.View;

using Reporting.Client;
using Reporting.Client.Model;

namespace Reporting.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class GenericGridSettingVisual : Window
    {

        ReportingSettingsViewModel _viewModel;
        ReportingVisual _view;

        public GenericGridSettingVisual(ReportingSettingsViewModel ViewModel, ReportingVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
          
          
        }
    }
}
