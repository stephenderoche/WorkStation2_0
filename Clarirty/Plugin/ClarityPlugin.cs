namespace Clarity.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Clarity",
        "Analytics",
        "9563CB5D-A571-4966-9036-F3536EA8A2BB",
        "pack://application:,,,/Clarity.Client;component/Images/linedata.png")]


  




    public class ClarityPlugin : BasePlugin<ClarityWidget, ClarityParams>
    {
     
    }
}
