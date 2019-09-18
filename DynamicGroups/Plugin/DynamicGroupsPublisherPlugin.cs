namespace DynamicGroups.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Dynamic Groups",
        "PM",
        "AEBDC73A-D243-4321-A87D-2FC85F82D328",
        "pack://application:,,,/DynamicGroups.Client;component/Images/FundAccounting.png")]

 


    public class DynamicGroupsPublisherPlugin : BasePlugin<DynamicGroupsPublisherWidget, DynamicGroupsPublisherParameters>
    {
     
    }
}
