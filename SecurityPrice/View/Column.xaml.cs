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
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using SecurityHistory.Client.ViewModel;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Utils;
using DevExpress.Utils.Serializing;
using DevExpress.Utils.Serializing.Helpers;


namespace SecurityHistory.Client.View
{
    /// <summary>
    /// Interaction logic for Column.xaml
    /// </summary>
    public partial class Column : Window
    {
     
        SecurityHistoryVisual _mw ;
        public Column(SecurityHistoryVisual m)
        {
            InitializeComponent();
            this._mw = m;
        }

        private TextEditSettings colEdSettings;



     


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridColumn columnDiff = new GridColumn();
            
            columnDiff.Header = txtName.Text;
            columnDiff.AllowUnboundExpressionEditor = true;
            if (cmbotype.Text == "numeric")
            {

                
                columnDiff.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;

                columnDiff.FieldName = "CCN" + txtName.Text;
                columnDiff.Name ="CCN" + txtName.Text;
                var te = new TextEditSettings();
                te.DisplayFormat = "n2";
                columnDiff.EditSettings = te;
                //columnDiff.SettingsMaskType = MaskType.RegEx;
                //((TextEditSettings)columnDiff.EditSettings).DisplayFormat = "{0:n2}";
                //((TextEditSettings)gridControl1.Columns["IssueType"].EditSettings).DisplayFormat = "{0:n2}";


            }

            else if (cmbotype.Text == "percent")

            {
                columnDiff.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                columnDiff.FieldName = "CCP" + txtName.Text;
                var te = new TextEditSettings();
                te.DisplayFormat = "p2";
                columnDiff.EditSettings = te;

            }

            else{
                columnDiff.FieldName = "CCS" + txtName.Text;
                columnDiff.UnboundType = DevExpress.Data.UnboundColumnType.String;

            }
           

            //columnDiff.DisplayFormat.FormatString = "dd-mm-yyyy";

            _mw._dataGrid.Columns.Add(columnDiff);



        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

     
    }
}
