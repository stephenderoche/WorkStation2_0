namespace PythonCloud.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Python",
        "Cloud",
        "7814944E-6FCE-4869-AEB9-6BFB7E0126DE",
        "pack://application:,,,/PythonCloud.Client;component/Images/cloud.png")]





    public class PythonCloudPlugin : BasePlugin<PythonCloudWidget, PythonCloudParams>
    {
     
    }
}
