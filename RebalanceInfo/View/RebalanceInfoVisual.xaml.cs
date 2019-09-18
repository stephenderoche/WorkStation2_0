namespace RebalanceInfo.Client.View
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
   using RebalanceInfo.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using RebalanceInfo.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
   
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
  


    public partial class RebalanceInfoVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "Rebal_account.xaml";
        string file2 = "Rebal_security.xaml";
        string file3 = "Rebal_session.xaml";
        string file4 = "Rebal_exclusions.xaml";
         # region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public RebalanceInfoViewerModel _vm;

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

         public RebalanceInfoVisual(RebalanceInfoViewerModel securityHistoryViewerModel)
         {


             InitializeComponent();

             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
   
           
             SerializeHelperBase f;

             AssignXML();


         }

    

        # region HelperProcedures

        public void DeskList()
        {

            // this.cmboHierarchy.SelectedItem = -1;
            this.cmboDesk.Items.Clear();
            if(cmboDesk.Items.Count != 0)
                this.cmboDesk.Items.Clear();
            int count = -1;
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    ApplicationMessageList messages = null;

                    DataSet ds = _vm.DBService.se_rebal_sessions(-1,out messages);
                    DataSet dsSector = new DataSet();



                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {

                            object item = row["session"];
                            object desk_id = row["rebal_session_id"];

                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                cmboDesk.Items.Add(new RebalanceInfo.Client.ComboBoxItem(Convert.ToString(item), Convert.ToInt64(desk_id)));

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
                _dataGridLink.RestoreLayoutFromXml(subPath + "\\" + file);
            }

        if (File.Exists((subPath + "\\" + file2)))
        {
            _dataGridsecurity.RestoreLayoutFromXml(subPath + "\\" + file2);
        }

        if (File.Exists((subPath + "\\" + file3)))
        {
            _dataSession.RestoreLayoutFromXml(subPath + "\\" + file3);
        }

        if (File.Exists((subPath + "\\" + file4)))
        {
            _dataGridExclusion.RestoreLayoutFromXml(subPath + "\\" + file4);
           
        }


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGridLink.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridsecurity.SaveLayoutToXml(subPath + "\\" + file2);
                _dataSession.SaveLayoutToXml(subPath + "\\" + file3);
                _dataGridExclusion.SaveLayoutToXml(subPath + "\\" + file4);

            }
            else
            {
                _dataGridLink.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridsecurity.SaveLayoutToXml(subPath + "\\" + file2);
                _dataSession.SaveLayoutToXml(subPath + "\\" + file3);
                _dataGridExclusion.SaveLayoutToXml(subPath + "\\" + file4);
            }
        }

        # endregion HelperProcedures



        # region Methods

      

        //private void _dataGridModel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    int index =
        //        Convert.ToInt32(_dataGrid
        //            .GetRowVisibleIndexByHandle(viewblotterGrid.FocusedRowData.RowHandle.Value).ToString());
        //    if (index > -1)
        //    {
        //        GridColumn block_id = _dataGrid.Columns["block_id"];
        //        GridColumn symbol = _dataGrid.Columns["symbol"];
        //        GridColumn security_id = _dataGrid.Columns["security_id"];
        //        if (block_id != null)
        //        {
        //            BlockId = Convert.ToInt32(_dataGrid.GetCellValue(index, block_id).ToString());
        //            DeskId = 20218;

        //            _vm.se_get_linked_cash(DeskId);
        //        }
        //    }


        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Get_From_info();
            //DeskList();
          
        

            
        }

        void column_CustomGetSerializableProperties(object sender, CustomGetSerializablePropertiesEventArgs e)
        {
            e.SetPropertySerializable(GridColumn.EditSettingsProperty, new DXSerializable());

        }


     

        private void cmboDesk_SelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmboDesk.Items.Count == 0)
            {
                //DeskList();
                //Thread.Sleep(1000);
            }

            else
            {
                if (!String.IsNullOrEmpty((cmboDesk.SelectedItem).ToString()))
                {
                    Desk = (cmboDesk.SelectedItem).ToString();
                    DeskId = Convert.ToInt32(((RebalanceInfo.Client.ComboBoxItem)cmboDesk.SelectedItem).HiddenValue);
                   _vm.Parameters.DeskName = Desk;
                    //_vm.Get_DeskData(DeskId);
                    _vm.se_rebal_sessions_account(DeskId);
                    _vm.se_rebal_sessions_security(DeskId);
                    _vm.se_rebal_pos_exclusion(DeskId);
                    _vm.se_rebal_sessions(DeskId);

                }
            }
        }



        static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            GridColumn column = e.Item.Tag as GridColumn;
            ColumnBehavior.SetIsRenameEditorActivated(column, !ColumnBehavior.GetIsRenameEditorActivated(column));
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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _vm.se_rebal_sessions_account(DeskId);
            _vm.se_rebal_sessions_security(DeskId);
            _vm.se_rebal_pos_exclusion(DeskId);
            _vm.se_rebal_sessions(DeskId);

            DeskList();
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
