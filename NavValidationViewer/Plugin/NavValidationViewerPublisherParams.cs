using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace NavValidationViewer.Client.Plugin
{
    public class NavValidationViewerPublisherParameters : ObservableObject, IWidgetParameters
    {
        private string _defaultTheme;
        private string _accountName;
        private DateTime _startDate;
        private string _loadHist;
        private string _ControlType;
        string _xml;
        Boolean _focusFail;
        Boolean _focusData;
   

        public NavValidationViewerPublisherParameters()
        {
            this._defaultTheme = "LightGray";
            this._accountName = string.Empty;
            this._startDate = DateTime.Today;

            this._ControlType = string.Empty;
            this._loadHist = string.Empty;
            this._xml = "ValidationView.xaml";
            this._focusData = false;
            this._focusFail = false;
        }


        public string DefaultTheme
        {
            get
            {
                return this._defaultTheme;
            }

            set
            {
                this._defaultTheme = value;
                this.RaisePropertyChanged("DefaultTheme");

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
        public Boolean FocusFail
        {
            get
            {
                return this._focusFail;
            }

            set
            {
                this._focusFail = value;
                this.RaisePropertyChanged("FocusFail");

            }
        }

        public Boolean FocusData
        {
            get
            {
                return this._focusData;
            }

            set
            {
                this._focusData = value;
                this.RaisePropertyChanged("FocusData");

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
        public string ControlType
        {
            get
            {
                return this._ControlType;
            }

            set
            {
                this._ControlType = value;
                this.RaisePropertyChanged("ControlType");

            }
        }

        public string LoadHist
        {
            get
            {
                return this._loadHist;
            }

            set
            {
                this._loadHist = value;
                this.RaisePropertyChanged("LoadHist");

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

            var ct = this._ControlType;
            var lh = this._loadHist;
            var xml = this._xml;

            if (ct == null)
                this._ControlType = string.Empty;
            if (lh == null)
                this._loadHist = string.Empty;
            if (xml == null)
                this._xml = string.Empty;

           XElement parameters = new XElement(
                                   "NavValidationParameters",
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("controlType", this._ControlType),
                                   new XAttribute("loadHist", this._loadHist),
                                   new XAttribute("xml", this.XML),
                                   new XAttribute("focusFail", this._focusFail),
                                   new XAttribute("focusData", this._focusData),
                                   new XAttribute("defualtTheme", this._defaultTheme)
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {
                XAttribute DefualtThemeAttribute = param.Attribute("defualtTheme");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute ControlTypeAttribute = param.Attribute("controlType");
                XAttribute LoadHistAttribute = param.Attribute("loadHist");
                XAttribute XMLAttribute = param.Attribute("xml");
                XAttribute focusFailAttribute = param.Attribute("focusFail");
                XAttribute focusDataAttribute = param.Attribute("focusData");
             
                try
                {

                    if (DefualtThemeAttribute != null)
                    {
                        this._defaultTheme = (string)DefualtThemeAttribute;

                    }

                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }


                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }

                    if (focusFailAttribute != null)
                    {
                        this._focusFail = (bool)focusFailAttribute;

                    }
                    else
                    {
                        this._focusFail = false;
                    }

                    if (focusDataAttribute != null)
                    {
                        this._focusData = (bool)focusDataAttribute;

                    }
                    else
                    {
                        this._focusData = false;
                    }


                    if (LoadHist != null)
                    {
                        this._loadHist = (string)LoadHistAttribute;

                    }
                    else
                    {
                        this._loadHist = String.Empty;
                    }


                    if (ControlType != null)
                    {
                        this._ControlType = (string)ControlTypeAttribute;

                    }
                    else
                    {
                        this._ControlType = String.Empty;
                    }





                    if (XML != null)
                    {
                        this._xml = (string)XMLAttribute;

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
