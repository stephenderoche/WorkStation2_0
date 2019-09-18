namespace RebalanceInfoSend.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Rebalance Info Send",
        "PM",
        "0F7C2447-AA28-4D9E-A515-EE2BD860C25E",
        "pack://application:,,,/RebalanceInfoSend.Client;component/Images/cash.png")]

 



    public class RebalanceInfoSendPlugin : BasePlugin<RebalanceInfoSendWidget, RebalanceInfoSendParams>
    {
     
    }
}
