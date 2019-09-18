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

namespace TraderVsPM.Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Commitment : Window
    {
        TraderVsPm _view;

        public Commitment(TraderVsPm View)
        {
            InitializeComponent();

            this._view = View;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string message = string.Format("Commitment price of {0} was added.", this.Spe_price.Text);
            _view.UpdateJournal(message);


            _view.UpdateCommitmentPrice(_view.BlockID, Convert.ToDecimal(this.Spe_price.Text));



        }
    }
}
