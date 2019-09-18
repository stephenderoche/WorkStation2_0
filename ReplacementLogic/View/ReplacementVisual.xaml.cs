namespace Replacement.Client.View
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
   using Replacement.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using Replacement.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
  


    public partial class ReplacementVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "ReplacementGrid.xaml";
         # region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public ReplacementViewerModel _vm;

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



    

        private int _desId = -1;
        public int DeskId
        {
            get { return _desId; }
            set { _desId = value; }
        }

        private string _desk = string.Empty;
        public string Desk
        {
            get { return _desk; }
            set { _desk = value; }
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

         public ReplacementVisual(ReplacementViewerModel securityHistoryViewerModel)
         {

             InitializeComponent();
             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
             this.comboBoxEdit1.Text = _vm.AccountName;
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
                                 _vm.DBService.ValidateAccountForUser(ourTextBox, out defaultAccountId, out defaultShortName);
                                 if (defaultAccountId != -1)
                                 {

                                     _vm.AccountId = defaultAccountId;
                                     _vm.AccountName = defaultShortName;
                                     _vm.Parameters.AccountName = _vm.AccountName;


                                     _vm.se_get_account_replacement(Convert.ToInt32(_vm.AccountId));

                                     // this.TXTHarvestamount.Text = "0";

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

                             this.Dispatcher.BeginInvoke(new Action(() =>
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
             string textEnteredPlusNew = "short_name Like '" + comboBoxEdit1.Text + "%'";
             this.comboBoxEdit1.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllAccounts;
                     if (someObject == null)
                     {
                         _vm.GetAcc();
                         return;
                     }
                     foreach (DataRow row in _vm.AllAccounts.Tables[0].Select(textEnteredPlusNew))
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

             Validate(_vm.Parameters.AccountName);


         }

         public void get_account_name(string account_id)
         {
             string textEnteredPlusNew = "account_id = " + account_id;

             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllAccounts;
                     if (someObject == null)
                     {
                         _vm.GetAcc();
                         return;
                     }
                     foreach (DataRow row in _vm.AllAccounts.Tables[0].Select(textEnteredPlusNew))
                     {
                         object item = row["short_name"];
                         _vm.AccountName = Convert.ToString(item);
                         _vm.Parameters.AccountName = _vm.AccountName;

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

         # region Security
        //buy list
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

                                     _vm.SecurityId = defaultSecurityId;
                                     _vm.BuyListID = _vm.SecurityId;
                                     _vm.Symbol = defaultSymbol;
                                   

                                     // Get_ChartData();
                                     retval = true;
                                 }
                                 if (defaultSecurityId != -1)
                                 {

                                     _vm.SecurityId = defaultSecurityId;
                                     _vm.BuyListID = _vm.SecurityId;
                                     _vm.Symbol = defaultSymbol;
                                  

                                     // Get_ChartData();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _vm.SecurityId = -1;
                                     _vm.Symbol = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.SecurityId = -1;
                                 _vm.Symbol = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _vm.SecurityId = -1;
                     _vm.Symbol = "";
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _vm.SecurityId = -1;
                 _vm.Symbol = "";
             }

             return retval;
         }
         private void SecurityComboBoxEdit_EditValueChanged_1(object sender, EditValueChangedEventArgs e)
         {
             string textEnteredPlusNew = "symbol Like '" + SecurityComboBoxEdit.Text + "%'";
             this.SecurityComboBoxEdit.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllSecurities;
                     if (someObject == null)
                     {
                        // GetSecurities();
                         return;
                     }
                     foreach (DataRow row in _vm.AllSecurities.Tables[0].Select(textEnteredPlusNew))
                     {

                         object item = row["symbol"];
                         object security_id = row["security_id"];

                         Dispatcher.BeginInvoke(new Action(() =>
                         {
                             SecurityComboBoxEdit.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(security_id)));



                         }), System.Windows.Threading.DispatcherPriority.Normal);

                         count = count + 1;
                         if (count == 100)
                         {
                             break;
                         }
                     }

                 });


             ValidateSecurity(_vm.Symbol);


         }
         private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateSecurity(comboBoxEdit.Text);
         }

        //replacement list
         public bool ValidateSecurityReplacement(string ourTextBox)
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

                                   

                                     _vm.Replacement_security_id = defaultSecurityId;
                                     _vm.ReplacementListid = _vm.Replacement_security_id;
                                     _vm.Replacementsymbol = defaultSymbol;


                                     // Get_ChartData();
                                     retval = true;
                                 }
                                 if (defaultSecurityId != -1)
                                 {

                                     _vm.Replacement_security_id = defaultSecurityId;
                                     _vm.ReplacementListid = _vm.Replacement_security_id;
                                     _vm.Replacementsymbol = defaultSymbol;


                                     // Get_ChartData();
                                     retval = true;
                                 }

                                 else
                                 {
                                     _vm.Replacement_security_id = -1;
                                     _vm.Replacementsymbol = "";

                                 }
                             }
                             catch (Exception ex)
                             {
                                 _vm.Replacement_security_id = -1;
                                 _vm.Replacementsymbol = "";
                                 throw ex;
                             }

                             Dispatcher.BeginInvoke(new Action(() =>
                             {

                             }), System.Windows.Threading.DispatcherPriority.Normal);

                         });
                 }

                 catch (Exception ex)
                 {
                     _vm.Replacement_security_id = -1;
                     _vm.Replacementsymbol = "";
                     throw ex;
                 }

                 return retval;
             }

             else
             {
                 _vm.Replacement_security_id = -1;
                 _vm.Replacementsymbol = "";
             }

             return retval;
         }
         private void SecurityComboBoxEdit_EditValueChanged_Replacement(object sender, EditValueChangedEventArgs e)
         {
             string textEnteredPlusNew = "symbol Like '" + SecurityComboBoxEditReplacment.Text + "%'";
             this.SecurityComboBoxEditReplacment.Items.Clear();
             int count = -1;
             ThreadPool.QueueUserWorkItem(
                 delegate(object eventArg)
                 {
                     var someObject = _vm.AllSecurities;
                     if (someObject == null)
                     {
                         // GetSecurities();
                         return;
                     }
                     foreach (DataRow row in _vm.AllSecurities.Tables[0].Select(textEnteredPlusNew))
                     {

                         object item = row["symbol"];
                         object security_id = row["security_id"];

                         Dispatcher.BeginInvoke(new Action(() =>
                         {
                             SecurityComboBoxEditReplacment.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(security_id)));



                         }), System.Windows.Threading.DispatcherPriority.Normal);

                         count = count + 1;
                         if (count == 100)
                         {
                             break;
                         }
                     }

                 });


             ValidateSecurityReplacement(_vm.Replacementsymbol);


         }
         private void SecurityComboBoxEdit_LostFocus_Replacement(object sender, RoutedEventArgs e)
         {
             ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


             ValidateSecurityReplacement(comboBoxEdit.Text);
         }


         # endregion Security

         # region Hierarchy
         public void HierarchySectorList()
         {

             // this.cmboHierarchy.SelectedItem = -1;
             this.HierachyComboBoxEditBuylist.Items.Clear();
             this.HierachyComboBoxEditReplacement.Items.Clear();
          
             //ThreadPool.QueueUserWorkItem(
             //    delegate(object eventArg)
             //    {


                     _vm.GetHierarchy();
                    



                     foreach (DataTable table in _vm.Hierarchy.Tables)
                     {
                         foreach (DataRow row in table.Rows)
                         {

                             object item = row["description"];
                             object hierarchy_sector_id = row["hierarchy_sector_id"];

                             //Dispatcher.BeginInvoke(new Action(() =>
                             //{
                                 HierachyComboBoxEditBuylist.Items.Add(new Replacement.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(hierarchy_sector_id)));
                                 HierachyComboBoxEditReplacement.Items.Add(new Replacement.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(hierarchy_sector_id)));
                              



                           //  }), System.Windows.Threading.DispatcherPriority.Normal);

                         }
                     }




               //  });
         }
         public void CountryList()
         {

             // this.cmboHierarchy.SelectedItem = -1;
             this.CountryComboBoxEditBuylist.Items.Clear();
             this.CountryComboBoxEditReplacement.Items.Clear();

             //ThreadPool.QueueUserWorkItem(
             //    delegate(object eventArg)
             //    {


             _vm.GetCountry();




             foreach (DataTable table in _vm.Country.Tables)
             {
                 foreach (DataRow row in table.Rows)
                 {

                     object item = row["mnemonic"];
                     object country_code = row["country_code"];

                     //Dispatcher.BeginInvoke(new Action(() =>
                     //{
                     CountryComboBoxEditBuylist.Items.Add(new Replacement.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(country_code)));
                     CountryComboBoxEditReplacement.Items.Add(new Replacement.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(country_code)));




                     //  }), System.Windows.Threading.DispatcherPriority.Normal);

                 }
             }




             //  });
         }
         # endregion Hierarchy

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

           // Symbol = String.IsNullOrEmpty(_vm.Parameters.SecurityName) ? String.Empty : _vm.Parameters.SecurityName;

        }

      

        private void AssignXML()
        {
           

            if (File.Exists((subPath + "\\" + file)))
            {
                _gridReplacement.RestoreLayoutFromXml(subPath + "\\" + file);
            }


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

              
                _gridReplacement.SaveLayoutToXml(subPath + "\\" + file);

            }
            else
            {
                
                _gridReplacement.SaveLayoutToXml(subPath + "\\" + file);
            }
        }

        # endregion HelperProcedures

       # region Methods

        private void _gridReplacement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            int index =
                Convert.ToInt32(_gridReplacement
                    .GetRowVisibleIndexByHandle(_viewGridReplacement.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn _buylisttypeid = _gridReplacement.Columns["buy_list_type_id"];
                GridColumn _buy_list_name = _gridReplacement.Columns["buy_list_name"];
                GridColumn _buy_list_id = _gridReplacement.Columns["buy_list_id"];
                GridColumn _replacement_list_type_id = _gridReplacement.Columns["replacement_list_type_id"];
                GridColumn _replacement_list_id = _gridReplacement.Columns["replacement_list_id"];
                GridColumn _replacement_name = _gridReplacement.Columns["replacement_name"];
                GridColumn _replacement_id = _gridReplacement.Columns["replacement_id"];




                if (_buylisttypeid != null)
                {


                    _vm.BuyListTypeID = Convert.ToInt32(_gridReplacement.GetCellValue(index, _buylisttypeid).ToString());
                    _vm.BuyListname = _gridReplacement.GetCellValue(index, _buy_list_name).ToString();
                    _vm.BuyListID = Convert.ToInt32(_gridReplacement.GetCellValue(index, _buy_list_id).ToString());
                    _vm.ReplacementTypeID = Convert.ToInt32(_gridReplacement.GetCellValue(index, _replacement_list_type_id).ToString());
                    _vm.Replacementname = _gridReplacement.GetCellValue(index, _replacement_name).ToString();
                    _vm.ReplacementListid = Convert.ToInt32(_gridReplacement.GetCellValue(index, _replacement_list_id).ToString());
                    _vm.Replacementid = Convert.ToInt32(_gridReplacement.GetCellValue(index, _replacement_id).ToString());


                    if (!string.IsNullOrEmpty(_vm.BuyListname))
                    {

                        if (_vm.BuyListTypeID == 0)
                        {
                            cmboBuyListType.SelectedIndex = 0;
                            SecurityComboBoxEdit.Text = _vm.BuyListname;
                        }

                        if (_vm.ReplacementTypeID == 0)
                        {
                            cmboReplacementType.SelectedIndex = 0;
                            SecurityComboBoxEditReplacment.Text = _vm.Replacementname;
                        }

                        if (_vm.BuyListTypeID == 1)
                        {
                            cmboBuyListType.SelectedIndex = 1;
                            HierachyComboBoxEditBuylist.Text = _vm.BuyListname;
                            
                        }

                        if (_vm.ReplacementTypeID == 1)
                        {
                            cmboReplacementType.SelectedIndex = 1;
                            HierachyComboBoxEditReplacement.Text = _vm.Replacementname;
                        }

                        if (_vm.BuyListTypeID == 2)
                        {
                            cmboBuyListType.SelectedIndex = 2;
                            CountryComboBoxEditBuylist.Text = _vm.BuyListname;

                        }

                        if (_vm.ReplacementTypeID == 2)
                        {
                            cmboReplacementType.SelectedIndex = 2;
                            CountryComboBoxEditReplacement.Text = _vm.Replacementname;
                        }

                    }


                }
            }


        }
        private void Delete_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            int replacementid;

            int i = 0;
            int[] listRowList = this._gridReplacement.GetSelectedRowHandles();

            for (i = 0; i < listRowList.Length; i++)
            {

                GridColumn colreplacementid = _gridReplacement.Columns["replacement_id"];

                if (colreplacementid != null)
                {

                    replacementid = Convert.ToInt32(_gridReplacement.GetCellValue(listRowList[i], colreplacementid).ToString());
                    _vm.Replacementid = replacementid;


                    _vm.se_delete_replacements(_vm.Replacementid);
                    count = count + 1;

                }

            }

            Thread.Sleep(1000);
            string LABEL = string.Format("You have Deleted {0} replacements.", count);
            System.Windows.MessageBox.Show(LABEL, "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

            _vm.se_get_account_replacement(_vm.AccountId);
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _vm.se_get_account_replacement(_vm.AccountId);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Get_From_info();
            _vm.se_get_account_replacement(_vm.AccountId);
            HierarchySectorList();
            CountryList();
            
        }

        void column_CustomGetSerializableProperties(object sender, CustomGetSerializablePropertiesEventArgs e)
        {
            e.SetPropertySerializable(GridColumn.EditSettingsProperty, new DXSerializable());

        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _vm.AccountName;


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
        #endregion Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _vm.se_update_replacements();
            Thread.Sleep(1000);
            _vm.se_get_account_replacement(_vm.AccountId);
        }

        private void cmboBuyListType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(cmboBuyListType.SelectedIndex == 0)
            {
                _vm.BuyListTypeID = 0;
                this.SecurityComboBoxEdit.Visibility = Visibility.Visible;
                this.HierachyComboBoxEditBuylist.Visibility = Visibility.Hidden;
                this.CountryComboBoxEditBuylist.Visibility = Visibility.Hidden;
            }

            if (cmboBuyListType.SelectedIndex == 1)
            {
                _vm.BuyListTypeID = 1;
                this.SecurityComboBoxEdit.Visibility = Visibility.Hidden;
                this.HierachyComboBoxEditBuylist.Visibility = Visibility.Visible;
                this.CountryComboBoxEditBuylist.Visibility = Visibility.Hidden;
            }

            if (cmboBuyListType.SelectedIndex == 2)
            {
                _vm.BuyListTypeID = 2;
                this.SecurityComboBoxEdit.Visibility = Visibility.Hidden;
                this.HierachyComboBoxEditBuylist.Visibility = Visibility.Hidden;
                this.CountryComboBoxEditBuylist.Visibility = Visibility.Visible;
            }


        }

        private void cmboReplacementType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmboReplacementType.SelectedIndex == 0)
            {
                _vm.ReplacementTypeID = 0;
                this.SecurityComboBoxEditReplacment.Visibility = Visibility.Visible;
                this.HierachyComboBoxEditReplacement.Visibility = Visibility.Hidden;
                this.CountryComboBoxEditReplacement.Visibility = Visibility.Hidden;
            }

            if (cmboReplacementType.SelectedIndex == 1)
            {
                _vm.ReplacementTypeID = 1;
                this.SecurityComboBoxEditReplacment.Visibility = Visibility.Hidden;
                this.HierachyComboBoxEditReplacement.Visibility = Visibility.Visible;
                this.CountryComboBoxEditReplacement.Visibility = Visibility.Hidden;
            }

            if (cmboReplacementType.SelectedIndex == 2)
            {
                _vm.ReplacementTypeID = 2;
                this.SecurityComboBoxEditReplacment.Visibility = Visibility.Hidden;
                this.HierachyComboBoxEditReplacement.Visibility = Visibility.Hidden;
                this.CountryComboBoxEditReplacement.Visibility = Visibility.Visible;
            }
        }

        private void cmboBuyListType_LostFocus(object sender, RoutedEventArgs e)
        {

            if (cmboBuyListType.SelectedIndex == 0)
            {
                _vm.BuyListTypeID = 0;
            }

            if (cmboBuyListType.SelectedIndex == 1)
            {
                _vm.BuyListTypeID = 1;
            }

            if (cmboBuyListType.SelectedIndex == 2)
            {
                _vm.BuyListTypeID = 2;
            }

        }

        private void cmboReplacementType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmboReplacementType.SelectedIndex == 0)
            {
                _vm.ReplacementTypeID = 0;
            }

            if (cmboReplacementType.SelectedIndex == 1)
            {
                _vm.ReplacementTypeID = 1;
            }

            if (cmboReplacementType.SelectedIndex == 2)
            {
                _vm.ReplacementTypeID = 2;
            }
        }

        private void HierachyComboBoxEditBuylist_LostFocus(object sender, RoutedEventArgs e)
        {
            if (HierachyComboBoxEditBuylist.SelectedIndex < 0) return;
           _vm.BuyListID = (Int32)((Replacement.Client.ComboBoxItem)HierachyComboBoxEditBuylist.SelectedItem).HiddenValue;
            //HierachyComboBoxEditBuylist.Items.Add(new Replacement.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(hierarchy_sector_id)));
        }

        private void HierachyComboBoxEditBuylist_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (HierachyComboBoxEditBuylist.SelectedIndex < 0) return;
            _vm.BuyListID = (Int32)((Replacement.Client.ComboBoxItem)HierachyComboBoxEditBuylist.SelectedItem).HiddenValue;
        }

        private void HierachyComboBoxEditReplacement_LostFocus(object sender, RoutedEventArgs e)
        {
            if (HierachyComboBoxEditReplacement.SelectedIndex < 0) return;
            _vm.ReplacementListid = (Int32)((Replacement.Client.ComboBoxItem)HierachyComboBoxEditReplacement.SelectedItem).HiddenValue;
        }

        private void HierachyComboBoxEditReplacement_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (HierachyComboBoxEditReplacement.SelectedIndex < 0) return;
            _vm.ReplacementListid = (Int32)((Replacement.Client.ComboBoxItem)HierachyComboBoxEditReplacement.SelectedItem).HiddenValue;
        }

        private void CountryComboBoxEditBuylist_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CountryComboBoxEditBuylist.SelectedIndex < 0) return;
            _vm.BuyListID = (Int32)((Replacement.Client.ComboBoxItem)CountryComboBoxEditBuylist.SelectedItem).HiddenValue;
        }

        private void CountryComboBoxEditBuylist_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (CountryComboBoxEditBuylist.SelectedIndex < 0) return;
            _vm.BuyListID = (Int32)((Replacement.Client.ComboBoxItem)CountryComboBoxEditBuylist.SelectedItem).HiddenValue;
        }

        private void CountryComboBoxEditReplacement_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CountryComboBoxEditReplacement.SelectedIndex < 0) return;
            _vm.ReplacementListid = (Int32)((Replacement.Client.ComboBoxItem)CountryComboBoxEditReplacement.SelectedItem).HiddenValue;
        }

        private void CountryComboBoxEditReplacement_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (CountryComboBoxEditReplacement.SelectedIndex < 0) return;
            _vm.ReplacementListid = (Int32)((Replacement.Client.ComboBoxItem)CountryComboBoxEditReplacement.SelectedItem).HiddenValue;
        }


    }
 
}
