using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using TraderVsPM.Client.ViewModel;
using TraderVsPM.Client;

namespace TraderVsPM.Client.Model
{
    public class TraderVsPmSettingsViewModel  : ObservableObject
    {
        //public TraderVsPmModel(TraderVsPmParameters parameters)
        //{
        //    this.Parameters = parameters;
        //}

        //public TraderVsPmParameters Parameters 
        //{
        //    get;
        //    set;
        //}

        public TraderVsPmSettingsViewModel _genericGridViewerModel;
        public TraderVsPm _genericGridViewerVisual;

        public TraderVsPmSettingsViewModel(TraderVsPmSettingsViewModel genericGridViewerModel, TraderVsPm genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
