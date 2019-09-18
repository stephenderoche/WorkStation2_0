namespace Reporting.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Controls;
    using SalesSharedContracts;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using DevExpress.Xpf.Grid;
    using DevExpress.Xpf.Printing;
    using DevExpress.Xpf.Bars;
    using System.Windows.Markup;
    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.Core.ConditionalFormatting;
    using DevExpress.Xpf.Core.Serialization;
    using DevExpress.Xpf.Ribbon;
  
    using Reporting.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using Reporting.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using Linedata.Client.Workstation.SharedReferences;




    public partial class ReportingVisual : System.Windows.Controls.UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
       
         private string getsecurityTextinfo = "";
         private int getsecurityLenthinfo = 0;
         public ReportingModel _view;

         # region Parameters

        
         private DataSet _allSecurities;
         public DataSet AllSecurities
         {
             set { _allSecurities = value; }
             get { return _allSecurities; }
         }

        private DataSet _allBrokers;
        public DataSet AllBrokers
        {
            set { _allBrokers = value; }
            get { return _allBrokers; }
        }


      

         # endregion Parameters
     

         public ReportingVisual(ReportingModel genericGridViewerModel)
         {


             InitializeComponent();


             this.DataContext = genericGridViewerModel;
             this._view = genericGridViewerModel;
            
             GetSecurities();
             GetBrokers();

           
          

             this.DataContext = genericGridViewerModel;
             this._view = genericGridViewerModel;
           
             this.comboBoxEdit1.Text = _view.AccountName;
             this.SecurityComboBoxEdit.Text = _view.Symbol;
           
             DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

             load_Commission_reason_code("All");
          

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
                                 _view.DBService.ValidateAccountForUser(ourTextBox, out defaultAccountId, out defaultShortName);
                                 if (defaultAccountId != -1)
                                 {

                                     _view.AccountId = defaultAccountId;
                                     _view.AccountName = defaultShortName;
                                     _view.Parameters.AccountName = _view.AccountName;

                                     // Get_ChartData();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _view.AccountId = -1;
                                     _view.AccountName = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _view.AccountId = -1;
                                 _view.AccountName = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _view.AccountId = -1;
                     _view.AccountName = "";
                     throw ex;
                 }



                 return retval;
             }
             else
             {
                 _view.AccountId = -1;
                 _view.AccountName = "";
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
                     var someObject = _view.AllAccounts;
                     if (someObject == null)
                     {
                         _view.GetAcc();
                         return;
                     }
                     foreach (DataRow row in _view.AllAccounts.Tables[0].Select(textEnteredPlusNew))
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

             Validate(_view.Parameters.AccountName);


         }

         //public void get_account_name(string account_id)
         //{
         //    string textEnteredPlusNew = "account_id = " + account_id;

         //    ThreadPool.QueueUserWorkItem(
         //        delegate(object eventArg)
         //        {
         //            var someObject = _view.M_AllAccounts;
         //            if (someObject == null)
         //            {
         //                _view.GetAcc();
         //                return;
         //            }
         //            foreach (DataRow row in _view.M_AllAccounts.Tables[0].Select(textEnteredPlusNew))
         //            {
         //                object item = row["short_name"];
         //                _view.AccountName = Convert.ToString(item);
         //                _view.Parameters.AccountName = _view.AccountName;

         //                Dispatcher.BeginInvoke(new Action(() =>
         //                {

         //                }), System.Windows.Threading.DispatcherPriority.Normal);
         //            }

         //        });
         //}

         private void comboBoxEdit1_LostFocus(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             Validate(comboBoxEdit.Text);
         }


        #endregion Account

        # region Security

         public bool ValidateSecurity(string ourTextBox)
         {
             bool retval = false;

             if (!String.IsNullOrEmpty(ourTextBox))
             {
                 try
                 {
                     ThreadPool.QueueUserWorkItem(
                         delegate(object eventArg)
                         {
                             int defaultSecurityId = -1;
                             string defaultSymbol = "";
                             try
                             {
                                 if (AllSecurities.Tables.Count > 0)
                                 {
                                     for (int rowIndex = 0; rowIndex < AllSecurities.Tables[0].Rows.Count; ++rowIndex)
                                     {
                                         if (ourTextBox.ToUpper().CompareTo(((AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString().ToUpper()) == 0)
                                         {
                                             defaultSymbol = ((AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString();
                                             defaultSecurityId = Convert.ToInt32((AllSecurities.Tables[0].Rows[rowIndex])["security_id"]);
                                             break;
                                         }
                                     }
                                 }


                                 if (defaultSecurityId != -1)
                                 {

                                     _view.SecurityId = defaultSecurityId;
                                     _view.Symbol = defaultSymbol;
                                     _view.Parameters.SecurityName = _view.Symbol;

                                     // Get_ChartData();
                                     retval = true;
                                 }
                                 if (defaultSecurityId != -1)
                                 {

                                     _view.SecurityId = defaultSecurityId;
                                     _view.Symbol = defaultSymbol;
                                     _view.Parameters.SecurityName = _view.Symbol;

                                     // Get_ChartData();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _view.SecurityId = -1;
                                     _view.Symbol = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _view.SecurityId = -1;
                                 _view.Symbol = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _view.SecurityId = -1;
                     _view.Symbol = "";
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _view.SecurityId = -1;
                 _view.Symbol = "";
             }

             return retval;
         }

         private void SecurityComboBoxEdit_EditValueChanged_1(object sender, EditValueChangedEventArgs e)
         {
             string textEnteredPlusNew = "symbol Like '" + SecurityComboBoxEdit.Text + "%'";
             this.SecurityComboBoxEdit.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = AllSecurities;
                     if (someObject == null)
                     {
                         GetSecurities();
                         return;
                     }
                     foreach (DataRow row in AllSecurities.Tables[0].Select(textEnteredPlusNew))
                     {

                         object item = row["symbol"];
                         object security_id = row["security_id"];

                         Dispatcher.BeginInvoke(new Action(() =>
                         {
                             SecurityComboBoxEdit.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(security_id)));



                         }), System.Windows.Threading.DispatcherPriority.Normal);

                         count = count + 1;
                         if (count == 100)
                         {
                             break;
                         }
                     }

                 });


             ValidateSecurity(_view.Parameters.SecurityName);


         }
         private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateSecurity(comboBoxEdit.Text);
         }

      

        # endregion Security

         # region Broker
         public bool ValidateBroker(string ourTextBox)
         {
             bool retval = false;

             if (!String.IsNullOrEmpty(ourTextBox))
             {
                 try
                 {
                     ThreadPool.QueueUserWorkItem(
                         delegate(object eventArg)
                         {
                             int defaultSecurityId = -1;
                             string defaultSymbol = "";
                             try
                             {
                                 if (AllSecurities.Tables.Count > 0)
                                 {
                                     for (int rowIndex = 0; rowIndex < AllBrokers.Tables[0].Rows.Count; ++rowIndex)
                                     {
                                         if (ourTextBox.ToUpper().CompareTo(((AllBrokers.Tables[0].Rows[rowIndex])["mnemonic"]).ToString().ToUpper()) == 0)
                                         {
                                             defaultSymbol = ((AllBrokers.Tables[0].Rows[rowIndex])["mnemonic"]).ToString();
                                             defaultSecurityId = Convert.ToInt32((AllBrokers.Tables[0].Rows[rowIndex])["broker_id"]);
                                             break;
                                         }
                                     }
                                 }



                                 if (defaultSecurityId != -1)
                                 {

                                     _view.BrokerID = defaultSecurityId;
                                     _view.BrokerMnemonic = defaultSymbol;
                                     _view.Parameters.Broker = _view.BrokerMnemonic;
                                  


                                     retval = true;
                                 }

                                 else
                                 {
                                     _view.BrokerID = -1;
                                     _view.BrokerMnemonic = "All";
                                   

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _view.BrokerID = -1;
                                 _view.BrokerMnemonic = "All";
                               
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _view.BrokerID = -1;
                     _view.BrokerMnemonic = "All";
                    
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _view.BrokerID = -1;
                 _view.BrokerMnemonic = "All";
                 
             }

             return retval;
         }
         private void BrokerComboBoxEdit_EditValueChanged_1(object sender, EditValueChangedEventArgs e)
         {
             string textEnteredPlusNew = "mnemonic Like '" + BrokerComboBoxEdit.Text + "%'";
             this.BrokerComboBoxEdit.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = AllBrokers;
                     if (someObject == null)
                     {
                         GetBrokers();
                         return;
                     }
                     foreach (DataRow row in AllBrokers.Tables[0].Select(textEnteredPlusNew))
                     {

                         object item = row["mnemonic"];
                         object broker_id = row["broker_id"];

                         Dispatcher.BeginInvoke(new Action(() =>
                         {
                             BrokerComboBoxEdit.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(broker_id)));



                         }), System.Windows.Threading.DispatcherPriority.Normal);

                         count = count + 1;
                         if (count == 100)
                         {
                             break;
                         }
                     }

                 });


             ValidateBroker(_view.Parameters.Broker);


         }
         private void BrokerComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateBroker(comboBoxEdit.Text);
         }
         # endregion Broker

        # region HelperProcedures

         private bool isNull(string a)
         {
             if (String.IsNullOrEmpty(a))
                 return true;
             else
                 return false;

             {
             }
         }

        private void Get_From_info()
        {

            _view.AccountName = _view.Parameters.AccountName;
            _view.Symbol = String.IsNullOrEmpty(_view.Parameters.SecurityName) ? String.Empty : _view.Parameters.SecurityName;
            _view.Desk = String.IsNullOrEmpty(_view.Parameters.DeskName) ? String.Empty : _view.Parameters.DeskName;
            _view.Report = String.IsNullOrEmpty(_view.Parameters.Report) ? String.Empty : _view.Parameters.Report;
            _view.EndDate = _view.Parameters.EndDate;
            _view.StartDate = _view.Parameters.StartDate;


            getDates();


        }

        private void load_Commission_reason_code(string m_strAssetCode)
        {

            cboReasonCodeType.Items.Add("All");
            cboReasonCodeType.Items.Add("Trade");
            cboReasonCodeType.Items.Add("OTC");
            cboReasonCodeType.Items.Add("Best");
            cboReasonCodeType.Items.Add("Impute");
            cboReasonCodeType.Items.Add("Resear");
            cboReasonCodeType.Items.Add("Market");
            cboReasonCodeType.Items.Add("Fund");
            cboReasonCodeType.Items.Add("Soft");
            cboReasonCodeType.Items.Add("Princ");
            cboReasonCodeType.Items.Add("Under");


            if (m_strAssetCode == "All")
                cboReasonCodeType.SelectedIndex = 0;
            else if (m_strAssetCode == "Trade")
                cboReasonCodeType.SelectedIndex = 1;
            else if (m_strAssetCode == "OTC")
                cboReasonCodeType.SelectedIndex = 2;
            else if (m_strAssetCode == "Best")
                cboReasonCodeType.SelectedIndex = 3;
            else if (m_strAssetCode == "Impute")
                cboReasonCodeType.SelectedIndex = 4;
            else if (m_strAssetCode == "Resear")
                cboReasonCodeType.SelectedIndex = 5;
            else if (m_strAssetCode == "Market")
                cboReasonCodeType.SelectedIndex = 6;
            else if (m_strAssetCode == "Fund")
                cboReasonCodeType.SelectedIndex = 7;
            else if (m_strAssetCode == "soft")
                cboReasonCodeType.SelectedIndex = 8;
            else if (m_strAssetCode == "Princ")
                cboReasonCodeType.SelectedIndex = 9;
            else if (m_strAssetCode == "Under")
                cboReasonCodeType.SelectedIndex = 10;
            else
                cboReasonCodeType.SelectedIndex = 0;

            get_asset_type_id();
        }
        public void GetBrokers()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    AllBrokers = _view.DBService.GetBrokerInfo();

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }

        public void GetSecurities()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    
                    AllSecurities = _view.DBService.GetSecurityInfo("");

                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }


        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {


                    DataSet ds = _view.DBService.GetListAllDesks();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["desk_name"];
                            object desk_id = row["desk_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new Reporting.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

                                count = count + 1;

                                if (_view.Desk == Convert.ToString(item))
                                {
                                    this.cmboDesk.SelectedIndex = count;
                                    _view.DeskId = Convert.ToInt32(desk_id);
                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _view.AccountName;


        }

        void getDates()
        {


            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(_view.StartDate.ToString(), out intStr);
            if (intResultTryParse == true)
            {
                _view.StartDate = (intStr);

            }
            else
            {
                _view.StartDate = DateTime.Today;
            }

            DateTime intStr1;
            bool intResultTryParse1 = DateTime.TryParse(_view.EndDate.ToString(), out intStr1);
            if (intResultTryParse1 == true)
            {
                _view.EndDate = intStr1;
            }
            else
            {
                _view.EndDate = DateTime.Today;
            }
        }

        private void get_asset_type_id()
        {

            if (cboReasonCodeType.SelectedIndex == 0)
            {
                _view.Reasoncode = -1;
            }
            else if (cboReasonCodeType.SelectedIndex == 1)
            {
                _view.Reasoncode = 0;
            }
            else if (cboReasonCodeType.SelectedIndex == 2)
            {
                _view.Reasoncode = 1;
            }
            else if (cboReasonCodeType.SelectedIndex == 3)
            {
                _view.Reasoncode = 2;
            }
            else if (cboReasonCodeType.SelectedIndex == 4)
            {
                _view.Reasoncode = 3;
            }
            else if (cboReasonCodeType.SelectedIndex == 5)
            {
                _view.Reasoncode = 4;
            }
            else if (cboReasonCodeType.SelectedIndex == 6)
            {
                _view.Reasoncode = 5;
            }
            else if (cboReasonCodeType.SelectedIndex == 7)
            {
                _view.Reasoncode = 6;
            }
            else if (cboReasonCodeType.SelectedIndex == 8)
            {
                _view.Reasoncode = 7;
            }
            else if (cboReasonCodeType.SelectedIndex == 9)
            {
                _view.Reasoncode = 8;
            }
            else if (cboReasonCodeType.SelectedIndex == 10)
            {
                _view.Reasoncode = 9;
            }
            else
            {
                _view.Reasoncode = -1;
            }

        }

      
        
        # endregion HelperProcedures

        # region Methods



        private void cboReasonCodeType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            get_asset_type_id();
            _view.AssetCode = (null == this.cboReasonCodeType.Text) ? "" : cboReasonCodeType.Text;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {



            Get_From_info();
            DeskList();

            //DataSet ds = new DataSet();



            // _dataGrid.GroupSummary.Add(SummaryItemType.Sum, "balance_value").DisplayFormat = "Sum: {0:c2}";

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

        private void cmboDesk_SelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmboDesk.Items.Count == 0)
            {
                DeskList();
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboDesk.SelectedItem).ToString()))
                {
                    _view.Desk = (cmboDesk.SelectedItem).ToString();
                    _view.DeskId = Convert.ToInt32(((Reporting.Client.ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);

                }
            }
        }

        private void end_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            getDates();
        }
        private void documentPreview1_Loaded(object sender, RoutedEventArgs e)
        {
            RibbonControl ribbon = DevExpress.Xpf.Core.Native.LayoutHelper.FindElementByType((sender as DocumentPreviewControl), typeof(RibbonControl)) as RibbonControl;
            if (ribbon != null)
            {
                ribbon.IsMinimized = true;
            }
        }

        private void start_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDates();

        }

       
        #endregion Methods

    


       #region Report Click
        private void RunBroker_Click(object sender, RoutedEventArgs e)
        {

          

            _view.ByBroker();
            var i = 0;

            do
            {
                i = i + 1;
                if (_view.DsByBroker == null)
                {
                    _view.ByBroker();

                }
                else
                {
                    ReportByBroker report = new ReportByBroker();

                    report.DataSource = _view.DsByBroker;
                    documentPreview1.DocumentSource = report;
                    report.DataMember = _view.DsByBroker.Tables[0].TableName;
                    report.CreateDocument();
                    i = 5;

                }

            } while (i < 5);


        }

        private void RunBrokerReason_Click(object sender, RoutedEventArgs e)
        {

            _view.ByBroker();
            var i = 0;

            do
            {
                i = i + 1;
                if (_view.DsByBroker == null)
                {
                    _view.ByBroker();

                }
                else
                {
                    ReportByBrokerReason report = new ReportByBrokerReason();

                    report.DataSource = _view.DsByBroker;
                    documentPreview1.DocumentSource = report;
                    report.DataMember = _view.DsByBroker.Tables[0].TableName;
                    report.CreateDocument();
                    i = 5;

                }

            } while (i < 5);
        }

        private void RunAccountReason_Click(object sender, RoutedEventArgs e)
        {


            _view.ByAccountReason();
            var i = 0;

            do
            {
                i = i + 1;
                if (_view.DsAccountReason == null)
                {
                    _view.ByAccountReason();

                }
                else
                {
                    ReportByAccountReason report = new ReportByAccountReason();

                    report.DataSource = _view.DsAccountReason;
                    documentPreview1.DocumentSource = report;
                    report.DataMember = _view.DsAccountReason.Tables[0].TableName;
                    report.CreateDocument();
                    i = 5;

                }

            } while (i < 5);
        }

        private void RunAccountBrokerReason_Click(object sender, RoutedEventArgs e)
        {
           

            
            _view.ByAccountBrokerReason();
            var i = 0;

            do
            {
                i = i + 1;
                if (_view.DsAccountBrokerReason == null)
                {
                    _view.ByAccountBrokerReason();

                }
                else
                {
                    ReportByAccountBrokerReason report = new ReportByAccountBrokerReason();

                    report.DataSource = _view.DsAccountBrokerReason;
                    documentPreview1.DocumentSource = report;
                    report.DataMember = _view.DsAccountBrokerReason.Tables[0].TableName;
                    report.CreateDocument();
                    i = 5;

                }

            } while (i < 5);

        }
        private void RunAccountSummary_Click(object sender, RoutedEventArgs e)
        {




            _view.ByAccountSummary();
            var i = 0;

            do
            {
                i = i + 1;
                if (_view.DsAccountSummary == null)
                {
                    _view.ByAccountSummary();

                }
                else
                {
                    ReportAccountSummary report = new ReportAccountSummary();

                    report.DataSource = _view.DsAccountSummary;
                    documentPreview1.DocumentSource = report;
                    report.DataMember = _view.DsAccountSummary.Tables[0].TableName;
                    report.CreateDocument();
                    i = 5;

                }

            } while (i < 5);


        }

       # endregion Report Click

        

       

    }
}
