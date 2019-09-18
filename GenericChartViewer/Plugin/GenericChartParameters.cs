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

namespace GenericChart
{
    public class GenericChartParameters : ObservableObject, IWidgetParameters
    {
        private string follower;
        private string _accountName;
        private string _visible;
        private string _dataType;
         private string _hierarchy;
         private bool _negitive;
         private bool _postive;
     

        public GenericChartParameters()
        {
          
            this.follower = string.Empty;
            this._accountName = string.Empty;
            this._visible = string.Empty;
            this._dataType = string.Empty;
            this._hierarchy = string.Empty;
        
              this._negitive = false;
            this._postive = false;
          
        }

        public bool Negitive
        {
            get
            {
                return this._negitive;
            }

            set
            {
                this._negitive = value;
                this.RaisePropertyChanged("Negitive");

            }
        }

        public bool Positive
        {
            get
            {
                return this._postive;
            }

            set
            {
                this._postive = value;
                this.RaisePropertyChanged("Positive");

            }
        }

        public string Hierarchy
        {
            get
            {
                return this._hierarchy;
            }

            set
            {
                this._hierarchy = value;
                this.RaisePropertyChanged("Hierarchy");

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

        public string DataType
        {
            get
            {
                return this._dataType;
            }

            set
            {
                this._dataType = value;
                this.RaisePropertyChanged("DataType");

            }
        }

        public XElement GetParams()
        {
            XElement parameters = new XElement(
                                   "GenericChartParameters",
                                   new XAttribute("follower", this.Follower),
                                   new XAttribute("accountName", this._accountName),
                                   new XAttribute("visibilty", this._visible),
                                   new XAttribute("dataType", this._dataType),
                                   new XAttribute("hierarchy", this._hierarchy),
                                   new XAttribute("negitive", this.Negitive),
                                   new XAttribute("positive", this.Positive)
                                  
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
                XAttribute DataTypeAttribute = param.Attribute("dataType");
                XAttribute HierarchyAttribute = param.Attribute("hierarchy");
                XAttribute NegitiveAttribute = param.Attribute("negitive");
                XAttribute PositiveAttribute = param.Attribute("positive");
              
                try
                {

                    if (NegitiveAttribute != null)
                    {
                        this._negitive = (bool)NegitiveAttribute;

                    }

                    if (PositiveAttribute != null)
                    {
                        this._postive = (bool)PositiveAttribute;

                    }

                    if (HierarchyAttribute != null)
                    {
                        this._hierarchy = (string)HierarchyAttribute;

                    }

                    if (DataTypeAttribute != null)
                    {
                        this.DataType = (string)DataTypeAttribute;

                    }


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
