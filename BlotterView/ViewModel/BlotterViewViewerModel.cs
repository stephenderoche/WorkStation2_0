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
using System.Net.Http;




namespace BlotterView.Client.ViewModel
{
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;
    using BlotterView.Client.Plugin;
    using BlotterView.Client.View;
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

    public class BlotterViewViewerModel : ReactiveObject, IDisposable
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(BlotterViewViewerModel));
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

        public BlotterViewParams Parameters
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
        public BlotterViewViewerModel(ILongviewAdapterClient longviewAdapterClient, IAccountSummaryDataProvider accountSummaryDataProvider)
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

                Logger.DisplayError(e.Message, e);

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
                    Get_BrokerData(Convert.ToInt32(result));
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
                    GetChecks();
                    return cellValue;
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
                        //Parameters.AccountName = AccountName;


                    }

                });
        }





        ObservableCollection<HistoricalDataResponse> _DataSeries = new ObservableCollection<HistoricalDataResponse>();
        PriceDataResponse _dataDelayedQuotes = new PriceDataResponse();
        ObservableCollection<Dividend> _dividend = new ObservableCollection<Dividend>();


        #region properties


        private DataTable _byBrokerDateTable;

        public DataTable ByBrokerDateTable
        {
            get { return _byBrokerDateTable; }
            set
            {
                _byBrokerDateTable = value;
                this.RaisePropertyChanged("ByBrokerDateTable");
            }
        }

        private DataTable _brokerDateTable;

        public DataTable BrokerDateTable
        {
            get { return _brokerDateTable; }
            set
            {
                _brokerDateTable = value;
                this.RaisePropertyChanged("BrokerDateTable");
            }
        }


        private DataTable _brokerTable;

        public DataTable BrokerTable
        {
            get { return _brokerTable; }
            set
            {
                _brokerTable = value;
                this.RaisePropertyChanged("BrokerTable");
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

       

       

        private string _newProperty;
        public string NewProperty
        {

            get
            {
                return string.Format("Symbol: {0}  Price: {1}  Bid: {2}  Ask: {3}", Parameters.SecurityName, DataDelayedQuotes.latestPrice, DataDelayedQuotes.iexBidPrice, DataDelayedQuotes.iexAskPrice);
                // return string.Format("You are <span style='color:red'>{0}</span> km. in city <span style='color:red'>{1}</span> km.", Symbol, DataDelayedQuotes.delayedPrice);
            }
            set
            {
                // _symbol = string.Format("{0} Last Price:{1}",value, DataDelayedQuotes.delayedPrice);
                _newProperty = string.Format("Sybmol:{0} Price:{1} Bid:{2} Ask: {3}", Parameters.SecurityName, DataDelayedQuotes.latestPrice, DataDelayedQuotes.iexBidPrice, DataDelayedQuotes.iexAskPrice);
                _newProperty = value;
                this.RaisePropertyChanged("NewProperty");
            }
        }

        private string _dividendRunAmount;

        public string DividendRunAmount
        {
            get { return _dividendRunAmount; }
            set { _dividendRunAmount = value; }
        }

        private string _runAmount;

        public string RunAmount
        {
            get { return _runAmount; }
            set { _runAmount = value; }
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
        public ObservableCollection<HistoricalDataResponse> DataSeries
        {
            get { return _DataSeries; }
            set
            {
                _DataSeries = value;
                this.RaisePropertyChanged("DataSeries");
            }
        }
        public PriceDataResponse DataDelayedQuotes
        {
            get { return _dataDelayedQuotes; }

            set
            {
                _dataDelayedQuotes = value;
                this.RaisePropertyChanged("DataDelayedQuotes");
            }
        }

        public ObservableCollection<Dividend> Dividends
        {
            get { return _dividend; }
            set
            {
                _dividend = value;
                this.RaisePropertyChanged("Dividends");
            }
        }
        #endregion

        #region HelperProcs
      


        public void Get_DeskData(Int32 _desk_id)
        {

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet dsRoboDrift = new DataSet();

                    //Transaction


                    dsRoboDrift = DBService.se_get_desk_trades(
                        Convert.ToInt32(_desk_id),
                        out messages);

                    MyDataTable = dsRoboDrift.Tables[0];

                });





        }

        public void Get_BrokerData(Int32 _block_id)
        {

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
                    DateTime nowDate = DateTime.Now;

                    DataSet dsRoboDrift = new DataSet();

                    //Transaction


                    dsRoboDrift = DBService.se_get_execution_info(
                        Convert.ToInt32(_block_id),
                        out messages);

                    BrokerTable = dsRoboDrift.Tables[0];
                    BrokerDateTable = dsRoboDrift.Tables[1];

                });





        }

   
        public void GetData()
        {

            if (RunAmount == null)

                RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/5y?token=pk_27cc9154604842ed9e604f1ef7f492b5", "STT");
           

            //RunAmount = string.Format(RunAmount, symbol);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(RunAmount);
                HttpResponseMessage response = client.GetAsync(RunAmount).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {

                    DataSeries = response.Content.ReadAsAsync<ObservableCollection<HistoricalDataResponse>>().GetAwaiter().GetResult();


                }
            }


        }

        public void GetDelayedQuote()
        {
            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/quote?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(RunAmount);
                HttpResponseMessage response = client.GetAsync(RunAmount).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {

                    DataDelayedQuotes = response.Content.ReadAsAsync<PriceDataResponse>().GetAwaiter().GetResult();
                    NewProperty = _symbol;

                }
            }


        }

        public void GetDividend()
        {

            if(DividendRunAmount == null)
                return;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(DividendRunAmount);
                HttpResponseMessage response = client.GetAsync(DividendRunAmount).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {

                    Dividends = response.Content.ReadAsAsync<ObservableCollection<Dividend>>().GetAwaiter().GetResult();

                }
            }


        }


        public void GetChecks()
        {

            //ThreadPool.QueueUserWorkItem(
            //    delegate(object eventArg)
            //    {
                    if (Parameters != null && Parameters.SecurityName != null)
                    {
                        if (Parameters.OneDay == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/1d?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);



                        }
                        else if (Parameters.FiveDay == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/5d?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            //_vm.DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/1m?token=pk_27cc9154604842ed9e604f1ef7f492b5", _vm.Symbol);

                        }
                        else if (Parameters.OneMonth == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/1m?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            //_vm.DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/1m?token=pk_27cc9154604842ed9e604f1ef7f492b5", _vm.Symbol);

                        }
                        else if (Parameters.ThreeMonth == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/3m?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/3m?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

                        }
                        else if (Parameters.SixMonth == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/6m?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/6m?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

                        }
                        else if (Parameters.YTD == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/ytd?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/ytd?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

                        }
                        else if (Parameters.OneYear == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/1y?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/1y?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

                        }
                        else if (Parameters.TwoYear == true)
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/2y?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/2y?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

                        }
                        else
                        {
                            RunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/chart/5y?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);
                            DividendRunAmount = string.Format("https://cloud.iexapis.com/stable/stock/{0}/dividends/5y?token=pk_27cc9154604842ed9e604f1ef7f492b5", Parameters.SecurityName);

                        }





                        GetData();
                        GetDelayedQuote();
                        GetDividend();

                    }


               // });//end thread




        }

        public void se_get_execution_info_by_broker(Int32 block_id, Int32 broker_id)
        {

            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;



                    //Model
                    DataSet dsSecurities = new DataSet();

                    dsSecurities = DBService.se_get_execution_info_by_broker(
                        block_id,
                        broker_id,
                        out messages);

                    if (dsSecurities.Tables.Count > 0)
                    {

                        ByBrokerDateTable = dsSecurities.Tables[0];

                    }

                });



        }
        #endregion HelperProcs

     
    }
}

