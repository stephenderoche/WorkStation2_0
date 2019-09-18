using System.Drawing;

namespace ComplianceDashBoard.Client.View
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
    using DevExpress.Xpf.Utils.Themes;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Core;
   
  
  
    using ComplianceDashBoard.Client.ViewModel;
    using System.IO;
    using System.Threading;
  
     using ComplianceDashBoard.Client;
  


    public partial class ComplianceDashBoardVisual : UserControl
    {
       
  
       
      
      
         private const string MSGBOX_TITLE_ERROR = "DashBoard";
        string subPath = "c:\\dashboard";
        string file = "BreachMainGrid.xaml";
        string file1 = "BreachDetailGrid.xaml";
        
         public ComplianceDashBoardModel _view;


        #region Parameters

        string _xml;
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
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
         # endregion Parameters

         public ComplianceDashBoardVisual(ComplianceDashBoardModel ViewerModel)
         {


             InitializeComponent();
             this._view = ViewerModel;
             this.DataContext = ViewerModel;
             this._view._viewm = ViewerModel;

             ApplicationThemeHelper.ApplicationThemeName = _view.Parameters.DefaultTheme;

             this.comboBoxEdit1.Text = _view.AccountName;
             this.TBToReview.IsChecked = _view.Parameters.ToReview;

        
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
                                    _view.se_get_cmpl_breach_mgt_report();

                             
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



        #region HelperProcedures


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


        }

        public void SaveXML()
        {


            bool exists = System.IO.Directory.Exists((subPath));

            if (!exists)
            {
                System.IO.Directory.CreateDirectory((subPath));

                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridDetail.SaveLayoutToXml(subPath + "\\" + file1);

            }
            else
            {
                _dataGrid.SaveLayoutToXml(subPath + "\\" + file);
                _dataGridDetail.SaveLayoutToXml(subPath + "\\" + file1);

            }
        }
        

        private void Get_From_info()
        {

            _view.AccountName = _view.Parameters.AccountName;
           

          


        }

 


     
  

     

        public void UpdateAccoutText()
        {


            comboBoxEdit1.Text = _view.AccountName;


        }

       

       
        # endregion HelperProcedures



        # region Methods

        private void _dataGridModel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            int _case_id;
            int _invocation_id;


            int index =
                Convert.ToInt32(_dataGrid
                    .GetRowVisibleIndexByHandle(_viewDataGrid.FocusedRowData.RowHandle.Value).ToString());



            if (index > -1)
            {



                GridColumn case_id = _dataGrid.Columns["case_id"];
                GridColumn invocation_id = _dataGrid.Columns["cmpl_invocation_id"];

                if (case_id != null)
                {


                    _case_id = Convert.ToInt32(_dataGrid.GetCellValue(index, case_id).ToString());
                    _invocation_id = Convert.ToInt32(_dataGrid.GetCellValue(index, invocation_id).ToString());

                    _view.get_cmpl_breach_details(_case_id, _invocation_id);

                }
            }


        }

     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
         
            Get_From_info();
           

        }

      

    

        #region Grid Stuff

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

        #endregion

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
        
        }
      

    
        #endregion Methods

        private void onClick(object sender, RoutedEventArgs e)
        {
            _view.se_get_cmpl_breach_mgt_report();
        }

       


     

      

      

    

    
      

       

      

        



    

    }
}
