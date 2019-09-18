using Linedata.Shared.Widget.Common;
namespace TraderVsPM.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Runtime.Serialization;

    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;

    using TraderVsPM.Client.ViewModel;
    //using TraderVsPmApp.Client.View;
    using TraderVsPM.Client.Model;
    // TraderVsPmApp.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;


    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
 
     [Export]
    public class TraderVsPmWidget : WidgetSubscriber, IWidget


    {


        private readonly IReactivePublisher publisher;
        private TraderVsPmParameters pluginParameters;
      
        TraderVsPm GenericWindow;
          [ImportingConstructor]
        public TraderVsPmWidget(IReactivePublisher publisher
           , TraderVsPmAppViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new TraderVsPmParameters();
            this.pluginParameters = new TraderVsPmParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (TraderVsPmParameters)this.Parameters;


            GenericWindow = new TraderVsPm(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
           
        }

        public TraderVsPmAppViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {

            //SecurityHistorySettingsViewModel settingsViewModel = new SecurityHistorySettingsViewModel(this.ViewModel, this.GenericWindow);
            //SecurityHistorySettingVisual window = new SecurityHistorySettingVisual(settingsViewModel, this.GenericWindow);

            //window.Owner = Application.Current.MainWindow;
            //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //window.ShowDialog();
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

        public FrameworkElement UiElement
        {
            get;
            private set;
        }
        public void Dispose()
        {
            TraderVsPm wb = this.UiElement as TraderVsPm;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

        public void InitWidget()
        {

            this.RegisterHandlerFor<WidgetMessages.AccountChanged>(this.Handle);
        }

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedBlock = message.AccountId;
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            this.ViewModel.get_account_name(Convert.ToString(message.AccountId));

            //this.GenericWindow.UpdateAccoutText();



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
