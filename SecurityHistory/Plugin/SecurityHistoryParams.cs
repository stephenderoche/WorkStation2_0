using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;
using DevExpress.Xpf.Core;
namespace SecurityHistory.Client.Plugin
{
    public class SecurityHistoryParams : ObservableObject, IWidgetParameters
    {
        
     
       
        private string _securityName;
        private string _deskName;
        private string _accountName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _issuerName;
        private string _broker;
        private string _defaultTheme;

       
      
        public SecurityHistoryParams()
        {

            this._defaultTheme = "LightGray";
            this._securityName = string.Empty;
            this._deskName = string.Empty;
            this._accountName = string.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
            this._issuerName =  string.Empty;
            this._broker = string.Empty;

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

        public string Broker
        {
            get
            {
                return this._broker;
            }

            set
            {
                this._broker = value;
                this.RaisePropertyChanged("Broker");

            }
        }

        public DateTime EndDate
        {
            get
            {
                return this._endDate;
            }

            set
            {
                this._endDate = value;
                this.RaisePropertyChanged("EndDate");

            }
        }

        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }

            set
            {
                this._startDate = value;
                this.RaisePropertyChanged("StartDate");

            }
        }

        public string SecurityName
        {
            get
            {
                return this._securityName;
            }

            set
            {
                this._securityName = value;
                this.RaisePropertyChanged("SecurityName");

            }
        }

        public string IssuerName
        {
            get
            {
                return this._issuerName;
            }

            set
            {
                this._issuerName = value;
                this.RaisePropertyChanged("IssuerName");

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
                                   "SecurityPriceParameters",
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("deskName", this._deskName),
                                  new XAttribute("accountName", this._accountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                   new XAttribute("issuerName", this._issuerName),
                                   new XAttribute("broker", string.IsNullOrEmpty(Broker)?"":Broker),
                                    new XAttribute("defualtTheme", this._defaultTheme)
                                  );

            

            return parameters;
        }

        


        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute DefualtThemeAttribute = param.Attribute("defualtTheme");
                XAttribute BrokerAttribute = param.Attribute("broker");
                XAttribute SecurityNameAttribute = param.Attribute("securityName");
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute EndDateAttribute = param.Attribute("endDate");
                XAttribute IssuerNameAttribute = param.Attribute("issuerName");
            
                try
                {

                    if (DefualtThemeAttribute != null)
                    {
                        this._defaultTheme = (string)DefualtThemeAttribute;

                    }

                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }

                    if (EndDateAttribute != null)
                    {
                        this._endDate = (DateTime)EndDateAttribute;

                    }

                    if (Broker != null)
                    {
                        this._broker = (string)BrokerAttribute;

                    }

                  
                    if (SecurityName != null)
                    {
                        this._securityName = (string)SecurityNameAttribute;

                    }
                    else
                    {
                        this._securityName = String.Empty;
                    }

                    if (IssuerName != null)
                    {
                        this._issuerName = (string)IssuerNameAttribute;

                    }
                    else
                    {
                        this._deskName = String.Empty;
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
