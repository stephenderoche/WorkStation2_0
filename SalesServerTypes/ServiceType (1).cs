// <copyright file="DetailServiceType.cs" company="Linedata Services" >
//  Copyright (c) Linedata Services. All rights reserved.
// </copyright>
// <summary>
//  Contains the DetailServiceType interface.
// </summary>

namespace HierarchyViewerAddIn.Server.AddInDetailServiceTypes
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
    using HierarchyViewerAddIn.Shared.ServiceContracts;
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
    using Linedata.sales.Widget.Server.Factories;
   

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
       
        DataSet se_get_account_tree(decimal account_id, out ApplicationMessageList messages);
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
        DataSet se_get_commissions(string account, string broker, int commission_reason_code, DateTime Start_date, DateTime end_date, int Budget_start_date, int commission_budget_period_code, int view_type, out ApplicationMessageList messages);

        [OperationContract]
        DataSet GetBrokerInfo();

        DataSet rpx_cmpl_breach_sum(decimal account_id, int isAccountId, int user_id, int display_passes, DateTime fromDate, DateTime toDate, out ApplicationMessageList messages);
        List<Account> GetAccounts(ref ApplicationMessageList applicationMessageList);

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
    }
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        UseSynchronizationContext = false,
        Name = "HierarchyViewerServiceContract")]
    public class ServiceTypes : ServiceTypeBase, IHierarchyViewerServiceContract, IServiceTypes
    {
        static private string dsn = "";
     
        string results;


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




        public DataSet se_get_commissions(string account, string broker, int commission_reason_code, DateTime Start_date, DateTime end_date, int Budget_start_date, int commission_budget_period_code, int view_type, out ApplicationMessageList messages)
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
                        param.DbType = DbType.String;
                        param.Value = account;
                        storedProcParams[0] = param;

                        //Param 2
                        param = command.CreateParameter();
                        param.ParameterName = "@broker";
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        param.Value = broker;
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


        public List<Account> GetAccounts(ref ApplicationMessageList applicationMessageList)
        {
            List<Account> accounts = Linedata.sales.Widget.Server.Factories.AccountFactory.CreateAccount();

             
            return accounts;
        }


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

    
    
    }
    }