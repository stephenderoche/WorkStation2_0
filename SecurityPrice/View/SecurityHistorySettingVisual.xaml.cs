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
using SecurityHistory.Client.ViewModel;
using SecurityHistory.Client.View;

using SecurityHistory.Client;
using SecurityHistory.Client.Model;

namespace SecurityHistory.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class SecurityHistorySettingVisual : Window
    {

        SecurityHistorySettingsViewModel _viewModel;
        SecurityHistoryVisual _view;

        public SecurityHistorySettingVisual(SecurityHistorySettingsViewModel ViewModel, SecurityHistoryVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
