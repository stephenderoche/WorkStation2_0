if exists (select * from sysobjects where name = 'se_get_top_securities_dasboard')
begin
	drop procedure se_get_top_securities_dasboard
	print 'PROCEDURE: se_get_top_securities_dasboard dropped'
end
go

create procedure [dbo].[se_get_top_securities_dasboard] --se_get_top_securities_dasboard 20419,10

(
	@account_id		numeric(10),
    @topx			int = 10
)

as
    declare @market_value					numeric(10);
    declare @ret_val int;
	declare @acct_cd numeric(10)
	declare @include_proposed bit = 1

 select @acct_cd = @account_id
 exec get_account_market_value @market_value = @market_value output,@account_id = @account_id,@market_value_type = 7,@currency_id = 1

 print @market_value
 ----------------------------------------------------------Work for groups----------------------------------------------------------
   create table #account (account_id numeric(10) not null);
 insert into #account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

					

select 
 top(@topx)
  symbol as Symbol,
  --security.security_id,
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
  
    ,2) as MV ,

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
          
    / @market_value,4) * 100 as PctTotal 

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
					orders.quantity,
					side.security_sign,
					0 as accrued_income,
					price.latest as unit_cost
				from account
   		   join orders
           on account.account_id = orders.account_id
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

join price 
on price.security_id = security.security_id
		join currency principal  			
				on security.principal_currency_id = principal.security_id 

group by symbol--,security.security_id
order by PctTotal desc




go
if @@error = 0 print 'PROCEDURE: se_get_top_securities_dasboard created'
else print 'PROCEDURE: se_get_top_securities_dasboard error on creation'
go