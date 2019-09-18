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
using ExceptionManagement.Client.ViewModel;
using ExceptionManagement.Client.View;

using ExceptionManagement.Client;
using ExceptionManagement.Client.Model;

namespace ExceptionManagement.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class DriftToolSettingVisual : Window
    {

        DriftToolViewerSettingsViewModel _viewModel;
        DriftToolViewerVisual _view;

        public DriftToolSettingVisual(DriftToolViewerSettingsViewModel ViewModel, DriftToolViewerVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
