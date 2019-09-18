using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;
using System.ComponentModel;

namespace Reporting.Client.Plugin
{
    public class ReportingParameters : INotifyPropertyChanged, IWidgetParameters
    {


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private string _accountName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _securityName;
        private string _report;
        private string _broker;
        private string _brokerReason;
     
        private string _deskName;
        string _xml;

        public ReportingParameters()
        {
           
            this._accountName = string.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
            this._securityName = string.Empty;
            this._report = string.Empty;
            this._broker = string.Empty;
            
            this._brokerReason = String.Empty;
          
            this._deskName = string.Empty;
            this._xml = "NavDashboard.xaml";
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


        public string BrokerReason
        {
            get
            {
                return this._brokerReason;
            }

            set
            {
                this._brokerReason = value;
                RaisePropertyChanged("BrokerReason");

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
                                   "ReportingParameters",
                                   new XAttribute("accountName", string.IsNullOrEmpty(AccountName) ? "" : AccountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("broker", string.IsNullOrEmpty(Broker) ? "" : Broker),
                                   new XAttribute("brokerReason", string.IsNullOrEmpty(BrokerReason) ? "" : BrokerReason),
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
                XAttribute BrokerAttribute = param.Attribute("broker");
                XAttribute BrokerReasonAttribute = param.Attribute("brokerReason");
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute ReportAttribute = param.Attribute("report");
                XAttribute XMLAttribute = param.Attribute("xml");
                try
                {
                    if (Broker != null)
                    {
                        this._broker = (string)BrokerAttribute;

                    }

                    if (BrokerReason != null)
                    {
                        this._brokerReason = (string)BrokerReasonAttribute;

                    }

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
