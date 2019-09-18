namespace CurrentOrders.Client.View
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
    using DevExpress.Data;
    using DevExpress.Xpf.Core;

    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
  
    using CurrentOrders.Client.ViewModel;
    using System.IO;
    using System.Threading;
  
     using CurrentOrders.Client;
  


    public partial class CurrentOrdersViewerVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "CurrentOrders.xaml";
         private const string MSGBOX_TITLE_ERROR = "Current Orders";
        
         public CurrentOrdersViewerModel _view;


         # region Parameters

        

         private DataSet _allSecurities;
         public DataSet AllSecurities
         {
             set { _allSecurities = value; }
             get { return _allSecurities; }
         }

    
         private Int32 _selectedAccountId = -1;
         public Int32 SelectedAccountId
         {
             set { _selectedAccountId = value; }
             get { return _selectedAccountId; }
         }

         private Int32 _orderid = -1;
         public Int32 Orderid
         {
             set { _orderid = value; }
             get { return _orderid; }
         }

         

         private string _selectAccountName = string.Empty;
         public string SelectAccountName
         {
             set { _selectAccountName = value; }
             get { return _selectAccountName; }
         }
         # endregion Parameters

         public CurrentOrdersViewerVisual(CurrentOrdersViewerModel ViewerModel)
         {


             InitializeComponent();
             this.DataContext = ViewerModel;
             this._view = ViewerModel;
             ApplicationThemeHelper.ApplicationThemeName = _view.Parameters.DefaultTheme;
             GetSecurities();
             this.comboBoxEdit1.Text = _view.AccountName;
              DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

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


                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId));
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

        

        private void comboBoxEdit1_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


            Validate(comboBoxEdit.Text);
        }


        #endregion Account

    

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
 
        }

    
        public void GetSecurities()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    
                    AllSecurities = _view.DBService.GetSecurityInfo("");

                  

                });
        }

 
        //}


       
        private void AssignXML()
        {


            if (File.Exists((subPath + "\\" + file)))
            {

                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);

            }

          
        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);

            }
        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _view.AccountName;


        }

      

       
        # endregion HelperProcedures



        # region Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId));
        }

        private void Button_Save_XML(object sender, RoutedEventArgs e)
        {
            SaveXML();
        }
        private void Approved_MenuItem_Click(object sender, RoutedEventArgs e)
        {

            int count = 0;
            // int selectedrow = 0;

            decimal orderID;
            decimal account_id;
         
            decimal security_id;

            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {


               

                GridColumn colaccount_id = _dataGrid.Columns["account_id"];
                GridColumn colorder_id = _dataGrid.Columns["order_id"];
                GridColumn colsecurity_id = _dataGrid.Columns["security_id"];


                if (colaccount_id != null)
                {
                    account_id = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colaccount_id).ToString());
                    orderID = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colorder_id).ToString());
                    security_id = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colsecurity_id).ToString());


                    _view.validate_proposed_order(Convert.ToInt32(orderID), Convert.ToInt32(account_id), 1, Convert.ToInt32(security_id), 1);
                            
                    count = count + 1;
                }

            }

            Thread.Sleep(1000);
            string LABEL = string.Format("You have Approved {0} orders.", count);
            System.Windows.MessageBox.Show(LABEL, "Approved", MessageBoxButton.OK, MessageBoxImage.Information);

            _view.Get_ChartData(Convert.ToInt32(_view.AccountId));

        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int index =
                Convert.ToInt32(_dataGrid.GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());



                if (index > -1)
                {

                    if (index > -1)
                    {

                        GridColumn colaccount_id = _dataGrid.Columns["account_id"];

                        if (colaccount_id != null)
                        {
                            if (_dataGrid.GetCellValue(index, colaccount_id).ToString() != null)
                            {

                                int _account_id = Convert.ToInt32(_dataGrid.GetCellValue(index, colaccount_id).ToString());


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

   

        private void Denied_MenuItem_Click(object sender, RoutedEventArgs e)
        {


            int count = 0;
            decimal orderID;
            decimal account_id;
         
            decimal security_id;


            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {


               

                GridColumn colaccount_id = _dataGrid.Columns["account_id"];
                GridColumn colorder_id = _dataGrid.Columns["order_id"];
                GridColumn colsecurity_id = _dataGrid.Columns["security_id"];




                if (colaccount_id != null)
                {
                    account_id = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colaccount_id).ToString());
                    orderID = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colorder_id).ToString());
                    security_id = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colsecurity_id).ToString());

                    _view.validate_proposed_order(Convert.ToInt32(orderID), Convert.ToInt32(account_id), 1, Convert.ToInt32(security_id), 2);


                    count = count + 1;
                }

            }

            Thread.Sleep(1000);
            string LABEL = string.Format("You have Denied{0} orders.", count);
            System.Windows.MessageBox.Show(LABEL, "Denied", MessageBoxButton.OK, MessageBoxImage.Information);

            _view.Get_ChartData(Convert.ToInt32(_view.AccountId));

        }

        private void Delete_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            int orderID;
            string _orderType;
      


            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();


       

            for (i = 0; i < listRowList.Length; i++)
            {

                GridColumn colorder_id = _dataGrid.Columns["order_id"];
                GridColumn colorder_type = _dataGrid.Columns["Order Type"];

                if (colorder_id != null)
                {

                    orderID = Convert.ToInt32(_dataGrid.GetCellValue(listRowList[i], colorder_id).ToString());
                    Orderid = orderID;
                    _orderType = Convert.ToString(_dataGrid.GetCellValue(listRowList[i], colorder_type).ToString());
              
                    _view.deleteOrders(orderID, _orderType);                 
                    count = count + 1;

                }

            }

            Thread.Sleep(1000);
            string LABEL = string.Format("You have Deleted {0} orders.", count);
            System.Windows.MessageBox.Show(LABEL, "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

            _view.Get_ChartData(Convert.ToInt32(_view.AccountId));
        }

    
      

        private void Get_pre_trade_anlytics(object sender, RoutedEventArgs e)
        {


            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            string[] symbol = new string[listRowList.Length];
            string[] quantity = new string[listRowList.Length];
            string[] country = new string[listRowList.Length];
            decimal qty;

            for (i = 0; i < listRowList.Length; i++)
            {



                GridColumn colsymbol = _dataGrid.Columns["Symbol1"];
                GridColumn colQuantity = _dataGrid.Columns["Quantity"];
                GridColumn colCountry = _dataGrid.Columns["Country"];
                GridColumn coldirection = _dataGrid.Columns["side_code"];


                if (colsymbol != null)
                {
                    symbol[i] = Convert.ToString(_dataGrid.GetCellValue(listRowList[i], colsymbol).ToString());
                    quantity[i] = Convert.ToString(_dataGrid.GetCellValue(listRowList[i], colQuantity).ToString());
                    country[i] = Convert.ToString(_dataGrid.GetCellValue(listRowList[i], colCountry).ToString());
                    string direction = Convert.ToString(_dataGrid.GetCellValue(listRowList[i], coldirection).ToString());

                    switch (int.Parse(direction))
                    {

                        case 2:
                            qty = Convert.ToDecimal(quantity[i]); //sell
                            qty = qty * -1;
                            quantity[i] = Convert.ToString(qty);
                            break;
                        case 3:
                            qty = Convert.ToDecimal(quantity[i]);
                            qty = qty * -1;
                            quantity[i] = Convert.ToString(qty); //sell mv
                            break;
                        case 4:
                            qty = Convert.ToDecimal(quantity[i]);
                            qty = qty * -1;
                            quantity[i] = Convert.ToString(qty); //BUY TO COVER
                            break;
                        case 5:
                            qty = Convert.ToDecimal(quantity[i]);
                            qty = qty * -1;
                            quantity[i] = Convert.ToString(qty); //BUY cover MV
                            break;

                        default:
                            break;
                    }

                }

            }


            frmLogic XWindow = new frmLogic(symbol, quantity, country);

            XWindow.ShowDialog();


        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId));
        }

        private void btnSubmitOrders_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            decimal orderID;
            decimal account_id;
            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();

            for (i = 0; i < listRowList.Length; i++)
            {



                GridColumn colaccount_id = _dataGrid.Columns["account_id"];
                GridColumn colorder_id = _dataGrid.Columns["order_id"];



                if (colaccount_id != null)
                {
                    account_id = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colaccount_id).ToString());
                    orderID = Convert.ToDecimal(_dataGrid.GetCellValue(listRowList[i], colorder_id).ToString());



                    _view.se_send_proposed(account_id, orderID);
                    count = count + 1;
                }

            }

            Thread.Sleep(1000);
            string LABEL = string.Format("You have Submitted {0} orders.", count);
            System.Windows.MessageBox.Show(LABEL, "Submit Orders", MessageBoxButton.OK, MessageBoxImage.Information);
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId));

        }

       
        private void ButtonGenerateProposed(object sender, RoutedEventArgs e)
        {
            string accountName;
            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {

                // ApplicationMessageList messages = null;
             

                GridColumn colaccountName = _dataGrid.Columns["Account Name"];





                if (colaccountName != null)
                {
                    accountName = _dataGrid.GetCellValue(listRowList[i], colaccountName).ToString();

                    string path = "c:\\DashBoard\\Reports";


                    if (File.Exists((path + "\\" + accountName + ".pdf")))
                    {

                        //          using (var fileStream = new FileStream(@"C:\DashBoard\Reports\wm01.pdf", FileMode.Open, FileAccess.Read))
                        //          {
                        //    System.Diagnostics.Process.Start(@"C:\DashBoard\Reports\wm01.pdf");

                        //}
                        System.Diagnostics.Process.Start(@path + "\\" + accountName + ".pdf");
                        return;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Please Select a report", "Report Generation", MessageBoxButton.OK, MessageBoxImage.Information);
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
        #endregion Methods

      

    

    
      

       

      

        



    

    }
}
