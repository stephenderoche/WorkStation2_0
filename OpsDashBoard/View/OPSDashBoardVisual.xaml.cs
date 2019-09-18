namespace OPSDashBoard.Client.View
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using SalesSharedContracts;
    using System.Diagnostics;
    using System.Windows.Media.Animation;
    using System.Threading;
    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using DevExpress.Xpf.Grid;
  
    using DevExpress.Xpf.Bars;
 
    using DevExpress.Xpf.Editors;
  
  
    using OPSDashBoard.Client.ViewModel;
    using System.IO;
   
     using OPSDashBoard.Client;
    using System.Windows.Media.Imaging;


    public partial class OPSDashBoardVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "OpsDashBoard.xaml";
         string file1 = "OpsDetailDashBoard.xaml";
         string file2 = "OpsSecurityDashBoard.xaml";
         private const string MSGBOX_TITLE_ERROR = "OPSDashBoard";
        
         public OPSDashBoardModel _view;


         # region Parameters

         decimal _pc;
        public decimal PC
         {
             set { _pc = value; }
             get { return _pc; }
         }
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

         private string _symbol = string.Empty;
         public string Symbol
         {
             get { return _symbol; }
             set { _symbol = value; }
         }

         private int _security_id = -1;
         public int SecurityId
         {
             get { return _security_id; }
             set { _security_id = value; }
         }

         private int _majorAssetCode = -1;
         public int MajorAssetCode
         {
             get { return _majorAssetCode; }
             set { _majorAssetCode = value; }
         }

         private string _majorAsset = string.Empty;
         public string MajorAsset
         {
             get { return _majorAsset; }
             set { _majorAsset = value; }
         }
         # endregion Parameters

         //# region Security

         //public bool ValidateSecurity(string ourTextBox)
         //{
         //    bool retval = false;

         //    if (ourTextBox == "All Securities")
         //    {
         //        SecurityId = -1;
         //        _view.Parameters.SecurityName = "All Securities";
         //         _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);
         //         return true;
         //    }

         //    if (!String.IsNullOrEmpty(ourTextBox) || ourTextBox != "All Securities")
         //    {
         //        try
         //        {
         //            ThreadPool.QueueUserWorkItem(
         //                delegate(object eventArg)
         //                {
         //                    int defaultSecurityId = -1;
         //                    string defaultSymbol = "";
         //                    try
         //                    {
         //                        if (AllSecurities.Tables.Count > 0)
         //                        {
         //                            for (int rowIndex = 0; rowIndex < AllSecurities.Tables[0].Rows.Count; ++rowIndex)
         //                            {
         //                                if (ourTextBox.ToUpper().CompareTo(((AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString().ToUpper()) == 0)
         //                                {
         //                                    defaultSymbol = ((AllSecurities.Tables[0].Rows[rowIndex])["symbol"]).ToString();
         //                                    defaultSecurityId = Convert.ToInt32((AllSecurities.Tables[0].Rows[rowIndex])["security_id"]);
         //                                    break;
         //                                }
         //                            }
         //                        }


         //                        if (defaultSecurityId != -1)
         //                        {

         //                            SecurityId = defaultSecurityId;
         //                            Symbol = defaultSymbol;
         //                            _view.Parameters.SecurityName = Symbol;

         //                            _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);
         //                            retval = true;
         //                        }
         //                        if (defaultSecurityId != -1)
         //                        {

         //                            SecurityId = defaultSecurityId;
         //                            Symbol = defaultSymbol;
         //                            _view.Parameters.SecurityName = Symbol;

         //                            _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);
         //                            retval = true;
         //                        }

         //                        else
         //                        {
         //                            SecurityId = -1;
         //                            Symbol = "";
                                   

         //                        }
         //                    }
         //                    catch (Exception ex)
         //                    {
         //                        SecurityId = -1;
         //                        Symbol = "";
         //                        throw ex;
         //                    }

         //                    Dispatcher.BeginInvoke(new Action(() =>
         //                    {

         //                    }), System.Windows.Threading.DispatcherPriority.Normal);

         //                });
         //        }

         //        catch (Exception ex)
         //        {
         //            SecurityId = -1;
         //            Symbol = "";
         //            throw ex;
         //        }

         //        return retval;
         //    }

         //    else
         //    {
         //        SecurityId = -1;
         //        Symbol = "All Securities";
         //       _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);
         //    }

         //    return retval;
         //}
         //private void SecurityComboBoxEdit_EditValueChanged_1(object sender, EditValueChangedEventArgs e)
         //{
         //    string textEnteredPlusNew = "symbol Like '" + SecurityComboBoxEdit.Text + "%'";
         //    this.SecurityComboBoxEdit.Items.Clear();
         //    int count = -1;
         //    ThreadPool.QueueUserWorkItem(
         //        delegate(object eventArg)
         //        {
         //            var someObject = AllSecurities;
         //            if (someObject == null)
         //            {
         //                GetSecurities();
         //                return;
         //            }
         //            foreach (DataRow row in AllSecurities.Tables[0].Select(textEnteredPlusNew))
         //            {

         //                object item = row["symbol"];
         //                object security_id = row["security_id"];

         //                Dispatcher.BeginInvoke(new Action(() =>
         //                {
         //                    SecurityComboBoxEdit.Items.Add(new AccountItem(Convert.ToString(item), Convert.ToInt64(security_id)));



         //                }), System.Windows.Threading.DispatcherPriority.Normal);

         //                count = count + 1;
         //                if (count == 100)
         //                {
         //                    break;
         //                }
         //            }

         //        });


         //    ValidateSecurity(_view.Parameters.SecurityName);


         //}
         //private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
         //{
         //    ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


         //    ValidateSecurity(comboBoxEdit.Text);
         //}

         //# endregion Security

         public OPSDashBoardVisual(OPSDashBoardModel ViewerModel)
         {


             InitializeComponent();
             MajorAssetList();
             this.DataContext = ViewerModel;
             this._view = ViewerModel;
           
             this.txtenddate.EditValue = _view.Parameters.RunDate;
             this.chkUseCurrent.IsChecked = _view.Parameters.UseCurrent;

          
             DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;

             AssignXML();

         }

        # region HelperProcedures

         public void MajorAssetList()
         {

             //this.cmboMajorAsset.SelectedItem = -1;
             this.cmboMajorAsset.Items.Clear();

             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("All Asset Types", -1));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Cash", 0));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Equity", 1));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Debt", 3));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Fund", 4));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Future", 5));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("FX", 6));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Option", 7));
             cmboMajorAsset.Items.Add(new OPSDashBoard.Client.ComboBoxItem("Index", 9));

         }
     
        private void AssignXML()
        {


            if (File.Exists((subPath + "\\" + file)))
            {
                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);
            }

            if (File.Exists((subPath + "\\" + file1)))
            {
                _dataGridDetail.RestoreLayoutFromXml(subPath + "\\" + file1);
            }


            if (File.Exists((subPath + "\\" + file2)))
            {
                _dataGridSecurity.RestoreLayoutFromXml(subPath + "\\" + file2);
            }

          
        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridDetail.SaveLayoutToXml(subPath + "\\" + file1);
                _dataGridSecurity.SaveLayoutToXml(subPath + "\\" + file2);

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridDetail.SaveLayoutToXml(subPath + "\\" + file1);
                _dataGridSecurity.SaveLayoutToXml(subPath + "\\" + file2);
                

            }
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
     
       
        # endregion HelperProcedures



        # region Methods




        private void cmboDesk_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty((cmboMajorAsset.SelectedItem).ToString()))
            {
                MajorAsset = (cmboMajorAsset.SelectedItem).ToString();
                MajorAssetCode = Convert.ToInt32(((OPSDashBoard.Client.ComboBoxItem)cmboMajorAsset.SelectedItem).HiddenValue);
                _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);

            }
            else
            {
                _view.Parameters.MajorAsset = "All Asset Types";
                MajorAssetCode = -1;
                _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {


            if (this.loadPath.Text == string.Empty)
            {
                MessageBox.Show("Path to Load .bat is empty");
                return;
            }
           _view.runLoad(this.loadPath.Text);

           _view.UseCurrent = Convert.ToInt32(_view.Parameters.UseCurrent);
           _view.RunTime = _view.Parameters.RunDate;

        

           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            this.txtenddate.EditValue = _view.Parameters.RunDate;
            this.chkUseCurrent.IsChecked = _view.Parameters.UseCurrent;
            this.loadPath.Text = _view.Parameters.LoadPath;

            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
            }

            GetSecurities();
            

            _view.se_get_download_messages_history(_view.Parameters.RunDate, Convert.ToInt32(_view.Parameters.UseCurrent));
           
           

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
      

        private void start_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            _view.se_get_download_messages_history(_view.Parameters.RunDate, Convert.ToInt32(_view.Parameters.UseCurrent));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _view.se_get_download_messages_history(_view.Parameters.RunDate, Convert.ToInt32(_view.Parameters.UseCurrent));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _view.se_get_download_messages_history(_view.Parameters.RunDate, Convert.ToInt32(_view.Parameters.UseCurrent));
        }

        private void _dataGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string _message_time;
            string _mnemonic;



            int index =
                Convert.ToInt32(_dataGrid
                    .GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                GridColumn message_time = _dataGrid.Columns["message_time"];
                GridColumn mnemonic = _dataGrid.Columns["Load Name"];


                if (message_time != null)
                {
                    _message_time = _dataGrid.GetCellValue(index, message_time).ToString();

                    if (_message_time == string.Empty)
                    {
                        _view.DetailTable.Clear();
                    }
                    _mnemonic = _dataGrid.GetCellValue(index, mnemonic).ToString();



                    if (!string.IsNullOrEmpty(_message_time))
                    {
                        _view.se_get_download_messages_history_detail(_message_time, _mnemonic);


                    }
                }
                else
                {
                    _view.DetailTable.Clear();
                }
            }
        }

        private void loadPath_LostFocus(object sender, RoutedEventArgs e)
        {

            string close = "pack://application:,,,/OPSDashBoard.Client;component/Images/close.png";
            string ok = "pack://application:,,,/OPSDashBoard.Client;component/Images/ok.png";
            if (File.Exists((loadPath.Text)))
            {
                ImgFileExist.Source = new BitmapImage(new Uri(String.Format("{0}", ok), UriKind.RelativeOrAbsolute));
                ImgFileExist.ToolTip = "Load file Exists";
            }
            else
            {
                ImgFileExist.Source = new BitmapImage(new Uri(String.Format("{0}", close), UriKind.RelativeOrAbsolute));
                ImgFileExist.ToolTip = "Load file does not Exists";
            }
        }

        private void loadPath_Loaded(object sender, RoutedEventArgs e)
        {
            string close = "pack://application:,,,/OPSDashBoard.Client;component/Images/close.png";
            string ok = "pack://application:,,,/OPSDashBoard.Client;component/Images/ok.png";
            if (File.Exists((loadPath.Text)))
            {
                ImgFileExist.Source = new BitmapImage(new Uri(String.Format("{0}", ok), UriKind.RelativeOrAbsolute));
                ImgFileExist.ToolTip = "Load file Exists";
            }
            else
            {
                ImgFileExist.Source = new BitmapImage(new Uri(String.Format("{0}", close), UriKind.RelativeOrAbsolute));
                ImgFileExist.ToolTip = "Load file does not Exists";
            }
        }

        private void Button_log_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists((logPath.Text)))
            {
                Process.Start(logPath.Text);
            }
            else
            {
                MessageBox.Show("No Log File at selected location");
            }
        }


        #endregion Methods

      

      

        private void BtnBadPrices(object sender, RoutedEventArgs e)
        {
            _view.se_get_security_data(SecurityId, MajorAssetCode, true, -1, _view.Parameters.JustHoldings, -1);
        }

        private void BtnPriceChange(object sender, RoutedEventArgs e)
        {
           
            _view.se_get_security_data(SecurityId, MajorAssetCode, false, _view.Parameters.PriceChange, _view.Parameters.JustHoldings, -1);
        }

        private void BtnMain(object sender, RoutedEventArgs e)
        {
            _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, -1);
        }

        private void BtnStale(object sender, RoutedEventArgs e)
        {
            _view.se_get_security_data(SecurityId, MajorAssetCode, false, -1, _view.Parameters.JustHoldings, Convert.ToInt32(_view.Parameters.Stale));
           
        }


        private void PrintSecurity_Click(object sender, RoutedEventArgs e)
        {

       
                _viewDataGridSecurity.ShowPrintPreview(this);
         

        }

   

       

    
     
    }
}
