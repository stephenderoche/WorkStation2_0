using Linedata.Shared.Widget.Common;
namespace RestrictedSecurity.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using RestrictedSecurity.Client.ViewModel;
    using RestrictedSecurity.Client.View;
    using RestrictedSecurity.Client.Model;
    using RestrictedSecurity.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class RestrictedSecurityWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private RestrictedSecurityVisual GenericWindow;
        private RestrictedSecurityParams pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public RestrictedSecurityWidget(IReactivePublisher publisher
           , RestrictedSecurityViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new RestrictedSecurityParams();
            this.pluginParameters = new RestrictedSecurityParams();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (RestrictedSecurityParams)this.Parameters;


            GenericWindow = new RestrictedSecurityVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~RestrictedSecurityWidget()
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


        public RestrictedSecurityViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            RestrictedSecuritySettingsViewModel settingsViewModel = new RestrictedSecuritySettingsViewModel(this.ViewModel, this.GenericWindow);
            RestrictedSecuritySettingVisual window = new RestrictedSecuritySettingVisual(settingsViewModel, this.GenericWindow);

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
            RestrictedSecurityVisual wb = this.UiElement as RestrictedSecurityVisual;

            wb.SaveXML();
        
            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedBlock = message.AccountId;



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
      
    }
}
