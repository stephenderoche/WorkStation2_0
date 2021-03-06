if exists (select * from sysobjects where name = 'se_get_cash_flows')
begin
	drop procedure se_get_cash_flows
	print 'PROCEDURE: se_get_cash_flows dropped'
end
go

create procedure [dbo].[se_get_cash_flows]--exec se_get_cash_flows 10338,7790,'10/01/2018','11/30/2020'
(       
 @account_id       numeric(10) = 1,
 @security_id      numeric(10) = -1,
 @date_start		 datetime = null,
 @date_end		     datetime = null
 )      
as   
begin
  create table #account (account_id numeric(10) not null,market_value float);
 insert into #account  
		  select
			        account.account_id,
					0
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;


select 
symbol,
'' as Cash,
account.short_name,
positions.security_id,
((positions.quantity/100)* Coalesce(security.principal_factor,1)) * payment as cash_flow,
cf_schedule.cf_date,
case when payment = 100 then 'Maturity'
else 'Coupon'
end as payment_type,
case when security.minor_asset_code = 51 or security.minor_asset_code = 57 then
     ((positions.quantity/100)* Coalesce(security.principal_factor,1)) * payment_to_principal
else
    0
end as 'Principle',
case when security.minor_asset_code = 51 or security.minor_asset_code = 57 then
     ((positions.quantity/100)* Coalesce(security.principal_factor,1)) * payment_to_interest
else
    ((positions.quantity/100)* Coalesce(security.principal_factor,1)) * payment
end as 'Interest'
from cf_schedule
join account on
account.account_id in (select account_id from #account)

join security on
security.security_id = cf_schedule.security_id
join positions on
positions.security_id = cf_schedule.security_id
where positions.account_id in (select account_id from #account)
and (Security.security_id = @security_id or @security_id = -1)
and cf_schedule.cf_date between @date_start and @date_end

END

go
if @@error = 0 print 'PROCEDURE: se_get_cash_flows created'
else print 'PROCEDURE: se_get_cash_flows error on creation'
go

--select * from cf_schedule