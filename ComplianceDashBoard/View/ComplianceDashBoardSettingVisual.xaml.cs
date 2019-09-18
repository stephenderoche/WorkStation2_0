
using System.Windows;

using ComplianceDashBoard.Client.Model;
using DevExpress.Xpf.Utils.Themes;
using DevExpress.Xpf.Data;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Settings;

namespace ComplianceDashBoard.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class ComplianceDashBoardSettingVisual : Window
    {

        ComplianceDashBoardSettingsViewModel _viewModel;
        ComplianceDashBoardVisual _view;

        public ComplianceDashBoardSettingVisual(ComplianceDashBoardSettingsViewModel ViewModel, ComplianceDashBoardVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            cbTheme.Text = _view._view.Parameters.DefaultTheme;
          
        }

        private void cbTheme_EditValueChanged(object send, EditValueChangedEventArgs e)
        {

            if (this.cbTheme.SelectedItem != null)
            {
                ApplicationThemeHelper.ApplicationThemeName = (this.cbTheme.SelectedItem as Theme).Name;
                _view._view.Parameters.DefaultTheme = cbTheme.Text;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbTheme.Text = _view._view.Parameters.DefaultTheme;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view._view.Parameters.DefaultTheme = cbTheme.Text;
        }

       
    }
}
