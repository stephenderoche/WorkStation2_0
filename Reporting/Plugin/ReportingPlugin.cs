namespace Reporting.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;

    //[Plugin(
    //    "Reporting",
    //    "Essentials",
    //    "798B3F81-E775-49F9-9CD5-B0712765A74B",
    //    "pack://application:,,,/Reporting.Client;component/Images/Analyze.png")]

    [Plugin(
       "Reporting",
       PluginCategories.PortfolioManagement,
       "798B3F81-E775-49F9-9CD5-B0712765A74B",
       "pack://application:,,,/Reporting.Client;component/Images/Analyze.png")]


    public class ReportingPlugin : BasePlugin<ReportingWidget, ReportingParameters>
    {
     
    }
}
