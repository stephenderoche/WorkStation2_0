using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using ComplianceDashBoard.Client.ViewModel;
using ComplianceDashBoard.Client.View;

namespace ComplianceDashBoard.Client.Model
{
    public class ComplianceDashBoardSettingsViewModel : ObservableObject
    {
        public ComplianceDashBoardModel _ViewerModel;
        public ComplianceDashBoardVisual _ViewerVisual;

        public ComplianceDashBoardSettingsViewModel(ComplianceDashBoardModel ViewerModel, ComplianceDashBoardVisual ViewerVisual)
        {
            this._ViewerModel = ViewerModel;
            this._ViewerVisual = ViewerVisual;
           
        }
    }
}
