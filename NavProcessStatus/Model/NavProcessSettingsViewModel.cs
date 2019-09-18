using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using NavProcess.Client.ViewModel;
using NavProcess.Client.View;

namespace NavProcess.Client.Model
{
    public class NavProcessSettingsViewModel : ObservableObject
    {
        public NavProcessViewerModel _ViewerModel;
        public NavProcessViewerVisual _ViewerVisual;

        public NavProcessSettingsViewModel(NavProcessViewerModel ViewerModel, NavProcessViewerVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
