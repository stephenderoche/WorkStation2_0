using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace SalesSharedContracts
{
    public class Rebal_info : ObservableObject
    {
        private string _session;
        private int _rebal_seesion_id;
        private int _model_id;
        private string _model_name;
        private int _owner_id;
        private string _owner_name;
        private string _description;
        private DateTime _completion_date;
        private string _sell_non_holdings;
        private string _exclude_encumbered;
        private string _includeAccrued;
        private string _clearPropsed;
        private string _normalize;
        private string _prevent_over;
        private string _redistribute_dis_mv;
        private string _prevent_neg_cash;
        private string _ignore_exist_cash;
        private string _sell_odd;
        private string _exclude_unclass_min;
        private int _min_qty;
        private int _round_qty;
        private string _restricted_list_processing;
        private string _severity_level;
        private string _order_direction;
        private string _price_type;

        public string Session
        {
            get { return _session; }
            set { _session = value; this.RaisePropertyChanged("Session"); }
        }
        public int Rebal_seesion_id
        {
            get { return _rebal_seesion_id; }
            set { _rebal_seesion_id = value; this.RaisePropertyChanged("Rebal_seesion_id"); }
        }
        public int Model_id
        {
            get { return _model_id; }
            set { _model_id = value; this.RaisePropertyChanged("Model_id"); }
        }
        public string Model_name
        {
            get { return _model_name; }
            set { _model_name = value; this.RaisePropertyChanged("Model_name"); }
        }
        public int Owner_id
        {
            get { return _owner_id; }
            set { _owner_id = value; this.RaisePropertyChanged("Owner_id"); }
        }
        public string Owner_name
        {
            get { return _owner_name; }
            set { _owner_name = value; this.RaisePropertyChanged("Owner_name"); }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; this.RaisePropertyChanged("Description"); }
        }
        public DateTime Completion_date
        {
            get { return Convert.ToDateTime(_completion_date.ToString("MM/dd/yyyy")); }
            set { _completion_date = value; this.RaisePropertyChanged("Completion_date"); }
        }
        public string Sell_non_holdings
        {
            get { return _sell_non_holdings; }
            set { _sell_non_holdings = value; this.RaisePropertyChanged("Sell_non_holdings"); }
        }
        public string Exclude_encumbered
        {
            get { return _exclude_encumbered; }
            set { _exclude_encumbered = value; this.RaisePropertyChanged("Exclude_encumbered"); }
        }
        public string IncludeAccrued
        {
            get { return _includeAccrued; }
            set { _includeAccrued = value; this.RaisePropertyChanged("IncludeAccrued"); }
        }
        public string ClearPropsed
        {
            get { return _clearPropsed; }
            set { _clearPropsed = value; this.RaisePropertyChanged("ClearPropsed"); }
        }
        public string Normalize
        {
            get { return _normalize; }
            set { _normalize = value; this.RaisePropertyChanged("Normalize"); }
        }
        public string Prevent_over
        {
            get { return _prevent_over; }
            set { _prevent_over = value; this.RaisePropertyChanged("Prevent_over"); }
        }
        public string Redistribute_dis_mv
        {
            get { return _redistribute_dis_mv; }
            set { _redistribute_dis_mv = value; this.RaisePropertyChanged("Redistribute_dis_mv"); }
        }
        public string Prevent_neg_cash
        {
            get { return _prevent_neg_cash; }
            set { _prevent_neg_cash = value; this.RaisePropertyChanged("Prevent_neg_cash"); }
        }
        public string Ignore_exist_cash
        {
            get { return _ignore_exist_cash; }
            set { _ignore_exist_cash = value; this.RaisePropertyChanged("Ignore_exist_cash"); }
        }
        public string Sell_odd
        {
            get { return _sell_odd; }
            set { _sell_odd = value; this.RaisePropertyChanged("Sell_odd"); }
        }
        public string Exclude_unclass_min
        {
            get { return _exclude_unclass_min; }
            set { _exclude_unclass_min = value; this.RaisePropertyChanged("Exclude_unclass_min"); }
        }
        public int Min_qty
        {
            get { return _min_qty; }
            set { _min_qty = value; this.RaisePropertyChanged("Min_qty"); }
        }
        public int Round_qty
        {
            get { return _round_qty; }
            set { _round_qty = value; this.RaisePropertyChanged("Round_qty"); }
        }
        public string Restricted_list_processing
        {
            get { return _restricted_list_processing; }
            set { _restricted_list_processing = value; this.RaisePropertyChanged("Restricted_list_processing"); }
        }
        public string Severity_level
        {
            get { return _severity_level; }
            set { _severity_level = value; this.RaisePropertyChanged("Severity_level"); }
        }
        public string Order_direction
        {
            get { return _order_direction; }
            set { _order_direction = value; this.RaisePropertyChanged("Order_direction"); }
        }
        public string Price_type
        {
            get { return _price_type; }
            set { _price_type = value; this.RaisePropertyChanged("Price_type"); }
        }
    }
}
