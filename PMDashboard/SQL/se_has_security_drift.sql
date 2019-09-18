  
if exists (select * from sysobjects where name = 'se_has_security_drift')
begin
	drop procedure se_has_security_drift
	print 'PROCEDURE: se_has_security_drift dropped'
end
go




/*
declare @HD Varchar(40)
  execute se_has_security_drift 
  @has_drift = @HD output,
  @account_id = 199,
  @topx = 10
  
  print @HD

  */



ALTER procedure [dbo].[se_has_security_drift] --se_has_security_drift 20453,10
(
    @has_drift varchar(40) output ,
	@account_id		numeric(10),
    @topx			int = 30
)
as
    declare @market_value					numeric(10);
    declare @ret_val int;
	declare @acct_cd numeric(10)
	declare @include_proposed bit = 1
	declare @benchmark_model_id	numeric(10); 

 select @acct_cd = @account_id
 exec get_account_market_value @market_value = @market_value output,@account_id = @account_id,@market_value_type = 7,@currency_id = 1

 print @market_value
 ----------------------------------------------------------Work for groups----------------------------------------------------------
   create table #account (account_id numeric(10) not null);
   create table #holdings_percent  (  security_id numeric(10),Holdings_percent float);
   create table #benchmark_model  (  security_id numeric(10),benchmark_percent float);
 insert into #account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;


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



	insert into #benchmark_model
select 
 distinct top(30)
  security.security_id,
round(model_security.target * 100,2)
 as pct_acct_total 
from
			(
				select 

					positions.security_id,

					positions.quantity,

					position_type.security_sign,

					positions.accrued_income,

					positions.unit_cost   

				from #account

               join positions

               on #account.account_id = positions.account_id

               join position_type 

             on positions.position_type_code = position_type.position_type_code
			 	union all
				select
					proposed_orders.security_id,
					proposed_orders.quantity,
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
           where @include_proposed = 1
		   and account.account_id = @account_id
		  union all
				select
					orders.security_id,
					orders.quantity * side.security_sign,
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
          where @include_proposed = 1
		  and orders.deleted = 0
		  and account.account_id in (select account_id from #account)--= @account_id

          ) holdings

join security on
holdings.security_id = security.security_id
and security.major_asset_code <> 0

join price 
on price.security_id = security.security_id
		join currency principal  			
				on security.principal_currency_id = principal.security_id 
join model_security on
model_security.security_id = security.security_id
and model_security.model_id = @benchmark_model_id


order by security_id desc

--select * from #benchmark_model

	------holding_percent

					
insert into #holdings_percent
select 
 top(30)
 #benchmark_model.security_id,

round(
 sum 
 (
      (coalesce(holdings.quantity,0)  
			        * holdings.security_sign  					
					* price.latest  					
					* security.pricing_factor  					
					* security.principal_factor
	  )
					 / principal.exchange_rate 
	 )  
          
    / @market_value,4) * 100 as pct_acct_total 

from
			(
				select 

					positions.security_id,

					positions.quantity,

					position_type.security_sign,

					positions.accrued_income,

					positions.unit_cost   

				from #account

               join positions

               on #account.account_id = positions.account_id

               join position_type 

             on positions.position_type_code = position_type.position_type_code
			 	union all
				select
					proposed_orders.security_id,
					proposed_orders.quantity,
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
           where @include_proposed = 1
		   and account.account_id = @account_id
		  union all
				select
					orders.security_id,
					orders.quantity ,
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
          where @include_proposed = 1
		  and orders.deleted = 0
		  and account.account_id in (select account_id from #account)--= @account_id

          ) holdings

join #benchmark_model on
holdings.security_id = #benchmark_model.security_id
join security on
security.security_id = #benchmark_model.security_id


join price 
on price.security_id = #benchmark_model.security_id
		join currency principal  			
				on security.principal_currency_id = principal.security_id 

group by #benchmark_model.security_id
order by security_id desc

--select * from #holdings_percent


if (
	select
	count(drift.max_target)

		from
		(
		 select
		 Coalesce
		(case  --1st case
		   when
		     Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0) < 0 then
		    case  --2nd case
		        when (Coalesce(#benchmark_model.benchmark_percent,0)/100) between model_drift_tolerance.min_target and model_drift_tolerance.max_target
			       then 
				   case --3rd case
				       when ABS(((Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0))/100)) > abs(((Coalesce(#benchmark_model.benchmark_percent,0)/100)* model_drift_tolerance.pct_below))
					   then 1
					else
					      0
					end  --3rd case
				 when ABS(((Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0))/100)) > abs(((Coalesce(#benchmark_model.benchmark_percent,0)/100)* .1))
				then 1

		    else
		     0
		    end  --2nd case
			 when
		     Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0) > 0 then
		    case  --2nd case
		        when (Coalesce(#benchmark_model.benchmark_percent,0)/100) between model_drift_tolerance.min_target and model_drift_tolerance.max_target
			       then 
				   case --3rd case
				       when ABS(((Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0))/100)) > abs(((Coalesce(#benchmark_model.benchmark_percent,0)/100)* model_drift_tolerance.pct_above))
					   then 1
					else
					      0
					end  --3rd case
				 when ABS(((Coalesce(#holdings_percent.Holdings_percent,0) - Coalesce(#benchmark_model.benchmark_percent,0))/100)) > abs(((Coalesce(#benchmark_model.benchmark_percent,0)/100)* .1))
				then 1
		    else
		     0
		    end
		 end,4)  as max_target
		  
		from #holdings_percent 
		left join #benchmark_model on
		#benchmark_model.security_id = #holdings_percent.security_id
		left join model_drift_tolerance on
		model_drift_tolerance.model_id = @benchmark_model_id
		and (Coalesce(#benchmark_model.benchmark_percent,0)/100) between model_drift_tolerance.min_target and model_drift_tolerance.max_target
		
	
		
		)drift
		where drift.max_target = 1
		
		) > 1

		select  @has_drift = 'Y' --as has_sector_drift
		else
		select @has_drift = 'N' --as has_sector_drift

	


	

