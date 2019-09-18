namespace TaxHarvestor.Client.View
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
  
    using TaxHarvestor.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using TaxHarvestor.Client;
  


    public partial class TaxHarvestorViewerVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "TaxHarvestor.xaml";
         private const string MSGBOX_TITLE_ERROR = "Tax Harvestor";
        
         public TaxHarvestorModel _view;


         # region Parameters

         private string m__symbol = string.Empty;

       

         private string _search;
         public string Search
         {
             set { _search = value; }
             get { return _search; }
         }

         private decimal _amount;
         public decimal Amount
         {
             set { _amount = value; }
             get { return _amount; }
         }

         private string method = "FIFO";
         public string Method
         {
             set { method = value; }
             get { return method; }
         }

      
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

         public TaxHarvestorViewerVisual(TaxHarvestorModel ViewerModel)
         {


             InitializeComponent();
             this.DataContext = ViewerModel;
             this._view = ViewerModel;
             GetSecurities();
         
             this.comboBoxEdit1.Text = _view.AccountName;

             DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

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


                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search);
                              
                                   // this.TXTHarvestamount.Text = "0";
                                  
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

        public void get_account_name(string account_id)
        {
            string textEnteredPlusNew = "account_id = " + account_id;

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
                        _view.AccountName = Convert.ToString(item);
                        _view.Parameters.AccountName = _view.AccountName;

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

    

        # region HelperProcedures



        private string IsStringNull(string a)
        {

            if (string.IsNullOrEmpty(a))
            {
                m__symbol = string.Empty;
                return m__symbol;

            }
            else
            {
                m__symbol = a;
                return m__symbol;
            }

        }

        public void se_rank_tax_lots_account()
        {


            try
            {
                ThreadPool.QueueUserWorkItem(
              delegate(object eventArg)
              {
                
                  ApplicationMessageList messages = null;


                  DataSet WashSale = new DataSet();


                  WashSale = _view.DBService.se_rank_tax_lots_account(

                   _view.AccountId,
                    method,
                    _amount,
                    IsStringNull(Search),
                    out messages);

                  // this.AccountHeader.DataContext = AccountHeader;





                  if (WashSale.Tables.Count > 0)
                  {

                      Dispatcher.BeginInvoke(new Action(() =>
                      {

                          this._dataGrid.ItemsSource = WashSale.Tables[0];




                      }), System.Windows.Threading.DispatcherPriority.Normal);

                  }
                  else
                      Dispatcher.BeginInvoke(new Action(() => { this._dataGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);



              });


                GetOrdersCount();


            }
            catch (Exception ex)
            {

                throw ex;
            }
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

            _view.AccountName = _view.Parameters.AccountName;
           

            AssignXML();


        }

     

        public void GetSecurities()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    //IHierarchyViewerServiceContract DBService = this.CreateServiceClient();

                    //ApplicationMessageList messages = null;
                    //DataSet dsSector = new DataSet();
                    //sector
                    AllSecurities = _view.DBService.GetSecurityInfo("");

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }


        public void GetOrdersCount()
        {
            int count = 0;
            _dataGrid.SelectAll();

            foreach (DataRowView row in _dataGrid.SelectedItems)
            {
                //open the accounts



                decimal selectedQTY = Convert.ToDecimal(row["selected_qty"]);

                if (selectedQTY > 0)
                {
                    count = count + 1;

                }

            }



            string LABEL = string.Format("You will created {0} orders at this Harvest Amount.", count);


            try
            {

                ThreadPool.QueueUserWorkItem(
              delegate(object eventArg)
              {
                  Dispatcher.BeginInvoke(new Action(() =>
                  {
                      this.lblSummary.Content = LABEL;


                  }), System.Windows.Threading.DispatcherPriority.Normal);

              });

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public void Get_ChartData()
        {

            //AssignXML();

            //this._dataGrid.ItemsSource = null;

        

                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet dsRoboDrift = new DataSet();

                    //Transaction


                    dsRoboDrift = _view.DBService.se_get_orders(
                    Convert.ToInt32(_view.AccountId),
                    out messages);

                    _view.MyDataTable = dsRoboDrift.Tables[0];



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

        

        private void TxtYtdDesiredTotal_LostFocus(object sender, RoutedEventArgs e)
        {

            decimal YTD;
            decimal DYTD;
            decimal HA;


            _dataGrid.SelectAll();

            foreach (DataRowView row in _dataGrid.SelectedItems)
            {
                //open the accounts



                try
                {

                    ThreadPool.QueueUserWorkItem(
                  delegate(object eventArg)
                  {
                      Dispatcher.BeginInvoke(new Action(() =>
                      {
                          YTD = Convert.ToDecimal(this.TXTYTDTotal.Text);
                          DYTD = Convert.ToDecimal(this.TxtYtdDesiredTotal.Text);
                          HA = DYTD - YTD;

                          Amount = Convert.ToDecimal(HA);

                          this.TXTHarvestamount.Text = HA.ToString("N2");


                      }), System.Windows.Threading.DispatcherPriority.Normal);

                  });

                }
                catch (Exception ex)
                {

                    throw ex;
                }



                _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            Search = txtSearch.Text;
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 

        }

        private void TXTHarvestamount_LostFocus(object sender, RoutedEventArgs e)
        {
            Amount = this.TXTHarvestamount.Value;
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
        }
   

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
        }

  

       
    

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
            }
            Get_From_info();


            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
         
           

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

        private void btnCreateOrders_Click(object sender, RoutedEventArgs e)
        {
            CreateOrders();
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



        public void CreateOrders()
        {
            int count = 0;


            _dataGrid.SelectAll();

            foreach (DataRowView row in _dataGrid.SelectedItems)
            {
                //open the accounts

                ApplicationMessageList messages = null;
               
                decimal selectedQTY = Convert.ToDecimal(row["selected_qty"]);

                if (selectedQTY > 0)
                {
                    count = count + 1;
                    _view.CreateOrders(
                        Convert.ToDecimal(row["account_id"]),
                        Convert.ToDecimal(row["security_id"]),
                        Convert.ToDecimal(row["selected_qty"]),
                        Convert.ToDateTime(row["trade_date"]),
                        Convert.ToDecimal(row["tax_lot_id"]));
                       

                          
                }

            }



            string LABEL = string.Format("You have created {0} orders.", count);


            try
            {

                ThreadPool.QueueUserWorkItem(
              delegate(object eventArg)
              {
                  Dispatcher.BeginInvoke(new Action(() =>
                  {
                     // this.lblSummary.Content = LABEL;


                  }), System.Windows.Threading.DispatcherPriority.Normal);

              });

            }
            catch (Exception ex)
            {

                throw ex;
            }

            

            System.Windows.MessageBox.Show(LABEL, "Orders Created", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        private void cmbTaxlotmethods_LostFocus(object sender, RoutedEventArgs e)
        {

            method = this.cmbTaxlotmethods.Text;
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
        }


        #endregion Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
        }

        private void BtnRecalculateClick(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), method, Amount, Search); 
        }

      

    

    
      

       

      

        



    

    }
}
