using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Documents;
using Linedata.Framework.Foundation;
using Linedata.Shared.Api.ServiceModel;

using HierarchyViewerAddIn.Shared.ServiceContracts;
using System.IO;
using PSG.AccountTreeControl;
using IpoViewerAddIn.Client.DetailReport.Model;
using Linedata.Client.AddIn.HostView.AppAddIns;
using Linedata.Client.AddIn;
using Linedata.Client;
using DevExpress.Xpf.Grid;
using System.Windows.Data;

namespace IpoViewerAddIn.Client.DetailReport
{
    
    public partial class IPOViewerMainWindow : UserControl
    {

        private int _countryCode = 1;
        private string _countryname = "United States";
        
       
        private DataSet dsSecurity = new DataSet();
        string subPath = "c:\\dashboard";
        string file = "IPOViewer.xaml";
        private int m_intAccountID = 73;
        private string m_strAccountShortName = "WM10";
        private DataTable m_AllAccounts;
        private DataSet m_AllSecurity1;
        private DataTable m_AllSecurity;
        private MTCacheConnection MTCache;
        private string m__symbol = string.Empty;
        private string method = "FIFO";

        private decimal _amount = 0;
        private string _search = string.Empty;
        private string _stRealizedGain;
        private string _ltRealizedGain;
        private int m_intAssetCode = 1;
        private string m_strAssetCode = "Equity";
        private string m_strSecurityShortName = "";
        private const string MSGBOX_TITLE_ERROR = "Security Lookup";
        private decimal _weight = 0;
        private decimal _midPrice = 0;
        private decimal _highPrice = 0;
   
        string _startDate;

        private List<string> legalAssets;
        //private MTCacheConnection MTCache;
        private int m_intSecurityID = -1;
        private string m_symbol = "";
      
      
        private decimal _accountId = 215;
        
        private string _chkText;

        private decimal _exchangeRate = 1;

        public decimal ExchangeRate
        {
            get { return _exchangeRate; }
            set { _exchangeRate = value; }
        }
      

        public decimal HighPrice
        {
            get { return _highPrice; }
            set { _highPrice = value; }
        }

        public decimal MidPrice
        {
            get { return _midPrice; }
            set { _midPrice = value; }
        }

        public decimal Weight
        {
            get { return _weight; }
             set {_weight = value;}
    
        }

                public int CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        public string CountryName
        {
            get { return _countryname; }
            set { _countryname = value; }
        }

        public decimal AccountId
        {
            set { _accountId = value; }
            get { return _accountId; }
        }

        public string AccountName
        {
            set { m_strAccountShortName = value; }
            get { return m_strAccountShortName; }
        }

        public string LtRealizedGain
        {
            set { _ltRealizedGain = value; }
            get { return _ltRealizedGain; }
        }

        public string StRealizedGain
        {
            set { _stRealizedGain = value; }
            get { return _stRealizedGain; }
        }
       
       
       
        public string Follower
        {
            set { _chkText = value; }
            get { return _chkText;  }
        }

        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }

        public string Search
        {
            set { _search = value; }
            get { return _search; }
        }

     
        public IPOViewerMainWindow(IpoViewerModel view)
        {
            InitializeComponent();

            MTCache = new MTCacheConnection();
            m_AllAccounts = MTCache.GetAllAccounts();
            m_AllSecurity1 = MTCache.GetListSecurity(m_intAssetCode);
            m_AllSecurity = m_AllSecurity1.Tables[0];
            this.txtAccount1.Text = m_strAccountShortName;

            DateTime nowDate = DateTime.Now;
            this.txtdate.Text = Convert.ToString(nowDate.ToString("MM/dd/yyyy"));
            this.DataContext = view;

        }




        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //setDates();
            if (File.Exists((subPath + "\\" + file)))
            {

                
                IpoData.RestoreLayoutFromXml(subPath + "\\" + file);
            }


