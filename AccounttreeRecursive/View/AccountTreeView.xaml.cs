namespace AccountTreeCashViewer.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using AccountTreeCashViewer.ViewModel;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
   
    using System.Threading;
    using SalesSharedContracts;
    using Linedata.Framework.Foundation;
    using System.Data;
    using System;
    using Linedata.Shared.Api.ServiceModel;
    using AccountTreeCashViewer;
    using System.IO;
    using DevExpress.Xpf.Bars;
    using DevExpress.Xpf.Grid;
    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.Core.ConditionalFormatting;
    using DevExpress.Xpf.Core.Serialization;
    using AccountTreeCashViewer.Helpers;
    using Linedata.Client.Workstation.SharedReferences;
    using System.Collections.ObjectModel;
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Xpf.Utils.Themes;

    using DevExpress.Xpf.Core;

    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;

    public partial class AccountTreeView : UserControl
    {

        public AccountTreeViewModel _view;
        public AccountTreeView(AccountTreeViewModel ViewModel)
        {
            
            InitializeComponent();
            this.DataContext = ViewModel;
            this._view = ViewModel;

            ApplicationThemeHelper.ApplicationThemeName = _view.Parameters.DefaultTheme;

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

                                    _view.Get_Tree_info(Convert.ToInt32(_view.AccountId));
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

        #region Methods
        private void treeListControl1_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            AccountTreeMap child = new AccountTreeMap();
 
            try
            {
                int index =
                    Convert.ToInt32(treeListControl1.GetRowVisibleIndexByHandle(treeListView1.FocusedRowData.RowHandle.Value).ToString());

                if (index > -1)
                {
                    child = e.NewItem as AccountTreeMap;

                    _view.SelectedAccountId = child.Child_account_id;
                    _view.SetSelectedAccount(Convert.ToInt64(_view.SelectedAccountId));

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred.  " + ex.Message, "Account", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            if (_view.SelectedAccountId != -1)
            {
                _view.OnOpenAppraisalReport((long)_view.SelectedAccountId);
            }
        }
        private void treeListControl1_Loaded(object sender, RoutedEventArgs e)
        {
            SmartExpandNodes(4);
        }
        private void SmartExpandNodes(int minChildCount)
        {
            TreeListNodeIterator nodeIterator = new TreeListNodeIterator(treeListView1.Nodes, true);
            while (nodeIterator.MoveNext())
                nodeIterator.Current.IsExpanded = nodeIterator.Current.Nodes.Count >= minChildCount;
        }
        private void Click_button(object sender, RoutedEventArgs e)
        {
            _view.Get_Tree_info(Convert.ToInt32(_view.AccountId));
        }
        private void user_loaded(object sender, RoutedEventArgs e)
        {
            Validate(_view.Parameters.AccountName);
        }
        #endregion Methods

    }
}
