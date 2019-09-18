//using TransactionsViewerAddIn.Client.DetailReport.Plugin;
using Linedata.Framework.Client.SubsPubEvents;
namespace IpoViewerAddIn.Client.DetailReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    //using Linedata.Shared.Widgets.OrderManager.DataContracts;
    using Linedata.Client.AddIn.HostView.AppAddIns;
    using Linedata.Client.AddIn;
    using Linedata.Client;
    using IpoViewerAddIn.Client.DetailReport.Model;

    public class IpoViewerWidget : IWidget
    {
        //private readonly ITabDataSubscriber<BlockedOrderEventArgs> subscriber;
        private ITabDataSubscriber<TabData1EventArgs> subscriber;
        IPOViewerMainWindow TransactinosViewerWindow;

      
        private IPOViewerParameters pluginParameters;
        private IpoViewerModel TransactionViewerModel;
        public IpoViewerWidget(ITabDataSubscriber<TabData1EventArgs> subscriber,
            IPOViewerParameters pluginParameters
            )
        {
            this.pluginParameters = pluginParameters;
            this.subscriber = subscriber;
            this.Parameters = new IPOViewerParameters();
           
        }

        public WidgetGroups Group { get; set; }

        public string Status { get; set; }

        public bool CanShowSettings
        {
            get { return true; }
        }

        public System.Windows.Size DesiredSize
        {
            get { return new Size(700, 300); }
        }

        public void InitWidget()
        {
            

              this.Parameters = new IPOViewerParameters();
            this.Parameters.SetParams(this.pluginParameters.GetParams());


            this.TransactionViewerModel = new IpoViewerModel((IPOViewerParameters)this.Parameters);

             TransactinosViewerWindow = new IPOViewerMainWindow(this.TransactionViewerModel);


            this.UiElement = TransactinosViewerWindow;



            LinedataApp.Instance.ApplicationNotification.RowChanged += ApplicationNotification_RowChanged;

            EventHandler<TabData1EventArgs> handler = new EventHandler<TabData1EventArgs>(this.TabData1Event);
            this.subscriber.Event += handler;
            this.TabData1Event(null, this.subscriber.GetLastPublishedData(this));
        }

        public IWidgetParameters Parameters
        {
            get;
            private set;
        }

        public void ShowSettings()
        {
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


            if( e.NewRow.RowType.ToString() == "Header")
            {
                return;

            }
            if (TransactinosViewerWindow.Follower == "LongView")
            {

                if (e.NewRow.RowType.ToString() == "Header")
                {
                    return;

                }

                var foo = e.ReportInstance.ReportType; //or any object from any class

                if (foo == null)
                {
                    return;
                }


                if (e.ReportInstance.ReportType.DisplayName == "Appraisal") 
                {
                    TransactinosViewerWindow.AccountId = Convert.ToDecimal(e.NewRow.GetCellValue("account_id"));
                TransactinosViewerWindow.AccountName = Convert.ToString(e.NewRow.GetCellValue("account_short_name"));
                TransactinosViewerWindow.UpdateAccoutText();
                TransactinosViewerWindow.se_get_ipo_data();
               

                }

                if (e.ReportInstance.ReportType.DisplayName == "CashBalance")
                {
                    TransactinosViewerWindow.AccountId = Convert.ToDecimal(e.NewRow.GetCellValue("account_id"));
                    TransactinosViewerWindow.AccountName = Convert.ToString(e.NewRow.GetCellValue("account_sh_name"));
                    TransactinosViewerWindow.UpdateAccoutText();
                    TransactinosViewerWindow.se_get_ipo_data();
                 
                }

                if (e.ReportInstance.ReportType.DisplayName == "ManagerBlotter")
                {
                    TransactinosViewerWindow.AccountId = Convert.ToDecimal(e.NewRow.GetCellValue("account_id"));
                    TransactinosViewerWindow.AccountName = Convert.ToString(e.NewRow.GetCellValue("account_sh_name"));
                    TransactinosViewerWindow.UpdateAccoutText();
                    TransactinosViewerWindow.se_get_ipo_data();
              ;
                

                }

                if (e.ReportInstance.ReportType.DisplayName == "Security X-Reference")
                {
                    TransactinosViewerWindow.AccountId = Convert.ToDecimal(e.NewRow.GetCellValue("account_id"));
                    TransactinosViewerWindow.AccountName = Convert.ToString(e.NewRow.GetCellValue("account_sh_name"));
                    TransactinosViewerWindow.UpdateAccoutText();
                    TransactinosViewerWindow.se_get_ipo_data();
                   
                }
                

                if (e.ReportInstance.ReportType.DisplayName == "OrderPreview")
                {
                    TransactinosViewerWindow.AccountId = Convert.ToDecimal(e.NewRow.GetCellValue("account_id"));
                    TransactinosViewerWindow.AccountName = Convert.ToString(e.NewRow.GetCellValue("account_sh_name"));
                    TransactinosViewerWindow.UpdateAccoutText();
                    TransactinosViewerWindow.se_get_ipo_data();
                 
                }


            }
           
        }

        private void TabData1Event(object sender, TabData1EventArgs e)
        {
            if (e != null && e.TabData != null)
            {
                if (TransactinosViewerWindow.Follower == "Account Widget")
                {
                    TransactinosViewerWindow.AccountId = Convert.ToDecimal(e.TabData.PublishedText);
                    TransactinosViewerWindow.AccountName = Convert.ToString(e.TabData.accountname);
                    TransactinosViewerWindow.UpdateAccoutText();
                    TransactinosViewerWindow.se_get_ipo_data();
                  
                }
            }
        }
      
    }
}
