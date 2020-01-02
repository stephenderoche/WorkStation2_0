using Linedata.Shared.Widget.Common;
namespace MutualFundsCash.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using MutualFundsCash.Client.ViewModel;
    using MutualFundsCash.Client.View;
    using MutualFundsCash.Client.Model;
    using MutualFundsCash.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class MutualFundsCashWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private MutualFundsCashVisual GenericWindow;
        private MutualFundsCashdParams pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public MutualFundsCashWidget(IReactivePublisher publisher
           , MutualFundsCashViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new MutualFundsCashdParams();
            this.pluginParameters = new MutualFundsCashdParams();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (MutualFundsCashdParams)this.Parameters;


            GenericWindow = new MutualFundsCashVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~MutualFundsCashWidget()
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


        public MutualFundsCashViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            MutualFundsCashSettingsViewModel settingsViewModel = new MutualFundsCashSettingsViewModel(this.ViewModel, this.GenericWindow);
            MutualFundsCashSettingVisual window = new MutualFundsCashSettingVisual(settingsViewModel, this.GenericWindow);

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
            MutualFundsCashVisual wb = this.UiElement as MutualFundsCashVisual;

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
