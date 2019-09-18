namespace LinkCash.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Cash Linking",
        "Trading",
        "E5A5B142-3D2A-4842-B38F-1C1956B94C88",
        "pack://application:,,,/LinkCash.Client;component/Images/cash.png")]




    public class LinkCashPlugin : BasePlugin<LinkCashWidget, LinkCashParams>
    {
     
    }
}
