namespace NavValidationViewer.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using HierarchyViewerAddIn.Shared.ServiceContracts;
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
  
    using NavValidationViewer.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using NavValidationViewer.Client;
  


    public partial class NavValidationViewerVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "ValidationView.xaml";
         string fileSummary = "ValidationViewSummary.xaml";
         string fileDataQuality = "ValidationViewDataQuality.xaml";
         string fileDetail = "ValidationViewDetail.xaml";
         string fileEffect = "ValidationViewEffect.xaml";
         string fileClass = "ValidationViewClass.xaml";
         private const string MSGBOX_TITLE_ERROR = "Nav Validation";
        
         public NavValidationViewerModel _view;


         # region Parameters

         DateTime _startDate;
         public DateTime StartDate
         {
             set { _startDate = value; }
             get { return _startDate; }
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

       

         
         private string _selectAccountName = string.Empty;
         public string SelectAccountName
         {
             set { _selectAccountName = value; }
             get { return _selectAccountName; }
         }


         private bool _focusFail;
         public bool FocusFail
         {
             get { return _focusFail; }
             set { _focusFail = value; }
         }

         private bool _focusData;
         public bool FocusData
         {
             get { return _focusData; }
             set { _focusData = value; }
         }

         private int _contoltypeId = -1;
         public int ControlTypeId
         {
             get { return _contoltypeId; }
             set { _contoltypeId = value; }
         }

         private string _conttolType = string.Empty;
         public string ControlType
         {
             get { return _conttolType; }
             set { _conttolType = value; }
         }

         private int _loadhist_id = -1;
         public int Loadhist_id
         {
             get { return _loadhist_id; }
             set { _loadhist_id = value; }
         }

         private string _loadHist = string.Empty;
         public string LoadHist
         {
             get { return _loadHist; }
             set { _loadHist = value; }
         }

         private int _nav_res_rule_result_id = -1;
         public int Nav_res_rule_result_id
         {
             get { return _nav_res_rule_result_id; }
             set { _nav_res_rule_result_id = value; }
         }


         string _xml;
         public string XML
         {
             set { _xml = value; }
             get { return _xml; }
         }

         # endregion Parameters

         public NavValidationViewerVisual(NavValidationViewerModel genericGridViewerModel)
         {


             InitializeComponent();
             this.DataContext = genericGridViewerModel;
             this._view = genericGridViewerModel;
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

                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), StartDate, ControlTypeId, Loadhist_id,
                                        _view.Parameters.FocusFail, _view.Parameters.FocusData);
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

        void getDates()
        {


            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(_view.Parameters.StartDate.ToString(), out intStr);
            if (intResultTryParse == true)
            {
                StartDate = (intStr);
                _view.Parameters.StartDate = (intStr);
            }
            else
            {
                StartDate = DateTime.Today;
                _view.Parameters.StartDate = (intStr);
            }


        }

        public void ControlTypeList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboControlType.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {





                    DataSet ds = _view.DBService.get_nav_control_types();
                    DataSet dsSector = new DataSet();





                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["name"];
                            object desk_id = row["nav_control_type_code"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {

                                if (count == -1)
                                {
                                    cmboControlType.Items.Add(new NavValidationViewer.Client.ComboBoxItem(Convert.ToString("All Controls"), Convert.ToInt64(-1)));
                                    count = count + 1;

                                    if (ControlType == "All Controls")
                                    {
                                        this.cmboControlType.SelectedIndex = count;
                                        ControlTypeId = -1;

                                    }



                                }
                                cmboControlType.Items.Add(new NavValidationViewer.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

                                count = count + 1;

                                if (ControlType == Convert.ToString(item))
                                {
                                    this.cmboControlType.SelectedIndex = count;
                                    ControlTypeId = Convert.ToInt32(desk_id);


                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }


        public void LoadHistList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboIntradayCode.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {


                    DataSet ds = _view.DBService.se_get_loadhist();
                    DataSet dsSector = new DataSet();

                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["mnemonic"];
                            object loadhist_definition_id = row["loadhist_definition_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboIntradayCode.Items.Add(new NavValidationViewer.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(loadhist_definition_id)));

                                count = count + 1;

                                if (LoadHist == Convert.ToString(item))
                                {
                                    this.cmboIntradayCode.SelectedIndex = count;
                                    Loadhist_id = Convert.ToInt32(loadhist_definition_id);

                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }

        private void Get_From_info()
        {

            _view.AccountName = _view.Parameters.AccountName;
           
            ControlType = String.IsNullOrEmpty(_view.Parameters.ControlType) ? String.Empty : _view.Parameters.ControlType;
            LoadHist = String.IsNullOrEmpty(_view.Parameters.LoadHist) ? String.Empty : _view.Parameters.LoadHist;
            StartDate = _view.Parameters.StartDate;

            ControlTypeList();
            LoadHistList();
            getDates();

         


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

                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }

        public void SaveXML()
        {


            if (isNull(XML))
            {
                XML = file;
            }

            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));


                _dataGrid.SaveLayoutToXml(subPath + "\\" + XML);
                SummaryGrid.SaveLayoutToXml(subPath + "\\" + fileSummary);
                this.DataQualityGrid.SaveLayoutToXml(subPath + "\\" + fileDataQuality);
                this.DetailGrid.SaveLayoutToXml(subPath + "\\" + fileDetail);
                this.effectGrid.SaveLayoutToXml(subPath + "\\" + fileEffect);
                this.ClassGrid.SaveLayoutToXml(subPath + "\\" + fileClass);
            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + XML);
                SummaryGrid.SaveLayoutToXml(subPath + "\\" + fileSummary);
                this.DataQualityGrid.SaveLayoutToXml(subPath + "\\" + fileDataQuality);
                this.DetailGrid.SaveLayoutToXml(subPath + "\\" + fileDetail);
                this.effectGrid.SaveLayoutToXml(subPath + "\\" + fileEffect);
                this.ClassGrid.SaveLayoutToXml(subPath + "\\" + fileClass);
            }
        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _view.AccountName;


        }

      

       
        # endregion HelperProcedures



        # region Methods


        private void Btn_Ruleset_Click(object sender, RoutedEventArgs e)
        {


            int count = 0;
            // int selectedrow = 0;

            string ruleset_first_rev_comment;
            //  int    rule_second_rev_comment;
            // string  ruleset_first_rev_comment;
            //   int ruleset_second_rev_comment;
            int comment_id;
            int nav_res_ruleset_review;


            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {

               

                GridColumn rule_first_rev_comment_id = _dataGrid.Columns["rule_first_rev_comment_id"];
                GridColumn rule_second_rev_comment_id = _dataGrid.Columns["rule_second_rev_comment_id"];
                GridColumn ruleset_first_rev_comment_id = _dataGrid.Columns["ruleset_first_rev_comment_id"];
                GridColumn ruleset_second_rev_comment_id = _dataGrid.Columns["ruleset_second_rev_comment_id"];
                GridColumn nav_res_ruleset_review_id = _dataGrid.Columns["nav_res_ruleset_review_id"];



                if (rule_first_rev_comment_id != null)
                {

                    ruleset_first_rev_comment = _dataGrid.GetCellValue(listRowList[i], ruleset_first_rev_comment_id).ToString();
                    nav_res_ruleset_review = Convert.ToInt32(_dataGrid.GetCellValue(listRowList[i], nav_res_ruleset_review_id).ToString());



                    if (string.IsNullOrEmpty(ruleset_first_rev_comment))
                    {
                        comment_id = -1;
                       
                       
                        _view.se_update_nav_ruleset_comments(comment_id, 1, nav_res_ruleset_review, this.rdochecked.IsChecked.Value, this.txtRulesetComment.Text);

                    }
                    else
                    {
                        

                        

                        _view.se_update_nav_ruleset_comments(Convert.ToInt32(ruleset_first_rev_comment), 1, nav_res_ruleset_review, this.rdochecked.IsChecked.Value, this.txtRulesetComment.Text);
                    }


                }

                count = count + 1;

            }

            Thread.Sleep(1000);
            string LABEL = string.Format("{0} Rule Set Comment(s) Updated.", count);
            System.Windows.MessageBox.Show(LABEL, "Comment", MessageBoxButton.OK, MessageBoxImage.Information);

            //upd



            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), StartDate, ControlTypeId, Loadhist_id, _view.Parameters.FocusFail, _view.Parameters.FocusData);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            // int selectedrow = 0;

            string rule_first_rev_comment;
            //  int    rule_second_rev_comment;
            // string  ruleset_first_rev_comment;
            //   int ruleset_second_rev_comment;
            int comment_id;
            int nav_res_rule_result;


            int i = 0;
            int[] listRowList = this._dataGrid.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {

                

                GridColumn rule_first_rev_comment_id = _dataGrid.Columns["rule_first_rev_comment_id"];
                GridColumn rule_second_rev_comment_id = _dataGrid.Columns["rule_second_rev_comment_id"];
                GridColumn ruleset_first_rev_comment_id = _dataGrid.Columns["ruleset_first_rev_comment_id"];
                GridColumn ruleset_second_rev_comment_id = _dataGrid.Columns["ruleset_second_rev_comment_id"];
                GridColumn nav_res_rule_result_id = _dataGrid.Columns["nav_res_rule_result_id"];



                if (rule_first_rev_comment_id != null)
                {

                    rule_first_rev_comment = _dataGrid.GetCellValue(listRowList[i], rule_first_rev_comment_id).ToString();
                    nav_res_rule_result = Convert.ToInt32(_dataGrid.GetCellValue(listRowList[i], nav_res_rule_result_id).ToString());

                    if (string.IsNullOrEmpty(rule_first_rev_comment))
                    {
                        comment_id = -1;

                        _view.se_update_nav_comments(comment_id, 1, nav_res_rule_result, txtRuleComment.Text);
                    }
                    else
                    {

                        _view.se_update_nav_comments(Convert.ToInt32(rule_first_rev_comment), 1, nav_res_rule_result, txtRuleComment.Text);
                    }

                }

                count = count + 1;

            }

            string LABEL = string.Format("{0} Rule Comment(s) added.", count);
            System.Windows.MessageBox.Show(LABEL, "Comment", MessageBoxButton.OK, MessageBoxImage.Information);

            //upd

            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), StartDate, ControlTypeId, Loadhist_id, _view.Parameters.FocusFail, _view.Parameters.FocusData);


        }
        private void cmboDesk_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmboControlType.Items.Count == 0)
            {
                ControlTypeList();
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboControlType.SelectedItem).ToString()))
                {
                    ControlType = (cmboControlType.SelectedItem).ToString();
                    ControlTypeId = Convert.ToInt32(((NavValidationViewer.Client.ComboBoxItem)cmboControlType.SelectedItem).HiddenValue);

                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), StartDate, ControlTypeId, Loadhist_id, _view.Parameters.FocusFail, _view.Parameters.FocusData);
        }

        private void cmboIntradayCode_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmboIntradayCode.Items.Count == 0)
            {
                ControlTypeList();
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboIntradayCode.SelectedItem).ToString()))
                {
                    LoadHist = (cmboIntradayCode.SelectedItem).ToString();
                    Loadhist_id = Convert.ToInt32(((NavValidationViewer.Client.ComboBoxItem)cmboIntradayCode.SelectedItem).HiddenValue);

                }
            }
        }

        private void end_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDates();

        }

        private void getComments(object sender, RoutedEventArgs e)
        {
            string rule_first_rev_comment;
            //  int    rule_second_rev_comment;
            string ruleset_first_rev_comment;
            //   int ruleset_second_rev_comment;
            string nav_res_rule_result;

            int index =
              Convert.ToInt32(_dataGrid.GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());
            


            if (index > -1)
            {

                ApplicationMessageList messages = null;
                

                GridColumn rule_first_rev_comment_id = _dataGrid.Columns["rule_first_rev_comment_id"];
                GridColumn rule_second_rev_comment_id = _dataGrid.Columns["rule_second_rev_comment_id"];
                GridColumn ruleset_first_rev_comment_id = _dataGrid.Columns["ruleset_first_rev_comment_id"];
                GridColumn ruleset_second_rev_comment_id = _dataGrid.Columns["ruleset_second_rev_comment_id"];
                GridColumn nav_res_rule_result_id = _dataGrid.Columns["nav_res_rule_result_id"];


                if (rule_first_rev_comment_id != null)
                {


                    rule_first_rev_comment = _dataGrid.GetCellValue(index, rule_first_rev_comment_id).ToString();

                    if (!string.IsNullOrEmpty(rule_first_rev_comment))
                    {

                        string results = _view.DBService.se_get_nav_comments(Convert.ToInt32(rule_first_rev_comment), 1, out messages);
                        this.txtRuleComment.Text = results;
                    }
                    else
                    {
                        this.txtRuleComment.Text = string.Empty;
                    }

                }



                if (ruleset_first_rev_comment_id != null)
                {


                    ruleset_first_rev_comment = _dataGrid.GetCellValue(index, ruleset_first_rev_comment_id).ToString();

                    if (!string.IsNullOrEmpty(ruleset_first_rev_comment))
                    {

                        string results = _view.DBService.se_get_nav_comments(Convert.ToInt32(ruleset_first_rev_comment), 1, out messages);
                        this.txtRulesetComment.Text = results;
                    }
                    else
                    {
                        this.txtRulesetComment.Text = string.Empty;
                    }

                }
                //Rule detail

                if (nav_res_rule_result_id != null)
                {


                    nav_res_rule_result = _dataGrid.GetCellValue(index, nav_res_rule_result_id).ToString();

                    if (!string.IsNullOrEmpty(nav_res_rule_result))
                    {

                        Nav_res_rule_result_id = Convert.ToInt32(nav_res_rule_result);
                        Get_RuleDetail();

                    }


                }



            }
        }

        public void Get_RuleDetail()
        {

           

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    
                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet SG = new DataSet();
                    DataSet DQG = new DataSet();
                    DataSet DG = new DataSet();
                    DataSet eG = new DataSet();
                    DataSet SCG = new DataSet();

                    # region Summary Grid
                    //Summary grid
                    SG = _view.DBService.nq_get_nav_linked_reports(
                        Nav_res_rule_result_id,
                           1,
                              out messages);

                    if (SG.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {

                            this.SummaryGrid.ItemsSource = SG.Tables[0];
                            //this.txtstartdate.Text = Convert.ToString(StartDate);
                            //this.txtenddate.Text = Convert.ToString(EndDate);



                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.SummaryGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);
                    # endregion Summary Grid

                    #region Data Quality Grid
                    //Quality grid
                    DQG = _view.DBService.nq_get_nav_linked_reports(
                       Nav_res_rule_result_id,
                          2,
                             out messages);

                    if (DQG.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.DataQualityGrid.ItemsSource = DQG.Tables[0];

                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.DataQualityGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);

                    #endregion Data Quality Grid

                    #region Data Grid
                    //Data grid
                    DG = _view.DBService.nq_get_nav_linked_reports(
                       Nav_res_rule_result_id,
                          3,
                             out messages);

                    if (DG.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.DetailGrid.ItemsSource = DG.Tables[0];

                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.DetailGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);
                    #endregion Data Grid

                    #region effect Grid
                    //Data grid
                    eG = _view.DBService.nq_get_nav_linked_reports(
                       Nav_res_rule_result_id,
                          4,
                             out messages);

                    if (DG.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.effectGrid.ItemsSource = eG.Tables[0];

                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.effectGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);
                    #endregion Data Grid

                    #region class Grid
                    //Data grid
                    SCG = _view.DBService.nq_get_nav_linked_reports(
                       Nav_res_rule_result_id,
                          5,
                             out messages);

                    if (DG.Tables.Count > 0)
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.ClassGrid.ItemsSource = SCG.Tables[0];

                        }), System.Windows.Threading.DispatcherPriority.Normal);

                    }
                    else
                        Dispatcher.BeginInvoke(new Action(() => { this.ClassGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);
                    #endregion Data Grid

                });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            if (isNull(XML))
            {
                XML = file;
            }

            XML = _view.Parameters.XML;

            if (File.Exists((subPath + "\\" + XML)))
            {
                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + XML);
            }

            if (File.Exists((subPath + "\\" + fileSummary)))
            {
                this.SummaryGrid.RestoreLayoutFromXml(subPath + "\\" + fileSummary);
            }

            if (File.Exists((subPath + "\\" + fileDataQuality)))
            {
                this.DataQualityGrid.RestoreLayoutFromXml(subPath + "\\" + fileDataQuality);
            }

            if (File.Exists((subPath + "\\" + fileDetail)))
            {
                this.DetailGrid.RestoreLayoutFromXml(subPath + "\\" + fileDetail);
            }

            if (File.Exists((subPath + "\\" + fileClass)))
            {
                this.ClassGrid.RestoreLayoutFromXml(subPath + "\\" + fileClass);
            }


            if (File.Exists((subPath + "\\" + fileEffect)))
            {
                this.effectGrid.RestoreLayoutFromXml(subPath + "\\" + fileEffect);
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

        private void _dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //string rule_first_rev_comment;
            ////  int    rule_second_rev_comment;
            //string ruleset_first_rev_comment;
            ////   int ruleset_second_rev_comment;
            //string nav_res_rule_result;

            //int index =
            //  Convert.ToInt32(_dataGrid.GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());



            //if (index > -1)
            //{

              

            //    GridColumn rule_first_rev_comment_id = _dataGrid.Columns["rule_first_rev_comment_id"];
            //    GridColumn rule_second_rev_comment_id = _dataGrid.Columns["rule_second_rev_comment_id"];
            //    GridColumn ruleset_first_rev_comment_id = _dataGrid.Columns["ruleset_first_rev_comment_id"];
            //    GridColumn ruleset_second_rev_comment_id = _dataGrid.Columns["ruleset_second_rev_comment_id"];
            //    GridColumn nav_res_rule_result_id = _dataGrid.Columns["nav_res_rule_result_id"];


            //    if (rule_first_rev_comment_id != null)
            //    {


            //        rule_first_rev_comment = _dataGrid.GetCellValue(index, rule_first_rev_comment_id).ToString();

            //        if (!string.IsNullOrEmpty(rule_first_rev_comment))
            //        {

                        
            //            _view.se_get_nav_comments(Convert.ToInt32(rule_first_rev_comment), 1);
            //            this.txtRuleComment.Text = _view.Results;
            //        }
            //        else
            //        {
            //            this.txtRuleComment.Text = string.Empty;
            //        }

            //    }



            //    if (ruleset_first_rev_comment_id != null)
            //    {


            //        ruleset_first_rev_comment = _dataGrid.GetCellValue(index, ruleset_first_rev_comment_id).ToString();

            //        if (!string.IsNullOrEmpty(ruleset_first_rev_comment))
            //        {

            //            //string results = dbService.se_get_nav_comments(Convert.ToInt32(ruleset_first_rev_comment), 1, out messages);
            //            _view.se_get_nav_ruleset_comments(Convert.ToInt32(ruleset_first_rev_comment), 1);
            //            this.txtRulesetComment.Text = _view.RuleSetResults;
            //        }
            //        else
            //        {
            //            this.txtRulesetComment.Text = string.Empty;
            //        }

            //    }
            //    //Rule detail

            //    if (nav_res_rule_result_id != null)
            //    {


            //        nav_res_rule_result = _dataGrid.GetCellValue(index, nav_res_rule_result_id).ToString();

            //        if (!string.IsNullOrEmpty(nav_res_rule_result))
            //        {

            //            Nav_res_rule_result_id = Convert.ToInt32(nav_res_rule_result);
            //            Get_RuleDetail();

            //        }


            //    }



            //}
        }

        private void _dataGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string rule_first_rev_comment;
            //  int    rule_second_rev_comment;
            string ruleset_first_rev_comment;
            //   int ruleset_second_rev_comment;
            string nav_res_rule_result;

            int index =
              Convert.ToInt32(_dataGrid.GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn rule_first_rev_comment_id = _dataGrid.Columns["rule_first_rev_comment_id"];
                GridColumn rule_second_rev_comment_id = _dataGrid.Columns["rule_second_rev_comment_id"];
                GridColumn ruleset_first_rev_comment_id = _dataGrid.Columns["ruleset_first_rev_comment_id"];
                GridColumn ruleset_second_rev_comment_id = _dataGrid.Columns["ruleset_second_rev_comment_id"];
                GridColumn nav_res_rule_result_id = _dataGrid.Columns["nav_res_rule_result_id"];


                if (rule_first_rev_comment_id != null)
                {


                    rule_first_rev_comment = _dataGrid.GetCellValue(index, rule_first_rev_comment_id).ToString();

                    if (!string.IsNullOrEmpty(rule_first_rev_comment))
                    {


                        _view.se_get_nav_comments(Convert.ToInt32(rule_first_rev_comment), 1);
                        this.txtRuleComment.Text = _view.Results;
                    }
                    else
                    {
                        this.txtRuleComment.Text = string.Empty;
                    }

                }



                if (ruleset_first_rev_comment_id != null)
                {


                    ruleset_first_rev_comment = _dataGrid.GetCellValue(index, ruleset_first_rev_comment_id).ToString();

                    if (!string.IsNullOrEmpty(ruleset_first_rev_comment))
                    {

                        //string results = dbService.se_get_nav_comments(Convert.ToInt32(ruleset_first_rev_comment), 1, out messages);
                        _view.se_get_nav_ruleset_comments(Convert.ToInt32(ruleset_first_rev_comment), 1);
                        this.txtRulesetComment.Text = _view.RuleSetResults;
                    }
                    else
                    {
                        this.txtRulesetComment.Text = string.Empty;
                    }

                }
                //Rule detail

                if (nav_res_rule_result_id != null)
                {


                    nav_res_rule_result = _dataGrid.GetCellValue(index, nav_res_rule_result_id).ToString();

                    if (!string.IsNullOrEmpty(nav_res_rule_result))
                    {

                        Nav_res_rule_result_id = Convert.ToInt32(nav_res_rule_result);
                        Get_RuleDetail();

                    }


                }



            }
        }

        
      

        

    

      

       

      

        



    

    }
}
