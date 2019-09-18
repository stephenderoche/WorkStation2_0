namespace Linedata.Framework.Client.TestWebBrowserWidget.Plugin
{
    using System.ComponentModel.Composition;
    using Linedata.Framework.Client.TestWebBrowserWidget.View;
    using Linedata.Framework.WidgetFrame.ThemesAndStyles;
    using Linedata.Framework.WidgetFrame.PluginBase;

    /// <summary>
    /// Plugin to show up on the client with a picture 
    /// </summary>
    [PluginAttribute(
        "Web browser", 
        "WebBrowser",
        "9E9E533D-E5D1-4DC4-A391-23244B6BB588",
        "pack://application:,,,/WebBrowserWidget;component/Resources/TestWebBrowserWidget.png", 
        "Web browser", 
        CanShowSettings=true)]

    public class WebBrowserPlugin : IPlugin
    {
        private readonly IThemeInfoProvider themeInfo;

        [ImportingConstructor]
        public WebBrowserPlugin(IThemeInfoProvider themeInfo)
        {
            this.themeInfo = themeInfo;
            this.Parameters = new WebBrowserParameters();
        }

        /// <summary>
        /// Gets the Name of the widget
        /// </summary>
        public string Name
        {
            get { return "Web browser"; }
        }

        /// <summary>
        /// Gets the Category of the widget
        /// </summary>
        public string Category
        {
            get { return "WebBrowser"; }
        }

        /// <summary>
        /// To create the Widget
        /// </summary>
        /// <returns>return IGtiWidget</returns>
        public IWidget CreateWidget()
        {
            return new WebBrowserWidget(this.themeInfo);
        }

        public IWidgetParameters Parameters
        {
            get;
            private set;
        }

        public void ShowSettings()
        {
            PluginSettingsDialog dialog = new PluginSettingsDialog((WebBrowserParameters)Parameters);
            dialog.ShowDialog();
        }
    }
}
