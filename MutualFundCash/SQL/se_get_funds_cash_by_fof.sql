--select * from account

if exists (select * from sysobjects where name = 'se_get_funds_cash_by_fof')
begin
	drop procedure se_get_funds_cash_by_fof
	print 'PROCEDURE: se_get_funds_cash_by_fof dropped'
end
go

CREATE procedure [dbo].[se_get_funds_cash_by_fof]--se_get_funds_cash_by_fof 11575
(       
 @account_id      varchar(40),
 @include_orders numeric(10) =1
 
 )      
as  
begin 
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

					


				select 
				security.symbol,
				short_name,
				case when side_code in (0,1) then (market_value)
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
				and security.user_id_15 = 'Y'
				group by symbol,short_name,proposed_orders.side_code,proposed_orders.market_value


				--se_get_funds_cash_by_fof 11575
				union all
				select 
				security.symbol,
				short_name,
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
				and security.user_id_15 = 'Y'
				group by symbol,short_name,orders.side_code,orders.market_value

end
go
if @@error = 0 print 'PROCEDURE: se_get_funds_cash_by_fof created'
else print 'PROCEDURE: se_get_funds_cash_by_fof on creation'
go




--update se_drift_summary
--set sector_drift = 'N',
--security_drift = 'N'