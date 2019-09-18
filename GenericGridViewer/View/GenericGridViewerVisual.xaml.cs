namespace GenericGrid.Client.View
{
    using System;
    using System.Linq;
    using System.Data;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
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
  
    using GenericGrid.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using GenericGrid.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
  


    public partial class GenericGridViewerVisual : UserControl
    {

       
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "NavDashboard.xaml";
         private string getsecurityTextinfo = "";
         private int getsecurityLenthinfo = 0;
         public GenericGridViewerModel _view;


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



         //private int _security_id = -1;
         //public int SecurityId
         //{
         //    get { return _security_id; }
         //    set { _security_id = value; }
         //}

       


         //private int _desId = -1;
         //public int DeskId
         //{
         //    get { return _desId; }
         //    set { _desId = value; }
         //}

         //private string _desk = string.Empty;
         //public string Desk
         //{
         //    get { return _desk; }
         //    set { _desk = value; }
         //}

         //private string _symbol = string.Empty;
         //public string Symbol
         //{
         //    get { return _symbol; }
         //    set { _symbol = value; }
         //}

         //private string _report = string.Empty;
         //public string Report
         //{
         //    get { return _report; }
         //    set { _report = value; }
         //}

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

         private bool _showblockId;
         public bool ShowBlockId
         {
             get { return _showblockId; }
             set { _showblockId = value; }
         }

         private int _blockId;
         public int BlockId
         {
             get { return _blockId; }
             set { _blockId = value; }

         }

         # endregion Parameters

         public GenericGridViewerVisual(GenericGridViewerModel genericGridViewerModel)
         {


             InitializeComponent();
             this.DataContext = genericGridViewerModel;
             this._view = genericGridViewerModel;
            
             GetSecurities();
             ReportList();
         
             this.comboBoxEdit1.Text = _view.AccountName;
             this.SecurityComboBoxEdit.Text = _view.Symbol;
          
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

                                   // Get_chart_data();
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

                                    _view.SecurityId = defaultSecurityId;
                                    _view.Symbol = defaultSymbol;
                                    _view.Parameters.SecurityName = _view.Symbol;

                                   // Get_ChartData();
                                    retval = true;
                                }
                                if (defaultSecurityId != -1)
                                {

                                    _view.SecurityId = defaultSecurityId;
                                    _view.Symbol = defaultSymbol;
                                    _view.Parameters.SecurityName = _view.Symbol;

                                   // Get_ChartData();
                                    retval = true;
                                }

                                else
                                {
                                    _view.SecurityId = -1;
                                    _view.Symbol = "";

                                }
                            }
                            catch (Exception ex)
                            {
                                _view.SecurityId = -1;
                                _view.Symbol = "";
                                throw ex;
                            }

                            Dispatcher.BeginInvoke(new Action(() =>
                            {

                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        });
                }

                catch (Exception ex)
                {
                    _view.SecurityId = -1;
                    _view.Symbol = "";
                    throw ex;
                }

                return retval;
            }

            else
            {
                _view.SecurityId = -1;
                _view.Symbol = "";
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
            _view.Symbol = String.IsNullOrEmpty(_view.Parameters.SecurityName) ? String.Empty : _view.Parameters.SecurityName;
            _view.Desk = String.IsNullOrEmpty(_view.Parameters.DeskName) ? String.Empty : _view.Parameters.DeskName;
            _view.Report = String.IsNullOrEmpty(_view.Parameters.Report) ? String.Empty : _view.Parameters.Report;
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


        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {


                    DataSet ds = _view.DBService.GetListAllDesks();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["desk_name"];
                            object desk_id = row["desk_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new GenericGrid.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

                                count = count + 1;

                                if (_view.Desk == Convert.ToString(item))
                                {
                                    this.cmboDesk.SelectedIndex = count;
                                    _view.DeskId = Convert.ToInt32(desk_id);
                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }


        public void ReportList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboReport.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    
                    ApplicationMessageList messages = null;

                    DataSet ds = _view.DBService.se_get_generic_param("ALL", out messages);
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["Report"];


                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboReport.Items.Add(new GenericGrid.Client.ComboBoxItem(Convert.ToString(item), 1));

                                count = count + 1;

                                if (_view.Report == Convert.ToString(item))
                                {
                                    this.cmboReport.SelectedIndex = count;

                                    SetReportParameter();

                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }

        public void SetReportParameter()
        {

            // this.cmboHierarchy.SelectedItem = -1;


            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    ApplicationMessageList messages = null;

                    DataSet ds = _view.DBService.se_get_generic_param(_view.Report, out messages);
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {


                            ShowAccount = Convert.ToBoolean(row["showAccount"]);
                            ShowDesk = Convert.ToBoolean(row["showDesk"]);
                            ShowSecurity = Convert.ToBoolean(row["showSecurity"]);
                            ShowStartDate = Convert.ToBoolean(row["showStartDate"]);
                            ShowEndDate = Convert.ToBoolean(row["showStartDate"]);
                            ShowBlockId = Convert.ToBoolean(row["showBlockID"]);



                            Dispatcher.BeginInvoke(new Action(() =>
                            {


                                comboBoxEdit1.IsEnabled = ShowAccount;
                                cmboDesk.IsEnabled = ShowDesk;
                                SecurityComboBoxEdit.IsEnabled = ShowSecurity;
                                txtstartdate.IsEnabled = ShowStartDate;
                                txtenddate.IsEnabled = ShowEndDate;
                                BlockID.IsEnabled = ShowBlockId;

                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
        }

        //public void Get_ChartData()
        //{

        //    //AssignXML();

        //    //this._dataGrid.ItemsSource = null;

        //    ThreadPool.QueueUserWorkItem(
        //        delegate(object eventArg)
        //        {

        //            ApplicationMessageList messages = null;
        //            DateTime nowDate = DateTime.Now;

        //            DataSet AccountTransaction = new DataSet();

        //            //Transaction
        //            AccountTransaction = _view.DBService.se_get_generic_view(
        //                      Convert.ToInt32(_view.AccountId),
        //                      DeskId,
        //                      SecurityId,
        //                      _view.Parameters.StartDate,
        //                      _view.Parameters.EndDate,
        //                      BlockId,
        //                      Report,
        //                      out messages);



        //            if (AccountTransaction.Tables.Count > 0)
        //            {

        //                Dispatcher.BeginInvoke(new Action(() =>
        //                {

        //                    source = new PagedCollectionView(GetEntities<DataTable>(AccountTransaction.Tables[0]));
        //                    source.PageSize = 150;

        //                    DataPager.PageSize = source.PageSize;
        //                    DataPager.Source = source;

        //                    this._dataGrid.ItemsSource = source;

                        


        //                }), System.Windows.Threading.DispatcherPriority.Normal);

        //            }
        //            else
        //                Dispatcher.BeginInvoke(new Action(() => { this._dataGrid.ItemsSource = null; }), System.Windows.Threading.DispatcherPriority.Normal);



        //        });
        //}


        //public IEnumerable<T> GetEntities<T>(DataTable dt)
        //{
        //    if (dt == null)
        //    {
        //        return null;
        //    }

        //    List<T> returnValue = new List<T>();
            
        //    List<string> typeProperties = new List<string>();

        //    T typeInstance = Activator.CreateInstance<T>();

        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        var prop = typeInstance.GetType().GetProperty(column.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        //        if (prop != null)
        //        {
        //            typeProperties.Add(column.ColumnName);
        //        }
        //    }

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        T entity = Activator.CreateInstance<T>();

        //        foreach (var propertyName in typeProperties)
        //        {

        //            if (row[propertyName] != DBNull.Value)
        //            {
        //                string str = row[propertyName].GetType().FullName;

        //                if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.String))
        //                {
        //                    object Val = row[propertyName].ToString();
        //                    entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
        //                }
        //                else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Guid))
        //                {
        //                    object Val = Guid.Parse(row[propertyName].ToString());
        //                    entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
        //                }
        //                else
        //                {
        //                    entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, row[propertyName], BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
        //                }
        //            }
        //            else
        //            {
        //                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, null, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
        //            }
        //        }

        //        returnValue.Add(entity);
        //    }

        //    return returnValue.AsEnumerable();
        //}

        private void AssignXML()
        {


            if (_dataGrid.Columns.Count > 1)
            {
                int maxColumnIndex = _dataGrid.Columns.Count - 6;
                int[] array = new int[maxColumnIndex];
                int a = 0;
                int b = 0;
                foreach (GridColumn gridColumn in _dataGrid.Columns)
                {
                    if (gridColumn.FieldName == "CustomColumn1" || gridColumn.FieldName == "CustomColumn2" ||
                        gridColumn.FieldName == "CustomColumn3" || gridColumn.FieldName == "CustomColumn4" ||
                        gridColumn.FieldName == "CustomColumn5" || gridColumn.FieldName == "CustomColumn6"
                        )
                    {

                    }
                    else
                    {

                        array[b] = a;
                        b++;
                    }

                    a++;

                }

                int count = 0;
                foreach (int i in array)
                {
                    _dataGrid.Columns.RemoveAt(i - count);
                    count++;
                }
            }

            file = _view.Report + "GenericGrid.xaml";


            if (File.Exists((subPath + "\\" + file)))
            {


                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);


            }

            SaveXML(_view.Report);
        }

        public void SaveXML(string report)
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGrid.SaveLayoutToXml(subPath + "\\" + report + "GenericGrid.xaml");

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + report + "GenericGrid.xaml");

            }
        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _view.AccountName;


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

            //RotateTransform rotateTransform1 = new RotateTransform(45, -50, 50);
            //_dataGrid.RenderTransform = rotateTransform1;

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
            DeskList();
           

            // _dataGrid.GroupSummary.Add(SummaryItemType.Sum, "balance_value").DisplayFormat = "Sum: {0:c2}";
           
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

        private void cmboDesk_SelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmboDesk.Items.Count == 0)
            {
                DeskList();
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboDesk.SelectedItem).ToString()))
                {
                    _view.Desk = (cmboDesk.SelectedItem).ToString();
                    _view.DeskId = Convert.ToInt32(((GenericGrid.Client.ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);

                }
            }
        }

        private void cmboReport_SelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmboReport.Items.Count == 0)
            {
                //ReportList();
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboReport.SelectedItem).ToString()))
                {



                    SaveXML(_view.Report);


                    _view.Report = (cmboReport.SelectedItem).ToString();

                   // setGridAngle();

                    SetReportParameter();

                    AssignXML();








                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Get_chart_data();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveXML(_view.Report);
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

        private void lblheader_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Get_chart_data();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Get_chart_data();
        }

      
        private void Get_chart_data()
        {
            _view.Get_ChartData();

            DataPager.PageSize = _view.source.PageSize;
            DataPager.Source = _view.source;
            DataContext = _view.source;
           // _view.PositionCurve = _view.source;
        }
       

      

        



    

    }
}
