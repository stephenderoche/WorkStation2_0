using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using StatPro2.Client.ViewModel;
using StatPro2.Client.View;

namespace StatPro2.Client.Model
{
    public class StatPro2SettingsViewModel : ObservableObject
    {
        public StatPro2ViewerModel _genericGridViewerModel;
        public StatPro2Visual _genericGridViewerVisual;

        public StatPro2SettingsViewModel(StatPro2ViewerModel genericGridViewerModel, StatPro2Visual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
