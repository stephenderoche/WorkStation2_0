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
using RebalanceInfo.Client.ViewModel;
using RebalanceInfo.Client.View;

using RebalanceInfo.Client;
using RebalanceInfo.Client.Model;

namespace RebalanceInfo.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class LinkCashSettingVisual : Window
    {

        ReblanceInfoSettingsViewModel _viewModel;
        RebalanceInfoVisual _view;

        public LinkCashSettingVisual(ReblanceInfoSettingsViewModel ViewModel, RebalanceInfoVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
