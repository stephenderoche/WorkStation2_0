

ALTER procedure [dbo].[get_asof_portfolio]
(
	@account_id			numeric(10),
	@intraday_code_id		numeric(10) ,
	@snapshot_date_in 		datetime = null,
	@snapshot				tinyint = 0
)
as
	declare @snapshot_date datetime;
	declare @ccy_load_id numeric(10);
	declare @acct_ccy_id numeric(10);
	declare @ccy_load_def_id numeric(10);
	declare @xrate_acct_to_system float;
	declare @ccy_load_offset smallint;
	declare @ccy_asof_time datetime;
	declare @ccy_asof_hour smallint;
	declare @ccy_asof_minute smallint;
	declare @model_id numeric(10);
	declare @asof_time datetime;
	declare @asof_hour smallint;
	declare @asof_minute smallint;
	declare @ccy_data_vendor_code smallint;
	declare @account_audit_id numeric(10);
begin
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
	select @asof_time = @snapshot_date;
	select 
	@asof_hour = asof_hour,
	@asof_minute = asof_minute
	from loadhist_definition
	where loadhist_definition_id = @intraday_code_id;
	select @asof_time = dateadd(hh, @asof_hour, @asof_time);
	select @asof_time = dateadd(mi, @asof_minute, @asof_time);
	select 
		@account_audit_id = account_audit.account_audit_id,
		@model_id = default_model_id,
		@acct_ccy_id = home_currency_id
	from
	(
		select 
			asof_time,
			max(modified_time) as modified_time
		from account_audit
		where account_id = @account_id
			and asof_time = 
					(
						select 
							max(asof_time) 
						from account_audit
						where account_id = @account_id
							and asof_time <= @asof_time
					) 
		group by asof_time
	) max_modified
	join account_audit
		on account_audit.account_id = @account_id
		and max_modified.asof_time = account_audit.asof_time
		and max_modified.modified_time = account_audit.modified_time
	where @snapshot = 0
	or (@snapshot = 1 and max_modified.asof_time = @asof_time);
	select @ccy_load_def_id = nav_control_activation.curr_loadhist_definition_id,
		   @ccy_load_offset = nav_control_activation.curr_day_offset
	from nav_control_activation
	where nav_control_activation.account_id = @account_id
		and nav_control_activation.acct_loadhist_definition_id = @intraday_code_id
		and nav_control_activation.deleted = 0;
	select @ccy_data_vendor_code = loadhist_definition.data_vendor_code
	from loadhist_definition
	where loadhist_definition_id = @intraday_code_id;
	if @ccy_load_offset != 0
	begin
		select @ccy_asof_time = dateadd(dd, @ccy_load_offset, @snapshot_date);
	end else begin
		select @ccy_asof_time = @snapshot_date;
	end;
	select 
	@ccy_asof_hour = asof_hour,
	@ccy_asof_minute = asof_minute
	from loadhist_definition
	where loadhist_definition_id = @ccy_load_def_id;
	select @ccy_asof_time = dateadd(hh, @ccy_asof_hour, @ccy_asof_time);
	select @ccy_asof_time = dateadd(mi, @ccy_asof_minute, @ccy_asof_time);
	select @xrate_acct_to_system = currency.exchange_rate
	from 
	(
		select 
			asof_time,
			max(modified_time) as modified_time
		from currency_audit
		where security_id = @acct_ccy_id
			and asof_time = 
				(
					select 
						max(asof_time) 
					from currency_audit
					where security_id = @acct_ccy_id
						and data_vendor_code = @ccy_data_vendor_code
						and asof_time <= @ccy_asof_time
				)
		group by asof_time
	) max_currency_audit
	join currency_audit currency
		on currency.security_id = @acct_ccy_id
		and max_currency_audit.asof_time = currency.asof_time
		and max_currency_audit.modified_time = currency.modified_time
		and currency.data_vendor_code = @ccy_data_vendor_code
	where @snapshot = 0
	or (@snapshot = 1 and max_currency_audit.asof_time = @asof_time);
	select
	    
		null as loadhist_position_id, 
		position_audit.position_id as position_id, 
		null as loadhist_account_portfolio_id,  
		position_audit.account_id as account_id,
		position_audit.security_id as security_id,
		position_audit.position_type_code as position_type_code,
		position_audit.quantity as quantity,
		position_audit.market_value_account as market_value_account,
		position_audit.price_account as price_account,
		position_audit.cost_account as cost_account,
		position_audit.unit_cost_account as unit_cost_account,
		position_audit.legacy_cost as cost_legacy, 
		position_audit.accrued_income as accrued_income, 
		position_audit.accrued_income_account as accrued_income_account,
		position_audit.borrowing_fees_account as borrowing_fees_account,
		position_audit.underlying_price_account as underlying_price_account,
		case max_ccy_modified_time.exchange_rate when 0 then null
						else @xrate_acct_to_system * (1 / max_ccy_modified_time.exchange_rate) end as exchange_rate_acct_to_local,
		null as ex_rate_acct_to_underlying,
		position_audit.principal_factor as principal_factor,
		position_audit.delta as delta,
		position_audit.macaulay_duration as macaulay_duration,
		position_audit.modified_duration as modified_duration,
		position_audit.implied_volatility as implied_volatility,
		null as position_currency_id, 
		position_audit.purchased_currency_id as purchased_currency_id,
		position_audit.sold_currency_id as sold_currency_id,
		position_audit.currency_purchased_volume as currency_purchased_volume,
		position_audit.currency_sold_volume as currency_sold_volume,
		null as modified_by_user_id, 
		position_audit.modified_time as modified_time,		
		account_audit.net_asset_value_account as net_asset_value_account,
		account_audit.total_asset_value_account as total_asset_value_account,
		@xrate_acct_to_system as exchange_rate_acct_to_system,
		null as loadhist_id, 
		@snapshot_date as snapshot_date,
		loadhist_definition.loadhist_definition_id as intraday_code_id,
		loadhist_definition.mnemonic as intraday_code,
		position_audit.pricing_date as pricing_date,
		@model_id as default_model_id,
		null as total_market_value_account, -- doesn't seem to be used anywhere so do we care?
		security.symbol as symbol1,
		security.name_1 as name1
	from 
	(
		select
			max_asof_time.account_id,
			max_asof_time.security_id,
			max_asof_time.position_type_code,
			max_asof_time.position_audit_asof_time,
			max(position_audit.modified_time) as position_audit_modified_time
		from 
		(
			select
				position_audit.account_id,
				position_audit.security_id,
				position_audit.position_type_code,
				max(asof_time) as position_audit_asof_time
			from position_audit
				where position_audit.account_id = @account_id
					  and position_audit.asof_time <= @asof_time
			group by position_audit.account_id,
						position_audit.security_id,
						position_audit.position_type_code
		) max_asof_time 
		join position_audit
			on max_asof_time.account_id = position_audit.account_id
			and max_asof_time.security_id = position_audit.security_id
			and max_asof_time.position_type_code = position_audit.position_type_code
			and max_asof_time.position_audit_asof_time = position_audit.asof_time
		group by max_asof_time.account_id,
					max_asof_time.security_id,
					max_asof_time.position_type_code,
					max_asof_time.position_audit_asof_time
	) max_modified_time 
	join position_audit
		on max_modified_time.account_id = position_audit.account_id
		and max_modified_time.security_id = position_audit.security_id
		and max_modified_time.position_type_code = position_audit.position_type_code
		and max_modified_time.position_audit_asof_time = position_audit.asof_time
		and max_modified_time.position_audit_modified_time = position_audit.modified_time
		and position_audit.quantity <> 0		
	join loadhist_definition
		on @intraday_code_id = loadhist_definition.loadhist_definition_id
	join account_audit
		on  account_audit.account_audit_id = @account_audit_id
	join security on
	position_audit.security_id  = security.security_id
	left outer join
	(
		select
			max_sec_asof_time.security_id,
			security_audit.principal_currency_id,
			max_sec_asof_time.asof_time,
			max(modified_time) as modified_time
		from 
		(
			select 
				security_audit.security_id,
				max(asof_time) as asof_time
			from security_audit 
			where asof_time <= @asof_time
			group by security_audit.security_id
		) max_sec_asof_time
		join security_audit 
			on max_sec_asof_time.security_id = security_audit.security_id
			and max_sec_asof_time.asof_time = security_audit.asof_time
		group by max_sec_asof_time.security_id,
					security_audit.principal_currency_id,
					max_sec_asof_time.asof_time
	) max_sec_modified_time
	on position_audit.security_id = max_sec_modified_time.security_id
	left outer join
	(
		select
			max_ccy_asof_time.security_id,
			max_ccy_asof_time.data_vendor_code,
			currency_audit.exchange_rate,
			max_ccy_asof_time.asof_time,
			max(modified_time) as modified_time
		from 
		(
			select 
				currency_audit.security_id,
				currency_audit.data_vendor_code,
				max(asof_time) as asof_time
			from currency_audit 
			where asof_time <= @ccy_asof_time
				and @ccy_data_vendor_code = currency_audit.data_vendor_code
			group by currency_audit.security_id,
					currency_audit.data_vendor_code
		) max_ccy_asof_time
		join currency_audit 
			on max_ccy_asof_time.security_id = currency_audit.security_id
			and max_ccy_asof_time.data_vendor_code = currency_audit.data_vendor_code
			and max_ccy_asof_time.asof_time = currency_audit.asof_time
		group by max_ccy_asof_time.security_id,
					max_ccy_asof_time.data_vendor_code,
					currency_audit.exchange_rate,
					max_ccy_asof_time.asof_time
	) max_ccy_modified_time
	on max_sec_modified_time.principal_currency_id = max_ccy_modified_time.security_id
	where 
		position_audit.account_id = @account_id
		and account_audit.deleted = 0
		and( @snapshot = 0 or (@snapshot = 1 and position_audit_asof_time = @asof_time));
end
