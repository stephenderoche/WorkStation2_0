namespace Clarity.Client.View
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
   using Clarity.Client.ViewModel;
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Linedata.Shared.Widget.Common;
     using Clarity.Client;
    using DevExpress.Xpf.Data;
    using DevExpress.Data;
    using DevExpress.Utils;
    using DevExpress.Utils.Serializing;
    using DevExpress.Utils.Serializing.Helpers;

    using System.Windows.Media.Animation;
    using System.Windows.Input;
    using Microsoft.CSharp.RuntimeBinder;
    using System.Windows.Navigation;
    using mshtml;
    using System.Reflection;
    using MouseEventArgs = System.Windows.Input.MouseEventArgs;
    using UserControl = System.Windows.Controls.UserControl;
   
  


    public partial class ClarityVisual : UserControl
    {
        public ClarityViewerModel _view;
        private bool isTitleDisplayed;
        
  
         # region Parameters

         # endregion Parameters

        public ClarityVisual(ClarityViewerModel ViewerModel)
         {
             InitializeComponent();
             this._view = ViewerModel;
             this.DataContext = ViewerModel;
             this.comboBoxEdit1.Text = _view.AccountName;
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
                                     
                                     _view.ProcessWebPage();


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
         public void UpdateAccoutText()
         {


             comboBoxEdit1.Text = _view.AccountName;


         }
        # endregion HelperProcedures



        # region Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
          
            
        }
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
            (this.DataContext as ClarityViewerModel).SetLoadingState(false);
            (this.DataContext as ClarityViewerModel).UrlForTitle = this.webBrowser.Source.ToString();
            (this.DataContext as ClarityViewerModel).UpdateTitle();
        }

        private void OnBeginNavigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            (this.DataContext as ClarityViewerModel).SetLoadingState(true);
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
                catch (Exception)
                {
                }
            }
        }
       
        #endregion Methods

        private void TitleNameTextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.isTitleDisplayed)
            {
                this.ShowTitles();
            }
        }
     



    }
 
}
