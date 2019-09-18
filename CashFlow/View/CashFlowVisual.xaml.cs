namespace CashFlow.Client.View
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
   
  
    using CashFlow.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using CashFlow.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
  


    public partial class CashFlowVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "CashFlow.xaml";
         private string getsecurityTextinfo = "";
         private int getsecurityLenthinfo = 0;
         public CashFlowViewerModel _view;


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


         private bool _showAccount;
         public bool ShowAccount
         {
             get { return _showAccount; }
             set { _showAccount = value; }
         }

         private bool _showDesk;
         public bool ShowDesk
         {
             get { return _showDesk; }
             set { _showDesk = value; }
         }

         private bool _showSecurity;
         public bool ShowSecurity
         {
             get { return _showSecurity; }
             set { _showSecurity = value; }
         }

         private bool _showStartDate;
         public bool ShowStartDate
         {
             get { return _showStartDate; }
             set { _showStartDate = value; }
         }

         private bool _showEndDate;
         public bool ShowEndDate
         {
             get { return _showEndDate; }
             set { _showEndDate = value; }
         }

        

         # endregion Parameters

         public CashFlowVisual(CashFlowViewerModel genericGridViewerModel)
         {


             InitializeComponent();
             this.DataContext = genericGridViewerModel;
             this._view = genericGridViewerModel;
             _view.CreateServiceClient();
            
          
             GetSecurities();
         
             this.comboBoxEdit1.Text = _view.AccountName;
             this.SecurityComboBoxEdit.Text = Symbol;
             
          

             

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

                                   // _view.Get_chart_data(AccountId, SecurityId,StartDate,EndDate);
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

                        Dispatcher.BeginInvoke(new Action(() =>
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
                                if (AllSecurities.Tables.Count > 0)
                                {
                                    for (int rowIndex = 0; rowIndex < AllSecurities.Tables[0].Rows.Count; ++rowIndex)
                                    {
                                        if (ourTextBox.ToUpper().CompareTo(((AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString().ToUpper()) == 0)
                                        {
                                            defaultSymbol = ((AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString();
                                            defaultSecurityId = Convert.ToInt32((AllSecurities.Tables[0].Rows[rowIndex])["security_id"]);
                                            break;
                                        }
                                    }
                                }


                                if (defaultSecurityId != -1)
                                {

                                    SecurityId = defaultSecurityId;
                                    Symbol = defaultSymbol;
                                    _view.Parameters.SecurityName = Symbol;

                                   // Get_ChartData();
                                    retval = true;
                                }
                                if (defaultSecurityId != -1)
                                {

                                    SecurityId = defaultSecurityId;
                                    Symbol = defaultSymbol;
                                    _view.Parameters.SecurityName = Symbol;

                                   // Get_ChartData();
                                    retval = true;
                                }

                                else
                                {
                                    SecurityId = -1;
                                    Symbol = "";

                                }
                            }
                            catch (Exception ex)
                            {
                                SecurityId = -1;
                                Symbol = "";
                                throw ex;
                            }

                            Dispatcher.BeginInvoke(new Action(() =>
                            {

                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        });
                }

                catch (Exception ex)
                {
                    SecurityId = -1;
                    Symbol = "";
                    throw ex;
                }

                return retval;
            }

            else
            {
                SecurityId = -1;
                Symbol = "";
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
                    var someObject = AllSecurities;
                    if (someObject == null)
                    {
                        GetSecurities();
                        return;
                    }
                    foreach (DataRow row in AllSecurities.Tables[0].Select(textEnteredPlusNew))
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


            ValidateSecurity(_view.Parameters.SecurityName);


        }
        private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


            ValidateSecurity(comboBoxEdit.Text);
        }

      

        # endregion Security

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
            Symbol = String.IsNullOrEmpty(_view.Parameters.SecurityName) ? String.Empty : _view.Parameters.SecurityName;
             EndDate = _view.Parameters.EndDate;
            StartDate = _view.Parameters.StartDate;

            AssignXML();

            getDates();

         
            
           


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





            XML = _view.Parameters.XML;


            if (File.Exists((subPath + "\\" + XML)))
            {


                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + XML);


            }


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGrid.SaveLayoutToXml(subPath + "\\" + XML);

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + XML);

            }
        }

        public void UpdateAccoutText()
        {


            _view.Parameters.AccountName = _view.AccountName;


        }

        void getDates()
        {


            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(StartDate.ToString(), out intStr);
            if (intResultTryParse == true)
            {
                StartDate = (intStr);

            }
            else
            {
                StartDate = DateTime.Today;
            }

            DateTime intStr1;
            bool intResultTryParse1 = DateTime.TryParse(EndDate.ToString(), out intStr1);
            if (intResultTryParse1 == true)
            {
                EndDate = intStr1;
            }
            else
            {
                EndDate = DateTime.Today;
            }
        }
      

       
        # endregion HelperProcedures



        # region Methods
        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {


            viewRoboDrift.ShowPrintPreview(this);

         

        }

        private void start_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDates();

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

        private void end_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDates();

        }

    


     

    
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_chart_data(_view.AccountId, SecurityId, _view.Parameters.StartDate, _view.Parameters.EndDate);
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
