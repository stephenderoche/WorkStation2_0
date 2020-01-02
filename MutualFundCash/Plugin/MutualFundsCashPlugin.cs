namespace MutualFundsCash.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Fund of Funds",
        "PM",
        "3F132EB8-D078-4E57-BF95-C48E9F5656A8",
        "pack://application:,,,/MutualFundsCash.Client;component/Images/cash.png")]





    public class RebalanceInfoSendPlugin : BasePlugin<MutualFundsCashWidget, MutualFundsCashdParams>
    {
     
    }
}