public class Dividend
{
    public string exDate { get; set; }

    public string paymentDate { get; set; }
    public string recordDate { get; set; }
    public string declaredDate { get; set; }
    public decimal? amount { get; set; }
    public string type { get; set; }
    public string qualified { get; set; }



}
public class PriceDataResponse
{
    //public string symbol { get; set; }

    //public decimal? delayedPrice { get; set; }
    //public decimal? high { get; set; }
    //public decimal? low { get; set; }
    //public decimal? delayedSize { get; set; }
    //public string delayedPriceTime { get; set; }
    //public string processedTime { get; set; }

    public string symbol { get; set; }
    public string companyName { get; set; }
    public string primaryExchange { get; set; }
    public string calculationPrice { get; set; }
    public decimal? open { get; set; }
    public string openTime { get; set; }
    public decimal? close { get; set; }
    public string closeTime { get; set; }
    public decimal? high { get; set; }
    public decimal? low { get; set; }
    public double? latestPrice { get; set; }
    public string latestSource { get; set; }
    public string latestTime { get; set; }
    public long? latestUpdate { get; set; }
    public int? latestVolume { get; set; }
    public double? iexRealtimePrice { get; set; }
    public int? iexRealtimeSize { get; set; }
    public long? iexLastUpdated { get; set; }
    public object delayedPrice { get; set; }
    public object delayedPriceTime { get; set; }
    public double? extendedPrice { get; set; }
    public double? extendedChange { get; set; }
    public double? extendedChangePercent { get; set; }
    public long? extendedPriceTime { get; set; }
    public double? previousClose { get; set; }
    public object previousVolume { get; set; }
    public double? change { get; set; }
    public double? changePercent { get; set; }
    public int? volume { get; set; }
    public double? iexMarketPercent { get; set; }
    public int? iexVolume { get; set; }
    public int? avgTotalVolume { get; set; }
    public double? iexBidPrice { get; set; }
    public int? iexBidSize { get; set; }
    public double? iexAskPrice { get; set; }
    public int? iexAskSize { get; set; }
    public long? marketCap { get; set; }
    public double? peRatio { get; set; }
    public double? week52High { get; set; }
    public double? week52Low { get; set; }
    public double? ytdChange { get; set; }
    public long? lastTradeTime { get; set; }
    public bool? isUSMarketOpen { get; set; }



}
public class HistoricalDataResponse
{

