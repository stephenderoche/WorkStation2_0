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
using Guages.Client.ViewModel;
using Guages.Client.View;
using Guages.Client.Model;
using System.Threading;
using SalesSharedContracts;
using Linedata.Framework.Foundation;
using System.Data;

using Linedata.Shared.Api.ServiceModel;
using Linedata.Client.Workstation.SharedReferences;


namespace Guages.Client.View
{
    /// <summary>
    /// Interaction logic for TopSecuritiesViewerSettingViewModel.xaml
    /// </summary>
    public partial class GuagesSettingView : Window
    {
        GuagesSettingViewModel _viewModel;
        GuagesView _view;


        public GuagesSettingView(GuagesSettingViewModel viewModel, GuagesView view)
        {
            InitializeComponent();
            
            this.DataContext = viewModel;
            this._viewModel = viewModel;
            this._view = view;

            GaugeTypeList();

            this.cmboGaugeType.Text = _viewModel._viewerModel.Parameters.GuageType.ToString();

            this.txtHigh.Text = _viewModel._viewerModel.Parameters.High.ToString();

            this.ChkReverse.IsChecked = _viewModel._viewerModel.Parameters.Reverse;
           
           
            
           // this._viewModel = viewModel;
        }

      

       

      

        private void cmboView_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel._viewerModel.Parameters.GuageType = this.cmboGaugeType.Text;
            _viewModel._viewerModel.Parameters.High = Convert.ToInt32(this.txtHigh.Text);
            _viewModel._viewerModel.Parameters.Reverse = this.ChkReverse.IsChecked.Value;


            _view.Gaugetype = this.cmboGaugeType.Text;
            _view.High = Convert.ToInt32(this.txtHigh.Text);
            _view.Reverse = this.ChkReverse.IsChecked.Value;
            _view.setform();
        }

        private ISalesSharedContracts _dbservice;
        public ISalesSharedContracts DBService
        {
            set { _dbservice = value; }
            get { return _dbservice; }
        }

        public ISalesSharedContracts CreateServiceClient()
        {
            try
            {
                DBService = ApiAccessor.Get<ISalesSharedContracts>();
                return DBService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GaugeTypeList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboGaugeType.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                   
                    ApplicationMessageList messages = null;

                    DataSet ds = _viewModel._viewerModel.DBService.se_get_gauge_type(out messages);
    
                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object gauge_type = row["gauge_type"];
                            object gauge_type_id = row["gauge_type_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboGaugeType.Items.Add(new ComboBoxItem(Convert.ToString(gauge_type), Convert.ToInt32(gauge_type_id)));

                                count = count + 1;

                                if (_viewModel._viewerModel.Parameters.GuageType.ToString() == Convert.ToString(gauge_type))
                                    this.cmboGaugeType.SelectedIndex = count;



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }


                    //this.DataContext = _view;


                });
        }

      

      

      
    }
}
