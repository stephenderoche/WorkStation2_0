using Linedata.Shared.Widget.Common;
namespace IPO.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using IPO.Client.ViewModel;
    using IPO.Client.View;
    using IPO.Client.Model;
  

    using System;


    [Export]

    public class IPOWidget : WidgetSubscriber, IWidget
    {
       
        private IPOVisual GenericWindow;
        private DataDashBoardParameters pluginParameters;
        private readonly IReactivePublisher publisher;
     

        [ImportingConstructor]
        public IPOWidget(IReactivePublisher publisher
           , IPOModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new DataDashBoardParameters();
            this.pluginParameters = new DataDashBoardParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (DataDashBoardParameters)this.Parameters;


            GenericWindow = new IPOVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~IPOWidget()
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


        public IPOModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            IPOSettingsViewModel settingsViewModel = new IPOSettingsViewModel(this.ViewModel, this.GenericWindow);
            IPOSettingVisual window = new IPOSettingVisual(settingsViewModel, this.GenericWindow);

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
            IPOVisual wb = this.UiElement as IPOVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            this.GenericWindow.get_account_name(Convert.ToString(message.AccountId));

            this.GenericWindow.UpdateAccoutText();
          
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
