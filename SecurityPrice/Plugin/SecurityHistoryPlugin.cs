namespace SecurityHistory.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Security Price",
        "PM",
        "85105AA5-6B6A-4754-95BE-C9CE53F257CE",
        "pack://application:,,,/SecurityHistory.Client;component/Images/FreddyKrueger.png")]



    public class SecurityHistoryPlugin : BasePlugin<SecurityHistoryWidget, SecurityHistoryParams>
    {
     
    }
}
