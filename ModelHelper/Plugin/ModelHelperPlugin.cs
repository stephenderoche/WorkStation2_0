namespace ModelHelper.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Model Helper",
        "PM",
        "41639F00-03AA-4BB0-A816-FBBCD5A526C1",
        "pack://application:,,,/ModelHelper.Client;component/Images/OnlineSupport.png")]



    public class ModelHelperPlugin : BasePlugin<CommissionViewerWidget, ModelHelperParameters>
    {
     
    }
}
