namespace SalesSharedContracts.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Linedata.Server.Interfaces;
    using Linedata.Shared.ReferenceData;

    public class AccountParameters : IAccountParams
    {
        public bool IsExcluded { get; set; }

        public double CashTarget { get; set; }

        public int CashTargetCurrencyId { get; set; }

        /// <summary>Gets the note field set for this account.</summary>
        public string Note { get; set; }

        /// <summary>Gets UserField1 for the account's rebalancer params.  This can return null.</summary>
        public object UserField1 { get; set; }

        /// <summary>Gets UserField2 for the account's rebalancer params.  This can return null.</summary>
        public object UserField2 { get; set; }

        /// <summary>Gets UserField3 for the account's rebalancer params.  This can return null.</summary>
        public object UserField3 { get; set; }

        /// <summary>Gets UserField4 for the account's rebalancer params.  This can return null.</summary>
        public object UserField4 { get; set; }

        /// <summary>Gets UserField5 for the account's rebalancer params.  This can return null.</summary>
        public object UserField5 { get; set; }

        /// <summary>Gets UserField6 for the account's rebalancer params.  This can return null.</summary>
        public object UserField6 { get; set; }

        /// <summary>Gets UserField7 for the account's rebalancer params.  This can return null.</summary>
        public object UserField7 { get; set; }

        /// <summary>Gets UserField8 for the account's rebalancer params.  This can return null.</summary>
        public object UserField8 { get; set; }

        // Methods
        public decimal GetSectorCash(int sectorId)
        {
            return 0;
        }

        public decimal GetSecurityExcludedQuantity(int securityId, PositionType positionType)
        {
            return 0;
        }

        /// <summary>Obsolete along with ISecurity </summary>
        //// [Obsolete("ISecurity has been deprecated.")]
        public bool IsSecurityUnchecked(ISecurity security, PositionType positionType)
        {
            return false;
        }

        /// <summary>
        ///  Used for securities selection in Advanced rebalancer wizard
        /// </summary>
        public bool IsSecurityUncheckedAdvanced(int securityId, PositionType positionType)
        {
            return false;
        }

        /// <summary>
        ///  Used for securities selection in Appraisal - client has special logic for securities with boo
        /// </summary>
        public bool IsSecurityIncludedAppraisal(int securityId, PositionType positionType)
        {
            return true;
        }

        /// <summary>
        ///  Used for securities selection in Appraisal
        /// </summary>
        public bool IsSecurityExcludedAppraisal(int securityId, PositionType positionType)
        {
            return false;
        }

        public bool IsSectorUnchecked(ISector sector)
        {
            return false;
        }
    }
}