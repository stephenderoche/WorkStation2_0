if exists (select * from sysobjects where name = 'se_get_security_data')
begin
	drop procedure se_get_security_data
	print 'PROCEDURE: se_get_security_data dropped'
end
go


create procedure [dbo].[se_get_security_data] --se_get_security_data -1,-1,0,-1,0,-1
(
	@security_id numeric(10) = -1 ,
	@major_asset_code numeric(10) = -1,
	@IsZeroOrNull bit = 1,
	@percentChange float = 5,
	@justHoldings bit,
	@Stale numeric(10) = -1
) 
as
	declare @ret_val int;
	declare @security_lookup_id numeric(10);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;

if(@justHoldings = 0)
	begin
		select
		    security.security_id,
			round(price.latest,4) as 'Lastest Price' ,
			round(price.closing,4) as 'Previous Price',
			case 
			    when price.latest is null then 0
				when  price.latest = 0 then 0
				when  price.closing is  null then 0
				when  price.closing = 0 then  0
				else
				round((Coalesce(price.latest,.01)-Coalesce(price.closing,.01))/Coalesce(price.latest,.01),4)*100
			 end as '% Change',
			country.mnemonic as Country,
			major_asset.mnemonic as 'Major Asset',
			minor_asset.mnemonic 'Minor Asset',   
			currency.mnemonic as 'Currency',
			security.modified_time,
			security.symbol,
			security.country_code,
			security.major_asset_code,
			security.minor_asset_code,
			security.exchange_code,
			security.settlement_days_override,
			security.principal_currency_id,
			security_trader_map.trader_id,
			security.name_1,
			security.issuer_type_code,
			issuer_type.mnemonic as issuer_type_mnemonic,
			issuer_type.issuer_type_name as issuer_type_name,
			security.pricing_factor,
			security.principal_factor,
			security.debt_type_code,
			round(income.exchange_rate,4) as income_exchange_rate,
			round(principal.exchange_rate,4) as principal_exchange_rate,
			security.income_currency_id,
			security.principal_payment_type_code,
			security.income_payment_type_code,
			coalesce(security_analytics.hedge_ratio, 1.0) as security_analytics_hedge_ratio,
			security.security_level_code,
			bp_parameters.bp_significant_digits,
			bp_parameters.bp_sfig_yield,
			security.name_2,
			security.legacy_currency_id,
			security.settlement_currency_id,
			security.accrued_interest_day_count,
			security.annual_dividend_rate,
			security.payment_frequency,
			security.coupon,
			security.income_factor,
			security.maturity_date,
			security.next_coupon_date,
			security.next_reset_date,
			security.daily_trading_volume,
			security.average_trading_volume,
			security.user_id_1,
			security.user_id_2,
			security.user_id_3,
			security.user_id_4,
			security.user_id_5,
			security.user_id_6,
			security.user_id_7,
			security.user_id_8,
			security.user_id_9,
			security.user_id_10,
			security.user_id_11,
			security.user_id_12,
			security.user_id_13,
			security.user_id_14,
			security.user_id_15,
			security.user_id_16,
			security.user_field_1,
			security.user_field_2,
			security.user_field_3,
			security.user_field_4,
			security.user_field_5,
			security.user_field_6,
			security.user_field_7,
			security.user_field_8,
			security.user_field_9,
			security.user_field_10,
			security.user_field_11,
			security.user_field_12,
			security.user_field_13,
			security.user_field_14,
			security.user_field_15,
			security.user_field_16,
			security.price_yield_code as price_type,
			security.bond_type_code,
			security.convertible_flag,
			security.issuer_id,
			security.underlying_security_id,
			security.shares_outstanding,
			security.sic_code,
			security.adr_flag,
			security.illiquid_flag,
			security.section_144A_flag,
			security.votes_per_share,
			security.private_placement,
			security.securities_related_flag,
			security.state,
			security.cmpl_security_type_id,
			security.contract_size,
			security.contract_type_code,
			security.strike_price,
			security.lookthrough_model_id,
			security.issue_date,
			security.cash_settlement_flag,
			security.user_flag_1,
			security.user_flag_2,
			security.security_attribute,
			security.nav_related_index_id,
			security.pair_clip,
			bp_parameters.bp_subtype,
			security_analytics.australian_price,
			security_analytics.premium_percent,
			security.tba_umb_mortgage_type_code,
			tba_umb_mortgage_type.tba_umb_product_type_code,
			tba_umb_mortgage_type.tba_umb_agency_type_code,
			security.odd_first_coupon_date,
			security.repo_haircut,
			security.repo_investment_rate,
			security.repo_spread,
			security.repo_reference_index_id,
			security.repo_collateral_type_id,
			security.repo_type_code,
			security.repo_required_collateral_mv,
			security.lookthrough_depth_limit,
			swp_security.tenor,
			swp_security.tenor_type,
			security.security_audit_id,
			swp_security.funded_flag,
			security.fixing_date,
			--rating_1.rank as quality_rating_1,
			--rating_1.quality_rating as quality_rating_name_1,
			--rating_2.rank as quality_rating_2,
			--rating_2.quality_rating as quality_rating_name_2,
			--rating_3.rank as quality_rating_3,
			--rating_3.quality_rating as quality_rating_name_3,
			--rating_4.rank as quality_rating_4,
			--rating_4.quality_rating as quality_rating_name_4,
			--rating_5.rank as quality_rating_5,
			--rating_5.quality_rating as quality_rating_name_5,
			--rating_6.rank as quality_rating_6,
			--rating_6.quality_rating as quality_rating_name_6,
			--rating_7.rank as quality_rating_7,
			--rating_7.quality_rating as quality_rating_name_7,
			--rating_8.rank as quality_rating_8,
			--rating_8.quality_rating as quality_rating_name_8,
			--rating_9.rank as quality_rating_9,
			--rating_9.quality_rating as quality_rating_name_9,
			--rating_10.rank as quality_rating_10,
			--rating_10.quality_rating as quality_rating_name_10,
			--rating_11.rank as quality_rating_11,
			--rating_11.quality_rating as quality_rating_name_11,
			--rating_12.rank as quality_rating_12,
			--rating_12.quality_rating as quality_rating_name_12,
			--rating_13.rank as quality_rating_13,
			--rating_13.quality_rating as quality_rating_name_13,
			--rating_14.rank as quality_rating_14,
			--rating_14.quality_rating as quality_rating_name_14,
			--rating_15.rank as quality_rating_15,
			--rating_15.quality_rating as quality_rating_name_15,
			--rating_16.rank as quality_rating_16,
			--rating_16.quality_rating as quality_rating_name_16,
			--rating_17.rank as quality_rating_17,
			--rating_17.quality_rating as quality_rating_name_17,
			--rating_18.rank as quality_rating_18,
			--rating_18.quality_rating as quality_rating_name_18,
			--rating_19.rank as quality_rating_19,
			--rating_19.quality_rating as quality_rating_name_19,
			--rating_20.rank as quality_rating_20,
			--rating_20.quality_rating as quality_rating_name_20,
			security.payment_frequency_interval,
			security.payment_day_of_week,
			security.exponential_coupon_calc_flag,
			security.use_simple_interest_flag,
			security.inflation_linked_bond_flag,
			security.future_inflation_rate,
			security.unit_traded_bond_flag,
			security.unit_traded_bond_size,
			security.benchmark_security_id,
			security.otc_ccp_id
		from security
		left outer join security_trader_map 
			on security.security_id = security_trader_map.security_id
		join currency principal 
			on security.principal_currency_id = principal.security_id
		left outer join currency income 
			on security.income_currency_id = income.security_id
		left outer join security_analytics 
			on security.security_id = security_analytics.security_id
		left outer join bp_parameters 
			on security.security_id = bp_parameters.security_id
		left outer join tba_umb_mortgage_type 
			on tba_umb_mortgage_type.tba_umb_mortgage_type_code = security.tba_umb_mortgage_type_code
		left outer join issuer_type 
			on security.issuer_type_code = issuer_type.issuer_type_code
		left outer join swp_security 
			on security.security_id = swp_security.security_id
			and security.deleted = 0
		join Price on
		price.security_id = security.security_id
		join country on
		security.country_code = country.country_code
		join major_asset on
		major_asset.major_asset_code = security.major_asset_code
		join minor_asset on
		security.minor_asset_code = minor_asset.minor_asset_code
		join currency on
		security.principal_currency_id = currency.security_id
	
		where 
			(security.security_id = @security_id or @security_id = -1)
	        and (Security.major_asset_code = @major_asset_code or @major_asset_code = -1)
			and (price.latest = 0 or @IsZeroOrNull = 0)
			and (@percentChange = -1 or(
			 case 
			    when price.latest is null then 0
				when  price.latest = 0 then 0
				when  price.closing is  null then 0
				when  price.closing = 0 then  0
				else
				abs(round
				(
				(Coalesce(price.latest,.01)-Coalesce(price.closing,.01))
				/Coalesce(price.latest,.01),4)*100
				) 
			 end > @percentChange ) )
			 and (security.modified_time <= (Getdate() - @Stale) or @Stale = -1)
			 and security.major_asset_code not in (0,9,11,12)
