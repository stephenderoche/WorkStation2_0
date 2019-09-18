using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace CashFlow.Client.Plugin
{
    public class CashFlowParameters : ObservableObject, IWidgetParameters
    {
        private string follower;
        private string _accountName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _securityName;
        private string _report;
     
        private string _deskName;
        string _xml;

        public CashFlowParameters()
        {
            this.follower = string.Empty;
            this._accountName = string.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
            this._securityName = string.Empty;
            this._report = string.Empty;
          
            this._deskName = string.Empty;
            this._xml = "CashFlow.xaml";
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

        public string Report
        {
            get
            {
                return this._report;
            }

            set
            {
                this._report = value;
                this.RaisePropertyChanged("Report");

            }
        }


        public string XML
        {
            get
            {
                return this._xml;
            }

            set
            {
                this._xml = value;
                this.RaisePropertyChanged("XML");

            }
        }
        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "CashFlowParameters",
                                   new XAttribute("follower", this.Follower),
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("deskName", this._deskName),
                                   new XAttribute("report", this._report),
                                   new XAttribute("xml", this.XML)
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
                XAttribute FollowerAttribute = param.Attribute("follower");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute EndDateAttribute = param.Attribute("endDate");
                XAttribute SecurityNameAttribute = param.Attribute("securityName");
              
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute ReportAttribute = param.Attribute("report");
                XAttribute XMLAttribute = param.Attribute("xml");
                try
                {


                    if (Report != null)
                    {
                        this._report = (string)ReportAttribute;

                    }
                    else
                    {
                        this._report = String.Empty;
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


                  

                    if (XML != null)
                    {
                        this._xml = (string)XMLAttribute;

                    }
                    if (FollowerAttribute != null)
                    {
                        this.follower = (string)FollowerAttribute;

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