            CountryList();
           
          
           
          
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {


            //gridTransactions.RestoreLayoutFromXml("c:\\Gridtransactions.xaml");

             // your code goes here

            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
                
                IpoData.SaveLayoutToXml(subPath + "\\" + file);
            }
            else
            {
                IpoData.SaveLayoutToXml(subPath + "\\" + file);
            }
         



        }


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


        private IHierarchyViewerServiceContract CreateServiceClient()
        {
            try
            {
                return TransportManagerProxyFactory<IHierarchyViewerServiceContract>.GetReusableInstance("HierarchyViewerServiceContract");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            Follower = cmboFollower.Text;
            setform();
        }


        private bool validateSingleAccount(TextBox ourTextBox)
        {
            bool retval = false;

            if (!String.IsNullOrEmpty(ourTextBox.Text))
            {
                try
                {
                    int defaultAccountId = -1;
                    string defaultShortName = "";
                    MTCacheConnection MTCache = new MTCacheConnection();
                    MTCache.ValidateAccountForUser(ourTextBox.Text, out defaultAccountId, out defaultShortName);
                    if (defaultAccountId != -1)
                    {
                        m_intAccountID = defaultAccountId;
                        m_strAccountShortName = defaultShortName;
                        ourTextBox.Text = m_strAccountShortName;        // Ensure upcased
                        retval = true;
                    }
                    else
                    {
                        m_intAccountID = -1;
                        m_strAccountShortName = "";
                        ourTextBox.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    m_intAccountID = -1;
                    m_strAccountShortName = "";
                    throw ex;
                }
            }
            else
            {
                m_intAccountID = -1;
                m_strAccountShortName = "";
                ourTextBox.Text = "";
            }
            return retval;

        }

        private void txtAccount_TextChanged(object sender, TextChangedEventArgs e)
        {

            //check for exact
            string textEnteredPlusNew = "short_name = '" + txtAccount1.Text + "'";
            DataRow[] rowz = m_AllAccounts.Select(textEnteredPlusNew);

            if (rowz.Length == 1)
            {

                m_intAccountID = Convert.ToInt32(rowz[0]["account_id"].ToString());
                _accountId = Convert.ToInt32(rowz[0]["account_id"].ToString());
                m_strAccountShortName = rowz[0]["short_name"].ToString();
                txtAccount1.Text = m_strAccountShortName;

                e.Handled = true;
                // Get_ChartData(TabIndex);
                return;
            }
            else
            {
                //check for almost
                textEnteredPlusNew = "short_name Like '" + txtAccount1.Text + "%'";
                rowz = m_AllAccounts.Select(textEnteredPlusNew);
                if (rowz.Length == 1)
                {
                    m_intAccountID = Convert.ToInt32(rowz[0]["account_id"].ToString());
                    _accountId = Convert.ToInt32(rowz[0]["account_id"].ToString());
                    m_strAccountShortName = rowz[0]["short_name"].ToString();
                    txtAccount1.Text = m_strAccountShortName;

                    e.Handled = true;
                    //Get_ChartData(TabIndex);
                    return;
                }
            }
            e.Handled = false;



        }

        private void txtAccount_LostFocus(object sender, RoutedEventArgs e)
        {
            string tempText = txtAccount1.Text;
            if (!validateSingleAccount(txtAccount1))
            {

                if (!String.IsNullOrEmpty(tempText))
                {

                    MessageBox.Show("You do not have access to an account with the name '" + tempText + "'.");
                }
            }

            
        }


        private void setform()
        {
            
            if (cmboFollower.Text != "Self")
            {
                txtAccount1.IsEnabled = false;
            }
            else
            {
                txtAccount1.IsEnabled = true;
                //setGL();
            }

        }


        public void UpdateAccoutText()
        {

            
                txtAccount1.Text = AccountName;
           

        }







        public void se_get_ipo_data()
        {

            int cc = cmbTargetWeight.Text == "Global" ? -1 : _countryCode;
            
      
            try
            {
                ThreadPool.QueueUserWorkItem(
              delegate(object eventArg)
              {
                  //decimal amount = Convert.ToDecimal(this.TXTHarvestamount.Value);
                  IHierarchyViewerServiceContract DBService = this.CreateServiceClient();
                  ApplicationMessageList messages = null;


                  DataSet IPOData = new DataSet();

               
         
                 
                  IHierarchyViewerServiceContract dbService = this.CreateServiceClient();
                  IPOData = dbService.se_get_ipo_data(
                    m_intAccountID,
                    cc,
                    _weight,
                    _midPrice,
                    _highPrice,
                    out messages);

                  // this.AccountHeader.DataContext = AccountHeader;

               


                  if (IPOData.Tables.Count > 0)
                  {

                      Dispatcher.BeginInvoke(new Action(() =>
                      {

                        
                          this.IpoData.ItemsSource = IPOData.Tables[0];




                      }), System.Windows.Threading.DispatcherPriority.Normal);

                  }
                  else
                      Dispatcher.BeginInvoke(new Action(() => { this.IpoData.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);



              });

              
                    
                    

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       

      

       

        private void btnCreateOrders_Click(object sender, RoutedEventArgs e)
        {
            CreateOrders();
        }

  

        public void CreateOrders()
        {
            int count = 0;


            
            IpoData.SelectAll();
            GridColumn colOnhold = IpoData.Columns["Inc"];

            foreach (DataRowView row in IpoData.SelectedItems)
            {
                //open the accounts

                ApplicationMessageList messages = null;
                IHierarchyViewerServiceContract dbService = this.CreateServiceClient();
                decimal selectedQTY;

                //Boolean isc = Convert.ToBoolean(row["Inc"]);
                //string isa = Convert.ToString(row["Inc"]);

                //string _on_hold = IpoData.GetCellValue(count, colOnhold).ToString();

                bool chk = Convert.ToBoolean(IpoData.GetCellValue(count, "Inc"));

              

                if (chk == true)
                {
                    if (rdoMidPrice.IsChecked == true)
                    {
                        selectedQTY = Convert.ToDecimal(row["Share Esitmate of Mid Offer Price"]);
                    }
                    else
                    {
                        selectedQTY = Convert.ToDecimal(row["Share Esitmate of High Offer Price"]);
                    }



                    if (selectedQTY > 0)
                    {
                        count = count + 1;
                        dbService.se_create_orders_ipo(
                            m_intSecurityID,
                            Convert.ToInt32(row["Account_id"]),
                              selectedQTY,

                               out messages);
                    }

                }
            }
            string LABEL = string.Format("You have created {0} orders.", count);


            //update



            //RefreshReport("Tools", "Refresh Database");
            //RefreshReport("View", "Refresh");

            System.Windows.MessageBox.Show(LABEL, "Orders Created", MessageBoxButton.OK, MessageBoxImage.Information);

        }

   

        public void RefreshReport(string menu1, string menu2)
        {
            List<string> refreshMenuPath = new List<string>();
            refreshMenuPath.Add(menu1);
            refreshMenuPath.Add(menu2);
            LinedataApp.Instance.ActiveMasterReport.InvokeMenuCommand(refreshMenuPath);


        }


     

        //security-------------------------------------------------------------------------------------------------------------------------

        private void txtSecurity_LostFocus(object sender, RoutedEventArgs e)
        {
            string tempText = txtAccount1.Text;
            if (!validateSingleSecurity(txtSecurity))
            {

                if (!String.IsNullOrEmpty(tempText))
                {

                    MessageBox.Show("You do not have access to an security with the name '" + tempText + "'.");
                }
            }
            else
            {
                string targetweight = this.cmbTargetWeight.Text;
                string security = this.txtSecurity.Text;
                string LABEL = string.Format("Share Estimate at the {0} level for security {1}.", targetweight, security);

                this.lblSummary.Content = LABEL;
            }
          
        }


        private bool validateSingleSecurity(System.Windows.Controls.TextBox ourTextBox)
        {
            string assetType = "";
            bool retval = false;
            //this.labelSecName.Content = "";
            ;
            if (!String.IsNullOrEmpty(ourTextBox.Text))
            {
                int intRowCount = 0;
                try
                {
                    MTCacheConnection MTCache = new MTCacheConnection();
                    DataSet ds = MTCache.GetSecurityInfo(ourTextBox.Text);
                    if (ds.Tables.Count > 0)
                    {
                        intRowCount = ds.Tables[0].Rows.Count;
                    }
                    if (intRowCount >= 1)
                    {
                      
                        if (isAssetValid("Equity"))
                        {

                            m_strSecurityShortName = (string)(ds.Tables[0].Rows[0])["symbol"];
                            ourTextBox.Text = m_strSecurityShortName;

                            decimal pricelatest = Convert.ToDecimal((ds.Tables[0].Rows[0])["latest"]);
                              _exchangeRate = Convert.ToDecimal((ds.Tables[0].Rows[0])["exchange_rate"]);
                            
                            txtpricelocalhigh.Text = Convert.ToString(Math.Round(pricelatest,4));
                            txtpricelocalmid.Text = Convert.ToString(Math.Round(pricelatest,4));

                            HighPrice = Convert.ToDecimal(Math.Round(pricelatest / _exchangeRate, 4));
                            MidPrice = Convert.ToDecimal(Math.Round(pricelatest / _exchangeRate, 4));

                            txtpriceusdhigh.Text = Convert.ToString(Math.Round(pricelatest / _exchangeRate, 4));
                            txtpriceusdmid.Text = Convert.ToString(Math.Round(pricelatest / _exchangeRate, 4));
                           
                            string country = ((string)(ds.Tables[0].Rows[0])["name"]);
                            int v = -1;

                            for (int i = 0; i < cmbCountry.Items.Count; i++)
                            {
                                string value = cmbCountry.Items[i].ToString();

                                if(value == country)
                                {
                                    v = i;
                                    _countryname = country;
                                 

                                    _countryCode = Convert.ToInt32((ds.Tables[0].Rows[0])["country_code"]);
                               
                                }
                                
                                    
                            }

                        

                            cmbCountry.SelectedIndex = v;

                           

                            
                           


                            // TODO - separate label and content

                            retval = true;
                        }
                        else
                        {
                            retval = false;
                        }
                    }
                }
                catch (Exception ex)
                {

                   // System.Windows.MessageBox.Show(this, "An error has occurred." + System.Environment.NewLine + "Unable to get information about entered security." + System.Environment.NewLine + ex.Message, MSGBOX_TITLE_ERROR, MessageBoxButton.OK);
                    m_intSecurityID = -1;
                }
            }
            else
            {
                m_intSecurityID = -1;
                m_strSecurityShortName = "";
                ourTextBox.Text = m_strSecurityShortName;

            }
            return retval;
        }

        private bool isAssetValid(string assetType)
        {
            return true;
        }

        private void txtSecurity_TextChanged(object sender, TextChangedEventArgs e)
        {
            //check for exact
            string textEnteredPlusNew = "mnemonic = '" + txtSecurity.Text + "'";
          
            DataRow[] rowz = m_AllSecurity.Select(textEnteredPlusNew);

            if (rowz.Length == 1)
            {

               
                m_intSecurityID = Convert.ToInt32(rowz[0]["security_id"].ToString());
                m_strSecurityShortName = rowz[0]["mnemonic"].ToString();
                 txtSecurity.Text = m_strSecurityShortName;

                

                e.Handled = true;
                // Get_ChartData(TabIndex);
                return;
            }
            else
            {
                
                //check for almost
                textEnteredPlusNew = "mnemonic Like '" + txtSecurity.Text + "%'";

             
                rowz = m_AllSecurity.Select(textEnteredPlusNew);
                if (rowz.Length == 1)
                {
                    m_intSecurityID = Convert.ToInt32(rowz[0]["security_id"].ToString());
                    m_strSecurityShortName = rowz[0]["mnemonic"].ToString();
                    txtSecurity.Text = m_strSecurityShortName;

                    e.Handled = true;
                    //Get_ChartData(TabIndex);
                    return;
                }
            }
            e.Handled = false;


        }

       

        public void CountryList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmbCountry.Items.Clear();

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    IHierarchyViewerServiceContract DBService = this.CreateServiceClient();
                  



                    DataSet ds = DBService.get_list_country();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["name"];
                            object county_code = row["country_code"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmbCountry.Items.Add(new ComboBoxItem(Convert.ToString(item), Convert.ToInt64(county_code)));

                                // this.cmboHierarchy.SelectedIndex = 0;

                                
                        


                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }


                    //this.DataContext = _view;


                });
        }

       

        private void cmbTargetWeight_DropDownClosed(object sender, EventArgs e)
        {
            string targetweight = this.cmbTargetWeight.Text;
            string security = this.txtSecurity.Text;
            string LABEL = string.Format("Share Estimate at the {0} level for security {1}.", targetweight, security);

           

           

            this.lblSummary.Content = LABEL;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.txtSecurity.Text != null & this.cmbTargetWeight.Text != null)
            {

                IpoData.DataContext = null;
                se_get_ipo_data();
            }
            else
            {
                MessageBox.Show("Please Fill in appropriate fields.");
            }

        }

        private void TXTWeight_LostFocus(object sender, RoutedEventArgs e)
        {
            _weight = Convert.ToDecimal(TXTWeight.Text);
        }

        private void txtpricelocalhigh_LostFocus(object sender, RoutedEventArgs e)
        {
            decimal high = Convert.ToDecimal(this.txtpricelocalhigh.Text);
            _highPrice = high / _exchangeRate;
           
            this.txtpriceusdhigh.Text = Convert.ToString(_highPrice);
        }

        private void txtpricelocalmid_LostFocus(object sender, RoutedEventArgs e)
        {
            decimal mid = Convert.ToDecimal(this.txtpricelocalmid.Text);
            _midPrice = mid/_exchangeRate;
            this.txtpriceusdmid.Text = Convert.ToString(_midPrice);
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

      


      
    }
}
