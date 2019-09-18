using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using CurrentOrders.Client.ViewModel;
using CurrentOrders.Client.View;

namespace CurrentOrders.Client.Model
{
    public class CurrentOrdersViewerSettingsViewModel : ObservableObject
    {
        public CurrentOrdersViewerModel _ViewerModel;
        public CurrentOrdersViewerVisual _ViewerVisual;

        public CurrentOrdersViewerSettingsViewModel(CurrentOrdersViewerModel ViewerModel, CurrentOrdersViewerVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
