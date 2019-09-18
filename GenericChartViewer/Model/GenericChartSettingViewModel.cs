using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using GenericChart;
using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
using Linedata.Client.Workstation.LongviewAdapterClient;
using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
using Linedata.Client.Workstation.SharedReferences;

using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.UISupport;
using Linedata.Shared.Widget.Common;
using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;


namespace GenericChart
{
  public  class GenericChartSettingViewModel: ObservableObject
    {
      public GenericChartViewModel _topSecuritiesViewerModel;
      public GenericChartView _topSecuritiesViewerMainWindow;
    
      private const string AccountIdColumnName = "account_id";

      public GenericChartSettingViewModel(GenericChartViewModel topSecuritiesViewerModel, GenericChartView topSecuritiesViewerMainWindow)
        {
            this._topSecuritiesViewerModel = topSecuritiesViewerModel;
            this._topSecuritiesViewerMainWindow = topSecuritiesViewerMainWindow;

           
        }

    

    }
}
