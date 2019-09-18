namespace Replacement.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Replacement",
        "PM",
        "BBE14126-3BF9-4618-91A7-94ADC07CADAC",
        "pack://application:,,,/Replacement.Client;component/Images/replace.png")]




    public class ReplacementPlugin : BasePlugin<ReplacementWidget, ReplacementParams>
    {
     
    }
}
