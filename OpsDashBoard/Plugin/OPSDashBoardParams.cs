using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace OPSDashBoard.Client.Plugin
{
    public class OPSDashBoardParameters : ObservableObject, IWidgetParameters
    {
       
        private string _loadPath;
        private string _logPath;
        private bool _useCurrent;
        private DateTime _runDate;
        private string _securityName;
        private string _majorAsset;
        Boolean _justHoldings;
        decimal _priceChange;
        decimal _stale;

        public OPSDashBoardParameters()
        {
            
            this._loadPath = string.Empty;
            this._logPath = string.Empty;
            this._useCurrent = false;
            this._runDate = DateTime.Today;
            this._securityName = string.Empty;
            this._majorAsset = string.Empty;
            this._justHoldings = true;
            this._priceChange = 5;
            this._stale = 1;
        }





        public string LoadPath
        {
            get
            {
                return this._loadPath;
            }

            set
            {
                this._loadPath = value;
                this.RaisePropertyChanged("LoadPath");

            }
        }

        public string LogPath
        {
            get
            {
                return this._logPath;
            }

            set
            {
                this._logPath = value;
                this.RaisePropertyChanged("LogPath");

            }
        }

        public bool UseCurrent
        {
            get
            {
                return this._useCurrent;
            }

            set
            {
                this._useCurrent = value;
                this.RaisePropertyChanged("UseCurrent");

            }
        }

        public DateTime RunDate
        {
            get
            {
                return this._runDate;
            }

            set
            {
                this._runDate = value;
                this.RaisePropertyChanged("RunDate");

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

        public string MajorAsset
        {
            get
            {
                return this._majorAsset;
            }

            set
            {
                this._majorAsset = value;
                this.RaisePropertyChanged("MajorAsset");

            }
        }

        public Boolean JustHoldings
        {
            get
            {
                return this._justHoldings;
            }

            set
            {
                this._justHoldings = value;
                this.RaisePropertyChanged("JustHoldings");

            }
        }

        public decimal PriceChange
        {
            get
            {
                return this._priceChange;
            }

            set
            {
                this._priceChange = value;
                this.RaisePropertyChanged("PriceChange");

            }
        }

        public decimal Stale
        {
            get
            {
                return this._stale;
            }

            set
            {
                this._stale = value;
                this.RaisePropertyChanged("Stale");

            }
        }

        public XElement GetParams()
        {

            var lp = this._loadPath;
            var sn = this._securityName;
            var ma = this._majorAsset;

            if (lp == null)
                this._securityName = string.Empty;
            if (sn == null)
                this._securityName = string.Empty;
            if (ma == null)
                this._majorAsset = string.Empty;

           XElement parameters = new XElement(
                                   "OPSDashBoardParameters",
                                   new XAttribute("loadPath", this._loadPath),
                                   new XAttribute("logPath", this._logPath),
                                   new XAttribute("useCurrent", this._useCurrent),
                                   new XAttribute("runDate", this._runDate),
                                   new XAttribute("securityName", this._securityName),
                                   new XAttribute("majorAsset", this._majorAsset),
                                   new XAttribute("justHoldings", this._justHoldings),
                                   new XAttribute("priceChange", this._priceChange),
                                   new XAttribute("stale", this._stale)
                                  
                                   );

            

            return parameters;
        }

        public void SetParams(XElement param)
        {
            if (null != param)
            {

                XAttribute LoadPathAttribute = param.Attribute("loadPath");
                XAttribute LogPathAttribute = param.Attribute("logPath");
                XAttribute UseCurrentAttribute = param.Attribute("useCurrent");
                XAttribute UseRunDate = param.Attribute("runDate");
                XAttribute SecurityNameAttribute = param.Attribute("securityName");
                XAttribute MajorAssetAttribute = param.Attribute("majorAsset");
                XAttribute JustHoldingsAttribute = param.Attribute("justHoldings");
                XAttribute PriceChangeAttribute = param.Attribute("priceChange");
                XAttribute StaleAttribute = param.Attribute("stale");

                try
                {

                    if (MajorAssetAttribute != null)
                    {
                        this._majorAsset = (string)MajorAssetAttribute;

                    }

                    if (LoadPathAttribute != null)
                    {
                        this._loadPath = (string)LoadPathAttribute;

                    }

                    if (LogPathAttribute != null)
                    {
                        this._logPath = (string)LogPathAttribute;

                    }

                    if (UseCurrentAttribute != null)
                    {
                        this._useCurrent = (bool)UseCurrentAttribute;

                    }

                    if (UseRunDate != null)
                    {
                        this._runDate = (DateTime)UseRunDate;

                    }

                    if (SecurityName != null)
                    {
                        this._securityName = (string)SecurityNameAttribute;

                    }
                    else
                    {
                        this._securityName = String.Empty;
                    }

                    if (JustHoldingsAttribute != null)
                    {
                        this._justHoldings = (Boolean)JustHoldingsAttribute;

                    }

                    if (PriceChangeAttribute != null)
                    {
                        this._priceChange = (decimal)PriceChangeAttribute;

                    }

                    if (StaleAttribute != null)
                    {
                        this._stale = (decimal)StaleAttribute;

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
