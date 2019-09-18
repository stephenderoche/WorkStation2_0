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
using Replacement.Client.ViewModel;
using Replacement.Client.View;

using Replacement.Client;
using Replacement.Client.Model;

namespace Replacement.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class ReplacementSettingVisual : Window
    {

        ReplacementSettingsViewModel _viewModel;
        ReplacementVisual _view;

        public ReplacementSettingVisual(ReplacementSettingsViewModel ViewModel, ReplacementVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

          
        }

       
    }
}
