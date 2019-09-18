namespace ComplianceDashBoard.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Compliance DashBoard",
        "Compliance",
        "83D128FD-C364-42DA-9EC5-6943C5A8D37D",
        "pack://application:,,,/ComplianceDashBoard.Client;component/Images/Compliance.png")]


 


    public class ComplianceDashBoardPlugin : BasePlugin<ComplianceDashBoardWidget, FIDashBoardParameters>
    {
     
    }
}
