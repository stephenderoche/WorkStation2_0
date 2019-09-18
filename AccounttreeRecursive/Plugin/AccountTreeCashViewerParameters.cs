using Linedata.Framework.WidgetFrame.PluginBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Schema;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Serialization;

namespace AccountTreeCashViewer.Plugin
{
    public class AccountTreeCashViewerParameters : ObservableObject, IWidgetParameters
    {
        private string _accountName;
        private string _tree_type;
        private DateTime _startDate;
        private string _loadHist;
        private string _ControlType;
        Boolean _focusFail;
        private string _defaultTheme;

        public AccountTreeCashViewerParameters()
        {

            this._accountName = string.Empty;
            this._tree_type = "Simple";
            this._startDate = DateTime.Today;
            this._ControlType = string.Empty;
            this._loadHist = string.Empty;
            this._focusFail = false;
            this._defaultTheme = "LightGray";

          
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

        public string Tree_type
        {
            get
            {
                return this._tree_type;
            }

            set
            {
                this._tree_type = value;
                this.RaisePropertyChanged("Tree_type");

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


        public XElement GetParams()
        {

            var ct = this._ControlType;
            var lh = this._loadHist;

            if (ct == null)
                this._ControlType = string.Empty;
            if (lh == null)
                this._loadHist = string.Empty;

            XElement parameters = new XElement(
                                   "AccountTree",
                                   new XAttribute("startDate", this._startDate),
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("treetype", this._tree_type),
                                     new XAttribute("controlType", this._ControlType),
                                   new XAttribute("loadHist", this._loadHist),
                                   new XAttribute("focusFail", this._focusFail),
                                   new XAttribute("defualtTheme", this._defaultTheme)
                                   );

            

            return parameters;
          
        }

        public System.Xml.Schema.XmlSchemaSet GetParamsSchemaSet()
        {
            return null;
        }

        public void SetParams(XElement param)
        {
            if (null != param )
            {
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute TreeTypeAttribute = param.Attribute("treetype");
                XAttribute ControlTypeAttribute = param.Attribute("controlType");
                XAttribute LoadHistAttribute = param.Attribute("loadHist");
                XAttribute focusFailAttribute = param.Attribute("focusFail");
                XAttribute DefualtThemeAttribute = param.Attribute("defualtTheme");
                try
                {

                    if (DefualtThemeAttribute != null)
                    {
                        this._defaultTheme = (string)DefualtThemeAttribute;

                    }

                    if (focusFailAttribute != null)
                    {
                        this._focusFail = (bool)focusFailAttribute;

                    }
                    else
                    {
                        this._focusFail = false;
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

                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }

                    if (TreeTypeAttribute != null)
                    {
                        this._tree_type = (string)TreeTypeAttribute;

                    }




                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }



                }
                catch (FormatException ex)
                {
                    Debug.WriteLine(string.Format("One of the parameter have wrong format : {0} ", ex.Message));
                }

            }

        }

        public void UpgradeParams(System.Xml.Linq.XElement param)
        {
        }
    }
}
