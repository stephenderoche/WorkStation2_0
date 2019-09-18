namespace AccountTreeCashViewer.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Account Tree",
        "PUblishers",
        "29356EE9-5730-4622-8D42-3ABEB1D38348",
        "pack://application:,,,/AccountTreeCashViewer.Client;component/Images/OakTree.png"
        )]


    public class TestSamplePlugin : BasePlugin<AccountTreeCashViewerWidget, AccountTreeCashViewerParameters>
    {
        
    }

  


    
}
   