namespace NavProcess.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Process Status",
        "NAV",
        "D6BD2477-18DF-490F-81CC-12FB1250D063",
        "pack://application:,,,/NavProcess.Client;component/Images/Classroom.png")]


    public class NavProcessPlugin : BasePlugin<NavProcessWidget, NavProcessParameters>
    {
     
    }
}
