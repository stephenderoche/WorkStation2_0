namespace PMDashBoard.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "PM DashBoard",
        "PM",
        "7B4A9152-55AD-40CF-A3B8-8659321B6C63",
        "pack://application:,,,/PMDashBoard.Client;component/Images/Dashboard.ico")]

 

    public class ModelHelperPlugin : BasePlugin<PMDashBoardWidget, PMDashBoardParameters>
    {
     
    }
}
