using Linedata.Shared.Widget.Common;
namespace DynamicGroups.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
   
    using DynamicGroups.Client.ViewModel;
    using DynamicGroups.Client.View;
    using DynamicGroups.Client.Model;
    using DynamicGroups.Client.Plugin;

    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    

    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;

    using System;


    [Export]

    public class DynamicGroupsPublisherWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private DynamicGroupsVisual GenericWindow;
        private DynamicGroupsPublisherParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public DynamicGroupsPublisherWidget(IReactivePublisher publisher
           , DynamicGroupsViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new DynamicGroupsPublisherParameters();
            this.pluginParameters = new DynamicGroupsPublisherParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (DynamicGroupsPublisherParameters)this.Parameters;


            GenericWindow = new DynamicGroupsVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~DynamicGroupsPublisherWidget()
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


        public DynamicGroupsViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            DynamicGroupsSettingsViewModel settingsViewModel = new DynamicGroupsSettingsViewModel(this.ViewModel, this.GenericWindow);
            DynamicGroupsSettingVisual window = new DynamicGroupsSettingVisual(settingsViewModel, this.GenericWindow);

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
            DynamicGroupsVisual wb = this.UiElement as DynamicGroupsVisual;

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
