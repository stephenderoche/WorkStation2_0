namespace RebalanceInfoSend.Client.View
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
    using RebalanceInfoSend.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using RebalanceInfoSend.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Utils.Themes;

    using DevExpress.Xpf.Core;
   
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
    using System.Threading.Tasks;
    using System.Diagnostics;


    public partial class RebalanceInfoVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "RebalOP.xaml";
        string file1 = "RebalOPdetail.xaml";
        string file2 = "RebalRestriction.xaml";
    
         # region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public RebalanceInfoSendViewerModel _vm;

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

         # endregion Parameters

         public RebalanceInfoVisual(RebalanceInfoSendViewerModel securityHistoryViewerModel)
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

        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
           // this.cmboDesk.Items.Clear();
            if(cmboDesk.Items.Count != 0)
                this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    ApplicationMessageList messages = null;

                    DataSet ds = _vm.DBService.se_rebal_sessions(-1,out messages);
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["session"];
                            object desk_id = row["rebal_session_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new RebalanceInfoSend.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

                                count = count + 1;

                                if (_vm.Parameters.DeskName == Convert.ToString(item))
                                {
                                    this.cmboDesk.SelectedIndex = count;
                                    _vm.DeskId = Convert.ToInt32(desk_id);
                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }

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
                _dataOP.RestoreLayoutFromXml(subPath + "\\" + file);
            }

            if (File.Exists((subPath + "\\" + file1)))
            {
                _dataOPDetail.RestoreLayoutFromXml(subPath + "\\" + file1);
            }

            if (File.Exists((subPath + "\\" + file2)))
            {
                _dataCompDetail.RestoreLayoutFromXml(subPath + "\\" + file2);
            }
     

        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataOP.SaveLayoutToXml(subPath + "\\" + file);

                _dataOPDetail.SaveLayoutToXml(subPath + "\\" + file1);
                _dataCompDetail.SaveLayoutToXml(subPath + "\\" + file2);
            }
            else
            {
                _dataOP.SaveLayoutToXml(subPath + "\\" + file);
                _dataOPDetail.SaveLayoutToXml(subPath + "\\" + file1);
                _dataCompDetail.SaveLayoutToXml(subPath + "\\" + file2);
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

     

        private void _dataOP_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            

            int index =
                Convert.ToInt32(_dataOP
                    .GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                

                GridColumn colsecurity_id = _dataOP.Columns["security_id"];

                if (colsecurity_id != null)
                {

                    _vm.Security_id = Convert.ToDecimal(_dataOP.GetCellValue(index, colsecurity_id).ToString());

                   // _vm.Se_get_rebal_detail_info(Convert.ToInt32(_vm.Security_id), _vm.DeskId);


                  
                }
               
            }

            _vm.Se_get_rebal_detail_info(Convert.ToInt32(_vm.Security_id), _vm.DeskId);
        }

        void column_CustomGetSerializableProperties(object sender, CustomGetSerializablePropertiesEventArgs e)
        {
            e.SetPropertySerializable(GridColumn.EditSettingsProperty, new DXSerializable());

        }

        private void cmboDesk_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmboDesk.Items.Count == 0)
            {
                //DeskList();
                //Thread.Sleep(1000);
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboDesk.SelectedItem).ToString()))
                {
                    _vm.Desk = (cmboDesk.SelectedItem).ToString();
                    _vm.DeskId = Convert.ToInt32(((RebalanceInfoSend.Client.ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);
                    _vm.Parameters.DeskName = _vm.Desk;
                    //_vm.Get_DeskData(DeskId);
                   //get_rebal_info_data();
                   //_vm.se_get_adv_rebal_op(1, _vm.DeskId);
                    //_vm.se_rebal_sessions(DeskId);

                }
            }
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

            DeskList();

           //_vm.se_get_adv_rebal_op(1, _vm.DeskId);
          // get_rebal_info_data();


        }

        private void p_Exited(object sender, EventArgs e)
        {

            _vm.set_indicator_state(false);
        }


        private async void BtnRefreshOP_Click(object sender, RoutedEventArgs e)
        {
            //_vm.se_get_adv_rebal_op(1, _vm.DeskId);

            Process process = new Process();
            process.Exited += new EventHandler(p_Exited);
            //process.StartInfo.FileName = _path;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            _vm.set_indicator_state(true);
            //Thread.Sleep(1000);

            var someObject = _vm.Detail;
            if (someObject != null)
            {
                _vm.Detail.Clear();
               
            }
           
            await Task.Run(() => _vm.se_get_adv_rebal_op(1, _vm.DeskId));
            await Task.Run(() =>  get_rebal_info_data());
           
            _vm.set_indicator_state(false);
       
            
            //_vm.UpdateMainGrid();
        }


        private async void LoadAccountsAndDesk()
        {
            //_vm.se_get_adv_rebal_op(1, _vm.DeskId);

            Process process = new Process();
          
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
        
            await Task.Run(() => _vm.se_GetAcc(_vm.AccountName));
            await Task.Run(() => DeskList());
         
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            _vm.getSecurity_id(this._dataOP);
            _vm.Send_Click(this._dataOP);
            _vm.se_get_adv_rebal_op(1, _vm.DeskId);

        }

        private void Complince_Button_Click(object sender, RoutedEventArgs e)
        {

            _vm.RunCompliance(_vm.AccountId);
        }

        private void Send_All_Click(object sender, RoutedEventArgs e)
        {

        




            _vm.getAllSecurity_id(this._dataOP);
            _vm.Send_Click(this._dataOP);
            _vm.se_get_adv_rebal_op(1, _vm.DeskId);
        }

        #endregion Methods

        private void Override_Click(object sender, RoutedEventArgs e)
        {
            RebalanceInfoSend.Client.View.Restriction GenericWindow = new Restriction(this._vm, this);
            GenericWindow.Show();


            //int count = 0;
            //// int selectedrow = 0;

            //int _a_id;
            //int _o_id;
            //int _s_id;

            //int i = 0;
            //int[] listRowList = this._dataCompDetail.GetSelectedRowHandles();
            //for (i = 0; i < listRowList.Length; i++)
            //{




            //    GridColumn colaccount_id = _dataCompDetail.Columns["single_account_id"];
            //    GridColumn colorder_id = _dataCompDetail.Columns["order_id"];
            //    GridColumn colsecurity_id = _dataCompDetail.Columns["security_id"];


            //    if (colaccount_id != null)
            //    {
            //        _a_id = Convert.ToInt32(_dataCompDetail.GetCellValue(listRowList[i], colaccount_id).ToString());
            //        _o_id = Convert.ToInt32(_dataCompDetail.GetCellValue(listRowList[i], colorder_id).ToString());
            //       _s_id = Convert.ToInt32(_dataCompDetail.GetCellValue(listRowList[i], colsecurity_id).ToString());


            //        _vm.se_add_first_override(_a_id,_s_id,_vm.Reason,_o_id);

            //        count = count + 1;
            //    }

            //}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _vm.Se_get_restriction_report();
        }

        //private void cmboDesk_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{

        //}

       

     
       

        

    }

}
