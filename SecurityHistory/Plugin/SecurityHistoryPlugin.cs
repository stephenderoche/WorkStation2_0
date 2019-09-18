namespace SecurityHistory.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Security History",
        "PM",
        "D2995427-C6B0-45B9-A5D2-AB13E67FFDB3",
        "pack://application:,,,/SecurityHistory.Client;component/Images/history.png")]



    public class SecurityHistoryPlugin : BasePlugin<SecurityHistoryWidget, SecurityHistoryParams>
    {
     
    }
}
