using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace TaxHarvestor.Client.Plugin
{
    public class TaxHarvestorPublisherParameters : ObservableObject, IWidgetParameters
    {
        private string follower;
        private string _accountName;
   

        public TaxHarvestorPublisherParameters()
        {
            this.follower = string.Empty;
            this._accountName = string.Empty;
         
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
                                   new XAttribute("accountName", this._accountName)
                                  
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
                XAttribute FollowerAttribute = param.Attribute("follower");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
              
             
                try
                {


                 

                  

                   
                 
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
