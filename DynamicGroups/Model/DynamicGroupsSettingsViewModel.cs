using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using DynamicGroups.Client.ViewModel;
using DynamicGroups.Client.View;

namespace DynamicGroups.Client.Model
{
    public class DynamicGroupsSettingsViewModel : ObservableObject
    {
        public DynamicGroupsViewerModel _ViewerModel;
        public DynamicGroupsVisual _ViewerVisual;

        public DynamicGroupsSettingsViewModel(DynamicGroupsViewerModel ViewerModel, DynamicGroupsVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
