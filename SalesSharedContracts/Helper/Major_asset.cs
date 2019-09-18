using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace SalesSharedContracts
{
    public class Major_asset : ObservableObject
    {

        private int _major_asset_code;
        private string _description;
        private bool _selected;

        public Major_asset(int Major_Asset_Code,string Description,bool Selected)
        {
            this._major_asset_code = Major_Asset_Code;
            this._description = Description;
            this._selected = Selected;
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; this.RaisePropertyChanged("Description"); }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; this.RaisePropertyChanged("Selected"); }
        }

        public int Major_asset_code
        {
            get { return _major_asset_code; }
            set { _major_asset_code = value; this.RaisePropertyChanged("Major_asset_code"); }
        }

    }
}
