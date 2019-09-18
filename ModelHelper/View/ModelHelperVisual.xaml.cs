namespace ModelHelper.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using SalesSharedContracts;

    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using DevExpress.Xpf.Grid;
    using System.Windows.Media;
    using DevExpress.Xpf.Bars;

    using DevExpress.Xpf.Editors;


    using ModelHelper.Client.ViewModel;
    using System.IO;
    using System.Threading;

    using ModelHelper.Client;



    public partial class ModelHelperVisual : UserControl
    {


        // private string MSGBOX_TITLE_ERROR = "Generic Viewer";

        string subPath = "c:\\dashboard";
        private string _fileModel = "ModelHelperModel.xaml";
        private string _fileAccount = "ModelHelperAccount.xaml";
        private string _fileBySecurity = "ModelHelperSecurity.xaml";
        private string _fileByPriceSecurity = "ModelHelperPriceSecurity.xaml";
        private string _fileByModelInfo = "ModelHelperModelInfo.xaml";

        private const string MSGBOX_TITLE_ERROR = "DataDashBoard";

        public ModelHelperModel _view;


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


        private Int32 _filterstatus = 1;

        public Int32 Filterstatus
        {
            set { _filterstatus = value; }
            get { return _filterstatus; }
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

        public ModelHelperVisual(ModelHelperModel ViewerModel)
        {


            InitializeComponent();
           
            this.DataContext = ViewerModel;
            this._view = ViewerModel;
        
            DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

            AssignXML();

        }


        # region HelperProcedures


        public void refreshChart()
        {
            _view.se_get_securities_on_model(_view.ModelId, _view.AccountId, GetfilterStatus());
        }


        private void Get_From_info()
        {

            _view.Get_ModelData();
        }

        public void GetSecurities()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    AllSecurities = _view.DBService.GetSecurityInfo("");

                    this.Dispatcher.BeginInvoke(new Action(() => { }),
                        System.Windows.Threading.DispatcherPriority.Normal);

                });
        }

        private void AssignXML()
        {


          

               
            if (File.Exists((subPath + "\\" + _fileModel)))
            {
                _dataGridModel.RestoreLayoutFromXml(subPath + "\\" + _fileModel);
            }

            if (File.Exists((subPath + "\\" + _fileAccount)))
            {
                _dataGridAccount.RestoreLayoutFromXml(subPath + "\\" + _fileAccount);
            }

            if (File.Exists((subPath + "\\" + _fileBySecurity)))
            {
                _dataGridSecurity.RestoreLayoutFromXml(subPath + "\\" + _fileBySecurity);
            }


            if (File.Exists((subPath + "\\" + _fileByModelInfo)))
            {
                _dataGridModelInfo.RestoreLayoutFromXml(subPath + "\\" + _fileByModelInfo);
            }

            if (File.Exists((subPath + "\\" + _fileByPriceSecurity)))
            {
                _dataGridPriceHistory.RestoreLayoutFromXml(subPath + "\\" + _fileByPriceSecurity);
            }




        }

        public void SaveXML()
        {


            if (!System.IO.Directory.Exists(subPath))
                System.IO.Directory.CreateDirectory((subPath));

            _dataGridModel.SaveLayoutToXml(subPath + "\\" + _fileModel);
            _dataGridAccount.SaveLayoutToXml(subPath + "\\" + _fileAccount);
            _dataGridSecurity.SaveLayoutToXml(subPath + "\\" + _fileBySecurity);
            _dataGridModelInfo.SaveLayoutToXml(subPath + "\\" + _fileByModelInfo);
            _dataGridPriceHistory.SaveLayoutToXml(subPath + "\\" + _fileByPriceSecurity);
        }

        public void UpdateAccoutText()
        {





        }




        # endregion HelperProcedures

        # region Methods


        public Int32 GetfilterStatus()
        {
            if (RdoAll.IsChecked == true)
                return 1;
            else if(RdoPositve.IsChecked == true)
                return 2;
            else
            {
                return 3;
            }

        }

        private void _dataGridAccount_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string account;
            string model;
            string _short_name;
            string _account_market_value;
            string _cash_shortfall;


            int index =
                Convert.ToInt32(_dataGridAccount
                    .GetRowVisibleIndexByHandle(_viewDataGridAccount.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                GridColumn account_id = _dataGridAccount.Columns["account_id"];
                GridColumn model_id = _dataGridAccount.Columns["model_id"];
                GridColumn short_name = _dataGridAccount.Columns["short_name"];
                GridColumn account_market_value = _dataGridAccount.Columns["account_market_value"];
                GridColumn cash_shortfall = _dataGridAccount.Columns["Cash_mv_shortfall"];

                if (account_id != null)
                {
                    account = _dataGridAccount.GetCellValue(index, account_id).ToString();
                    _view.AccountId = Convert.ToInt32(_dataGridAccount.GetCellValue(index, account_id).ToString());
                    _view.ModelId = Convert.ToInt32(_dataGridAccount.GetCellValue(index, model_id).ToString());
                    _account_market_value = _dataGridAccount.GetCellValue(index, account_market_value).ToString();
                    _short_name = _dataGridAccount.GetCellValue(index, short_name).ToString();
                    _cash_shortfall = _dataGridAccount.GetCellValue(index, cash_shortfall).ToString();


                    if (!string.IsNullOrEmpty(account))
                    {
                        _view.se_get_securities_on_model(_view.ModelId, _view.AccountId, GetfilterStatus());

                       


                        lblSecurityDrift.Content = string.Format("Security Drift: {0} ", _short_name);
                        LblacountMV.Content = string.Format("Account MV: {0:C2} ", Convert.ToDecimal(_account_market_value));
                        Lblcashshortfall.Content = string.Format("Cash ShortFall: {0:C2} ", Convert.ToDecimal(_cash_shortfall));
                    }
                }
            }
        }

        private void _dataGridModel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            string model;
          


            int index =
                Convert.ToInt32(_dataGridModel
                    .GetRowVisibleIndexByHandle(_viewDataGridModel.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn model_id = _dataGridModel.Columns["model_id"];
                GridColumn modelName = _dataGridModel.Columns["name"];





                if (model_id != null)
                {


                    model = _dataGridModel.GetCellValue(index, model_id).ToString();
                    string mn = _dataGridModel.GetCellValue(index, modelName).ToString();


                    string ModelName = string.Format("Models accounts: {0} ", mn);
                    lblModelaccounts.Content = ModelName;

                    if (!string.IsNullOrEmpty(model))
                    {


                        _view.se_get_accounts_on_model(Convert.ToInt32(model));
                        _view.se_get_top_of_model(Convert.ToInt32(model));

                    }


                }
            }


        }

        private void _dataGridPriceHistory_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {




            int index =
                Convert.ToInt32(_dataGridPriceHistory
                    .GetRowVisibleIndexByHandle(_viewGridPriceHistory.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                GridColumn security_id = _dataGridPriceHistory.Columns["security_id"];
                GridColumn symbol = _dataGridPriceHistory.Columns["symbol"];
                GridColumn purchase = _dataGridPriceHistory.Columns["purchase_price"];
                GridColumn target = _dataGridPriceHistory.Columns["target_price"];

                if (security_id != null)
                {

                    _view.SecurityId = Convert.ToInt32(_dataGridPriceHistory.GetCellValue(index, security_id).ToString());
                    _view.Symbol = _dataGridPriceHistory.GetCellValue(index, symbol).ToString();
                    _view.Purchase_price = Convert.ToDecimal(_dataGridPriceHistory.GetCellValue(index, purchase).ToString());
                    _view.Target_price = Convert.ToDecimal(_dataGridPriceHistory.GetCellValue(index, target).ToString());

                    UpdateSecurityTargets UST = new UpdateSecurityTargets(_view);

                    UST.Owner = Application.Current.MainWindow;
                    UST.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    UST.ShowDialog();

                }
            }
        }

        private void PrintSecurity_Click(object sender, RoutedEventArgs e)
        {

            if (_dataGridSecurity.Visibility == Visibility.Visible)
            _viewDataGridSecurity.ShowPrintPreview(this);
            else
           _viewGridPriceHistory.ShowPrintPreview(this);

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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            refreshChart();
        }

        private void TextEdit_LostFocus(object sender, RoutedEventArgs e)
        {


            _view.Get_UpdateCashPercent(Convert.ToDecimal(TxtTarget.Text));
        }

   

        #endregion Methods

      
        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index =
                    Convert.ToInt32(_dataGridAccount.GetRowVisibleIndexByHandle(_viewDataGridAccount.FocusedRowData.RowHandle.Value).ToString());



                if (index > -1)
                {

                    if (index > -1)
                    {

                        GridColumn colaccount_id = _dataGridAccount.Columns["account_id"];

                        if (colaccount_id != null)
                        {
                            if (_dataGridAccount.GetCellValue(index, colaccount_id).ToString() != null)
                            {

                                int _account_id = Convert.ToInt32(_dataGridAccount.GetCellValue(index, colaccount_id).ToString());


                                if (_account_id != -1)
                                {
                                    _view.OnOpenAppraisalReport((long)_account_id);


                                }


                            }
                        }
                    }



                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred.  " + ex.Message, MSGBOX_TITLE_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

       

       
       
    }
}