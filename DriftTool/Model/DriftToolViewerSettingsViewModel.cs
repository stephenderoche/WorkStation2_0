using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using ExceptionManagement.Client.ViewModel;
using ExceptionManagement.Client.View;

namespace ExceptionManagement.Client.Model
{
    public class DriftToolViewerSettingsViewModel : ObservableObject
    {
        public DriftToolViewerModel _ViewerModel;
        public DriftToolViewerVisual _ViewerVisual;

        public DriftToolViewerSettingsViewModel(DriftToolViewerModel ViewerModel, DriftToolViewerVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
