namespace PythonCloud.Client.View
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
   using PythonCloud.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using PythonCloud.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Core.Serialization;
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;
    using System.Windows.Navigation;
    //using mshtml;
    using System.Reflection;
  


    public partial class PythonCloudVisual : UserControl
    {

        string subPath = "c:\\dashboard";
        string file = "CashFlowCload.xaml";
        string file2 = "accountPositions.xaml";
        string file3 = "NewsPositions.xaml";
         # region Parameters


        private string getsecurityTextinfo = "";
        private int getsecurityLenthinfo = 0;
        public PythonCloudViewerModel _vm;

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

         public PythonCloudVisual(PythonCloudViewerModel securityHistoryViewerModel)
         {


             InitializeComponent();
            

             this.DataContext = securityHistoryViewerModel;
             this._vm = securityHistoryViewerModel;
       
         
           
           

             AssignXML();


         }

        # region Security

   

      

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

            
            this._dataGrid.Visibility = System.Windows.Visibility.Visible;
            this._dataGridCash.Visibility = System.Windows.Visibility.Hidden;

        }

      

        private void AssignXML()
        {


            if (File.Exists((subPath + "\\" + file)))
            {
                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + file);
            }

            if (File.Exists((subPath + "\\" + file2)))
            {
                _dataGridCash.RestoreLayoutFromXml(subPath + "\\" + file2);
            }

            if (File.Exists((subPath + "\\" + file3)))
            {
                _dataGridNews.RestoreLayoutFromXml(subPath + "\\" + file3);
            }


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));


                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridCash.SaveLayoutToXml(subPath + "\\" + file2);
                _dataGridNews.SaveLayoutToXml(subPath + "\\" + file3);

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridCash.SaveLayoutToXml(subPath + "\\" + file2);
                _dataGridNews.SaveLayoutToXml(subPath + "\\" + file3);
            }
        }

        # endregion HelperProcedures



        # region Methods

      

       

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Get_From_info();
          //  DeskList();
            
            //DXSerializer.AddEndDeserializingHandler(_dataGrid, new EndDeserializingEventHandler(OnEndDeserializing));
            //DXSerializer.AddStartSerializingHandler(_dataGrid, new RoutedEventHandler(OnStartSerializing));

            
        }

        void column_CustomGetSerializableProperties(object sender, CustomGetSerializablePropertiesEventArgs e)
        {
            e.SetPropertySerializable(GridColumn.EditSettingsProperty, new DXSerializable());

        }

        #endregion Methods

        private void _dataGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {


            int index =
                Convert.ToInt32(_dataGrid
                    .GetRowVisibleIndexByHandle(viewGrid.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn symbol = _dataGrid.Columns["Symbol"];






                if (symbol != null)
                {


                    _vm.Symbol = _dataGrid.GetCellValue(index, symbol).ToString();



                    if (!string.IsNullOrEmpty(_vm.Symbol))
                    {

                        _vm.GetNews();
                     

                    }


                }
            }


        }

        private void _dataGridCash_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {


            int index =
                Convert.ToInt32(_dataGridCash
                    .GetRowVisibleIndexByHandle(viewCashGrid.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn symbol = _dataGridCash.Columns["symbol"];






                if (symbol != null)
                {


                    _vm.Symbol = _dataGridCash.GetCellValue(index, symbol).ToString();



                    if (!string.IsNullOrEmpty(_vm.Symbol))
                    {

                        _vm.GetNews();


                    }


                }
            }


        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            //// Disable JavaScript Errors
            //this.InjectDisableScript();
            dynamic activeX = this.webBrowser.GetType().InvokeMember("ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, this.webBrowser, new object[] { });

            activeX.Silent = true;

            this.webBrowser.Navigated -= new System.Windows.Navigation.NavigatedEventHandler(this.OnNavigated);
        }
        
        
        private void _dataGridNews_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {


            string Url;
            int index =
                Convert.ToInt32(_dataGridNews
                    .GetRowVisibleIndexByHandle(viewNewsGrid.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn _url = _dataGridNews.Columns["url"];






                if (_url != null)
                {


                    Url = _dataGridNews.GetCellValue(index, _url).ToString();



                    if (!string.IsNullOrEmpty(Url))
                    {

                       this.webBrowser.Navigate(Url);


                    }


                }
            }


        }

        private void RadioButtonPositions_Checked(object sender, RoutedEventArgs e)
        {
           

            this._dataGrid.Visibility = System.Windows.Visibility.Hidden;
            this._dataGridCash.Visibility = System.Windows.Visibility.Visible;
        }

         private void RadioButtonCash_Checked(object sender, RoutedEventArgs e)
        {
            this._dataGrid.Visibility = System.Windows.Visibility.Visible;
            this._dataGridCash.Visibility = System.Windows.Visibility.Hidden;
        }

    }
   


}
