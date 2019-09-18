using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace TraderVsPM.Client
{
    /// <summary>
    /// Interaction logic for SecurityLookup.xaml
    /// </summary>
    public partial class SecurityLookup : Window
    {
        private int mintSecurityId = -1;
        private string mstrShortName = "";
        private string mstrIssuerShortName = "";
        private DataSet dsSecurity = new DataSet();

        public int SecurityId
        {
            get { return mintSecurityId; }
            set { mintSecurityId = value; }
        }

        public string ShortName
        {
            get { return mstrShortName; }
            set { mstrShortName = value; }
        }

        public string IssuerShortName
        {
            get { return mstrIssuerShortName; }
            set { mstrIssuerShortName = value; }
        }

        public SecurityLookup(DataSet ds)
        {
            InitializeComponent();

            dsSecurity = ds;
            dsSecurity.Namespace = "SECURITY";

            this.lstSecurity.ItemsSource = dsSecurity.Tables[0].DefaultView;
            this.lstSecurity.DisplayMemberPath = "mnemonic";
            this.lstSecurity.SelectedValuePath = "security_id";
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
            mintSecurityId = Convert.ToInt32((dsSecurity.Tables[0].Rows[this.lstSecurity.SelectedIndex])["security_id"]);
            mstrShortName = (string)(dsSecurity.Tables[0].Rows[this.lstSecurity.SelectedIndex])["mnemonic"];
           // mstrIssuerShortName = (string)(dsSecurity.Tables[0].Rows[this.lstSecurity.SelectedIndex])["short_name"];

        }

        
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }
}
