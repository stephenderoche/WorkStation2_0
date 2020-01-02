  
if exists (select * from sysobjects where name = 'se_get_parent_mutual_funds_orders_info')
begin
	drop procedure se_get_parent_mutual_funds_orders_info
	print 'PROCEDURE: se_get_parent_mutual_funds_orders_info dropped'
end
go

CREATE procedure [dbo].[se_get_parent_mutual_funds_orders_info]--se_get_parent_mutual_funds_orders_info 11577,1
(       
 @account_id       numeric(10) = 1,
 @include_orders tinyint = 1
 
 )      
as  
declare @total_funds numeric(10),
@internal_orders numeric(10),
@NAV float,
@min_account_id numeric(10),
@Reds float,
@subs Float,
@short_name varchar(40),
@cash_percent float,
@cash_amount float
begin 



create table #Holdings ( account_id numeric(10), short_name varchar(40),symbol varchar(40), internal_funds Varchar(40), subs_amount float, reds_amount float);
create table #t_account ( account_id numeric(10));
          
		  insert into #t_account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

                select account.account_id,
				short_name,
				security.symbol,
				order_id,
				price.latest,
				'Proposed' as order_type,
				case when security.user_id_15 = 'Y' then 'Y'
				else 'N'
				end as 'internal_funds',
				case when side_code in (0,1) then market_value 
				else 0
				end as 'subs_amount',
				case when side_code in (2,3) then (market_value * -1)
				else 0
				end as 'reds_amount'
			
				
				from
				proposed_orders
				join account on
				proposed_orders.account_id = account.account_id
				join security on
				proposed_orders.security_id = security.security_id
				join price on
				price.security_id = proposed_orders.security_id
				where proposed_orders.account_id in (select account_id from #t_account)
				union all
				select account.account_id,
				short_name,
				security.symbol,
				order_id,
				price.latest,
				'Ordered' as order_type,
				case when security.user_id_15 = 'Y' then 'Y'
				else 'N'
				end as 'internal_funds',
				case when side_code in (0,1) then market_value 
				else 0
				end as 'subs_amount',
				case when side_code in (2,3) then (market_value * -1)
				else 0
				end as 'reds_amount'
				from
				orders
				join account on
				orders.account_id = account.account_id
				join security on
				orders.security_id = security.security_id
				join price on
				price.security_id = orders.security_id
				where orders.account_id in (select account_id from #t_account)
				and orders.deleted = 0 and @include_orders = 1

				

end
go
if @@error = 0 print 'PROCEDURE: se_get_parent_mutual_funds_orders_info created'
else print 'PROCEDURE: se_get_parent_mutual_funds_orders_info error on creation'
go


--update se_drift_summary
--set sector_drift = 'N',
--security_drift = 'N'