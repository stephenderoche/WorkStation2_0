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
using PythonCloud.Client.ViewModel;
using PythonCloud.Client.View;

using PythonCloud.Client;
using PythonCloud.Client.Model;

namespace PythonCloud.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class PythonCloudSettingVisual : Window
    {

        PythonCloudSettingsViewModel _viewModel;
        PythonCloudVisual _view;

        public PythonCloudSettingVisual(PythonCloudSettingsViewModel ViewModel, PythonCloudVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
