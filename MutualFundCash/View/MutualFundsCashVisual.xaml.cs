namespace MutualFundsCash.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
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
    using DevExpress.Xpf.Editors.Settings;
    using MutualFundsCash.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using MutualFundsCash.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Utils.Themes;
  
    using DevExpress.Xpf.Core;
   
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
    using System.Threading.Tasks;
    using System.Diagnostics;


    public partial class MutualFundsCashVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "ParentFunds.xaml";
        string file1 = "ChildFunds.xaml";
        string file2 = "CashbyFunds.xaml";
        private const string MSGBOX_TITLE_ERROR = "Mutual Funds";
        #region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public MutualFundsCashViewerModel _vm;

        string _header = string.Empty;
        public string Header
        {
            set { _header = value; }
            get { return _header; }
        }

        int _colIndex = -1;
        public int ColIndex
        {
            set { _colIndex = value; }
            get { return _colIndex; }
        }
        
      
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

         DateTime _endDate;
         public DateTime EndDate
         {
             set { _endDate = value; }
             get { return _endDate; }
         }


       

         string _xml;
         public string XML
         {
             set { _xml = value; }
             get { return _xml; }
         }



         private int _security_id = -1;
         public int SecurityId
         {
             get { return _security_id; }
             set { _security_id = value; }
         }

      

    
         private string _symbol = string.Empty;
         public string Symbol
         {
             get { return _symbol; }
             set { _symbol = value; }
         }


       

        private int _blockId = -1;
        public int BlockId
        {
            get { return _blockId; }
            set { _blockId = value; }
        }

        private int _brokerId = -1;
        public int BrokerId
        {
            get { return _brokerId; }
            set { _brokerId = value; }
        }

        private bool _includeOrders;
        public bool IncludeOrders
        {
            set { _includeOrders = value; }
            get { return _includeOrders; }
        }

        private bool _allFunds;
        public bool AllFunds
        {
            set { _allFunds = value; }
            get { return _allFunds; }
        }

        #endregion Parameters

        public MutualFundsCashVisual(MutualFundsCashViewerModel securityHistoryViewerModel)
         {


             InitializeComponent();

             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
             ApplicationThemeHelper.ApplicationThemeName = _vm.Parameters.DefaultTheme;
           
          
             AssignXML();
             
             LoadAccountsAndDesk();

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
                                 _vm.DBService.ValidateAccountForUser(ourTextBox, out defaultAccountId, out defaultShortName);
                                 if (defaultAccountId != -1)
                                 {

                                     _vm.AccountId = defaultAccountId;
                                     _vm.AccountName = defaultShortName;
                                     _vm.Parameters.AccountName = _vm.AccountName;

                                     // Get_chart_data();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _vm.AccountId = -1;
                                     _vm.AccountName = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.AccountId = -1;
                                 _vm.AccountName = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _vm.AccountId = -1;
                     _vm.AccountName = "";
                     throw ex;
                 }



                 return retval;
             }
             else
             {
                 _vm.AccountId = -1;
                 _vm.AccountName = "";
             }

             return retval;
         }

         private void comboBoxEdit1_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
         {
             _vm.Parameters.AccountName = comboBoxEdit1.Text;
             string textEnteredPlusNew = "short_name Like '" + _vm.Parameters.AccountName + "%'";
             //string textEnteredPlusNew = "short_name Like '" + comboBoxEdit1.Text + "%'";
             string textEnteredPlus = _vm.Parameters.AccountName;
             this.comboBoxEdit1.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllAccounts;
                     if (someObject == null)
                     {
                         _vm.se_GetAcc(textEnteredPlus);
                         return;
                     }

                     int l = _vm.AllAccounts.Tables[0].Select(textEnteredPlusNew).Length;
                     if (l == 0)
                     {

                         _vm.se_GetAcc(_vm.Parameters.AccountName);

                     }

                     foreach (DataRow row in _vm.AllAccounts.Tables[0].Select(textEnteredPlusNew))
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

             Validate(_vm.Parameters.AccountName);
             //Validate(textEnteredPlus);

         }

         //public void get_account_name(string account_id)
         //{
         //    string textEnteredPlusNew = "account_id = " + account_id;

         //    ThreadPool.QueueUserWorkItem(
         //        delegate(object eventArg)
         //        {
         //            var someObject = _vm.AllAccounts;
         //            if (someObject == null)
         //            {
         //                _vm.GetAcc();
         //                return;
         //            }
         //            foreach (DataRow row in _vm.AllAccounts.Tables[0].Select(textEnteredPlusNew))
         //            {
         //                object item = row["short_name"];
         //                _vm.AccountName = Convert.ToString(item);
         //                _vm.Parameters.AccountName = _vm.AccountName;

         //                Dispatcher.BeginInvoke(new Action(() =>
         //                {

         //                }), System.Windows.Threading.DispatcherPriority.Normal);
         //            }

         //        });
         //}

         private void comboBoxEdit1_LostFocus(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             
         }

         private void comboBoxEdit1_SelectedIndexChanged(object sender, RoutedEventArgs e)
         {
             Validate(_vm.Parameters.AccountName);
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

            Symbol = String.IsNullOrEmpty(_vm.Parameters.SecurityName) ? String.Empty : _vm.Parameters.SecurityName;
            Validate(_vm.Parameters.AccountName);
        }

      

        private void AssignXML()
        {
            if (File.Exists((subPath + "\\" + file)))
            {
                ParentViewer.RestoreLayoutFromXml(subPath + "\\" + file);
            }

            if (File.Exists((subPath + "\\" + file1)))
            {
                OrderViewer.RestoreLayoutFromXml(subPath + "\\" + file1);
            }

            if (File.Exists((subPath + "\\" + file2)))
            {
                fundsByFOF.RestoreLayoutFromXml(subPath + "\\" + file2);
            }


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                ParentViewer.SaveLayoutToXml(subPath + "\\" + file);
                OrderViewer.SaveLayoutToXml(subPath + "\\" + file1);
                fundsByFOF.SaveLayoutToXml(subPath + "\\" + file2);
            }
            else
            {
                ParentViewer.SaveLayoutToXml(subPath + "\\" + file);
                OrderViewer.SaveLayoutToXml(subPath + "\\" + file1);
                fundsByFOF.SaveLayoutToXml(subPath + "\\" + file2);
            }
        }

        public void get_rebal_info_data()
        {



            ApplicationMessageList messages = null;



            _vm.Rebalinfo = _vm.DBService.rebal_info(1,
              Convert.ToInt32(_vm.DeskId),
              out messages);

            //this.lblSessionId.Content = _vm.Rebalinfo.Rebal_seesion_id;
            //this.lblDate.Content = Convert.ToString(Rebalinfo.Completion_date.ToString("MM/dd/yyyy"));
            //this.lblinitby.Content = Rebalinfo.Owner_name;
            //this.lblModel.Content = Rebalinfo.Model_name;
            //this.lblLBLgenerateorders.Content = Rebalinfo.Prevent_neg_cash;
            //this.lblExcludeEncumbered.Content = Rebalinfo.Exclude_encumbered;

            //this.lblOrderDirection.Content = Rebalinfo.Order_direction;
            //this.lblNormalizeTargets.Content = Rebalinfo.Normalize;
            //this.lblClearProformas.Content = Rebalinfo.ClearPropsed;
            //this.lblsellodd.Content = Rebalinfo.Sell_odd;
            //this.lblNonModel.Content = Rebalinfo.Sell_non_holdings;
            //this.lblaccrued.Content = Rebalinfo.IncludeAccrued;
            //this.lblredistribute.Content = Rebalinfo.Redistribute_dis_mv;


            //this.lblRestritiontype.Content = Rebalinfo.Restricted_list_processing;
            //this.lblSeverityLevel.Content = Rebalinfo.Severity_level;
            //this.lblMinType.Content = "Stated # Shares";
            //this.lblMinQty.Content = Rebalinfo.Min_qty;
            //this.lblRoundingType.Content = "Stated Qty Shares";
            //this.lblroundQty.Content = Rebalinfo.Round_qty;
            //this.lbltotalaccounts.Content = Convert.ToString(Rebalinfo.Prevent_over); ;


        }
        # endregion HelperProcedures



        # region Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Get_From_info();
            Validate(_vm.Parameters.AccountName);
            
        }

     

   
        void column_CustomGetSerializableProperties(object sender, CustomGetSerializablePropertiesEventArgs e)
        {
            e.SetPropertySerializable(GridColumn.EditSettingsProperty, new DXSerializable());

        }

   
        static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            GridColumn column = e.Item.Tag as GridColumn;
            ColumnBehavior.SetIsRenameEditorActivated(column, !ColumnBehavior.GetIsRenameEditorActivated(column));
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
    

        private void OnRenameEditorLostFocus(object sender, RoutedEventArgs e)
        {
            TextEdit edit = sender as TextEdit;
            edit.Visibility = System.Windows.Visibility.Hidden;

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

        private void grid_AutoGeneratingColumn_1(object sender, DevExpress.Xpf.Grid.AutoGeneratingColumnEventArgs e)
        {
            //if (e.Column.FieldName == "col2")
            //{
            //    var te = new TextEditSettings();
            //    te.DisplayFormat = "n2";
            //    e.Column.EditSettings = te;
            //}

            var te = new TextEditSettings();

          
                string _header = e.Column.FieldName;
                if (_header.Substring(0, 2) == "CC")
                {
                    if (_header.Substring(2, 3) == "N")
                    {
                        te.DisplayFormat = "n2";
                        e.Column.EditSettings = te;
                    }

                    if (_header.Substring(2, 3) == "P")
                    {
                        te.DisplayFormat = "p2";
                        e.Column.EditSettings = te;
                    }
             
            }


        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {


           //_vm.se_get_adv_rebal_op(1, _vm.DeskId);
          // get_rebal_info_data();


        }

      

     


        private async void LoadAccountsAndDesk()
        {
            //_vm.se_get_adv_rebal_op(1, _vm.DeskId);

            Process process = new Process();
          
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
        
            await Task.Run(() => _vm.se_GetAcc(_vm.AccountName));
         
         
        }




        #endregion Methods

   

        private void ParentViewer_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //InitializeComponent();

            int _PaccountId;
            try
            {
                int index =
                Convert.ToInt32(ParentViewer.GetRowVisibleIndexByHandle(viewParent.FocusedRowData.RowHandle.Value).ToString());



                if (index > -1)
                {

                    if (index > -1)
                    {

                        GridColumn colaccount_id = ParentViewer.Columns["account_id"];



                        if (colaccount_id != null)
                        {
                            if ((ParentViewer.GetCellValue(index, colaccount_id).ToString() != null))
                            {
                                _PaccountId = Convert.ToInt32(ParentViewer.GetCellValue(index, colaccount_id).ToString());
                                if (!AllFunds)
                                    Orders(_PaccountId);
                                else
                                    Orders(Convert.ToInt32(_vm.AccountId));


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

        private void btnSubmitOrders_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            decimal orderID;
            decimal account_id;
            ApplicationMessageList messages = null;
            int i = 0;
            int[] listRowList = this.OrderViewer.GetSelectedRowHandles();

            for (i = 0; i < listRowList.Length; i++)
            {

                


                GridColumn colaccount_id = OrderViewer.Columns["account_id"];
                GridColumn colorder_id = OrderViewer.Columns["order_id"];



                if (colaccount_id != null)
                {
                    account_id = Convert.ToDecimal(OrderViewer.GetCellValue(listRowList[i], colaccount_id).ToString());
                    orderID = Convert.ToDecimal(OrderViewer.GetCellValue(listRowList[i], colorder_id).ToString());



                    _vm.DBService.se_send_proposed(account_id, orderID, out messages);
                    count = count + 1;
                }

               

        }

            _vm.AccountUniqueID(this.OrderViewer);
            Thread.Sleep(1000);

            _vm.addCash(this.OrderViewer);

            string LABEL = string.Format("You have Submitted {0} orders.", count);


            //update




            gridtransaction();

            System.Windows.MessageBox.Show(LABEL, "Submit Orders", MessageBoxButton.OK, MessageBoxImage.Information);

        }

    

        public void gridtransaction()
        {

            ThreadPool.QueueUserWorkItem(
                delegate (object eventArg)
                {
                    
                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet pv = new DataSet();
                    DataSet fof = new DataSet();
                    pv = _vm.DBService.se_get_parent_mutual_funds_info(
                      Convert.ToInt32(_vm.AccountId),
                      IncludeOrders,
                      out messages);
                    // this.AccountHeader.DataContext = AccountHeader;
                    fof = _vm.DBService.se_get_funds_cash_by_fof(
                     Convert.ToInt32(_vm.AccountId),
                     Convert.ToInt32(IncludeOrders),
                     out messages);




                    if (pv.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {

                            this.ParentViewer.ItemsSource = pv.Tables[0];




                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.ParentViewer.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);



                    if (fof.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {

                            this.fundsByFOF.ItemsSource = fof.Tables[0];




                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.fundsByFOF.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }

        public void Orders(int account)
        {

            ThreadPool.QueueUserWorkItem(
                delegate (object eventArg)
                {
                    
                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet pv = new DataSet();

                    pv = _vm.DBService.se_get_parent_mutual_funds_orders_info(
                      Convert.ToInt32(account),
                      IncludeOrders,
                      out messages);
                    // this.AccountHeader.DataContext = AccountHeader;





                    if (pv.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {

                            this.OrderViewer.ItemsSource = pv.Tables[0];




                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.OrderViewer.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);



                });
        }

        private void chkIncludeOrders_Unchecked(object sender, RoutedEventArgs e)
        {
            IncludeOrders = false;
            gridtransaction();
        }

        private void chkIncludeOrders_Checked(object sender, RoutedEventArgs e)
        {
            IncludeOrders = true;
            gridtransaction();
        }

        private void chkAllfunds_Unchecked(object sender, RoutedEventArgs e)
        {
            AllFunds = false;
        }

        private void chkAllfunds_Checked(object sender, RoutedEventArgs e)
        {
            AllFunds = true;
        }

        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            gridtransaction();
        }

    }

}
