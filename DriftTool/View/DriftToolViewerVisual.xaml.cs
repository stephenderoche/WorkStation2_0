namespace ExceptionManagement.Client.View
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
  
    using ExceptionManagement.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using ExceptionManagement.Client;
  


    public partial class DriftToolViewerVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "ExceptionTool.xaml";
         private const string MSGBOX_TITLE_ERROR = "Exceptions";
        
         public DriftToolViewerModel _view;


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

        
         private string _selectAccountName = string.Empty;
         public string SelectAccountName
         {
             set { _selectAccountName = value; }
             get { return _selectAccountName; }
         }
         # endregion Parameters

         public DriftToolViewerVisual(DriftToolViewerModel genericGridViewerModel)
         {


             InitializeComponent();
             this.DataContext = genericGridViewerModel;
             this._view = genericGridViewerModel;
            
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

                            Dispatcher.BeginInvoke(new Action(() =>
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

                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }

      

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
         private void _dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                int index =
                Convert.ToInt32(_dataGrid.GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());


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

                                _view.WidgetName = "DriftTool";
                                 _view.SetSelectedAccount(Convert.ToInt64(_account_id));


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
        private void Rebalance_Click(object sender, RoutedEventArgs e)
        {

            
          
            ApplicationMessageList messages = null;

          

        
                                            int i = 0;
                                            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
                                            for (i = 0; i < listRowList.Length; i++)
                                            {


                                                GridColumn colAccount = _dataGrid.Columns["short_name"];
                                                GridColumn colaccount_id = _dataGrid.Columns["account_id"];
                                                GridColumn colOnhold = _dataGrid.Columns["on_hold"];



                                                if (colaccount_id != null)
                                                {
                                                    if (_dataGrid.GetCellValue(listRowList[i], colAccount).ToString() != null)
                                                    {

                                                        string _account_name = _dataGrid.GetCellValue(listRowList[i], colAccount).ToString();
                                                        SelectAccountName = _account_name;
                                                        string _account_id = _dataGrid.GetCellValue(listRowList[i], colaccount_id).ToString();
                                                        SelectedAccountId = Convert.ToInt32(_account_id);
                                                        string _on_hold = _dataGrid.GetCellValue(listRowList[i], colOnhold).ToString();

                                                        if (_on_hold == "False")
                                                        {


                                                            _view.se_update_drift_summary(Convert.ToInt32(_account_id), _account_name);

                                                        }
                                                    }
                                                }


                                                //send orders

                                            }

                                                System.Windows.MessageBox.Show(string.Format("You have Robo Rebalanced {0} accounts", i), "Robo Rebalance", MessageBoxButton.OK, MessageBoxImage.Information);




                                                _view.Get_ChartData(Convert.ToInt32(_view.AccountId));

        }

        private void se_send_robo_proposed()
        {

            ThreadPool.QueueUserWorkItem(
               delegate(object eventArg)
               {

                   ApplicationMessageList messages = null;


                   _view.DBService.se_send_robo_proposed(
                                                Convert.ToInt32(1),
                                                  out messages);

                   //Get_ChartData();

                   Dispatcher.BeginInvoke(new Action(() =>
                   {

                      // Get_ChartData();

                   }), System.Windows.Threading.DispatcherPriority.Normal);
               });
        }

        private void RebalanceProposed_Click(object sender, RoutedEventArgs e)
        {
            //DataRow rowData;
            ApplicationMessageList messages = null;
          
            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {


                GridColumn colAccount = _dataGrid.Columns["short_name"];
                GridColumn colaccount_id = _dataGrid.Columns["account_id"];
                GridColumn colOnhold = _dataGrid.Columns["on_hold"];



                if (colaccount_id != null)
                {
                    if (_dataGrid.GetCellValue(listRowList[i], colAccount).ToString() != null)
                    {

                        string _account_name = _dataGrid.GetCellValue(listRowList[i], colAccount).ToString();
                        string _account_id = _dataGrid.GetCellValue(listRowList[i], colaccount_id).ToString();
                        string _on_hold = _dataGrid.GetCellValue(listRowList[i], colOnhold).ToString();

                        if (_on_hold == "False")
                        {
                           // Rebalance(_account_name, "Regular");
                        }
                        _view.DBService.se_update_drift_summary(
                              Convert.ToInt32(_account_id),
                                out messages);

                    }
                }

            }



            //Get_ChartData();



            System.Windows.MessageBox.Show(string.Format("You have Robo Rebalanced {0} accounts", i), "Robo Rebalance", MessageBoxButton.OK, MessageBoxImage.Information);


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
