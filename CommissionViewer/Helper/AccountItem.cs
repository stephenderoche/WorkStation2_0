﻿using System;


namespace CommissionViewer.Client
{
    class AccountItem
    {
        string displayValue;
        Int64 hiddenValue;

        //Constructor
        public AccountItem(string d, Int64 h)
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
            return  String.IsNullOrEmpty(displayValue)? "d" : displayValue ;
        }

    }
}
