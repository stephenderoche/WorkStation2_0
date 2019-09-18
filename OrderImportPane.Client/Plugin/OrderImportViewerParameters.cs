namespace OrderImportPane.Client
{
   
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

    public class OrderImportPaneViewerParameters : ObservableObject,IWidgetParameters
    {
        private string follower;
        private string _accountName;
        private string _visible;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _followingWidget;
        private bool _isHeader;
        private bool _acknowledge;
        private string _custodian;
        private string _message_type;

        public OrderImportPaneViewerParameters()
        {
          
            this.follower = string.Empty;
            this._accountName = string.Empty;
            this._visible = string.Empty;
            this._startDate = DateTime.Today;
            this._endDate = DateTime.Today;
            this._followingWidget = "MY Message";
            this._isHeader = true;
            this._acknowledge = true;
            this._custodian = "All";
            this._message_type = "All";

          
        }

        public bool Acknowledge
        {
            get
            {
                return this._acknowledge;
            }

            set
            {
                this._acknowledge = value;
                this.RaisePropertyChanged("Acknowledge");

            }
        }

        public bool IsHeader
        {
            get
            {
                return this._isHeader;
            }

            set
            {
                this._isHeader = value;
                this.RaisePropertyChanged("IsHeader");

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

        public string Message_type
        {
            get
            {
                return this._message_type;
            }

            set
            {
                this._message_type = value;
                this.RaisePropertyChanged("Message_type");

            }
        }

        public string FollowingWidget
        {
            get
            {
                return this._followingWidget;
            }

            set
            {
                this._followingWidget = value;
                this.RaisePropertyChanged("FollowingWidget");

            }
        }

        public DateTime EndDate
        {
            get
            {
                return this._endDate;
            }

            set
            {
                this._endDate = value;
                this.RaisePropertyChanged("EndDate");

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

        public string Visibilty
        {
            get
            {
                return this._visible;
            }

            set
            {
                this._visible = value;
                this.RaisePropertyChanged("Visibilty");

            }
        }


        public string Follower
        {
            get
            {
                return this.follower;
            }

            set
            {
                this.follower = value;
                this.RaisePropertyChanged("Follower");
                
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
            XElement parameters = new XElement(
                                   "NavShareParameters",
                                   new XAttribute("follower", this.Follower),
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("visibilty", this._visible),
                                     new XAttribute("startDate", this._startDate),
                                   new XAttribute("endDate", this._endDate),
                                     new XAttribute("followingWidget", this._followingWidget),
                                      new XAttribute("custodian", this.Custodian),
                                     new XAttribute("messagetype", this.Message_type),
                                     new XAttribute("isheader", this._isHeader),
                                     new XAttribute("acknowledge", this._acknowledge)
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
                XAttribute FollowerAttribute = param.Attribute("follower");
                XAttribute AccountNameAttribute = param.Attribute("accountName");
                XAttribute VisibiltyAttribute = param.Attribute("visibilty");
                XAttribute StartDateAttribute = param.Attribute("startDate");
                XAttribute EndDateAttribute = param.Attribute("endDate");
                XAttribute FollowingWidget = param.Attribute("followingWidget");
                XAttribute CustodianAttribute = param.Attribute("custodian");
                XAttribute MessageTypeAttribute = param.Attribute("messagetype");
                XAttribute IsHeaderAttribute = param.Attribute("isheader");
                XAttribute AcknowledgeAttribute = param.Attribute("acknowledge");
                try
                {
                    if (FollowerAttribute != null)
                    {
                        this.follower = (string)FollowerAttribute;
                       
                    }

                    if (AccountNameAttribute != null)
                    {
                        this._accountName = (string)AccountNameAttribute;

                    }

                    if (VisibiltyAttribute != null)
                    {
                        this._visible = (string)VisibiltyAttribute;

                    }

                    if (StartDateAttribute != null)
                    {
                        this._startDate = (DateTime)StartDateAttribute;

                    }

                    if (EndDateAttribute != null)
                    {
                        this._endDate = (DateTime)EndDateAttribute;

                    }

                    if (FollowingWidget != null)
                    {
                        this._followingWidget = (string)FollowingWidget;

                    }

                    if (CustodianAttribute != null)
                    {
                        this._custodian = (string)CustodianAttribute;

                    }


                    if (MessageTypeAttribute != null)
                    {
                        this._message_type = (string)MessageTypeAttribute;

                    }

                    if (IsHeaderAttribute != null)
                    {
                        this._isHeader = (bool)IsHeaderAttribute;
                    }

                    if (AcknowledgeAttribute != null)
                    {
                        this._acknowledge = (bool)AcknowledgeAttribute;
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
