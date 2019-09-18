using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace DynamicGroups.Client.Plugin
{
    public class DynamicGroupsPublisherParameters : ObservableObject, IWidgetParameters
    {
       
        private string _accountName;
        private bool _toReview;
   

        public DynamicGroupsPublisherParameters()
        {
            
            this._accountName = string.Empty;
            this._toReview = true;
         
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

        public bool ToReview
        {
            get
            {
                return this._toReview;
            }

            set
            {
                this._toReview = value;
                this.RaisePropertyChanged("ToReview");

            }
        }

       
        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "GenericGridParameters",
                                   
                                   new XAttribute("accountName", this._accountName),
                                  new XAttribute("toReview", this._toReview)
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
                
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute ToReviewAttribute = param.Attribute("toReview");
             
                try
                {


                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }


                    if (ToReviewAttribute != null)
                    {
                        this._toReview = (bool)ToReviewAttribute;

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
