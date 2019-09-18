using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using FIDashBoard.Client.ViewModel;
using FIDashBoard.Client.View;

namespace FIDashBoard.Client.Model
{
    public class FIDashBoardSettingsViewModel : ObservableObject
    {
        public FIDashBoardModel _ViewerModel;
        public FIDashBoardVisual _ViewerVisual;

        public FIDashBoardSettingsViewModel(FIDashBoardModel ViewerModel, FIDashBoardVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
