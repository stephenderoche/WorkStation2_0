namespace Guages.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Gauges",
        "Essentials",
        "BDC1B1D8-E8CE-4F07-83F7-9B1CABBFC812",
        "pack://application:,,,/Guages.Client;component/Images/Speed.png"
        )]

    public class GuagesPlugin : BasePlugin<GaugesWidget, GuagesParameters>
    {
        
    }

  


    
}
   