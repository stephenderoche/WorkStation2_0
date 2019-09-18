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
using DevExpress.Xpf.Utils.Themes;
using DevExpress.Xpf.Data;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Settings;

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

            cbTheme.Text = _view._vm.Parameters.DefaultTheme;

          
        }

        private void cbTheme_EditValueChanged(object send, EditValueChangedEventArgs e)
        {

            if (this.cbTheme.SelectedItem != null)
            {
                ApplicationThemeHelper.ApplicationThemeName = (this.cbTheme.SelectedItem as Theme).Name;
                _view._vm.Parameters.DefaultTheme = cbTheme.Text;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           cbTheme.Text = _view._vm.Parameters.DefaultTheme;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view._vm.Parameters.DefaultTheme = cbTheme.Text;
        }
       
    }
}
