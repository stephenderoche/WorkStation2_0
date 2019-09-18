namespace DataDashBoard.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Data DashBoard",
        "PM",
        "30DCC226-D15F-4EC1-9F6C-A05DBCB5F2B2",
        "pack://application:,,,/DataDashBoard.Client;component/Images/SystemTask.png")]


    public class DataDashBoardPlugin : BasePlugin<DataDashBoardWidget, DataDashBoardParameters>
    {
     
    }
}
