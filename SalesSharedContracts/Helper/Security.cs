namespace SalesSharedContracts
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Linq;

    

    /// <summary>
    /// This is pretty much a verbatim translation of the Security.vb class into C#.
    /// </summary>
    /// 
    /// 
    /// 

   

    public class Security 
    {
        


        public Security(int securityId, string symbol,string issuer,int majorAssetCode,string majorAssetMnemonic)
           
        {
            this.SecurityId = securityId;
            this.Symbol = symbol;
            this.Issuer = issuer;
            this.MajorAssetCode = majorAssetCode;
            this.MajorAssetMnemonic = majorAssetMnemonic;
            
        }

       


       
    
     

        public int SecurityId
        {
            get;
            private set;
        }

        public string Symbol
        {
            get;
            private set;
        }

        public string Issuer
        {
            get;
            private set;
        }

         public int MajorAssetCode
        {
            get;
            private set;
        }

        public string MajorAssetMnemonic
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes members to somewhat reasonable default values. 
        /// </summary>
      

        //public decimal EffectivePriceInUSD
        //{
        //    get
        //    {
        //        return AdjustedPrice * PrincipalFactor * PricingFactor / PrincipalExchangeRate;


        //    }
        //}

        
   
}

     
    }
