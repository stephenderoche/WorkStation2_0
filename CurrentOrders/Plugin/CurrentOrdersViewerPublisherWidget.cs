using Linedata.Shared.Widget.Common;
namespace CurrentOrders.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using CurrentOrders.Client.ViewModel;
    using CurrentOrders.Client.View;
    using CurrentOrders.Client.Model;
    using CurrentOrders.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class CurrentOrdersViewerPublisherWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private CurrentOrdersViewerVisual GenericWindow;
        private CurrentOrdersViewerPublisherParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public CurrentOrdersViewerPublisherWidget(IReactivePublisher publisher
           , CurrentOrdersViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new CurrentOrdersViewerPublisherParameters();
            this.pluginParameters = new CurrentOrdersViewerPublisherParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (CurrentOrdersViewerPublisherParameters)this.Parameters;


            GenericWindow = new CurrentOrdersViewerVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~CurrentOrdersViewerPublisherWidget()
        {
          
            this.Dispose(false);
        }



        public bool CanShowSettings
        {
            get { return false; }
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


        public CurrentOrdersViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            CurrentOrdersViewerSettingsViewModel settingsViewModel = new CurrentOrdersViewerSettingsViewModel(this.ViewModel, this.GenericWindow);
            CurrentOrdersSettingVisual window = new CurrentOrdersSettingVisual(settingsViewModel, this.GenericWindow);

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
            CurrentOrdersViewerVisual wb = this.UiElement as CurrentOrdersViewerVisual;

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
