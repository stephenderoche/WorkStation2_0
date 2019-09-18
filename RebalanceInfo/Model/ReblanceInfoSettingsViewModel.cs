using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using RebalanceInfo.Client.ViewModel;
using RebalanceInfo.Client.View;

namespace RebalanceInfo.Client.Model
{
    public class ReblanceInfoSettingsViewModel : ObservableObject
    {
        public RebalanceInfoViewerModel _genericGridViewerModel;
        public RebalanceInfoVisual _genericGridViewerVisual;

        public ReblanceInfoSettingsViewModel(RebalanceInfoViewerModel genericGridViewerModel, RebalanceInfoVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
