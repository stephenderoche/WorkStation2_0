using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using GenericGrid.Client.ViewModel;
using GenericGrid.Client.View;

namespace GenericGrid.Client.Model
{
    public class GenericGridViewerSettingsViewModel : ObservableObject
    {
        public GenericGridViewerModel _genericGridViewerModel;
        public GenericGridViewerVisual _genericGridViewerVisual;

        public GenericGridViewerSettingsViewModel(GenericGridViewerModel genericGridViewerModel, GenericGridViewerVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
