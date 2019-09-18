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
using RebalanceInfoSend.Client.ViewModel;
using DevExpress.Xpf.Grid;

namespace RebalanceInfoSend.Client.View
{
    /// <summary>
    /// Interaction logic for Restriction.xaml
    /// </summary>
    public partial class Restriction : Window
    {

        public RebalanceInfoSendViewerModel _vm;
        public RebalanceInfoVisual _ris;

        public Restriction(RebalanceInfoSendViewerModel securityHistoryViewerModel,RebalanceInfoVisual ris)
        {
            InitializeComponent();
            this._vm = securityHistoryViewerModel;
            this._ris = ris;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _vm.Reason = txtresaon.Text;
            Override_Click();
            this.Close();
        }

        private void Override_Click()
        {
            
           


            int count = 0;
            // int selectedrow = 0;

            int _a_id;
            int _o_id;
            int _s_id;

            int i = 0;
            int[] listRowList = _ris._dataCompDetail.GetSelectedRowHandles();
            for (i = 0; i < listRowList.Length; i++)
            {




                GridColumn colaccount_id = _ris._dataCompDetail.Columns["single_account_id"];
                GridColumn colorder_id = _ris._dataCompDetail.Columns["order_id"];
                GridColumn colsecurity_id = _ris._dataCompDetail.Columns["security_id"];


                if (colaccount_id != null)
                {
                    _a_id = Convert.ToInt32(_ris._dataCompDetail.GetCellValue(listRowList[i], colaccount_id).ToString());
                    _o_id = Convert.ToInt32(_ris._dataCompDetail.GetCellValue(listRowList[i], colorder_id).ToString());
                    _s_id = Convert.ToInt32(_ris._dataCompDetail.GetCellValue(listRowList[i], colsecurity_id).ToString());


                    _vm.se_add_first_override(_a_id, _s_id, _vm.Reason, _o_id);

                    count = count + 1;
                }

            }
        }
    }
}
