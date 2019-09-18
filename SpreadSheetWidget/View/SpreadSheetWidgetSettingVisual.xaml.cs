
using System.Windows;

using SpreadSheetWidget.Client.Model;
using System.Windows.Media.Imaging;
using System.IO;

namespace SpreadSheetWidget.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class SpreadSheetWidgetSettingVisual : Window
    {

        SpreadSheetWidgetViewModel _viewModel;
        SpreadSheetWidgetVisual _view;

        public SpreadSheetWidgetSettingVisual(SpreadSheetWidgetViewModel ViewModel, SpreadSheetWidgetVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.chkVisible.IsChecked = _viewModel._ViewerModel.Parameters.Visible;
            this.Path.Text = _viewModel._ViewerModel.Parameters.Path;

          
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

            _viewModel._ViewerModel.Parameters.Visible = this.chkVisible.IsChecked.Value;
            _viewModel._ViewerModel.Parameters.Path = this.Path.Text;
            _view.AssignXML();
        }

      

       
       
    }
}
