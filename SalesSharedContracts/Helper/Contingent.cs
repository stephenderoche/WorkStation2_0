using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace SalesSharedContracts
{
    public class Contingent : ObservableObject
    {

        private int _account_id;
        private string _account_name;
        private string _share_class;
        private int _share_class_type_code;
        private decimal _class_ratio;
        private decimal _starting_nav;
        private decimal _income;
        private decimal _amort;
        private decimal _expense;
        private decimal _expenseclass;
        private decimal _realizedgl;
        private decimal _realizedcurrgl;
        private decimal _unrealizedgl;
        private decimal _unrealizedcurrgl;
        private decimal _distibution;
        private decimal _capitalStock;
        private decimal _ending_nav;
        private decimal _subsReds;
        private decimal _starting_sharesoutstanding;
        private decimal _ending_sharesoutstanding;
        private decimal _contingentnav;
        private decimal _actualnav;
        private DateTime _navDate;

        public int Account_id
        {
            get { return _account_id; }
            set { _account_id = value; this.RaisePropertyChanged("Account_id"); }
        }

        public string Account_name
        {
            get { return _account_name; }
            set { _account_name = value; this.RaisePropertyChanged("Account_name"); }
        }

        public string Share_class
        {
            get { return _share_class; }
            set { _share_class = value; this.RaisePropertyChanged("Share_class"); }
        }

        public int Share_class_type_code
        {
            get { return _share_class_type_code; }
            set { _share_class_type_code = value; this.RaisePropertyChanged("Share_class_type_code"); }
        }

     
        public decimal Class_ratio
        {
            get { return _class_ratio; }
            set { _class_ratio = value; this.RaisePropertyChanged("Class_ratio"); }
        }

        public decimal Starting_nav
        {
            get { return _starting_nav; }
            set { _starting_nav = value; this.RaisePropertyChanged("Starting_nav"); }
        }

        public decimal Income
        {
            get { return _income; }
            set { _income = value; this.RaisePropertyChanged("Income"); }
        }

        public decimal Amort
        {
            get { return _amort; }
            set { _amort = value; this.RaisePropertyChanged("Amort"); }
        }

        public decimal Expense
        {
            get { return _expense; }
            set { _expense = value; this.RaisePropertyChanged("Expense"); }
        }

        public decimal Expenseclass
        {
            get { return _expenseclass; }
            set { _expenseclass = value; this.RaisePropertyChanged("Expenseclass"); }
        }

        public decimal Realizedgl
        {
            get { return _realizedgl; }
            set { _realizedgl = value; this.RaisePropertyChanged("Realizedgl"); }
        }

        public decimal Realizedcurrgl
        {
            get { return _realizedcurrgl; }
            set { _realizedcurrgl = value; this.RaisePropertyChanged("Realizedcurrgl"); }
        }

        public decimal Unrealizedgl
        {
            get { return _unrealizedgl; }
            set { _unrealizedgl = value; this.RaisePropertyChanged("Unrealizedgl"); }
        }

        public decimal Unrealizedcurrgl
        {
            get { return _unrealizedcurrgl; }
            set { _unrealizedcurrgl = value; this.RaisePropertyChanged("Unrealizedcurrgl"); }
        }

        public decimal Distibution
        {
            get { return _distibution; }
            set { _distibution = value; this.RaisePropertyChanged("Distibution"); }
        }

        public decimal CapitalStock
        {
            get { return _capitalStock; }
            set { _capitalStock = value; this.RaisePropertyChanged("CapitalStock"); }
        }

        public decimal Ending_nav
        {
            get { return _ending_nav; }
            set { _ending_nav = value; this.RaisePropertyChanged("Ending_nav"); }
        }

        public decimal SubsReds
        {
            get { return _subsReds; }
            set { _subsReds = value; this.RaisePropertyChanged("SubsReds"); }
        }

        public decimal Starting_sharesoutstanding
        {
            get { return _starting_sharesoutstanding; }
            set { _starting_sharesoutstanding = value; this.RaisePropertyChanged("Starting_sharesoutstanding"); }
        }

        public decimal Ending_sharesoutstanding
        {
            get { return _ending_sharesoutstanding; }
            set { _ending_sharesoutstanding = value; this.RaisePropertyChanged("Ending_sharesoutstanding"); }
        }

        public decimal Contingentnav
        {
            get { return _contingentnav; }
            set { _contingentnav = value; this.RaisePropertyChanged("Contingentnav"); }
        }

        public decimal Actualnav
        {
            get { return _actualnav; }
            set { _actualnav = value; this.RaisePropertyChanged("Actualnav"); }
        }
        
   

        public DateTime NavDate
        {
            get { return Convert.ToDateTime(_navDate.ToString("MM/dd/yyyy")); }
            set { _navDate = value; this.RaisePropertyChanged("LastRebalance"); }
        }

     



    }
}
