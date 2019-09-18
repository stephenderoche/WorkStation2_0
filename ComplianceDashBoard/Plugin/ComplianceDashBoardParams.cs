using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace ComplianceDashBoard.Client.Plugin
{
    public class FIDashBoardParameters : ObservableObject, IWidgetParameters
    {
       
        private string _accountName;
        private bool _toReview;
        private bool _review;
        private bool _toApprove;
        private bool _approve;
        private bool _activeFail;
        private bool _passiveFail;
        private bool _showPass;
        private bool _showWarning;
        private bool _showAsleep;
        private bool _showOpen;
        private bool _showClosed;
        private bool _showNoAction;
        private bool _showSuspend;
        private bool _showResolved;
        private bool _lastDay;
        private bool _twoDay;
        private bool _fiveDay;
        private bool _month;
        private string _defaultTheme;

        public FIDashBoardParameters()
        {
            this._defaultTheme = "LightGray";
            this._accountName = string.Empty;
            this._toReview = true;
            this._review = true;
            this._toApprove = true;
            this._approve = true;
            this._activeFail = true;
            this._passiveFail = true;
            this._showPass = true;
            this._showWarning = true;
            this._showAsleep = true;
            this._showOpen = true;
            this._showClosed = false;
            this._showNoAction = true;
            this._showSuspend = true;
            this._showResolved = false;
            this._lastDay = false;
            this._twoDay = false;
            this._fiveDay = false;
            this._month = true;

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
        public bool Review
        {
            get
            {
                return this._review;
            }

            set
            {
                this._review = value;
                this.RaisePropertyChanged("Review");

            }
        }
        public bool ToApprove
        {
            get
            {
                return this._toApprove;
            }

            set
            {
                this._toApprove = value;
                this.RaisePropertyChanged("ToApprove");

            }
        }
        public bool Approve
        {
            get
            {
                return this._approve;
            }

            set
            {
                this._approve = value;
                this.RaisePropertyChanged("Approve");

            }
        }
        public bool ActiveFail
        {
            get
            {
                return this._activeFail;
            }

            set
            {
                this._activeFail = value;
                this.RaisePropertyChanged("ActiveFail");

            }
        }
        public bool PassiveFail
        {
            get
            {
                return this._passiveFail;
            }

            set
            {
                this._passiveFail = value;
                this.RaisePropertyChanged("PassiveFail");

            }
        }
        public bool ShowPass
        {
            get
            {
                return this._showPass;
            }

            set
            {
                this._showPass = value;
                this.RaisePropertyChanged("ShowPass");

            }
        }
        public bool ShowWarning
        {
            get
            {
                return this._showWarning;
            }

            set
            {
                this._showWarning = value;
                this.RaisePropertyChanged("ShowWarning");

            }
        }
        public bool ShowAsleep
        {
            get
            {
                return this._showAsleep;
            }

            set
            {
                this._showAsleep = value;
                this.RaisePropertyChanged("ShowAsleep");

            }
        }
        public bool ShowOpen
        {
            get
            {
                return this._showOpen;
            }

            set
            {
                this._showOpen = value;
                this.RaisePropertyChanged("ShowOpen");

            }
        }
        public bool ShowClosed
        {
            get
            {
                return this._showClosed;
            }

            set
            {
                this._showClosed = value;
                this.RaisePropertyChanged("ShowClosed");

            }
        }
        public bool ShowNoAction
        {
            get
            {
                return this._showNoAction;
            }

            set
            {
                this._showNoAction = value;
                this.RaisePropertyChanged("ShowNoAction");

            }
        }
        public bool ShowSuspend
        {
            get
            {
                return this._showSuspend;
            }

            set
            {
                this._showSuspend = value;
                this.RaisePropertyChanged("ShowSuspend");

            }
        }
        public bool ShowResolved
        {
            get
            {
                return this._showResolved;
            }

            set
            {
                this._showResolved = value;
                this.RaisePropertyChanged("ShowResolved");

            }
        }
        public bool LastDay
        {
            get
            {
                return this._lastDay;
            }

            set
            {
                this._lastDay = value;
                this.RaisePropertyChanged("LastDay");

            }
        }
        public bool TwoDay
        {
            get
            {
                return this._twoDay;
            }

            set
            {
                this._twoDay = value;
                this.RaisePropertyChanged("TwoDay");

            }
        }
        public bool FiveDay
        {
            get
            {
                return this._fiveDay;
            }

            set
            {
                this._fiveDay = value;
                this.RaisePropertyChanged("FiveDay");

            }
        }
        public bool Month
        {
            get
            {
                return this._month;
            }

            set
            {
                this._month = value;
                this.RaisePropertyChanged("Month");

            }
        }
         
       
        public XElement GetParams()
        {
           XElement parameters = new XElement(
                                  "ComplianceDashBoard",
                                  new XAttribute("accountName", this._accountName),
                                 new XAttribute("toReview", this._toReview),
                                 new XAttribute("review", this._review),
                                 new XAttribute("toApprove", this._toApprove),
                                 new XAttribute("approve", this._approve),
                                 new XAttribute("activeFail", this._activeFail),
                                 new XAttribute("passiveFail", this._passiveFail),
                                 new XAttribute("showPass", this._showPass),
                                 new XAttribute("showWarning", this._showWarning),
                                  new XAttribute("showAsleep", this._showAsleep),
                                  new XAttribute("showOpen", this._showOpen),
                                  new XAttribute("showClosed", this._showClosed),
                                  new XAttribute("showNoAction", this._showNoAction),
                                  new XAttribute("showSuspend", this._showSuspend),
                                  new XAttribute("showResolved", this._showResolved),
                                  new XAttribute("lastDay", this._lastDay),
                                  new XAttribute("twoDay", this._twoDay),
                                  new XAttribute("fiveDay", this._fiveDay),
                                  new XAttribute("month", this._month),
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
                XAttribute ToReviewAttribute = param.Attribute("toReview");
                XAttribute ReviewAttribute = param.Attribute("review");
                XAttribute ToApproveAttribute = param.Attribute("toApprove");
                XAttribute ApproveAttribute = param.Attribute("approve");
                XAttribute ActiveFailAttribute = param.Attribute("activeFail");
                XAttribute PassiveFailAttribute = param.Attribute("passiveFail");
                XAttribute ShowPassAttribute = param.Attribute("showPass");
                XAttribute ShowWarningAttribute = param.Attribute("showWarning");
                XAttribute ShowAsleepAttribute = param.Attribute("showAsleep");
                XAttribute ShowOpenAttribute = param.Attribute("showOpen");
                XAttribute ShowClosedAttribute = param.Attribute("showClosed");
                XAttribute ShowNoActionAttribute = param.Attribute("showNoAction");
                XAttribute ShowSuspendAttribute = param.Attribute("showSuspend");
                XAttribute ShowResolvedAttribute = param.Attribute("showResolved");
                XAttribute LastDayAttribute = param.Attribute("lastDay");
                XAttribute TwoDayAttribute = param.Attribute("twoDay");
                XAttribute FiveDayAttribute = param.Attribute("fiveDay");
                XAttribute MonthAttribute = param.Attribute("month");
                try
                {

                    if (DefualtThemeAttribute != null)
                    {
                        this._defaultTheme = (string)DefualtThemeAttribute;

                    }
                    if (ToReviewAttribute != null)
                    {
                        this._toReview = (bool)ToReviewAttribute;
                    }
                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;
                    }
                    if (ReviewAttribute != null)
                    {
                        this._review = (bool)ReviewAttribute;
                    }

                    if (ToApproveAttribute != null)
                    {
                        this._toApprove = (bool)ToApproveAttribute;
                    }

                    if (ApproveAttribute != null)
                    {
                        this._approve = (bool)ApproveAttribute;
                    }

                    if (ActiveFailAttribute != null)
                    {
                        this._activeFail = (bool)ActiveFailAttribute;
                    }

                    if (PassiveFailAttribute != null)
                    {
                        this._passiveFail = (bool)PassiveFailAttribute;
                    }

                    if (ShowPassAttribute != null)
                    {
                        this._showPass = (bool)ShowPassAttribute;
                    }

                    if (ShowWarningAttribute != null)
                    {
                        this._showWarning = (bool)ShowWarningAttribute;
                    }

                    if (ShowAsleepAttribute != null)
                    {
                        this._showAsleep = (bool)ShowAsleepAttribute;
                    }

                    if (ShowOpenAttribute != null)
                    {
                        this._showOpen = (bool)ShowOpenAttribute;
                    }

                    if (ShowClosedAttribute != null)
                    {
                        this._showClosed = (bool)ShowClosedAttribute;
                    }

                    if (ShowNoActionAttribute != null)
                    {
                        this._showNoAction= (bool)ShowNoActionAttribute;
                    }

                    if (ShowSuspendAttribute != null)
                    {
                        this._showSuspend = (bool)ShowSuspendAttribute;
                    }

                    if (ShowResolvedAttribute != null)
                    {
                        this._showResolved = (bool)ShowResolvedAttribute;
                    }

                    if (LastDayAttribute != null)
                    {
                        this._lastDay = (bool)LastDayAttribute;
                    }


                    if (TwoDayAttribute != null)
                    {
                        this._twoDay = (bool)TwoDayAttribute;
                    }

                    if (FiveDayAttribute != null)
                    {
                        this._fiveDay = (bool)FiveDayAttribute;
                    }

                    if (FiveDayAttribute != null)
                    {
                        this._fiveDay = (bool)FiveDayAttribute;
                    }


                    if (MonthAttribute != null)
                    {
                        this._month = (bool)MonthAttribute;
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
