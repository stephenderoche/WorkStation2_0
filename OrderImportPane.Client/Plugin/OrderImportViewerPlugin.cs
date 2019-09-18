using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.Client.SubsPubEvents;
using System.ComponentModel.Composition;
using Linedata.Client.Dashboard.DashboardBridge;
using Linedata.Client.AddIn.AppAddIns.LongView;


namespace OrderImportPane.Client
{
    [Plugin(
        "Orders",
        "Order Import",
        "0FCD653E-781A-41C4-A2B0-8E1D57B2760C",
        "pack://application:,,,/OrderImportPane.Client;component/Images/Import.png")]
    public class OrderImportViewerPlugin : IPlugin  
    {
       
      
        private OrderImportPaneViewerParameters pluginParameters;
     

         [ImportingConstructor]
        public OrderImportViewerPlugin()
        {
            
            this.pluginParameters = new OrderImportPaneViewerParameters();
          
        }

        public string Name
        {
            get
            {
                return "Order Import";
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
            return new OrderImportViewerWidget( this.pluginParameters); 
        }

        public void ShowSettings()
        {
        }
    }
}
