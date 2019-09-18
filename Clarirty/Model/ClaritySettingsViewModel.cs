using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using Clarity.Client.ViewModel;
using Clarity.Client.View;

namespace Clarity.Client.Model
{
    public class ClaritySettingsViewModel : ObservableObject
    {
        public ClarityViewerModel _genericGridViewerModel;
        public ClarityVisual _genericGridViewerVisual;

        public ClaritySettingsViewModel(ClarityViewerModel genericGridViewerModel, ClarityVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
