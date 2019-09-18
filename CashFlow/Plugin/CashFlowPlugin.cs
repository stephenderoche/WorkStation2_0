namespace CashFlow.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Cash Flow",
        "PM",
        "E0C7E29E-C2BA-447F-BBFE-41E43DB2EB23",
        "pack://application:,,,/CashFlow.Client;component/Images/Input.png")]

   

    public class CashFlowPlugin : BasePlugin<CashFlowWidget, CashFlowParameters>
    {
     
    }
}
