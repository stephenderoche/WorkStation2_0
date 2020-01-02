using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using MutualFundsCash.Client.ViewModel;
using MutualFundsCash.Client.View;

namespace MutualFundsCash.Client.Model
{
    public class MutualFundsCashSettingsViewModel : ObservableObject
    {
        public MutualFundsCashViewerModel _genericGridViewerModel;
        public MutualFundsCashVisual _genericGridViewerVisual;

        public MutualFundsCashSettingsViewModel(MutualFundsCashViewerModel genericGridViewerModel, MutualFundsCashVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
