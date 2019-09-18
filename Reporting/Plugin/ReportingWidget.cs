
namespace Reporting.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using System.Collections.Generic;
    using System;

    using Reporting.Client.ViewModel;
    using Reporting.Client.View;
    using Reporting.Client.Model;
    using Reporting.Client.Plugin;

    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using ReactiveUI;
    using Splat;
    


    [Export]

    public class ReportingWidget : WidgetSubscriber, IWidget
    {
        private readonly IReactivePublisher publisher;
        private ReportingVisual GenericWindow;
        private ReportingParameters pluginParameters;
        private const string AccountIdColumnName = "account_id";

        [ImportingConstructor]
        public ReportingWidget(IReactivePublisher publisher
           , ReportingModel genericGridViewerMode)
            : base(publisher)
        {

            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new ReportingParameters();
            this.pluginParameters = new ReportingParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (ReportingParameters)this.Parameters;


            GenericWindow = new ReportingVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~ReportingWidget()
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


        public ReportingModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
            ReportingSettingsViewModel settingsViewModel = new ReportingSettingsViewModel(this.ViewModel, this.GenericWindow);
            GenericGridSettingVisual window = new GenericGridSettingVisual(settingsViewModel, this.GenericWindow);

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
            ReportingVisual wb = this.UiElement as ReportingVisual;

           // wb.SaveXML(this.GenericWindow._view.Parameters.Report);

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = message.AccountId;
            this.ViewModel.get_account_name(Convert.ToString(message.AccountId));

            this.GenericWindow.UpdateAccoutText();
           // this.GenericWindow.Get_ChartData();
        }


        //public ICommonDataProvider CommonDataProvider
        //{
        //    get;
        //    set;
        //}

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
