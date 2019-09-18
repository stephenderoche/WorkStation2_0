namespace NavContingentDashBoard.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Contingent DashBoard",
        "Nav",
        "842B2277-AA0C-4C4D-AE7D-66699239F835",
        "pack://application:,,,/NavContingentDashBoard.Client;component/Images/Navigate.png")]





    public class NavContingentDashBoardPlugin : BasePlugin<NavContingentDashBoardWidget, NavContingentDashBoardParameters>
    {
     
    }
}
