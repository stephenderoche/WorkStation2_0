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



namespace AccountTreeCashViewer.ViewModel
{

 
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;

    using Linedata.Client.Workstation.LongviewAdapterClient;
    using Linedata.Framework.Foundation;
    using Linedata.Client.Workstation.SharedReferences;
    using Linedata.Client.Widget.AccountSummaryDataProvider;
  
    using Linedata.Client.Workstation.LongviewAdapter.DataContracts;
    using Linedata.Client.Workstation.LongviewAdapterClient.EventArgs;
    using Linedata.Framework.WidgetFrame.UISupport;
    using Linedata.Shared.Workstation.Api.PortfolioManagement.DataContracts;
    using log4net;
    using System.IO;
    using AccountTreeCashViewer.Plugin;
    using AccountTreeCashViewer.View;
    using ReactiveUI;

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]

    public class AccountTreeViewModel :   ReactiveObject, IDisposable
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AccountTreeViewModel));
        private const string AccountIdColumnName = "account_id";
        private readonly ILongviewAdapterClient longviewAdapterClient;
        private readonly IReactivePublisher publisher;
        private BasicAccountInfo selectedAccount;
        private bool? canOpenAppraisalReport;
        private static List<BasicAccountInfo> accountList;
        private long? accountIdFromParams;
        private bool disposed = false;


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

        #region CoreParameters

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

       private long publishedAccount;
       public long PublishedAccount
        {
            get { return this.publishedAccount; }

            set
            {
                this.publishedAccount = value;
                this.RaisePropertyChanged("PublishedAccount");
            }
        }

       public BasicAccountInfo SelectedAccount
       {
           get { return this.selectedAccount; }

           set
           {
               // unsubscribe to previously selected account


               // display the new account summary
               this.selectedAccount = value;
               this.RaisePropertyChanged("SelectedAccount");



           }
       }

       public AccountTreeCashViewerParameters Parameters { get; set; }

        #endregion CoreParameters

       # region CoreSpecific
       
       [ImportingConstructor]
        public AccountTreeViewModel(ILongviewAdapterClient longviewAdapterClient,
             IReactivePublisher publisher)
        {
            this.publisher = publisher;
            CreateServiceClient();
            GetAcc();
            if (AccountId != -1)
                Get_Tree_info(Convert.ToInt32(AccountId));
            this.longviewAdapterClient = longviewAdapterClient;
            this.longviewAdapterClient.RowChanged += this.RowChanged;
            this.SubscribeToLongview();

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
            if (rowChangedEventArgs == null || rowChangedEventArgs.NewRow == null ||
                rowChangedEventArgs.ReportInstance == null)
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

            this.publisher.Publish(new WidgetMessages.AccountChanged(this.publisher.GetTabId(this.ParentWidget),
                this.ParentWidget.Group, accountId));
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
             
                        this.longviewAdapterClient.Dispose();
                    }

                  
                }

                this.disposed = true;
            }
        }
        private bool IsSubscribingToReportType(ReportType reportType)
        {
            return SubscribedReportTypes.Contains(reportType);
        }
        public IWidget ParentWidget { get; set; }
        public WidgetGroups Group { get; set; }
        private void SubscribeToLongview()
        {
            SubscriptionParameters subscriptionParameters = new SubscriptionParameters()
            {
                CellNames = new[] {AccountIdColumnName}
            };

            this.longviewAdapterClient.Subscribe(subscriptionParameters);
        }
        private long? GetAccountId(Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row,
            string columnName)
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
                    return (long) result;
                }
            }
            catch (KeyNotFoundException e)
            {
                //this.workstationLogger.LogException(e);
                Logger.DisplayError(e.Message, e);
            }

            return null;
        }
        private long? GetAccountId(IList<Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow> gridRows,
            string columnName)
        {
            if (gridRows == null)
            {
                return null;
            }

            Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row =
                gridRows.FirstOrDefault(x => x.RowType == RowType.Detail);

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
                    return (long) result;
                }
            }
            catch (KeyNotFoundException e)
            {
                //this.workstationLogger.LogException(e);
                Logger.DisplayError(e.Message, e);
            }

            return null;
        }
        private string GetCellValue(Linedata.Client.Workstation.LongviewAdapter.DataContracts.GridRow row,
            string columnName)
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
                   // this.workstationLogger.LogException(e);
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
                        this.longviewAdapterClient.CanOpenAppraisal((long) this.SelectedAccount.Id);
                }
            }

            return this.canOpenAppraisalReport ?? false;
        }
       #endregion  CoreSpecific


        #region WidgetSpecific
        #region WidgetSpecificParameters

        private DataSet m_AllAccounts;
        public DataSet AllAccounts
        {
            set { m_AllAccounts = value; }
            get { return m_AllAccounts; }
        }

        private int _selectedAccountId = -1;
        public int SelectedAccountId
        {
            set { _selectedAccountId = value; }
            get { return _selectedAccountId; }
        }

        private decimal _accountId;
        public decimal AccountId
        {
            set { _accountId = value; }
            get { return _accountId; }
        }

        private string _accountName;
        public string AccountName
        {
            set { _accountName = value; }
            get { return _accountName; }
        }

        private ISalesSharedContracts _dbservice;
        public ISalesSharedContracts DBService
        {
            set { _dbservice = value; }
            get { return _dbservice; }
        }

        List<AccountTreeMap> _accountTree = new List<AccountTreeMap>();
        public List<AccountTreeMap> AccountTree
        {
            get { return _accountTree; }
            set
            {
                _accountTree = value;

                this.RaisePropertyChanged("AccountTree");
            }
        }

        #endregion WidgetSpecificParameters

        public void GetAcc()
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                   
                   AllAccounts = DBService.GetAllAccounts2();

                   

                });
        }
        public List<AccountTreeMap> Get_Tree_info(int acc)
        {

            List<AccountTreeMap> AI = new List<AccountTreeMap>();
            DataSet ds = new DataSet();
            ThreadPool.QueueUserWorkItem(
                delegate(object eventArg)
                {
                    ApplicationMessageList messages = null;
                 

                    AccountTree = DBService.se_get_account_tree1(
                        Convert.ToInt32(acc),
                        out messages);

                });




            return AccountTree;


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

        #endregion WidgetSpecific
    }
}
