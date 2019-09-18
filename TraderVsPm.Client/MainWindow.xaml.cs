using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using SecurityHistoryApp.Shared.ServiceContracts;
using Linedata.Framework.Foundation;
using Linedata.Shared.Api.ServiceModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;
using SecurityHistoryTPlus;
//using Linedata.DataCache;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

using System.Text;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;


using System.Runtime.Serialization;

using Linedata.Shared.Api.UI;



namespace SecurityHistoryTPlus.Client

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SecurityHistoryMainWindow : Window
    {
       
      
    
        static private string dsn = "";
        private bool suspendChangesHandling;
   
        private string originalName;
        private string distributionName;
        private int currentPercent;
        private DataSet m_StrategyDataSet;
        private IList<Security> lookupSecurity;
        public Security securtyinfo;
        private long securityId;
        private string security_symbol = "";
        private string issuer_name = "";
        private string search;
        private string begindate;
        private string enddate;
        private string account;
       

        public SecurityHistoryMainWindow()
        {
           
            InitializeComponent();

            //begindate = this.txtstartdate.Text;
            //enddate = this.txtenddate.Text;
            //account = this.txtaccount_id.Text;
            //search = this.txtBrokerSearch.Text.ToUpper();
            //if (this.radioSecurity.IsChecked == true)
            //    security_symbol = this.SecurityTextBox.Text;

            //if (this.radioissuer.IsChecked == true)
            //    issuer_name = this.txtissuer.Text;
            ////GetWhichCheckBox();

            // GetSecurityHistory();
              
            
             //m_StrategyDataSet = new DataSet();
             

       
        }

        private ISecurityHistoryServicesContracts CreateServiceClient()
        {
            try
            {
                return TransportManagerProxyFactory<ISecurityHistoryServicesContracts>.GetReusableInstance("SecurityHistoryApp");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private DataSet GetSecurityHistory()
        {
            DataSet ds = new DataSet();
          
           
              try
              {
                  
                    //Get_History_date();
                    //Get_Position_data();
               
              }
              catch (Exception ex)
              {

                  throw ex;
              }

                 
            return ds;
        }


        private void WhichCheckBox()
        {
            if (this.radioSecurity.IsChecked == true)
                security_symbol = this.SecurityTextBox.Text;

            if (this.radioissuer.IsChecked == true)
                issuer_name = this.txtissuer.Text;
        }


        private void GetWhichCheckBox()
        {
            try
            {
             ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
           

                Dispatcher.BeginInvoke(new Action(() =>
                {
                WhichCheckBox();
                }));
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void Get_History_date()
        {

            DataSet ds = new DataSet();

           

            try
            {
           
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    

             ApplicationMessageList messages = null;
             ISecurityHistoryServicesContracts dbService = this.CreateServiceClient();
                ds = dbService.se_get_ticket_report(
                    account,
                    security_symbol,
                    issuer_name,
                    search,
                    Convert.ToDateTime(begindate),
                    Convert.ToDateTime(enddate),
                    
                    out messages);
                 Dispatcher.BeginInvoke(new Action(() => { this.dataGrid.DataContext = ds; }), System.Windows.Threading.DispatcherPriority.Normal);
                
                });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void get_string_search()
        {
            try
            {
             ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    
                    
                    Dispatcher.BeginInvoke(new Action(() => { search = this.txtBrokerSearch.Text.ToUpper(); }), System.Windows.Threading.DispatcherPriority.Normal);
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void Get_Position_data()
        {

            DataSet ds = new DataSet();

            try
            {
                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {

                        ApplicationMessageList messages = null;
                        ISecurityHistoryServicesContracts dbService = this.CreateServiceClient();
                        ds = dbService.se_get_positions_report(
                        account,
                        security_symbol,
                        issuer_name,
                        search,
                        out messages);
                        Dispatcher.BeginInvoke(new Action(() => { this.dataGridPositions.DataContext = ds; }), System.Windows.Threading.DispatcherPriority.Normal);

                    });

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

       

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        
        }

      
    
        private void filter(object sender, RoutedEventArgs e)
        {
            GetSecurityHistory();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetSecurityHistory();
        }

      

       

    
     
    }
}
