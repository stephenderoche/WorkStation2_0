using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace DataDashBoard.Client.Plugin
{
    public class DataDashBoardParameters : ObservableObject, IWidgetParameters
    {
       
        private string _accountName;
        private string _xml;

        public DataDashBoardParameters()
        {
            
            this._accountName = string.Empty;
            this._xml = "DataDashBoard.xaml";
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

            if(string.IsNullOrEmpty(XML))
            {XML = "";}
           XElement parameters = new XElement(
                                   "DataDashBoardParameters",
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("xml", XML)
                                  
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
               
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute XMLAttribute = param.Attribute("xml");
             
                try
                {




                    if (XML != null)
                    {
                        this._xml = (string)XMLAttribute;

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
