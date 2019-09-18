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
using AccountTreeCashViewer.ViewModel;
using AccountTreeCashViewer.View;
using AccountTreeCashViewer.Model;
using System.Threading;
using SalesSharedContracts;
using Linedata.Framework.Foundation;
using System.Data;
using Linedata.Shared.Api.ServiceModel;
using DevExpress.Xpf.Utils.Themes;
using DevExpress.Xpf.Data;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Settings;



namespace AccountTreeCashViewer.View
{
    /// <summary>
    /// Interaction logic for TopSecuritiesViewerSettingViewModel.xaml
    /// </summary>
    public partial class AccountTreeCashViewerSettingView : Window
    {
        AccountTreeCashSettingViewModel _viewModel;
        AccountTreeView _view;


        public AccountTreeCashViewerSettingView(AccountTreeCashSettingViewModel viewModel, AccountTreeView view)
        {
            InitializeComponent();
           
            this.DataContext = viewModel;
            this._viewModel = viewModel;
            this._view = view;

           
          
           
           
            
           // this._viewModel = viewModel;
        }

        private ISalesSharedContracts _dbservice;
        public ISalesSharedContracts DBService
        {
            set { _dbservice = value; }
            get { return _dbservice; }
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
