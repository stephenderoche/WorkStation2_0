namespace Linedata.Framework.Client.TestWebBrowserWidget.Plugin
{
    using System;
    using System.Windows;
    using System.ComponentModel.Composition;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.Client.TestWebBrowserWidget.View;
    using Linedata.Framework.Client.TestWebBrowserWidget.ViewModel;
    using Linedata.Framework.WidgetFrame.ThemesAndStyles;
 
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    using Linedata.Shared.Widget.Common;


    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;


    [Export]
    public class WebBrowserWidget : WidgetSubscriber, IWidget
    {
        private readonly IThemeInfoProvider themeInfo;
        private readonly IReactivePublisher publisher;
        private WebBrowserView GenericWindow;
        private WebBrowserParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]

        public WebBrowserWidget( IReactivePublisher publisher
           , WebBrowserViewModel genericGridViewerMode)
            : base(publisher)
        
        {
            
           
            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;
           

            this.Parameters = new WebBrowserParameters();
            this.pluginParameters = new WebBrowserParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (WebBrowserParameters)this.Parameters;


            GenericWindow = new WebBrowserView(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;

            this.ChangeTheme(this.themeInfo.CurrentTheme);
            this.themeInfo.ThemeChangedEvent += new EventHandler<ThemeChangedEventArgs>(this.ThemeChanged);
        }

        ~WebBrowserWidget()
        {
          
            this.Dispose(false);
        }



        public bool CanShowSettings
        {
            get { return true; }
        }

        public CommunicationMode CommunicationMode
        {
            get { return CommunicationMode.Subscriber; }
        }

        public Size DesiredSize
        {
            get { return new Size(300, 200); }
        }



        //public WidgetGroups Group
        //{
        //    get
        //    {
        //        return this.ViewModel.Group;
        //    }

        //    set
        //    {
        //        this.ViewModel.Group = value;
        //    }
        //}

        public void InitWidget()
        {
            this.RegisterHandlerFor<WidgetMessages.AccountChanged>(this.Handle);
        }


        public WebBrowserViewModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            //BlotterViewSettingsViewModel settingsViewModel = new BlotterViewSettingsViewModel(this.ViewModel, this.GenericWindow);
            //BlotterViewSettingVisual window = new BlotterViewSettingVisual(settingsViewModel, this.GenericWindow);

            //window.Owner = Application.Current.MainWindow;
            //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //window.ShowDialog();
        }

        public FrameworkElement UiElement
        {
            get;
            private set;
        }
        //public void  Dispose()
        //{
        //    WebBrowserView wb = this.UiElement as WebBrowserView;

           
        
        //    GC.SuppressFinalize(this);
        //}

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            //this.ViewModel.PublishedBlock = message.AccountId;



            //this.ViewModel.GetChecks();
        }


      

        public class AccountChanged : GroupMessage
        {
            public long AccountId { get; private set; }

            public AccountChanged(
                Guid tabId,
                WidgetGroups group,
                long accountId)
                : base(group, tabId)
            {
                AccountId = accountId;
            }
        }
      

       // public FrameworkElement UiElement
       // {
       //     get;
       //     private set;
       // }

       //public CommunicationMode CommunicationMode 
       //{ 
       //     get;
       //    private set;
       // }

       // public Size DesiredSize
       // {
       //     get
       //     {
       //         return new Size(751, 174);
       //     }
       // }

       // public IWidgetParameters Parameters
       // {
       //     get;
       //     private set;
       // }

       // public bool CanShowSettings
       // {
       //     get { return false; }
       // }

       // public void ShowSettings()
       // {
       // }

       // public WebBrowserViewModel ViewModel { get; private set; }
       // public void Dispose()
       // {
       //     WebBrowserView wb = this.UiElement as WebBrowserView;
       //     if (wb != null && wb.webBrowser != null)
       //     {
       //         wb.webBrowser.Dispose();
       //     }

       //     //this.themeInfo.ThemeChangedEvent -= this.ThemeChanged;
       //     GC.SuppressFinalize(this);
       // }

        public void ChangeTheme(ThemeEnum theme)
        {

        }

       // public void InitWidget()
       // {
       // }

       // public WidgetGroups Group { get; set; }

       // public string Status { get; set; }

        private void ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            this.ChangeTheme(e.Theme);
        }
    }
}
