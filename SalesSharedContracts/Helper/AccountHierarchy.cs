using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace SalesSharedContracts
{
    public class AccountHierarchy : ObservableObject
    {
        private Int32 _parentSectorId;
        private string _parentName;
        private Int32 _childSectorId;
        private string _childName;
        private string _symbol;
        private Int32 _securityID;
        private decimal _quantity;
        private decimal _proposedQuantity;
        private decimal _orderQuantity;
        private decimal _totalQuantity;
        private decimal _securityMV;
        private decimal _accountMV;
        private decimal _securityPercent;

        public string ParentName
        {
            get { return _parentName; }
            set { _parentName = value; this.RaisePropertyChanged("ParentName"); }
        }
        public Int32 ParentSectorId
        {
            get { return _parentSectorId; }
            set { _parentSectorId = value; this.RaisePropertyChanged("ParentSectorId"); }
        }
        public Int32 ChildSectorId
        {
            get { return _childSectorId; }
            set { _childSectorId = value; this.RaisePropertyChanged("ChildSectorId"); }
        }
        public string ChildName
        {
            get { return _childName; }
            set { _childName = value; this.RaisePropertyChanged("ChildName"); }
        }
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; this.RaisePropertyChanged("Symbol"); }
        }
        public Int32 SecurityID
        {
            get { return _securityID; }
            set { _securityID = value; this.RaisePropertyChanged("SecurityID"); }
        }
        public decimal Quantity
        {
            get { return _quantity; }
            set { _quantity = value; this.RaisePropertyChanged("Quantity"); }
        }
        public decimal ProposedQuantity
        {
            get { return _proposedQuantity; }
            set { _proposedQuantity = value; this.RaisePropertyChanged("ProposedQuantity"); }
        }
        public decimal OrderQuantity
        {
            get { return _orderQuantity; }
            set { _orderQuantity = value; this.RaisePropertyChanged("OrderQuantity"); }
        }
        public decimal TotalQuantity
        {
            get { return _totalQuantity; }
            set { _totalQuantity = value; this.RaisePropertyChanged("TotalQuantity"); }
        }
        public decimal SecurityMV
        {
            get { return _securityMV; }
            set { _securityMV = value; this.RaisePropertyChanged("SecurityMV"); }
        }
        public decimal AccountMV
        {
            get { return _accountMV; }
            set { _accountMV = value; this.RaisePropertyChanged("AccountMV"); }
        }
        public decimal SecurityPercent
        {
            get { return _securityPercent; }
            set { _securityPercent = value; this.RaisePropertyChanged("SecurityPercent"); }
        }
    }
}
