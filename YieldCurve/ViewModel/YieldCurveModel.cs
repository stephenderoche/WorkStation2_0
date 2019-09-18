using Linedata.Framework.WidgetFrame.MvvmFoundation;
using SalesSharedContracts;
using Linedata.Shared.Api.ServiceModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using System.Windows;

using System.Windows.Controls;

using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Bars;
using System.Windows.Markup;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.Serialization;


namespace YieldCurve.Client.ViewModel
{
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;
    using YieldCurve.Client.Plugin;
    using YieldCurve.Client.View;
    using System.Windows.Media;
    using Linedata.Client.Workstation.LongviewAdapterClient;
    using Linedata.Framework.Foundation;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.IO;

    using log4net;
    using Linedata.Client.Workstation.SharedReferences;
    using ReactiveUI;
    
    

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]

    public class YieldCurveModel : ReactiveObject, IDisposable
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(YieldCurveModel));
        private const string AccountIdColumnName = "account_id";
        private static List<BasicAccountInfo> accountList;
        private readonly IAccountSummaryDataProvider accountSummaryDataProvider;
        private readonly IAccountSummaryNotifier accountSummaryNotifier;
        private readonly ILongviewAdapterClient longviewAdapterClient;
        private long? accountIdFromParams;
        private Dictionary<string, string> currencySymbols;
        private string currencySymbol;
        private bool disposed = false;

        private BasicAccountInfo selectedAccount;
        private bool? canOpenAppraisalReport;

        # region CoreParameters

        public YieldCurveParameters Parameters
        {
            get;
            set;
        }


        private static readonly ReportType[] SubscribedReportTypes =
        {
            ReportType.Appraisal,
            ReportType.OrderPreview,
            ReportType.ManagerBlotter,
            ReportType.SecurityXRef,
            ReportType.TraderBlotter,
            ReportType.CashBalance,
            ReportType.TicketSummary
        };

        private long publishedAccount;
        public long PublishedAccount
        {
            get
            {
                return this.publishedAccount;
            }

            set
            {
                this.publishedAccount = value;
                this.RaisePropertyChanged("PublishedAccount");
            }
        }

        public List<BasicAccountInfo> AccountList
        {
            get
            {
                return accountList;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref accountList, value);


            }
        }

        public BasicAccountInfo SelectedAccount
        {
            get
            {
                return this.selectedAccount;
            }

            set
            {
                // unsubscribe to previously selected account
                if (this.selectedAccount != null)
                {
                    this.accountSummaryDataProvider.UnsubscribeToUpdates((long)this.selectedAccount.Id);
                }

                // display the new account summary
                this.RaiseAndSetIfChanged(ref this.selectedAccount, value);



                if (this.selectedAccount != null)
                {
                    this.UpdateAccountSummary((long)this.selectedAccount.Id);
                    //// subscribe to newly selected account
                    this.accountSummaryDataProvider.SubscribeToUpdates((long)this.selectedAccount.Id);
                    //// publish selected account to other listening widgets
                    //this.publisher.Publish(new WidgetMessages.AccountChanged(this.publisher.GetTabId(this.ParentWidget), this.ParentWidget.Group, (long)this.selectedAccount.Id));
                }
            }

        }

        public IWidget ParentWidget { get; set; }

        public WidgetGroups Group { get; set; }
        private string _mv;
        public string MV
        {
            get
            {
                return this._mv;
            }

            set
            {
                this._mv = value;
                this.RaisePropertyChanged("MV");
            }
        }

        # endregion CoreParameters

        [ImportingConstructor]
        public YieldCurveModel(ILongviewAdapterClient longviewAdapterClient, IAccountSummaryDataProvider accountSummaryDataProvider)
        {
            CreateServiceClient();
            GetAcc();
            this.accountSummaryDataProvider = accountSummaryDataProvider;
            this.accountSummaryNotifier = this.accountSummaryDataProvider.AccountSummaryNotifier;
            this.longviewAdapterClient = longviewAdapterClient;
            this.longviewAdapterClient.RowChanged += this.RowChanged;
            this.accountSummaryNotifier.AccountSummaryUpdated += this.AccountSummaryUpdated;
            this.longviewAdapterClient.ActiveReportChanged += this.ActiveReportChanged;
            this.longviewAdapterClient.ReportInstanceRefresh += this.ReportInstanceRefresh;
            this.longviewAdapterClient.AccountChanged += this.AccountChanged;
            this.SubscribeToLongview();
        }


        #region CoreMethods
        public void UpdateAccountSummary(long accountId)
        {
            this.accountSummaryDataProvider.GetDetailAccountInfo(accountId, this.EndGetAccountSummaryInfos);
        }
        private void RowChanged(object sender, RowChangedEventArgs rowChangedEventArgs)
        {
            // only gray group should listen to LV reports
            if (this.Group == WidgetGroups.Gray)
            {
                this.RowSelectionChanged(sender, rowChangedEventArgs);
            }
        }
        private void RowSelectionChanged(object sender, RowChangedEventArgs rowChangedEventArgs)
        {
            if (rowChangedEventArgs == null || rowChangedEventArgs.NewRow == null || rowChangedEventArgs.ReportInstance == null)
            {
                return;
            }

            if (!this.IsSubscribingToReportType(rowChangedEventArgs.ReportInstance.ReportType))
            {
                return;
            }

            if (rowChangedEventArgs.ReportInstance.ReportType == ReportType.TicketSummary)
            {
                return;
            }



            long? accountId = this.GetAccountId(rowChangedEventArgs.NewRow, AccountIdColumnName);
            get_account_name(Convert.ToString(accountId));
            this.PublishedAccount = Convert.ToInt32(accountId);

            SetSelectedAccount(accountId.Value);


            // this.TopSecuritiesViewerWindow.Securitieschart();
        }
        internal void SetSelectedAccount(long accountId)
        {
            if (this.AccountList == null)
            {
                this.accountIdFromParams = accountId;

            }
            else
            {
                this.SelectedAccount = this.AccountList.FirstOrDefault(x => x.Id == accountId);
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.longviewAdapterClient != null)
                    {
                        this.longviewAdapterClient.UnSubscribe();
                        this.longviewAdapterClient.RowChanged -= this.RowChanged;
                        this.longviewAdapterClient.ActiveReportChanged += this.ActiveReportChanged;
                        this.longviewAdapterClient.ReportInstanceRefresh += this.ReportInstanceRefresh;
                        this.longviewAdapterClient.AccountChanged += this.AccountChanged;
                        this.longviewAdapterClient.Dispose();
                    }

                    if (this.accountSummaryDataProvider != null)
                    {
                        this.accountSummaryDataProvider.Dispose();
                    }

                    if (this.SelectedAccount != null)
                    {
                        //// this.accountSummaryDataProvider.UnsubscribeToUpdates((long)this.selectedAccount.Id);
                        this.accountSummaryNotifier.AccountSummaryUpdated -= this.AccountSummaryUpdated;
                    }
                }

                this.disposed = true;
            }
        }
        private void EndGetAccountSummaryInfos(DetailAccountInfo detailAccountInfo)
        {
            if (detailAccountInfo == null)
            {
                return;
            }



            this.CurrencySymbol = this.GetCurrencySymbol(detailAccountInfo.AccountBaseCurrencyMnemonic);
        }
        private string GetCurrencySymbol(string code)
        {
            string symbol = string.Empty;

            if (this.currencySymbols.TryGetValue(code, out symbol))
            {
                return symbol;
            }

            //// search for culture 
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures))
            {
                if (culture.IsNeutralCulture || culture.Name.Length == 0)
                {
                    continue;
                }

                RegionInfo region = new RegionInfo(culture.LCID);

                if (string.Equals(region.ISOCurrencySymbol, code, StringComparison.InvariantCultureIgnoreCase))
                {
                    symbol = region.CurrencySymbol;
                    break;
                }
            }

            this.currencySymbols.Add(code, symbol);
            return symbol;
        }
        public string CurrencySymbol
        {
            get
            {
                return this.currencySymbol;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.currencySymbol, value);
            }
        }
        private void AccountSummaryUpdated(object sender, AccountChangedEventArgs accountChangedEventArgs)
        {
            if (accountChangedEventArgs == null || this.SelectedAccount == null
                || accountChangedEventArgs.AccountId != this.SelectedAccount.Id)
            {
                // user has selected a different account
                return;
            }

            UIHelper.RunOnUIThread(() => this.UpdateAccountSummary(accountChangedEventArgs.AccountId));
        }
        private void AcountSelectionChanged(object sender, AccountChangedEventArgs accountChangedEventArgs)
        {
            if (accountChangedEventArgs == null || accountChangedEventArgs.AccountId == 0)
            {
                return;
            }

            this.SetSelectedAccountAsRequired(accountChangedEventArgs.AccountId);
        }
        private void AccountChanged(object sender, AccountChangedEventArgs accountChangedEventArgs)
        {
            // only gray group should listen to LV reports
            if (this.ParentWidget.Group == WidgetGroups.Gray)
            {
                this.AcountSelectionChanged(sender, accountChangedEventArgs);
            }
        }
        private void SetSelectedAccountAsRequired(long? accountId)
        {
            if (accountId.HasValue)
            {
                if (this.selectedAccount != null && accountId == this.selectedAccount.Id)
                {
                    return;
                }

                UIHelper.RunOnUIThread(() => this.SetSelectedAccount(accountId.Value));
            }

        }
        private void ReportInstanceRefreshTriggered(object sender, ReportInstanceRefreshEventArgs reportInstanceRefreshEventArgs)
        {
            if (reportInstanceRefreshEventArgs == null || reportInstanceRefreshEventArgs.Report == null || reportInstanceRefreshEventArgs.Report.ReportGrid == null)
            {
                return;
            }

            if (!this.IsSubscribingToReportType(reportInstanceRefreshEventArgs.Report.ReportType))
            {
                return;
            }

            if (reportInstanceRefreshEventArgs.Report.ReportType == ReportType.TicketSummary
                || reportInstanceRefreshEventArgs.Report.ReportType == ReportType.TraderBlotter)
            {
                return;
            }

            long? accountId = this.GetAccountId(reportInstanceRefreshEventArgs.Report.ReportGrid.AllRows, AccountIdColumnName);
            get_account_name(Convert.ToString(accountId));
            this.SetSelectedAccountAsRequired(accountId);
        }
        private void ReportInstanceRefresh(object sender, ReportInstanceRefreshEventArgs reportInstanceRefreshEventArgs)
        {
            // only gray group should listen to LV reports
            if (this.ParentWidget.Group == WidgetGroups.Gray)
            {
                this.ReportInstanceRefreshTriggered(sender, reportInstanceRefreshEventArgs);
            }
        }
        private void ActiveReportChanged(object sender, ActiveReportChangedEventArgs activeReportChangedEventArgs)
        {
            // only gray group should listen to LV reports
            if (this.ParentWidget.Group == WidgetGroups.Gray)
            {
                this.ActiveReportSelectionChanged(sender, activeReportChangedEventArgs);
            }
        }
        private void ActiveReportSelectionChanged(object sender, ActiveReportChangedEventArgs activeReportChangedEventArgs)
        {
            if (activeReportChangedEventArgs == null || activeReportChangedEventArgs.NewReport == null || activeReportChangedEventArgs.NewReport.ReportGrid == null)
            {
                return;
            }

            if (!this.IsSubscribingToReportType(activeReportChangedEventArgs.NewReport.ReportType))
            {
                return;
            }

            if (activeReportChangedEventArgs.NewReport.ReportType == ReportType.TicketSummary
                || activeReportChangedEventArgs.NewReport.ReportType == ReportType.TraderBlotter)
            {
                return;
            }

            long? accountId = this.GetAccountId(activeReportChangedEventArgs.NewReport.ReportGrid.AllRows, AccountIdColumnName);
            get_account_name(Convert.ToString(accountId));


            this.SetSelectedAccountAsRequired(accountId);
        }
        private bool IsSubscribingToReportType(ReportType reportType)
        {
            return SubscribedReportTypes.Contains(reportType);
        }
        private void SubscribeToLongview()
        {
            SubscriptionParameters subscriptionParameters = new SubscriptionParameters()
            {
                CellNames = new[] { AccountIdColumnName }
            };

            this.longviewAdapterClient.Subscribe(subscriptionParameters);
        }
        private long? GetAccountId(Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row, string columnName)
        {
            if (row == null)
            {
                return null;
            }

            try
            {
                decimal result;
                string cellValue = this.GetCellValue(row, columnName);
                if (decimal.TryParse(cellValue, out result))
                {
                    return (long)result;
                }
            }
            catch (KeyNotFoundException e)
            {
                //this.workstationLogger.LogException(e);

                Logger.DisplayError(e.Message, e);
            }

            return null;
        }
        private long? GetAccountId(IList<Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow> gridRows, string columnName)
        {
            if (gridRows == null)
            {
                return null;
            }

            Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row = gridRows.FirstOrDefault(x => x.RowType == RowType.Detail);

            if (row == null)
            {
                return null;
            }

            try
            {
                decimal result;
                string cellValue = row.GetCellValue(columnName);
                if (decimal.TryParse(cellValue, out result))
                {
                    return (long)result;
                }
            }
            catch (KeyNotFoundException e)
            {
                Logger.DisplayError(e.Message, e);
            }

            return null;
        }
        private string GetCellValue(Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row, string columnName)
        {
            string result = string.Empty;

            if (row != null && row.RowType == RowType.Detail)
            {
                try
                {
                    result = row.GetCellValue(columnName);
                }
                catch (KeyNotFoundException e)
                {
                    Logger.DisplayError(e.Message, e);
                }
            }

            return result;
        }
        public void OnOpenAppraisalReport(long accountID)
        {
            this.longviewAdapterClient.OpenAppraisal(accountID);
        }
        private bool CanOpenAppraisalReport()
        {
            if (!this.canOpenAppraisalReport.HasValue)
            {
                if (this.SelectedAccount != null)
                {
                    this.canOpenAppraisalReport =
                        this.longviewAdapterClient.CanOpenAppraisal((long)this.SelectedAccount.Id);
                }
            }

            return this.canOpenAppraisalReport ?? false;
        }
        #endregion CoreMethods

        #region WidgetParameters
        private ISalesSharedContracts _dbservice;
        public ISalesSharedContracts DBService
        {
            set { _dbservice = value; }
            get { return _dbservice; }
        }
        private DataSet m_AllAccounts;
        public DataSet AllAccounts
        {
            set { m_AllAccounts = value; }
            get { return m_AllAccounts; }
        }

        private int _accountId = -1;
        public int AccountId
        {
            set { _accountId = value; }
            get { return _accountId; }
        }

        private string _accountName = string.Empty;
        public string AccountName
        {
            set { _accountName = value; }
            get { return _accountName; }
        }

        #endregion WidgetParameters

        public ISalesSharedContracts CreateServiceClient()
        {
            try
            {
                DBService = ApiAccessor.Get<ISalesSharedContracts>();
                return DBService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetAcc()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {

                    AllAccounts = DBService.GetAllAccounts2();



                });
        }
        public void get_account_name(string account_id)
        {
            string textEnteredPlusNew = "account_id = " + account_id;

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    var someObject = AllAccounts;
                    if (someObject == null)
                    {
                        GetAcc();
                        return;
                    }
                    foreach (DataRow row in AllAccounts.Tables[0].Select(textEnteredPlusNew))
                    {
                        object item = row["short_name"];
                        AccountName = Convert.ToString(item);
                        Parameters.AccountName = AccountName;


                    }

                });
        }


       
       

      

      
        
    


     

        public void Get_yield_curve()
        {



            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet dsRoboDrift = new DataSet();
                    DataSet dsRoboDrift17 = new DataSet();
                    DataSet dsRoboDrift16 = new DataSet();
                    DataSet dsRoboDrift15 = new DataSet();
                    DataSet dsParallel = new DataSet();

                    //Transaction


                    dsRoboDrift = DBService.se_get_yield_curve(
                        "Yield Curve",
                        out messages);

                    MyDataTable = dsRoboDrift.Tables[0];

                    dsRoboDrift17 = DBService.se_get_yield_curve(
                        "2017 Yield Curve",
                        out messages);

                    MyDataTable2017 = dsRoboDrift17.Tables[0];

                    dsRoboDrift16 = DBService.se_get_yield_curve(
                        "2016 Yield Curve",
                        out messages);

                    MyDataTable2016 = dsRoboDrift16.Tables[0];

                    dsRoboDrift15 = DBService.se_get_yield_curve(
                        "2015 Yield Curve",
                        out messages);

                    MyDataTable2015 = dsRoboDrift15.Tables[0];

                  

                });

        }

        public void Get_parallel_curve()
        {

      
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
                 
                    DataSet dsParallel = new DataSet();

                    dsParallel = DBService.se_get_parallel_shift(
                        "Yield Curve",
                        Convert.ToDecimal(ShiftChange),
                        out messages);

                    MyDataTableParallel = dsParallel.Tables[0];
                });

        }


        public void Get_position_curve(int account_id)
        {


            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;

                    DataSet dsParallel = new DataSet();

                    dsParallel = DBService.se_get_position_curve(
                       
                        account_id,
                        out messages);

                    PositionCurve = dsParallel.Tables[0];
                });

        }

        public void Get_bond_change(int account_id)
        {


            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;

                    DataSet dsBondChange = new DataSet();
                    DataSet dsDistributionYield = new DataSet();
                    DataSet dsDistributionDuration= new DataSet();
                    DataSet dsDistributionRating = new DataSet();
                    DataSet dsDistributionSector = new DataSet();
                    DataSet dsContYield = new DataSet();
                    DataSet dsContDuration = new DataSet();

                    dsBondChange = DBService.se_get_bond_change(

                        account_id,
                        ShiftChange/100,
                        out messages);

                    BondChange = dsBondChange.Tables[0];
                    GetChangesInMarketValue();


                    dsDistributionDuration = DBService.se_get_analytic_ditribution(

                        account_id,
                        1,
                        out messages);

                    DistributionDuration = dsDistributionDuration.Tables[0];

                    dsDistributionRating = DBService.se_get_analytic_ditribution(

                        account_id,
                        2,
                        out messages);

                    DistributionRating = dsDistributionRating.Tables[0];

                    dsDistributionYield = DBService.se_get_analytic_ditribution(

                        account_id,
                        3,
                        out messages);

                    DistributionYield = dsDistributionYield.Tables[0];


                    dsDistributionRating = DBService.se_get_analytic_ditribution(

                        account_id,
                        2,
                        out messages);

                    DistributionRating = dsDistributionRating.Tables[0];



                    dsDistributionSector = DBService.se_get_analytic_ditribution(

                        account_id,
                        4,
                        out messages);

                    DistributionSector = dsDistributionSector.Tables[0];



                    dsContDuration = DBService.se_get_contibution_ditribution(

                        account_id,
                        1,
                        out messages);

                    ContDuration= dsContDuration.Tables[0];

                    foreach (DataRow dr in ContDuration.Rows)
                    {
                        _weightedDuration = dr["WA_Duration"].ToString();
                        WeightedDuration = string.Format("Weighted Duration: {0:n4}", _weightedDuration);
                        break;
                        
                    }

                    dsContYield = DBService.se_get_contibution_ditribution(

                        account_id,
                        2,
                        out messages);

                    ContYield = dsContYield.Tables[0];


                    foreach (DataRow dr in ContYield.Rows)
                    {
                        _weightedYield = dr["WA_Yield"].ToString();
                        WeightedYield = string.Format("Weighted Yield: {0:n4}", _weightedYield);
                        break;
                        
                    }

                


                });

        }


        public void GetChangesInMarketValue()
        {

            double sum = 0;
            double sumAdjusted = 0;
            double Direction = 1;
            
          

                foreach (DataRow dr in BondChange.Rows)
            {
                sum += double.Parse(dr["PVOfBond"].ToString());
                sumAdjusted += double.Parse(dr["NewValue"].ToString());
            }

            if (ShiftChange > 1)
            {
                Direction = sumAdjusted - sum;
               
            }

            else
            {
                Direction = -1 * (sum - sumAdjusted);
                
            }

            //PVOfBond = string.Format("Current Market Value: {0:n2}   Change in MV: {1:n2} - ({2:p2})  New Market Value Due to Shift: {3:n2} ", sum, Direction, 1- (sum / sumAdjusted), sumAdjusted);
            PVOfBond = string.Format("{0:n2}",sum);
            //AdjPortfolioMarket = string.Format("{1:n2}  {2:p2}", Direction, 1 - (sum / sumAdjusted));
            AdjPortfolioMarket = string.Format("{0:n2}  {1:p2}", Direction, 1 - (sum / sumAdjusted));
            ShiftPortfolioMarket = string.Format("{0:n2}", sumAdjusted);
        }


        #region Parametere

      

       
