using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using DataDashBoard.Client.ViewModel;
using DataDashBoard.Client.View;

namespace DataDashBoard.Client.Model
{
    public class DataDashBoardSettingsViewModel : ObservableObject
    {
        public DataDashBoardModel _ViewerModel;
        public DataDashBoardVisual _ViewerVisual;

        public DataDashBoardSettingsViewModel(DataDashBoardModel ViewerModel, DataDashBoardVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
