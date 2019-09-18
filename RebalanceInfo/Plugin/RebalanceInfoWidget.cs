using Linedata.Shared.Widget.Common;
namespace RebalanceInfo.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using RebalanceInfo.Client.ViewModel;
    using RebalanceInfo.Client.View;
    using RebalanceInfo.Client.Model;
    using RebalanceInfo.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class RebalanceInfoWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private RebalanceInfoVisual GenericWindow;
        private RebalanceInfoParams pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public RebalanceInfoWidget(IReactivePublisher publisher
           , RebalanceInfoViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new RebalanceInfoParams();
            this.pluginParameters = new RebalanceInfoParams();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (RebalanceInfoParams)this.Parameters;


            GenericWindow = new RebalanceInfoVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~RebalanceInfoWidget()
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


        public RebalanceInfoViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            ReblanceInfoSettingsViewModel settingsViewModel = new ReblanceInfoSettingsViewModel(this.ViewModel, this.GenericWindow);
            LinkCashSettingVisual window = new LinkCashSettingVisual(settingsViewModel, this.GenericWindow);

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
            RebalanceInfoVisual wb = this.UiElement as RebalanceInfoVisual;

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
