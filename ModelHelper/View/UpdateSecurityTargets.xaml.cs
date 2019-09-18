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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModelHelper.Client.ViewModel;
using System.Threading;

namespace ModelHelper.Client.View
{
    /// <summary>
    /// Inte[raction logic for UpdateSecurityTargets.xaml
    /// </summary>
    public partial class UpdateSecurityTargets : Window
    {

        public ModelHelperModel _view;

        public UpdateSecurityTargets(ModelHelperModel ViewerModel)
        {
            InitializeComponent();
            this.DataContext = ViewerModel;
            this._view = ViewerModel;
            _view.se_get_price_journal();
        }

   
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LBLSymbol.Content = string.Format("{0} ", _view.Symbol);
            TXTPurchasePrice.Text = string.Format("{0:n4}", _view.Purchase_price);
            TXTTargetPrice.Text = string.Format("{0:n4}", _view.Target_price);
        }

        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtEntry.Text))
            {
                _view.se_update_price_journal(TxtEntry.Text);
                System.Threading.Thread.Sleep(1000);
                _view.se_get_price_journal();
                TxtEntry.Text = String.Empty;

            }
        }

        private void AddPrice_Click(object sender, RoutedEventArgs e)
        {
            
                _view.Purchase_price = Convert.ToDecimal(TXTPurchasePrice.Text);
                _view.Target_price = Convert.ToDecimal(TXTTargetPrice.Text);
                _view.se_update_price_history();
            

        }
    }
}
