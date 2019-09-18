using Linedata.Shared.Widget.Common;

namespace GenericChart
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Client.Workstation.LongviewAdapterClient;
    
    using GenericChart;
    
  
    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
   
   
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.Collections.Generic;
     
    using System;


    [Export]
    public class GenericChartWidget : WidgetSubscriber, IWidget
    {
       
        private readonly IReactivePublisher publisher;
        private  GenericChartView TopSecuritiesViewerWindow;
        private GenericChartParameters pluginParameters;
      
        private const string AccountIdColumnName = "account_id";
      
        

        [ImportingConstructor]
        public GenericChartWidget(IReactivePublisher publisher
           , GenericChartViewModel testSampleViewModel): base(publisher)
        {


            this.ViewModel = testSampleViewModel;
            this.ViewModel.ParentWidget = this;
           

            this.Parameters = new GenericChartParameters();
            this.pluginParameters = new GenericChartParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (GenericChartParameters)this.Parameters;

         
            TopSecuritiesViewerWindow = new GenericChartView(this.ViewModel);
            this.UiElement = TopSecuritiesViewerWindow;
            this.publisher = publisher;
        }


        ~GenericChartWidget()
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

      
        public GenericChartViewModel ViewModel { get; private set; }
     
        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
            GenericChartSettingViewModel settingsViewModel = new GenericChartSettingViewModel(this.ViewModel, this.TopSecuritiesViewerWindow);
            GenericChartSettingView window = new GenericChartSettingView(this.ViewModel, this.TopSecuritiesViewerWindow);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public FrameworkElement UiElement { get; private set; }

        public void Dispose()
        {
            GenericChartView wb = this.UiElement as GenericChartView;

            wb.savexml(this.TopSecuritiesViewerWindow.view.Parameters.DataType);
   
            GC.SuppressFinalize(this);
        }

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            this.ViewModel.get_account_name(Convert.ToString(message.AccountId));
            
            this.TopSecuritiesViewerWindow.UpdateAccoutText();
            this.TopSecuritiesViewerWindow.Securitieschart();
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
