namespace SpreadSheetWidget.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

    [Plugin(
        "Spreadsheet",
        "Essentials",
        "CD0CE719-01FF-4D8C-A121-0A9FD255B49C",
        "pack://application:,,,/SpreadSheetWidget;component/Images/Spreadsheet.png")]


   

    public class SpreadSheetWidgetPlugin : BasePlugin<SpreadSheetWidgetWidget, SpreadSheetWidgetParameters>
    {
     
    }
}
