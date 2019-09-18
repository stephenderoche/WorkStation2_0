using Linedata.Framework.Client.SubsPubEvents;
namespace OrderImportPane.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Client.AddIn.HostView.AppAddIns;
    using Linedata.Client.AddIn;
    using Linedata.Client;
    using Linedata.Client.Dashboard.DashboardBridge;
    using Linedata.Client.AddIn.AppAddIns.LongView;
    using Linedata.Client.AddIn.AddInView.V1.Shared;
    using OrderImportPane.Client.Model;
    //using OrderImportPane.Client.View;

    public class OrderImportViewerWidget :  IWidget
    {
      
        OrderImportControl TopSecuritiesViewerWindow;

        private OrderImportPaneViewerParameters pluginParameters;
        private OrderImportPaneViewModel TopSecuritiesViewModel;
    
      


      



        public OrderImportViewerWidget(
            OrderImportPaneViewerParameters pluginParameters
            )
        {
            this.pluginParameters = pluginParameters;
         
            this.Parameters = new OrderImportPaneViewerParameters();

       
        }

        public WidgetGroups Group { get; set; }

        public string Status { get; set; }

        public bool CanShowSettings
        {
            get { return false; }
        }

        public System.Windows.Size DesiredSize
        {
            get { return new Size(700, 300); }
        }

        public void InitWidget()
        {

            this.Parameters = new OrderImportPaneViewerParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());


            this.TopSecuritiesViewModel = new OrderImportPaneViewModel((OrderImportPaneViewerParameters)this.Parameters);

            TopSecuritiesViewerWindow = new OrderImportControl( );
           
            this.UiElement = TopSecuritiesViewerWindow;
           
           
        }

      

        public IWidgetParameters Parameters
        {
             get;
            private set;
        }

        public void ShowSettings()


        {

            //   NavSubMessageViewerSettingViewModel settingsViewModel = new NavSubMessageViewerSettingViewModel(this.TopSecuritiesViewModel, this.TopSecuritiesViewerWindow);
           
            //     NavSubMessageViewerSettingView window = new NavSubMessageViewerSettingView(settingsViewModel, this.TopSecuritiesViewerWindow);

            //window.Owner = Application.Current.MainWindow;
            //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //window.ShowDialog();
        }

        public System.Windows.FrameworkElement UiElement
        {
            get;
            private set;
        }

        public void Dispose()
        {
        }

        void ApplicationNotification_RowChanged(object sender, RowChangedEventArgs e)
        {

           


        }

        private void TabData1Event(object sender, TabData1EventArgs e)
        {
            if (e != null && e.TabData != null)
            {
                //if (TopSecuritiesViewerWindow.Follower == "Account Widget")
                //{
                //    if (e.TabData.widget == "Message Viewer")
                //    {
                //        if (e.TabData.widget == TopSecuritiesViewerWindow.Followingwidget)
                //        {
                //            TopSecuritiesViewerWindow.LinkID = Convert.ToInt32(e.TabData.link_id);
                           
                //            TopSecuritiesViewerWindow.Get_ChartData();
                //        }
                //    }
                   
                //}
            }
        }
      
    }
}