private string _pVOfBond;
public string PVOfBond
    {
    set
    {
               

        _pVOfBond = value;
        this.RaisePropertyChanged("PVOfBond");
    }
    get { return _pVOfBond; }
    }

       
        private string _adjBond;
        public string AdjBond
        {
            set
            {


                _adjBond = value;
                this.RaisePropertyChanged("AdjBond");
            }
            get { return _adjBond; }
        }

        private string _adjPortfolioMarket;
        public string AdjPortfolioMarket
        {
            set
            {


                _adjPortfolioMarket = value;
                this.RaisePropertyChanged("AdjPortfolioMarket");
            }
            get { return _adjPortfolioMarket; }
        }

        private string _shiftPortfolioMarket;
        public string ShiftPortfolioMarket
        {
            set
            {
                _shiftPortfolioMarket = value;
                this.RaisePropertyChanged("ShiftPortfolioMarket");
            }
            get { return _shiftPortfolioMarket; }
        }



        private string _weightedDuration = "Weighted Duration:";

        public string WeightedDuration
        {
            get { return _weightedDuration; }
            set
            {
                _weightedDuration = value;
                this.RaisePropertyChanged("WeightedDuration");
            }
        }

        private string _weightedYield = "Weighted Yield:";

        public string WeightedYield
        {
            get { return _weightedYield; }
            set
            {
                _weightedYield = value;
                this.RaisePropertyChanged("WeightedYield");
            }
        }



        private DataTable contDuration;

        public DataTable ContDuration
        {
            get { return contDuration; }
            set
            {
                contDuration = value;
                this.RaisePropertyChanged("ContDuration");
            }
        }

        private DataTable contYield;

        public DataTable ContYield
        {
            get { return contYield; }
            set
            {
                contYield = value;
                this.RaisePropertyChanged("ContYield");
            }
        }
  


        private DataTable myDataTable;

        public DataTable MyDataTable
        {
            get { return myDataTable; }
            set
            {
                myDataTable = value;
                this.RaisePropertyChanged("MyDataTable");
            }
        }

        private DataTable myDataTable2017;

        public DataTable MyDataTable2017
        {
            get { return myDataTable2017; }
            set
            {
                myDataTable2017 = value;
                this.RaisePropertyChanged("MyDataTable2017");
            }
        }

        private DataTable myDataTable2016;

        public DataTable MyDataTable2016
        {
            get { return myDataTable2016; }
            set
            {
                myDataTable2016 = value;
                this.RaisePropertyChanged("MyDataTable2016");
            }
        }

        private DataTable myDataTable2015;

        public DataTable MyDataTable2015
        {
            get { return myDataTable2015; }
            set
            {
                myDataTable2015 = value;
                this.RaisePropertyChanged("MyDataTable2015");
            }
        }

        private DataTable myDataTableParallel;

        public DataTable MyDataTableParallel
        {
            get { return myDataTableParallel; }
            set
            {
                
                myDataTableParallel = value;
                this.RaisePropertyChanged("MyDataTableParallel");
            }
        }


        private DataTable bondChange;

        public DataTable BondChange
        {
            get { return bondChange; }
            set
            {

                bondChange = value;
                this.RaisePropertyChanged("BondChange");
            }
        }


        private DataTable positionCurve;

        public DataTable PositionCurve
        {
            get { return positionCurve; }
            set
            {

                positionCurve = value;
                this.RaisePropertyChanged("PositionCurve");
            }
        }



        private DataTable distributionSector;

        public DataTable DistributionSector
        {
            get { return distributionSector; }
            set
            {
                distributionSector = value;
                this.RaisePropertyChanged("DistributionSector");
            }
        }

        private DataTable distributionDuration;

        public DataTable DistributionDuration
        {
            get { return distributionDuration; }
            set
            {
                distributionDuration = value;
                this.RaisePropertyChanged("distributionDuration");
            }
        }

        private DataTable distributionYield;

        public DataTable DistributionYield
        {
            get { return distributionYield; }
            set
            {
                distributionYield = value;
                this.RaisePropertyChanged("DistributionYield");
            }
        }

        private DataTable distributionRating;

        public DataTable DistributionRating
        {
            get { return distributionRating; }
            set
            {
                distributionRating = value;
                this.RaisePropertyChanged("DistributionRating");
            }
        }

        private decimal _shiftChange = 100;
        public decimal ShiftChange
        {
            set
            {
               

                _shiftChange = value;
                this.RaisePropertyChanged("ShiftChange");
            }
            get { return _shiftChange; }
        }

       


        #endregion Parameters

       
      
    }
}
