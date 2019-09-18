namespace SecurityHistory.Client.View
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
    using SecurityHistory.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using SecurityHistory.Client;
    using DevExpress.Xpf.Utils.Themes;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Core;
   
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
    using System.Threading.Tasks;
    using System.Diagnostics;


    public partial class SecurityHistoryVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "SecurityHistory.xaml";
    
   
        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;

        private string getissuerTextinfo = "";
        private int getissuerLenthinfo = 0;

        public SecurityHistoryViewerModel _vm;
    
      

         public SecurityHistoryVisual(SecurityHistoryViewerModel securityHistoryViewerModel)
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
                                 if (_vm.AllBrokers.Tables.Count > 0)
                                 {
                                     for (int rowIndex = 0; rowIndex < _vm.AllBrokers.Tables[0].Rows.Count; ++rowIndex)
                                     {
                                         if (ourTextBox.ToUpper().CompareTo(((_vm.AllBrokers.Tables[0].Rows[rowIndex])["mnemonic"]).ToString().ToUpper()) == 0)
                                         {
                                             defaultSymbol = ((_vm.AllBrokers.Tables[0].Rows[rowIndex])["mnemonic"]).ToString();
                                             defaultSecurityId = Convert.ToInt32((_vm.AllBrokers.Tables[0].Rows[rowIndex])["broker_id"]);
                                             break;
                                         }
                                     }
                                 }



                                 if (defaultSecurityId != -1)
                                 {

                                     _vm.BrokerID = defaultSecurityId;
                                     _vm.BrokerMnemonic = defaultSymbol;
                                     _vm.Parameters.Broker = _vm.BrokerMnemonic;
                                   


                                     retval = true;
                                 }

                                 else
                                 {
                                     _vm.BrokerID = -1;
                                     _vm.BrokerMnemonic = "";
                                    

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.BrokerID = -1;
                                 _vm.BrokerMnemonic = "";
                               
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _vm.BrokerID = -1;
                     _vm.BrokerMnemonic = "";
                   
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _vm.BrokerID = -1;
                 _vm.BrokerMnemonic = "All";
                 
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
                     var someObject = _vm.AllBrokers;
                     if (someObject == null)
                     {
                         _vm.GetBrokers();
                         return;
                     }
                     foreach (DataRow row in _vm.AllBrokers.Tables[0].Select(textEnteredPlusNew))
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


             ValidateBroker(_vm.Parameters.Broker);


         }
         private void BrokerComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateBroker(comboBoxEdit.Text);
         }
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

            

        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                dataGrid.SaveLayoutToXml(subPath + "\\" + file);

            }
            else
            {
                dataGrid.SaveLayoutToXml(subPath + "\\" + file);
            
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

     

     
       

     
       

        

    }

}
