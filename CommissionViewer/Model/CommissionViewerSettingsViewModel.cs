using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using CommissionViewer.Client.ViewModel;
using CommissionViewer.Client.View;

namespace CommissionViewer.Client.Model
{
    public class CommissionViewerSettingsViewModel : ObservableObject
    {
        public CommissionViewerModel _ViewerModel;
        public CommissionViewerVisual _ViewerVisual;

        public CommissionViewerSettingsViewModel(CommissionViewerModel ViewerModel, CommissionViewerVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
