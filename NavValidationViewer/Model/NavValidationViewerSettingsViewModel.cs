using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using NavValidationViewer.Client.ViewModel;
using NavValidationViewer.Client.View;

namespace NavValidationViewer.Client.Model
{
    public class NavValidationViewerSettingsViewModel : ObservableObject
    {
        public NavValidationViewerModel _ViewerModel;
        public NavValidationViewerVisual _ViewerVisual;

        public NavValidationViewerSettingsViewModel(NavValidationViewerModel ViewerModel, NavValidationViewerVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
