using System.Drawing;

namespace SpreadSheetWidget.Client.View
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
    using DevExpress.Spreadsheet;
  
    using SpreadSheetWidget.Client.ViewModel;
    using System.IO;
    using System.Threading;
    
     using SpreadSheetWidget.Client;
  


    public partial class SpreadSheetWidgetVisual : UserControl
    {
       
  
       
      
      
         private const string MSGBOX_TITLE_ERROR = "Yield Curve";
        string subPath = "c:\\dashboard\\Spreadsheets";
        string file = "PerformanceData.xaml";
       
        
         public SpreadSheetWidgetModel _view;


        #region Parameters

        string _xml;
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
        }

        IWorkbook _workbook;
        public IWorkbook Workbook
        {
            set { _workbook = value; }
            get { return _workbook; }
        }

         # endregion Parameters

         public SpreadSheetWidgetVisual(SpreadSheetWidgetModel ViewerModel)
         {


             InitializeComponent();

             this.DataContext = ViewerModel;
             this._view = ViewerModel;
             
             Workbook = spreadsheetControl1.Document;

           

             //Workbook.LoadDocument("C:\\DashBoard\\Spreadsheets\\Workstation.xlsx", DocumentFormat.Xlsx);
             
          




         }

       



 



        #region HelperProcedures


        public void AssignXML()
        {





            XML = _view.Parameters.Path;


            if (File.Exists((XML)))
            {



                Workbook.LoadDocument( XML, DocumentFormat.Xlsx);


            }

            spreadsheetControl1.Options.View.ShowColumnHeaders = _view.Parameters.Visible;
            spreadsheetControl1.Options.View.ShowRowHeaders = _view.Parameters.Visible;


        }

        public void SaveXML()
        {

            //XML = _view.Parameters.XML;

            //bool exists = System.IO.Directory.Exists((subPath));

            //if (!exists)
            //{
            //    System.IO.Directory.CreateDirectory((subPath));
            //    Workbook.LoadDocument(subPath + "\\" + XML, DocumentFormat.Xlsx);

            //}
            //else
            //{
            //    Workbook.LoadDocument(subPath + "\\" + XML, DocumentFormat.Xlsx);

            //}
        }
     

        private void Get_From_info()
        {

          

          


        }


        # endregion HelperProcedures



        # region Methods

    

     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            XML = _view.Parameters.Path;


            AssignXML();

        }

 
        #endregion Methods

       


     

      

      

    

    
      

       

      

        



    

    }
}
