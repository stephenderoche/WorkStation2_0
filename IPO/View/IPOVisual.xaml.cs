namespace IPO.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using SalesSharedContracts;

    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using DevExpress.Xpf.Grid;
  
    using DevExpress.Xpf.Bars;
 
    using DevExpress.Xpf.Editors;
  
  
    using IPO.Client.ViewModel;
    using System.IO;
    using System.Threading;
  
     using IPO.Client;
  


    public partial class IPOVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "IPOViewer.xaml";
         private const string MSGBOX_TITLE_ERROR = "DataDashBoard";
        
         public IPOModel _view;


         # region Parameters

        string _xml;
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
        }
         private DataSet m_AllAccounts;
         public DataSet M_AllAccounts
         {
             set { m_AllAccounts = value; }
             get { return m_AllAccounts; }
         }

         private DataSet _allSecurities;
         public DataSet AllSecurities
         {
             set { _allSecurities = value; }
             get { return _allSecurities; }
         }


         private decimal _exchangeRate = 1;

         public decimal ExchangeRate
         {
             get { return _exchangeRate; }
             set { _exchangeRate = value; }
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

         # endregion Parameters

         public IPOVisual(IPOModel ViewerModel)
         {


             InitializeComponent();
             this.DataContext = ViewerModel;
             this._view = ViewerModel;
             
            
             GetSecurities();
         
             this.comboBoxEdit1.Text = _view.AccountName;

             DateTime nowDate = DateTime.Now;
             this.txtdate.Text = Convert.ToString(nowDate.ToString("MM/dd/yyyy"));

            
          
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
                    var someObject = m_AllAccounts;
                    if (someObject == null)
                    {
                        _view.GetAcc();
                        return;
                    }
                    foreach (DataRow row in m_AllAccounts.Tables[0].Select(textEnteredPlusNew))
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
                    var someObject = m_AllAccounts;
                    if (someObject == null)
                    {
                        _view.GetAcc();
                        return;
                    }
                    foreach (DataRow row in m_AllAccounts.Tables[0].Select(textEnteredPlusNew))
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
                                            decimal pricelatest = Convert.ToDecimal((AllSecurities.Tables[0].Rows[rowIndex])["latest"]);
                                            _exchangeRate = Convert.ToDecimal(((AllSecurities.Tables[0].Rows[rowIndex])["exchange_rate"]));

                                            _view.Latest = Convert.ToDecimal(Math.Round(pricelatest, 4));
                                            //txtpricelocalmid.Text = Convert.ToString(Math.Round(pricelatest, 4));

                                            _view.HighPrice = Convert.ToDecimal(Math.Round(pricelatest / _exchangeRate, 4));
                                           _view.MidPrice = Convert.ToDecimal(Math.Round(pricelatest / _exchangeRate, 4));

                                            //txtpriceusdhigh.Text = Convert.ToString(Math.Round(pricelatest / _exchangeRate, 4));
                                            //txtpriceusdmid.Text = Convert.ToString(Math.Round(pricelatest / _exchangeRate, 4));

                                            string country = ((string)(AllSecurities.Tables[0].Rows[rowIndex])["name"]);
                                            int v = -1;

                                            for (int i = 0; i < cmbCountry.Items.Count; i++)
                                            {
                                                string value = cmbCountry.Items[i].ToString();

                                                if (value == country)
                                                {
                                                    v = i;
                                                    _view.CountryName = country;


                                                    _view.CountryCode = Convert.ToInt32((AllSecurities.Tables[0].Rows[rowIndex])["country_code"]);

                                                }


                                            }



                                          //  cmbCountry.SelectedIndex = v;
                                            break;
                                        }
                                    }
                                }


                                if (defaultSecurityId != -1)
                                {

                                    SecurityId = defaultSecurityId;
                                    Symbol = defaultSymbol;
                                   // _view.Parameters.SecurityName = Symbol;

                                    // Get_ChartData();
                                    retval = true;
                                }
                                if (defaultSecurityId != -1)
                                {

                                    SecurityId = defaultSecurityId;
                                    Symbol = defaultSymbol;
                                   // _view.Parameters.SecurityName = Symbol;

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


            ValidateSecurity(Symbol);


        }
        private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


            ValidateSecurity(comboBoxEdit.Text);
        }



        # endregion Security

        # region HelperProcedures

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

                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }
        private void Get_From_info()
        {

            _view.AccountName = _view.Parameters.AccountName;
           

          


        }

        private void AssignXML()
        {





            XML = _view.Parameters.XML;


            if (File.Exists((subPath + "\\" + file)))
            {


                IpoData.RestoreLayoutFromXml(subPath + "\\" + file);


            }

          
        }

        public void SaveXML()
        {


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

        public void UpdateAccoutText()
        {


            _view.Parameters.AccountName = _view.AccountName;


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
               // IHierarchyViewerServiceContract dbService = this.CreateServiceClient();
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
                        _view.DBService.se_create_orders_ipo(
                            _security_id,
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

        public void se_get_ipo_data()
        {

            int cc = cmbTargetWeight.Text == "Global" ? -1 : _view.CountryCode;


            try
            {
                ThreadPool.QueueUserWorkItem(
              delegate(object eventArg)
              {
                  //decimal amount = Convert.ToDecimal(this.TXTHarvestamount.Value);
                 // IHierarchyViewerServiceContract DBService = this.CreateServiceClient();
                  ApplicationMessageList messages = null;


                  DataSet IPOData = new DataSet();




                  //IHierarchyViewerServiceContract dbService = this.CreateServiceClient();
                  IPOData = _view.DBService.se_get_ipo_data(
                    _view.AccountId,
                    cc,
                    _view.Weight,
                    _view.MidPrice,
                    _view.HighPrice,
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

        public void CountryList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmbCountry.Items.Clear();

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    //IHierarchyViewerServiceContract DBService = this.CreateServiceClient();




                    DataSet ds = _view.DBService.get_list_country();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["name"];
                            object county_code = row["country_code"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmbCountry.Items.Add(new IPO.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(county_code)));

                                // this.cmboHierarchy.SelectedIndex = 0;





                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }


                    //this.DataContext = _view;


                });
        }

        # endregion HelperProcedures



        # region Methods

      
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
            }
            Get_From_info();

            CountryList();

        }

        private void cmbTargetWeight_DropDownClosed(object sender, EventArgs e)
        {
            string targetweight = this.cmbTargetWeight.Text;
            string security = this.SecurityComboBoxEdit.Text;
            string LABEL = string.Format("Share Estimate at the {0} level for security {1}.", targetweight, security);





            this.lblSummary.Content = LABEL;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.SecurityComboBoxEdit.Text != null & this.cmbTargetWeight.Text != null)
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
            _view.Weight = Convert.ToDecimal(TXTWeight.Text);
        }

        private void txtpricelocalhigh_LostFocus(object sender, RoutedEventArgs e)
        {
            decimal high = Convert.ToDecimal(this.txtpricelocalhigh.Text);
            _view.HighPrice = high / _exchangeRate;

            this.txtpriceusdhigh.Text = Convert.ToString(_view.HighPrice);
        }

        private void txtpricelocalmid_LostFocus(object sender, RoutedEventArgs e)
        {
            decimal mid = Convert.ToDecimal(this.txtpricelocalmid.Text);
            _view.MidPrice = mid / _exchangeRate;
            this.txtpriceusdmid.Text = Convert.ToString(_view.MidPrice);
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
        #endregion Methods

    }
}
