namespace RestrictedSecurity.Client.View
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
    using RestrictedSecurity.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using RestrictedSecurity.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
   
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
    using System.Threading.Tasks;
    using System.Diagnostics;


    public partial class RestrictedSecurityVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "RestrictedSecurity.xaml";
      
      
    
         # region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public RestrictedSecurityViewerModel _vm;

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

         public RestrictedSecurityVisual(RestrictedSecurityViewerModel securityHistoryViewerModel)
         {


             InitializeComponent();

             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
   
           
          
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
                _dataOP.RestoreLayoutFromXml(subPath + "\\" + file);
            }

          

        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataOP.SaveLayoutToXml(subPath + "\\" + file);

                
            }
            else
            {
                _dataOP.SaveLayoutToXml(subPath + "\\" + file);
                
            }
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

        private void p_Exited(object sender, EventArgs e)
        {

            _vm.set_indicator_state(false);
        }

        private  void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            

          _vm.Se_get_restricted_security();
         
           
        }

        private void BtnReRunRules_Click(object sender, RoutedEventArgs e)
        {
            _vm.Se_run_rules();
            //int a = 0;
          
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

      

       

     
       

        

    }

}
