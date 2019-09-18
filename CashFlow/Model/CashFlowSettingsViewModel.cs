using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using CashFlow.Client.ViewModel;
using CashFlow.Client.View;

namespace CashFlow.Client.Model
{
    public class CashFlowSettingsViewModel : ObservableObject
    {
        public CashFlowViewerModel _genericGridViewerModel;
        public CashFlowVisual _genericGridViewerVisual;

        public CashFlowSettingsViewModel(CashFlowViewerModel genericGridViewerModel, CashFlowVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
