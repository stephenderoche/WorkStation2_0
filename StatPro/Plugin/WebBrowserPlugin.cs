namespace Linedata.Framework.Client.TestWebBrowserWidget.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;
   

    [Plugin(
        "StatPro", 
        "Analytics",
        "02D87DA8-6737-45AE-9795-F0078E82DFE0",
        "pack://application:,,,/Linedata.Framework.Client.TestWebBrowserWidget;component/Images/statpro.png", 
        "Web browser", 
        CanShowSettings=true)]

    

   
    public class WebBrowserPlugin : BasePlugin<WebBrowserWidget, WebBrowserParameters>
    {
        
    }


    //public class WebBrowserPlugin : IPlugin
    //{
    //    private readonly IThemeInfoProvider themeInfo;

    //    [ImportingConstructor]
    //    public WebBrowserPlugin(IThemeInfoProvider themeInfo)
    //    {
    //        this.themeInfo = themeInfo;
    //        this.Parameters = new WebBrowserParameters();
    //    }

    //    /// <summary>
    //    /// Gets the Name of the widget
    //    /// </summary>
    //    public string Name
    //    {
    //        get { return "Web browser"; }
    //    }

    //    /// <summary>
    //    /// Gets the Category of the widget
    //    /// </summary>
    //    public string Category
    //    {
    //        get { return "WebBrowser"; }
    //    }

    //    /// <summary>
    //    /// To create the Widget
    //    /// </summary>
    //    /// <returns>return IGtiWidget</returns>
    //    public IWidget CreateWidget()
    //    {
    //        return new WebBrowserWidget(this.themeInfo);
    //    }

    //    public IWidgetParameters Parameters
    //    {
    //        get;
    //        private set;
    //    }

    //    public void ShowSettings()
    //    {
    //        PluginSettingsDialog dialog = new PluginSettingsDialog((WebBrowserParameters)Parameters);
    //        dialog.ShowDialog();
    //    }
    //}
}
