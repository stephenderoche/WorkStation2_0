namespace TraderVsPM.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Framework.WidgetFrame.MvvmFoundation;
    using System.Xml.Schema;
    using System.Xml.Linq;
    using System.Diagnostics;
   

    public class TraderVsPmParameters :  ObservableObject, IWidgetParameters
    {
        
        private string _accountName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _securityName;
        private string _issuerName;
        private string _deskName;
        private Boolean _active;
        public TraderVsPmParameters()
        {
          
            this._accountName = string.Empty;
            this._startDate = (DateTime.Today) ;
            this._endDate = DateTime.Today;
            this._securityName = string.Empty;
            this._issuerName = string.Empty;
            this._deskName = string.Empty;
            this._active = true;
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

        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "TraderVsHistoryParameters",
                                   
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("issuerName", this._issuerName),
                                   new XAttribute("deskName", this._deskName),
                                   new XAttribute("active", this._active)
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
               
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute EndDateAttribute = param.Attribute("endDate");
                XAttribute SecurityNameAttribute = param.Attribute("securityName");
                XAttribute IssuerNameAttribute = param.Attribute("issuerName");
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute ActiveAttribute = param.Attribute("active");
                try
                {

                    if (ActiveAttribute != null)
                    {
                        this._active = (bool)ActiveAttribute;

                    }
                    else
                    {
                        this._deskName = String.Empty;
                    }

                    if (DeskName != null)
                    {
                        this._deskName = (string)DeskNameAttribute;

                    }
                    else
                    {
                        this._deskName = String.Empty;
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

                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }

                    if (EndDateAttribute != null)
                    {
                        this._endDate = (DateTime)EndDateAttribute;

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
 