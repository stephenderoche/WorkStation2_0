// <copyright file="DetailServiceType.cs" company="Linedata Services" >// <copyright file="DetailServiceType.cs" company="Linedata Services" >
//  Copyright (c) Linedata Services. All rights reserved.
// </copyright>
// <summary>
//  Contains the DetailServiceType interface.
// </summary>

namespace SalesSharedContracts.Types
{
    using System;
    using System.Data;
    using System.ServiceModel;
    using Linedata.Server.Api;
    using Linedata.Shared.Api.DataContracts;
   using Linedata.Shared.Api.ServiceModel;
    using Linedata.Server.DbClasses;
    using Linedata.Server.DbInterfaces;
    using Linedata.Framework.Foundation;
    using SalesSharedContracts;
    using Linedata.Framework.WidgetFrame.MvvmFoundation;
  
    using System.Collections.Generic;
    // Rebalance References
    using Linedata.Server.Rebalance;
    using Linedata.Server.ReferenceData;
    using Linedata.Framework.Foundation.Engine;
    using Linedata.Server.DBL;
    using Linedata.Server.Interfaces;
    using Linedata.Server.DataProvider;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

   

    public interface IServiceTypes
    {
        DataSet se_get_hierarchy(out ApplicationMessageList messages);
        string se_get_default_hierarchy(int account_id, out ApplicationMessageList messages);
        DataSet se_fi_asset_allocation_pru(int account_id, string Hierarchy, out ApplicationMessageList messages);
        DataSet se_get_account_info(int account_id, out ApplicationMessageList messages);
        DataSet se_get_accounts(int account_id, out ApplicationMessageList messages);
        DataSet se_get_security_info(int security_id, out ApplicationMessageList messages);
        DataSet se_allocation_history(int account_id, int security_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages); //DataSet se_get_ticket_report(string account, string security, string issuer, string search, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages);
        DataSet se_get_drift_dashboard(int account_id, int just_drift, out ApplicationMessageList messages);
        DataSet se_get_top_securities_dasboard(int account_id, out ApplicationMessageList messages);
        DataSet se_benchmark_vs_holdings(int account_id, string Hierarchy, out ApplicationMessageList messages);
        //dashboard cash

        DataSet se_cash_balance(int account_id, int is_account_id, int include_negatives, int include_positives, int user_id, int currency_id, out ApplicationMessageList messages);
        DataSet se_get_maturities(int account_id, int @Date_offset, out ApplicationMessageList messages);

        //dashboard - compliance
        DataSet se_cmpl_get_top_security_breaches(int account_id, int userId, out ApplicationMessageList messages);
        DataSet se_cmpl_case_statistics(int account_id, int userId, out ApplicationMessageList messages);
        DataSet se_cmpl_get_top_account_breaches(int account_id, int userId, out ApplicationMessageList messages);
        DataSet se_cmpl_get_breaches_by_servitiy(int account_id, int userId, out ApplicationMessageList messages);
        DataSet se_cmpl_get_missing_data(int account_id, int userId, out ApplicationMessageList messages);

        //DashBoard - Positions


        DataSet se_get_positions_info(int account_id, int userId, out ApplicationMessageList messages);
        DataSet se_get_TaxLot_detail(int account_id, int security_id, out ApplicationMessageList messages);

        DataSet se_get_tax_lot_chart(int account_id, int security_id, out ApplicationMessageList messages);

        string se_get_cash_amount(int account_id, out ApplicationMessageList messages);
        string se_get_account_MV(int account_id, out ApplicationMessageList messages);
        DataSet se_get_tax_lot_position_data(int account_id, string symbol, out ApplicationMessageList messages);

        //robo dashboard

        DataSet se_get_drift_summary(int account_id, out ApplicationMessageList messages);
        void se_send_robo_proposed(int account_id, out ApplicationMessageList messages);
        void se_update_drift_summary(int account_id, out ApplicationMessageList messages);

        //treeviewer
       
     
        [OperationContract]
        DataSet se_get_account_tree(decimal account_id, out ApplicationMessageList messages);

        [OperationContract]
        List<AccountTreeMap> se_get_account_tree1(int account_id, out ApplicationMessageList messages);

        int se_get_default_model_id(int account_id, out ApplicationMessageList messages);
        DataSet se_aggregate_rebalance_sql(decimal account_id, decimal security_id, decimal @secondary_security_id, decimal suggested, decimal create_orders, out ApplicationMessageList messages);
        DataSet se_get_child_household_accounts(Int32 @account_id, out ApplicationMessageList messages);
        void se_aggregate_rebalance(Dictionary<long, short> accountIdAndType, long Account_group_id, long? default_model_id, string type, out ApplicationMessageList messages);

        //wash sales viewer
        [OperationContract]
        DataSet se_get_wash_sale_viewer(decimal account_id, out ApplicationMessageList messages);

        //account harvest tool
        [OperationContract]
        DataSet se_rank_tax_lots_account(decimal account_id, string relief_method_decription, decimal Offsetgain, string security_search, out ApplicationMessageList messages);

        [OperationContract]
        void se_create_orders_for_account_harvest(decimal account_id,
            decimal security_id,
            decimal quantity,
            string SideMnemonic,
            decimal Tax_quantity,
            decimal Tax_cost,
            DateTime SettmentDate,
            decimal @Tax_lot_id, out ApplicationMessageList messages);
        //commission
        [OperationContract]
        DataSet se_get_commissions(int account_id, int broker_id, int commission_reason_code, DateTime Start_date, DateTime end_date, int Budget_start_date, int commission_budget_period_code, int view_type, out ApplicationMessageList messages);

        [OperationContract]
        DataSet GetBrokerInfo();

        DataSet rpx_cmpl_breach_sum(decimal account_id, int isAccountId, int user_id, int display_passes, DateTime fromDate, DateTime toDate, out ApplicationMessageList messages);
        //List<Account> GetAccounts(ref ApplicationMessageList applicationMessageList);

        //nav
        DataSet get_asof_account_dashboard(int account_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages);
        [OperationContract]
        DataSet get_asof_dashboard_detail(int account_id, int intraday_code_id, DateTime snapshot_date, int nav_control_type, out ApplicationMessageList messages);

        DataSet GetListSecurity(int assetCode);

        DataSet get_list_country();

        DataSet se_get_ipo_data(int account_id, int country_code, decimal target, decimal MidPrice, decimal HighPrice, out ApplicationMessageList messages);

        DataSet se_create_orders_ipo(int security_id, int account_id, decimal @quantity, out ApplicationMessageList messages);

        Account_info se_get_general_accounts(int account_id, out ApplicationMessageList messages);

        DataSet se_get_top_issuers_dasboard(int account_id, out ApplicationMessageList messages);

        DataSet se_get_performance_summary(int account_id, out ApplicationMessageList messages);

        DataSet se_get_orders(int account_id, out ApplicationMessageList messages);


        void validate_proposed_order(decimal order_id, decimal account_id, decimal position_type_code, decimal security_id, decimal validation_status, int current_user, out ApplicationMessageList messages);

        void se_send_proposed(decimal account_id, decimal order_id, out ApplicationMessageList messages);

        void se_update_general_accounts(int account_id, string short_name, string model_name, string major_account, string account_holder,
           string account_holder_contact, string relief_method,
      string name

           , DateTime inception_date
           , decimal initial_funding_value,
                int user_id, DateTime close_date
               , DateTime composite_entry_date, DateTime composite_exit_date,
            DateTime performance_start_date, int taxable, int distribution, decimal distribution_amount, string distribution_frequency, DateTime last_distribution, DateTime next_distribution,
            int ips_on_file, DateTime last_ips_review_date, string next_ips_review_date,
           DateTime last_reg_9_review
             ,string reg_9_review_date
            , decimal management_fee
            , string custodian,
            DateTime next_rebalance, string Rebalance_frequency, int AutoRebalance, int SecurityDrift, int SectorDrift 
            ,int eligible
          , out ApplicationMessageList messages);

        DataSet se_get_top_accounts(int account_id, out ApplicationMessageList messages);

        DataSet se_get_assets_under_management(int account_id, out ApplicationMessageList messages);

        DataSet se_get_groups(out ApplicationMessageList messages);

        //List<Major_asset> se_get_major_asset_selected(out ApplicationMessageList messages);
        DataSet se_get_major_asset_selected(out ApplicationMessageList messages);

        void se_update_major_asset_selected(bool selected, int major_asset_code, string description, int user_id, out ApplicationMessageList messages);

        DataSet se_get_account_share_class(int account_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages);

        DataSet se_get_indicative_nav(int account_id, DateTime trade_date_start, out ApplicationMessageList messages);

        DataSet se_get_myNav(int MyNav_id, out ApplicationMessageList messages);

        DataSet se_get_contingency_dashboard(int account_id, out ApplicationMessageList messages);

