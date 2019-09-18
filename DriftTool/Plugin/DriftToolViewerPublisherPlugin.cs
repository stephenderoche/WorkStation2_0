namespace ExceptionManagement.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Exception Managment",
        "PM",
        "98F5C57B-4520-4445-BDA3-3DE272314147",
        "pack://application:,,,/ExceptionManagement.Client;component/Images/Exception.png")]


    public class DriftToolViewerPublisherPlugin : BasePlugin<DriftToolViewerPublisherWidget, DriftToolViewerPublisherParameters>
    {
     
    }
}
