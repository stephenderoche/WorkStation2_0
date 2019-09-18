using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using Guages.Client.ViewModel;
using Guages.Client.View;
using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
using Linedata.Client.Workstation.LongviewAdapterClient;
using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
using Linedata.Client.Workstation.SharedReferences;

using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.UISupport;
using Linedata.Shared.Widget.Common;
using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;


namespace Guages.Client.Model
{
  public  class GuagesSettingViewModel: ObservableObject
    {
      public GuagesViewModel _viewerModel;
      public GuagesView _Viewer;
    
      private const string AccountIdColumnName = "account_id";

      public GuagesSettingViewModel(GuagesViewModel ViewerModel, GuagesView ViewerMainWindow)
        {
            this._viewerModel = ViewerModel;
            this._Viewer = ViewerMainWindow;

           
        }

    

    }
}