        void se_update_contingency_dashboard(int account_share_class_audit_id, string type, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_message(DateTime trade_date_start, DateTime trade_date_end, string custodian,
            string Message_type, bool IsHeader, bool acknowleged, int processed, out ApplicationMessageList messages);

        [OperationContract]
        int se_get_message_count(out ApplicationMessageList messages);


        [OperationContract]
        DataSet se_get_message_count_grid(out ApplicationMessageList messages);

        DataSet se_get_message_groups(string custodian, string message_type, bool acknowleged, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_sub_message(int link_id, bool IsHeader, bool acknowleged, out ApplicationMessageList messages);

        void se_update_message(int message_id, bool acknowleged, string notes, int @assignment, int processed, out ApplicationMessageList messages);

        DataSet se_get_users(out ApplicationMessageList messages);

        int se_get_gauge_message_count(string guagetype, string custodian, out ApplicationMessageList messages);

        DataSet se_get_gauge_type(out ApplicationMessageList messages);

        DataSet se_get_corporate_actions(out ApplicationMessageList messages);

        DataSet se_get_corporate_actions_positions(int account_id, int security_id, out ApplicationMessageList messages);

        void process_corporate_actions(out ApplicationMessageList messages);

        DataSet get_cmpl_breach_mgt_report(
           int account_id,
           bool show_closed_breaches,
           bool show_sleeping_breaches,
           bool show_reviewed_breaches,
           bool show_nonreportable_breaches,
           bool hide_expired_rule_breaches,
           int? case_owner_id,
           bool show_noaction_breaches,
           bool show_pass_breaches,
           bool show_approved_breaches,
           DateTime start_date,
           DateTime end_date,
           bool show_to_review_breaches,
           bool show_to_approved_breaches,
           bool show_active_fails,
           bool show_passive_fails,
           bool show_open_breaches,
           bool show_suspend_breaches,
           bool show_resovled_breaches,
           bool show_warning_breaches,
           out ApplicationMessageList messages);

        void update_cmpl_case2(
           int cmpl_case_id,
           int cmpl_case_state_id,
           int active,
           int reviewed,
           int reportable,
           DateTime? awaken_time,
           int archive_ref_update,
           int? suppress_noaction_recurrences,
           int? approver,
           decimal? tolerance_lvl_pct,
           decimal? tolerance_value,
           out ApplicationMessageList messages);

        DataSet get_cmpl_breach_details(
           int cmpl_case_id,
           int display_currency_id,
           int cmpl_invocation_id,
           out ApplicationMessageList messages);

        DataSet get_cmpl_breach_actions(
            int cmpl_case_id,
            out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_parent_mutual_funds_orders_info(
            int account_id,
            bool include_orders,
            out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_parent_mutual_funds_info(
            int account_id,
            bool include_orders,
            out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_process_status(
            int account_id,
            DateTime date,
            out ApplicationMessageList messages);

        [OperationContract]
        Contingent se_get_indicative_nav1(int account_id,
            DateTime start_date, int share_class_type_code,
            out ApplicationMessageList messages);

        DataSet se_get_ticket_report(int account, int security, int issuer, string search,
            DateTime trade_date_start, DateTime trade_date_end, Int32 m_intAssetCode, out ApplicationMessageList messages);
         DataSet se_get_positions_report(string account, string security, string issuer, string search, decimal? user_id, out ApplicationMessageList messages);
        Security GetSecurityBySymbol(string symbol, out ApplicationMessageList messages);
        DataSet GetIssuerInfo();
        DataSet GetIssuername(int security_id, out ApplicationMessageList messages);

        DataSet GetListAllDesks();

        [OperationContract]
        DataSet se_get_tradervspm_orders(int desk_id, string account, string security, string issuer,
        string search, DateTime trade_date_start, DateTime trade_date_end, bool active, int major_asset_code, bool crossable, out ApplicationMessageList messages);


        DataSet se_get_journaling(int Block_id, out ApplicationMessageList messages);
        void se_update_journaling(int Block_id, string journal_entry, out ApplicationMessageList messages);


        void se_update_committment_price(int Block_id, decimal price, out ApplicationMessageList messages);

 
        DataSet se_get_generic_param(string report, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_generic_view(int account_id, int desk_id, int security_id, DateTime startdate, DateTime enddate, int block_id,string report, out ApplicationMessageList messages);

        DataSet se_get_account_children(decimal account_id, out ApplicationMessageList messages);
        DataSet se_get_account_children_with_percents(decimal account_id, decimal cash_amount, out ApplicationMessageList messages);
        void se_update_cash_position(decimal account_id, decimal cash_amount, out ApplicationMessageList messages);

        [OperationContract]
        void delete_proposed(int order_id, out ApplicationMessageList messages);

        [OperationContract]
        void delete_order(int order_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet get_nav_control_types();

        [OperationContract]
        DataSet se_get_nav_multi_approval_view(decimal account_id, DateTime nav_date, int control_type_code, int loadhist_definition_id, bool focusfail, bool focusData, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_loadhist();

        [OperationContract]
        string se_get_nav_comments(int comment_id, int comment_type, out ApplicationMessageList messages);
        [OperationContract]
        void se_update_nav_comments(int comment_id, int comment_type, int nav_res_rule_result_id, string comment, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_nav_ruleset_comments(int first_or_dq_rev_comm_text_id, int comment_type, int nav_res_ruleset_review_id, bool nav_status_ok, string comment, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_nav_account_tree(int account_id, int intraday_code_id, DateTime snapshot_date_in, int control_type, out ApplicationMessageList messages);

        [OperationContract]
        DataSet nq_get_nav_linked_reports(int nav_res_rule_result_id, int nav_report_order, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_currency_mvmt_report_sum(int account_id, int display_currency_id, out ApplicationMessageList messages);

        DataSet se_get_currency_mvmt_detail(DateTime from_date, DateTime end_date, int account_id, int currency_code, out ApplicationMessageList messages);

        [OperationContract]
        void se_create_orders(int principal_currency_id, int counter_currency_id, DateTime settlementDate, int account_id, decimal quantity, out ApplicationMessageList messages);

        [OperationContract]
        DataSet GetAllAccounts2();

        [OperationContract]
        void ValidateAccountForUser(string accountShortName, out int defaultAccountId, out string defaultShortName);

        DataSet GetSecurityInfo(string symbol);

        [OperationContract]
        DataSet se_get_bb_data(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_bb_data(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_modelcount(out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_accounts_on_model(int model_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_securities_on_model(int model_id, int account_id, int filterType, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_top_of_model(int model_id, int topx, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_yield_curve(string name, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_parallel_shift(string name, decimal pct_change, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_bond_change(int account_id, decimal pct_change, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_analytic_ditribution(int account_id, int analytic_value, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_contibution_ditribution(int account_id, int analytic_value, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_cash_flows(int account_id, int security_id, DateTime date_start, DateTime date_end, out ApplicationMessageList messages);


        [OperationContract]
        DataSet se_get_desk_trades(int desk_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_execution_info(int block_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_execution_info_by_broker(int block_id, int broker_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet rpx_account_compare(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_price_journal(int model_id, int security_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_price_journal(int model_id, int security_id, string @entry, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_price_history(int model_id, int security_id, decimal purchase_price, decimal target_price, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_dashoard_summary(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        List<AccountHierarchy> se_get_account_hierarchy(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_analytic_benchmark(int account_id, int analytic_type, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_contingent_status(DateTime asofDate, out ApplicationMessageList messages);

        [OperationContract]
        string se_get_account_guid(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_download_messages_history(DateTime date, int current, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_download_messages_history_detail(DateTime message_time, string mnemonic, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_security_data(int security_id, int major_asset_code, Boolean IsZeroOrNull, decimal percentChange, Boolean justHoldings, int Stale, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_linked_cash(int desk_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_rebal_sessions( int session_id,out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_rebal_sessions_account(int session_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_rebal_sessions_security(int session_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_rebal_pos_exclusion(int session_id, out ApplicationMessageList messages);


        [OperationContract]
        DataSet se_get_account_replacement(int account_id, out ApplicationMessageList messages);


        [OperationContract]
        void se_delete_replacement(int replacement_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_replacement(int replacement_id, int buy_list_type_id, int buy_list_id, int replacement_list_type_id, int replacement_list_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_replacement_hierarchy();

        [OperationContract]
        DataSet se_get_replacement_country();

        [OperationContract]
        DataSet se_get_adv_rebal_op(int account_id, int session_id, out ApplicationMessageList messages);

        Rebal_info rebal_info(int account_id, int session_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_send_orders_faster(int account_id, int security_id, out ApplicationMessageList messages);

        [OperationContract]
        void runCompliance(decimal account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_rebal_detail_info(int @security_id, int session_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_list_account(string @short_name, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_restricted_security(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_run_rules(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_restriction_report(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_add_first_override(int account_id, int security_id, string reason, int order_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_add_internal_mf_cash(string short_name, decimal cash_amount, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_funds_cash_by_fof(int account_id, int @include_orders, out ApplicationMessageList messages);

    }
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        UseSynchronizationContext = false,
        Name = "SalesSharedContracts")]
    public class ServiceTypes : ServiceTypeBase, ISalesSharedContracts, IServiceTypes
    {
        static private string dsn = "";
     
        string results;
        Security ds;
        static private decimal? user_id1 = -1;

        public ServiceTypes()
        {
            dsn = ServerSettings.ApplicationDataSource.Name;


        }

        public DataSet GetBrokerInfo()
        {
            ApplicationMessageList messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        int numberOfParams = 0;
                        int actualParams = 0;

                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                       // IDbDataParameter param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("get_list_brokers", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        {
                            command.Parameters.Add(paramToAdd);
                        }
                        Debug.Assert(actualParams == numberOfParams);

                        IDataReader reader = command.ExecuteReader();
                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

            return ds;
        }




        public DataSet se_get_commissions(int account_id, int broker_id, int commission_reason_code, DateTime Start_date, DateTime end_date, int Budget_start_date, int commission_budget_period_code, int view_type, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[8];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@broker_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = broker_id;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@commission_reason_code";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = commission_reason_code;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@Start_date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = Start_date;
                        storedProcParams[3] = param;


                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@end_date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = end_date;
                        storedProcParams[4] = param;

                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@Budget_start_date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = Budget_start_date;
                        storedProcParams[5] = param;

                        //Param 7
                        param = command.CreateParameter();
                        param.ParameterName = "@commission_budget_period_code";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = commission_budget_period_code;
                        storedProcParams[6] = param;

                        //Param 8
                        param = command.CreateParameter();
                        param.ParameterName = "@view_type";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = view_type;
                        storedProcParams[7] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_commissions", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

     

        private int getUserId()
        {
            if (ServerSettings.IoiDataSource.CurrentUser != null)
            {
                return (int)ServerSettings.ApplicationDataSource.CurrentUser;

            }
            return 0;
        }

        public DataSet se_get_hierarchy( out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                        //IDbDataParameter param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_hierarchy", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_top_securities_dasboard(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_top_securities_dasboard", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_drift_dashboard(int account_id, int just_drift, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@JustDrift";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = just_drift;
                        storedProcParams[1] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_drift_dashboard", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_fi_asset_allocation_pru(int account_id, string Hierarchy, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@hierarchy_name";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = Hierarchy;
                        storedProcParams[1] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_fi_asset_allocation_pru_by_accout_id", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public string se_get_default_hierarchy(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();



            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;





                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_default_hierarchy_by_account_id", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToString(values[0]);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


            return results;
        }

        public decimal? GetAccountID(string AccountName, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            Decimal? results = new Decimal?();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@AccountName";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = AccountName;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_id", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToInt64(values[0]);
                        }

                    }
                }
            }


            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


            return results;
        }

        public DataSet se_get_account_info(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_info_dashboard", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_accounts(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_accounts", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_security_info(int security_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[0] = param;


                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_security_info", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_allocation_history(int account_id, int security_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[1] = param;




                        param = command.CreateParameter();
                        param.ParameterName = "@fromDate";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = trade_date_start;
                        storedProcParams[2] = param;


                        param = command.CreateParameter();
                        param.ParameterName = "@toDate";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = trade_date_end;
                        storedProcParams[3] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_allocation_history", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_benchmark_vs_holdings(int account_id, string Hierarchy, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@hierarchy_name";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = Hierarchy;
                        storedProcParams[1] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@userid";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[2] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_benchmark_vs_holdings", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_cash_balance(int account_id, int is_account_id, int include_negatives, int include_positives, int user_id, int currency_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@is_account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = is_account_id;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@include_negatives";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = include_negatives;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@include_positives";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = include_positives;
                        storedProcParams[3] = param;

                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[4] = param;

                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@currency_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = currency_id;
                        storedProcParams[5] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_cash_balance", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_maturities(int account_id, int Date_offset, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@Date_offset";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = Date_offset;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_maturities", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }
        //dashboard - Compliance
        public DataSet se_cmpl_get_top_security_breaches(int account_id, int Date_offset, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@userId";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_cmpl_get_top_security_breaches", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_cmpl_case_statistics(int account_id, int userId, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_cmpl_case_statistics", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_cmpl_get_missing_data(int account_id, int userId, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_cmpl_get_missing_data", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_cmpl_get_top_account_breaches(int account_id, int userId, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_cmpl_get_top_account_breaches", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_cmpl_get_breaches_by_servitiy(int account_id, int userId, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_cmpl_get_breaches_by_servitiy", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        //dashboard - positions
        public DataSet se_get_positions_info(int account_id, int userId, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_positions_info", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_TaxLot_detail(int account_id, int security_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_TaxLot_detail", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_tax_lot_chart(int account_id, int security_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_tax_lot_chart", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public string se_get_cash_amount(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();



            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;





                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_cash_amount", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToString(values[0]);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


            return results;
        }

        public string se_get_account_MV(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_MV", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToString(values[0]);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


            return results;
        }

        public DataSet se_get_tax_lot_position_data(int account_id, string symbol, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@symbol";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = symbol;
                        storedProcParams[1] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_tax_lot_position_data", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        //robo dashboard



        public DataSet se_get_drift_summary(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_drift_summary", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public void se_send_robo_proposed(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_send_robo_proposed", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }


        public void se_update_drift_summary(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_update_drift_summary", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public DataSet se_get_account_tree(decimal account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();

            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        //int numberOfParams = 0;
                        //int actualParams = 0;

                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;



                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_tree", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);


                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

            return ds;
        }

        public List<AccountTreeMap> se_get_account_tree1(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            List<AccountTreeMap> AccountTreeMapList = new List<AccountTreeMap>();
            DataSet ds = null;

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_tree1", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                        DataTable dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            //foreach (DataRow row in dataTable.Rows)
                            //{
                            //    AI[x].Parent_account_id = Convert.ToInt32(row["parent_account_id"].ToString());
                            //    AI[x].Parent_Account_Name = row["Parent_Account_Name"].ToString();
                            //    AI[x].Child_type = row["child_type"].ToString();
                            //    AI[x].Child_account_id = Convert.ToInt32(row["child_account_id"].ToString());
                            //    AI[x].Child_Account_Name = row["Child_Account_Name"].ToString();
                            //    x = x + 1;
                            //}

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AccountTreeMap ATM = new AccountTreeMap();
                                ATM.Parent_account_id = Convert.ToInt32(dt.Rows[i]["Parent_account_id"].ToString());
                                ATM.Parent_Account_Name = dt.Rows[i]["Parent_Account_Name"].ToString();
                                ATM.Child_type = dt.Rows[i]["child_type"].ToString();
                                ATM.Child_account_id = Convert.ToInt32(dt.Rows[i]["Child_account_id"].ToString());
                                ATM.Child_Account_Name = dt.Rows[i]["Child_Account_Name"].ToString();

                                AccountTreeMapList.Add(ATM);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return AccountTreeMapList;
        }

        public int se_get_default_model_id(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            int results = new int();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_default_model_id", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToInt32(values[0]);
                        }

                    }
                }
            }


            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


            return results;
        }

        public DataSet se_aggregate_rebalance_sql(decimal account_id, decimal security_id, decimal @secondary_security_id, decimal suggested, decimal create_orders, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = security_id;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@secondary_security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = secondary_security_id;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@suggested";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = suggested;
                        storedProcParams[3] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@create_orders";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = create_orders;
                        storedProcParams[4] = param;




                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_aggregate_rebalance", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);


                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_child_household_accounts(Int32 @account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();


            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        //int numberOfParams = 0;
                        //int actualParams = 0;

                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;



                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_child_household_accounts", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        {
                            command.Parameters.Add(paramToAdd);
                        }
                        //Debug.Assert(actualParams == numberOfParams);

                        IDataReader reader = command.ExecuteReader();
                        //accountIdsAndType = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);



                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);






                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

            return ds;
        }

        public void se_aggregate_rebalance(Dictionary<long, short> accountIdAndType, long Account_group_id, long? default_model_id, string type, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();


            RebalancingObjective rebalanceType;
            if (type == "Advanced")
                rebalanceType = RebalancingObjective.Advanced;
            else if (type == "Aggregate")
                rebalanceType = RebalancingObjective.Aggregate;
            else if (type == "Cash Target")
                rebalanceType = RebalancingObjective.CashTarget;
            else if (type == "Custom")
                rebalanceType = RebalancingObjective.Custom;
            else if (type == "Regular")
                rebalanceType = RebalancingObjective.Regular;
            else if (type == "Special")
                rebalanceType = RebalancingObjective.Special;
            else
                rebalanceType = RebalancingObjective.XRefSecurity;


            EngineCallData engineCallData = new EngineCallData(new ClientCallData(new UserSettings(), messages), ServerSettings.ApplicationDataSource);
            RebalanceParametersManaged rebalParams = new RebalanceParametersManaged();
            IDictionary<long, DblAggregatePortfolio> aggPortfolioAccounts = null;
            List<Tuple<IRebalancerParams, DblAggregatePortfolio>> rebalancerParams = new List<Tuple<IRebalancerParams, DblAggregatePortfolio>>();

            try
            {
                DblAggregatePortfolio.ClassFactory.Parameters portfolioParams = new DblAggregatePortfolio.ClassFactory.Parameters();
                portfolioParams.AccountIdList = new List<long>();

                portfolioParams.DataProvider = new CurrentDataProvider();
                portfolioParams.AccountHierarchyIdList.Add(-1);

                foreach (KeyValuePair<long, short> accountInfo in accountIdAndType)
                {
                    portfolioParams.AccountIdList.Add(accountInfo.Key);
                    portfolioParams.AccountTypeList.Add(accountInfo.Value);

                    rebalParams.SetAccountParams(accountInfo.Key, new AccountParameters());
                }

                portfolioParams.ModelSourcing = DblAggregatePortfolio.FactoryModelSourcing.UseAccountDefault;


                portfolioParams.DoPersistSelfModels = false;

                aggPortfolioAccounts = DblAggregatePortfolio.ClassFactory.Instance.Create(engineCallData, portfolioParams);

                rebalParams.ObjectiveRebal = rebalanceType;


                if (aggPortfolioAccounts.Count <= 0)
                {
                    engineCallData.ClientCallData.AppMessageList.Add(new ApplicationMessage("Error loading rebalancer accounts", ApplicationMessageType.WarningNoPopup));
                    return;
                }

                List<AggregateModel> rebalModelsAndParams =
                    new List<AggregateModel>();

                foreach (DblAggregatePortfolio portfolio in aggPortfolioAccounts.Values)
                {
                    rebalancerParams.Add(new Tuple<IRebalancerParams, DblAggregatePortfolio>(rebalParams, portfolio));
                    rebalModelsAndParams.Add(portfolio.AggregateModel);
                }

                CentralRebalancer test = new CentralRebalancer();



                // rebal.RebalanceToModel(engineCallData, rebalModelsAndParams, true);
                List<long> accountIds = new List<long>(portfolioParams.AccountIdList);
                int orderCount;

                rebalParams.SessionId = 2;

                //KeyValuePair<long, short> leadAccount = accountIdAndType.F
                test.InitializeAndRebalance(engineCallData, rebalParams, Account_group_id, 3, accountIds, default_model_id, null, null, true, out orderCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("RunRebalanceToDefaultModel: {0} \n {1}", e.ToString(), e.StackTrace.ToString());
            }
        }

        public DataSet se_get_wash_sale_viewer(decimal account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_wash_sale_viewer", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_rank_tax_lots_account(decimal account_id, string relief_method_decription, decimal Offsetgain, string security_search, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@relief_method_decription";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = relief_method_decription;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@Offsetgain";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = Offsetgain;
                        storedProcParams[2] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@security_search";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = security_search;
                        storedProcParams[3] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_rank_tax_lots_account", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public void se_create_orders_for_account_harvest(decimal account_id,
          decimal security_id,
          decimal quantity,
          string SideMnemonic,
          decimal Tax_quantity,
          decimal Tax_cost,
          DateTime SettmentDate,
          decimal @Tax_lot_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();


            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[8];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = security_id;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@quantity";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = quantity;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@SideMnemonic";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = SideMnemonic;
                        storedProcParams[3] = param;

                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@Tax_quantity";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = Tax_quantity;
                        storedProcParams[4] = param;

                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@Tax_cost";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = Tax_cost;
                        storedProcParams[5] = param;

                        //Param 7
                        param = command.CreateParameter();
                        param.ParameterName = "@SettmentDate";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = SettmentDate;
                        storedProcParams[6] = param;

                        //Param 8
                        param = command.CreateParameter();
                        param.ParameterName = "@Tax_lot_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = @Tax_lot_id;
                        storedProcParams[7] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_create_orders_for_account_harvest", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();


                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public DataSet rpx_cmpl_breach_sum(decimal account_id, int isAccountId, int user_id, int display_passes, DateTime fromDate, DateTime toDate, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@acctID ";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@isAccountId";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = isAccountId;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@userId";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@display_passes";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = 1;
                        storedProcParams[3] = param;

                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@fromDate";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = fromDate;
                        storedProcParams[4] = param;


                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@toDate";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = toDate;
                        storedProcParams[5] = param;

                       



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("rpx_cmpl_breach_sum", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }


        //public List<Account> GetAccounts(ref ApplicationMessageList applicationMessageList)
        //{
        //    List<Account> accounts = Linedata.sales.Widget.Server.Factories.AccountFactory.CreateAccount();

             
        //    return accounts;
        //}


        public DataSet get_asof_account_dashboard(int account_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                       




                        param = command.CreateParameter();
                        param.ParameterName = "@start_date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = trade_date_start;
                        storedProcParams[1] = param;


                        param = command.CreateParameter();
                        param.ParameterName = "@end_date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = trade_date_end;
                        storedProcParams[2] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_asof_account_dashboard", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

       public  DataSet get_asof_dashboard_detail(int account_id, int intraday_code_id, DateTime snapshot_date, int nav_control_type, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        param = command.CreateParameter();
                        param.ParameterName = "@intraday_code_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = intraday_code_id;
                        storedProcParams[1] = param;



                        param = command.CreateParameter();
                        param.ParameterName = "@snapshot_date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = snapshot_date;
                        storedProcParams[2] = param;


                        param = command.CreateParameter();
                        param.ParameterName = "@nav_control_type";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = nav_control_type;
                        storedProcParams[3] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_asof_dashboard_detail", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }


       public DataSet GetListSecurity(int assetCode)
       {
           ApplicationMessageList messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       int numberOfParams = 1;
                       int actualParams = 0;

                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@major_asset_code";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = assetCode;
                       storedProcParams[actualParams++] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("get_list_security", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       {
                           command.Parameters.Add(paramToAdd);
                       }
                       Debug.Assert(actualParams == numberOfParams);

                       IDataReader reader = command.ExecuteReader();
                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }
           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

           return  ds;
       }

       public DataSet get_list_country()
       {
           ApplicationMessageList messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       int numberOfParams = 0;
                       int actualParams = 0;

                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                       // IDbDataParameter param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("get_list_country", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       {
                           command.Parameters.Add(paramToAdd);
                       }
                       Debug.Assert(actualParams == numberOfParams);

                       IDataReader reader = command.ExecuteReader();
                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                   }
               }
           }
           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

           return ds;
       }

       public DataSet se_get_ipo_data(int account_id, int country_code, decimal target, decimal MidPrice, decimal HighPrice, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@country_code";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = country_code;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@target";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = target;
                       storedProcParams[2] = param;

                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "@MidPrice";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = MidPrice;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@HighPrice";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = HighPrice;
                       storedProcParams[4] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_ipo_data", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }


       public DataSet  se_create_orders_ipo(int security_id, int account_id, decimal @quantity, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@security_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = security_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@quantity";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = quantity;
                       storedProcParams[2] = param;

                    

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_create_orders_ipo", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public Account_info se_get_general_accounts(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           Account_info AI = new Account_info();
           DataSet ds = null;

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                      



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_general_accounts", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                       DataTable dataTable = ds.Tables[0];
                       if (dataTable.Rows.Count > 0)
                       {
                           foreach (DataRow row in dataTable.Rows)
                           {
                               AI.Account_name = row["Account Name"].ToString();
                               AI.Account_type = row["Account Type"].ToString();
                               AI.Model_name = row["Model Name"].ToString();
                               AI.Account_holder = row["Account Holder"].ToString();
                               AI.Account_holder_contact = row["Account Holder Contact"].ToString();
                               AI.Manager = row["Manager"].ToString();
                               AI.ReliefMethod = row["Relief Method"].ToString();
                               AI.InceptionDate = Convert.ToDateTime(row["Inception Date"]);
                               AI.Funding_value = Convert.ToInt64(row["Initial Funding Value"]);
                               AI.Eligible = Convert.ToBoolean(row["Eligible"]);
                               AI.PerformanceStartDate = Convert.ToDateTime(row["performance_start_date"]);
                               AI.CloseDate = Convert.ToDateTime(row["close_date"]);
                               AI.CompositeEntryDate = Convert.ToDateTime(row["composite_entry_date"]);
                               AI.CompositeExitDate = Convert.ToDateTime(row["composite_exit_date"]);
                               AI.Taxable = Convert.ToBoolean(row["taxable"]);
                               AI.Distribution = Convert.ToBoolean(row["distribution"]);
                               AI.DistributionAmount = Convert.ToInt64(row["distribution_amount"]);
                               AI.DistributionFrequency = Convert.ToString(row["distribution_frequency"]);
                               AI.LastDistribution = Convert.ToDateTime(row["last_distribution"]);
                               AI.NextDistribution = Convert.ToDateTime(row["next_distribution"]);
                               AI.IPS = Convert.ToBoolean(row["ips_on_file"]);
                               AI.LastipsReview = Convert.ToDateTime(row["last_ips_review_date"]);
                               AI.NextipsReview =  row["next_ips_review_date"].ToString();
                               AI.LastRegReview = Convert.ToDateTime(row["last_reg_9_review"]);
                               AI.NextRegReview = row["reg_9_review_date"].ToString();
                               AI.ManagmentFee = Convert.ToDecimal(row["management_fee"]);
                               AI.Custodian = row["custodian"].ToString();
                               AI.AutoRebalance = Convert.ToBoolean(row["AutoRebalance"]);
                               AI.SecurityRebalance = Convert.ToBoolean(row["SecurityDrift"]);
                               AI.SectorRebalance = Convert.ToBoolean(row["SectorDrift"]);
                               AI.RebalanceFrequency = row["Rebalance_frequency"].ToString();
                               AI.LastRebalance = Convert.ToDateTime(row["last_rebalance"]);
                               AI.NextRebalance= Convert.ToDateTime(row["next_rebalance"]);
                           }
                       }
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return AI;
       }

       public DataSet se_get_top_issuers_dasboard(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_top_issuers_dasboard", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_performance_summary(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_performance_summary", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_orders(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_orders", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public void validate_proposed_order(decimal order_id, decimal account_id, decimal position_type_code, decimal security_id, decimal validation_status, int current_user, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@order_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = order_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = account_id;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@position_type_code";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = position_type_code;
                       storedProcParams[2] = param;

                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "@security_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = security_id;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@validation_status";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = validation_status;
                       storedProcParams[4] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@current_user";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = getUserId();
                       storedProcParams[5] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("validate_proposed_order", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();


                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

       }

       public  void se_send_proposed( decimal account_id, decimal order_id,  out ApplicationMessageList messages)
           {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                  

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@order_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = order_id;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@current_user";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = getUserId();
                       storedProcParams[2] = param;




                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_send_proposed", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();


                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

       }

       public void se_update_general_accounts(int account_id, string short_name, string model_name, string major_account, string account_holder,
             string account_holder_contact, string relief_method,
        string name

             , DateTime inception_date
             , decimal initial_funding_value,
                  int user_id, DateTime close_date
                 , DateTime composite_entry_date, DateTime composite_exit_date,
              DateTime performance_start_date, int taxable, int distribution, decimal distribution_amount, string distribution_frequency, DateTime last_distribution, DateTime next_distribution,
              int ips_on_file, DateTime last_ips_review_date, string next_ips_review_date,
             DateTime last_reg_9_review
            ,string reg_9_review_date
           
           , decimal management_fee
           ,string custodian,
           DateTime next_rebalance, string Rebalance_frequency, int AutoRebalance, int SecurityDrift, int SectorDrift
           , int eligible
            , out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[34];
                       IDbDataParameter param;



                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@short_name";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = short_name;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@model_name";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = model_name;
                       storedProcParams[2] = param;

                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "major_account";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = major_account;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@account_holder";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_holder;
                       storedProcParams[4] = param;

                       //Param 6
                       param = command.CreateParameter();
                       param.ParameterName = "@account_holder_contact";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_holder_contact;
                       storedProcParams[5] = param;


                       //Param 7
                       param = command.CreateParameter();
                       param.ParameterName = "@relief_method";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = relief_method;
                       storedProcParams[6] = param;

                       //Param 8
                       param = command.CreateParameter();
                       param.ParameterName = "@name";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = name;
                       storedProcParams[7] = param;

                       //Param 9
                       param = command.CreateParameter();
                       param.ParameterName = "@inception_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = inception_date;
                       storedProcParams[8] = param;

                       //Param 10
                       param = command.CreateParameter();
                       param.ParameterName = "@initial_funding_value";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = initial_funding_value;
                       storedProcParams[9] = param;

                       //Param 11
                       param = command.CreateParameter();
                       param.ParameterName = "@user_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = getUserId();
                       storedProcParams[10] = param;

                       //Param 12
                       param = command.CreateParameter();
                       param.ParameterName = "@close_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = close_date;
                       storedProcParams[11] = param;

                       //Param 13
                       param = command.CreateParameter();
                       param.ParameterName = "@composite_entry_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = composite_entry_date;
                       storedProcParams[12] = param;

                       //Param 14
                       param = command.CreateParameter();
                       param.ParameterName = "@composite_exit_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = composite_exit_date;
                       storedProcParams[13] = param;

                       //Param 15
                       param = command.CreateParameter();
                       param.ParameterName = "@performance_start_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = performance_start_date;
                       storedProcParams[14] = param;

                       //Param 16
                       param = command.CreateParameter();
                       param.ParameterName = "@taxable";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = taxable;
                       storedProcParams[15] = param;

                       //Param 17
                       param = command.CreateParameter();
                       param.ParameterName = "@distribution";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = distribution;
                       storedProcParams[16] = param;

                       //Param 18
                       param = command.CreateParameter();
                       param.ParameterName = "distribution_amount";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = distribution_amount;
                       storedProcParams[17] = param;

                       //Param 19
                       param = command.CreateParameter();
                       param.ParameterName = "@distribution_frequency";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = distribution_frequency;
                       storedProcParams[18] = param;

                       //Param 20
                       param = command.CreateParameter();
                       param.ParameterName = "@last_distribution";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = last_distribution;
                       storedProcParams[19] = param;

                       //Param 21
                       param = command.CreateParameter();
                       param.ParameterName = "@next_distribution";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = next_distribution;
                       storedProcParams[20] = param;

                       //Param 22
                       param = command.CreateParameter();
                       param.ParameterName = "@ips_on_file";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = ips_on_file;
                       storedProcParams[21] = param;

                       //Param 23
                       param = command.CreateParameter();
                       param.ParameterName = "@last_ips_review_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = last_ips_review_date;
                       storedProcParams[22] = param;

                       //Param 24
                       param = command.CreateParameter();
                       param.ParameterName = "@next_ips_review_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = next_ips_review_date;
                       storedProcParams[23] = param;

                       //Param 25
                       param = command.CreateParameter();
                       param.ParameterName = "@last_reg_9_review";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = last_reg_9_review;
                       storedProcParams[24] = param;

                       //Param 26
                       param = command.CreateParameter();
                       param.ParameterName = "@reg_9_review_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = reg_9_review_date;
                       storedProcParams[25] = param;

                       //Param 27
                       param = command.CreateParameter();
                       param.ParameterName = "@management_fee";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = management_fee;
                       storedProcParams[26] = param;

                       //Param 28
                       param = command.CreateParameter();
                       param.ParameterName = "@custodian";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = custodian;
                       storedProcParams[27] = param;

                       //Param 29
                       param = command.CreateParameter();
                       param.ParameterName = "@next_rebalance";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = next_rebalance;
                       storedProcParams[28] = param;

                       //Param 30
                       param = command.CreateParameter();
                       param.ParameterName = "@Rebalance_frequency";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = Rebalance_frequency;
                       storedProcParams[29] = param;

                       //Param 31
                       param = command.CreateParameter();
                       param.ParameterName = "@AutoRebalance";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = AutoRebalance;
                       storedProcParams[30] = param;

                       //Param 32
                       param = command.CreateParameter();
                       param.ParameterName = "@SecurityDrift";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = SecurityDrift;
                       storedProcParams[31] = param;

                       //Param 33
                       param = command.CreateParameter();
                       param.ParameterName = "@SectorDrift";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = SectorDrift;
                       storedProcParams[32] = param;

                        //Param 34
                       param = command.CreateParameter();
                       param.ParameterName = "@eligible";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = eligible;
                       storedProcParams[33] = param;

                       



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_update_general_accounts", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();


                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

       }

       public DataSet se_get_top_accounts(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_top_accounts", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_assets_under_management(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_assets_under_management", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_groups(out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                       //IDbDataParameter param;

                       ////Param 1
                       //param = command.CreateParameter();
                       //param.ParameterName = "@account_id";
                       //param.Direction = ParameterDirection.Input;
                       //param.DbType = DbType.Int32;
                       //param.Value = account_id;
                       //storedProcParams[0] = param;

                      



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_groups", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }
       

        public DataSet se_get_major_asset_selected(out ApplicationMessageList messages)

       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();
           List <Major_asset> MA = new List<Major_asset>();
           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@user_id  ";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = getUserId();
                       storedProcParams[0] = param;





                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_major_asset_selected", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                       //DataTable dataTable = ds.Tables[0];
                     
                       //if (dataTable.Rows.Count > 0)
                       //{
                       //    foreach (DataRow row in dataTable.Rows)
                       //    {

                       //        int _major_asset_code;
                       //        bool _selected;
                       //        string _description;

                       //        _major_asset_code = Convert.ToInt32(row["major_asset_code"]);

                       //        _selected = Convert.ToBoolean(row["selected"]);

                       //        _description = Convert.ToString(row["description"]);

                       //        MA.Add(new Major_asset(_major_asset_code,_description,_selected));
                             
                       //    }
                       //}
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public void se_update_major_asset_selected(bool selected, int major_asset_code, string description, int user_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();
            List<Major_asset> MA = new List<Major_asset>();
            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@selected";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Boolean;
                        param.Value = selected;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@major_asset_code";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = major_asset_code;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@description";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = description;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@user_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[3] = param;





                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_update_major_asset_selected", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                    
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
           
        }

       public DataSet se_get_account_share_class(int account_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[0] = param;






                       param = command.CreateParameter();
                       param.ParameterName = "@start_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = trade_date_start;
                       storedProcParams[1] = param;


                       param = command.CreateParameter();
                       param.ParameterName = "@end_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = trade_date_end;
                       storedProcParams[2] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_account_share_class", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_indicative_nav(int account_id, DateTime trade_date_start, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@start_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = trade_date_start;
                       storedProcParams[1] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_indicative_nav", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }


       public DataSet se_get_myNav(int MyNav_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@MyNav_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = MyNav_id;
                       storedProcParams[0] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_myNav", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }


       public DataSet se_get_contingency_dashboard(int account_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_contingency_dashboard", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public void se_update_contingency_dashboard(int account_share_class_audit_id, string type, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();
           List<Major_asset> MA = new List<Major_asset>();
           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_share_class_audit_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_share_class_audit_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@type";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = type;
                       storedProcParams[1] = param;

         

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@user_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = getUserId();
                       storedProcParams[2] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_update_contingency_dashboard", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);


                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

       }


       public DataSet se_get_message(DateTime trade_date_start, DateTime trade_date_end, string custodian,
            string Message_type, bool IsHeader, bool acknowleged, int processed, out ApplicationMessageList messages)

       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[7];
                       IDbDataParameter param;

                      

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@start_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = trade_date_start;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@end_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = trade_date_end;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@custodian";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = custodian;
                       storedProcParams[2] = param;

                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "@Message_type";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = Message_type;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@IsHeader";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = IsHeader;
                       storedProcParams[4] = param;

                       //Param 6
                       param = command.CreateParameter();
                       param.ParameterName = "@acknowleged";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = acknowleged;
                       storedProcParams[5] = param;

                       //Param 7
                       param = command.CreateParameter();
                       param.ParameterName = "@processed";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = processed;
                       storedProcParams[6] = param;




                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_message", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_message_count_grid( out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                      // IDbDataParameter param;

                       ////Param 1
                       //param = command.CreateParameter();
                       //param.ParameterName = "@account_id";
                       //param.Direction = ParameterDirection.Input;
                       //param.DbType = DbType.String;
                       //param.Value = account_id;
                       //storedProcParams[0] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_message_count_grid", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public int se_get_message_count(out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           int results = new int();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                      

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_message_count", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       while (reader.Read())
                       {
                           object[] values = new object[reader.FieldCount];
                           reader.GetValues(values);
                           results = Convert.ToInt32(values[0]);
                       }

                   }
               }
           }


           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }


           return results;
       }

       public DataSet se_get_message_groups(string custodian, string message_type, bool acknowleged, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@custodian";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = custodian;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@message_type";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = message_type;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@acknowleged ";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = acknowleged;
                       storedProcParams[2] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_message_groups", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);



                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_sub_message(int link_id, bool IsHeader, bool acknowleged, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@link_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = link_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@IsHeader";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = IsHeader;
                       storedProcParams[1] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@acknowleged";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = acknowleged;
                       storedProcParams[2] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_sub_message", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);



                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public void se_update_message(int message_id, bool acknowleged, string notes, int @assignment, int processed, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@message_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = message_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@acknowleged";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = acknowleged;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@notes";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = notes;
                       storedProcParams[2] = param;

                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "@assignment";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = assignment;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@processed";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = processed;
                       storedProcParams[4] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_update_message", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);



                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
          
       }

       public DataSet se_get_users(out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[0];


                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_users", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                   }
               }
           }


           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }


           return ds;
       }


       public int se_get_gauge_message_count(string guagetype, string custodian, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           int results = new int();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@guagetype";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = guagetype;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@custodian";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = custodian;
                       storedProcParams[1] = param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_gauge_message_count", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       while (reader.Read())
                       {
                           object[] values = new object[reader.FieldCount];
                           reader.GetValues(values);
                           results = Convert.ToInt32(values[0]);
                       }
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return results;
       }

       public DataSet se_get_gauge_type(out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                       //IDbDataParameter param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_gauge_type", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_corporate_actions(out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                       //IDbDataParameter param;

                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_corporate_actions", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_corporate_actions_positions(int account_id, int security_id, out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@security_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = security_id;
                       storedProcParams[1] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_corporate_actions_positions", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }


       public void process_corporate_actions( out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@ex_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = null;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@current_user";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = getUserId();
                       storedProcParams[1] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("process_corporate_actions", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }

       }


       public DataSet get_cmpl_breach_mgt_report(
            int account_id,
            bool show_closed_breaches,
            bool show_sleeping_breaches,
            bool show_reviewed_breaches,
            bool show_nonreportable_breaches,
            bool hide_expired_rule_breaches,
            int? case_owner_id,
            bool show_noaction_breaches,
            bool show_pass_breaches,
            bool show_approved_breaches,
            DateTime start_date,
            DateTime end_date,
            bool show_to_review_breaches,
            bool show_to_approved_breaches,
            bool show_active_fails,
            bool show_passive_fails,
            bool show_open_breaches,
            bool show_suspend_breaches,
            bool show_resovled_breaches,
            bool show_warning_breaches,
            out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[20];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.String;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@show_closed_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_closed_breaches;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@show_sleeping_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_sleeping_breaches;
                       storedProcParams[2] = param;


                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "@show_reviewed_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_reviewed_breaches;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@show_nonreportable_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_nonreportable_breaches;
                       storedProcParams[4] = param;

                       //Param 6
                       param = command.CreateParameter();
                       param.ParameterName = "@hide_expired_rule_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = hide_expired_rule_breaches;
                       storedProcParams[5] = param;

                     

                       //Param 7
                       param = command.CreateParameter();
                       param.ParameterName = "@case_owner_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = getUserId();
                       storedProcParams[6] = param;

                       //Param 8
                       param = command.CreateParameter();
                       param.ParameterName = "@show_noaction_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_noaction_breaches;
                       storedProcParams[7] = param;

                       //Param 9
                       param = command.CreateParameter();
                       param.ParameterName = "@show_pass_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_pass_breaches;
                       storedProcParams[8] = param;

                       //Param 10
                       param = command.CreateParameter();
                       param.ParameterName = "@show_approved_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_approved_breaches;
                       storedProcParams[9] = param;

                       //Param 11
                       param = command.CreateParameter();
                       param.ParameterName = "@start_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = start_date;
                       storedProcParams[10] = param;

                       //Param 12
                       param = command.CreateParameter();
                       param.ParameterName = "@end_date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = end_date;
                       storedProcParams[11] = param;

                       //Param 13
                       param = command.CreateParameter();
                       param.ParameterName = "@show_to_review_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_to_review_breaches;
                       storedProcParams[12] = param;

                       //Param 14
                       param = command.CreateParameter();
                       param.ParameterName = "@show_to_approved_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_to_approved_breaches;
                       storedProcParams[13] = param;

                       //Param 15
                       param = command.CreateParameter();
                       param.ParameterName = "@show_active_fails";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_active_fails;
                       storedProcParams[14] = param;

                       //Param 16
                       param = command.CreateParameter();
                       param.ParameterName = "@show_passive_fails";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_passive_fails;
                       storedProcParams[15] = param;

                       //Param 17
                       param = command.CreateParameter();
                       param.ParameterName = "@show_open_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_open_breaches;
                       storedProcParams[16] = param;

                       //Param 18
                       param = command.CreateParameter();
                       param.ParameterName = "@show_suspend_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_suspend_breaches;
                       storedProcParams[17] = param;

                       //Param 19
                       param = command.CreateParameter();
                       param.ParameterName = "@show_resovled_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_resovled_breaches;
                       storedProcParams[18] = param;

                       //Param 20
                       param = command.CreateParameter();
                       param.ParameterName = "@show_warning_breaches";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Boolean;
                       param.Value = show_warning_breaches;
                       storedProcParams[19] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_cmpl_breach_mgt_report", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }



       public void update_cmpl_case2(
            int cmpl_case_id,
            int cmpl_case_state_id,
            int active,
            int reviewed,
            int reportable,
            DateTime? awaken_time,
            int archive_ref_update,
            int? suppress_noaction_recurrences,
            int? approver,
            decimal? tolerance_lvl_pct,
            decimal? tolerance_value,
            out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[12];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@cmpl_case_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = cmpl_case_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@cmpl_case_state_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = cmpl_case_state_id;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@active";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = active;
                       storedProcParams[2] = param;


                       //Param 4
                       param = command.CreateParameter();
                       param.ParameterName = "@reviewed";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = reviewed;
                       storedProcParams[3] = param;

                       //Param 5
                       param = command.CreateParameter();
                       param.ParameterName = "@reportable";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = reportable;
                       storedProcParams[4] = param;

                       //Param 6
                       param = command.CreateParameter();
                       param.ParameterName = "@awaken_time";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = awaken_time;
                       storedProcParams[5] = param;

                       //Param 7
                       param = command.CreateParameter();
                       param.ParameterName = "@archive_ref_update";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = archive_ref_update;
                       storedProcParams[6] = param;



                       //Param 8
                       param = command.CreateParameter();
                       param.ParameterName = "@suppress_noaction_recurrences";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = suppress_noaction_recurrences;
                       storedProcParams[7] = param;

                       //Param 9
                       param = command.CreateParameter();
                       param.ParameterName = "@approver";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = approver;
                       storedProcParams[8] = param;

                       //Param 10
                       param = command.CreateParameter();
                       param.ParameterName = "@tolerance_lvl_pct";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = tolerance_lvl_pct;
                       storedProcParams[9] = param;

                       //Param 11
                       param = command.CreateParameter();
                       param.ParameterName = "@tolerance_value";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Decimal;
                       param.Value = tolerance_value;
                       storedProcParams[10] = param;

                       //Param 12
                       param = command.CreateParameter();
                       param.ParameterName = "@current_user";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = getUserId();
                       storedProcParams[11] = param;



                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_update_cmpl_case2", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
          
       }

       public  DataSet get_cmpl_breach_details(
            int cmpl_case_id,
            int display_currency_id,
            int cmpl_invocation_id,
            out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@cmpl_case_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = cmpl_case_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@display_currency_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = display_currency_id;
                       storedProcParams[1] = param;

                       //Param 3
                       param = command.CreateParameter();
                       param.ParameterName = "@cmpl_invocation_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = cmpl_invocation_id;
                       storedProcParams[2] = param;


                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("get_cmpl_breach_details", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }


       public DataSet get_cmpl_breach_actions(
          int cmpl_case_id,
          out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@cmpl_case_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = cmpl_case_id;
                       storedProcParams[0] = param;

               
                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("get_cmpl_breach_actions", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_parent_mutual_funds_orders_info(
            int account_id,
            bool include_orders,
            out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@include_orders";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = include_orders;
                       storedProcParams[1] = param;


                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_parent_mutual_funds_orders_info", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

       public DataSet se_get_parent_mutual_funds_info(
          int account_id,
          bool include_orders,
          out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@include_orders";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = include_orders;
                       storedProcParams[1] = param;


                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_parent_mutual_funds_info", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

         public  DataSet se_get_process_status(
            int account_id,
            DateTime date,
            out ApplicationMessageList messages)
       {
           messages = new ApplicationMessageList();
           DataSet ds = new DataSet();

           try
           {
               using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
               {
                   connection.Open();

                   using (ILinedataDbCommand command = connection.CreateCommand())
                   {
                       command.CommandType = CommandType.StoredProcedure;
                       IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                       IDbDataParameter param;

                       //Param 1
                       param = command.CreateParameter();
                       param.ParameterName = "@account_id";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.Int32;
                       param.Value = account_id;
                       storedProcParams[0] = param;

                       //Param 2
                       param = command.CreateParameter();
                       param.ParameterName = "@date";
                       param.Direction = ParameterDirection.Input;
                       param.DbType = DbType.DateTime;
                       param.Value = date;
                       storedProcParams[1] = param;


                       storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                       command.CommandText = connection.BuildCommandText("se_get_process_status", storedProcParams);

                       foreach (IDbDataParameter paramToAdd in storedProcParams)
                       { command.Parameters.Add(paramToAdd); }

                       IDataReader reader = command.ExecuteReader();

                       ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                   }
               }
           }

           catch (Exception ex)
           {
               ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
               messages.Add(message);
           }
           return ds;
       }

         public Contingent se_get_indicative_nav1(int account_id,
          DateTime start_date, int share_class_type_code,
          out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             Contingent AI = new Contingent();
             DataSet ds = null;

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = account_id;
                         storedProcParams[0] = param;
                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@start_date";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = start_date;
                         storedProcParams[1] = param;
                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@share_class_type_code";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = share_class_type_code;
                         storedProcParams[2] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_indicative_nav", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                         DataTable dataTable = ds.Tables[0];
                         if (dataTable.Rows.Count > 0)
                         {
                             foreach (DataRow row in dataTable.Rows)
                             {
                                 AI.Account_id = Convert.ToInt32(row["account_id"]);
                                 AI.Account_name = row["Account_name"].ToString();
                                 AI.Share_class =  row["Share_class"].ToString();
                                 AI.Share_class_type_code = Convert.ToInt32(row["share_class_type_code"]);
                                 AI.Class_ratio = Convert.ToDecimal(row["Class_Ratio"]);
                                 AI.Starting_nav = Convert.ToDecimal(row["Starting_Nav"]);
                                 AI.Income = Convert.ToDecimal(row["Income_Daily_Accrual"]);
                                 AI.Amort = Convert.ToDecimal(row["AmortAccret_Daily_Accrual"]);
                                 AI.Expense = Convert.ToDecimal(row["ExpenseDailyAccrualAllocation"]);
                                 AI.Expenseclass = Convert.ToDecimal(row["ClassSpecificDailyExpenseAccruals"]);
                                 AI.Realizedgl = Convert.ToDecimal(row["RealizedGL"]);
                                 AI.Realizedcurrgl = Convert.ToDecimal(row["RealizedCurrGL"]);
                                 AI.Unrealizedgl = Convert.ToDecimal(row["UnrealizedGL"]);
                                 AI.Unrealizedcurrgl = Convert.ToDecimal(row["FwdUnrealizedGL"]);
                                 AI.Distibution = Convert.ToDecimal(row["Distribution"]);
                                 AI.CapitalStock = Convert.ToDecimal(row["CapitalStock"]);
                                 AI.Ending_nav = Convert.ToDecimal(row["IndTotalNetAssets"]);
                                 AI.SubsReds = Convert.ToDecimal(row["subsReds"]);
                                 AI.Starting_sharesoutstanding = Convert.ToDecimal(row["StartingSharesOutstanding"]);
                                 AI.Ending_sharesoutstanding = Convert.ToDecimal(row["SharesOutstanding"]);
                                 AI.Contingentnav = Convert.ToDecimal(row["IndNAVperShare"]);
                                 AI.Actualnav = Convert.ToDecimal(row["ActualNAVperShare"]);
                                 AI.NavDate = Convert.ToDateTime(row["Asof"]);

                       
                             }
                         }
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return AI;
         }


         public DataSet GetIssuerInfo()
         {
             ApplicationMessageList messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         int numberOfParams = 0;
                         int actualParams = 0;

                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                        



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("get_list_issuer", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         {
                             command.Parameters.Add(paramToAdd);
                         }
                         Debug.Assert(actualParams == numberOfParams);

                         IDataReader reader = command.ExecuteReader();
                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                     }
                 }
             }
             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

             return ds;
         }




         public  DataSet se_get_ticket_report(int account, int security, int issuer, string search,
            DateTime trade_date_start, DateTime trade_date_end, Int32 m_intAssetCode, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[8];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = account;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@security";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = security;
                         storedProcParams[1] = param;

                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@issuer";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = issuer;
                         storedProcParams[2] = param;

                         //Param 4
                         param = command.CreateParameter();
                         param.ParameterName = "@search";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = search;
                         storedProcParams[3] = param;


                         //Param 5
                         param = command.CreateParameter();
                         param.ParameterName = "@trade_date_start";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = trade_date_start;
                         storedProcParams[4] = param;

                         //Param 6
                         param = command.CreateParameter();
                         param.ParameterName = "@trade_date_end";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = trade_date_end;
                         storedProcParams[5] = param;

                         //Param 7
                         param = command.CreateParameter();
                         param.ParameterName = "@userId";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = getUserId();
                         storedProcParams[6] = param;

                         //Param 8
                         param = command.CreateParameter();
                         param.ParameterName = "@m_intAssetCode";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = m_intAssetCode;
                         storedProcParams[7] = param;

                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_ticket_report", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }


         public DataSet se_get_positions_report(string account, string security, string issuer, string search, decimal? user_id, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = account;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@security";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = security;
                         storedProcParams[1] = param;

                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@issuer";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = issuer;
                         storedProcParams[2] = param;

                         //Param 4
                         param = command.CreateParameter();
                         param.ParameterName = "@search";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = search;
                         storedProcParams[3] = param;

                         //Param 5
                         param = command.CreateParameter();
                         param.ParameterName = "@userId";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = user_id1;
                         storedProcParams[4] = param;


                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_positions_report", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }

         public Security GetSecurityBySymbol(string symbol, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();


             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@symbol";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = symbol;
                         storedProcParams[0] = param;


                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_security_by_symbol", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         //ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                         while (reader.Read())
                         {
                             object[] values = new object[reader.FieldCount];
                             reader.GetValues(values);
                             ds = new Security(
                               Convert.ToInt32(values[0]),
                               Convert.ToString(values[1]),
                               Convert.ToString(values[2]),
                               Convert.ToInt32(values[3]),
                               Convert.ToString(values[4])
                             );


                         }
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }

         public DataSet GetIssuername(int security_id, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@security_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = security_id;
                         storedProcParams[0] = param;


                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_Issuer_info", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);



                     }
                 }
             }


             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }

         public DataSet GetListAllDesks()
         {
             ApplicationMessageList messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;

                         command.CommandText = connection.BuildCommandText("get_list_all_desks", null);

                         IDataReader reader = command.ExecuteReader();
                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }
             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

             return ds;
         }

        

         public DataSet se_get_tradervspm_orders(int desk_id, string account, string security, string issuer,
        string search, DateTime trade_date_start, DateTime trade_date_end, bool active, int major_asset_code, bool crossable, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[10];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@desk_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = desk_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@account";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = account;
                         storedProcParams[1] = param;

                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@security";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = security;
                         storedProcParams[2] = param;

                         //Param 4
                         param = command.CreateParameter();
                         param.ParameterName = "@issuer";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = issuer;
                         storedProcParams[3] = param;

                         //Param 5
                         param = command.CreateParameter();
                         param.ParameterName = "@search";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = search;
                         storedProcParams[4] = param;


                         //Param 6
                         param = command.CreateParameter();
                         param.ParameterName = "@trade_date_start";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = trade_date_start;
                         storedProcParams[5] = param;

                         //Param 7
                         param = command.CreateParameter();
                         param.ParameterName = "@trade_date_end";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = trade_date_end;
                         storedProcParams[6] = param;

                         //Param 8
                         param = command.CreateParameter();
                         param.ParameterName = "@active";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Boolean;
                         param.Value = active;
                         storedProcParams[7] = param;

                         //Param 9
                         param = command.CreateParameter();
                         param.ParameterName = "@major_asset_code";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = major_asset_code;
                         storedProcParams[8] = param;

                         //Param 9
                         param = command.CreateParameter();
                         param.ParameterName = "@crossable";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Boolean;
                         param.Value = crossable;
                         storedProcParams[9] = param;

                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_tradervspm_orders", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }

         public DataSet se_get_journaling(int Block_id, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@block_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = Block_id;
                         storedProcParams[0] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_journaling", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }


         public void se_update_journaling(int Block_id, string journal_entry, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();


             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@block_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = Block_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@journal_entry";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = journal_entry;
                         storedProcParams[1] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@user_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = getUserId();
                         storedProcParams[2] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_update_journaling", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();


                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

         }

         public void se_update_committment_price(int Block_id, decimal price, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();


             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@block_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = Block_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@price";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = price;
                         storedProcParams[1] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@user_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = getUserId();
                         storedProcParams[2] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_update_committment_price", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();


                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

         }

         public DataSet se_get_generic_param(string report, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@report";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = report;
                         storedProcParams[0] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_generic_param", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }


         public DataSet  se_get_generic_view(int account_id,int desk_id,int security_id,DateTime startdate,DateTime enddate, int block_id,string report, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[8];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = account_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@desk_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = desk_id;
                         storedProcParams[1] = param;

                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@security_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = security_id;
                         storedProcParams[2] = param;

                         //Param 4
                         param = command.CreateParameter();
                         param.ParameterName = "@startdate";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = startdate;
                         storedProcParams[3] = param;


                         //Param 5
                         param = command.CreateParameter();
                         param.ParameterName = "@enddate";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = enddate;
                         storedProcParams[4] = param;

                         //Param 6
                         param = command.CreateParameter();
                         param.ParameterName = "@block_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = block_id;
                         storedProcParams[5] = param;

                         //Param 6
                         param = command.CreateParameter();
                         param.ParameterName = "@block_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = block_id;
                         storedProcParams[5] = param;

                         //Param 7
                         param = command.CreateParameter();
                         param.ParameterName = "@report";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = report;
                         storedProcParams[6] = param;

                         //Param 8
                         param = command.CreateParameter();
                         param.ParameterName = "@user_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = getUserId();
                         storedProcParams[7] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_generic_view", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }

         public DataSet se_get_account_children(decimal account_id, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.String;
                         param.Value = account_id;
                         storedProcParams[0] = param;

                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_account_children", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }


         public DataSet se_get_account_children_with_percents(decimal account_id, decimal cash_amount, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = account_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@cash_amount";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = cash_amount;
                         storedProcParams[1] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_account_children_with_percents", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }


         public void se_update_cash_position(decimal account_id, decimal cash_amount, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = account_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@cash_amount";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = cash_amount;
                         storedProcParams[1] = param;

                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@current_user";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Decimal;
                         param.Value = getUserId();
                         storedProcParams[2] = param;

                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_update_cash_position", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

         }

         public void delete_proposed(int order_id, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@order_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = order_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@allow_delete_pre_executed";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = 0;
                         storedProcParams[1] = param;


                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@current_user";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = getUserId();
                         storedProcParams[2] = param;


                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("delete_proposed", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

         }

         public void delete_order(int order_id, out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@order_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = order_id;
                         storedProcParams[0] = param;

                         


                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@current_user";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = getUserId();
                         storedProcParams[1] = param;


                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("delete_order", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

         }

         public DataSet get_nav_control_types()
         {
             ApplicationMessageList messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         int numberOfParams = 0;
                         int actualParams = 0;

                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                         // IDbDataParameter param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("get_nav_control_types", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         {
                             command.Parameters.Add(paramToAdd);
                         }
                         Debug.Assert(actualParams == numberOfParams);

                         IDataReader reader = command.ExecuteReader();
                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                     }
                 }
             }
             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

             return ds;
         }

         public DataSet  se_get_nav_multi_approval_view(decimal account_id, DateTime nav_date,int control_type_code, 
             int loadhist_definition_id, bool focusfail, bool focusData,out ApplicationMessageList messages)
         {
             messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[7];
                         IDbDataParameter param;

                         //Param 1
                         param = command.CreateParameter();
                         param.ParameterName = "@account_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = account_id;
                         storedProcParams[0] = param;

                         //Param 2
                         param = command.CreateParameter();
                         param.ParameterName = "@nav_date";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.DateTime;
                         param.Value = nav_date;
                         storedProcParams[1] = param;

                         //Param 3
                         param = command.CreateParameter();
                         param.ParameterName = "@control_type_code";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = control_type_code;
                         storedProcParams[2] = param;

                         //Param 4
                         param = command.CreateParameter();
                         param.ParameterName = "@loadhist_definition_id";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = loadhist_definition_id;
                         storedProcParams[3] = param;

                         //Param 5
                         param = command.CreateParameter();
                         param.ParameterName = "@current_user";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Int32;
                         param.Value = getUserId();
                         storedProcParams[4] = param;

                         //Param 6
                         param = command.CreateParameter();
                         param.ParameterName = "@focusfail";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Boolean;
                         param.Value = focusfail;
                         storedProcParams[5] = param;

                         //Param 7
                         param = command.CreateParameter();
                         param.ParameterName = "@focusData";
                         param.Direction = ParameterDirection.Input;
                         param.DbType = DbType.Boolean;
                         param.Value = focusData;
                         storedProcParams[6] = param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_nav_multi_approval_view", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         { command.Parameters.Add(paramToAdd); }

                         IDataReader reader = command.ExecuteReader();

                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                     }
                 }
             }

             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }
             return ds;
         }


         public DataSet se_get_loadhist()
         {
             ApplicationMessageList messages = new ApplicationMessageList();
             DataSet ds = new DataSet();

             try
             {
                 using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                 {
                     connection.Open();

                     using (ILinedataDbCommand command = connection.CreateCommand())
                     {
                         int numberOfParams = 0;
                         int actualParams = 0;

                         command.CommandType = CommandType.StoredProcedure;
                         IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                         // IDbDataParameter param;



                         storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                         command.CommandText = connection.BuildCommandText("se_get_loadhist", storedProcParams);

                         foreach (IDbDataParameter paramToAdd in storedProcParams)
                         {
                             command.Parameters.Add(paramToAdd);
                         }
                         Debug.Assert(actualParams == numberOfParams);

                         IDataReader reader = command.ExecuteReader();
                         ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                     }
                 }
             }
             catch (Exception ex)
             {
                 ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                 messages.Add(message);
             }

             return ds;
         }

          public string se_get_nav_comments(int comment_id,int comment_type, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();



            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@comment_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = comment_id;
                        storedProcParams[0] = param;

                          //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@comment_type";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = comment_type;
                        storedProcParams[1] = param;





                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_nav_comments", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToString(values[0]);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


            return results;
        }

          public void se_update_nav_comments(int comment_id, int comment_type, int nav_res_rule_result_id, string comment, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@comment_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = comment_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@comment_type";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = comment_type;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@nav_res_rule_result_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = nav_res_rule_result_id;
                          storedProcParams[2] = param;

                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@comment";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.String;
                          param.Value = comment;
                          storedProcParams[3] = param;

                          //Param 5
                          param = command.CreateParameter();
                          param.ParameterName = "@current_user";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = getUserId();
                          storedProcParams[4] = param;


                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_update_nav_comments", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

          }


          public void se_update_nav_ruleset_comments(int first_or_dq_rev_comm_text_id, int comment_type, int nav_res_ruleset_review_id, bool nav_status_ok, string comment, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@first_or_dq_rev_comm_text_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = first_or_dq_rev_comm_text_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@comment_type";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = comment_type;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@nav_res_ruleset_review_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = nav_res_ruleset_review_id;
                          storedProcParams[2] = param;


                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@nav_status_ok";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Boolean;
                          param.Value = nav_status_ok;
                          storedProcParams[3] = param;

                          //Param 5
                          param = command.CreateParameter();
                          param.ParameterName = "@comment";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.String;
                          param.Value = comment;
                          storedProcParams[4] = param;

                          //Param 6
                          param = command.CreateParameter();
                          param.ParameterName = "@first_or_dq_rev_by";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = getUserId();
                          storedProcParams[5] = param;


                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_update_nav_ruleset_comments", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

          }

          public DataSet se_get_nav_account_tree(int account_id, int intraday_code_id, DateTime snapshot_date_in, int control_type, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@intraday_code_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = intraday_code_id;
                          storedProcParams[1] = param;




                          param = command.CreateParameter();
                          param.ParameterName = "@snapshot_date_in";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.DateTime;
                          param.Value = snapshot_date_in;
                          storedProcParams[2] = param;


                          param = command.CreateParameter();
                          param.ParameterName = "@control_type";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = control_type;
                          storedProcParams[3] = param;

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_nav_account_tree", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet nq_get_nav_linked_reports(int nav_res_rule_result_id, int nav_report_order, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@nav_res_rule_result_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = nav_res_rule_result_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@nav_report_order";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = nav_report_order;
                          storedProcParams[1] = param;

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("nq_get_nav_linked_reports", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_currency_mvmt_report_sum(int account_id, int display_currency_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@display_currency_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = display_currency_id;
                          storedProcParams[1] = param;

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_currency_mvmt_report_sum", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_currency_mvmt_detail(DateTime from_date, DateTime end_date, int account_id, int currency_code, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@from_date";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.DateTime;
                          param.Value = from_date;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@end_date";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.DateTime;
                          param.Value = end_date;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[2] = param;

                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@currency_code";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = currency_code;
                          storedProcParams[3] = param;

                          //Param 5
                          param = command.CreateParameter();
                          param.ParameterName = "@current_user";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = getUserId();
                          storedProcParams[4] = param;

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_currency_mvmt_detail", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public void se_create_orders(int principal_currency_id, int counter_currency_id, DateTime settlementDate, int account_id, decimal quantity, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[5];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@principal_currency_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = principal_currency_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@counter_currency_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = counter_currency_id;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@settlementDate";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.DateTime;
                          param.Value = settlementDate;
                          storedProcParams[2] = param;


                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[3] = param;

                          //Param 5
                          param = command.CreateParameter();
                          param.ParameterName = "@quantity";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Decimal;
                          param.Value = quantity;
                          storedProcParams[4] = param;

                        


                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_create_orders", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

          }

          public DataSet GetAllAccounts2()
          {
              ApplicationMessageList messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@isAdmin";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = 1;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@includeAdHoc";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = 0;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@current_user";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = getUserId();
                          storedProcParams[2] = param;


                     
                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("get_list_account", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

              return ds;

          }

          public void ValidateAccountForUser(string accountShortName, out int defaultAccountId, out string defaultShortName)
          {
              defaultAccountId = -1;
              defaultShortName = "";
              int numberOfParams = 1;
              int actualParams = 0;

              ApplicationMessageList messages = new ApplicationMessageList();
              DataSet ds = new DataSet();
              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                          IDbDataParameter param;

                          param = command.CreateParameter();
                          param.ParameterName = "@current_user";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = getUserId();
                          storedProcParams[actualParams++] = param;

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("get_account_tree", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          {
                              command.Parameters.Add(paramToAdd);
                          }
                          Debug.Assert(actualParams == numberOfParams);

                          IDataReader reader = command.ExecuteReader();
                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                          if (ds.Tables.Count > 1)
                          {
                              for (int rowIndex = 0; rowIndex < ds.Tables[1].Rows.Count; ++rowIndex)
                              {
                                  if (accountShortName.ToUpper().CompareTo(((ds.Tables[1].Rows[rowIndex])["short_name"]).ToString().ToUpper()) == 0)
                                  {
                                      defaultShortName = ((ds.Tables[1].Rows[rowIndex])["short_name"]).ToString();
                                      defaultAccountId = Convert.ToInt32((ds.Tables[1].Rows[rowIndex])["account_id"]);
                                      break;
                                  }
                              }
                          }
                      }
                  }
              }
              catch (Exception ex)
              {
                  throw new Exception(ex.Message);
              }
          }

          public DataSet GetSecurityInfo(string symbol)
          {
              ApplicationMessageList messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          int numberOfParams = 1;
                          int actualParams = 0;

                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                          IDbDataParameter param;

                          //Param
                          param = command.CreateParameter();
                          param.ParameterName = "@symbol";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.String;
                          param.Value = symbol;
                          storedProcParams[actualParams++] = param;

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("psg_get_security_and_price", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          {
                              command.Parameters.Add(paramToAdd);
                          }
                          Debug.Assert(actualParams == numberOfParams);

                          IDataReader reader = command.ExecuteReader();
                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }
              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

              return ds;
          }


          public DataSet se_get_bb_data(int account_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_bb_data", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public void se_update_bb_data(int account_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_update_bb_data", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

          }

          public DataSet se_get_modelcount( out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[0];
                         

                        
                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_modelcount", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }


          public DataSet se_get_accounts_on_model(int model_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@model_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = model_id;
                          storedProcParams[0] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_accounts_on_model", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

              return ds;
          }

          public DataSet se_get_securities_on_model(int model_id, int account_id, int filterType, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@model_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = model_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@filter_type";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = filterType;
                          storedProcParams[2] = param; 

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_securities_on_model", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_top_of_model(int model_id, int topx, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@model_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = model_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@topx";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = topx;
                          storedProcParams[1] = param;

                       

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_top_of_model", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_yield_curve(string name, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@name";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.String;
                          param.Value = name;
                          storedProcParams[0] = param;

                         


                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_yield_curve", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_parallel_shift(string name, decimal pct_change, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@name";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.String;
                          param.Value = name;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@pct_change";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Decimal;
                          param.Value = pct_change;
                          storedProcParams[1] = param;





                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_parallel_shift", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_position_curve(int account_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

              

                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_position_curve", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_bond_change(int account_id, decimal pct_change, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@pct_change";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Decimal;
                          param.Value = pct_change;
                          storedProcParams[1] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_bond_change", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_analytic_ditribution(int account_id, int analytic_value, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@analytic_value";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value =analytic_value;
                          storedProcParams[1] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_analytic_ditribution", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet se_get_contibution_ditribution(int account_id, int analytic_value, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@analytic_value";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = analytic_value;
                          storedProcParams[1] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_contibution_ditribution", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public DataSet  se_get_cash_flows(int account_id, int security_id, DateTime date_start, DateTime date_end, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@security_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = security_id;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@date_start";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.DateTime;
                          param.Value = date_start;
                          storedProcParams[2] = param;

                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@date_end";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.DateTime;
                          param.Value = date_end;
                          storedProcParams[3] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_cash_flows", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }


          public DataSet se_get_desk_trades(int desk_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@desk_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = desk_id;
                          storedProcParams[0] = param;




                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_desk_trades", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }


          public DataSet se_get_execution_info(int block_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@block_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = block_id;
                          storedProcParams[0] = param;




                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_execution_info", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }


          public DataSet se_get_execution_info_by_broker(int block_id, int broker_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@block_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = block_id;
                          storedProcParams[0] = param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@broker_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = broker_id;
                          storedProcParams[1] = param;




                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_execution_info_by_broker", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }


          public DataSet rpx_account_compare(int account_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@account_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = account_id;
                          storedProcParams[0] = param;

                         



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("rpx_account_compare", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }


          public DataSet se_get_price_journal(int model_id, int security_id, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@model_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = model_id;
                          storedProcParams[0] = param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@security_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = security_id;
                          storedProcParams[1] = param;




                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_get_price_journal", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }
              return ds;
          }

          public void se_update_price_journal(int model_id, int security_id, string @entry, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@model_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = model_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@security_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = security_id;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@entry";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.String;
                          param.Value = entry;
                          storedProcParams[2] = param;

                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@current_user";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = getUserId();
                          storedProcParams[3] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_update_price_journal", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

          }

          
        public void se_update_price_history(int model_id, int security_id, decimal purchase_price, decimal target_price, out ApplicationMessageList messages)
          {
              messages = new ApplicationMessageList();
              DataSet ds = new DataSet();

              try
              {
                  using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                  {
                      connection.Open();

                      using (ILinedataDbCommand command = connection.CreateCommand())
                      {
                          command.CommandType = CommandType.StoredProcedure;
                          IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                          IDbDataParameter param;

                          //Param 1
                          param = command.CreateParameter();
                          param.ParameterName = "@model_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = model_id;
                          storedProcParams[0] = param;

                          //Param 2
                          param = command.CreateParameter();
                          param.ParameterName = "@security_id";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Int32;
                          param.Value = security_id;
                          storedProcParams[1] = param;

                          //Param 3
                          param = command.CreateParameter();
                          param.ParameterName = "@purchase_price";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Decimal;
                          param.Value = purchase_price;
                          storedProcParams[2] = param;

                          //Param 4
                          param = command.CreateParameter();
                          param.ParameterName = "@target_price";
                          param.Direction = ParameterDirection.Input;
                          param.DbType = DbType.Decimal;
                          param.Value = target_price;
                          storedProcParams[3] = param;



                          storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                          command.CommandText = connection.BuildCommandText("se_update_price_history", storedProcParams);

                          foreach (IDbDataParameter paramToAdd in storedProcParams)
                          { command.Parameters.Add(paramToAdd); }

                          IDataReader reader = command.ExecuteReader();

                          ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                      }
                  }
              }

              catch (Exception ex)
              {
                  ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                  messages.Add(message);
              }

          }

        public DataSet se_get_dashoard_summary(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;





                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_dashoard_summary", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public List<AccountHierarchy> se_get_account_hierarchy(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            List<AccountHierarchy> AccountHierarchyList = new List<AccountHierarchy>();
            DataSet ds = null;

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_hierarchy", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                        DataTable dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                          

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AccountHierarchy Ah = new AccountHierarchy();
                                Ah.ParentSectorId = Convert.ToInt32(dt.Rows[i]["parent_sector_id"].ToString());
                                Ah.ParentName = dt.Rows[i]["ParentName"].ToString();
                                Ah.ChildSectorId = Convert.ToInt32(dt.Rows[i]["Child_sector_id"].ToString());
                                Ah.ChildName = dt.Rows[i]["ChildName"].ToString();
                                Ah.Symbol = dt.Rows[i]["symbol"].ToString();
                                Ah.SecurityID = Convert.ToInt32(dt.Rows[i]["security_id"].ToString());
                                Ah.Quantity = Convert.ToDecimal(dt.Rows[i]["booked_quantity"].ToString());
                                Ah.ProposedQuantity = Convert.ToDecimal(dt.Rows[i]["proposed_quantity"].ToString());
                                Ah.OrderQuantity = Convert.ToDecimal(dt.Rows[i]["order_quantity"].ToString());
                                Ah.TotalQuantity = Convert.ToDecimal(dt.Rows[i]["total_quantity"].ToString());
                                Ah.SecurityMV = Convert.ToDecimal(dt.Rows[i]["security_market_value"].ToString());
                                Ah.AccountMV = Convert.ToDecimal(dt.Rows[i]["account_market_value"].ToString());
                                if (Ah.AccountMV != 0)
                                Ah.SecurityPercent = Decimal.Round((Ah.SecurityMV / Ah.AccountMV * 100),2);
                                AccountHierarchyList.Add(Ah);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return AccountHierarchyList;
        }

        public DataSet se_get_analytic_benchmark(int account_id, int analytic_type, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[3];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@analytic_type";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = analytic_type;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[2] = param;




                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_analytic_benchmark", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_contingent_status(DateTime asofDate, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@asofDate";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = asofDate;
                        storedProcParams[0] = param;

                      

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_contingent_status", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();
                       
                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public string se_get_account_guid(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
        

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                       

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_guid", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            results = Convert.ToString(values[0]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return results;
        }

        public DataSet se_get_download_messages_history(DateTime date, int current, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@date";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = date;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@current";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = current;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_download_messages_history", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_download_messages_history_detail(DateTime message_time, string mnemonic, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@message_time";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.DateTime;
                        param.Value = message_time;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@mnemonic";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = mnemonic;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_download_messages_history_detail", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_security_data(int security_id, int major_asset_code, Boolean IsZeroOrNull, decimal percentChange, Boolean justHoldings, int Stale, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@major_asset_code";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = major_asset_code;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@IsZeroOrNull";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Boolean;
                        param.Value = IsZeroOrNull;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@percentChange";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = percentChange;
                        storedProcParams[3] = param;

                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@justHoldings";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Boolean;
                        param.Value = justHoldings;
                        storedProcParams[4] = param;

                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@Stale";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = Stale;
                        storedProcParams[5] = param;




                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_security_data", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_linked_cash(int desk_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@desk_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = desk_id;
                        storedProcParams[0] = param;

                      

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_linked_cash", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_rebal_sessions_account(int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_rebal_sessions_account", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_rebal_sessions(int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_rebal_sessions", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_rebal_sessions_security(int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_rebal_sessions_security", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_rebal_pos_exclusion(int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_rebal_pos_exclusion", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public DataSet se_get_account_replacement(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_account_replacement", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public void se_delete_replacement(int replacement_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@replacement_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = replacement_id;
                        storedProcParams[0] = param;


                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;




                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_delete_replacement", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public void se_update_replacement(int replacement_id, int buy_list_type_id, int buy_list_id, int replacement_list_type_id, int replacement_list_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@replacement_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = replacement_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@buy_list_type_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = buy_list_type_id;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@buy_list_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = buy_list_id;
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@replacement_list_type_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = replacement_list_type_id;
                        storedProcParams[3] = param;

                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@replacement_list_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = replacement_list_id;
                        storedProcParams[4] = param;


                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[5] = param;


                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_update_replacement", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public DataSet se_get_replacement_hierarchy()
        {
            ApplicationMessageList messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        int numberOfParams = 0;
                        int actualParams = 0;

                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                        // IDbDataParameter param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_replacement_hierarchy", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        {
                            command.Parameters.Add(paramToAdd);
                        }
                        Debug.Assert(actualParams == numberOfParams);

                        IDataReader reader = command.ExecuteReader();
                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

            return ds;
        }

        public DataSet se_get_replacement_country()
        {
            ApplicationMessageList messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        int numberOfParams = 0;
                        int actualParams = 0;

                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[numberOfParams];
                        // IDbDataParameter param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_replacement_country", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        {
                            command.Parameters.Add(paramToAdd);
                        }
                        Debug.Assert(actualParams == numberOfParams);

                        IDataReader reader = command.ExecuteReader();
                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

            return ds;
        }

        public DataSet se_get_adv_rebal_op(int account_id, int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_adv_rebal_op", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public Rebal_info rebal_info(int account_id, int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            Rebal_info RI = new Rebal_info();
            DataSet ds = null;

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                      
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[0] = param;




                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_rebal_sessions", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);

                        DataTable dataTable = ds.Tables[0];
                        if (dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                RI.Session = row["session"].ToString();
                                RI.Rebal_seesion_id = Convert.ToInt32(row["rebal_session_id"]);
                                RI.Model_id = Convert.ToInt32(row["model_id"]);
                                RI.Model_name= row["model_name"].ToString();
                                RI.Owner_id = Convert.ToInt32(row["owner_id"]);
                                RI.Owner_name = row["name"].ToString();
                                RI.Completion_date = Convert.ToDateTime(row["completion_date"]);
                                RI.Description = row["description"].ToString();
                                RI.Sell_non_holdings = row["Sell non model Holdings"].ToString();
                                RI.Exclude_encumbered= row["Exclude Encumbered"].ToString();
                                RI.IncludeAccrued = row["include_accrued_interest"].ToString();
                                RI.ClearPropsed = row["clear_proposed"].ToString();
                                RI.Normalize = row["Normalize Targets"].ToString();
                                RI.Prevent_over = row["prevent_over_adjustment"].ToString();
                                RI.Redistribute_dis_mv = row["redistribute_disqualified_mv"].ToString();
                                RI.Prevent_neg_cash = row["prevent_negative_cash"].ToString();
                                RI.Ignore_exist_cash = row["ignore_existing_cash"].ToString();
                                RI.Sell_odd = row["sell_off_odd_lots"].ToString();
                                RI.Exclude_unclass_min = row["exclude_unclass_for_min"].ToString();
                                RI.Min_qty = Convert.ToInt32(row["minimum_quantity"]);
                                RI.Round_qty = Convert.ToInt32(row["rounding_quantity"]);
                                RI.Restricted_list_processing = row["Restricted list Processing"].ToString();
                                RI.Severity_level = row["Severity Level"].ToString();
                                RI.Order_direction = row["Order Direction"].ToString();
                                RI.Price_type = row["Price Type"].ToString();
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return RI;
        }

        public void se_send_orders_faster(int account_id, int security_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[1] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_send_orders_faster", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public void runCompliance(decimal account_id, out ApplicationMessageList messages)
        {
            messages = new Linedata.Framework.Foundation.ApplicationMessageList();
            //   int results = new int();


            try
            {

                bool whatBool = Linedata.Server.Api.ApiObject.Compliance.RunComplianceOnAccount(account_id, out messages);

            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }


        }

        public DataSet se_get_rebal_detail_info(int @security_id, int session_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@session_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = session_id;
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_rebal_detail_info", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }


        public DataSet se_get_list_account(string @short_name, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[4];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@isAdmin";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = 0;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@includeAdHoc";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = 0;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[2] = param;

                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@short_name";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = short_name;
                        storedProcParams[3] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_list_account", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

            return ds;

        }


        public DataSet se_get_restricted_security(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;



                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_restricted_security", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public void se_run_rules(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[1] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_run_rules", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public DataSet se_get_restriction_report(int account_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[1];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_restriction_report", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }

        public void  se_add_first_override(int account_id, int security_id,string reason,int order_id, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[6];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;


                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@security_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = security_id;
                        storedProcParams[1] = param;

                        //Param 3
                        param = command.CreateParameter();
                        param.ParameterName = "@reason";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = reason;
                        storedProcParams[2] = param;


                        //Param 4
                        param = command.CreateParameter();
                        param.ParameterName = "@order_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = order_id;
                        storedProcParams[3] = param;

                        //Param 5
                        param = command.CreateParameter();
                        param.ParameterName = "@current_user";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = getUserId();
                        storedProcParams[4] = param;

                        //Param 6
                        param = command.CreateParameter();
                        param.ParameterName = "@position_type_code";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = 0;
                        storedProcParams[5] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_add_first_override", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public void se_add_internal_mf_cash(string short_name, decimal cash_amount, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@short_name";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = short_name;
                        storedProcParams[0] = param;


                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@cash_amount";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Decimal;
                        param.Value = cash_amount;
                        storedProcParams[1] = param;

                     

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_add_internal_mf_cash", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }

        }

        public DataSet se_get_funds_cash_by_fof(int account_id, int @include_orders, out ApplicationMessageList messages)
        {
            messages = new ApplicationMessageList();
            DataSet ds = new DataSet();

            try
            {
                using (ILinedataDbConnection connection = DbFactory.CreateConnection(ServerSettings.ApplicationDataSource, messages))
                {
                    connection.Open();

                    using (ILinedataDbCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        IDbDataParameter[] storedProcParams = new IDbDataParameter[2];
                        IDbDataParameter param;

                        //Param 1
                        param = command.CreateParameter();
                        param.ParameterName = "@account_id";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = account_id;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@include_orders";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        param.Value = @include_orders;
                        storedProcParams[1] = param;

                        storedProcParams = connection.AdjustParamsForServer(storedProcParams, 0);

                        command.CommandText = connection.BuildCommandText("se_get_funds_cash_by_fof", storedProcParams);

                        foreach (IDbDataParameter paramToAdd in storedProcParams)
                        { command.Parameters.Add(paramToAdd); }

                        IDataReader reader = command.ExecuteReader();

                        ds = ServiceTypeUtil.ConvertDataReaderToDataSet(reader);
                    }
                }
            }

            catch (Exception ex)
            {
                ApplicationMessage message = new ApplicationMessage(ex, ApplicationMessageType.Error);
                messages.Add(message);
            }
            return ds;
        }
    }
    }
    