end
else
begin
		select
		    distinct security.security_id,
			round(price.latest,4) as 'Lastest Price' ,
			round(price.closing,4) as 'Previous Price',
			case 
			    when price.latest is null then 0
				when  price.latest = 0 then 0
				when  price.closing is  null then 0
				when  price.closing = 0 then  0
				else
				round((Coalesce(price.latest,.01)-Coalesce(price.closing,.01))/Coalesce(price.latest,.01),4)*100
			 end as '% Change',
		    country.mnemonic as Country,
			major_asset.mnemonic as 'Major Asset',
			minor_asset.mnemonic 'Minor Asset', 
			currency.mnemonic as 'Currency',
			security.modified_time,
			security.symbol,
			security.country_code,
			security.major_asset_code,
			security.minor_asset_code,
			security.exchange_code,
			security.settlement_days_override,
			security.principal_currency_id,
			security_trader_map.trader_id,
			security.name_1,
			security.issuer_type_code,
			issuer_type.mnemonic as issuer_type_mnemonic,
			issuer_type.issuer_type_name as issuer_type_name,
			security.pricing_factor,
			security.principal_factor,
			security.debt_type_code,
			round(income.exchange_rate,4) as income_exchange_rate,
			round(principal.exchange_rate,4) as principal_exchange_rate,
			security.income_currency_id,
			security.principal_payment_type_code,
			security.income_payment_type_code,
			coalesce(security_analytics.hedge_ratio, 1.0) as security_analytics_hedge_ratio,
			security.security_level_code,
			bp_parameters.bp_significant_digits,
			bp_parameters.bp_sfig_yield,
			security.name_2,
			security.legacy_currency_id,
			security.settlement_currency_id,
			security.accrued_interest_day_count,
			security.annual_dividend_rate,
			security.payment_frequency,
			security.coupon,
			security.income_factor,
			security.maturity_date,
			security.next_coupon_date,
			security.next_reset_date,
			security.daily_trading_volume,
			security.average_trading_volume,
			security.user_id_1,
			security.user_id_2,
			security.user_id_3,
			security.user_id_4,
			security.user_id_5,
			security.user_id_6,
			security.user_id_7,
			security.user_id_8,
			security.user_id_9,
			security.user_id_10,
			security.user_id_11,
			security.user_id_12,
			security.user_id_13,
			security.user_id_14,
			security.user_id_15,
			security.user_id_16,
			security.user_field_1,
			security.user_field_2,
			security.user_field_3,
			security.user_field_4,
			security.user_field_5,
			security.user_field_6,
			security.user_field_7,
			security.user_field_8,
			security.user_field_9,
			security.user_field_10,
			security.user_field_11,
			security.user_field_12,
			security.user_field_13,
			security.user_field_14,
			security.user_field_15,
			security.user_field_16,
			security.price_yield_code as price_type,
			security.bond_type_code,
			security.convertible_flag,
			security.issuer_id,
			security.underlying_security_id,
			security.shares_outstanding,
			security.sic_code,
			security.adr_flag,
			security.illiquid_flag,
			security.section_144A_flag,
			security.votes_per_share,
			security.private_placement,
			security.securities_related_flag,
			security.state,
			security.cmpl_security_type_id,
			security.contract_size,
			security.contract_type_code,
			security.strike_price,
			security.lookthrough_model_id,
			security.issue_date,
			security.cash_settlement_flag,
			security.user_flag_1,
			security.user_flag_2,
			security.security_attribute,
			security.nav_related_index_id,
			security.pair_clip,
			bp_parameters.bp_subtype,
			security_analytics.australian_price,
			security_analytics.premium_percent,
			security.tba_umb_mortgage_type_code,
			tba_umb_mortgage_type.tba_umb_product_type_code,
			tba_umb_mortgage_type.tba_umb_agency_type_code,
			security.odd_first_coupon_date,
			security.repo_haircut,
			security.repo_investment_rate,
			security.repo_spread,
			security.repo_reference_index_id,
			security.repo_collateral_type_id,
			security.repo_type_code,
			security.repo_required_collateral_mv,
			security.lookthrough_depth_limit,
			swp_security.tenor,
			swp_security.tenor_type,
			security.security_audit_id,
			swp_security.funded_flag,
			security.fixing_date,
			--rating_1.rank as quality_rating_1,
			--rating_1.quality_rating as quality_rating_name_1,
			--rating_2.rank as quality_rating_2,
			--rating_2.quality_rating as quality_rating_name_2,
			--rating_3.rank as quality_rating_3,
			--rating_3.quality_rating as quality_rating_name_3,
			--rating_4.rank as quality_rating_4,
			--rating_4.quality_rating as quality_rating_name_4,
			--rating_5.rank as quality_rating_5,
			--rating_5.quality_rating as quality_rating_name_5,
			--rating_6.rank as quality_rating_6,
			--rating_6.quality_rating as quality_rating_name_6,
			--rating_7.rank as quality_rating_7,
			--rating_7.quality_rating as quality_rating_name_7,
			--rating_8.rank as quality_rating_8,
			--rating_8.quality_rating as quality_rating_name_8,
			--rating_9.rank as quality_rating_9,
			--rating_9.quality_rating as quality_rating_name_9,
			--rating_10.rank as quality_rating_10,
			--rating_10.quality_rating as quality_rating_name_10,
			--rating_11.rank as quality_rating_11,
			--rating_11.quality_rating as quality_rating_name_11,
			--rating_12.rank as quality_rating_12,
			--rating_12.quality_rating as quality_rating_name_12,
			--rating_13.rank as quality_rating_13,
			--rating_13.quality_rating as quality_rating_name_13,
			--rating_14.rank as quality_rating_14,
			--rating_14.quality_rating as quality_rating_name_14,
			--rating_15.rank as quality_rating_15,
			--rating_15.quality_rating as quality_rating_name_15,
			--rating_16.rank as quality_rating_16,
			--rating_16.quality_rating as quality_rating_name_16,
			--rating_17.rank as quality_rating_17,
			--rating_17.quality_rating as quality_rating_name_17,
			--rating_18.rank as quality_rating_18,
			--rating_18.quality_rating as quality_rating_name_18,
			--rating_19.rank as quality_rating_19,
			--rating_19.quality_rating as quality_rating_name_19,
			--rating_20.rank as quality_rating_20,
			--rating_20.quality_rating as quality_rating_name_20,
			security.payment_frequency_interval,
			security.payment_day_of_week,
			security.exponential_coupon_calc_flag,
			security.use_simple_interest_flag,
			security.inflation_linked_bond_flag,
			security.future_inflation_rate,
			security.unit_traded_bond_flag,
			security.unit_traded_bond_size,
			security.benchmark_security_id,
			security.otc_ccp_id
		from security
		left outer join security_trader_map 
			on security.security_id = security_trader_map.security_id
		join currency principal 
			on security.principal_currency_id = principal.security_id
		left outer join currency income 
			on security.income_currency_id = income.security_id
		left outer join security_analytics 
			on security.security_id = security_analytics.security_id
		left outer join bp_parameters 
			on security.security_id = bp_parameters.security_id
		left outer join tba_umb_mortgage_type 
			on tba_umb_mortgage_type.tba_umb_mortgage_type_code = security.tba_umb_mortgage_type_code
		left outer join issuer_type 
			on security.issuer_type_code = issuer_type.issuer_type_code
		left outer join swp_security 
			on security.security_id = swp_security.security_id
			and security.deleted = 0
		join Price on
		price.security_id = security.security_id
		join positions on 
		positions.security_id = security.security_id
		join country on
		security.country_code = country.country_code
		join major_asset on
		major_asset.major_asset_code = security.major_asset_code
		join minor_asset on
		security.minor_asset_code = minor_asset.minor_asset_code
	    join currency on
		security.principal_currency_id = currency.security_id
		where 
			(security.security_id = @security_id or @security_id = -1)
	        and (Security.major_asset_code = @major_asset_code or @major_asset_code = -1)
			and (price.latest = 0 or @IsZeroOrNull = 0)
			and (@percentChange = -1 or(
			 case 
			    when price.latest is null then 0
				when  price.latest = 0 then 0
				when  price.closing is  null then 0
				when  price.closing = 0 then  0
				else
				abs(round
				(
				(Coalesce(price.latest,.01)-Coalesce(price.closing,.01))
				/Coalesce(price.latest,.01),4)*100
				) 
			 end > @percentChange ) )
			 and (security.modified_time <= (Getdate() - @Stale) or @Stale = -1)
			 and security.major_asset_code not in (0,9,11,12)
end
			
end


go
if @@error = 0 print 'PROCEDURE: se_get_security_data created'
else print 'PROCEDURE: se_get_security_data error on creation'
go