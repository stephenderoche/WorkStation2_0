USE [NAV_753]
GO
/****** Object:  StoredProcedure [dbo].[get_nav_account_non_security]    Script Date: 11/20/2018 10:55:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  procedure [dbo].[get_nav_account_non_security]
(	@account_id					numeric(10),
	@snapshot_date_in			datetime,
	@account_loadhist_def_id	numeric(10)
)
as
	declare @snapshot_date datetime;
begin

	DECLARE @continue_flag     bit,  
		 @cps_get_nav_account_non_security  varchar(60),  
		 @ret_val int    
 
	Select @continue_flag = 1

	select @cps_get_nav_account_non_security = name from sysobjects where name ='cps_get_nav_account_non_security' and type ='P'  
	if @cps_get_nav_account_non_security is not null  
	begin   
		execute @ret_val = @cps_get_nav_account_non_security @continue_flag output,  
													@account_id,
													@snapshot_date_in,
													@account_loadhist_def_id
	if @ret_val <> 0 and @ret_val < 60000
	begin  
		return @ret_val  
	end 
		else begin  
			if @continue_flag = 0  
			begin  
				return  
			end  
		end  
	end  
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	if @snapshot_date_in is null
	begin
		select @snapshot_date = convert(datetime, convert(nvarchar(10), getdate(), 112), 112);
	end else begin
		select @snapshot_date = @snapshot_date_in;
	end;
	select
		null as cmpl_account_balance_id,
		cmpl_acct_bal_audit.cmpl_acct_bal_audit_id as loadhist_account_balance_id,
		cmpl_acct_bal_audit.account_id,
		account.client_account_id,
		account.short_name as account_short_name,
		cmpl_acct_bal_audit.balance_id,
		cmpl_balance.name as balance_name,
		cmpl_acct_bal_audit.currency_id,
		security.symbol as currency_symbol,
		security.name_1 as currency_name,
		cmpl_acct_bal_audit.balance_value,
		cmpl_acct_bal_audit.exchange_rate_local_to_account as exchange_rate_local_to_account,
		cmpl_acct_bal_audit.comments,
		convert(datetime, convert(nvarchar(10), cmpl_acct_bal_audit.asof_time, 112), 112) as portfolio_date,
		loadhist_definition.loadhist_definition_id as intraday_code_id,
		loadhist_definition.mnemonic as intraday_code,
		account_audit.net_asset_value_account as net_asset_value_account,
		account_audit.total_asset_value_account as total_asset_value_account
	from loadhist_definition
		join cmpl_acct_bal_audit
			on  datepart(mi,cmpl_acct_bal_audit.asof_time) = loadhist_definition.asof_minute
			and datepart(hh,cmpl_acct_bal_audit.asof_time) = loadhist_definition.asof_hour
			and datepart(ss,cmpl_acct_bal_audit.asof_time) = 0
			and	datepart(millisecond,cmpl_acct_bal_audit.asof_time) = 0
			and cmpl_acct_bal_audit.account_id = @account_id
		join account
			on cmpl_acct_bal_audit.account_id = account.account_id
				and account.deleted = 0
				and account.ad_hoc_flag = 0
		join cmpl_balance
			on cmpl_acct_bal_audit.balance_id = cmpl_balance.cmpl_balance_id
		join security
			on cmpl_acct_bal_audit.currency_id = security.security_id
		join account_audit
			on cmpl_acct_bal_audit.asof_time = account_audit.asof_time
				and cmpl_acct_bal_audit.account_id = account_audit.account_id
	where 
		convert(datetime, convert(nvarchar(10), cmpl_acct_bal_audit.asof_time, 112), 112) = @snapshot_date
		and loadhist_definition.loadhist_definition_id = @account_loadhist_def_id
		and loadhist_definition.deleted  = 0
		and loadhist_definition.loadhist_definition_type_code = 1
		and cmpl_acct_bal_audit.deleted = 0;
end

