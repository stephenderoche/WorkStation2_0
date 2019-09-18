namespace TraderVsPM.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
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
    using TraderVsPM.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using TraderVsPM.Client;
    using DevExpress.Xpf.Utils.Themes;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Core;
   
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
    using System.Threading.Tasks;
    using System.Diagnostics;


    public partial class TraderVsPMVisual : UserControl
    {
        private const string MSGBOX_TITLE_ERROR = "Trader Vs. PM";
        string subPath = "c:\\dashboard";
        string file = "TraderVsPm.xaml";
        string file1 = "Journaling.xaml";
    
   
        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;

        private string getissuerTextinfo = "";
        private int getissuerLenthinfo = 0;

        public TraderVsPMViewerModel _vm;

        private string _desk = string.Empty;
        public string Desk
        {
            get { return _desk; }
            set { _desk = value; }
        }
      

        private int _block_id;
        public int BlockID
        {
            get { return _block_id; }
            set { _block_id = value; }

        }
        private decimal _remainingqty;
        public decimal Remainingqty
        {
            get { return _remainingqty; }
            set { _remainingqty = value; }
        }

        private decimal _executedQty;
        public decimal ExecutedQty
        {
            get { return _executedQty; }
            set { _executedQty = value; }
        }

        private decimal _averagePriceExecuted;
        public decimal AveragePriceExecuted
        {
            get { return _averagePriceExecuted; }
            set { _averagePriceExecuted = value; }
        }

        private decimal _estimatedExecuted;
        public decimal EstimatedExecuted
        {
            get { return _estimatedExecuted; }
            set { _estimatedExecuted = value; }
        }

        private string _search;
        public string Search
        {
            get { return _search; }
            set { _search = value; }
        }

     

         public TraderVsPMVisual(TraderVsPMViewerModel securityHistoryViewerModel)
         {


             InitializeComponent();
             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
             ApplicationThemeHelper.ApplicationThemeName =_vm.Parameters.DefaultTheme;
            
   
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
                             string defaultShortName = "ALL";
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
                                     _vm.AccountName = "ALL";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.AccountId = -1;
                                 _vm.AccountName = "ALL";
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
                     _vm.AccountName = "ALL";
                     throw ex;
                 }



                 return retval;
             }
             else
             {
                 _vm.AccountId = -1;
                 _vm.AccountName = "ALL";
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
                   //  _vm.se_GetAcc(textEnteredPlus);
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

     

         private void comboBoxEdit1_SelectedIndexChanged(object sender, RoutedEventArgs e)
         {
             Validate(_vm.Parameters.AccountName);
         }

         private void comboBoxEdit1_LostFocus(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;

             _vm.Parameters.AccountName = comboBoxEdit1.Text;

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
                             var someObject = _vm.AllSecurities;
                             if (someObject == null)
                             {
                                 _vm.GetSecurities();
                                 return;
                             }

                             int defaultSecurityId = -1;
                             string defaultSymbol = "";
                             try
                             {
                                 if (_vm.AllSecurities.Tables.Count > 0)
                                 {
                                     for (int rowIndex = 0; rowIndex < _vm.AllSecurities.Tables[0].Rows.Count; ++rowIndex)
                                     {
                                         if (ourTextBox.ToUpper().CompareTo(((_vm.AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString().ToUpper()) == 0)
                                         {
                                             defaultSymbol = ((_vm.AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString();
                                             defaultSecurityId = Convert.ToInt32((_vm.AllSecurities.Tables[0].Rows[rowIndex])["security_id"]);
                                             break;
                                         }
                                     }
                                 }


                               
                                 if (defaultSecurityId != -1)
                                 {

                                     _vm.Security_id = defaultSecurityId;
                                     _vm.SecurityName = defaultSymbol;
                                     _vm.Parameters.SecurityName = _vm.SecurityName;

                                     // Get_ChartData();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _vm.Security_id = -1;
                                     _vm.SecurityName = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.Security_id = -1;
                                 _vm.SecurityName = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _vm.Security_id = -1;
                     _vm.SecurityName = "";
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _vm.Security_id = -1;
                 _vm.SecurityName = "";
             }

             return retval;
         }
         private void SecurityComboBoxEdit_EditValueChanged_1(object sender, EditValueChangedEventArgs e)
         {
             string textEnteredPlusNew = "symbol Like '" + SecurityTextBox.Text + "%'";
             this.SecurityTextBox.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllSecurities;
                     if (someObject == null)
                     {
                         _vm.GetSecurities();
                         return;
                     }
                     foreach (DataRow row in _vm.AllSecurities.Tables[0].Select(textEnteredPlusNew))
                     {

                         object item = row["symbol"];
                         object security_id = row["security_id"];

                         Dispatcher.BeginInvoke(new Action(() =>
                         {
                             SecurityTextBox.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(security_id)));



                         }), System.Windows.Threading.DispatcherPriority.Normal);

                         count = count + 1;
                         if (count == 100)
                         {
                             break;
                         }
                     }

                 });


             ValidateSecurity(_vm.Parameters.SecurityName);


         }
         private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateSecurity(comboBoxEdit.Text);
         }
       

         # endregion Security

         # region Issuer

         public bool ValidateIssuer(string ourTextBox)
         {
             bool retval = false;

             if (!String.IsNullOrEmpty(ourTextBox))
             {
                 try
                 {
                     ThreadPool.QueueUserWorkItem(
                         delegate(object eventArg)
                         {
                             var someObject = _vm.AllIssuers;
                             if (someObject == null)
                             {
                                 _vm.GetIssuers();
                                 return;
                             }

                             int defaultIssuerId = -1;
                             string defaultIssuer = "";
                             try
                             {
                                 if (_vm.AllIssuers.Tables.Count > 0)
                                 {
                                     for (int rowIndex = 0; rowIndex < _vm.AllIssuers.Tables[0].Rows.Count; ++rowIndex)
                                     {
                                         if (ourTextBox.ToUpper().CompareTo(((_vm.AllIssuers.Tables[0].Rows[rowIndex])["short_name"]).ToString().ToUpper()) == 0)
                                         {
                                             defaultIssuer = ((_vm.AllIssuers.Tables[0].Rows[rowIndex])["short_name"]).ToString();
                                             defaultIssuerId = Convert.ToInt32((_vm.AllIssuers.Tables[0].Rows[rowIndex])["issuer_id"]);
                                             break;
                                         }
                                     }
                                 }



                                 if (defaultIssuerId != -1)
                                 {

                                     _vm.Issuer_id = defaultIssuerId;
                                     _vm.IssuerName = defaultIssuer;
                                     _vm.Parameters.IssuerName = _vm.IssuerName;

                                     // Get_ChartData();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _vm.Issuer_id = -1;
                                     _vm.IssuerName = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.Issuer_id = -1;
                                 _vm.IssuerName = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _vm.Issuer_id = -1;
                     _vm.IssuerName = "";
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _vm.Issuer_id = -1;
                 _vm.IssuerName = "";
             }

             return retval;
         }
         private void IssuerComboBoxEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
         {
             string textEnteredPlusNew = "short_name Like '" + txtissuer.Text + "%'";
             this.txtissuer.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllIssuers;
                     if (someObject == null)
                     {
                         _vm.GetIssuers();
                         return;
                     }
                     foreach (DataRow row in _vm.AllIssuers.Tables[0].Select(textEnteredPlusNew))
                     {

                         object item = row["short_name"];
                         object issuer_id = row["issuer_id"];

                         Dispatcher.BeginInvoke(new Action(() =>
                         {
                             txtissuer.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(issuer_id)));



                         }), System.Windows.Threading.DispatcherPriority.Normal);

                         count = count + 1;
                         if (count == 100)
                         {
                             break;
                         }
                     }

                 });


             ValidateIssuer(_vm.Parameters.IssuerName);


         }
         private void IssuerComboBoxEdit_LostFocus(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateIssuer(comboBoxEdit.Text);
         }

        

         # endregion Issuer

         # region Broker
         //public bool ValidateBroker(string ourTextBox)
         //{
         //    bool retval = false;

         //    if (!String.IsNullOrEmpty(ourTextBox))
         //    {
         //        try
         //        {
         //            ThreadPool.QueueUserWorkItem(
         //                delegate(object eventArg)
         //                {
         //                    int defaultSecurityId = -1;
         //                    string defaultSymbol = "";
         //                    try
         //                    {
         //                        if (_vm.AllBrokers.Tables.Count > 0)
         //                        {
         //                            for (int rowIndex = 0; rowIndex < _vm.AllBrokers.Tables[0].Rows.Count; ++rowIndex)
         //                            {
         //                                if (ourTextBox.ToUpper().CompareTo(((_vm.AllBrokers.Tables[0].Rows[rowIndex])["mnemonic"]).ToString().ToUpper()) == 0)
         //                                {
         //                                    defaultSymbol = ((_vm.AllBrokers.Tables[0].Rows[rowIndex])["mnemonic"]).ToString();
         //                                    defaultSecurityId = Convert.ToInt32((_vm.AllBrokers.Tables[0].Rows[rowIndex])["broker_id"]);
         //                                    break;
         //                                }
         //                            }
         //                        }



         //                        if (defaultSecurityId != -1)
         //                        {

         //                            _vm.BrokerID = defaultSecurityId;
         //                            _vm.BrokerMnemonic = defaultSymbol;
         //                            _vm.Parameters.Broker = _vm.BrokerMnemonic;
                                   


         //                            retval = true;
         //                        }

         //                        else
         //                        {
         //                            _vm.BrokerID = -1;
         //                            _vm.BrokerMnemonic = "";
                                    

         //                        }
         //                    }
         //                    catch (Exception ex)
         //                    {
         //                        _vm.BrokerID = -1;
         //                        _vm.BrokerMnemonic = "";
                               
         //                        throw ex;
         //                    }

         //                    Dispatcher.BeginInvoke(new Action(() =>
         //                    {

         //                    }), System.Windows.Threading.DispatcherPriority.Normal);

         //                });
         //        }

         //        catch (Exception ex)
         //        {
         //            _vm.BrokerID = -1;
         //            _vm.BrokerMnemonic = "";
                   
         //            throw ex;
         //        }

         //        return retval;
         //    }

         //    else
         //    {
         //        _vm.BrokerID = -1;
         //        _vm.BrokerMnemonic = "All";
                 
         //    }

         //    return retval;
         //}
         //private void BrokerComboBoxEdit_EditValueChanged_1(object sender, EditValueChangedEventArgs e)
         //{
         //    string textEnteredPlusNew = "mnemonic Like '" + BrokerComboBoxEdit.Text + "%'";
         //    this.BrokerComboBoxEdit.Items.Clear();
         //    int count = -1;
         //    ThreadPool.QueueUserWorkItem(
         //        delegate(object eventArg)
         //        {
         //            var someObject = _vm.AllBrokers;
         //            if (someObject == null)
         //            {
         //                _vm.GetBrokers();
         //                return;
         //            }
         //            foreach (DataRow row in _vm.AllBrokers.Tables[0].Select(textEnteredPlusNew))
         //            {

         //                object item = row["mnemonic"];
         //                object broker_id = row["broker_id"];

         //                Dispatcher.BeginInvoke(new Action(() =>
         //                {
         //                    BrokerComboBoxEdit.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(broker_id)));



         //                }), System.Windows.Threading.DispatcherPriority.Normal);

         //                count = count + 1;
         //                if (count == 100)
         //                {
         //                    break;
         //                }
         //            }

         //        });


         //    ValidateBroker(_vm.Parameters.Broker);


         //}
         //private void BrokerComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         //{
         //    ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


         //    ValidateBroker(comboBoxEdit.Text);
         //}
         # endregion Broker


        # region HelperProcedures

        private void Get_From_info()
        {
            
           
            _vm.EndDate = _vm.Parameters.EndDate;
            _vm.StartDate = _vm.Parameters.StartDate;

            getDates();
            Validate(_vm.Parameters.AccountName);
            ValidateSecurity(_vm.Parameters.SecurityName);
            //ValidateIssuer(_vm.Parameters.IssuerName);
        }



        void getDates()
        {


            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(_vm.StartDate.ToString(), out intStr);
            if (intResultTryParse == true)
            {
                _vm.StartDate = (intStr);

            }
            else
            {
                _vm.StartDate = DateTime.Today;
            }

            DateTime intStr1;
            bool intResultTryParse1 = DateTime.TryParse(_vm.EndDate.ToString(), out intStr1);
            if (intResultTryParse1 == true)
            {
                _vm.EndDate = intStr1;
            }
            else
            {
                _vm.EndDate = DateTime.Today;
            }
        }
        private void AssignXML()
        {
            if (File.Exists((subPath + "\\" + file)))
            {
                dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);
            }

            if (File.Exists((subPath + "\\" + file1)))
            {
                journalGrid.RestoreLayoutFromXml(subPath + "\\" + file1);
            }

        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                journalGrid.SaveLayoutToXml(subPath + "\\" + file1);
            }
            else
            {
                dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                journalGrid.SaveLayoutToXml(subPath + "\\" + file1);
            }
        }

    
        # endregion HelperProcedures



        # region Methods

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    ApplicationThemeHelper.ApplicationThemeName = Theme.DefaultThemeName;
        //}

        //private void cbTheme_EditValueChanged(object send, EditValueChangedEventArgs e)
        //{
        //    ApplicationThemeHelper.ApplicationThemeName = (this.cbTheme.SelectedItem as Theme).Name;
        //}
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DeskList();
            Get_From_info();
            load_asset_type();
            ApplicationThemeHelper.ApplicationThemeName = _vm.Parameters.DefaultTheme;
            
        }

        private void button_Update(object sender, RoutedEventArgs e)
        {

            _vm.StartDate = _vm.Parameters.StartDate;
            _vm.EndDate = _vm.Parameters.EndDate;
            _vm.Get_History_date();

        }

        private void Button_Security(object sender, RoutedEventArgs e)
        {
            int intRowCount = 0;

            var task = new System.Threading.Tasks.Task(() =>
            {
                try
                {
                    int asset_type_code = 0;




                    asset_type_code = _vm.IntAssetCode;//Convert.ToInt32(cboAssetType.SelectedValue);

                    DataSet ds = new DataSet();
                
                    ds = _vm.DBService.GetListSecurity(asset_type_code);


                    if (ds.Tables.Count > 0)
                    {
                        intRowCount = ds.Tables[0].Rows.Count;
                    }

                    switch (intRowCount)
                    {
                        case 1:
                            _vm.Security_id = Convert.ToInt32((ds.Tables[0].Rows[0])["security_id"]);
                            _vm.SecurityName = (string)(ds.Tables[0].Rows[0])["mnemonic"];
                            //m_issuershortname = (string)(ds.Tables[0].Rows[0])["short_name"];

                           _vm.IssuerName = String.IsNullOrEmpty((string)(ds.Tables[0].Rows[0])["short_name"]) ?
                      String.Empty :
                      (string)(ds.Tables[0].Rows[0])["short_name"];


                           this.SecurityTextBox.Text = _vm.SecurityName;
                            break;
                        default:

                            DataView dv = ds.Tables[0].DefaultView;

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                getSecurityTextBoxInfo();
                            }));


                            if (getsecurityLenthinfo > 0)
                            {
                                string strExpr = "mnemonic like '" + getsecurityTextinfo + "%'";
                                dv.RowFilter = strExpr;
                            }
                            string strSort = "mnemonic ASC";
                            dv.Sort = strSort;

                            DataSet newDS = new DataSet();
                            DataTable newDT = dv.ToTable();
                            newDS.Tables.Add(newDT);

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                SecurityLookup securityLookup = new SecurityLookup(newDS);
                                //securityLookup.Owner = this;

                                if (securityLookup.ShowDialog() == true)
                                {


                                    SetSecurityTextBoxInfo(securityLookup);
                                }
                                else
                                {
                                    //this.SecurityTextBox.SelectAll();
                                }

                            }));





                            break;


                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                    //System.Windows.MessageBox.Show(this, "An error has occurred." + System.Environment.NewLine + "SecurityClick" + System.Environment.NewLine + ex.Message, MSGBOX_TITLE_ERROR, MessageBoxButton.OK);
                }

            });
            task.Start();
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

        private async void LoadAccountsAndDesk()
        {
           

            Process process = new Process();
          
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
        
            await Task.Run(() => _vm.se_GetAcc(_vm.AccountName));
            await Task.Run(() => _vm.GetSecurities());
            await Task.Run(() => _vm.GetIssuers());
            await Task.Run(() => _vm.GetBrokers());
         
        }

        private void SetSecurityTextBoxInfo(SecurityLookup securityLookup)
        {

            _vm.Security_id = securityLookup.SecurityId;
            _vm.SecurityName = securityLookup.ShortName;
            _vm.IssuerName = securityLookup.IssuerShortName;

            _vm.IssuerName = String.IsNullOrEmpty(securityLookup.IssuerShortName) ?
            String.Empty :
            securityLookup.IssuerShortName;

          

            this.SecurityTextBox.Text = _vm.SecurityName;

            this.txtissuer.Text = _vm.IssuerName;
        }


        private void load_asset_type()
        {

            cboAssetType.Items.Clear();

            cboAssetType.Items.Add("All");
            cboAssetType.Items.Add("Equity");
            cboAssetType.Items.Add("Debt");
            cboAssetType.Items.Add("Future");
            cboAssetType.Items.Add("Option");
            cboAssetType.Items.Add("Fund");
            cboAssetType.SelectedIndex = 0;

            get_asset_type_id();
        }


        private void get_asset_type_id()
        {

            if (cboAssetType.SelectedIndex == 0)
            {
                _vm.IntAssetCode = -1;
            }
            if (cboAssetType.SelectedIndex == 1)
            {
                _vm.IntAssetCode = 1;
            }
            else if (cboAssetType.SelectedIndex == 2)
            {
                _vm.IntAssetCode = 3;
            }
            else if (cboAssetType.SelectedIndex == 3)
            {
                _vm.IntAssetCode = 5;
            }
            else if (cboAssetType.SelectedIndex == 4)
            {
                _vm.IntAssetCode = 7;
            }
            else if (cboAssetType.SelectedIndex == 5)
            {
                _vm.IntAssetCode = 4;
            }
            else
            {
                _vm.IntAssetCode = -1;
            }

        }


        private void cboAssetType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            get_asset_type_id();
            _vm.StrAssetCode = (null == cboAssetType.Text) ? "" : cboAssetType.Text;
        }
        private void getSecurityTextBoxInfo()
        {

            
            getsecurityTextinfo = this.SecurityTextBox.Text.ToString();
            getsecurityLenthinfo = this.SecurityTextBox.Text.Length;
        }

        private void cboAssetType_SelectionChanged_1(object sender, EditValueChangedEventArgs e)
        {
            get_asset_type_id();
        }

        private void btnIssuer_Click(object sender, RoutedEventArgs e)
        {
            int intRowCount = 0;

            var task = new System.Threading.Tasks.Task(() =>
            {
                try
                {


                    DataSet ds = _vm.DBService.GetIssuerInfo();

                    if (ds.Tables.Count > 0)
                    {
                        intRowCount = ds.Tables[0].Rows.Count;
                    }

                    switch (intRowCount)
                    {
                        case 1:
                            _vm.Issuer_id = Convert.ToInt32((ds.Tables[0].Rows[0])["issuer_id"]);

                            _vm.IssuerName = (string)(ds.Tables[0].Rows[0])["short_name"];
                            this.txtissuer.Text = _vm.IssuerName;
                            break;
                        default:

                            DataView dv = ds.Tables[0].DefaultView;

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                getIssuerTextBoxInfo();
                            }));



                            if (getissuerLenthinfo > 0)
                            {
                                string strExpr = "short_name like '" + getissuerTextinfo + "%'";
                                dv.RowFilter = strExpr;
                            }
                            string strSort = "short_name ASC";
                            dv.Sort = strSort;

                            DataSet newDS = new DataSet();
                            DataTable newDT = dv.ToTable();
                            newDS.Tables.Add(newDT);






                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                IssuerLookup IssuerLookup = new IssuerLookup(newDS);

                                //IssuerLookup.Owner = this;

                                if (IssuerLookup.ShowDialog() == true)
                                {


                                    SetissuerTextBoxInfo(IssuerLookup);           // get full name and current price
                                }
                                else
                                {
                                    
                                }


                            }));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                                  }

            });
            task.Start();
        }

        private void SetissuerTextBoxInfo(IssuerLookup IssuerLookup)
        {



            _vm.IssuerName = String.IsNullOrEmpty(IssuerLookup.IssuerShortName) ?
            String.Empty :
            IssuerLookup.IssuerShortName;


            this.txtissuer.Text = _vm.IssuerName;
        }

        private void getIssuerTextBoxInfo()
        {

          
            getissuerTextinfo = this.txtissuer.Text.ToString();
            getissuerLenthinfo = this.txtissuer.Text.Length;
        }

        private void txtissuer_TextChanged(object sender, TextChangedEventArgs e)
        {
            getIssuerTextBoxInfo();
        }


        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _vm.AccountName;


        }

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            StyleManager Style1 = new StyleManager(dataGrid);

            Style1.Show();
        }
        #endregion Methods


        private void add_commitment(object sender, RoutedEventArgs e)
        {

            //DataRow rowData;

            int i = 0;
            int[] listRowList = this.dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {


                GridColumn colBlock_id = dataGrid.Columns["block_id"];


                if (colBlock_id != null)
                {
                    if (dataGrid.GetCellValue(listRowList[i], colBlock_id).ToString() != null)
                    {

                        BlockID = Convert.ToInt32(dataGrid.GetCellValue(listRowList[i], colBlock_id).ToString());



                    }
                }

            }


            Commitment AddNew = new Commitment(this);
            AddNew.ShowDialog();





        }


        public void UpdateJournal(string note)
        {

            try
            {

                ApplicationMessageList messages = null;
                
                _vm.DBService.se_update_journaling(
                   Convert.ToInt32(_block_id),
                   note,
                   out messages);


            }
            catch (Exception ex)
            {

                throw ex;
            }

            Get_Packages();
        }


        public void UpdateCommitmentPrice(Int32 block_id, decimal price)
        {

            try
            {

                ApplicationMessageList messages = null;

                _vm.DBService.se_update_committment_price(
                   Convert.ToInt32(_block_id),
                   price,
                   out messages);


            }
            catch (Exception ex)
            {

                throw ex;
            }


            GetSecurityHistory();


        }


        public void Get_Packages()
        {
            DataSet ds = new DataSet();
            try
            {

                ApplicationMessageList messages = null;
               
                ds = _vm.DBService.se_get_journaling(
                    Convert.ToInt32(_block_id),
                    out messages);
                this.journalGrid.DataContext = ds;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private DataSet GetSecurityHistory()
        {
            DataSet ds = new DataSet();


            try
            {

                Get_History_date();



            }
            catch (Exception ex)
            {

                throw ex;
            }


            return ds;
        }

        private void Get_History_date()
        {


            DataSet ds = new DataSet();



            try
            {
                this.dataGrid.ItemsSource = null;

                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {


                        ApplicationMessageList messages = null;
                       
                        ds = _vm.DBService.se_get_tradervspm_orders(

                            _vm.DeskId,
                            _vm.AccountName,
                            _vm.Symbol,
                            _vm.IssuerName,
                            Search,
                            Convert.ToDateTime(_vm.StartDate),
                            Convert.ToDateTime(_vm.EndDate),
                            _vm.Active,
                            _vm.IntAssetCode,
                            _vm.Crossable,
                            out messages);

                        if (ds.Tables.Count > 0)
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.dataGrid.ItemsSource = ds.Tables[0];
                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                        else
                            Dispatcher.BeginInvoke(new Action(() => { this.dataGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);


                    });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {


                    DataSet ds = _vm.DBService.GetListAllDesks();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["desk_name"];
                            object desk_id = row["desk_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new TraderVsPM.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

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
        private void cmboDesk_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmboDesk.Items.Count == 0)
            {
                DeskList();
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboDesk.SelectedItem).ToString()))
                {
                    Desk = (cmboDesk.SelectedItem).ToString();
                    _vm.DeskId = Convert.ToInt32(((TraderVsPM.Client.ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);
                    _vm.Parameters.DeskName = Desk;
                    //_vm.Get_DeskData(DeskId);

                }
            }
        }

        private void chkCrossable_Checked(object sender, RoutedEventArgs e)
        {
            _vm.Crossable = true;
        }

        private void chkCrossable_Unchecked(object sender, RoutedEventArgs e)
        {
            _vm.Crossable = false;
        }
        private void SetActive()
        {
            if (this.chkActive.IsChecked == true)
            {
                txtstartdate.IsEnabled = false;
                txtenddate.IsEnabled = false;
                _vm.Active = true;
                chkCrossable.IsEnabled = true;

            }
            else
            {
                txtstartdate.IsEnabled = true;
                txtenddate.IsEnabled = true;
                _vm.Active = false;
                chkCrossable.IsEnabled = false;

            }
        }
        private void chkActive_Unchecked(object sender, RoutedEventArgs e)
        {
            SetActive();
        }

        private void chkActive_Checked(object sender, RoutedEventArgs e)
        {
            SetActive();
        }
        private void dataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            try
            {
                string symbol;



                int index = Convert.ToInt32(dataGrid.GetRowVisibleIndexByHandle(viewPMVsTrader.FocusedRowData.RowHandle.Value).ToString());



                if (index > -1)
                {

                    if (index > -1)
                    {

                        GridColumn colblock_id = dataGrid.Columns["block_id"];
                        GridColumn colsymbol = dataGrid.Columns["symbol"];
                        GridColumn colCommittedPrice = dataGrid.Columns["committed_price"];
                        GridColumn colRemaining = dataGrid.Columns["remaining"];
                        GridColumn colExecutedqty = dataGrid.Columns["quantity_executed"];
                        GridColumn colAvgPriceExecuted = dataGrid.Columns["average_price_executed"];

                        if (colblock_id != null)
                        {
                            if ((dataGrid.GetCellValue(index, colblock_id).ToString() != null))
                            {
                                BlockID = Convert.ToInt32(dataGrid.GetCellValue(index, colblock_id).ToString());
                                symbol = dataGrid.GetCellValue(index, colsymbol).ToString();
                                EstimatedExecuted = Convert.ToDecimal(dataGrid.GetCellValue(index, colCommittedPrice).ToString());
                                Remainingqty = Convert.ToDecimal(dataGrid.GetCellValue(index, colRemaining).ToString());
                                ExecutedQty = Convert.ToDecimal(dataGrid.GetCellValue(index, colExecutedqty).ToString());
                                AveragePriceExecuted = Convert.ToDecimal(dataGrid.GetCellValue(index, colAvgPriceExecuted).ToString());

                                lblJournal.Content = string.Format("Journaling for Security: {0} : Block Id = {1}", symbol, BlockID);

                                lblIfIBuy.Content = string.Format("   If I buy  {0}", symbol);
                                lblataprice.Content = string.Format("   At a Price of");
                                this.Spe_Remaining.Text = Convert.ToString(Remainingqty);
                                this.Spe_price.Text = Convert.ToString(EstimatedExecuted);

                                Update_Forcaster(Remainingqty, EstimatedExecuted, ExecutedQty, AveragePriceExecuted);
                                Get_Packages();


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

        void GridControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && viewTJournal.ActiveEditor != null && viewTJournal.FocusedRowHandle == GridControl.NewItemRowHandle)
            {
                viewTJournal.CommitEditing();
            }
        }

        private void Button_Click_Add_new_row(object sender, RoutedEventArgs e)
        {
            AddNewRow AddNew = new AddNewRow(this);

            AddNew.ShowDialog();
        }


        private void Update_Forcaster(decimal qtyRemianing, decimal committedPrice, decimal qtyexecuted, decimal average_price_executed)
        {
            this.Spe_Remaining.Text = Convert.ToString(qtyRemianing);
            this.Spe_price.Text = Convert.ToString(committedPrice);

            decimal estimated_execution_price = Convert.ToDecimal(Spe_price.Text);
            decimal Total_qty = qtyRemianing + qtyexecuted;

            if (Total_qty != 0)
            {
                decimal estimated_average_price = (qtyRemianing / Total_qty * estimated_execution_price) + (qtyexecuted / Total_qty * average_price_executed);

                lblavgwillbe.Content = string.Format("   Average will be  {0}", Math.Round(estimated_average_price, 4));


            }


        }

        private void Spe_Remaining_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Remainingqty = Convert.ToDecimal(Spe_Remaining.Text);
            Update_Forcaster(Remainingqty, EstimatedExecuted, ExecutedQty, AveragePriceExecuted);
        }

        private void Spe_price_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            EstimatedExecuted = Convert.ToDecimal(Spe_price.Text);
            Update_Forcaster(Remainingqty, EstimatedExecuted, ExecutedQty, AveragePriceExecuted);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Update_Forcaster(Remainingqty, EstimatedExecuted, ExecutedQty, AveragePriceExecuted);
        }
        
    }

}
