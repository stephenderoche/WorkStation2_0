namespace YieldCurve.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Yield Curve",
        "PM",
        "208995D1-2D3D-4BDD-BF00-6B6565C43FE8",
        "pack://application:,,,/YieldCurve.Client;component/Images/Stocks.png")]

   

    public class YieldCurvePlugin : BasePlugin<YieldCurveWidget, YieldCurveParameters>
    {
     
    }
}
