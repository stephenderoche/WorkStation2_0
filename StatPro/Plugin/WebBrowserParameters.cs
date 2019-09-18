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

        private string section;

        private string accountName;

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

        public string Section
        {
            get
            {
                return this.section;
            }

            set
            {
                this.section = value;
                this.RaisePropertyChanged("Section");
            }
        }

        public string AccountName
        {
            get
            {
                return this.accountName;
            }

            set
            {
                this.accountName = value;
                this.RaisePropertyChanged("AccountName");
            }
        }




        public XElement GetParams()
        {
            if (string.IsNullOrEmpty(this.Title)
                && string.IsNullOrEmpty(this.URL)
                && string.IsNullOrEmpty(this.Section))
            {
                return new XElement("WebBrowserParameters");
            }

           
            XElement paramsElement = new XElement(
                                            "WebBrowserParameters",
                                            new XAttribute("Title", string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title),
                                            new XAttribute("URL", string.IsNullOrEmpty(this.URL) ? string.Empty : this.URL),
                                             new XAttribute("Section", string.IsNullOrEmpty(this.Section) ? string.Empty : this.Section),
                                             new XAttribute("AccountName", string.IsNullOrEmpty(this.AccountName) ? string.Empty : this.AccountName),
                                            new XAttribute("Version", "1.0"));
            return paramsElement;
        }

        public void SetParams(XElement param)
        {
            XAttribute titleAttribute = param.Attribute("Title");
            this.Title = (string)titleAttribute;

            XAttribute UrlAttribute = param.Attribute("URL");
            this.URL = (string)UrlAttribute;

            XAttribute SectionAttribute = param.Attribute("Section");
            this.Section = (string)SectionAttribute;

            XAttribute AccountNameAttribute = param.Attribute("AccountName");
            this.AccountName = (string)AccountNameAttribute;
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
