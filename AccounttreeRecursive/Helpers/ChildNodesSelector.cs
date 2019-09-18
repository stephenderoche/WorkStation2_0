using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Grid;
using System.Collections;

namespace AccountTreeCashViewer
{
    public class CustomChildrenSelector : IChildNodesSelector {
        public IEnumerable SelectChildren(object item) {
            if(item is Child)
                return null;
            else if(item is Childlist)
                return (item as Childlist).Child;
          
            return null;
        }
    }
}
