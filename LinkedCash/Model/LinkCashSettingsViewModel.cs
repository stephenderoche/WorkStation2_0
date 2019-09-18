using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using LinkCash.Client.ViewModel;
using LinkCash.Client.View;

namespace LinkCash.Client.Model
{
    public class LinkCashSettingsViewModel : ObservableObject
    {
        public LinkCashViewerModel _genericGridViewerModel;
        public LinkCashVisual _genericGridViewerVisual;

        public LinkCashSettingsViewModel(LinkCashViewerModel genericGridViewerModel, LinkCashVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
