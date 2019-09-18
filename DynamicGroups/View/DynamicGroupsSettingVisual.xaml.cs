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
using DynamicGroups.Client.ViewModel;
using DynamicGroups.Client.View;

using DynamicGroups.Client;
using DynamicGroups.Client.Model;

namespace DynamicGroups.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class DynamicGroupsSettingVisual : Window
    {

        DynamicGroupsSettingsViewModel _viewModel;
        DynamicGroupsVisual _view;

        public DynamicGroupsSettingVisual(DynamicGroupsSettingsViewModel ViewModel, DynamicGroupsVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
