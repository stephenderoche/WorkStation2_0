using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace SalesSharedContracts
{
    public class AccountTreeMap : ObservableObject
    {
        private Int32 _parent_account_id;
        private string _parent_Account_Name;
        private string _child_type;
        private Int32 _child_account_id;
        private string _child_Account_Name;


        public Int32 Parent_account_id
        {
            get { return _parent_account_id; }
            set { _parent_account_id = value; this.RaisePropertyChanged("Parent_account_id"); }
        }

        public string Parent_Account_Name
        {
            get { return _parent_Account_Name; }
            set { _parent_Account_Name = value; this.RaisePropertyChanged("Parent_Account_Name"); }
        }

        public string Child_type
        {
            get { return _child_type; }
            set { _child_type = value; this.RaisePropertyChanged("Child_type"); }
        }

        public Int32 Child_account_id
        {
            get { return _child_account_id; }
            set { _child_account_id = value; this.RaisePropertyChanged("Child_account_id"); }
        }

        public string Child_Account_Name
        {
            get { return _child_Account_Name; }
            set { _child_Account_Name = value; this.RaisePropertyChanged("Child_Account_Name"); }
        }
    }
}
