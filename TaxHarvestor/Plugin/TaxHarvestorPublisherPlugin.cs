namespace TaxHarvestor.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Tax Harvesting",
        "PM",
        "6AF62D8F-6DFC-4961-BC75-D75AED55540A",
        "pack://application:,,,/TaxHarvestor.Client;component/Images/Harvestor.png")]



    public class TaxHarvestorPublisherPlugin : BasePlugin<TaxHarvestorPublisherWidget, TaxHarvestorPublisherParameters>
    {
     
    }
}
