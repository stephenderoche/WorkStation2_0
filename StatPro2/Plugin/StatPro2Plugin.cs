namespace StatPro2.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Stat Pro",
        "Analytics",
        "B1D94661-B804-4D28-B116-FCCAEA7F0D6B",
        "pack://application:,,,/StatPro2.Client;component/Images/statpro.png")]






    public class StatPro2Plugin : BasePlugin<StatPro2Widget, StatPro2Params>
    {
     
    }
}
