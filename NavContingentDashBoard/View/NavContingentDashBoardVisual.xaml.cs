namespace NavContingentDashBoard.Client.View
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


    using NavContingentDashBoard.Client.ViewModel;
    using System.IO;
    using System.Threading;

    using NavContingentDashBoard.Client;



    public partial class NavContingentDashBoardVisual : UserControl
    {


        // private string MSGBOX_TITLE_ERROR = "Generic Viewer";

        string subPath = "c:\\dashboard";
        private string _fileContingentStatus = "ContingentStatus.xaml";
        private string _fileIndicativeGrid = "IndicativeGrid.xaml";
      

        private const string MSGBOX_TITLE_ERROR = "Contingent DashBoard";

        public NavContingentDashBoardModel _view;


        # region Parameters

      
     

        private DataSet _allSecurities;

        public DataSet AllSecurities
        {
            set { _allSecurities = value; }
            get { return _allSecurities; }
        }

        DateTime _startDate;

        public DateTime StartDate
        {
            set { _startDate = value; }
            get { return _startDate; }
        }


        # endregion Parameters

        public NavContingentDashBoardVisual(NavContingentDashBoardModel ViewerModel)
        {


            InitializeComponent();
           
            this.DataContext = ViewerModel;
            this._view = ViewerModel;
        
            DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

            AssignXML();

        }


        # region HelperProcedures


      



        private void start_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDates();
            _view.se_get_contingent_status(_view.Parameters.StartDate);
        }

        void getDates()
        {




            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(Convert.ToString(_view.Parameters.StartDate), out intStr);
            if (intResultTryParse == true)
            {
                StartDate = System.Convert.ToDateTime(intStr);

            }
            else
            {
                StartDate = System.Convert.ToDateTime(DateTime.Now);





            }
        }
        private void Get_From_info()
        {

            _view.se_get_contingent_status(_view.Parameters.StartDate);
         
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

            if (File.Exists((subPath + "\\" + _fileContingentStatus)))
            {
                _dataGridContingentStatus.RestoreLayoutFromXml(subPath + "\\" + _fileContingentStatus);
            }

            if (File.Exists((subPath + "\\" + _fileIndicativeGrid)))
            {
                IndicativeGrid.RestoreLayoutFromXml(subPath + "\\" + _fileIndicativeGrid);
            }



        }

        public void SaveXML()
        {


            if (!System.IO.Directory.Exists(subPath))
                System.IO.Directory.CreateDirectory((subPath));

            _dataGridContingentStatus.SaveLayoutToXml(subPath + "\\" + _fileContingentStatus);

            IndicativeGrid.SaveLayoutToXml(subPath + "\\" + _fileIndicativeGrid);
           
         
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

        private void GridContingentStatus_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string account;
          
         

            int index =
                Convert.ToInt32(_dataGridContingentStatus
                    .GetRowVisibleIndexByHandle(_viewGridContingentStatus.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                GridColumn account_id = _dataGridContingentStatus.Columns["account_id"];
           

                if (account_id != null)
                {
                    account = _dataGridContingentStatus.GetCellValue(index, account_id).ToString();
                    _view.AccountId = Convert.ToInt32(_dataGridContingentStatus.GetCellValue(index, account_id).ToString());
                  


                    if (!string.IsNullOrEmpty(account))
                    {
                        _view.se_get_indicative_nav(_view.Parameters.StartDate, _view.AccountId);


                    }
                }
            }
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
            //refreshChart();
        }

      

   

        #endregion Methods

      
        //private void OpenReport_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        int index =
        //            Convert.ToInt32(_dataGridAccount.GetRowVisibleIndexByHandle(_viewDataGridAccount.FocusedRowData.RowHandle.Value).ToString());



        //        if (index > -1)
        //        {

        //            if (index > -1)
        //            {

        //                GridColumn colaccount_id = _dataGridAccount.Columns["account_id"];

        //                if (colaccount_id != null)
        //                {
        //                    if (_dataGridAccount.GetCellValue(index, colaccount_id).ToString() != null)
        //                    {

        //                        int _account_id = Convert.ToInt32(_dataGridAccount.GetCellValue(index, colaccount_id).ToString());


        //                        if (_account_id != -1)
        //                        {
        //                            _view.OnOpenAppraisalReport((long)_account_id);


        //                        }


        //                    }
        //                }
        //            }



        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error has occurred.  " + ex.Message, MSGBOX_TITLE_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //}

       

       
       
    }
}