using Linedata.Shared.Widget.Common;
namespace SpreadSheetWidget.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using SpreadSheetWidget.Client.ViewModel;
    using SpreadSheetWidget.Client.View;
    using SpreadSheetWidget.Client.Model;
   


    using System;


    [Export]

    public class SpreadSheetWidgetWidget : WidgetSubscriber, IWidget
    {
       
        private SpreadSheetWidgetVisual GenericWindow;
        private SpreadSheetWidgetParameters pluginParameters;
        private readonly IReactivePublisher publisher;

        public SpreadSheetWidgetWidget(IReactivePublisher publisher, bool suppressCustomMessages = false) : base(publisher, suppressCustomMessages)
        {
        }

        [ImportingConstructor]
        public SpreadSheetWidgetWidget(IReactivePublisher publisher
           , SpreadSheetWidgetModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new SpreadSheetWidgetParameters();
            this.pluginParameters = new SpreadSheetWidgetParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (SpreadSheetWidgetParameters)this.Parameters;


            GenericWindow = new SpreadSheetWidgetVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~SpreadSheetWidgetWidget()
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


        public SpreadSheetWidgetModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            SpreadSheetWidgetViewModel settingsViewModel = new SpreadSheetWidgetViewModel(this.ViewModel, this.GenericWindow);
            SpreadSheetWidgetSettingVisual window = new SpreadSheetWidgetSettingVisual(settingsViewModel, this.GenericWindow);

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
            SpreadSheetWidgetVisual wb = this.UiElement as SpreadSheetWidgetVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
           // this.GenericWindow.AccountId = message.AccountId;
            //this.GenericWindow.get_account_name(Convert.ToString(message.AccountId));

           // this.GenericWindow.UpdateAccoutText();
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
