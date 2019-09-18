namespace Linedata.Framework.Client.TestWebBrowserWidget.Plugin
{
    using System;
    using System.Windows;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.Client.TestWebBrowserWidget.View;
    using Linedata.Framework.Client.TestWebBrowserWidget.ViewModel;
    using Linedata.Framework.WidgetFrame.ThemesAndStyles;


    public sealed class WebBrowserWidget : IWidget
    {
        private readonly IThemeInfoProvider themeInfo;
        private WebBrowserViewModel viewModel;

        public WebBrowserWidget(IThemeInfoProvider themeInfo)
        {
            this.Parameters = new WebBrowserParameters();
            this.themeInfo = themeInfo;

            this.viewModel = new WebBrowserViewModel((WebBrowserParameters)this.Parameters);

            this.UiElement = new WebBrowserView();
            this.UiElement.DataContext = this.viewModel;

            this.ChangeTheme(this.themeInfo.CurrentTheme);
            this.themeInfo.ThemeChangedEvent += new EventHandler<ThemeChangedEventArgs>(this.ThemeChanged);
        }

        public FrameworkElement UiElement
        {
            get;
            private set;
        }

       public CommunicationMode CommunicationMode 
       { 
            get;
           private set;
        }

        public Size DesiredSize
        {
            get
            {
                return new Size(751, 174);
            }
        }

        public IWidgetParameters Parameters
        {
            get;
            private set;
        }

        public bool CanShowSettings
        {
            get { return false; }
        }

        public void ShowSettings()
        {
        }

        public void Dispose()
        {
            WebBrowserView wb = this.UiElement as WebBrowserView;
            if (wb != null && wb.webBrowser != null)
            {
                wb.webBrowser.Dispose();
            }

            //this.themeInfo.ThemeChangedEvent -= this.ThemeChanged;
            GC.SuppressFinalize(this);
        }

        public void ChangeTheme(ThemeEnum theme)
        {
            ////this.viewModel.ApplicationBackgroundBrush = XceedThemes.Instance.ApplicationBackgroundBrush[theme];
            ////this.viewModel.HighlightBackgroundBrush = XceedThemes.Instance.HighlightBackgroundBrush[theme];
            ////this.viewModel.ControlBackgroundBrush = XceedThemes.Instance.ControlBackgroundBrush[theme];
            ////this.viewModel.ControlForegroundBrush = XceedThemes.Instance.ControlForegroundBrush[theme];
            ////this.viewModel.BorderBrush = XceedThemes.Instance.BorderBrush[theme];

            ////if (ThemeEnum.MediaPlayer == theme)
            ////{
            ////    this.viewModel.ControlBrush = XceedThemes.Instance.ControlForegroundBrush[theme];
            ////}
            ////else
            ////{
            ////    this.viewModel.ControlBrush = XceedThemes.Instance.ApplicationBackgroundBrush[theme];
            ////}
        }

        public void InitWidget()
        {
        }

        public WidgetGroups Group { get; set; }

        public string Status { get; set; }

        private void ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            this.ChangeTheme(e.Theme);
        }
    }
}
