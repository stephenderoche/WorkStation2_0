namespace GenericGrid.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Generic Grid",
        "Essentials",
        "C00085ED-E420-4B4B-8484-D9FC7FBD45EB",
        "pack://application:,,,/GenericGrid.Client;component/Images/Metamorphose.png")]


    public class GenericGridViewerPublisherPlugin : BasePlugin<GenericGridViewerPublisherWidget, GenericGridViewerPublisherParameters>
    {
     
    }
}
