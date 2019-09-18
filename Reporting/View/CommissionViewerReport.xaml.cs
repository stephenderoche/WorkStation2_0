using System;
using System.Data;
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
using Linedata.Framework.Foundation;
using Linedata.Shared.Api.ServiceModel;

using System.Threading;
using SalesSharedContracts;
using Linedata.Framework.WidgetFrame.PluginBase;

//using Linedata.Shared.CoreDataServices;


namespace Reporting.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataSet _ds;
        private int _report_type;
      

        public MainWindow(DataSet DS,int ReportType)
        {
            this._ds = DS;
            this._report_type = ReportType;
            InitializeComponent();
        }

     

    


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


          ////  update_report_datasource();
          //  DataSet ds = new DataSet();
          //  ApplicationMessageList messages = null;
          //  IReportingServiceContract dbService = this.CreateServiceClient();
          //  ds = dbService.se_rpx_proposed_orders(
          //  m_account_id,
          //  out messages);


          

            if (_report_type == 2)
            {
                ReportByBrokerReason report = new ReportByBrokerReason();
                report.DataSource = _ds;
                documentPreview.DocumentSource = report;
                report.CreateDocument();
            }

            if (_report_type == 1)
            {
                ReportByBroker report = new ReportByBroker();
                report.DataSource = _ds;
                documentPreview.DocumentSource = report;
                report.CreateDocument();
            }

            if (_report_type == 3)
            {
                ReportByAccountReason report = new ReportByAccountReason();
                report.DataSource = _ds;
                documentPreview.DocumentSource = report;
                report.CreateDocument();
            }

            if (_report_type == 4)
            {
                ReportByAccountBrokerReason report = new ReportByAccountBrokerReason();
                report.DataSource = _ds;
                documentPreview.DocumentSource = report;
                report.CreateDocument();
            }
        }




     
    }
}
