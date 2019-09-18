using Linedata.Shared.Widget.Common;
namespace CashFlow.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;

    using CashFlow.Client.ViewModel;
    using CashFlow.Client.View;
    using CashFlow.Client.Model;
    using CashFlow.Client.Plugin;


    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using ReactiveUI;
    using Splat;
    

    using System;


    [Export]

    public class CashFlowWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private CashFlowVisual GenericWindow;
        private CashFlowParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public CashFlowWidget(IReactivePublisher publisher
           , CashFlowViewerModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new CashFlowParameters();
            this.pluginParameters = new CashFlowParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (CashFlowParameters)this.Parameters;


            GenericWindow = new CashFlowVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~CashFlowWidget()
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


        public CashFlowViewerModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            CashFlowSettingsViewModel settingsViewModel = new CashFlowSettingsViewModel(this.ViewModel, this.GenericWindow);
            CashFlowSettingVisual window = new CashFlowSettingVisual(settingsViewModel, this.GenericWindow);

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
            CashFlowVisual wb = this.UiElement as CashFlowVisual;

           wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            this.GenericWindow.get_account_name(Convert.ToString(message.AccountId));

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
