namespace FIDashBoard.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "FI DashBoard",
        "PM",
        "5524A831-55FC-4B5D-951A-98D2DA64C7A1",
        "pack://application:,,,/FIDashBoard.Client;component/Images/Bonds.ico")]


   

    public class FIDashBoardPlugin : BasePlugin<FIDashBoardWidget, FIDashBoardParameters>
    {
     
    }
}
