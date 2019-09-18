
using System.Windows;

using ModelHelper.Client.Model;

namespace ModelHelper.Client.View
{
    /// <summary>
    /// Interaction logic for NavDashboardSettingVisual.xaml
    /// </summary>
    public partial class ModelHelperSettingVisual : Window
    {

        ModelHelperSettingsViewModel _viewModel;
        ModelHelperVisual _view;

        public ModelHelperSettingVisual(ModelHelperSettingsViewModel ViewModel, ModelHelperVisual View)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = View;
            this._viewModel = ViewModel;

            this.txtXml.Text = _viewModel._ViewerModel.Parameters.XML;

          
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view.XML = this.txtXml.Text;
            _viewModel._ViewerModel.Parameters.XML = this.txtXml.Text;
        }

       
    }
}
