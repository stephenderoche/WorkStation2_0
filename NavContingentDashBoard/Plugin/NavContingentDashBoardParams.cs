using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace NavContingentDashBoard.Client.Plugin
{
    public class NavContingentDashBoardParameters : ObservableObject, IWidgetParameters
    {
       
        private string _accountName;
        private string _xml;
        private string _broker;
        private string _brokerReason;
        private DateTime _startDate;
        private DateTime _endDate;


        public NavContingentDashBoardParameters()
        {
            
            this._accountName = string.Empty;
            this._broker = string.Empty;
            this._xml = "DataDashBoard.xaml";
            this._brokerReason = String.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
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
                this.RaisePropertyChanged("BrokerReason");

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
                                   "ModelHelperParameters",
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("xml",string.IsNullOrEmpty(XML)?"":XML),
                                   new XAttribute("broker", string.IsNullOrEmpty(Broker)?"":Broker),
                                   new XAttribute("brokerReason", string.IsNullOrEmpty(BrokerReason) ? "" : BrokerReason),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate)
                                   );
          
            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
               
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute XMLAttribute = param.Attribute("xml");
                XAttribute BrokerAttribute = param.Attribute("broker");
                XAttribute BrokerReasonAttribute = param.Attribute("brokerReason");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute EndDateAttribute = param.Attribute("endDate");
                try
                {



                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }

                    if (EndDateAttribute != null)
                    {
                        this._endDate = (DateTime)EndDateAttribute;

                    }


                    if (XML != null)
                    {
                        this._xml = (string)XMLAttribute;

                    }

                    if (Broker != null)
                    {
                        this._broker = (string)BrokerAttribute;

                    }

                    if (BrokerReason != null)
                    {
                        this._brokerReason = (string)BrokerReasonAttribute;

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
