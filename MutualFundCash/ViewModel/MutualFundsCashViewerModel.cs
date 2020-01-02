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
using System.Diagnostics;
using System.Windows.Controls;

using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Bars;
using System.Windows.Markup;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.Serialization;
using System.Net.Http;
using System.Threading.Tasks;


namespace MutualFundsCash.Client.ViewModel
{
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;
    using MutualFundsCash.Client.Plugin;
    using MutualFundsCash.Client.View;
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

    public class MutualFundsCashViewerModel : ReactiveObject, IDisposable
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MutualFundsCashViewerModel));
        private const string AccountIdColumnName = "account_id";
        private const string BlockIdColumnName = "block_id";
        private const string SymbolColumnName = "symbol";
        private const string ModelIdColumnName = "model_id"; // how to get model_id?
        private const string SecurityIdColumnName = "security_id";
        private const string TicketIdColumnName = "ticket_id";
        private static List<BasicAccountInfo> accountList;
        private readonly IAccountSummaryDataProvider accountSummaryDataProvider;
        private readonly IAccountSummaryNotifier accountSummaryNotifier;
        private readonly ILongviewAdapterClient longviewAdapterClient;
        private long? accountIdFromParams;
        private Dictionary<string, string> currencySymbols;
        private string currencySymbol;
        private bool disposed = false;
        private readonly IReactivePublisher publisher;
        private BasicAccountInfo selectedAccount;
        private bool? canOpenAppraisalReport;
         
        # region CoreParameters

        public MutualFundsCashdParams Parameters
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

        private long publishedBlock;
        public long PublishedBlock
        {
            get
            {
                return this.publishedBlock;
            }

            set
            {
                this.publishedBlock = value;
                this.RaisePropertyChanged("PublishedBlock");
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
        public MutualFundsCashViewerModel(ILongviewAdapterClient longviewAdapterClient, IAccountSummaryDataProvider accountSummaryDataProvider)
        {

            CreateServiceClient();
            //GetAcc();
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

            if (rowChangedEventArgs.ReportInstance.ReportType == ReportType.TraderBlotter)
            {
                long? blockId = this.GetBlockId(rowChangedEventArgs.NewRow, BlockIdColumnName);

                this.PublishedBlock = Convert.ToInt32(blockId);
                Parameters.SecurityName = this.GetSymbol(rowChangedEventArgs.NewRow, SymbolColumnName);
                SetSelectedAccount(blockId.Value);

            }



            //long? accountId = this.GetAccountId(rowChangedEventArgs.NewRow, AccountIdColumnName);
            //get_account_name(Convert.ToString(accountId));
            //this.PublishedAccount = Convert.ToInt32(accountId);

            //SetSelectedAccount(accountId.Value);


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
                CellNames = new[]
             {
                 AccountIdColumnName,
                 BlockIdColumnName,
                 SymbolColumnName,
                 ModelIdColumnName,
                 SecurityIdColumnName,
                 TicketIdColumnName
             }
        
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

                Logger.Error(e.Message, e);
            }

            return null;
        }
        private long? GetBlockId(Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row, string columnName)
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
                   // Get_BrokerData(Convert.ToInt32(result));
                    return (long)result;
                }
            }
            catch (KeyNotFoundException e)
            {
                //this.workstationLogger.LogException(e);

                Logger.Error(e.Message, e);
            }

            return null;
        }
        private string GetSymbol(Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row, string columnName)
        {
            if (row == null)
            {
                return null;
            }

            try
            {
                
                string cellValue = this.GetCellValue(row, columnName);

                if (cellValue != null)
                {
                    
                    return cellValue;
                }

               
            }
            catch (KeyNotFoundException e)
            {
                //this.workstationLogger.LogException(e);

                Logger.Error(e.Message, e);
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
                Logger.Error(e.Message, e);
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
                    Logger.Error(e.Message, e);
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

    

        #endregion WidgetParameters



        private Rebal_info _rebal_info;

        public Rebal_info Rebalinfo
        {
            get
            {
                return _rebal_info;
            }

            set
            {
                _rebal_info = value;

                this.RaisePropertyChanged("Rebalinfo");
            }
        }

        private int _accountId = -1;
        public int AccountId
        {
            set { _accountId = value; }
            get { return _accountId; }
        }

        private string _reason = "";
        public string Reason
        {
            set { _reason = value; }
            get { return _reason; }
        }


        private string _accountName = string.Empty;
        public string AccountName
        {
            set { _accountName = value; }
            get { return _accountName; }
        }


        private int _desId = -1;
        public int DeskId
        {
            get { return _desId; }
            set { _desId = value; }
        }

        private string _desk = string.Empty;
        public string Desk
        {
            get { return _desk; }
            set { _desk = value; }


        }

        bool _isWaitIndicatorVisible2;

        public bool IsWaitIndicatorVisible2
        {
            get
            {
                return _isWaitIndicatorVisible2;
            }

            set
            {
                _isWaitIndicatorVisible2 = value;

                this.RaisePropertyChanged("IsWaitIndicatorVisible2");
            }
        }
        
        bool _isWaitIndicatorVisible;

        public bool IsWaitIndicatorVisible
        {
            get
            {
                return _isWaitIndicatorVisible;
            }

            set
            {
                _isWaitIndicatorVisible = value;

                this.RaisePropertyChanged("IsWaitIndicatorVisible");
            }
        }


        bool _isSendEnabled = true;

        public bool IsSendEnabled
        {
            get
            {
                return _isSendEnabled;
            }

            set
            {
                _isSendEnabled = value;

                this.RaisePropertyChanged("IsSendEnabled");
            }
        }

        bool _isComplianceEnabled = true;

        public bool IsComplianceEnabled
        {
            get
            {
                return _isComplianceEnabled;
            }

            set
            {
                _isComplianceEnabled = value;

                this.RaisePropertyChanged("IsComplianceEnabled");
            }
        }

        string _processedAccount = string.Empty;

        public string ProcessedAccount
        {
            get
            {
                return _processedAccount;
            }

            set
            {
                _processedAccount = value;

                this.RaisePropertyChanged("ProcessedAccount");
            }
        }

        decimal _security_id = -1;

        public decimal Security_id
        {
            get
            {
                return _security_id;
            }

            set
            {
                _security_id = value;

                this.RaisePropertyChanged("Security_id");
            }
        }


        private IList<string> _accountUniqueIDList;

        public IList<string> AccountUniqueIDList
        {
            get
            {
                return _accountUniqueIDList;
            }

            set
            {
                _accountUniqueIDList = value;

                this.RaisePropertyChanged("AccountUniqueIDList");
            }
        }

        private IList<string> _accountCashIDList;

        public IList<string> AccountCashIDList
        {
            get
            {
                return _accountCashIDList;
            }

            set
            {
                _accountCashIDList = value;

                this.RaisePropertyChanged("AccountCashIDList");
            }
        }

        private IList<decimal> _cashAmountIDList;

        public IList<decimal> CashAmountIDList
        {
            get
            {
                return _cashAmountIDList;
            }

            set
            {
                _cashAmountIDList = value;

                this.RaisePropertyChanged("CashAmountIDList");
            }
        }

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
        //public void GetAcc()
        //{
        //    ThreadPool.QueueUserWorkItem(
        //        delegate(object eventArg)
        //        {

        //            AllAccounts = DBService.GetAllAccounts2();



        //        });
        //}

        public void se_GetAcc(string text)
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
                    AllAccounts = DBService.se_get_list_account(text,out messages);



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
                        se_GetAcc("%");
                        return;
                    }
                    foreach (DataRow row in AllAccounts.Tables[0].Select(textEnteredPlusNew))
                    {
                        object item = row["short_name"];
                        AccountName = Convert.ToString(item);
                        //Parameters.AccountName = AccountName;


                    }

                });
        }

        #region properties


        private DataTable _detail;

        public DataTable Detail
        {
            get { return _detail; }
            set
            {
                _detail = value;
                this.RaisePropertyChanged("Detail");
            }
        }

        private DataTable _compliance;

        public DataTable Compliance
        {
            get { return _compliance; }
            set
            {
                _compliance = value;
                this.RaisePropertyChanged("Compliance");
            }
        }

        private DataTable _linkTable;

        public DataTable LinkTable
        {
            get { return _linkTable; }
            set
            {
                _linkTable = value;
                this.RaisePropertyChanged("LinkTable");
            }
        }

        private DataTable _exclusions;

        public DataTable Exclusions
        {
            get { return _exclusions; }
            set
            {
                _exclusions = value;
                this.RaisePropertyChanged("Exclusions");
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



        private DataTable _sessions;

        public DataTable Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                this.RaisePropertyChanged("Sessions");
            }
        }
       

       

    
       

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set
            {
                // _symbol = string.Format("{0} Last Price:{1}",value, DataDelayedQuotes.delayedPrice);
                _symbol = value;
                this.RaisePropertyChanged("Symbol");
            }
        }
    
        #endregion

        #region HelperProcs




        


        public void AccountCashID(DevExpress.Xpf.Grid.GridControl a) 
        {

            int i = 0;
            decimal total_red;
            decimal total_sub;
            string cashSymbol;
            string cashAccountShort;
            //a.SelectAll();
            int[] listRowList = a.GetSelectedRowHandles();

            AccountCashIDList = new List<string>();
            CashAmountIDList = new List<decimal>();
            for (i = 0; i < listRowList.Length; i++)
            {
                GridColumn colaccount_id = a.Columns["symbol"];
                GridColumn colshort = a.Columns["short_name"];
                GridColumn colred = a.Columns["reds_amount"];
                GridColumn colsub = a.Columns["subs_amount"];
                if (colaccount_id != null)
                {
                    cashSymbol = Convert.ToString(a.GetCellValue(listRowList[i], colaccount_id).ToString());
                    cashAccountShort = Convert.ToString(a.GetCellValue(listRowList[i], colshort).ToString());
                    total_red = Convert.ToDecimal(a.GetCellValue(listRowList[i], colred).ToString());
                    total_sub = Convert.ToDecimal(a.GetCellValue(listRowList[i], colsub).ToString());
                    // _se_send_orders_faster(Convert.ToInt32(_account_id), Convert.ToInt32(Security_id));
                    //SecurityIDList.Add(Security_id);

                    if (cashAccountShort == ProcessedAccount)
                    {
                        if (!AccountCashIDList.Contains(cashSymbol))
                        {
                            AccountCashIDList.Add(cashSymbol);
                            CashAmountIDList.Add(total_red + total_sub);
                        }
                    }
                }

            }
        
        }

        public void AccountUniqueID(DevExpress.Xpf.Grid.GridControl a)
        {

            int i = 0;
        
            string cashSymbol;
            //a.SelectAll();
            int[] listRowList = a.GetSelectedRowHandles();

            AccountUniqueIDList = new List<string>();
      
            for (i = 0; i < listRowList.Length; i++)
            {
                GridColumn colaccount_id = a.Columns["short_name"];
          
                if (colaccount_id != null)
                {
                    cashSymbol = Convert.ToString(a.GetCellValue(listRowList[i], colaccount_id).ToString());
             

                    if (!AccountUniqueIDList.Contains(cashSymbol))
                    {
                        AccountUniqueIDList.Add(cashSymbol);
                       
                    }

                }

            }

        }

        public void addCash(DevExpress.Xpf.Grid.GridControl a)
        {
           
            for (int z = 0; z < AccountUniqueIDList.Count; z++) // Loop through List with for
            {

                ProcessedAccount = AccountUniqueIDList[z];
            AccountCashID( a);

            ApplicationMessageList messages = null;

                for (int x = 0; x < AccountCashIDList.Count; x++) // Loop through List with for
                {

                    DBService.se_add_internal_mf_cash(Convert.ToString(AccountCashIDList[x]), CashAmountIDList[x], out messages);
                }
            }
        }
      


       

      

        #endregion HelperProcs


    }
}