    //public HistoricalDataResponse(string _date,
    //  double? _high,
    // double? _open,
    // double? _low,
    //  double? _close,
    // int? _volume,
    //  int? _unadjustedVolume,
    //  double? _change,
    //  double? _changePercent,
    //  double? _vwap,
    //  string _label,
    //  double? _changeOverTime)
    //{
    //    this.Date = _date;
    //    this.High = _high;
    //    this.Open = _open;
    //    this.Low = _low;
    //    this.Close = _close;
    //    this.UnadjustedVolume = _unadjustedVolume;
    //    this.Change = _change;
    //    this.ChangePercent = _changePercent;
    //    this.Vwap = _vwap;
    //    this.Label = _label;
    //    this.ChangeOverTime = _changeOverTime;

    //}



    //private string _date;
    //private double? _high;
    //private double? _open;
    //private double? _low;
    //private double? _close;
    //private int? _volume;
    //private int? _unadjustedVolume;
    //private double? _change;
    //private double? _changePercent;
    //private double? _vwap;
    //private string _label;
    //private double? _changeOverTime;

    //public string Date
    //{
    //    get { return _date; }
    //    set
    //    {
    //        _date = value;

    //    }
    //}
    //public double? Open
    //{
    //    get { return _open; }
    //    set
    //    {
    //        _open = value;

    //    }
    //}

    //public double? High
    //{
    //    get { return _high; }
    //    set
    //    {
    //        _high = value;

    //    }
    //}

    //public double? Low
    //{
    //    get { return _low; }
    //    set
    //    {
    //        _low = value;

    //    }
    //}
    //public double? Close
    //{
    //    get { return _close; }
    //    set
    //    {
    //        _close = value;

    //    }
    //}
    //public int? Volume
    //{
    //    get { return _volume; }
    //    set
    //    {
    //        _volume = value;

    //    }
    //}
    //public int? UnadjustedVolume
    //{
    //    get { return _unadjustedVolume; }
    //    set
    //    {
    //        _unadjustedVolume = value;

    //    }
    //}
    //public double? Change
    //{
    //    get { return _change; }
    //    set
    //    {
    //        _change = value;

    //    }
    //}
    //public double? ChangePercent
    //{
    //    get { return _changePercent; }
    //    set
    //    {
    //        _changePercent = value;

    //    }
    //}
    //public double? Vwap
    //{
    //    get { return _vwap; }
    //    set
    //    {
    //        _vwap = value;

    //    }
    //}
    //public string Label
    //{
    //    get { return _label; }
    //    set
    //    {
    //        _label = value;

    //    }
    //}
    //public double? ChangeOverTime
    //{
    //    get { return _changeOverTime; }
    //    set
    //    {
    //        _changeOverTime = value;

    //    }
    //}

    public string date { get; set; }
    public double uClose { get; set; }
    public double uOpen { get; set; }
    public double uHigh { get; set; }
    public double uLow { get; set; }
    public int uVolume { get; set; }
    public double close { get; set; }
    public double open { get; set; }
    public double high { get; set; }
    public double low { get; set; }
    public int volume { get; set; }
    public double change { get; set; }
    public double changePercent { get; set; }
    public string label { get; set; }
    public double changeOverTime { get; set; }


}
