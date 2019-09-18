namespace CommissionViewer.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Commission Viewer",
        "Essentials",
        "116EC222-2506-43D9-ADBB-61D54FB0AA05",
        "pack://application:,,,/CommissionViewer.Client;component/Images/SalesPerformance.png")]

   


    public class CommissionViewerPlugin : BasePlugin<CommissionViewerWidget, CommissionViewerParameters>
    {
     
    }
}
