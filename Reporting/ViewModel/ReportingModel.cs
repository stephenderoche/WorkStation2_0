
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
using DevExpress.Mvvm.Native;
using DevExpress.Utils.Filtering;
using Reporting.Client.View;
using ReactiveUI;

namespace Reporting.Client.ViewModel
{
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;
    using DevExpress.Mvvm;
    using DevExpress.XtraReports.UI;
    using Reporting.Client.Plugin;
    using Reporting.Client.View;


    using Linedata.Client.Workstation.LongviewAdapterClient;
    using Linedata.Framework.Foundation;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using System.IO;
    using System.Windows.Media;
    using log4net;
    using Linedata.Client.Workstation.SharedReferences;
    using ReactiveUI;

  

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]

    public class ReportingModel :  ReactiveObject, IDisposable
    

    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReportingModel));
        private const string AccountIdColumnName = "account_id";
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


        # region CoreParameters

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
        public ReportingParameters Parameters
        {
            get;
            set;
        }
        # endregion CoreParameters
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

        [ImportingConstructor]
        public ReportingModel(ILongviewAdapterClient longviewAdapterClient,IAccountSummaryDataProvider accountSummaryDataProvider)
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
        #endregion CoreMethods


        #region Widget
      


        # region WidgetParameters

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

        private DataSet _allSecurities;
        public DataSet AllSecurities
        {
            set { _allSecurities = value; }
            get { return _allSecurities; }
        }

        DateTime _startDate;
        public DateTime StartDate
        {
            set { _startDate = value; }
            get { return _startDate; }
        }

        DateTime _endDate;
        public DateTime EndDate
        {
            set { _endDate = value; }
            get { return _endDate; }
        }

        private string _brokerMnemonic = "ALL";
        public string BrokerMnemonic
        {
            set { _brokerMnemonic = value; }
            get { return _brokerMnemonic; }
        }

        private int _brokerID = -1;
        public int BrokerID
        {
            set { _brokerID = value; }
            get { return _brokerID; }
        }

        private int _reasoncode = -1;
        public int Reasoncode
        {
            set { _reasoncode = value; }
            get { return _reasoncode; }
        }


        private string _assetCode = "";
        public string AssetCode
        {
            set { _assetCode = value; }
            get { return _assetCode; }
        }


        string _xml;
        public string XML
        {
            set { _xml = value; }
            get { return _xml; }
        }



        private int _security_id = -1;
        public int SecurityId
        {
            get { return _security_id; }
            set { _security_id = value; }
        }

        private decimal _accountId = -1;
        public decimal AccountId
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

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _report = string.Empty;
        public string Report
        {
            get { return _report; }
            set { _report = value; }
        }

        private bool _showAccount;
        public bool ShowAccount
        {
            get { return _showAccount; }
            set { _showAccount = value; }
        }

        private bool _showDesk;
        public bool ShowDesk
        {
            get { return _showDesk; }
            set { _showDesk = value; }
        }

        private bool _showSecurity;
        public bool ShowSecurity
        {
            get { return _showSecurity; }
            set { _showSecurity = value; }
        }

        private bool _showStartDate;
        public bool ShowStartDate
        {
            get { return _showStartDate; }
            set { _showStartDate = value; }
        }

        private bool _showEndDate;
        public bool ShowEndDate
        {
            get { return _showEndDate; }
            set { _showEndDate = value; }
        }

        private bool _showblockId;
        public bool ShowBlockId
        {
            get { return _showblockId; }
            set { _showblockId = value; }
        }

        private int _blockId;
        public int BlockId
        {
            get { return _blockId; }
            set { _blockId = value; }

        }

        # endregion WidgetParameters

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

        private DataSet _dsByBroker;
        public DataSet DsByBroker
        {
            get { return _dsByBroker; }
            set
            {
                _dsByBroker = value;
                this.RaisePropertyChanged("DsByBroker");
            }
        }
        public void ByBroker()
        {

            try
            {

                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {

                        ApplicationMessageList messages = null;
                       
                        //ByBroker


                        DsByBroker = DBService.se_get_commissions(
                            -1,
                            BrokerID,
                            Reasoncode,
                            Parameters.StartDate,
                            Parameters.EndDate,
                            1,
                            1,
                            15,
                            out messages);

                   
                       
                       

                    });

                
              
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }

        private DataSet _dsBrokerReason;
        public DataSet DsBrokerReason
        {
            get { return _dsBrokerReason; }
            set
            {
                _dsBrokerReason = value;
                this.RaisePropertyChanged("DsBrokerReason");
            }
        }
        public void ByBrokerReason()
        {

          

            try
            {

                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {


                        ApplicationMessageList messages = null;

                        //ByBroker


                        DsBrokerReason = DBService.se_get_commissions(
                            -1,
                            BrokerID,
                            Reasoncode,
                            Parameters.StartDate,
                            Parameters.EndDate,
                            1,
                            1,
                            7,
                            out messages);




                    });

                //ShowForm();
            }
            catch (Exception ex)
            {

                throw ex;
            }

          
        }


        private DataSet _dsAccountBrokerReason;
        public DataSet DsAccountBrokerReason
        {
            get { return _dsAccountBrokerReason; }
            set
            {
                _dsAccountBrokerReason = value;
                this.RaisePropertyChanged("DsAccountBrokerReason");
            }
        }
       public void ByAccountBrokerReason()
        {

            try
            {

                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {


                        ApplicationMessageList messages = null;

                        //ByBroker


                        DsAccountBrokerReason = DBService.se_get_commissions(
                            Convert.ToInt32(AccountId),
                            BrokerID,
                            Reasoncode,
                            Parameters.StartDate,
                            Parameters.EndDate,
                            -1,
                            1,
                            3,
                            out messages);




                    });

                //ShowForm();
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }


        private DataSet _dsAccountReason;
        public DataSet DsAccountReason
        {
            get { return _dsAccountReason; }
            set
            {
                _dsAccountReason = value;
                this.RaisePropertyChanged("DsAccountReason");
            }
        }
        public void ByAccountReason()
        {

            try
            {

                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {

                        ApplicationMessageList messages = null;

                        DsAccountReason = DBService.se_get_commissions(
                            Convert.ToInt32(AccountId),
                            BrokerID,
                            Reasoncode,
                            Parameters.StartDate,
                            Parameters.EndDate,
                            -1,
                            1,
                            19,
                            out messages);




                    });

                //ShowForm();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private DataSet _dsAccountSummary;
        public DataSet DsAccountSummary
        {
            get { return _dsAccountSummary; }
            set
            {
                _dsAccountSummary = value;
                this.RaisePropertyChanged("DsAccountSummary");
            }
        }
        public void ByAccountSummary()
        {

            try
            {

                ThreadPool.QueueUserWorkItem(
                    delegate(object eventArg)
                    {


                        ApplicationMessageList messages = null;
                        DataSet dsByAccountSummary = new DataSet();
                        //ByBroker


                        DsAccountSummary = DBService.rpx_account_compare(
                            Convert.ToInt32(AccountId),
                            out messages);


                       


                    });

                //ShowForm();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
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

        #endregion Widget

    }

}
