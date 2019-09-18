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
using PythonCloud.Client.ViewModel;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Utils;
using DevExpress.Utils.Serializing;
using DevExpress.Utils.Serializing.Helpers;


namespace PythonCloud.Client.View
{
    /// <summary>
    /// Interaction logic for Column.xaml
    /// </summary>
    public partial class Column : Window
    {
     
        PythonCloudVisual _mw ;
        public Column(PythonCloudVisual m)
        {
            InitializeComponent();
            this._mw = m;
        }

        private TextEditSettings colEdSettings;



     


  

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

     
    }
}
