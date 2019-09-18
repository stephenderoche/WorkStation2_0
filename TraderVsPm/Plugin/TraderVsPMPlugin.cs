namespace TraderVsPM.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Trade Vs. PM",
        "PM",
        "E3FBF1C0-AEA0-4B2F-9F71-1C08567FA2EB",
        "pack://application:,,,/TraderVsPM.Client;component/Images/Resize.png")]




    public class SecurityHistoryPlugin : BasePlugin<TraderVsPMWidget, TraderVsPMParams>
    {
     
    }
}
