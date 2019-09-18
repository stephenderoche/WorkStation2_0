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

namespace TraderVsPM.Client
{
    /// <summary>
    /// Interaction logic for IssuerLookup.xaml
    /// </summary>
    public partial class IssuerLookup : Window
    {
        private int mintIssuerId = -1;
        private string mstrIssuerShortName = "";
        private DataSet dsIssuer = new DataSet();

        public IssuerLookup(DataSet ds)
        {
            InitializeComponent();

            dsIssuer = ds;
            dsIssuer.Namespace = "Issuer";

            this.lstSecurity.ItemsSource = dsIssuer.Tables[0].DefaultView;
            this.lstSecurity.DisplayMemberPath = "short_name";
            this.lstSecurity.SelectedValuePath = "issuer_id";
        }


        public string IssuerShortName
        {
            get { return mstrIssuerShortName; }
            set { mstrIssuerShortName = value; }
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
            mintIssuerId = Convert.ToInt32((dsIssuer.Tables[0].Rows[this.lstSecurity.SelectedIndex])["issuer_id"]);

            mstrIssuerShortName = (string)(dsIssuer.Tables[0].Rows[this.lstSecurity.SelectedIndex])["short_name"];
        }
    }
}
