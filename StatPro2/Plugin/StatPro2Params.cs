using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace StatPro2.Client.Plugin
{
    public class StatPro2Params : ObservableObject, IWidgetParameters
    {



        private string _accountName;

        private string title;

        private string url;

        private string section;

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
                return this._accountName;
            }

            set
            {
                this._accountName = value;
                this.RaisePropertyChanged("AccountName");

            }
        }
       
      
        public StatPro2Params()
        {

            this._accountName = string.Empty;
            this.title= string.Empty;
            this.section = string.Empty;
            this.url = string.Empty;

        }


     
        public XElement GetParams()
        {

            //if (string.IsNullOrEmpty(this.Title)
            //   && string.IsNullOrEmpty(this.URL)
            //   && string.IsNullOrEmpty(this.Section))
            //{
            //    return new XElement("WebBrowserParameters");
            //}

           XElement parameters = new XElement(
                                   "StatProParameters",
                                   new XAttribute("Title", string.IsNullOrEmpty(this.Title) ? string.Empty : this.Title),
                                   new XAttribute("URL", string.IsNullOrEmpty(this.URL) ? string.Empty : this.URL),
                                   new XAttribute("Section", string.IsNullOrEmpty(this.Section) ? string.Empty : this.Section),
                                   new XAttribute("accountName", string.IsNullOrEmpty(this.AccountName) ? string.Empty : this.AccountName)
                                 );

            

            return parameters;
        }

        


        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute titleAttribute = param.Attribute("Title");
                this.Title = (string)titleAttribute;

                XAttribute UrlAttribute = param.Attribute("URL");
                this.URL = (string)UrlAttribute;

                XAttribute SectionAttribute = param.Attribute("Section");
                this.Section = (string)SectionAttribute;
               
            
                try
                {


                   

                  
                    if (AccountName != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }
                    else
                    {
                        this._accountName = String.Empty;
                    }

                }
                catch (FormatException ex)
                {
                    Debug.WriteLine(string.Format("One of the parameter have wrong format : {0} ", ex.Message));
                }
            }
        }

        public void UpgradeParams(XElement param)
        {
        }

        public XmlSchemaSet GetParamsSchemaSet()
        {
            return null;
        }
    }
}
