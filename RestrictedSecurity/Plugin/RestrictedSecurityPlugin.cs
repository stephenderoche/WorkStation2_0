namespace RestrictedSecurity.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Restricted Security",
        "PM",
        "FBF840DD-F91A-45C0-9CFE-BC2AC50A5273",
        "pack://application:,,,/RestrictedSecurity.Client;component/Images/restriction.png")]



    public class RestrictedSecurityPlugin : BasePlugin<RestrictedSecurityWidget, RestrictedSecurityParams>
    {
     
    }
}
