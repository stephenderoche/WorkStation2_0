using Linedata.Shared.Widget.Common;
namespace CommissionViewer.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using CommissionViewer.Client.ViewModel;
    using CommissionViewer.Client.View;
    using CommissionViewer.Client.Model;
  


    using System;


    [Export]

    public class CommissionViewerWidget : WidgetSubscriber, IWidget
    {
       
        private CommissionViewerVisual GenericWindow;
        private CommissionViewerParameters pluginParameters;
        private readonly IReactivePublisher publisher;
     

        [ImportingConstructor]
        public CommissionViewerWidget(IReactivePublisher publisher
           , CommissionViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new CommissionViewerParameters();
            this.pluginParameters = new CommissionViewerParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (CommissionViewerParameters)this.Parameters;


            GenericWindow = new CommissionViewerVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~CommissionViewerWidget()
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


        public CommissionViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            CommissionViewerSettingsViewModel settingsViewModel = new CommissionViewerSettingsViewModel(this.ViewModel, this.GenericWindow);
            CommissionViewerSettingVisual window = new CommissionViewerSettingVisual(settingsViewModel, this.GenericWindow);

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
            CommissionViewerVisual wb = this.UiElement as CommissionViewerVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            this.GenericWindow.get_account_name(Convert.ToString(message.AccountId));

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
