using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linedata.Framework.WidgetFrame.MvvmFoundation;

namespace IpoViewerAddIn.Client.DetailReport.Model
{
    public class IpoViewerModel : ObservableObject
    {
        public IpoViewerModel(IPOViewerParameters parameters)
        {
            this.Parameters = parameters;
        }

        public IPOViewerParameters Parameters
        {
            get;
            set;
        }
    }
}
