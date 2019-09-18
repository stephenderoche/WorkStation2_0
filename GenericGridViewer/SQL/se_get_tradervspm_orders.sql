
if exists (select * from sysobjects where name = 'se_get_tradervspm_orders')
begin
	drop procedure se_get_tradervspm_orders
	print 'PROCEDURE: se_get_tradervspm_orders dropped'
end
go


create procedure [dbo].[se_get_tradervspm_orders]--se_get_tradervspm_orders 28,-1,-1,-1,'07/01/2013','07/01/2018',-1
(     
 @desk_id                numeric(10),
 @account_id				 numeric(10) = -1,
 @security_id				 numeric(10) = -1,
 @issuer_id                numeric(10) = -1,
 @trade_date_start		 datetime = null,
 @trade_date_end		 datetime = null,
 @major_asset_code       numeric(10) =-1

 )    
as 

begin	


create table #t_desk ( desk_id numeric(10));

          insert into #t_desk  
		  select
			        desk.desk_id
					from desk_tree_map
					join desk on desk_tree_map.child_id = desk.desk_id
					where desk_tree_map.parent_id = @desk_id 
						and desk.deleted = 0

select
blocked_orders.block_id,
security.symbol,
coalesce(account.short_name, '*') as account,
desk.name as desk_name,
user_info.name,
side.mnemonic as Direction,
round(coalesce(coalesce(blocked_orders.quantity_executed,1)/NULLIF(blocked_orders.quantity_ordered,0),0)* 100,2) as percentdone,
blocked_orders.quantity_ordered,
blocked_orders.quantity_placed,
blocked_orders.quantity_executed,
coalesce(blocked_orders.quantity_ordered,0) - coalesce(blocked_orders.quantity_executed,0)as remaining,
round(blocked_orders.average_price_executed,2) as average_price_executed,
price.latest as arrival_price, 
blocked_orders.created_time,
blocked_orders.block_note as note
from blocked_orders
join security on
security.security_id = blocked_orders.security_id
left join account on 
account.account_id = blocked_orders.account_id
join desk on
desk.desk_id = blocked_orders.trader_id
join user_info on
user_info.user_id = blocked_orders.created_by
join price on
price.security_id = blocked_orders.security_id
join side on
side.side_code = blocked_orders.side_code
join orders on
blocked_orders.block_id = orders.block_id
where blocked_orders.trader_id in (select desk_id from #t_desk)
and blocked_orders.deleted = 0
and (blocked_orders.security_id = @security_id or  @security_id = -1)
and (security.issuer_id = @issuer_id or @issuer_id = -1) 
and (security.major_asset_code = @major_asset_code or @major_asset_code = -1) 
and (orders.account_id = @account_id or  @account_id = -1)
				

union

select
blocked_orders_history.block_id,
security.symbol,
coalesce(account.short_name, '*') as account,
desk.name,
user_info.name,
side.mnemonic as Direction,
round(coalesce(coalesce(blocked_orders_history.total_quantity_executed,1)/NULLIF(blocked_orders_history.quantity_ordered,0),0)* 100,2) as percentdone,
blocked_orders_history.quantity_ordered,
blocked_orders_history.quantity_placed,
blocked_orders_history.total_quantity_executed as quantity_executed,
coalesce(blocked_orders_history.quantity_ordered,0) - coalesce(blocked_orders_history.quantity_executed,0)as remaining,
round(blocked_orders_history.total_average_price_executed,2) as average_price_executed,
price.latest as arrival_price,
blocked_orders_history.created_time,
blocked_orders_history.block_note as note
from blocked_orders_history
join security on
security.security_id = blocked_orders_history.security_id
left join account on 
account.account_id = blocked_orders_history.account_id
join desk on
desk.desk_id = blocked_orders_history.trader_id
join user_info on
user_info.user_id = blocked_orders_history.created_by
join price on
price.security_id = blocked_orders_history.security_id
join side on
side.side_code = blocked_orders_history.side_code
join orders_history on
blocked_orders_history.block_id = orders_history.block_id
where blocked_orders_history.trader_id in (select desk_id from #t_desk)
and blocked_orders_history.deleted = 0
and blocked_orders_history.created_time between coalesce(@trade_date_start,blocked_orders_history.created_time) and
coalesce(@trade_date_end,blocked_orders_history.created_time)
and (blocked_orders_history.security_id = @security_id or  @security_id = -1)
				and (security.issuer_id = @issuer_id or @issuer_id = -1) 
				and (security.major_asset_code = @major_asset_code or @major_asset_code = -1) 
				and (orders_history.account_id = @account_id or  @account_id = -1)

END
go
if @@error = 0 print 'PROCEDURE: se_get_tradervspm_orders created'
else print 'PROCEDURE: se_get_tradervspm_orders error on creation'
go

--select 
--created_time,
--* from blocked_orders_history where trader_id = 28