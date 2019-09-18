namespace Linedata.Framework.Client.TestWebBrowserWidget.View
{
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using Linedata.Framework.Client.TestWebBrowserWidget.ViewModel;
    using mshtml;
    using MouseEventArgs = System.Windows.Input.MouseEventArgs;
    using UserControl = System.Windows.Controls.UserControl;
    using System;

    /// <summary>
    /// Interaction logic for WebBrowserView.xaml
    /// </summary>
    public partial class WebBrowserView : UserControl
    {
        private bool isTitleDisplayed;

        public WebBrowserView()
        {
            InitializeComponent();
            this.isTitleDisplayed = false;
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

        private void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //// Disable JavaScript Errors
            this.InjectDisableScript();
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
