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
using CashFlow.Client.ViewModel;
using CashFlow.Client.View;

using CashFlow.Client;
using CashFlow.Client.Model;

namespace CashFlow.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class CashFlowSettingVisual : Window
    {

        CashFlowSettingsViewModel _viewModel;
        CashFlowVisual _view;

        public CashFlowSettingVisual(CashFlowSettingsViewModel ViewModel, CashFlowVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.txtXml.Text = _viewModel._genericGridViewerModel.Parameters.XML;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view.XML = this.txtXml.Text;
            _viewModel._genericGridViewerModel.Parameters.XML = this.txtXml.Text;
        }
    }
}
