using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.Client.SubsPubEvents;
using System.ComponentModel.Composition;

namespace IpoViewerAddIn.Client.DetailReport
{

    [Plugin(
        "IPO Viewer",
        "Orders",
        "7B0BA079-F756-410F-B2AE-CF7D5DC7069A",
         "pack://application:,,,/IpoViewerAddIn.Client.DetailReport;component/Images/IPO.png")]
    public class TaxlotAccountViewerPlugin : IPlugin
    {
        private IPOViewerParameters pluginParameters;
        private ITabDataSubscriber<TabData1EventArgs> subscriber;

         [ImportingConstructor]
        public TaxlotAccountViewerPlugin(ITabDataSubscriber<TabData1EventArgs> subscriber)
        {
            this.pluginParameters = new IPOViewerParameters();
            this.subscriber = subscriber;
        }

        public string Name
        {
            get
            {
                return "IPO Viewer";
            }
        }

        public string Category
        {
            get
            {
                return "Orders";
            }
        }

        public IWidgetParameters Parameters
        {
            get;
            private set;
        }


        public IWidget CreateWidget()
        {
            return new IpoViewerWidget(this.subscriber, this.pluginParameters);
        }

        public void ShowSettings()
        {
        }
    }
}
