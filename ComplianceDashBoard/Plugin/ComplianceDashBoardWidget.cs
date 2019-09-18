using Linedata.Shared.Widget.Common;
namespace ComplianceDashBoard.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using ComplianceDashBoard.Client.ViewModel;
    using ComplianceDashBoard.Client.View;
    using ComplianceDashBoard.Client.Model;
  


    using System;


    [Export]

    public class ComplianceDashBoardWidget : WidgetSubscriber, IWidget
    {
       
        private ComplianceDashBoardVisual GenericWindow;
        private FIDashBoardParameters pluginParameters;
        private readonly IReactivePublisher publisher;

        public ComplianceDashBoardWidget(IReactivePublisher publisher, bool suppressCustomMessages = false) : base(publisher, suppressCustomMessages)
        {
        }

        [ImportingConstructor]
        public ComplianceDashBoardWidget(IReactivePublisher publisher
           , ComplianceDashBoardModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new FIDashBoardParameters();
            this.pluginParameters = new FIDashBoardParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (FIDashBoardParameters)this.Parameters;


            GenericWindow = new ComplianceDashBoardVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~ComplianceDashBoardWidget()
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


        public ComplianceDashBoardModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            ComplianceDashBoardSettingsViewModel settingsViewModel = new ComplianceDashBoardSettingsViewModel(this.ViewModel, this.GenericWindow);
            ComplianceDashBoardSettingVisual window = new ComplianceDashBoardSettingVisual(settingsViewModel, this.GenericWindow);

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
            ComplianceDashBoardVisual wb = this.UiElement as ComplianceDashBoardVisual;

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
