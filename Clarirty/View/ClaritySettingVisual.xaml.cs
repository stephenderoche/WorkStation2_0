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
using Clarity.Client.ViewModel;
using Clarity.Client.View;

using Clarity.Client;
using Clarity.Client.Model;

namespace Clarity.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class ClaritySettingVisual : Window
    {

        ClaritySettingsViewModel _viewModel;
        ClarityVisual _view;

        public ClaritySettingVisual(ClaritySettingsViewModel ViewModel, ClarityVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
