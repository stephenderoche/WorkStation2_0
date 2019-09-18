using Linedata.Shared.Widget.Common;
namespace ExceptionManagement.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using ExceptionManagement.Client.ViewModel;
    using ExceptionManagement.Client.View;
    using ExceptionManagement.Client.Model;
    using ExceptionManagement.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
  

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class DriftToolViewerPublisherWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private DriftToolViewerVisual GenericWindow;
        private DriftToolViewerPublisherParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public DriftToolViewerPublisherWidget(IReactivePublisher publisher
           , DriftToolViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new DriftToolViewerPublisherParameters();
            this.pluginParameters = new DriftToolViewerPublisherParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (DriftToolViewerPublisherParameters)this.Parameters;


            GenericWindow = new DriftToolViewerVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~DriftToolViewerPublisherWidget()
        {
          
            this.Dispose(false);
        }



        public bool CanShowSettings
        {
            get { return false; }
        }

        public CommunicationMode CommunicationMode
        {
            get { return CommunicationMode.Publisher; }
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
            //if (this.ViewModel.WidgetName != "DriftTool")
            //{
            //     this.RegisterHandlerFor<WidgetMessages.AccountChanged>(this.Handle);
            //}
        }


        public DriftToolViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            DriftToolViewerSettingsViewModel settingsViewModel = new DriftToolViewerSettingsViewModel(this.ViewModel, this.GenericWindow);
            DriftToolSettingVisual window = new DriftToolSettingVisual(settingsViewModel, this.GenericWindow);

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
            DriftToolViewerVisual wb = this.UiElement as DriftToolViewerVisual;

          wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {

            if (this.ViewModel.WidgetName != "DriftTool")
            {

                this.ViewModel.PublishedAccount = message.AccountId;
                this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
                this.ViewModel.get_account_name(Convert.ToString(message.AccountId));

                this.GenericWindow.UpdateAccoutText();
            }
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
