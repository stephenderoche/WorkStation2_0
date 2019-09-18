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
namespace TraderVsPM.Client.Plugin
{
    public class TraderVsPMParams : ObservableObject, IWidgetParameters
    {
        
     
       
        private string _securityName;
        private string _deskName;
        private string _accountName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _issuerName;
        private string _broker;
        private string _defaultTheme;
        private Boolean _active;
        private Boolean _crossable;
      
        public TraderVsPMParams()
        {

            this._defaultTheme = "LightGray";
            this._securityName = string.Empty;
            this._deskName = string.Empty;
            this._accountName = string.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
            this._issuerName =  string.Empty;
            this._broker = string.Empty;
            this._active = true;
            this._crossable = true;
        }


        public Boolean Active
        {
            get
            {
                return this._active;
            }

            set
            {
                this._active = value;
                this.RaisePropertyChanged("Active");

            }
        }

        public Boolean Crossable
        {
            get
            {
                return this._crossable;
            }

            set
            {
                this._crossable = value;
                this.RaisePropertyChanged("Crossable");

            }
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
        public string DeskName
        {
            get
            {
                return this._deskName;
            }

            set
            {
                this._deskName = value;
                this.RaisePropertyChanged("DeskName");

            }
        }
     
        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "tradervsPMParameters",
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("deskName", this._deskName),
                                  new XAttribute("accountName", this._accountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                   new XAttribute("issuerName", this._issuerName),
                                   new XAttribute("broker", string.IsNullOrEmpty(Broker)?"":Broker),
                                    new XAttribute("defualtTheme", this._defaultTheme),
                                    new XAttribute("active", this._active),
                                    new XAttribute("crossable", this._crossable)
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
                XAttribute ActiveAttribute = param.Attribute("active");
                XAttribute CrossableAttribute = param.Attribute("crossable");
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
                        this._issuerName = String.Empty;
                    }

                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }


                    if (DeskNameAttribute != null)
                    {
                        this._deskName = (string)DeskNameAttribute;

                    }

                    if (CrossableAttribute != null)
                    {
                        this._crossable = (bool)CrossableAttribute;

                    }

                    if (ActiveAttribute != null)
                    {
                        this._active = (bool)ActiveAttribute;

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
