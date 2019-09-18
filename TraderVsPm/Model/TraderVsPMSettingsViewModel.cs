using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using TraderVsPM.Client.ViewModel;
using TraderVsPM.Client.View;

namespace TraderVsPM.Client.Model
{
    public class TraderVsPMSettingsViewModel : ObservableObject
    {
        public TraderVsPMViewerModel _genericGridViewerModel;
        public TraderVsPMVisual _genericGridViewerVisual;

        public TraderVsPMSettingsViewModel(TraderVsPMViewerModel genericGridViewerModel, TraderVsPMVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
