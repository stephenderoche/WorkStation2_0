using Linedata.Shared.Widget.Common;
namespace ModelHelper.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using ModelHelper.Client.ViewModel;
    using ModelHelper.Client.View;
    using ModelHelper.Client.Model;



    using System;


    [Export]

    public class CommissionViewerWidget : WidgetSubscriber, IWidget
    {
       
        private ModelHelperVisual GenericWindow;
        private ModelHelperParameters pluginParameters;
        private readonly IReactivePublisher publisher;
     

        [ImportingConstructor]
        public CommissionViewerWidget(IReactivePublisher publisher
           , ModelHelperModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new ModelHelperParameters();
            this.pluginParameters = new ModelHelperParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (ModelHelperParameters)this.Parameters;


            GenericWindow = new ModelHelperVisual(this.ViewModel);
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


        public ModelHelperModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            ModelHelperSettingsViewModel settingsViewModel = new ModelHelperSettingsViewModel(this.ViewModel, this.GenericWindow);
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
            ModelHelperVisual wb = this.UiElement as ModelHelperVisual;

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
