using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace OrderImportPane.Client.Model
{
  public  class OrderImportPaneViewerSettingViewModel : ObservableObject
    {
      public OrderImportPaneViewModel _topSecuritiesViewerModel;
      public OrderImportControl _topSecuritiesViewerMainWindow;

      public OrderImportPaneViewerSettingViewModel(OrderImportPaneViewModel topSecuritiesViewerModel, OrderImportControl topSecuritiesViewerMainWindow)
        {
            this._topSecuritiesViewerModel = topSecuritiesViewerModel;
            this._topSecuritiesViewerMainWindow = topSecuritiesViewerMainWindow;
           
        }

    }
}
