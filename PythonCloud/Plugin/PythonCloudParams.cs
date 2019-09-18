using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace PythonCloud.Client.Plugin
{
    public class PythonCloudParams : ObservableObject, IWidgetParameters
    {
        
     
       
        private string _securityName;
        private string _deskName;
        private bool _1d;
        private bool _5d;
        private bool _1m;
        private bool _3m;
        private bool _6m;
        private bool _YTD;
        private bool _1y;
        private bool _2y;
        private bool _5y;



       
      
        public PythonCloudParams()
        {
           
            this._securityName = string.Empty;
            this._deskName = string.Empty;
            this._1d = false;
            this._5d = false;
            this._1m = false;
            this._3m = false;
            this._6m = false;
            this._YTD = false;
            this._1y = false;
            this._2y = false;
            this._5y = true;


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

        public bool OneDay
        {
            get
            {
                return this._1d;
            }

            set
            {
                this._1d = value;
                this.RaisePropertyChanged("OneDay");

            }
        }
        public bool FiveDay
        {
            get
            {
                return this._5d;
            }

            set
            {
                this._5d = value;
                this.RaisePropertyChanged("FiveDay");

            }
        }
        public bool OneMonth
        {
            get
            {
                return this._1m;
            }

            set
            {
                this._1m = value;
                this.RaisePropertyChanged("OneMonth");

            }
        }
        public bool ThreeMonth
        {
            get
            {
                return this._3m;
            }

            set
            {
                this._3m = value;
                this.RaisePropertyChanged("ThreeMonth");

            }
        }
        public bool SixMonth
        {
            get
            {
                return this._6m;
            }

            set
            {
                this._6m = value;
                this.RaisePropertyChanged("SixMonth");

            }
        }
        public bool YTD
        {
            get
            {
                return this._YTD;
            }

            set
            {
                this._YTD = value;
                this.RaisePropertyChanged("YTD");

            }
        }
        public bool OneYear
        {
            get
            {
                return this._1y;
            }

            set
            {
                this._1y = value;
                this.RaisePropertyChanged("OneYear");

            }
        }
        public bool TwoYear
        {
            get
            {
                return this._2y;
            }

            set
            {
                this._2y = value;
                this.RaisePropertyChanged("TwoYear");

            }
        }
        public bool FiveYear
        {
            get
            {
                return this._5y;
            }

            set
            {
                this._5y = value;
                this.RaisePropertyChanged("FiveYear");

            }
        }

     
        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "SecurityPriceParameters",
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("deskName", this._deskName),
                                   new XAttribute("oned", this._1d),
                                   new XAttribute("fived", this._5d),
                                   new XAttribute("onem", this._1m),
                                   new XAttribute("threem", this._3m),
                                   new XAttribute("sixm", this._6m),
                                   new XAttribute("YTD", this._YTD),
                                   new XAttribute("oney", this._1y),
                                   new XAttribute("twoy", this._2y),
                                    new XAttribute("fivey", this._5y));

            

            return parameters;
        }

        


        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute SecurityNameAttribute = param.Attribute("securityName");
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute onedAttribute = param.Attribute("oned");
                XAttribute fivedAttribute = param.Attribute("fived");
                XAttribute onemAttribute = param.Attribute("onem");
                XAttribute threemAttribute = param.Attribute("threem");
                XAttribute sixmAttribute = param.Attribute("sixm");
                XAttribute YTDdAttribute = param.Attribute("YTD");
                XAttribute oneyAttribute = param.Attribute("oney");
                XAttribute twoyAttribute = param.Attribute("twoy");
                XAttribute fiveyAttribute = param.Attribute("fivey");
               
            
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



                    if (onedAttribute != null)
                    {
                        this._1d = (bool)onedAttribute;
                    }

                    if (fivedAttribute != null)
                    {
                        this._5d = (bool)fivedAttribute;
                    }

                    if (onemAttribute != null)
                    {
                        this._1m = (bool)onemAttribute;
                    }

                    if (threemAttribute != null)
                    {
                        this._3m = (bool)threemAttribute;
                    }

                    if (sixmAttribute != null)
                    {
                        this._6m = (bool)sixmAttribute;
                    }

                    if (YTDdAttribute != null)
                    {
                        this._YTD = (bool)YTDdAttribute;
                    }

                    if (oneyAttribute != null)
                    {
                        this._1y = (bool)oneyAttribute;
                    }

                    if (twoyAttribute != null)
                    {
                        this._2y = (bool)twoyAttribute;
                    }

                    if (fiveyAttribute != null)
                    {
                        this._5y = (bool)fiveyAttribute;
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
