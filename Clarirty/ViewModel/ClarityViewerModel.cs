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




namespace Clarity.Client.ViewModel
{
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;
    using Clarity.Client.Plugin;
    using Clarity.Client.View;
    using System.Windows.Media;
    using Linedata.Client.Workstation.LongviewAdapterClient;
    using Linedata.Framework.Foundation;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.IO;
    using Clarity.Client.Helpers;

    using log4net;
    using Linedata.Client.Workstation.SharedReferences;
    using ReactiveUI;
 
  

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]

    public class ClarityViewerModel : ReactiveObject, IDisposable
    {

        //web stuff
        private string urlForTitle;

        private ICommand processWebPageCommand;
        private ICommand backCommand;
        private ICommand forwardCommand;

        private IDictionary<string, string> titles;

        private Brush applicationBackgroundBrush;
        private Brush highlightBackgroundBrush;
        private Brush controlBackgroundBrush;
        private Brush controlForegroundBrush;
        private Brush borderBrush;
        private Brush controlBrush;

        private int back;
        private int forward;
        private bool isLoading;
        private bool? isBackOrForwardAction;

    //web stuff
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ClarityViewerModel));
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

        public ClarityParams Parameters
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

        private string _guid = string.Empty;
        public string Guid
        {
            get
            {
                return this._guid;
            }

            set
            {
                this._guid = value;
                this.RaisePropertyChanged("Guid");
            }
        }

        # endregion CoreParameters
        

        [ImportingConstructor]
        public ClarityViewerModel(ILongviewAdapterClient longviewAdapterClient, IAccountSummaryDataProvider accountSummaryDataProvider)
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

            

            //web stuff
           
            this.back = 0;
            this.forward = 0;
            this.isBackOrForwardAction = null;
            this.SetLoadingState(false);
            this.titles = new Dictionary<string, string>();
            this.RaisePropertyChanged("BrowserVisibility");
            //web stuff
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

        #region WidgetProperties
        public string UrlForTitle
        {
            get
            {
                return this.urlForTitle;
            }

            set
            {
                this.urlForTitle = value;
                this.RaisePropertyChanged("UrlForTitle");
            }
        }

        public Visibility BrowserVisibility
        {
            get
            {
                if (!WebBrowserMvvmHelper.VerifyUrlFormat(this.Parameters.URL))
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        public ICommand ProcessWebPageCommand
        {
            get
            {
                if (this.processWebPageCommand == null)
                {
                    this.processWebPageCommand = new RelayCommand(this.ProcessWebPage, this.CanProcessWebPage);
                }

                return this.processWebPageCommand;
            }
        }

        public ICommand BackCommand
        {
            get
            {
                if (this.backCommand == null)
                {
                    this.backCommand = new RelayCommand(this.GoBack, this.CanGoBack);
                }

                return this.backCommand;
            }
        }

        public ICommand ForwardCommand
        {
            get
            {
                if (this.forwardCommand == null)
                {
                    this.forwardCommand = new RelayCommand(this.GoForward, this.CanGoForward);
                }

                return this.forwardCommand;
            }
        }

        public int Back
        {
            get
            {
                return this.back;
            }

            set
            {
                this.back = value;
                this.RaisePropertyChanged("Back");
            }
        }

        public int Forward
        {
            get
            {
                return this.forward;
            }

            set
            {
                this.forward = value;
                this.RaisePropertyChanged("Forward");
            }
        }

        public Visibility LoadingVisibility
        {
            get
            {
                if (this.isLoading)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public Brush ApplicationBackgroundBrush
        {
            get
            {
                return this.applicationBackgroundBrush;
            }

            set
            {
                this.applicationBackgroundBrush = value;
                this.RaisePropertyChanged("ApplicationBackgroundBrush");
            }
        }

        public Brush HighlightBackgroundBrush
        {
            get
            {
                return this.highlightBackgroundBrush;
            }

            set
            {
                this.highlightBackgroundBrush = value;
                this.RaisePropertyChanged("HighlightBackgroundBrush");
            }
        }

        public Brush ControlBackgroundBrush
        {
            get
            {
                return this.controlBackgroundBrush;
            }

            set
            {
                this.controlBackgroundBrush = value;
                this.RaisePropertyChanged("ControlBackgroundBrush");
            }
        }

        public Brush ControlForegroundBrush
        {
            get
            {
                return this.controlForegroundBrush;
            }

            set
            {
                this.controlForegroundBrush = value;
                this.RaisePropertyChanged("ControlForegroundBrush");
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return this.borderBrush;
            }

            set
            {
                this.borderBrush = value;
                this.RaisePropertyChanged("BorderBrush");
            }
        }

        public Brush ControlBrush
        {
            get
            {
                return this.controlBrush;
            }

            set
            {
                this.controlBrush = value;
                this.RaisePropertyChanged("ControlBrush");
            }
        }

        public void SetLoadingState(bool loading)
        {
            this.isLoading = loading;
            this.RaisePropertyChanged("LoadingVisibility");
        }

        public void UpdateTitle()
        {
            if (null != this.titles)
            {
                if (!string.IsNullOrEmpty(this.urlForTitle))
                {
                    if ((this.isBackOrForwardAction.HasValue && this.isBackOrForwardAction.Value) || !this.isBackOrForwardAction.HasValue)
                    {
                        if (this.titles.ContainsKey(this.urlForTitle))
                        {
                            this.Parameters.Title = this.titles[this.urlForTitle];
                        }
                        else
                        {
                            this.titles.Add(this.urlForTitle, this.Parameters.Title);
                        }
                    }
                    else
                    {
                        if (this.titles.ContainsKey(this.urlForTitle))
                        {
                            this.titles.Remove(this.urlForTitle);
                        }

                        this.titles.Add(this.urlForTitle, this.Parameters.Title);
                    }
                }
            }

            this.isBackOrForwardAction = null;
        }

        public void ProcessWebPage()
        {
            this.isBackOrForwardAction = false;
            string Code = string.Empty;

            if (AccountId > 1)
            {
                Code = "&portfolio=" + Guid;


                if (this.Parameters.Section == "Funds-Summary")
                    
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/funds/{0}/summary?ordfield=SEC_SYMBOL&ordorder=ASC&posfield=SYMBOL&posorder=ASC&trdfield=SEC_SYMBOL&trdorder=ASC", AccountId);
                   
                else if
                    (this.Parameters.Section == "Funds- RiskSummary")
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/funds/{0}/risksummary/?ordfield=SEC_SYMBOL&ordorder=ASC&posfield=SYMBOL&posorder=ASC&trdfield=SEC_SYMBOL&trdorder=ASC", AccountId);

                else if
                   (this.Parameters.Section == "Funds-Holdings")
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/funds/{0}/position?ordfield=SEC_SYMBOL&ordorder=ASC&posfield=SYMBOL&posorder=ASC&trdfield=SEC_SYMBOL&trdorder=ASC", AccountId);

                else if
                  (this.Parameters.Section == "Funds-Orders")
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/funds/{0}/blotter?ordfield=SEC_SYMBOL&ordorder=ASC&posfield=SYMBOL&posorder=ASC&trdfield=SEC_SYMBOL&trdorder=ASC", AccountId);
                else if
                (this.Parameters.Section == "Funds-Trades")
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/funds/{0}/trades?ordfield=SEC_SYMBOL&ordorder=ASC&posfield=SYMBOL&posorder=ASC&trdfield=SEC_SYMBOL&trdorder=ASC", AccountId);

                else if
              (this.Parameters.Section == "Orders-Summary")
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/orders/summary");

                else if
             (this.Parameters.Section == "Orders-Orders")
                    this.UrlForTitle = string.Format("https://clarity-se.linedata.technology/orders/allOrders?allordorder=ASC&allordfield=SEC_SYMBOL");
                  
                else
                    this.UrlForTitle = "https://clarity-se.linedata.technology/orders/allOrders?allordorder=ASC&allordfield=SEC_SYMBOL";


                this.Parameters.URL = WebBrowserMvvmHelper.NormalizeUrl(this.UrlForTitle);

                this.RaisePropertyChanged("BrowserVisibility");
            }
            else
            {
                if (this.Parameters.Section == "Funds")
                    this.UrlForTitle = "https://clarity-se.linedata.technology/funds/10928/summary?posfield=SYMBOL&posorder=ASC";
                else if
                    (this.Parameters.Section == "Positions")
                    this.UrlForTitle = "https://clarity-se.linedata.technology/positions/3208/LONG/summary/";
                else
                    this.UrlForTitle = "https://clarity-se.linedata.technology/orders/allOrders?allordorder=ASC&allordfield=SEC_SYMBOL";


                this.Parameters.URL = WebBrowserMvvmHelper.NormalizeUrl(this.UrlForTitle);

                this.RaisePropertyChanged("BrowserVisibility");
            }
        }

        private bool CanProcessWebPage()
        {
            return WebBrowserMvvmHelper.VerifyUrlFormat(this.UrlForTitle);
        }

        private bool CanGoForward()
        {
            return !this.isLoading;
        }

        private void GoForward()
        {
            this.isBackOrForwardAction = true;
            this.Forward = this.forward + 1;
        }

        private bool CanGoBack()
        {
            return !this.isLoading;
        }

        private void GoBack()
        {
            this.isBackOrForwardAction = true;
            this.Back = this.back + 1;
        }
        #endregion WidgetProperties

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
            se_get_account_guid();
        }
        public void se_get_account_guid()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
              
                    Guid = DBService.se_get_account_guid(
                        AccountId,
                        out messages);
                });
        }

        #region properties


     

       

       

        #endregion

        #region HelperProcs

        #endregion HelperProcs

     
    }
}
