using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using RestrictedSecurity.Client.ViewModel;
using RestrictedSecurity.Client.View;

namespace RestrictedSecurity.Client.Model
{
    public class RestrictedSecuritySettingsViewModel : ObservableObject
    {
        public RestrictedSecurityViewerModel _genericGridViewerModel;
        public RestrictedSecurityVisual _genericGridViewerVisual;

        public RestrictedSecuritySettingsViewModel(RestrictedSecurityViewerModel genericGridViewerModel, RestrictedSecurityVisual genericGridViewerVisual)
        {
            this._genericGridViewerModel = genericGridViewerModel;
            this._genericGridViewerVisual = genericGridViewerVisual;
           
        }
    }
}
