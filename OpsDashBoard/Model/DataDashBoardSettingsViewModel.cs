using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using OPSDashBoard.Client.ViewModel;
using OPSDashBoard.Client.View;

namespace OPSDashBoard.Client.Model
{
    public class DataDashBoardSettingsViewModel : ObservableObject
    {
        public OPSDashBoardModel _ViewerModel;
        public OPSDashBoardVisual _ViewerVisual;

        public DataDashBoardSettingsViewModel(OPSDashBoardModel ViewerModel, OPSDashBoardVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
