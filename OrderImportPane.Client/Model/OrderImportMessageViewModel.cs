using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Linedata.Framework.WidgetFrame.MvvmFoundation;


namespace OrderImportPane.Client.Model
{
   public class OrderImportPaneViewModel : ObservableObject
    {

       public OrderImportPaneViewModel(OrderImportPaneViewerParameters parameters)
        {
            this.Parameters = parameters;
        }

       public OrderImportPaneViewerParameters Parameters
        {
            get;
            set;
        }
    }
}
