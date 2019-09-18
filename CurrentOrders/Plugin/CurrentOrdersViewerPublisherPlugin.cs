namespace CurrentOrders.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Current Orders",
        "PM",
        "F384BFD9-DA61-4211-8695-3D6221DC2AC0",
        "pack://application:,,,/CurrentOrders.Client;component/Images/PurchaseOrder.png")]

  

    public class CurrentOrdersViewerPublisherPlugin : BasePlugin<CurrentOrdersViewerPublisherWidget, CurrentOrdersViewerPublisherParameters>
    {
     
    }
}
