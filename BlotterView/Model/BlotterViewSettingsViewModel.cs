using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using BlotterView.Client.ViewModel;
using BlotterView.Client.View;

namespace BlotterView.Client.Model
{
    public class BlotterViewSettingsViewModel : ObservableObject
    {
        public BlotterViewViewerModel _genericGridViewerModel;
        public BlotterViewVisual _genericGridViewerVisual;

        public BlotterViewSettingsViewModel(BlotterViewViewerModel genericGridViewerModel, BlotterViewVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
