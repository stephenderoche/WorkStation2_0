using Linedata.Shared.Widget.Common;

namespace Guages.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
    using Guages.Client.View;
    using Guages.Client.ViewModel;
    using Guages.Client.Model;
    using Guages.Client.Plugin;
  
    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    
   
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;
     
    using System;


    [Export]
    public class GaugesWidget : WidgetSubscriber, IWidget
    {
       
        private readonly IReactivePublisher publisher;
        private GuagesView _viewerWindow;
        private GuagesParameters pluginParameters;
      
        private const string AccountIdColumnName = "account_id";
      
        

        [ImportingConstructor]
        public GaugesWidget(IReactivePublisher publisher
           , GuagesViewModel ViewModel): base(publisher)
        {


            this.ViewModel = ViewModel;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new GuagesParameters();
            this.pluginParameters = new GuagesParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (GuagesParameters)this.Parameters;


            _viewerWindow = new GuagesView(this.ViewModel);
            this.UiElement = _viewerWindow;
            this.publisher = publisher;
        }


        ~GaugesWidget()
        {
            this.Dispose(false);
        }


        public bool CanShowSettings
        {
            get { return true; }
        }

        public CommunicationMode CommunicationMode
        {
            get { return CommunicationMode.None; }
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
            this.RegisterHandlerFor<WidgetMessages.AccountChanged>(this.Handle);
        }

      
        public GuagesViewModel ViewModel { get; private set; }
     
        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
            GuagesSettingViewModel settingsViewModel = new GuagesSettingViewModel(this.ViewModel, this._viewerWindow);
            GuagesSettingView window = new GuagesSettingView(settingsViewModel, this._viewerWindow);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public FrameworkElement UiElement { get; private set; }

        public void Dispose()
        {
            GuagesView wb = this.UiElement as GuagesView;

          
   
            GC.SuppressFinalize(this);
        }

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
          
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
