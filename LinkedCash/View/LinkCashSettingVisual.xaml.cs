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
using LinkCash.Client.ViewModel;
using LinkCash.Client.View;

using LinkCash.Client;
using LinkCash.Client.Model;

namespace LinkCash.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class LinkCashSettingVisual : Window
    {

        LinkCashSettingsViewModel _viewModel;
        LinkCashVisual _view;

        public LinkCashSettingVisual(LinkCashSettingsViewModel ViewModel, LinkCashVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
