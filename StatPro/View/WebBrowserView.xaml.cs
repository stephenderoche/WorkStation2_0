namespace Linedata.Framework.Client.TestWebBrowserWidget.View
{
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using Linedata.Framework.Client.TestWebBrowserWidget.ViewModel;
    using Microsoft.CSharp.RuntimeBinder;
    using System.Windows.Navigation;
    using mshtml;
    using System.Reflection;
    using MouseEventArgs = System.Windows.Input.MouseEventArgs;
    using UserControl = System.Windows.Controls.UserControl;
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using SalesSharedContracts;
    using Linedata.Framework.Client.TestWebBrowserWidget.Helpers;

    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using DevExpress.Xpf.Grid;
    using System.Windows.Media;
    using DevExpress.Xpf.Bars;

    using DevExpress.Xpf.Editors;

    using System.IO;
    using System.Threading;

    /// <summary>
    /// Interaction logic for WebBrowserView.xaml
    /// </summary>
    public partial class WebBrowserView : UserControl
    {
        private bool isTitleDisplayed;
        public WebBrowserViewModel _view;
        public WebBrowserView(WebBrowserViewModel ViewerModel)
        {
            
            InitializeComponent();
            this._view = ViewerModel;
            this.isTitleDisplayed = false;
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


        private void OnTitleMouseEnter(object sender, MouseEventArgs e)
        {
            if (!this.isTitleDisplayed)
            {
                this.ShowTitles();
            }
        }

        private void HideTitles()
        {
            Storyboard hideGrid = (Storyboard)this.FindResource("HideAnimation");

            if (this.SettingGrid.Height > 0)
            {
                hideGrid.Begin();
            }

            this.isTitleDisplayed = false;
        }

        private void ShowTitles()
        {
            if (this.SettingGrid.Height == 0)
            {
                Storyboard expandGrid = (Storyboard)this.FindResource("ExpandAnimation");
                expandGrid.Begin();
            }

            this.isTitleDisplayed = true;
        }

        private void OnEndNavigating(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            (this.DataContext as WebBrowserViewModel).SetLoadingState(false);
            (this.DataContext as WebBrowserViewModel).UrlForTitle = this.webBrowser.Source.ToString();
            (this.DataContext as WebBrowserViewModel).UpdateTitle();
        }

        private void OnBeginNavigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            (this.DataContext as WebBrowserViewModel).SetLoadingState(true);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.isTitleDisplayed)
            {
                this.HideTitles();
            }
            else
            {
                this.ShowTitles();
            }
        }

        //private void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    //// Disable JavaScript Errors
        //    this.InjectDisableScript();
        //}

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


        /// <summary>
        /// Inject JS code that should disable popup errors
        /// But some JS errors still appears : Need to be fixed.
        /// </summary>
        private void InjectDisableScript()
        {
            string disableScriptError = @"function noError() {return true;} window.onerror = noError;";

            HTMLDocumentClass doc = webBrowser.Document as HTMLDocumentClass;
            HTMLDocument doc2 = webBrowser.Document as HTMLDocument;

            IHTMLScriptElement scriptErrorSuppressed = (IHTMLScriptElement)doc2.createElement("SCRIPT");
            scriptErrorSuppressed.type = "text/javascript";
            scriptErrorSuppressed.text = disableScriptError;

            IHTMLElementCollection nodes = doc.getElementsByTagName("head");

            foreach (IHTMLElement elem in nodes)
            {
                try 
                { 
                    HTMLHeadElementClass head = (HTMLHeadElementClass)elem;
                    head.appendChild((IHTMLDOMNode)scriptErrorSuppressed);
                }
                catch(Exception)
                {
                }
            }
        }
    }
}
