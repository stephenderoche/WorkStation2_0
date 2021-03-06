  
if exists (select * from sysobjects where name = 'se_get_account_market_value_by_country')
begin
	drop procedure se_get_account_market_value_by_country
	print 'PROCEDURE: se_get_account_market_value_by_country dropped'
end
go

/*
declare @market_value		numeric(10)

exec se_get_account_market_value_by_country @market_value = @market_value output,@account_id = 20465.00,@currency_id = 1,@country_code = 69
print @market_value
*/
create procedure [dbo].[se_get_account_market_value_by_country] --se_get_account_market_value 20465.00,7,1
(	 
    @market_value		float output,
	@account_id 		numeric(10), 
	@currency_id		numeric(10) = 1,
	@country_code     numeric(10) = -1
) 
as
	declare @exchange_rate	float;
	declare @mv_allocated	float;
	declare @market_value1		float
	
	
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	select	@exchange_rate = exchange_rate
	from	currency
	where	security_id = @currency_id;
	select	@exchange_rate = coalesce(@exchange_rate, 1.0);

		select	@market_value1  = sum(case 
										when security.major_asset_code = 0
											then (positions.quantity 
													* position_type.security_sign
													* price.latest
													* security.pricing_factor 
													* security.principal_factor 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code in (1, 4, 9)
											then (positions.quantity 
													* position_type.security_sign
													* price.latest
													* security.pricing_factor 
													* security.principal_factor 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
													+ (positions.accrued_income 
													* position_type.security_sign
													/ coalesce(income_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code in (2, 3, 10)
											then (positions.quantity 
													* position_type.security_sign
													* price.latest
													* security.pricing_factor 
													* security.principal_factor 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
													+ (positions.accrued_income 
													* position_type.security_sign
													/ coalesce(income_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 5
											then (positions.quantity 
													* position_type.security_sign
													* (price.latest - coalesce(positions.last_mark, positions.unit_cost, 0.0))
													* security.pricing_factor 
													* security.principal_factor 
													* coalesce(security.contract_size, 1.0)
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 6
											and minor_asset_code = 88
											then 0.0
										when security.major_asset_code = 6
											then (positions.quantity 
													* position_type.security_sign
													* (price.latest - coalesce(positions.unit_cost, 0.0))
													* security.pricing_factor 
													* coalesce(security.contract_size, 1.0)
													/ coalesce(settlement_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 7               
											then (positions.quantity 
													* position_type.security_sign
													* price.latest
													* security.pricing_factor 
													* security.principal_factor 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 12      
											and minor_asset_code = 101
											and swp_security.funded_flag = 1
											then (positions.quantity 
													* position_type.security_sign
													* price.latest
													* security.pricing_factor 
													* security.principal_factor 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 12      
											and minor_asset_code = 101
											and swp_security.funded_flag = 0
											then (positions.quantity 
													* position_type.security_sign
													* price.latest
													* security.pricing_factor 
													* security.principal_factor 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
													+ (positions.accrued_income 
													* position_type.security_sign
													/ coalesce(income_currency.exchange_rate, 1.0)
													)
													- (positions.financing_cost
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 12
											then (positions.present_market_value
													* 1 
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
										when security.major_asset_code = 13
											then (positions.quantity 
													* position_type.security_sign
													* (price.latest - coalesce(positions.unit_cost, 0.0))
													* security.pricing_factor 
													* security.principal_factor
													* coalesce(security.contract_size, 1.0)
													/ coalesce(principal_currency.exchange_rate, 1.0)
													)
													+ (positions.accrued_income 
													* position_type.security_sign
													/ coalesce(income_currency.exchange_rate, 1.0)
													)
										else 0.0
										end
									)
		from account_hierarchy_map
		join positions
			on account_hierarchy_map.child_id = positions.account_id
		join security
			on positions.security_id = security.security_id
		left outer join swp_security
			on positions.security_id = swp_security.security_id
		join price
			on price.security_id = security.security_id
		left outer join currency principal_currency
			on security.principal_currency_id = principal_currency.security_id
		left outer join currency income_currency
			on security.income_currency_id = income_currency.security_id
		left outer join currency settlement_currency
			on security.settlement_currency_id = settlement_currency.security_id
		join position_type
			on positions.position_type_code = position_type.position_type_code
		where account_hierarchy_map.parent_id = @account_id
		and (security.country_code = @country_code
			or @country_code = -1)
		
		
	select @market_value  = coalesce(@market_value1 , 0.0);
	return;
end


go

if @@error = 0 print 'PROCEDURE: se_get_account_market_value_by_country created'
else print 'PROCEDURE: se_get_account_market_value_by_country error on creation'
go