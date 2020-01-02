// <copyright file="IDetailServiceContract.cs" company="Linedata Services" >
//  Copyright (c) Linedata Services. All rights reserved.
// </copyright>
// <summary>
//  Contains the IDetailServiceContract interface.
// </summary>

namespace SalesSharedContracts
{
    using System.Data;
    using System.ServiceModel;
    using Linedata.Framework.Foundation;
    using Linedata.Shared.Api.ServiceModel;
    using System;
    using System.Collections.Generic;
   


    /// <summary>
    /// The shared service contract that defines the behavoir of the service.
    /// </summary>
    [AutoService("/SalesSharedContracts")] // Tells the Engine Host to load this service at the endpoint /{DsnId}/SdkSamples/DetailReport.
    [ServiceContract(Name = "SalesSharedContracts", Namespace = "http://www.ldsam.com/wcf/contracts/SalesSharedContracts")]
    [ServiceKnownType(typeof(ApplicationMessageList))]
    public interface ISalesSharedContracts
    {
        /// <summary>
        /// Gets the corresponding desk name from an id.
        /// </summary>
        /// <param name="deskId">The desk id.</param>
        /// <param name="messages">Ouput messages.</param>
        /// <returns>The corresponding desk name.</returns>


        //dasboard - general

        [OperationContract]
        DataSet se_get_hierarchy( out ApplicationMessageList messages);

