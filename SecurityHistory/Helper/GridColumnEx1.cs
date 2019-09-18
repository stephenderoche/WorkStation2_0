using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using System.IO;
using DevExpress.Utils.Serializing;
using DevExpress.Xpf.Bars;

namespace SecurityHistory.Client
{
    public class GridColumnEx : GridColumn
    {
        [XtraSerializableProperty]
        public string Format
        {
            get { return ActualEditSettings.DisplayFormat; }
            set { if (ActualEditSettings != null) ActualEditSettings.DisplayFormat = value; }
        }
    }
}
