namespace Linedata.Framework.Client.TestWebBrowserWidget.Plugin
{
    using System.ComponentModel;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Linedata.Framework.WidgetFrame.PluginBase;

    /// <summary>
    /// In case to configure the web browser with parameters 
    /// </summary>
    public class WebBrowserParameters : IWidgetParameters, INotifyPropertyChanged
    {
        private string title;

        private string url;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        public string URL
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value;
                this.RaisePropertyChanged("URL");
            }
        }

        public XElement GetParams()
        {
            if (string.IsNullOrEmpty(this.Title)
                && string.IsNullOrEmpty(this.URL))
            {
                return new XElement("WebBrowserParameters");
            }

            XElement paramsElement = new XElement(
                                            "WebBrowserParameters",
                                            new XAttribute("Title", string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title),
                                            new XAttribute("URL", string.IsNullOrEmpty(this.URL) ? string.Empty : this.URL),
                                            new XAttribute("Version", "1.0"));
            return paramsElement;
        }

        public void SetParams(XElement param)
        {
            XAttribute titleAttribute = param.Attribute("Title");
            this.Title = (string)titleAttribute;

            XAttribute UrlAttribute = param.Attribute("URL");
            this.URL = (string)UrlAttribute;
        }

        public XmlSchemaSet GetParamsSchemaSet()
        {
            return null;
        }

        public void UpgradeParams(XElement param)
        {
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
