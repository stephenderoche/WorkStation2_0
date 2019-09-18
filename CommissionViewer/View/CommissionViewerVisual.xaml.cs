namespace CommissionViewer.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using SalesSharedContracts;

    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using DevExpress.Xpf.Grid;
  
    using DevExpress.Xpf.Bars;
 
    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.Utils.Themes;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Core;
  
  
    using CommissionViewer.Client.ViewModel;
    using System.IO;
    using System.Threading;
  
     using CommissionViewer.Client;
  


    public partial class CommissionViewerVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         private string _fileByBroker = "ByBroker.xaml";
         private string _fileByBrokerReason = "ByBrokerReason.xaml";
         private string _fileByAccountBrokerReason = "ByAccountBrokerReason.xaml";
         private string _fileByAccountReason = "ByAccountReason.xaml";

         private const string MSGBOX_TITLE_ERROR = "DataDashBoard";
        
         public CommissionViewerModel _view;


         # region Parameters

         string _xml = "ByBrokerGrid";
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
        }
        

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


        DateTime _startDate;
        public DateTime StartDate
        {
            set { _startDate = value; }
            get { return _startDate; }
        }

        DateTime _endDate;
        public DateTime EndDate
        {
            set { _endDate = value; }
            get { return _endDate; }
        }


       
        


         private Int32 _selectedAccountId = -1;
         public Int32 SelectedAccountId
         {
             set { _selectedAccountId = value; }
             get { return _selectedAccountId; }
         }

       

        

         private string _selectAccountName = string.Empty;
         public string SelectAccountName
         {
             set { _selectAccountName = value; }
             get { return _selectAccountName; }
         }

        private string _brokerMnemonic = "ALL";
        public string BrokerMnemonic
        {
            set { _brokerMnemonic = value; }
            get { return _brokerMnemonic; }
        }

        private int _brokerID = -1;
        public int BrokerID
        {
            set { _brokerID = value; }
            get { return _brokerID; }
        }

        private int _reasoncode = -1;
        public int Reasoncode
        {
            set { _reasoncode = value; }
            get { return _reasoncode; }
        }

        private string _assetCode = "";
        public string AssetCode
        {
            set { _assetCode = value; }
            get { return _assetCode; }
        }

        private DateTime _begindate;
        public DateTime Begindate
        {
            set { _begindate = value; }
            get { return _begindate; }
        }

        private DateTime _enddate;
        public DateTime Enddate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }


         # endregion Parameters

         public CommissionViewerVisual(CommissionViewerModel ViewerModel)
         {


             InitializeComponent();
             this.DataContext = ViewerModel;
             this._view = ViewerModel;

             ApplicationThemeHelper.ApplicationThemeName = _view.Parameters.DefaultTheme;
         
             GetSecurities();
             GetBrokers();
         
             this.comboBoxEdit1.Text = _view.AccountName;



            
             
             DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;


             load_Commission_reason_code("All");

            AssignXML();

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


                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, _view.Parameters.StartDate, _view.Parameters.EndDate);
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

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {

                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        });
                }

                catch (Exception ex)
                {
                    _view.AccountId = -1;
                    _view.AccountName = "";
                    _view.Get_ChartData(-1, Convert.ToInt32(BrokerID), Reasoncode, _view.Parameters.StartDate, _view.Parameters.EndDate);
                    throw ex;
                }

              

                return retval;
            }
            else
            {
                _view.AccountId = -1;
                _view.AccountName = "";
                _view.Get_ChartData(-1, Convert.ToInt32(BrokerID), Reasoncode, _view.Parameters.StartDate, _view.Parameters.EndDate);
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

                        this.Dispatcher.BeginInvoke(new Action(() =>
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

        public void get_account_name(string account_id)
        {
            string textEnteredPlusNew = "account_id = " + account_id;

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
                        _view.AccountName = Convert.ToString(item);
                        _view.Parameters.AccountName = _view.AccountName;

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {

                        }), System.Windows.Threading.DispatcherPriority.Normal);
                    }

                });
        }

        private void comboBoxEdit1_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


            Validate(comboBoxEdit.Text);
        }


        #endregion Account

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

                                    BrokerID = defaultSecurityId;
                                    BrokerMnemonic = defaultSymbol;
                                    _view.Parameters.Broker = BrokerMnemonic;
                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, Begindate, Enddate);

                             
                                    retval = true;
                                }

                                else
                                {
                                    BrokerID = -1;
                                    BrokerMnemonic = "All";
                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, Begindate, Enddate);
                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                BrokerID = -1;
                                BrokerMnemonic = "All";
                                _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, Begindate, Enddate);
                                throw ex;
                            }

                            Dispatcher.BeginInvoke(new Action(() =>
                            {

                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        });
                }

                catch (Exception ex)
                {
                    BrokerID = -1;
                    BrokerMnemonic = "All";
                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, Begindate, Enddate);
                    throw ex;
                }

                return retval;
            }

            else
            {
                BrokerID = -1;
                BrokerMnemonic = "All";
                _view.Get_ChartData(Convert.ToInt32(_view.AccountId), -1, Reasoncode, Begindate, Enddate);
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
        private void get_asset_type_id()
        {

            if (cboReasonCodeType.SelectedIndex == 0)
            {
               Reasoncode = -1;
            }
            else if (cboReasonCodeType.SelectedIndex == 1)
            {
                Reasoncode = 0;
            }
            else if (cboReasonCodeType.SelectedIndex == 2)
            {
                Reasoncode = 1;
            }
            else if (cboReasonCodeType.SelectedIndex == 3)
            {
                Reasoncode = 2;
            }
            else if (cboReasonCodeType.SelectedIndex == 4)
            {
                Reasoncode = 3;
            }
            else if (cboReasonCodeType.SelectedIndex == 5)
            {
                Reasoncode = 4;
            }
            else if (cboReasonCodeType.SelectedIndex == 6)
            {
                Reasoncode = 5;
            }
            else if (cboReasonCodeType.SelectedIndex == 7)
            {
                Reasoncode = 6;
            }
            else if (cboReasonCodeType.SelectedIndex == 8)
            {
                Reasoncode = 7;
            }
            else if (cboReasonCodeType.SelectedIndex == 9)
            {
                Reasoncode = 8;
            }
            else if (cboReasonCodeType.SelectedIndex == 10)
            {
                Reasoncode = 9;
            }
            else
            {
                Reasoncode = -1;
            }

        }
     
        private void Get_From_info()
        {

            _view.AccountName = _view.Parameters.AccountName;
            EndDate = _view.Parameters.EndDate;
            StartDate = _view.Parameters.StartDate;

          

            getDates();


            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, _view.Parameters.StartDate, _view.Parameters.EndDate);

          


        }

       

        public void GetSecurities()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    AllSecurities = _view.DBService.GetSecurityInfo("");

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
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

        void getDates()
        {


            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(StartDate.ToString(), out intStr);
            if (intResultTryParse == true)
            {
                StartDate = (intStr);

            }
            else
            {
                StartDate = DateTime.Today;
            }

            DateTime intStr1;
            bool intResultTryParse1 = DateTime.TryParse(EndDate.ToString(), out intStr1);
            if (intResultTryParse1 == true)
            {
                EndDate = intStr1;
            }
            else
            {
                EndDate = DateTime.Today;
            }
        }
        private void AssignXML()
        {





            if (File.Exists((subPath + "\\" + _fileByBroker)))
            {
                ByBrokerGrid.RestoreLayoutFromXml(subPath + "\\" + _fileByBroker);
            }

            if (File.Exists((subPath + "\\" + _fileByBrokerReason)))
            {
                ByBrokerReasonGrid.RestoreLayoutFromXml(subPath + "\\" + _fileByBrokerReason);
            }

            if (File.Exists((subPath + "\\" + _fileByAccountBrokerReason)))
            {
                ByBrokerAccountReasonGrid.RestoreLayoutFromXml(subPath + "\\" + _fileByAccountBrokerReason);
            }

            if (File.Exists((subPath + "\\" + _fileByAccountReason)))
            {
                ByAccountReasonGrid.RestoreLayoutFromXml(subPath + "\\" + _fileByAccountReason);
            }



        }

        public void SaveXML()
        {


            if (!System.IO.Directory.Exists(subPath))
                System.IO.Directory.CreateDirectory((subPath));

            ByBrokerGrid.SaveLayoutToXml(subPath + "\\" + _fileByBroker);
            ByBrokerReasonGrid.SaveLayoutToXml(subPath + "\\" + _fileByBrokerReason);
            ByBrokerAccountReasonGrid.SaveLayoutToXml(subPath + "\\" + _fileByAccountBrokerReason);
            ByAccountReasonGrid.SaveLayoutToXml(subPath + "\\" + _fileByAccountReason);
        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _view.AccountName;


        }

       

       
        # endregion HelperProcedures

        # region Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Begindate = Convert.ToDateTime(DPFromDate.DisplayDate.ToString());
            //Enddate = Convert.ToDateTime(DPToDate.DisplayDate.ToString());

            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToInt32(BrokerID), Reasoncode, _view.Parameters.StartDate, _view.Parameters.EndDate);
          
        }

        private void cboReasonCodeType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            get_asset_type_id();
            AssetCode = (null == this.cboReasonCodeType.Text) ? "" : cboReasonCodeType.Text;
        }

   
      
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
            }
            Get_From_info();
         
           

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

        private void CommissionTabs_SelectionChanged(object sender, RoutedEventArgs e)
        {
            setform();
        }
        public void setform()
        {

            if (this.CommissionTabs.SelectedIndex == 0)
            {

                comboBoxEdit1.IsEnabled = false;
                
                cboReasonCodeType.IsEnabled = false;
            }

            if (this.CommissionTabs.SelectedIndex == 1)
            {

                comboBoxEdit1.IsEnabled = false;
                
                cboReasonCodeType.IsEnabled = true;
            }

            if (this.CommissionTabs.SelectedIndex == 2)
            {

                comboBoxEdit1.IsEnabled = true;
               
                cboReasonCodeType.IsEnabled = true;
            }

            if (this.CommissionTabs.SelectedIndex == 3)
            {

                comboBoxEdit1.IsEnabled = true;
                
                cboReasonCodeType.IsEnabled = true;
            }




        }
        #endregion Methods

      

    

    
      

       

      

        



    

    }
}
