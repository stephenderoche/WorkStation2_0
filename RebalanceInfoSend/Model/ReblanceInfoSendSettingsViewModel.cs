using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using RebalanceInfoSend.Client.ViewModel;
using RebalanceInfoSend.Client.View;

namespace RebalanceInfoSend.Client.Model
{
    public class ReblanceInfoSendSettingsViewModel : ObservableObject
    {
        public RebalanceInfoSendViewerModel _genericGridViewerModel;
        public RebalanceInfoVisual _genericGridViewerVisual;

        public ReblanceInfoSendSettingsViewModel(RebalanceInfoSendViewerModel genericGridViewerModel, RebalanceInfoVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
