using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace MutualFundsCash.Client.Plugin
{
    public class MutualFundsCashdParams : ObservableObject, IWidgetParameters
    {
        
     
       
        private string _securityName;
        private string _deskName;
        private string _accountName;
        private string _defaultTheme;
        private bool _includeOrders;
        private bool _allfunds;



        public MutualFundsCashdParams()
        {
            this._defaultTheme = "LightGray";
            this._securityName = string.Empty;
            this._deskName = string.Empty;
            this._accountName = string.Empty;
            this._includeOrders = true;
            this._allfunds = true;


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

        public bool IncludeOrders
        {
            get
            {
                return this._includeOrders;
            }

            set
            {
                this._includeOrders = value;
                this.RaisePropertyChanged("IncludeOrders");

            }
        }

        public bool Allfunds
        {
            get
            {
                return this._allfunds;
            }

            set
            {
                this._allfunds = value;
                this.RaisePropertyChanged("Allfunds");

            }
        }

        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                   "SecurityPriceParameters",
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("deskName", this._deskName),
                                  new XAttribute("accountName", this._accountName),
                                  new XAttribute("defualtTheme", this._defaultTheme),
                                    new XAttribute("includeOrders", this.IncludeOrders),
                                    new XAttribute("allFunds", this.Allfunds)
                                  );

            

            return parameters;
        }

        


        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute DefualtThemeAttribute = param.Attribute("defualtTheme");
                XAttribute SecurityNameAttribute = param.Attribute("securityName");
                XAttribute DeskNameAttribute = param.Attribute("deskName");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute IncludeOrdersAttribute = param.Attribute("includeOrders");
                XAttribute AllFundsAttribute = param.Attribute("allFunds");

                try
                {


                    if (DefualtThemeAttribute != null)
                    {
                        this._defaultTheme = (string)DefualtThemeAttribute;

                    }

                  
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


                    if (IncludeOrdersAttribute != null)
                    {
                        this._includeOrders = (bool)IncludeOrdersAttribute;

                    }

                    if (AllFundsAttribute != null)
                    {
                        this._allfunds = (bool)AllFundsAttribute;

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
