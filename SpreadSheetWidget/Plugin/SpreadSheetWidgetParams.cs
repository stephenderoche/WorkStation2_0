 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace SpreadSheetWidget.Client.Plugin
{
    public class SpreadSheetWidgetParameters : ObservableObject, IWidgetParameters
    {
       
        private string _accountName;
        private string _path;
        private bool _visible;
       

        public SpreadSheetWidgetParameters()
        {
            
            this._accountName = string.Empty;
            this._path = string.Empty;
            this._visible = true;
        }

        public bool Visible
        {
            get
            {
                return this._visible;
            }

            set
            {
                this._visible = value;
                this.RaisePropertyChanged("Visible");

            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }

            set
            {
                this._path = value;
                this.RaisePropertyChanged("Path");

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

            if(string.IsNullOrEmpty(Path))
            {Path = "";}
           XElement parameters = new XElement(
                                   "SpreadSheetParameters",
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("path", this._path),
                                   new XAttribute("visible", this._visible)
                                  
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
               
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute PathAttribute = param.Attribute("path");
                XAttribute VisibleAttribute = param.Attribute("visible");
             
                try
                {



                    if (VisibleAttribute != null)
                    {
                        this._visible = (bool)VisibleAttribute;

                    }


                    if (PathAttribute != null)
                    {
                        this._path = (string)PathAttribute;

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
