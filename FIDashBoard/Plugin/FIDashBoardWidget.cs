using Linedata.Shared.Widget.Common;
namespace FIDashBoard.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using FIDashBoard.Client.ViewModel;
    using FIDashBoard.Client.View;
    using FIDashBoard.Client.Model;
  


    using System;


    [Export]

    public class FIDashBoardWidget : WidgetSubscriber, IWidget
    {
       
        private FIDashBoardVisual GenericWindow;
        private FIDashBoardParameters pluginParameters;
        private readonly IReactivePublisher publisher;

        public FIDashBoardWidget(IReactivePublisher publisher, bool suppressCustomMessages = false) : base(publisher, suppressCustomMessages)
        {
        }

        [ImportingConstructor]
        public FIDashBoardWidget(IReactivePublisher publisher
           , FIDashBoardModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new FIDashBoardParameters();
            this.pluginParameters = new FIDashBoardParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (FIDashBoardParameters)this.Parameters;


            GenericWindow = new FIDashBoardVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~FIDashBoardWidget()
        {
          
            this.Dispose(false);
        }



        public bool CanShowSettings
        {
            get { return false; }
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


        public FIDashBoardModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            FIDashBoardSettingsViewModel settingsViewModel = new FIDashBoardSettingsViewModel(this.ViewModel, this.GenericWindow);
            FIDashBoardSettingVisual window = new FIDashBoardSettingVisual(settingsViewModel, this.GenericWindow);

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
            FIDashBoardVisual wb = this.UiElement as FIDashBoardVisual;

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
