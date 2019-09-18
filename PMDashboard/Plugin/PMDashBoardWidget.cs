using Linedata.Shared.Widget.Common;
namespace PMDashBoard.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using PMDashBoard.Client.ViewModel;
    using PMDashBoard.Client.View;
    using PMDashBoard.Client.Model;



    using System;


    [Export]

    public class PMDashBoardWidget : WidgetSubscriber, IWidget
    {
       
        private PMDashBoardVisual GenericWindow;
        private PMDashBoardParameters pluginParameters;
        private readonly IReactivePublisher publisher;
     

        [ImportingConstructor]
        public PMDashBoardWidget(IReactivePublisher publisher
           , PMDashBoardModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new PMDashBoardParameters();
            this.pluginParameters = new PMDashBoardParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (PMDashBoardParameters)this.Parameters;


            GenericWindow = new PMDashBoardVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~PMDashBoardWidget()
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


        public PMDashBoardModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            PMDashBoardSettingsViewModel settingsViewModel = new PMDashBoardSettingsViewModel(this.ViewModel, this.GenericWindow);
            PMDashBoardSettingVisual window = new PMDashBoardSettingVisual(settingsViewModel, this.GenericWindow);

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
            PMDashBoardVisual wb = this.UiElement as PMDashBoardVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            //this.GenericWindow.AccountId = message.AccountId;
         

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
