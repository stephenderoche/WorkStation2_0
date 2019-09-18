using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using PMDashBoard.Client.ViewModel;
using PMDashBoard.Client.View;

namespace PMDashBoard.Client.Model
{
    public class PMDashBoardSettingsViewModel : ObservableObject
    {
        public PMDashBoardModel _ViewerModel;
        public PMDashBoardVisual _ViewerVisual;

        public PMDashBoardSettingsViewModel(PMDashBoardModel ViewerModel, PMDashBoardVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
