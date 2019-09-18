namespace PMDashBoard.Client.View
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


    using PMDashBoard.Client.ViewModel;
    using System.IO;
    using System.Threading;

    using PMDashBoard.Client;



    public partial class PMDashBoardVisual : UserControl
    {


        // private string MSGBOX_TITLE_ERROR = "Generic Viewer";

        string subPath = "c:\\dashboard";
        private string _fileModel = "ModelHelperModel.xaml";
        private string _fileAccount = "ModelHelperAccount.xaml";
        private string _fileBySecurity = "ModelHelperSecurity.xaml";
        private string _fileByPriceSecurity = "ModelHelperPriceSecurity.xaml";
        private string _fileByModelInfo = "ModelHelperModelInfo.xaml";
        private string _fileBySummary = "PMDashboardBySummary.xaml";
        private string _fileByHierarchy = "PMDashboardByHierarchy.xaml";

        private const string MSGBOX_TITLE_ERROR = "DataDashBoard";

        public PMDashBoardModel _view;


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

        public PMDashBoardVisual(PMDashBoardModel ViewerModel)
        {


            InitializeComponent();
           
            this.DataContext = ViewerModel;
            this._view = ViewerModel;
        
            DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

            AssignXML();

        }




        # region HelperProcedures


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


          

        
          


          

            if (File.Exists((subPath + "\\" + _fileBySummary)))
            {
                _dataGridSummary.RestoreLayoutFromXml(subPath + "\\" + _fileBySummary);
            }

            if (File.Exists((subPath + "\\" + _fileByHierarchy)))
            {
                _dataGridHierarchy.RestoreLayoutFromXml(subPath + "\\" + _fileByHierarchy);
            }

            


        }

        public void SaveXML()
        {


            if (!System.IO.Directory.Exists(subPath))
                System.IO.Directory.CreateDirectory((subPath));

          
            _dataGridSummary.SaveLayoutToXml(subPath + "\\" + _fileBySummary);
            _dataGridHierarchy.SaveLayoutToXml(subPath + "\\" + _fileByHierarchy);
        }

        public void UpdateAccoutText()
        {





        }




        # endregion HelperProcedures

        # region Methods


        private void _dataGridSummary_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string model;
            string account;

            int index =
                Convert.ToInt32(_dataGridSummary
                    .GetRowVisibleIndexByHandle(_viewDataGridSummary.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                GridColumn model_id = _dataGridSummary.Columns["model_id"];
                GridColumn modelName = _dataGridSummary.Columns["model_name"];
                GridColumn account_id = _dataGridSummary.Columns["account_id"];
                GridColumn account_name = _dataGridSummary.Columns["short_name"];
                GridColumn manager_name = _dataGridSummary.Columns["manager"];
                GridColumn hasMaturity = _dataGridSummary.Columns["has_maturity"];
                GridColumn beta = _dataGridSummary.Columns["Beta"];

                if (model_id != null)
                {
                    model = _dataGridSummary.GetCellValue(index, model_id).ToString();
                    _view.SelectedModelName = _dataGridSummary.GetCellValue(index, modelName).ToString();
                    _view.SelectedManagerName = _dataGridSummary.GetCellValue(index, manager_name).ToString();
                    _view.SelectedHasMaturity = _dataGridSummary.GetCellValue(index, hasMaturity).ToString();
                    _view.SelectedBeta= _dataGridSummary.GetCellValue(index, beta).ToString();

                    account = _dataGridSummary.GetCellValue(index, account_id).ToString();
                    _view.SelectedAccountID = Convert.ToInt32(_dataGridSummary.GetCellValue(index, account_id).ToString());
                    _view.SelectedAccountName = _dataGridSummary.GetCellValue(index, account_name).ToString();
                   

                    if (!string.IsNullOrEmpty(model))
                    {
                        _view.se_get_top_of_model(_view.SelectedAccountID);
                        _view.se_get_account_hierarchy(_view.SelectedAccountID);
                        _view.se_get_securities_on_model(Convert.ToInt32(model),_view.SelectedAccountID, 1);
                        _view.se_benchmark_vs_holdings();
                    }
                }
            }

        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index =
                    Convert.ToInt32(_dataGridSummary.GetRowVisibleIndexByHandle(_viewDataGridSummary.FocusedRowData.RowHandle.Value).ToString());



                if (index > -1)
                {

                    if (index > -1)
                    {

                        GridColumn colaccount_id = _dataGridSummary.Columns["account_id"];

                        if (colaccount_id != null)
                        {
                            if (_dataGridSummary.GetCellValue(index, colaccount_id).ToString() != null)
                            {

                                int _account_id = Convert.ToInt32(_dataGridSummary.GetCellValue(index, colaccount_id).ToString());


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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
            }

           



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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _view.se_get_dashoard_summary();
        }

        #endregion Methods

       

      
      
     
       

       
       
    }
}