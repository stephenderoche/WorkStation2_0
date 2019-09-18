using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using System.Data;
using System.Threading;
using SalesSharedContracts;
using System.IO;
using System.Globalization;
using Linedata.Shared.Api.ServiceModel;
using Linedata.Client.Workstation.SharedReferences;
using Linedata.Framework.Foundation;
using Linedata.Shared.Api.ServiceModel;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using SalesSharedContracts;
using Linedata.Shared.Api.ServiceModel;

using System.Collections;

using System.Configuration;

using System.Threading.Tasks;
using TraderVsPM.Client.Model;
using TraderVsPM.Client.ViewModel;



namespace TraderVsPM.Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TraderVsPm : UserControl
    {
       
        private string security_symbol = "";
        private string issuer_name = "";
        private string search = "";
        private string begindate;
        private string enddate;
        private string account;
        public string _account_name;
        private const string MSGBOX_TITLE_ERROR = "Trader Vs. PM";
        private int _accountId = -1;
        private string _accountName = "";
       
        private int m_intAssetCode = -1;
        private string m_strAssetCode = "";
       
        private List<string> legalAssets;
        //private MTCacheConnection MTCache;
        private int m_intSecurityID = -1;

        private string m_strSecurityShortName = "";
        private string m_issuershortname = "";
        private int m_Issuer_id;
        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        private string getissuerTextinfo = "";
        private int getissuerLenthinfo = 0;
        string subPath = "c:\\dashboard";
        string file = "TraderVsPm.xaml";
        string file1 = "Journaling.xaml";
        TraderVsPmAppViewerModel _view;

        private ISalesSharedContracts _dbservice;
        public ISalesSharedContracts DBService
        {
            set { _dbservice = value; }
            get { return _dbservice; }
        }


        DateTime _startDate;
        DateTime _endDate;

        public DateTime StartDate
        {
            set { _startDate = value; }
            get { return _startDate; }
        }

        public DateTime EndDate
        {
            set { _endDate = value; }
            get { return _endDate; }
        }

        public Int32 AccountId
        {
            set { _accountId = value; }
            get { return _accountId; }
        }

        public string AccountName
        {
            set { _accountName = value; }
            get { return _accountName; }
        }

        private int _desId;
          public int DeskId
        {
            get { return _desId; }
            set { _desId = value; }
        }

         private string _desk;
         public string Desk
         {
             get { return _desk; }
             set { _desk = value; }
         }

         private bool _active;
         public bool Active
         {
             get { return _active; }
             set { _active = value; }
         }

         private bool _crossable;
         public bool Crossable
         {
             get { return _crossable; }
             set { _crossable = value; }
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



         public TraderVsPm(TraderVsPmAppViewerModel view)
        {
            InitializeComponent();

            

            this.DataContext = view;
            this._view = view;

            DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;
           
        }


        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ISalesSharedContracts DBService = this.CreateServiceClient();
                



                    DataSet ds = DBService.GetListAllDesks();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["desk_name"];
                            object desk_id = row["desk_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

                                count = count + 1;

                                if (Desk == Convert.ToString(item))
                                {
                                    this.cmboDesk.SelectedIndex = count;
                                    DeskId = Convert.ToInt32(desk_id);
                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }

        void getDates()
        {
           

            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(this.txtstartdate.Text, out intStr);
            if (intResultTryParse == true)
            {
                _startDate = (intStr);

            }
            else
            {
                _startDate = DateTime.Today;
            }

            DateTime intStr1;
            bool intResultTryParse1 = DateTime.TryParse(this.txtenddate.Text, out intStr1);
            if (intResultTryParse1 == true)
            {
                _endDate = intStr1;
            }
            else
            {
                _endDate = DateTime.Today;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //setDates();
            //if (File.Exists((subPath + "\\" + file)))
            //{
            //    dataGrid.RestoreLayoutFromXml(subPath + "\\" + file); 
            //}

            //if (File.Exists((subPath + "\\" + file1)))
            //{
            //    journalGrid.RestoreLayoutFromXml(subPath + "\\" + file1);
            //}

            load_asset_type();
            Get_From_info();

            Desk = _view.Parameters.DeskName;
            EndDate = _view.Parameters.EndDate;
            StartDate = _view.Parameters.StartDate;
            Active = _view.Parameters.Active;

            SetActive();
            getDates();
           

            GetSecurityHistory();
           
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {


            //bool exists = System.IO.Directory.Exists((subPath));

            //if (!exists)
            //{
            //    System.IO.Directory.CreateDirectory((subPath));
            //    dataGrid.SaveLayoutToXml(subPath + "\\" + file);
            //    journalGrid.SaveLayoutToXml(subPath + "\\" + file1);
            //}
            //else
            //{
            //    dataGrid.SaveLayoutToXml(subPath + "\\" + file);
            //    journalGrid.SaveLayoutToXml(subPath + "\\" + file1);
            //}




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
        private void get_asset_type_id()
        {

            if (cboAssetType.SelectedIndex == 0)
            {
                m_intAssetCode = -1;
            }
            if (cboAssetType.SelectedIndex == 1)
            {
                m_intAssetCode = 1;
            }
            else if (cboAssetType.SelectedIndex == 2)
            {
                m_intAssetCode = 3;
            }
            else if (cboAssetType.SelectedIndex == 3)
            {
                m_intAssetCode = 5;
            }
            else if (cboAssetType.SelectedIndex == 4)
            {
                m_intAssetCode = 7;
            }
            else if (cboAssetType.SelectedIndex == 5)
            {
                m_intAssetCode = 4;
            }
            else
            {
                m_intAssetCode = -1;
            }

        }

        private void cboAssetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            get_asset_type_id();
            m_strAssetCode = (null == cboAssetType.Text) ? "" : cboAssetType.Text;
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
                        ISalesSharedContracts dbService = this.CreateServiceClient();
                        ds = dbService.se_get_tradervspm_orders(

                            DeskId,
                            account,
                            security_symbol,
                            issuer_name,
                            search,
                            Convert.ToDateTime(begindate),
                            Convert.ToDateTime(enddate),
                            Active,
                            m_intAssetCode,
                            Crossable,
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

    


        private bool isAssetValid(string assetType)
        {
            return legalAssets.Contains(assetType.ToUpper());
        }

        //private void btnAccount_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.Cursor = System.Windows.Input.Cursors.Wait;

        //    var task = new System.Threading.Tasks.Task(() =>
        //       {
        //                        try
        //                        {

        //                                this.Dispatcher.BeginInvoke(new Action(() =>
        //                                {
        //                                        AccountTree accountTreeLookup = new AccountTree(2);

        //                                        //accountTreeLookup.Owner = 198;

        //                                        if (accountTreeLookup.ShowDialog() ==true)
        //                                        {
        //                                            setAccountinfo(accountTreeLookup);
        //                                        }
        //                                        //else
        //                                        //{
        //                                        //    //this.txtaccount_id.SelectAll();
        //                                        //}
        //                                }));

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                             this.Dispatcher.BeginInvoke(new Action(() =>
        //                                {
                                           
        //                            System.Windows.MessageBox.Show("An error has occurred.  " + ex.Message, MSGBOX_TITLE_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
        //                                }));
        //                        }
        //                        finally
        //                        {
        //                            this.Dispatcher.BeginInvoke(new Action(() =>
        //                                {
        //                            this.Cursor = System.Windows.Input.Cursors.Arrow;
        //                                }));
        //                        }

        //       });
        //    task.Start();
        //}

        //private void setAccountinfo(AccountTree accountTreeLookup)
        //{
        //    AccountId = accountTreeLookup.AccountID;
        //    AccountName = accountTreeLookup.ShortName;
        //    this.txtaccount_id.Text = AccountName;
        //}

        private void Get_From_info()
        {
            begindate = this.txtstartdate.Text;
            enddate = this.txtenddate.Text;
            account = this.txtaccount_id.Text;
            search = this.txtBrokerSearch.Text.ToUpper();
            if (this.radioSecurity.IsChecked == true)
            {
                security_symbol = this.SecurityTextBox.Text;
                issuer_name = "";
            }

            if (this.radioissuer.IsChecked == true)
            {
                issuer_name = this.txtissuer.Text;
                security_symbol = "";
            }

        }

        private void SetActive()
        {
            if (this.chkActive.IsChecked == true)
            {
                txtstartdate.IsEnabled = false;
                txtenddate.IsEnabled = false;
                Active = true;
                chkCrossable.IsEnabled = true;
                
            }
            else
            {
                txtstartdate.IsEnabled = true;
                txtenddate.IsEnabled = true;
                Active = false;
                chkCrossable.IsEnabled = false;

            }
        }

        private void filter(object sender, RoutedEventArgs e)
        {
            Get_From_info();
            GetSecurityHistory();
        }

      

        public ISalesSharedContracts CreateServiceClient()
        {
            try
            {
                DBService = ApiAccessor.Get<ISalesSharedContracts>();
                return DBService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            Get_From_info();
            GetSecurityHistory();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             int intRowCount = 0;

             var task = new System.Threading.Tasks.Task(() =>
               {
                   try
                   {
                       int asset_type_code = 0;

                      


                       asset_type_code = m_intAssetCode;//Convert.ToInt32(cboAssetType.SelectedValue);

                       DataSet ds = new DataSet();
                       ISalesSharedContracts  dbService = this.CreateServiceClient();
                       ds = dbService.GetListSecurity(asset_type_code);


                       if (ds.Tables.Count > 0)
                       {
                           intRowCount = ds.Tables[0].Rows.Count;
                       }

                       switch (intRowCount)
                       {
                           case 1:
                               m_intSecurityID = Convert.ToInt32((ds.Tables[0].Rows[0])["security_id"]);
                               m_strSecurityShortName = (string)(ds.Tables[0].Rows[0])["mnemonic"];
                               //m_issuershortname = (string)(ds.Tables[0].Rows[0])["short_name"];

                               m_issuershortname = String.IsNullOrEmpty((string)(ds.Tables[0].Rows[0])["short_name"]) ?
                         String.Empty :
                         (string)(ds.Tables[0].Rows[0])["short_name"];


                               this.SecurityTextBox.Text = m_strSecurityShortName;
                               break;
                           default:

                               DataView dv = ds.Tables[0].DefaultView;

                               this.Dispatcher.BeginInvoke(new Action(() =>
                               {
                                   getSecurityTextBoxInfo();
                               }));

                               
                               if ( getsecurityLenthinfo> 0)
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

        private void SetSecurityTextBoxInfo(SecurityLookup securityLookup)
        {

            m_intSecurityID = securityLookup.SecurityId;
            m_strSecurityShortName = securityLookup.ShortName;
            m_issuershortname = securityLookup.IssuerShortName;

            m_issuershortname = String.IsNullOrEmpty(securityLookup.IssuerShortName) ?
            String.Empty :
            securityLookup.IssuerShortName;
           this.SecurityTextBox.Text = m_strSecurityShortName;
           
            this.txtissuer.Text = m_issuershortname;
        }


        private void getSecurityTextBoxInfo()
        {

                getsecurityTextinfo = this.SecurityTextBox.Text.ToString();
                getsecurityLenthinfo = this.SecurityTextBox.Text.Length;
        }
        

        private void cboAssetType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
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


                                    ISalesSharedContracts dbService = this.CreateServiceClient();

                                     DataSet ds = dbService.GetIssuerInfo(); 

                                    if (ds.Tables.Count > 0)
                                    {
                                        intRowCount = ds.Tables[0].Rows.Count;
                                    }

                                    switch (intRowCount)
                                    {
                                        case 1:
                                            m_Issuer_id = Convert.ToInt32((ds.Tables[0].Rows[0])["issuer_id"]);
                        
                                            m_issuershortname = (string)(ds.Tables[0].Rows[0])["short_name"];
                                            this.txtissuer.Text = m_issuershortname;
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

        private void SetissuerTextBoxInfo(IssuerLookup IssuerLookup)
        {



            m_issuershortname = String.IsNullOrEmpty(IssuerLookup.IssuerShortName) ?
            String.Empty :
            IssuerLookup.IssuerShortName;
          

            this.txtissuer.Text = m_issuershortname;
        }


        private void getIssuerTextBoxInfo()
        {

            getissuerTextinfo = this.txtissuer.Text.ToString();
            getissuerLenthinfo = this.txtissuer.Text.Length;
        }

        private void SecurityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            getSecurityTextBoxInfo();
        }

        private void txtissuer_TextChanged(object sender, TextChangedEventArgs e)
        {
            getIssuerTextBoxInfo();
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

        private void cmboDesk_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
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
                    DeskId = Convert.ToInt32(((ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);
               
                }
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

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            System.Windows.Controls.TextBox t = e.EditingElement as System.Windows.Controls.TextBox;  // Assumes columns are all TextBoxes


            string a = t.Text;

            int editRow = e.Row.GetIndex();
            string b = e.Column.Header.ToString();

            UpdateJournal(a);
        }

        public void UpdateJournal(string note)
        {

            try
            {

                ApplicationMessageList messages = null;
                ISalesSharedContracts dbService = this.CreateServiceClient();
                dbService.se_update_journaling(
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

        public void Get_Packages()
        {
            DataSet ds = new DataSet();
            try
            {

                ApplicationMessageList messages = null;
                ISalesSharedContracts dbService = this.CreateServiceClient();
                ds = dbService.se_get_journaling(
                    Convert.ToInt32(_block_id),
                    out messages);
                this.journalGrid.DataContext = ds;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

   

     

        private void dataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            try
            {
                string symbol;
              


                int index =Convert.ToInt32(dataGrid.GetRowVisibleIndexByHandle(viewPMVsTrader.FocusedRowData.RowHandle.Value).ToString());



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


       private void Update_Forcaster(decimal qtyRemianing, decimal committedPrice, decimal qtyexecuted,decimal average_price_executed)
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


       public void UpdateCommitmentPrice(Int32 block_id,decimal price)
       {

           try
           {

               ApplicationMessageList messages = null;
               ISalesSharedContracts dbService = this.CreateServiceClient();
               dbService.se_update_committment_price(
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

       private void chkCrossable_Checked(object sender, RoutedEventArgs e)
       {
           Crossable = true;
       }

       private void chkCrossable_Unchecked(object sender, RoutedEventArgs e)
       {
           Crossable = false;
       }
       
//--End class


       }
    

   

    public class MyConverter : IValueConverter
    {
        public double MaxHeaderHeight
        {
            get;
            set;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value > MaxHeaderHeight)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}

    
       

