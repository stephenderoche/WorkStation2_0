if exists (select * from sysobjects where name = 'rpx_account_compare')
begin
	drop procedure rpx_account_compare
	print 'PROCEDURE: rpx_account_compare dropped'
end
go
create procedure [dbo].[rpx_account_compare] --rpx_account_compare 199
(	
	@account_id numeric(10)
)
as
	declare @porfolio_market_value		float;
	declare @ret_val int;
	declare @continue_flag				int;
	declare @currency					nvarchar(40);
	declare @exchange_rate				float;
	declare @cps_rpx_account_compare	nvarchar(30);
	declare @cpe_rpx_account_compare	nvarchar(30);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	select @continue_flag = 1
select @cps_rpx_account_compare = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cps_rpx_account_compare'
	and sysobjects.type = 'P'
if @cps_rpx_account_compare is not null
begin
	execute @ret_val = cps_rpx_account_compare
		@continue_flag output, @account_id
	if (@ret_val != 0 and @ret_val < 60000) or @continue_flag = 0
	begin
		return @ret_val
	end
end
	select 
		@exchange_rate = currency.exchange_rate,
		@currency = currency.mnemonic	
	from currency
	join account
		on currency.security_id = account.home_currency_id
		and account.account_id = @account_id;
	execute @ret_val = get_account_market_value  
						@market_value = @porfolio_market_value output, 
						@account_id = @account_id, 
						@market_value_type = 7, 
						@currency_id = -1
						;
	select 
		coalesce(positions.quantity, 0)
			* position_type.security_sign					as quantity_booked,
		round(coalesce(positions.unit_cost_base, 0), 2)	as average_cost, 
		round(coalesce(positions.cost, 0) 
			* position_type.security_sign
			* security.pricing_factor 
            * security.principal_factor
			/ principal.exchange_rate, 2)					as cost, 
		security.name_1										as description,
		round(price.latest, 2)								as price, 
		getdate()												as run_date,
		'No Model'											as model_name,
		@account_id											as account_id,
		account.short_name									as short_name,
		account.name_1										as account_name,
		account.nav_date									as nav_date,
		round(coalesce(account.net_asset_value, 0), 12)		as net_asset_value,
		round(coalesce(account.total_asset_value, 0), 12)	as total_asset_value,
		@currency											as currency,
		'Non-Model Holdings'								as model_classification,
		security.symbol										as symbol,
		case
			when  security.major_asset_code <> 12 
				then
					round(positions.quantity 
							* position_type.security_sign
							* price.latest
							* security.pricing_factor 
							* security.principal_factor 
							/ coalesce(principal.exchange_rate, 1.0)
							+ positions.accrued_income 
							/ coalesce(income.exchange_rate, 1.0), 12)
			else round(coalesce(positions.present_market_value, 0) 
						/ coalesce(income.exchange_rate, 1.0), 12)
			end												as market_value, 
 		round(((case
			when  security.major_asset_code <> 12 
				then
					positions.quantity 
						* position_type.security_sign
						* price.latest
						* security.pricing_factor 
						* security.principal_factor 
						/ coalesce(principal.exchange_rate, 1.0)
						+ positions.accrued_income 
						/ coalesce(income.exchange_rate, 1.0)
			else coalesce(positions.present_market_value, 0) 
					/ coalesce(income.exchange_rate, 1.0) 
			end) / @porfolio_market_value), 4)				as percent_port, 
		0													as model_percent,
		round(@porfolio_market_value, 2)					as total_market_value
	from positions
	join security 
		on security.security_id = positions.security_id
	join account 
		on positions.account_id = account.account_id
	left outer join currency principal 
		on security.principal_currency_id = principal.security_id
    left outer join currency income    
		on security.income_currency_id = income.security_id
	left outer join price 
		on positions.security_id = price.security_id
	left outer join position_type            
		on positions.position_type_code = position_type.position_type_code
	left outer join model_security 
		on model_security.model_id = account.default_model_id
		and security.security_id = model_security.security_id
	left outer join model 
		on model.model_id = model_security.model_id
	where positions.account_id = @account_id
		and positions.security_id not in	(
												select distinct security_id 
												from model_security 
												where model_id = account.default_model_id
											)
	union
	select 
		round(coalesce(positions.quantity, 0)
				* position_type.security_sign, 22)			as quantity_booked,
		round(coalesce(positions.unit_cost_base, 0), 12)	as average_cost,
		round(coalesce(positions.cost, 0)
				* position_type.security_sign
				* security.pricing_factor 
				* security.principal_factor
				/ principal.exchange_rate, 12)				as cost,
		security.name_1										as description,
		price.latest										as price,
		getdate()												as run_date,
		model.name											as model_name,
		@account_id											as account_id,
		account.short_name									as short_name,
		account.name_1										as account_name,
		account.nav_date									as nav_date,
		round(coalesce(account.net_asset_value, 0), 2)		as net_asset_value,
		round(coalesce(account.total_asset_value, 0), 2)	as total_asset_value,
		@currency											as currency,
		'Model holdings and Model non-holdings'				as model_classification,
		security.symbol										as symbol,
		case
			when  security.major_asset_code <> 12 
				then
					round(positions.quantity 
							* position_type.security_sign
							* price.latest
							* security.pricing_factor 
							* security.principal_factor 
							/ coalesce(principal.exchange_rate, 1.0)
							+ positions.accrued_income 
							/ coalesce(income.exchange_rate, 1.0), 12)
			else round(coalesce(positions.present_market_value, 0) 
						/ coalesce(income.exchange_rate, 1.0), 2)
			end												as market_value,
 		case
			when model.model_type = 0
				then round((((coalesce(positions.quantity, 0)  
                              * position_type.security_sign 
                              * price.latest 
                              * security.pricing_factor 
                              * security.principal_factor) 
                              / principal.exchange_rate )
                        + ((coalesce(positions.accrued_income, 0) 
                              * position_type.security_sign )
                              / income.exchange_rate)) 
						/ @porfolio_market_value,2)
			else 	round(((((coalesce(positions.quantity, 0)  
								* position_type.security_sign 
								* price.latest 
								* security.pricing_factor 
								* security.principal_factor) 
								/ principal.exchange_rate )
						+ ((coalesce(positions.accrued_income, 0) 
								* position_type.security_sign )
								/ income.exchange_rate))
					/ (sector_target.target * @porfolio_market_value)), 2)	
			end												as percent_port,
		round(coalesce(model_security.target, 0),2)					as model_percent,
		@porfolio_market_value								as total_market_value
	from positions
	join account 
		on account.account_id = positions.account_id
	join security 
		on security.security_id = positions.security_id
	left outer join model_security 
		on model_security.model_id = account.default_model_id 
		and security.security_id = model_security.security_id
	left outer join model
		on model.model_id = model_security.model_id
	left outer join currency principal 
		on security.principal_currency_id = principal.security_id
	left outer join currency income    
		on security.income_currency_id      = income.security_id
	left outer join price 
		on price.security_id = positions.security_id
	left outer join position_type            
		on positions.position_type_code = position_type.position_type_code
	left outer join 
	(	select  model_security.security_id	as security_id,
				model_sector_target.target	as target
		from model
		join account
			on model.model_id = account.default_model_id
			and account.account_id = @account_id
		join model_security 
			on model.model_id = model_security.model_id
		join model_sector_target 
			on model.model_id = model_sector_target.model_id
		join hierarchy_sector_map 
			on hierarchy_sector_map.hierarchy_sector_id = model_sector_target.hierarchy_sector_id
			and hierarchy_sector_map.security_id = model_security.security_id
	) sector_target
		on security.security_id = sector_target.security_id
	where account.account_id = @account_id
		and positions.security_id in	(
											select distinct security_id 
											from model_security 
											where model_id = account.default_model_id
										) 	
	union
	select 
		0												as quantity_booked,
		0												as average_cost,
		0												as cost,
		security.name_1									as description,
		price.latest									as price,
		getdate()											as run_date,
		model.name										as model_name,
		@account_id										as account_id,
		account.short_name								as short_name,
		account.name_1									as account_name,
		account.nav_date								as nav_date,
		coalesce(account.net_asset_value, 0)			as net_asset_value,
		coalesce(account.total_asset_value, 0)			as total_asset_value,
		@currency										as currency,
		'Model holdings and Model non-holdings'			as model_classification,
		security.symbol									as symbol,
		0												as market_value,
 		0												as percent_port,
		coalesce(model_security.target, 0)				as model_percent,
		@porfolio_market_value							as total_market_value
	from account
	join model_security 
		on account.default_model_id = model_security.model_id
	join security 
		on security.security_id = model_security.security_id
	join currency 
		on security.principal_currency_id = currency.security_id
	left outer join price 
		on price.security_id = model_security.security_id
	left outer join model 
		on model.model_id = model_security.model_id
	join 
	(	select  model_security.security_id	as security_id,
				model_sector_target.target	as target
		from model
		join account
			on model.model_id = account.default_model_id
			and account.account_id = @account_id
		join model_security 
			on model.model_id = model_security.model_id
		join model_sector_target 
			on model.model_id = model_sector_target.model_id
		join hierarchy_sector_map 
			on hierarchy_sector_map.hierarchy_sector_id = model_sector_target.hierarchy_sector_id
			and hierarchy_sector_map.security_id = model_security.security_id
	) sector_target
		on security.security_id = sector_target.security_id
	where account.account_id = @account_id
		and model_security.security_id not in	(
													select distinct security_id 
													from positions
													where positions.account_id = @account_id
												)
	order by
		model_classification,
		symbol;
	select @cpe_rpx_account_compare = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cpe_rpx_account_compare'
	and sysobjects.type = 'P'
if @cpe_rpx_account_compare is not null
begin
	execute @ret_val = cpe_rpx_account_compare
		@account_id
	if (@ret_val != 0 and @ret_val < 60000)
	begin
		return @ret_val
	end
end
end


go
if @@error = 0 print 'PROCEDURE: rpx_account_compare created'
else print 'PROCEDURE: rpx_account_compare error on creation'
go
