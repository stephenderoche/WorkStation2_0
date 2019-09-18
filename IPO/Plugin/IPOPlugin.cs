namespace IPO.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "IPO",
        "PM",
        "FCD21793-9A2F-49AB-A718-0C3697FD5458",
        "pack://application:,,,/IPO.Client;component/Images/SystemTask.png")]




    public class IPOPlugin : BasePlugin<IPOWidget, DataDashBoardParameters>
    {
     
    }
}
