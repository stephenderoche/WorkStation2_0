namespace OPSDashBoard.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "OPS DashBoard",
        "Operations",
        "7D624AF7-B278-4A1E-A6D7-A226652C23AD",
        "pack://application:,,,/OPSDashBoard.Client;component/Images/SystemTask.png")]





    public class OPSDashBoardPlugin : BasePlugin<OPSDashBoardWidget, OPSDashBoardParameters>
    {
     
    }
}
