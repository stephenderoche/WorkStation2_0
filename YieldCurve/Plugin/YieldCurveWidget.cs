using Linedata.Shared.Widget.Common;
namespace YieldCurve.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using YieldCurve.Client.ViewModel;
    using YieldCurve.Client.View;
    using YieldCurve.Client.Model;
  


    using System;


    [Export]

    public class YieldCurveWidget : WidgetSubscriber, IWidget
    {
       
        private YieldCurveVisual GenericWindow;
        private YieldCurveParameters pluginParameters;
        private readonly IReactivePublisher publisher;

        public YieldCurveWidget(IReactivePublisher publisher, bool suppressCustomMessages = false) : base(publisher, suppressCustomMessages)
        {
        }

        [ImportingConstructor]
        public YieldCurveWidget(IReactivePublisher publisher
           , YieldCurveModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new YieldCurveParameters();
            this.pluginParameters = new YieldCurveParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (YieldCurveParameters)this.Parameters;


            GenericWindow = new YieldCurveVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~YieldCurveWidget()
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



        public WidgetGroups Group
        {
            get
            {
                return this.ViewModel.Group;
            }

            set
            {
                this.ViewModel.Group = value;
            }
        }

        public void InitWidget()
        {
            this.RegisterHandlerFor<WidgetMessages.AccountChanged>(this.Handle);
        }


        public YieldCurveModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            YieldCurveSettingsViewModel settingsViewModel = new YieldCurveSettingsViewModel(this.ViewModel, this.GenericWindow);
            YieldCurveSettingVisual window = new YieldCurveSettingVisual(settingsViewModel, this.GenericWindow);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public FrameworkElement UiElement
        {
            get;
            private set;
        }
        public void  Dispose()
        {
            YieldCurveVisual wb = this.UiElement as YieldCurveVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            this.ViewModel.get_account_name(Convert.ToString(message.AccountId));

            this.GenericWindow.UpdateAccoutText();
           // this.GenericWindow.Get_ChartData();
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
      
    }
}
