namespace RebalanceInfo.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Rebalance Info",
        "PM",
        "0317447C-0705-4155-9EBB-D826D189308D",
        "pack://application:,,,/RebalanceInfo.Client;component/Images/cash.png")]

  


    public class RebalanceInfoPlugin : BasePlugin<RebalanceInfoWidget, RebalanceInfoParams>
    {
     
    }
}
