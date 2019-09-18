namespace NavValidationViewer.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Nav Validation",
        "Nav",
        "4F1A259E-10F0-4B34-9E99-ABA612F27E4D",
        "pack://application:,,,/NavValidationViewer.Client;component/Images/FacebookLike.png")]




    public class NavValidationViewerPublisherPlugin : BasePlugin<NavValidationViewerPublisherWidget, NavValidationViewerPublisherParameters>
    {
     
    }
}
