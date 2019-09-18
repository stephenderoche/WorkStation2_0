using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using Replacement.Client.ViewModel;
using Replacement.Client.View;

namespace Replacement.Client.Model
{
    public class ReplacementSettingsViewModel : ObservableObject
    {
        public ReplacementViewerModel _genericGridViewerModel;
        public ReplacementVisual _genericGridViewerVisual;

        public ReplacementSettingsViewModel(ReplacementViewerModel genericGridViewerModel, ReplacementVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
