using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using PythonCloud.Client.ViewModel;
using PythonCloud.Client.View;

namespace PythonCloud.Client.Model
{
    public class PythonCloudSettingsViewModel : ObservableObject
    {
        public PythonCloudViewerModel _genericGridViewerModel;
        public PythonCloudVisual _genericGridViewerVisual;

        public PythonCloudSettingsViewModel(PythonCloudViewerModel genericGridViewerModel, PythonCloudVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
