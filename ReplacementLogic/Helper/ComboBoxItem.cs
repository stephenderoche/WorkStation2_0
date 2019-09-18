using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replacement.Client
{
    class ComboBoxItem
    {
        string displayValue;
        Int64 hiddenValue;

        //Constructor
        public ComboBoxItem(string d, Int64 h)
        {
            displayValue = d;
            hiddenValue = h;
        }

        //Accessor
        public Int64 HiddenValue
        {
            get
            {
                return hiddenValue;
            }
        }

        //Override ToString method
        public override string ToString()
        {
            return displayValue;
        }

    }
}
