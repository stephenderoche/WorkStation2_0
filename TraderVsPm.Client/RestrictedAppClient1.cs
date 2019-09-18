using System;
using System.AddIn;
using System.Drawing;
using Linedata.Client.AddIn.AppAddIns.LongView;
using Linedata.Client.AddIn.AddInView.V1.Shared;
using Linedata.Framework.Foundation;
using System.Windows;
using Linedata.Client.AddIn.AppAddIns;
using SecurityHistoryApp.Properties;
using System.Collections.Generic;



namespace SecurityHistoryApp.Client
{
    [AddIn(
       "SecurityHistoryApp Application",
       Publisher = "Linedata Services, Inc.")]
    public class RestrictedBrokerAppClient1 : LongViewAddInBase
    {

        /// <summary>
        /// Stores a reference to the created menu item for manipulation.
        /// </summary>
        private IMenuItem m_menuItem;


        /// <summary>
        /// Holds the current instance of the app and will not allow to create another instance
        /// </summary>
        public static string SimpleMenuApp
        {
            get;
            set;
        }

        /// <summary>
        /// LongView core will invoke this internally
        /// </summary>
        /// <param name="menuItems"></param>
        public override void InitMenuItems(IMenuItems menuItems)
        {
            try
            {
                base.InitMenuItems(menuItems);
                Icon icon = null;
                try
                {
                    //icon = new Icon(typeof(SimpleMenuApp.Client.SimpleMenuAppClient1), "AllOrNoneAllocation2.ico");

                   // var bmp = new Bitmap(SimpleMenuAppClient1.SimpleMenuApp.
                    
                }
                catch (Exception ex)
                {
                    this.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.Error));
                }
                this.m_menuItem = menuItems.AddTopLevelMenu("SecurityHistoryApp App", TopLevelMenu.GlobalTools, icon);
                this.m_menuItem.Click += new System.EventHandler<ClickEventArgs>(this.MenuItem_Click);
            }
            catch (Exception ex)
            {
                this.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.Error));
            }
        }

        /// <summary>
        /// This is fired when someone clicks on the menu item associated with the event.
        /// </summary>
        /// <param name="sender">Always is null.</param>
        /// <param name="e">The event args associated with this event. It will typically contain the <see cref="IReportInstance"/>.</param>
        private void MenuItem_Click(object sender, ClickEventArgs e)
        {
            try
            {

            //      if (LongViewApp.ActiveMasterReport.ReportType.ReportTypeGuid != ReportGuids.TraderBlotter)
            //{
            //    MessageService.LogMessage(new ApplicationMessage("This application runs from the blotter", ApplicationMessageType.Alert));
            //    return;
            //}
            //if (LongViewApp.ActiveMasterReport.ReportGrid.CurrentSelectedRow == null)
            //{
            //    MessageService.LogMessage(new ApplicationMessage("Please select a row on the blotter", ApplicationMessageType.Alert));
            //    return;
            //}
            //string accountID = LongViewApp.ActiveMasterReport.ReportGrid.CurrentSelectedRow.GetCellValue("block_id");
            //decimal daccountID = 0;
            //if (!decimal.TryParse(accountID, out daccountID))
            //{
            //    MessageService.LogMessage(new ApplicationMessage("Unable to read the block ID. Please Select a Blocked row", ApplicationMessageType.Alert));
            //    return;
            //}
            
                //Check if instance is already running
                if (SimpleMenuApp != null)
                {
                    this.MessageService.LogMessage(new ApplicationMessage(
                        "There is an instance of Account Level Project already running.",
                        ApplicationMessageType.Error));
                }
                else
                {
                    SimpleMenuApp = "APPRUNNING";
                    RestrictedBrokerClientMain1 clientMain = new RestrictedBrokerClientMain1();
                    clientMain.ClientLongViewAddInBase = this;
                    clientMain.RunSimpleMenuAppTool();
                }


            }
            catch (Exception ex)
            {
                SimpleMenuApp = null;
                this.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.Error));
            }

            SimpleMenuApp = null;
        }

       
       

    }
}