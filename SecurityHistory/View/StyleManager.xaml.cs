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

namespace SecurityHistory.Client.View
{
    /// <summary>
    /// Interaction logic for StyleManager.xaml
    /// </summary>
    public partial class StyleManager : Window
    {
        DevExpress.Xpf.Grid.GridControl _mW;
        public StyleManager(DevExpress.Xpf.Grid.GridControl MW)
        {
            InitializeComponent();
            this._mW = MW;

            DevExpress.Xpf.Grid.GridControl.AllowInfiniteGridSize = true;


            gfy.ItemsSource = _mW.Columns;


            List<string> mylist = new List<string>();
            int i = 0;
            for (i = 0; i < _mW.Columns.Count; i++)
            {
                mylist.Add(_mW.Columns[i].HeaderCaption.ToString());
            }

            ColumnsToStyle.ItemsSource = mylist;
        }
    }
}
