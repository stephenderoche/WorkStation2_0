using Linedata.Shared.Widget.Common;

namespace AccountTreeCashViewer.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
    using AccountTreeCashViewer.View;
    using AccountTreeCashViewer.ViewModel;
    using AccountTreeCashViewer.Model;
    using AccountTreeCashViewer.Plugin;
  
    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
 
   
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;
     
    using System;


    [Export]
    public class AccountTreeCashViewerWidget : WidgetSubscriber, IWidget
    {
       
        private readonly IReactivePublisher publisher;
        private AccountTreeView ViewerWindow;
        private AccountTreeCashViewerParameters pluginParameters;
      
        private const string AccountIdColumnName = "account_id";
      
        

        [ImportingConstructor]
        public AccountTreeCashViewerWidget(IReactivePublisher publisher
           , AccountTreeViewModel testSampleViewModel): base(publisher)
        {


            this.ViewModel = testSampleViewModel;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new AccountTreeCashViewerParameters();
            this.pluginParameters = new AccountTreeCashViewerParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (AccountTreeCashViewerParameters)this.Parameters;


            ViewerWindow = new AccountTreeView(this.ViewModel);
            this.UiElement = ViewerWindow;
            this.publisher = publisher;
        }


        ~AccountTreeCashViewerWidget()
        {
            this.Dispose(false);
        }


        public bool CanShowSettings
        {
            get { return true; }
        }

        public CommunicationMode CommunicationMode
        {
            get { return CommunicationMode.Publisher; }
        }

        public Size DesiredSize
        {
            get
            {
                return new Size(300, 300);
            }
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
            //this.RegisterHandlerFor<WidgetMessages.AccountChanged>(this.Handle);
        }

      
        public AccountTreeViewModel ViewModel { get; private set; }
     
        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
            AccountTreeCashSettingViewModel settingsViewModel = new AccountTreeCashSettingViewModel(this.ViewModel, this.ViewerWindow);
            AccountTreeCashViewerSettingView window = new AccountTreeCashViewerSettingView(settingsViewModel, this.ViewerWindow);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public FrameworkElement UiElement { get; private set; }

        public void Dispose()
        {
            AccountTreeView wb = this.UiElement as AccountTreeView;

   
            GC.SuppressFinalize(this);
        }

        public void Handle(WidgetMessages.AccountChanged message)
        {
           // this.ViewModel.PublishedAccount = message.AccountId;
            //this.ViewerWindow.AccountId = message.AccountId;
            //this.ViewerWindow.get_account_name(Convert.ToString(message.AccountId));
            
            
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
