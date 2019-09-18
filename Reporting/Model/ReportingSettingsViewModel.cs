using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using Reporting.Client.ViewModel;
using Reporting.Client.View;

namespace Reporting.Client.Model
{
    public class ReportingSettingsViewModel : ObservableObject
    {
        public ReportingModel _genericGridViewerModel;
        public ReportingVisual _genericGridViewerVisual;

        public ReportingSettingsViewModel(ReportingModel genericGridViewerModel, ReportingVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
