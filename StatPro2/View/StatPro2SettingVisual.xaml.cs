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
using StatPro2.Client.ViewModel;
using StatPro2.Client.View;

using StatPro2.Client;
using StatPro2.Client.Model;

namespace StatPro2.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class StatPro2SettingVisual : Window
    {

        StatPro2SettingsViewModel _viewModel;
        StatPro2Visual _view;

        public StatPro2SettingVisual(StatPro2SettingsViewModel ViewModel, StatPro2Visual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
