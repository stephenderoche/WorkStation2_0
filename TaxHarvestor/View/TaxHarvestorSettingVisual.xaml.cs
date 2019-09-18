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
using TaxHarvestor.Client.ViewModel;
using TaxHarvestor.Client.View;

using TaxHarvestor.Client;
using TaxHarvestor.Client.Model;

namespace TaxHarvestor.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class TaxHarvestorSettingVisual : Window
    {

        TaxHarvestorSettingsViewModel _viewModel;
        TaxHarvestorViewerVisual _view;

        public TaxHarvestorSettingVisual(TaxHarvestorSettingsViewModel ViewModel, TaxHarvestorViewerVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
