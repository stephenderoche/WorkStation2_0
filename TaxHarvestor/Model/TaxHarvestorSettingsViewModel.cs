using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using TaxHarvestor.Client.ViewModel;
using TaxHarvestor.Client.View;

namespace TaxHarvestor.Client.Model
{
    public class TaxHarvestorSettingsViewModel : ObservableObject
    {
        public TaxHarvestorModel _ViewerModel;
        public TaxHarvestorViewerVisual _ViewerVisual;

        public TaxHarvestorSettingsViewModel(TaxHarvestorModel ViewerModel, TaxHarvestorViewerVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
