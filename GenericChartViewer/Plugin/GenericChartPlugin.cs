namespace GenericChart
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Generic Chart",
        "PM",
        "11E38071-2D05-4C93-8C46-D986E04446A5",
        "pack://application:,,,/GenericChart;component/Images/Swatch.png"
        )]
    public class GenericChartPlugin : BasePlugin<GenericChartWidget, GenericChartParameters>
    {
        
    }

  


    
}
   