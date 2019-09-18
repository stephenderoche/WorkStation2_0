using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace CurrentOrders.Client.Plugin
{
    public class CurrentOrdersViewerPublisherParameters : ObservableObject, IWidgetParameters
    {
        private string follower;
        private string _accountName;
        private string _defaultTheme;

        public CurrentOrdersViewerPublisherParameters()
        {

            this._defaultTheme = "LightGray"; 
            this.follower = string.Empty;
            this._accountName = string.Empty;
         
        }

        public string DefaultTheme
        {
            get
            {
                return this._defaultTheme;
            }

            set
            {
                this._defaultTheme = value;
                this.RaisePropertyChanged("DefaultTheme");

            }
        }

        public string Follower
        {
            get
            {
                return this.follower;
            }

            set
            {
                this.follower = value;
                this.RaisePropertyChanged("Follower");

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

       
        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "GenericGridParameters",
                                   new XAttribute("follower", this.Follower),
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("defualtTheme", this._defaultTheme)
                                  
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
                XAttribute FollowerAttribute = param.Attribute("follower");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute DefualtThemeAttribute = param.Attribute("defualtTheme");
             
                try
                {




                    if (DefualtThemeAttribute != null)
                    {
                        this._defaultTheme = (string)DefualtThemeAttribute;

                    }

                   
                 
                    if (FollowerAttribute != null)
                    {
                        this.follower = (string)FollowerAttribute;

                    }

                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

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
