namespace NavProcess.Client.View
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
  
    using NavProcess.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using NavProcess.Client;
  


    public partial class NavProcessViewerVisual : UserControl
    {
       
  
       // private string MSGBOX_TITLE_ERROR = "Generic Viewer";
      
         string subPath = "c:\\dashboard";
         string file = "NavProcessStatus.xaml";
         private const string MSGBOX_TITLE_ERROR = "Nav Process";
        
         public NavProcessViewerModel _view;


         # region Parameters

        


         private string _accountName = string.Empty;
         public string AccountName
         {
             set { _accountName = value; }
             get { return _accountName; }
         }

          string _xml;
         public string XML
         {
             set { _xml = value; }
             get { return _xml; }
         }

         string _startDate;
         public string StartDate
         {
             set { _startDate = value; }
             get { return _startDate; }
         }
       

      
         # endregion Parameters

         public NavProcessViewerVisual(NavProcessViewerModel ViewerModel)
         {


             InitializeComponent();
            
           
         
             this.comboBoxEdit1.Text = AccountName;



             this.DataContext = ViewerModel;
             this._view = ViewerModel;
            
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
                                    _view.Parameters.AccountName = AccountName;


                                    _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToDateTime(StartDate));
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

        private void Get_From_info()
        {

            AccountName = _view.Parameters.AccountName;
           

            AssignXML();


        }

   

       
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




            if (isNull(XML))
            {
                XML = file;
            }

            XML = _view.Parameters.XML;

            if (File.Exists((subPath + "\\" + XML)))
            {
                _dataGrid.RestoreLayoutFromXml(subPath + "\\" + XML);
            }

            SaveXML();
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
            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + XML);
            }
        }

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = AccountName;


        }

        void getDates()
        {

            DateTime intStr;
            bool intResultTryParse = DateTime.TryParse(Convert.ToString(_view.Parameters.StartDate), out intStr);
            if (intResultTryParse == true)
            {
                _startDate = Convert.ToString(intStr);

            }
            else
            {
                _startDate = "01/01/2015";
            }

        }
       
        # endregion HelperProcedures



        # region Methods
     

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            _view.Get_ChartData(Convert.ToInt32(_view.AccountId), Convert.ToDateTime(StartDate));
        }

    

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            bool exists = System.IO.Directory.Exists((subPath));
            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));
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

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {

            _viewDataGrid.ShowPrintPreview(this);
           
        }
        #endregion Methods

        

       

      

    

    
      

       

      

        



    

    }
}
