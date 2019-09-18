using Linedata.Shared.Widget.Common;
namespace OPSDashBoard.Client.Plugin
{
    using System.ComponentModel.Composition;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
 
   
    using OPSDashBoard.Client.ViewModel;
    using OPSDashBoard.Client.View;
    using OPSDashBoard.Client.Model;
  

    using System;
    using System.Threading;


    [Export]

    public class OPSDashBoardWidget : WidgetSubscriber, IWidget
    {
       
        private OPSDashBoardVisual GenericWindow;
        private OPSDashBoardParameters pluginParameters;
        private readonly IReactivePublisher publisher;
     

        [ImportingConstructor]
        public OPSDashBoardWidget(IReactivePublisher publisher
           , OPSDashBoardModel genericGridViewerMode)
            : base(publisher)
        {
            
            this.ViewModel = genericGridViewerMode;
            this.ViewModel.ParentWidget = this;


            this.Parameters = new OPSDashBoardParameters();
            this.pluginParameters = new OPSDashBoardParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());
            this.ViewModel.Parameters = (OPSDashBoardParameters)this.Parameters;


            GenericWindow = new OPSDashBoardVisual(this.ViewModel);
            this.UiElement = GenericWindow;
            this.publisher = publisher;
            
        }

      
        ~OPSDashBoardWidget()
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


        public OPSDashBoardModel ViewModel { get; private set; }

        public IWidgetParameters Parameters { get; private set; }

        public void ShowSettings()
        {
          
         
        }

        public FrameworkElement UiElement
        {
            get;
            private set;
        }
        public void  Dispose()
        {
            OPSDashBoardVisual wb = this.UiElement as OPSDashBoardVisual;

            wb.SaveXML();

            GC.SuppressFinalize(this);
        }

     

        public void Handle(WidgetMessages.AccountChanged message)
        {
            this.ViewModel.PublishedAccount = message.AccountId;
            this.ViewModel.AccountId = Convert.ToInt32(message.AccountId);
            //this.GenericWindow.get_account_name(Convert.ToString(message.AccountId));

            //this.GenericWindow.UpdateAccoutText();
          
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
