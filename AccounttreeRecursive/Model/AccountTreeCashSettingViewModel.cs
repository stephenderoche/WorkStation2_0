using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;
using AccountTreeCashViewer.ViewModel;
using AccountTreeCashViewer.View;
using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
using Linedata.Client.Workstation.LongviewAdapterClient;
using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
using Linedata.Client.Workstation.SharedReferences;

using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Framework.WidgetFrame.UISupport;
using Linedata.Shared.Widget.Common;
using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;


namespace AccountTreeCashViewer.Model
{
  public  class AccountTreeCashSettingViewModel: ObservableObject
    {
      public AccountTreeViewModel _ViewerModel;
      public AccountTreeView _ViewerMainWindow;
    
      private const string AccountIdColumnName = "account_id";

      public AccountTreeCashSettingViewModel(AccountTreeViewModel viewerModel, AccountTreeView viewerMainWindow)
        {
            this._ViewerModel = viewerModel;
            this._ViewerMainWindow = viewerMainWindow;

           
        }

    

    }
}
