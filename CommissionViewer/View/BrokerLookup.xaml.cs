using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace CommissionViewer.Client
{
    /// <summary>
    /// Interaction logic for IssuerLookup.xaml
    /// </summary>
    public partial class BrokerLookup : Window
    {
        private int mintBrokerId = -1;
        private string mstrBrokerMnemonic = "";
        private DataSet dsBroker = new DataSet();

        public BrokerLookup(DataSet ds)
        {
            InitializeComponent();

            dsBroker = ds;
            dsBroker.Namespace = "Broker";

            this.lstSecurity.ItemsSource = dsBroker.Tables[0].DefaultView;
            this.lstSecurity.DisplayMemberPath = "mnemonic";
            this.lstSecurity.SelectedValuePath = "broker_id";
        }


        public string BrokerMnemonic
        {
            get { return mstrBrokerMnemonic; }
            set { mstrBrokerMnemonic = value; }
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void lstSecurity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void lstSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mintBrokerId = Convert.ToInt32((dsBroker.Tables[0].Rows[this.lstSecurity.SelectedIndex])["broker_id"]);

            mstrBrokerMnemonic = (string)(dsBroker.Tables[0].Rows[this.lstSecurity.SelectedIndex])["mnemonic"];
        }
    }
}
