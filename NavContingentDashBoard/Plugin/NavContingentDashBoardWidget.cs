using Linedata.Shared.Widget.Common;
namespace NavContingentDashBoard.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using NavContingentDashBoard.Client.ViewModel;
    using NavContingentDashBoard.Client.View;
    using NavContingentDashBoard.Client.Model;



    using System;


    [Export]

    public class NavContingentDashBoardWidget : WidgetSubscriber, IWidget
    {
       
        private NavContingentDashBoardVisual GenericWindow;
        private NavContingentDashBoardParameters pluginParameters;
        private readonly IReactivePublisher publisher;
     

        [ImportingConstructor]
        public NavContingentDashBoardWidget(IReactivePublisher publisher
           , NavContingentDashBoardModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new NavContingentDashBoardParameters();
            this.pluginParameters = new NavContingentDashBoardParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (NavContingentDashBoardParameters)this.Parameters;


            GenericWindow = new NavContingentDashBoardVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~NavContingentDashBoardWidget()
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


        public NavContingentDashBoardModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            NavContingentDashBoardSettingsViewModel settingsViewModel = new NavContingentDashBoardSettingsViewModel(this.ViewModel, this.GenericWindow);
            ModelHelperSettingVisual window = new ModelHelperSettingVisual(settingsViewModel, this.GenericWindow);

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
            NavContingentDashBoardVisual wb = this.UiElement as NavContingentDashBoardVisual;

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
