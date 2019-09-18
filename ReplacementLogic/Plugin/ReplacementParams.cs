using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace Replacement.Client.Plugin
{
    public class ReplacementParams : ObservableObject, IWidgetParameters
    {
        
     
       
       
        private string _accountName;
  
        public ReplacementParams()
        {
           
          
            this._accountName = string.Empty;
            


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
                                   "ReplacementParameters",
                                   
                                   new XAttribute("accountName", this._accountName)
                                   
                                   );

            

            return parameters;
        }

        


        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute AccountNameAttribute = param.Attribute("accountName");
               
            
                try
                {


                   

                  
                    if (AccountName != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }
                    else
                    {
                        this._accountName = String.Empty;
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
