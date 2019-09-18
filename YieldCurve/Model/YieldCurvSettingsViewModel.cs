using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using YieldCurve.Client.ViewModel;
using YieldCurve.Client.View;

namespace YieldCurve.Client.Model
{
    public class YieldCurveSettingsViewModel : ObservableObject
    {
        public YieldCurveModel _ViewerModel;
        public YieldCurveVisual _ViewerVisual;

        public YieldCurveSettingsViewModel(YieldCurveModel ViewerModel, YieldCurveVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
