using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using SecurityHistory.Client.ViewModel;
using SecurityHistory.Client.View;

namespace SecurityHistory.Client.Model
{
    public class SecurityHistorySettingsViewModel : ObservableObject
    {
        public SecurityHistoryViewerModel _genericGridViewerModel;
        public SecurityHistoryVisual _genericGridViewerVisual;

        public SecurityHistorySettingsViewModel(SecurityHistoryViewerModel genericGridViewerModel, SecurityHistoryVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
