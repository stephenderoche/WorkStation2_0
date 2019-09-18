using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace SalesSharedContracts
{
    public class Account_info : ObservableObject
    {

        private int _account_id;
        private string _account_name;
        private string _account_type;
        private string _model_name;
        private string _account_holder;
        private string _account_holder_contact;
        private string _manager;
        private DateTime _inception_date;
        private float _funding_value;
        private DateTime _performance_start_date;
        private DateTime _close_date;
        private DateTime _composite_entry_date;
        private DateTime _composite_exit_date;
        private bool _taxable;
        private bool _distribution;
        private float _distribution_amount;
        private string _distribution_frequency;
        private DateTime _last_distribution;
        private DateTime _next_distribution;
        private bool _ips;
        private DateTime _lastipsReview;
        private string _nextipsReview;
        private DateTime _lastRegReview;
        private string _nextRegReview;
        private decimal _managmentFee;
        private string _custodian;
        private bool _securityRebalance;
        private bool _sectorRebalance;
        private bool _autoRebalance;
        private string _rebalanceFrequency;
        private DateTime _lastRebalance;
        private DateTime _nextRebalance;
        private string _reliefMethod;
        private bool _eligible;


        public bool Eligible
        {
            get { return _eligible; }
            set { _eligible = value; this.RaisePropertyChanged("Eligible"); }
        }

        public string ReliefMethod
        {
            get { return _reliefMethod; }
            set { _reliefMethod = value; this.RaisePropertyChanged("ReliefMethod"); }
        }

        public DateTime LastRebalance
        {
            get { return Convert.ToDateTime(_lastRebalance.ToString("MM/dd/yyyy")); }
            set { _lastRebalance = value; this.RaisePropertyChanged("LastRebalance"); }
        }

        public DateTime NextRebalance
        {
            get { return Convert.ToDateTime(_nextRebalance.ToString("MM/dd/yyyy")); }
            set { _nextRebalance = value; this.RaisePropertyChanged("NextRebalance"); }
        }

        public string RebalanceFrequency
        {
            get { return _rebalanceFrequency; }
            set { _rebalanceFrequency = value; this.RaisePropertyChanged("RebalanceFrequency"); }
        }

        public bool SecurityRebalance
        {
            get { return _securityRebalance; }
            set { _securityRebalance = value; this.RaisePropertyChanged("SecurityRebalance"); }
        }

        public bool SectorRebalance
        {
            get { return _sectorRebalance; }
            set { _sectorRebalance = value; this.RaisePropertyChanged("SectorRebalance"); }
        }

        public bool AutoRebalance
        {
            get { return _autoRebalance; }
            set { _autoRebalance = value; this.RaisePropertyChanged("AutoRebalance"); }
        }

        public string Custodian
        {
            get { return _custodian; }
            set { _custodian = value; this.RaisePropertyChanged("Custodian"); }
        }

        public decimal ManagmentFee
        {
            get { return _managmentFee; }
            set { _managmentFee = value; this.RaisePropertyChanged("ManagmentFee"); }
        }

        public DateTime LastRegReview
        {
            get { return Convert.ToDateTime(_lastRegReview.ToString("MM/dd/yyyy")); }
            set { _lastRegReview = value; this.RaisePropertyChanged("LastRegReview"); }
        }

        public string NextRegReview
        {
            get { return _nextRegReview; }
            set { _nextRegReview = value; this.RaisePropertyChanged("NextRegReview"); }
        }


        public string NextipsReview
        {
            get { return _nextipsReview; }
            set { _nextipsReview = value; this.RaisePropertyChanged("NextipsReview"); }
        }

        public DateTime LastipsReview
        {
            get { return Convert.ToDateTime(_lastipsReview.ToString("MM/dd/yyyy")); }
            set { _lastipsReview = value; this.RaisePropertyChanged("LastipsReview"); }
        }

        public bool IPS
        {
            get { return _ips; }
            set { _ips = value; this.RaisePropertyChanged("IPS"); }
        }

        public DateTime NextDistribution
        {
            get { return Convert.ToDateTime(_next_distribution.ToString("MM/dd/yyyy")); }
            set { _next_distribution = value; this.RaisePropertyChanged("NextDistribution"); }
        }

        public DateTime LastDistribution
        {
            get { return Convert.ToDateTime(_last_distribution.ToString("MM/dd/yyyy")); }
            set { _last_distribution = value; this.RaisePropertyChanged("LastDistribution"); }
        }

        public string DistributionFrequency
        {
            get { return _distribution_frequency; }
            set { _distribution_frequency = value; this.RaisePropertyChanged("DistributionFrequency"); }
        }

        public float DistributionAmount
        {
            get { return _distribution_amount; }
            set { _distribution_amount = value; this.RaisePropertyChanged("DistributionAmount"); }
        }

        public bool Distribution
        {
            get { return _distribution;  }
            set { _distribution = value; this.RaisePropertyChanged("Distribution"); }
        }

        public bool Taxable
        {
            get { return _taxable; }
            set { _taxable = value; this.RaisePropertyChanged("Taxable"); }
        }

        public DateTime PerformanceStartDate
        {
            get { return Convert.ToDateTime(_performance_start_date.ToString("MM/dd/yyyy")); }
            set { _performance_start_date = value; this.RaisePropertyChanged("PerformanceStartDate"); }
        }
        public DateTime CloseDate
        {
            get { return Convert.ToDateTime(_close_date.ToString("MM/dd/yyyy")); }
            set { _close_date = value; this.RaisePropertyChanged("CloseDate"); }
        }
        public DateTime CompositeEntryDate
        {
            get { return Convert.ToDateTime(_composite_entry_date.ToString("MM/dd/yyyy")); }
            set { _composite_entry_date = value; this.RaisePropertyChanged("CompositeEntryDate"); }
        }
        public DateTime CompositeExitDate
        {
            get { return Convert.ToDateTime(_composite_exit_date.ToString("MM/dd/yyyy")); }
            set { _composite_exit_date = value; this.RaisePropertyChanged("CompositeExitDate"); }
        }

        public int Account_id
        {
            get { return _account_id; }
            set { _account_id = value; this.RaisePropertyChanged("Account_id"); }
        }

        public string Account_name
        {
            get { return _account_name; }
            set { _account_name = value; this.RaisePropertyChanged("Account_name"); ; }
        }

        public string Account_type
        {
            get { return _account_type; }
            set { _account_type = value; this.RaisePropertyChanged("Account_type"); }
        }

        public string Model_name
        {
            get { return _model_name; }
            set { _model_name = value; this.RaisePropertyChanged("Model_name"); }
        }

        public string Account_holder
        {
            get { return _account_holder; }
            set { _account_holder = value; this.RaisePropertyChanged("Account_holder"); }
        }

        public string Account_holder_contact
        {
            get { return _account_holder_contact; }
            set { _account_holder_contact = value; this.RaisePropertyChanged("Account_holder_contact"); }
        }

        public string Manager
        {
            get { return _manager; }
            set { _manager = value; this.RaisePropertyChanged("Manager"); }
        }

        public DateTime InceptionDate
        {
            get { return Convert.ToDateTime(_inception_date.ToString("MM/dd/yyyy")); }
            set { _inception_date = value; this.RaisePropertyChanged("InceptionDate"); }
        }

        public float Funding_value
        {
            get { return _funding_value ; }
            set { _funding_value = value; this.RaisePropertyChanged("Funding_value"); }
        }




    }
}
