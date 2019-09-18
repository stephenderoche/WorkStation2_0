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
using GenericChart;
using System.Threading;
using SalesSharedContracts;
using Linedata.Framework.Foundation;
using System.Data;

using Linedata.Shared.Api.ServiceModel;


namespace GenericChart
{
    /// <summary>
    /// Interaction logic for TopSecuritiesViewerSettingViewModel.xaml
    /// </summary>
    public partial class GenericChartSettingView : Window
    {
        GenericChartViewModel _viewModel;
        GenericChartView _view;


        public GenericChartSettingView(GenericChartViewModel viewModel, GenericChartView view)
        {
            InitializeComponent();
           
            this.DataContext = viewModel;
            this._viewModel = viewModel;
            this._view = view;
            this.cmboView1.Text = viewModel.Parameters.Visibilty;
            this.cmboDataType.Text = viewModel.Parameters.DataType;
           HierarchyList();
           
            
          
        }

        private int _hierarchyId = -1;
        public int HierarchyId
        {
            get { return _hierarchyId; }
            set { _hierarchyId = value; }
        }
        private string _hierarchyName = "Major Asset";
        public string HierarchyName
        {
            get { return _hierarchyName; }
            set { _hierarchyName = value; }
        }
        private string _dt;
        public string DT
        {
            get { return _dt; }
            set { _dt = value; }
        }
        private void cmboView_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _view.cmboView.Text = cmboView1.Text;
            _view.ChartType = cmboDataType.Text;

            _viewModel.Parameters.DataType = cmboDataType.Text;
            _view.lblheader.Content = cmboDataType.Text;
            if (cmboDataType.SelectedIndex == 3)
            {
           
                _view.lblhierarchy.Content = (cmboHierarchy.SelectedItem).ToString();
                _view.HierarchyName = (cmboHierarchy.SelectedItem).ToString();
                _viewModel.Parameters.Hierarchy = (cmboHierarchy.SelectedItem).ToString();
            }
            else
            {
                _view.lblhierarchy.Content = string.Empty;
            }

         
            _view.Securitieschart();
            _view.setGrids();
            _view.setcolor();
        }
        public void HierarchyList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboHierarchy.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    
                    ApplicationMessageList messages = null;



                    DataSet ds = _viewModel.DBService.se_get_hierarchy(out messages);
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["name"];
                            object hierarchy_id = row["hierarchy_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboHierarchy.Items.Add(new ComboBoxItem(Convert.ToString(item), Convert.ToInt64(hierarchy_id)));

                                count = count + 1;

                                if (_hierarchyName == Convert.ToString(item))
                                    this.cmboHierarchy.SelectedIndex = count;



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }


                    //this.DataContext = _view;


                });
        }
        private void window_loaded(object sender, RoutedEventArgs e)
        {

                DT = cmboDataType.Text;
                _view.savexml(_viewModel.Parameters.DataType);
        }

      
    }
}
