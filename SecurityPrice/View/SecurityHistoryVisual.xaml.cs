namespace SecurityHistory.Client.View
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
   using SecurityHistory.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using SecurityHistory.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Core.Serialization;
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
  


    public partial class SecurityHistoryVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "BlotterOrders.xaml";
        string file2 = "BrokerExecutions.xaml";
         # region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public SecurityHistoryViewerModel _vm;

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

         public SecurityHistoryVisual(SecurityHistoryViewerModel securityHistoryViewerModel)
         {


             InitializeComponent();

             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
             GetSecurities();
         
           
             this.SecurityComboBoxEdit.Text = Symbol;
 
            
           
             SerializeHelperBase f;

             AssignXML();


         }

        # region Security

        public void GetSecurities()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                   
                    AllSecurities = _vm.DBService.GetSecurityInfo("");

                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }), System.Windows.Threading.DispatcherPriority.Normal);

                });
        }
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
                                    _vm.Parameters.SecurityName = Symbol;
                                    _vm.GetChecks();
                                 

                                    //GetChecks();
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


        

        }
        private void SecurityComboBoxEdit_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;


            ValidateSecurity(comboBoxEdit.Text);




        }

      

        # endregion Security

        # region HelperProcedures

        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {


                    DataSet ds = _vm.DBService.GetListAllDesks();
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["desk_name"];
                            object desk_id = row["desk_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new SecurityHistory.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

                                count = count + 1;

                                if (_vm.Parameters.DeskName == Convert.ToString(item))
                                {
                                    this.cmboDesk.SelectedIndex = count;
                                    DeskId = Convert.ToInt32(desk_id);
                                }



                            }), System.Windows.Threading.DispatcherPriority.Normal);

                        }
                    }




                });
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

            Symbol = String.IsNullOrEmpty(_vm.Parameters.SecurityName) ? String.Empty : _vm.Parameters.SecurityName;

        }

      

        private void AssignXML()
        {
            if (File.Exists((subPath + "\\" + file)))
            {
                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);
            }

        if (File.Exists((subPath + "\\" + file2)))
        {
            _dataGridBroker.RestoreLayoutFromXml(subPath + "\\" + file2);
        }


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridBroker.SaveLayoutToXml(subPath + "\\" + file2);

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridBroker.SaveLayoutToXml(subPath + "\\" + file2);
            }
        }

        # endregion HelperProcedures



        # region Methods

        private void _dataGridModel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index =
                Convert.ToInt32(_dataGrid
                    .GetRowVisibleIndexByHandle(viewblotterGrid.FocusedRowData.RowHandle.Value).ToString());
            if (index > -1)
            {
                GridColumn block_id = _dataGrid.Columns["block_id"];
                GridColumn symbol = _dataGrid.Columns["symbol"];
                GridColumn security_id = _dataGrid.Columns["security_id"];
                if (block_id != null)
                {
                    BlockId = Convert.ToInt32(_dataGrid.GetCellValue(index, block_id).ToString());
                    SecurityId = Convert.ToInt32(_dataGrid.GetCellValue(index, security_id).ToString());
                    Symbol = _dataGrid.GetCellValue(index, symbol).ToString();

                    _vm.Get_BrokerData(BlockId);
                    _vm.Parameters.SecurityName = Symbol;
                    _vm.GetChecks();
                }
            }


        }

      

       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Get_From_info();
            DeskList();
            _vm.Get_BrokerData(BlockId);
            //DXSerializer.AddEndDeserializingHandler(_dataGrid, new EndDeserializingEventHandler(OnEndDeserializing));
            //DXSerializer.AddStartSerializingHandler(_dataGrid, new RoutedEventHandler(OnStartSerializing));

            
        }

        void column_CustomGetSerializableProperties(object sender, CustomGetSerializablePropertiesEventArgs e)
        {
            e.SetPropertySerializable(GridColumn.EditSettingsProperty, new DXSerializable());

        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           _vm.GetChecks();
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
           _vm.GetChecks();
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
                    Desk = (cmboDesk.SelectedItem).ToString();
                    DeskId = Convert.ToInt32(((SecurityHistory.Client.ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);
                    _vm.Parameters.DeskName = Desk;
                    _vm.Get_DeskData(DeskId);

                }
            }
        }

        void OnShowGridMenu(object sender, GridMenuEventArgs e)
        {

            //bloomberg
            if (e.MenuType == GridMenuType.Column)
            {
                // e.Customizations.Add(new BarButtonItem { Content = "Add BBG Data Point" });

                BarButtonItem item = new BarButtonItem();
                item.Content = "Add Custom Data Point";
                item.ItemClick += OnItemClickBBG;

                e.Customizations.Add(item);
            }

            //Remove
            if (e.MenuType == GridMenuType.Column)
            {
                // e.Customizations.Add(new BarButtonItem { Content = "Add BBG Data Point" });

                GridColumnHeader columnHeader = e.TargetElement as GridColumnHeader;
                GridColumn column = columnHeader.DataContext as GridColumn;

                BarButtonItem item = new BarButtonItem();
                item.Content = "Remove Column";
                GridColumnMenuInfo menuInfo = (GridColumnMenuInfo)e.MenuInfo;
                GridColumn columnHeaderindex = menuInfo.Column;
                // MessageBox.Show(Convert.ToString(GridControl.Columns[column.FieldName].VisibleIndex));
                ColIndex = _dataGrid.Columns[columnHeaderindex.FieldName].VisibleIndex;
                Header = _dataGrid.Columns[columnHeaderindex.FieldName].FieldName;
                item.ItemClick += OnItemClickRemove;

                e.Customizations.Add(item);

            }
            
            
            
            
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

        private void OnItemClickBBG(object sender, ItemClickEventArgs e)
        {

            Column a = new Column(this);
            a.Show();

      



        }

        private void OnItemClickRemove(object sender, ItemClickEventArgs e)
        {



            if (ColIndex != -1)
                if (Header.Substring(0, 2) == "CC")
                    _dataGrid.Columns.RemoveAt(ColIndex);

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
            //if (e.Column.FieldName == "col2")
            //{
            //    var te = new TextEditSettings();
            //    te.DisplayFormat = "n2";
            //    e.Column.EditSettings = te;
            //}

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

       public void serialize()
        {
            DXSerializer.AddEndDeserializingHandler(_dataGrid, new EndDeserializingEventHandler(OnEndDeserializing));
            DXSerializer.AddStartSerializingHandler(_dataGrid, new RoutedEventHandler(OnStartSerializing));
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _vm.GetChecks();
        }

        void OnEndDeserializing(object sender, EndDeserializingEventArgs e)
        {
            foreach (GridColumn col in _dataGrid.Columns)
            {
                MyGridColumn myCol = col as MyGridColumn;
                if (myCol != null)
                {
                    TextEditSettings colEdSettings = myCol.EditSettings as TextEditSettings;
                    if (colEdSettings != null)
                    {
                        colEdSettings.MaskType = myCol.SettingsMaskType;
                        colEdSettings.Mask = myCol.SettingsMask;
                        colEdSettings.DisplayFormat = myCol.SettingsDisplayFormatString;
                    }
                }
            }
        }
        void OnStartSerializing(object sender, RoutedEventArgs e)
        {
            foreach (GridColumn col in _dataGrid.Columns)
            {
                MyGridColumn myCol = col as MyGridColumn;
                if (myCol != null)
                {
                    TextEditSettings colEdSettings = myCol.EditSettings as TextEditSettings;
                    if (colEdSettings != null)
                    {
                        myCol.SettingsMaskType = colEdSettings.MaskType;
                        myCol.SettingsMask = colEdSettings.Mask;
                        myCol.SettingsDisplayFormatString = colEdSettings.DisplayFormat;
                    }
                }

     


    }

     
}



    }
    public class MyGridColumn : GridColumn
    {


        [XtraSerializableProperty]
        public MaskType SettingsMaskType
        {
            get;
            set;
        }

        [XtraSerializableProperty]
        public string SettingsMask
        {
            get;
            set;
        }
        [XtraSerializableProperty]
        public string SettingsDisplayFormatString
        {
            get;
            set;
        }
    }



}
