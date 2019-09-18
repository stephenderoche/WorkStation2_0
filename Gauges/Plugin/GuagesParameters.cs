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

namespace Guages.Client.Plugin
{
    public class GuagesParameters : ObservableObject, IWidgetParameters
    {
        private Int32 _message_count;
        private Int32 _low;
        private Int32 _medium;
        private Int32 _high;
        private string _guageType;
        private string _custodian;
        private bool _doNotPoll;
        private bool _reverse;

        public GuagesParameters()
        {

            this._message_count = 0;
            this._low = 10;
            this._medium = 20;
            this._high = 20;
            this._guageType = string.Empty;
            this._custodian = "ALL";
            this._doNotPoll = false;
            this._reverse = false;
         
        }

        public Int32 Count
        {
            get
            {
                return this._message_count;
            }

            set
            {
                this._message_count = value;
                this.RaisePropertyChanged("Count");

            }
        }
        public Int32 Low
        {
            get
            {
                return this._low;
            }

            set
            {
                this._low = value;
                this.RaisePropertyChanged("Low");

            }
        }
        public Int32 Medium
        {
            get
            {
                return this._medium;
            }

            set
            {
                this._medium = value;
                this.RaisePropertyChanged("Medium");

            }
        }
        public Int32 High
        {
            get
            {
                return this._high;
            }

            set
            {
                this._high = value;
                this.RaisePropertyChanged("High");

            }
        }
        public string GuageType
        {
            get
            {
                return this._guageType;
            }

            set
            {
                this._guageType = value;
                this.RaisePropertyChanged("GuageType");

            }
        }
        public string Custodian
        {
            get
            {
                return this._custodian;
            }

            set
            {
                this._custodian = value;
                this.RaisePropertyChanged("Custodian");

            }
        }

        public bool DoNotPoll
        {
            get
            {
                return this._doNotPoll;
            }

            set
            {
                this._doNotPoll = value;
                this.RaisePropertyChanged("NoNotPoll");

            }
        }

        public bool Reverse
        {
            get
            {
                return this._reverse;
            }

            set
            {
                this._reverse = value;
                this.RaisePropertyChanged("Reverse");

            }
        }

        public XElement GetParams()
        {
            XElement parameters = new XElement(
                                   "GuagesParameters",
                                   new XAttribute("count", this._message_count),
                                   new XAttribute("low", this._low),
                                   new XAttribute("medium", this._medium),
                                   new XAttribute("high", this._high),
                                   new XAttribute("guageType", this._guageType),
                                   new XAttribute("custodian", this._custodian),
                                   new XAttribute("doNotPoll", this._doNotPoll),
                                    new XAttribute("reverse", this._reverse)
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
                XAttribute CountAttribute = param.Attribute("count");
                XAttribute LowAttribute = param.Attribute("low");
                XAttribute HighAttribute = param.Attribute("high");
                XAttribute MeduimAttribute = param.Attribute("medium");
                XAttribute GuageTypeAttribute = param.Attribute("guageType");
                XAttribute CustodianAttribute = param.Attribute("custodian");
                XAttribute DoNotPollAttribute = param.Attribute("doNotPoll");
                XAttribute Reverse= param.Attribute("reverse");
                try
                {
                    if (Reverse != null)
                    {
                        this._reverse = (bool)Reverse;
                    }

                    if (DoNotPollAttribute != null)
                    {
                        this._doNotPoll = (bool)DoNotPollAttribute;
                    }

                    if (CustodianAttribute != null)
                    {
                        this._custodian = (string)CustodianAttribute;
                    }

                    if (GuageTypeAttribute != null)
                    {
                        this._guageType = (string)GuageTypeAttribute;
                    }

                    if (CountAttribute != null)
                    {
                        this._message_count = (Int32)CountAttribute;
                    }

                    if (LowAttribute != null)
                    {
                        this._low = (Int32)LowAttribute;
                    }
                    if (MeduimAttribute != null)
                    {
                        this._medium = (Int32)MeduimAttribute;
                    }

                    if (HighAttribute != null)
                    {
                        this._high = (Int32)HighAttribute;
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
