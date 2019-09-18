using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using NavContingentDashBoard.Client.ViewModel;
using NavContingentDashBoard.Client.View;

namespace NavContingentDashBoard.Client.Model
{
    public class NavContingentDashBoardSettingsViewModel : ObservableObject
    {
        public NavContingentDashBoardModel _ViewerModel;
        public NavContingentDashBoardVisual _ViewerVisual;

        public NavContingentDashBoardSettingsViewModel(NavContingentDashBoardModel ViewerModel, NavContingentDashBoardVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
