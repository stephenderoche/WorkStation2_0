namespace GenericChart
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
   
    using System.Threading;
    using SalesSharedContracts;
    using Linedata.Framework.Foundation;
    using System.Data;
    using System;
    using Linedata.Shared.Api.ServiceModel;
   
    using System.IO;
    using DevExpress.Xpf.Bars;
    using DevExpress.Xpf.Grid;
    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.Core.ConditionalFormatting;
    using DevExpress.Xpf.Core.Serialization;
    using DevExpress.Xpf.Charts;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Interaction logic for TestSampleView.xaml
    /// </summary>
    public partial class GenericChartView : UserControl
    {
      
      
 
        public GenericChartViewModel view;
        string subPath = "c:\\dashboard";
        string file = "VisualGrid.xaml";
      

        # region Parameters

       

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

      

        private string _chkText;
        public string Follower
        {
            set { _chkText = value; }
            get { return _chkText; }
        }

        private string _chartType;
        public string ChartType 
        {
            set { _chartType = value; }
            get { return _chartType; }
        }

        public Diagram Diagram
        {
            get { return this.ChartHierarchy.Diagram; }
        }

        #endregion Parameters


        public GenericChartView(GenericChartViewModel ViewModel)
        {
            
            InitializeComponent();
            this.view = ViewModel;
            this.DataContext = ViewModel;

            this.view = ViewModel;
            ChartType = view.Parameters.DataType;
            HierarchyName = view.Parameters.Hierarchy;
        }

     
    
        # region Account
    
        public bool Validate(string ourTextBox)
        {
            bool retval = false;

            if (!String.IsNullOrEmpty(ourTextBox))
            {
                try
                {
                    ThreadPool.QueueUserWorkItem(
                        delegate(object eventArg)
                        {
                            int defaultAccountId = -1;
                            string defaultShortName = "";
                            try
                            {

                                
                                view.DBService.ValidateAccountForUser(ourTextBox, out defaultAccountId, out defaultShortName);
                                if (defaultAccountId != -1)
                                {

                                    view.AccountId = defaultAccountId;
                                    view.AccountName = defaultShortName;
                                    view.Parameters.AccountName = view.AccountName;
                                  
                                    Securitieschart();
                                    retval = true;
                                }

                                else
                                {
                                    view.AccountId = -1;
                                    view.AccountName = "";

                                }
                            }
                            catch (Exception ex)
                            {
                                view.AccountId = -1;
                                view.AccountName = "";
                                throw ex;
                            }

                            Dispatcher.BeginInvoke(new Action(() =>
                            {

                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        });
                }

                catch (Exception ex)
                {
                    view.AccountId = -1;
                    view.AccountName = "";
                    throw ex;
                }

                return retval;
            }

            return retval;
        }

        private void comboBoxEdit1_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            string textEnteredPlusNew = "short_name Like '" + comboBoxEdit1.Text + "%'";
            this.comboBoxEdit1.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    var someObject = view.AllAccounts;
                    if (someObject == null)
                    {
                        GetAcc();
                        return;
                    }
                    foreach (DataRow row in view.AllAccounts.Tables[0].Select(textEnteredPlusNew))
                    {

                        object item = row["short_name"];
                        object account_id = row["account_id"];

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            comboBoxEdit1.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(account_id)));

                            

                        }), System.Windows.Threading.DispatcherPriority.Normal);

                        count = count + 1;
                        if (count == 100)
                        {
                            break;
                        }
                    }

                });

            Validate(view.Parameters.AccountName);


        }

        public void get_account_name(string account_id)
        {
            string textEnteredPlusNew = "account_id = " + account_id;
          
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    var someObject = view.AllAccounts;
                    if (someObject == null)
                    {
                        GetAcc();
                        return;
                    }
                    foreach (DataRow row in view.AllAccounts.Tables[0].Select(textEnteredPlusNew))
                    {
                        object item = row["short_name"];
                        view.AccountName = Convert.ToString(item);
                        view.Parameters.AccountName = view.AccountName;

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                           
                        }), System.Windows.Threading.DispatcherPriority.Normal);
                    }

                });
        }

        #endregion Account

        # region Helperfunctions

        public void Securitieschart()
        {



            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    ApplicationMessageList messages = null;

                    DataSet dsSector = new DataSet();
                    //sector

                    if (view.Parameters.DataType == string.Empty)
                    {
                        return;
                    }




                    if (view.Parameters.DataType == "Top Accounts")
                        dsSector = view.DBService.se_get_top_accounts(
                           Convert.ToInt32(view.AccountId),
                            out messages);
                    if (view.Parameters.DataType == "Top Issuers")
                        dsSector = view.DBService.se_get_top_issuers_dasboard(
                           Convert.ToInt32(view.AccountId),
                            out messages);
                    if (view.Parameters.DataType == "Top Securities")
                        dsSector = view.DBService.se_get_top_securities_dasboard(
                           Convert.ToInt32(view.AccountId),
                            out messages);
                    if (view.Parameters.DataType == "Hierarchy")
                    {
                        if (view.Parameters.Hierarchy == string.Empty)
                        {


                            dsSector = view.DBService.se_fi_asset_allocation_pru(
                        Convert.ToInt32(view.AccountId),
                        "Major Asset",
                         out messages);
                        }
                        else
                        {

                            dsSector = view.DBService.se_fi_asset_allocation_pru(
                           Convert.ToInt32(view.AccountId),
                            view.Parameters.Hierarchy,
                            out messages);

                            if (dsSector.Tables.Count > 0)
                                if (dsSector.Tables[0].Rows.Count > 0)
                                {

                                    DataColumnCollection columns = dsSector.Tables[0].Columns;

                                    if (columns.Contains("MV"))
                                    {
                                        double a = Convert.ToDouble(dsSector.Tables[0].Rows[0]["MV"].ToString());
                                        view.MV = Convert.ToString(a.ToString("n2")); 
                                    }
                                    else
                                    {
                                        view.MV = "0";
                                    }
                                    
                                
                                }
                           

                            
                          
                        }
                    }

                    if (view.Parameters.DataType == "Performance")
                    {
                        dsSector = view.DBService.se_get_performance_summary(
                           Convert.ToInt32(view.AccountId),
                            out messages);
                    }



                    if (view.Parameters.DataType == "Top Compliance Securities")
                    {
                        dsSector = view.DBService.se_cmpl_get_top_security_breaches(
                     Convert.ToInt32(view.AccountId),
                     1,
                     out messages);
                    }

                    if (view.Parameters.DataType == "Compliance Breaches")
                    {
                        dsSector = view.DBService.se_cmpl_get_breaches_by_servitiy(
                     Convert.ToInt32(view.AccountId),
                     1,
                     out messages);
                    }
                    if (view.Parameters.DataType == "VsBenchmark")
                    {
                        dsSector = view.DBService.se_benchmark_vs_holdings(
                          Convert.ToInt32(view.AccountId),
                          view.DBService.se_get_default_hierarchy(Convert.ToInt32(view.AccountId), out messages),
                         out messages);
                    }

                    if (view.Parameters.DataType == "Under Management")
                    {
                        dsSector = view.DBService.se_get_assets_under_management(
                    Convert.ToInt32(view.AccountId),
                     out messages);
                    }

                    if (view.Parameters.DataType == "Maturities")
                    {
                        dsSector = view.DBService.se_get_maturities(
                        Convert.ToInt32(view.AccountId),
                        60,
                          out messages);
                    }

                    if (view.Parameters.DataType == "Aged Breaches")
                    {
                        dsSector = view.DBService.se_cmpl_case_statistics(
                       Convert.ToInt32(view.AccountId),
                       1,
                       out messages);
                    }

                   

                    if (view.Parameters.DataType == "Cash Bar")
                    {
                        dsSector = view.DBService.se_cash_balance(
                      Convert.ToInt32(view.AccountId),
                      1,
                      Convert.ToInt32(view.Parameters.Negitive),
                      Convert.ToInt32(view.Parameters.Positive),
                      1,
                      1,
                      out messages);
                 
                    }



                    if (dsSector != null && dsSector.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {

                            this._dataGrid.ItemsSource = null;

                            if (view.Parameters.DataType == "Top Accounts")
                            {
                                this.TopAccountsBar.DataSource = dsSector.Tables[0]; ;
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                            }

                            if (view.Parameters.DataType == "Top Issuers")
                            {
                                this.TopIssuers.DataSource = dsSector.Tables[0];
                                this.TopIssuers.UpdateData();
                                this._dataGrid.ItemsSource = dsSector.Tables[0];

                            }
                            if (view.Parameters.DataType == "Top Securities")
                            {
                                this.TopSecurities.DataSource = dsSector.Tables[0];
                                this.TopSecurities.UpdateData();
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                            }

                            if (view.Parameters.DataType == "Hierarchy")
                            {
                                this.ChartHierarchy.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.ChartHierarchy.UpdateData();
                            }

                            if (view.Parameters.DataType == "Performance")
                            {
                                this.ChartPerformance.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.ChartPerformance.UpdateData();
                            }



                            if (view.Parameters.DataType == "Top Compliance Securities")
                            {
                                this.ChartTopCmplSecurities.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.ChartTopCmplSecurities.UpdateData();
                            }

                            if (view.Parameters.DataType == "Compliance Breaches")
                            {
                                this.ComplianceSeverityChart.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                //this.ComplianceSeverityCharty.UpdateData();
                            }



                            if (view.Parameters.DataType == "VsBenchmark")
                            {
                                this.ChartVsBenchmark.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.ChartVsBenchmark.UpdateData();
                            }


                            if (view.Parameters.DataType == "Under Management")
                            {
                                this.AssetUnderBubble.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.AssetUnderBubble.UpdateData();
                            }

                            if (view.Parameters.DataType == "Maturities")
                            {
                                this.ChartMaturities.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.ChartMaturities.UpdateData();
                            }

                            if (view.Parameters.DataType == "Aged Breaches")
                            {
                                this.AgedBreaches.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.AgedBreaches.UpdateData();
                            }

                            if (view.Parameters.DataType == "Cash Bar")
                            {
                                this.CashBar.DataSource = dsSector.Tables[0];
                                this._dataGrid.ItemsSource = dsSector.Tables[0];
                                this.CashBar.UpdateData();
                            }

                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.ChartBreachesBySeverity.DataContext = null; this.ChartTopCmplSecurities.DataContext = null; this.ChartPerformance.DataContext = null; this.ChartHierarchy.DataContext = null; this.TopSecurities.DataContext = null; this.TopIssuers.DataContext = null; this.TopAccountsBar.DataContext = null; this._dataGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);



                });

           

          
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Validate(view.Parameters.AccountName);
            Securitieschart();
           

            if (pieTotalLabel != null)
                ((Storyboard)pieTotalLabel.Resources["pieTotalLabelStoryboard"]).Begin();
            if (legend != null && this.ChartHierarchy.Diagram.Series[0].Points.Count > 0)
                ((Storyboard)legend.Resources["legendStoryboard"]).Begin();

            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
            }

            setGrids();
            setcolor();
        }
        public void setcolor()
        {


            if (cmboView.Text == "Grid")
            {
                this.lblColorIndicator.Background = Brushes.Green;
            }
            else
            {
                this.lblColorIndicator.Background = Brushes.Orange;
            }

        }

        public void UpdateAccoutText()
        {


            view.Parameters.AccountName = view.AccountName;


        }

        public void setGrids()
        {
            if (cmboView.Text == "Chart")
            {
                _dataGrid.Visibility = System.Windows.Visibility.Hidden;

                if (view.Parameters.DataType == "Top Accounts")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Visible;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;
                }

                if (view.Parameters.DataType == "Top Issuers")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Visible;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;
                }

                if (view.Parameters.DataType == "Top Securities")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Visible;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;

                }

                if (view.Parameters.DataType == "Hierarchy")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Visible;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;

                }

                if (view.Parameters.DataType == "Performance")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Visible;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;

                }

                if (view.Parameters.DataType == "Top Compliance Securities")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Visible;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;

                }

                if (view.Parameters.DataType == "Compliance Breaches")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Visible;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;
                }

               

                      if (view.Parameters.DataType == "VsBenchmark")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Visible;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;
                }


                 if (view.Parameters.DataType == "Under Management")
                {
                    TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                    TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                    TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                    ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                    ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                    ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                    ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                    AssetUnderBubble.Visibility = System.Windows.Visibility.Visible;
                    ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                    AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                    CashBar.Visibility = System.Windows.Visibility.Hidden;
                    rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                    rdoPositve.Visibility = System.Windows.Visibility.Hidden;
                }

                 if (view.Parameters.DataType == "Maturities")
                 {
                     TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                     TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                     TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                     ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                     ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                     ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                     ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                     ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                     AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                     ChartMaturities.Visibility = System.Windows.Visibility.Visible;
                     AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                     CashBar.Visibility = System.Windows.Visibility.Hidden;
                     rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                     rdoPositve.Visibility = System.Windows.Visibility.Hidden;

                 }


                 if (view.Parameters.DataType == "Aged Breaches")
                 {
                     TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                     TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                     TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                     ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                     ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                     ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                     ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                     ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                     AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                     ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                     AgedBreaches.Visibility = System.Windows.Visibility.Visible;
                     CashBar.Visibility = System.Windows.Visibility.Hidden;
                     rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                     rdoPositve.Visibility = System.Windows.Visibility.Hidden;
                     

                 }

                 if (view.Parameters.DataType == "Cash Bar")
                 {
                     TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                     TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                     TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                     ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                     ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                     ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                     ChartBreachesBySeverity.Visibility = System.Windows.Visibility.Hidden;
                     ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                     AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                     ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                     AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                     CashBar.Visibility = System.Windows.Visibility.Visible;
                     rdoNegitve.Visibility = System.Windows.Visibility.Visible;
                     rdoPositve.Visibility = System.Windows.Visibility.Visible;


                 }
               
               
            }
            else
            {
                AssignXML();
                _dataGrid.Visibility = System.Windows.Visibility.Visible;
                TopAccountsBar.Visibility = System.Windows.Visibility.Hidden;
                TopIssuers.Visibility = System.Windows.Visibility.Hidden;
                TopSecurities.Visibility = System.Windows.Visibility.Hidden;
                ChartHierarchy.Visibility = System.Windows.Visibility.Hidden;
                ChartPerformance.Visibility = System.Windows.Visibility.Hidden;
                ChartTopCmplSecurities.Visibility = System.Windows.Visibility.Hidden;
                ComplianceSeverityChart.Visibility = System.Windows.Visibility.Hidden;
                ChartVsBenchmark.Visibility = System.Windows.Visibility.Hidden;
                AssetUnderBubble.Visibility = System.Windows.Visibility.Hidden;
                ChartMaturities.Visibility = System.Windows.Visibility.Hidden;
                AgedBreaches.Visibility = System.Windows.Visibility.Hidden;
                CashBar.Visibility = System.Windows.Visibility.Hidden;
                rdoNegitve.Visibility = System.Windows.Visibility.Hidden;
                rdoPositve.Visibility = System.Windows.Visibility.Hidden;
            }

            
         
        }

        public void GetAcc()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                   
                    view.AllAccounts = view.DBService.GetAllAccounts2();

                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }

      

        public void AssignXML()
        {
            if (_dataGrid.Columns.Count > 1)
            {
                int maxColumnIndex = _dataGrid.Columns.Count - 6;
                int[] array = new int[maxColumnIndex];
                int a = 0;
                int b = 0;
                foreach (GridColumn gridColumn in _dataGrid.Columns)
                {
                    if (gridColumn.FieldName == "CustomColumn1" || gridColumn.FieldName == "CustomColumn2" ||
                        gridColumn.FieldName == "CustomColumn3" || gridColumn.FieldName == "CustomColumn4" ||
                        gridColumn.FieldName == "CustomColumn5" || gridColumn.FieldName == "CustomColumn6"
                        )
                    {

                    }
                    else
                    {

                        array[b] = a;
                        b++;
                    }

                    a++;

                }

                int count = 0;
                foreach (int i in array)
                {
                        _dataGrid.Columns.RemoveAt(i - count);
                        count++;
                }
            }
          


            file = view.Parameters.DataType + "Generic.xaml";


            if (File.Exists((subPath + "\\" + file)))
            {
                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);
            }

            savexml(view.Parameters.DataType);
        }

        public void savexml(string DataType)
        {
            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));


                _dataGrid.SaveLayoutToXml(subPath + "\\" + DataType + "Generic.xaml");

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + DataType + "Generic.xaml");

            }
        }

        public void ClearRows()
        {
            _dataGrid.Columns.Clear();
        }
        # endregion Helperfunctions

        # region Methods

        private void cmboView_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            setGrids();
        }

        private void viewAccountTax_CustomRowAppearance(object sender, CustomRowAppearanceEventArgs e)
        {
            e.Result = e.ConditionalValue;
            e.Handled = true;
        }

        private void viewAccountTax_CustomCellAppearance(object sender, CustomCellAppearanceEventArgs e)
        {
            e.Result = e.ConditionalValue;
            e.Handled = true;
        }

       

        private void comboBoxEdit1_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


            Validate(comboBoxEdit.Text);
        }

      

        void OnShowGridMenu(object sender, GridMenuEventArgs e)
        {
            if (e.MenuType == GridMenuType.Column)
            {
                GridColumnHeader columnHeader = e.TargetElement as GridColumnHeader;
                GridColumn column = columnHeader.DataContext as GridColumn;
                bool showColumnHeaderEditor = ColumnBehavior.GetIsRenameEditorActivated(column);
                BarButtonItem item = new BarButtonItem();
                if (showColumnHeaderEditor)
                    item.Content = "Hide ColumnHeader Editor";
                else
                    item.Content = "Show ColumnHeader Editor";
                item.ItemClick += OnItemClick;
                item.Tag = column;
                e.Customizations.Add(item);
            }
        }

        static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            GridColumn column = e.Item.Tag as GridColumn;
            ColumnBehavior.SetIsRenameEditorActivated(column, !ColumnBehavior.GetIsRenameEditorActivated(column));
        }

        private void OnRenameEditorLostFocus(object sender, RoutedEventArgs e)
        {
            TextEdit edit = sender as TextEdit;
            edit.Visibility = System.Windows.Visibility.Hidden;
        }

        #endregion methods



    }
}
