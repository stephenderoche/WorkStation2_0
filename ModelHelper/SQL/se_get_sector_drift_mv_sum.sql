  
if exists (select * from sysobjects where name = 'se_get_sector_drift_mv_sum')
begin
	drop procedure se_get_sector_drift_mv_sum
	print 'PROCEDURE: se_get_sector_drift_mv_sum dropped'
end
go

/*
declare @has_drift varchar(40)
exec se_get_sector_drift_mv_sum 
@has_drift = @has_drift output,
@account_id = 10929

print @has_drift


*/


create procedure [dbo].[se_get_sector_drift_mv_sum]

(	
    @has_drift float output ,
	@account_id			numeric(10)
    
)
as
	declare @ret_val				int;
	declare @account_name			nvarchar(40);
	declare @account_short_name		nvarchar(40);
	declare @model_name				nvarchar(40); 
    declare @benchmark_model_id	numeric(10); 
    declare @total_asset			float;      
    declare @total_asset_cost		float;    
    declare @total_income			float;  
    declare @hierarchy_id			numeric(10);
    declare @sector_id				numeric(10);
    declare @sector_mv				float;
    declare @account_market_value	numeric(10);
    declare @run_date				datetime;
	declare @include_proposed	bit;
	declare @model_type numeric(10);

begin
                    set nocount on;
                    declare @ec__errno int;
                    declare @sp_initial_trancount int;
                    declare @sp_trancount int;

	-- create temple to hold the IDs of the requested accounts

	create table #account  	        (account_id numeric(10) not null);
	create table #holdings_percent  (  Asset_class varchar(40),Holdings_percent float);
	create table #benchmark_model  	(  Asset_class varchar(40),benchmark_percent float);
	create table #portfoli_model  	(  Asset_class varchar(40),benchmark_percent float);

	select 
		@hierarchy_id = default_hierarchy_id
	from account
	where account_id  = @account_id;

	--print @hierarchy_id

	/* populate #account with reqeuested accounts */
	 insert into #account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;
	

		execute get_account_market_value 
							    @market_value		= @account_market_value output, 
								@account_id			= @account_id, 
								@market_value_type	= 4, 
								@currency_id		= 1

								--print @account_market_value

	if(@account_market_value = 0)
	   begin

	   select @has_drift = 0
	   return @has_drift 

	   end
		--------------------------Get benchmark model-----------------------------------------------------

	select @benchmark_model_id =  default_benchmark_id from account where account_id = @account_id
	declare @model_id numeric(10)
	select @model_id = default_model_id from account where account_id = @account_id

	if (@model_id is null or @model_id < 0)
	   begin
	       select @has_drift = 'N'
		   return
	   end

	select @benchmark_model_id = coalesce(@benchmark_model_id,@model_id)


		--------------------------Get Holdings percent-----------------------------------------------------
		
		insert into #holdings_percent
		select 
			case
				when hierarchy_sector.description is not null then hierarchy_sector.description
				else 'Unknown'
				end as asset_class,
			--hierarchy_sector.description,
                round(detail.percent_mv, 4) 
				* 100 
				as percent_mv
		from 
		(
			select  
				case
					when hierarchy_sector.parent_sector_id is null then hierarchy_sector.hierarchy_sector_id
					when parent1.parent_sector_id is null then parent1.hierarchy_sector_id
                    when parent2.parent_sector_id is null then parent2.hierarchy_sector_id
					when parent3.parent_sector_id is null then parent3.hierarchy_sector_id
					when parent4.parent_sector_id is null then parent4.hierarchy_sector_id
					when parent5.parent_sector_id is null then parent5.hierarchy_sector_id
					when parent6.parent_sector_id is null then parent6.hierarchy_sector_id
					else null 
					end as hierarchy_sector_id,
				sum(
						(holdings.quantity
						 * price.latest
                          * security.pricing_factor
							 * security.principal_factor
						 / case when currency.exchange_rate is null then 1
							when currency.exchange_rate = 0 then 1
							else currency.exchange_rate
							end
							)
						+ 
						(holdings.accrued_income
						 / case when income_currency.exchange_rate is null then 1
								when income_currency.exchange_rate = 0 then 1
                                else income_currency.exchange_rate
								end
						)
						* coalesce(holdings.security_sign, 1.0)      
					) 
					
					/ @account_market_value 
					as percent_mv
					
			from
			(
				select 
					positions.security_id,
					positions.quantity,
					position_type.security_sign,
					positions.accrued_income,
					positions.unit_cost   
				from account
               join positions
               on account.account_id = positions.account_id
               join position_type 
             on positions.position_type_code = position_type.position_type_code
			 where account.account_id in (select account_id from #account)--= @account_id
				union all
				select
					proposed_orders.security_id,
					proposed_orders.quantity * side.security_sign ,
					side.security_sign,
					0 as accrued_income,
					price.latest as unit_cost
				from account
   			join proposed_orders
           on account.account_id = proposed_orders.account_id
           join side
           on proposed_orders.side_code = side.side_code
		   join price
           on proposed_orders.security_id = price.security_id
           where 
		   account.account_id = @account_id
		  union all
				select
					orders.security_id,
					orders.quantity * side.security_sign ,
					side.security_sign,
					0 as accrued_income,
					price.latest as unit_cost
				from account
   		   join orders
           on account.account_id = orders.account_id
		   and orders.deleted = 0
           join side
           on orders.side_code = side.side_code
		   join price
           on orders.security_id = price.security_id
          where 
		  orders.deleted = 0
		  and account.account_id in (select account_id from #account)--= @account_id
		  
          ) holdings
        join security
        on holdings.security_id = security.security_id
			join price 
		    on security.security_id  = price.security_id
			join currency 
			on security.principal_currency_id = currency.security_id
			join currency income_currency 
			on security.income_currency_id = income_currency.security_id
			left outer join hierarchy_sector_map
			on hierarchy_sector_map.hierarchy_id = @hierarchy_id
				and holdings.security_id = hierarchy_sector_map.security_id
			left outer join hierarchy_sector
			on hierarchy_sector_map.hierarchy_sector_id = hierarchy_sector.hierarchy_sector_id
			left outer join hierarchy_sector parent1
			on hierarchy_sector.parent_sector_id = parent1.hierarchy_sector_id
			left outer join hierarchy_sector parent2
			on parent1.parent_sector_id = parent2.hierarchy_sector_id
			left outer join hierarchy_sector parent3
			on parent2.parent_sector_id = parent3.hierarchy_sector_id
			left outer join hierarchy_sector parent4
			on parent3.parent_sector_id = parent4.hierarchy_sector_id
			left outer join hierarchy_sector parent5
				on parent4.parent_sector_id = parent5.hierarchy_sector_id
			left outer join hierarchy_sector parent6
				on parent5.parent_sector_id = parent6.hierarchy_sector_id
			left outer join security_analytics
				    on security_analytics.security_id = security.security_id
			group by case
						when hierarchy_sector.parent_sector_id is null then hierarchy_sector.hierarchy_sector_id
						when parent1.parent_sector_id is null then parent1.hierarchy_sector_id
						when parent2.parent_sector_id is null then parent2.hierarchy_sector_id
						when parent3.parent_sector_id is null then parent3.hierarchy_sector_id
						when parent4.parent_sector_id is null then parent4.hierarchy_sector_id
						when parent5.parent_sector_id is null then parent5.hierarchy_sector_id
						when parent6.parent_sector_id is null then parent6.hierarchy_sector_id
						else null
						end 
		) detail
		join account
		on account.account_id in (select account_id from #account)--= @account_id
		left outer join hierarchy_sector
		on detail.hierarchy_sector_id = hierarchy_sector.hierarchy_sector_id
	     and hierarchy_sector.cash_indicator = 0


		 delete from #holdings_percent where Asset_class = 'Unknown'

		--select * from #holdings_percent

		select @model_type = model_type from Model where model_id = @model_id

	if(@model_type = 0)
	begin
		--------------------------Get portfolio percent-----------------------------------------------------
		
		insert into  #benchmark_model
		select 
			case
				when hierarchy_sector.description is not null then hierarchy_sector.description
				else 'Unknown'
				end as asset_class,
                round(detail.percent_mv, 4) * 100 as percent_mv
		from 
		(
			select  
				case
					when hierarchy_sector.parent_sector_id is null then hierarchy_sector.hierarchy_sector_id
					when parent1.parent_sector_id is null then parent1.hierarchy_sector_id
                    when parent2.parent_sector_id is null then parent2.hierarchy_sector_id
					when parent3.parent_sector_id is null then parent3.hierarchy_sector_id
					when parent4.parent_sector_id is null then parent4.hierarchy_sector_id
					when parent5.parent_sector_id is null then parent5.hierarchy_sector_id
					when parent6.parent_sector_id is null then parent6.hierarchy_sector_id
					else null 
					end as hierarchy_sector_id,
				sum(holdings.target)
						as percent_mv
					
			from
			(
				select 
					model_security.target,
					model_security.security_id
				    from model_security
					where model_security.model_id = @benchmark_model_id
				
          ) holdings
        join security
        on holdings.security_id = security.security_id
			left outer join hierarchy_sector_map
			on hierarchy_sector_map.hierarchy_id = @hierarchy_id
				and holdings.security_id = hierarchy_sector_map.security_id
			left outer join hierarchy_sector
			on hierarchy_sector_map.hierarchy_sector_id = hierarchy_sector.hierarchy_sector_id
			left outer join hierarchy_sector parent1
			on hierarchy_sector.parent_sector_id = parent1.hierarchy_sector_id
			left outer join hierarchy_sector parent2
			on parent1.parent_sector_id = parent2.hierarchy_sector_id
			left outer join hierarchy_sector parent3
			on parent2.parent_sector_id = parent3.hierarchy_sector_id
			left outer join hierarchy_sector parent4
			on parent3.parent_sector_id = parent4.hierarchy_sector_id
			left outer join hierarchy_sector parent5
		    on parent4.parent_sector_id = parent5.hierarchy_sector_id
			left outer join hierarchy_sector parent6
		    on parent5.parent_sector_id = parent6.hierarchy_sector_id
		
			group by case
						when hierarchy_sector.parent_sector_id is null then hierarchy_sector.hierarchy_sector_id
						when parent1.parent_sector_id is null then parent1.hierarchy_sector_id
						when parent2.parent_sector_id is null then parent2.hierarchy_sector_id
						when parent3.parent_sector_id is null then parent3.hierarchy_sector_id
						when parent4.parent_sector_id is null then parent4.hierarchy_sector_id
						when parent5.parent_sector_id is null then parent5.hierarchy_sector_id
						when parent6.parent_sector_id is null then parent6.hierarchy_sector_id
						else null
						end 
		) detail
		join account
		on account.account_id in (select account_id from #account)--= @account_id
		left outer join hierarchy_sector
		on detail.hierarchy_sector_id = hierarchy_sector.hierarchy_sector_id
		and hierarchy_sector.cash_indicator = 0
	
	end
	else
	begin
		
		insert into #benchmark_model
		select description,
model_sector_target.target * 100 as benchmark_percent
from hierarchy_sector 
join model_sector_target on
model_sector_target.hierarchy_sector_id = hierarchy_sector.hierarchy_sector_id
and model_id = @benchmark_model_id
where depth = 0 and hierarchy_id = @hierarchy_id

end

select @has_drift =
     sum(
	      ((Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0))/100)) * @account_market_value
		  -- sum(Coalesce(#holdings_percent.Holdings_percent,0))
	     
	
from #holdings_percent 
		left join #benchmark_model on
		#benchmark_model.Asset_class = #holdings_percent.Asset_class
		
		and #holdings_percent.Asset_class <> 'Unknown'



end