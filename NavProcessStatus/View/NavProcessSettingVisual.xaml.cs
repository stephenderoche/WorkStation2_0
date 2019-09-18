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
using NavProcess.Client.ViewModel;
using NavProcess.Client.View;

using NavProcess.Client;
using NavProcess.Client.Model;

namespace NavProcess.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class NavProcessSettingVisual : Window
    {

        NavProcessSettingsViewModel _viewModel;
        NavProcessViewerVisual _view;

        public NavProcessSettingVisual(NavProcessSettingsViewModel ViewModel, NavProcessViewerVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
