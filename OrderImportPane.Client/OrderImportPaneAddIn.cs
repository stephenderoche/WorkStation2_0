using System.AddIn;
using System.Drawing;
using System.Windows;
using Linedata.Client.AddIn.AddInView.V1.AppAddIns;
using Linedata.Client.AddIn.AddInView.V1.Shared;
using Linedata.Client.AddIn.AppAddIns.LongView;

namespace OrderImportPane.ClientTool
{
    [AddIn(
        "Order Import Pane",
        Publisher = "Linedata Services, Inc.")]
    public class OrderImportPaneAddIn : LongViewAddInBase
    {
        /// <summary>
        /// Stores a reference to the created menu item for manipulation.
        /// </summary>
        private IMenuItem menuItem;

        Icon icon;

        ///// <summary>
        ///// Stores a reference to the pane represented in the LongView frame.
        ///// </summary>
        public IAddInTaskPane pane;

        OrderImportControl _orderImportControl;

        /// <summary>
        /// This will create the basic task pane.
        /// </summary>
        /// <param name="taskPanes">The collection of task panes to populate.</param>
        public override void InitTaskPanes(IAddInTaskPanes taskPanes)
        {
            base.InitTaskPanes(taskPanes);

            _orderImportControl = new OrderImportControl(this);
            pane = taskPanes.Add(_orderImportControl, "Order Importer", 0);
            _orderImportControl.addin = this;
            DefaultMenuItems();

            //this.pane.FrameworkElement.AddToEventRoute(new EventRoute(new RoutedEvent(
        }

        /// <summary>
        /// This will create all menu items associated with this addin.
        /// </summary>
        /// <param name="menuItems">The collection of menu items to populate.</param>

        public override void InitMenuItems(IMenuItems menuItems)
        {
            base.InitMenuItems(menuItems);

            icon = new Icon(typeof(OrderImportPaneAddIn), "data_add.ico");
            {
                menuItem = menuItems.AddTopLevelMenu("Order Import Pane", TopLevelMenu.GlobalView, icon);
                menuItem.Click += MenuItem_Click;
            }
        }

        /// <summary>
        /// This will default the checked and enabled options for the menu items we created.
        /// </summary>
        private void DefaultMenuItems()
        {
            if (menuItem == null)
            {
                MessageBox.Show("DefaultMenuItems before we have menu items");
                return;
            }
            menuItem.Checked = pane.Visible;
            menuItem.Enabled = true;
        }

        /// <summary>
        /// This is fired when someone clicks on the menu item associated with the event.
        /// </summary>
        /// <param name="sender">Always is null.</param>
        /// <param name="e">The event args associated with this event. It will typically contain the <see cref="IReportInstance"/>.</param>
        ///
        private void MenuItem_Click(object sender, ClickEventArgs e)
        {
            bool isVisible = pane.Visible;
            pane.Visible = !isVisible;
            menuItem.Checked = !isVisible;
        }
    }
}