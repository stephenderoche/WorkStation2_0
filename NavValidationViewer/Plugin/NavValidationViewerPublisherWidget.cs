using Linedata.Shared.Widget.Common;
namespace NavValidationViewer.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using NavValidationViewer.Client.ViewModel;
    using NavValidationViewer.Client.View;
    using NavValidationViewer.Client.Model;
    using NavValidationViewer.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class NavValidationViewerPublisherWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private NavValidationViewerVisual GenericWindow;
        private NavValidationViewerPublisherParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public NavValidationViewerPublisherWidget(IReactivePublisher publisher
           , NavValidationViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new NavValidationViewerPublisherParameters();
            this.pluginParameters = new NavValidationViewerPublisherParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (NavValidationViewerPublisherParameters)this.Parameters;


            GenericWindow = new NavValidationViewerVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~NavValidationViewerPublisherWidget()
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

        public NavValidationViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            NavValidationViewerSettingsViewModel settingsViewModel = new NavValidationViewerSettingsViewModel(this.ViewModel, this.GenericWindow);
            NavValidationSettingVisual window = new NavValidationSettingVisual(settingsViewModel, this.GenericWindow);

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
            NavValidationViewerVisual wb = this.UiElement as NavValidationViewerVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {

            

                this.ViewModel.PublishedAccount = message.AccountId;
                this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
                this.ViewModel.get_account_name(Convert.ToString(message.AccountId));

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
