using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace RestrictedSecurity.Client.Plugin
{
    public class RestrictedSecurityParams : ObservableObject, IWidgetParameters
    {
        
     
       
        private string _securityName;
        private string _deskName;
        private string _accountName;


       
      
        public RestrictedSecurityParams()
        {
           
            this._securityName = string.Empty;
            this._deskName = string.Empty;
            this._accountName = string.Empty;


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
                                  new XAttribute("accountName", this._accountName));

            

            return parameters;
        }

        


        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute SecurityNameAttribute = param.Attribute("securityName");
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
               
            
                try
                {


                   

                  
                    if (SecurityName != null)
                    {
                        this._securityName = (string)SecurityNameAttribute;

                    }
                    else
                    {
                        this._securityName = String.Empty;
                    }

                    if (DeskName != null)
                    {
                        this._deskName = (string)DeskNameAttribute;

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
