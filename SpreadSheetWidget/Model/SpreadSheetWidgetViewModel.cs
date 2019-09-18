using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using SpreadSheetWidget.Client.ViewModel;
using SpreadSheetWidget.Client.View;

namespace SpreadSheetWidget.Client.Model
{
    public class SpreadSheetWidgetViewModel : ObservableObject
    {
        public SpreadSheetWidgetModel _ViewerModel;
        public SpreadSheetWidgetVisual _ViewerVisual;

        public SpreadSheetWidgetViewModel(SpreadSheetWidgetModel ViewerModel, SpreadSheetWidgetVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
