using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using IPO.Client.ViewModel;
using IPO.Client.View;

namespace IPO.Client.Model
{
    public class IPOSettingsViewModel : ObservableObject
    {
        public IPOModel _ViewerModel;
        public IPOVisual _ViewerVisual;

        public IPOSettingsViewModel(IPOModel ViewerModel, IPOVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
