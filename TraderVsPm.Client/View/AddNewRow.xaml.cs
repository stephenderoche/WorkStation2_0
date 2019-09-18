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
    /// Interaction logic for AddNewRow.xaml
    /// </summary>
    public partial class AddNewRow : Window
    {
        TraderVsPm _view;
        public AddNewRow(TraderVsPm View)
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
            _view.UpdateJournal(this.txtJournal.Text);
        }
    }
}