        [OperationContract]
        string se_get_default_hierarchy(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_fi_asset_allocation_pru(int account_id, string Hierarchy, out ApplicationMessageList messages);
        [OperationContract]
        decimal? GetAccountID(string AccountName, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_get_account_info(int account_id, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_get_accounts(int account_id, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_get_security_info(int security_id, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_allocation_history(int account_id, int security_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_get_drift_dashboard(int account_id, int just_drift, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_get_top_securities_dasboard(int account_id, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_benchmark_vs_holdings(int account_id, string Hierarchy, out ApplicationMessageList messages);

        //DashBoard - Cash
        [OperationContract]
        DataSet se_cash_balance(int account_id, int is_account_id, int include_negatives, int include_positives, int user_id, int currency_id, out ApplicationMessageList messages);
        //DashBoard - Maturities
        [OperationContract]
        DataSet se_get_maturities(int account_id, int @Date_offset, out ApplicationMessageList messages);

        //DashBoard - Compliance

        [OperationContract]
        DataSet se_cmpl_get_top_security_breaches(int account_id, int userId, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_cmpl_case_statistics(int account_id, int userId, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_cmpl_get_top_account_breaches(int account_id, int userId, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_cmpl_get_breaches_by_servitiy(int account_id, int userId, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_cmpl_get_missing_data(int account_id, int userId, out ApplicationMessageList messages);

        //DashBoard - Positions

        [OperationContract]
        DataSet se_get_positions_info(int account_id, int userId, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_TaxLot_detail(int account_id, int security_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_tax_lot_chart(int account_id, int security_id, out ApplicationMessageList messages);

        //reporting



        [OperationContract]
        string se_get_cash_amount(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        string se_get_account_MV(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_tax_lot_position_data(int account_id, string symbol, out ApplicationMessageList messages);

        //robo dashboard
        [OperationContract]
        DataSet se_get_drift_summary(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_send_robo_proposed(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_drift_summary(int account_id, out ApplicationMessageList messages);

        //treeviewer
        [OperationContract]
        DataSet se_get_account_tree(decimal account_id, out ApplicationMessageList messages);

        [OperationContract]
        List<AccountTreeMap> se_get_account_tree1(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_nav_account_tree(int account_id, int intraday_code_id, DateTime snapshot_date_in, int control_type, out ApplicationMessageList messages);


        [OperationContract]
        int se_get_default_model_id(int account_id, out ApplicationMessageList messages);


        [OperationContract]
        DataSet se_aggregate_rebalance_sql(decimal account_id, decimal security_id, decimal @secondary_security_id, decimal suggested, decimal create_orders, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_child_household_accounts(Int32 @account_id, out ApplicationMessageList messages);


        [OperationContract]
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

        [OperationContract]
        DataSet se_get_commissions(int account_id, int broker_id, int commission_reason_code, DateTime Start_date, DateTime end_date, int Budget_start_date, int commission_budget_period_code, int view_type, out ApplicationMessageList messages);

        [OperationContract]
        DataSet GetBrokerInfo();

        //compliance rules

        [OperationContract]
        DataSet rpx_cmpl_breach_sum(decimal account_id, int isAccountId, int user_id, int display_passes, DateTime fromDate, DateTime toDate, out ApplicationMessageList messages);



        //[OperationContract]
        //List<Account> GetAccounts(ref ApplicationMessageList applicationMessageList);

        //Nav Dashboard
        [OperationContract]
        DataSet get_asof_account_dashboard(int account_id,  DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages);


        [OperationContract]
        DataSet get_asof_dashboard_detail(int account_id, int intraday_code_id, DateTime snapshot_date, int nav_control_type, out ApplicationMessageList messages);


        [OperationContract]
         DataSet GetListSecurity(int assetCode);


        [OperationContract]
        DataSet get_list_country();

        [OperationContract]
        DataSet se_get_ipo_data(int account_id, int country_code,decimal target,decimal MidPrice,decimal HighPrice,  out ApplicationMessageList messages);

         [OperationContract]
        DataSet se_create_orders_ipo(int security_id, int account_id, decimal @quantity, out ApplicationMessageList messages);

         [OperationContract]
         Account_info se_get_general_accounts(int account_id, out ApplicationMessageList messages);

         [OperationContract]
         DataSet se_get_top_issuers_dasboard(int account_id, out ApplicationMessageList messages);

         [OperationContract]
         DataSet se_get_performance_summary(int account_id, out ApplicationMessageList messages);
         [OperationContract]
         DataSet se_get_orders(int account_id, out ApplicationMessageList messages);

         [OperationContract]
         void validate_proposed_order(decimal order_id, decimal account_id, decimal position_type_code, decimal security_id, decimal validation_status, int current_user, out ApplicationMessageList messages);

         [OperationContract]
         void se_send_proposed( decimal account_id, decimal order_id,  out ApplicationMessageList messages);

         [OperationContract]
         void se_update_general_accounts(int account_id, string short_name, string model_name, string major_account, string account_holder,
             string account_holder_contact, string relief_method,
        string name

             , DateTime inception_date
             , decimal initial_funding_value
                  ,int user_id, DateTime close_date
                 , DateTime composite_entry_date, DateTime composite_exit_date,
              DateTime performance_start_date, int taxable, int distribution, decimal distribution_amount, string distribution_frequency, DateTime last_distribution, DateTime next_distribution,
              int ips_on_file, DateTime last_ips_review_date, string next_ips_review_date,
             DateTime last_reg_9_review 
            ,string reg_9_review_date
             , decimal management_fee
             ,string custodian,
             DateTime next_rebalance, string Rebalance_frequency, int AutoRebalance, int SecurityDrift, int SectorDrift 
             ,int eligible
            , out ApplicationMessageList messages);

        [OperationContract]
         DataSet se_get_top_accounts(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_assets_under_management(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_groups(out ApplicationMessageList messages);

        [OperationContract]
     
        DataSet se_get_major_asset_selected(out ApplicationMessageList messages);


        [OperationContract]

        void se_update_major_asset_selected(bool selected,int major_asset_code,string description,int user_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_account_share_class(int account_id, DateTime trade_date_start, DateTime trade_date_end, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_indicative_nav(int account_id,DateTime trade_date_start, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_myNav(int MyNav_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_contingency_dashboard(int account_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_contingency_dashboard(int account_share_class_audit_id, string type ,out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_message(DateTime trade_date_start, DateTime trade_date_end,string custodian, 
            string Message_type, bool IsHeader,bool acknowleged,int processed, out ApplicationMessageList messages);


        [OperationContract]
        int se_get_message_count( out ApplicationMessageList messages);


        [OperationContract]
        DataSet se_get_message_count_grid(out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_message_groups(string custodian, string message_type, bool acknowleged, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_sub_message(int link_id, bool IsHeader, bool acknowleged ,out ApplicationMessageList messages);

        [OperationContract]
        void se_update_message(int message_id, bool acknowleged, string notes, int @assignment,int processed,out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_users(out ApplicationMessageList messages);

        [OperationContract]
        int se_get_gauge_message_count(string guagetype, string custodian, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_gauge_type(out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_corporate_actions(out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_corporate_actions_positions(int account_id, int security_id, out ApplicationMessageList messages);

        [OperationContract]
        void process_corporate_actions( out ApplicationMessageList messages);

        [OperationContract]
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

        [OperationContract]
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


        [OperationContract]
        DataSet get_cmpl_breach_details(
            int cmpl_case_id,
            int display_currency_id,
            int cmpl_invocation_id,
            out ApplicationMessageList messages);

        [OperationContract]
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
            DateTime start_date,int share_class_type_code, 
            out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_ticket_report(int account, int security, int issuer, string search, 
            DateTime trade_date_start, DateTime trade_date_end, Int32 m_intAssetCode, out ApplicationMessageList messages);
        [DataContractFormat]
        Security GetSecurityBySymbol(string symbol, out ApplicationMessageList messages);
        [OperationContract]
        DataSet se_get_positions_report(string account, string security, string issuer, string search, decimal? user_id, out ApplicationMessageList messages);
        [OperationContract]
        DataSet GetIssuerInfo();
        [OperationContract]
        DataSet GetIssuername(int security_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet GetListAllDesks();

        [OperationContract]
        DataSet se_get_tradervspm_orders(int desk_id, string account, string security, string issuer,
        string search, DateTime trade_date_start, DateTime trade_date_end, bool active, int major_asset_code, bool crossable, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_journaling(int Block_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_journaling(int Block_id, string journal_entry, out ApplicationMessageList messages);

        [OperationContract]
        void se_update_committment_price(int Block_id, decimal price, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_generic_param(string report, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_generic_view(int account_id,int desk_id,int security_id,DateTime startdate,DateTime enddate, int block_id,string report, out ApplicationMessageList messages);



        [OperationContract]
        DataSet se_get_account_children(decimal account_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_account_children_with_percents(decimal account_id, decimal cash_amount, out ApplicationMessageList messages);

        [OperationContract]
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
        string se_get_nav_comments(int comment_id,int comment_type, out ApplicationMessageList messages);
        [OperationContract]
        void se_update_nav_comments(int comment_id, int comment_type, int nav_res_rule_result_id,string comment,out ApplicationMessageList messages);

        [OperationContract]
        void se_update_nav_ruleset_comments(int first_or_dq_rev_comm_text_id, int comment_type, int nav_res_ruleset_review_id, bool nav_status_ok, string comment, out ApplicationMessageList messages);

        [OperationContract]
        DataSet nq_get_nav_linked_reports(int nav_res_rule_result_id, int nav_report_order, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_currency_mvmt_report_sum(int account_id, int display_currency_id, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_currency_mvmt_detail(DateTime from_date, DateTime end_date, int account_id, int currency_code, out ApplicationMessageList messages);

        [OperationContract]
        void se_create_orders(int principal_currency_id, int counter_currency_id, DateTime settlementDate, int account_id, decimal quantity, out ApplicationMessageList messages);

        [OperationContract]
        DataSet GetAllAccounts2();

        [OperationContract]
        void ValidateAccountForUser(string accountShortName, out int defaultAccountId, out string defaultShortName);

        [OperationContract]
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
        DataSet se_get_securities_on_model(int model_id,int account_id,int filterType, out ApplicationMessageList messages);


        [OperationContract]
        DataSet se_get_top_of_model(int model_id, int topx, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_yield_curve(string name, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_parallel_shift(string name,decimal pct_change ,out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_position_curve(int account_id, out ApplicationMessageList messages);

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
        DataSet se_get_execution_info_by_broker(int block_id,int broker_id, out ApplicationMessageList messages);

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
        DataSet se_get_download_messages_history(DateTime date,int current, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_download_messages_history_detail(DateTime message_time, string mnemonic, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_security_data(int security_id, int major_asset_code,Boolean IsZeroOrNull,decimal percentChange,Boolean justHoldings,int Stale , out ApplicationMessageList messages);

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

        [OperationContract]
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
        void se_add_first_override(int account_id, int security_id,string reason,int order_id, out ApplicationMessageList messages);

        [OperationContract]
        void se_add_internal_mf_cash(string short_name, decimal cash_amount, out ApplicationMessageList messages);

        [OperationContract]
        DataSet se_get_funds_cash_by_fof(int account_id,int @include_orders , out ApplicationMessageList messages);
    }
}