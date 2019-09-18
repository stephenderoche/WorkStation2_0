using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using ModelHelper.Client.ViewModel;
using ModelHelper.Client.View;

namespace ModelHelper.Client.Model
{
    public class ModelHelperSettingsViewModel : ObservableObject
    {
        public ModelHelperModel _ViewerModel;
        public ModelHelperVisual _ViewerVisual;

        public ModelHelperSettingsViewModel(ModelHelperModel ViewerModel, ModelHelperVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
