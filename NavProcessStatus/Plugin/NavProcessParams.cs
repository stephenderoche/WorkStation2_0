using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace NavProcess.Client.Plugin
{
    public class NavProcessParameters : ObservableObject, IWidgetParameters
    {
     
        private string _accountName;
        private DateTime _startDate;
        private DateTime _endDate;
        string _xml;
   

        public NavProcessParameters()
        {
           
            this._accountName = string.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
            this._xml = "NavProcessStatus.xaml";
         
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
                                   "ProcessStatus",
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                   new XAttribute("xml", this.XML)
                                  
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
                XAttribute XMLAttribute = param.Attribute("xml");
             
                try
                {


                    if (XML != null)
                    {
                        this._xml = (string)XMLAttribute;

                    }

                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }

                    if (EndDateAttribute != null)
                    {
                        this._endDate = (DateTime)EndDateAttribute;

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
