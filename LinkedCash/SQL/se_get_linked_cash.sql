

if exists (select * from sysobjects where name = 'se_get_linked_cash')
begin
       drop procedure se_get_linked_cash
       print 'PROCEDURE: se_get_linked_cash dropped'
end
go

create procedure se_get_linked_cash  --se_get_linked_cash  20218

(  
	@desk_id numeric(10) = null

) 

as
begin


create table #desk (desk_id numeric(10) not null);
insert into #desk
         select child_id
		 from desk 
		 join desk_tree_map on 
		 desk.desk_id = desk_tree_map.child_id  
		 where desk_tree_map.child_type = 4 
		 and desk_tree_map.parent_id = @desk_id


select
 link_id,
 side.mnemonic,
 blocked_orders.security_id,
 security.symbol,
round(Sum(quantity_ordered),0) as Quantity_ordered,
coalesce(round(Sum(quantity_ordered * Coalesce(blocked_orders.user_field_1,price.latest) * side.market_value_sign),2),2) estimated_cash,
Coalesce(blocked_orders.user_field_1,price.latest) as estimated_price,
round(sum(quantity_executed)/Sum(quantity_ordered) * 100,2) as percent_done,
round(Sum(quantity_executed * average_price_executed * side.market_value_sign),2)  as actual_cash,
case 
    when sum(quantity_executed) = 0 then 0
	else
round(sum(quantity_executed*average_price_executed)/sum(quantity_executed),2)
end as 'avgerage executed price',
round(Sum(quantity_executed *average_price_executed* side.market_value_sign) + Sum((quantity_ordered -quantity_executed) * Coalesce(NULLIF(average_price_executed,0),coalesce(blocked_orders.user_field_1,price.latest)) * side.market_value_sign),2) ending_Estimated_cash,
price.latest as current_price,
round(((Sum(quantity_ordered *Coalesce(blocked_orders.user_field_1,price.latest)* side.market_value_sign))
 -((Sum(quantity_executed *average_price_executed* side.market_value_sign) + (Sum((quantity_ordered -quantity_executed) * Coalesce(NULLIF(average_price_executed,0),coalesce(blocked_orders.user_field_1,price.latest)) * side.market_value_sign)))))*-1,2) as estimated_cash_difference

from blocked_orders 
join security on
security.security_id= blocked_orders.security_id
join price on
blocked_orders.security_id = price.security_id
join side on
blocked_orders.side_code = side.side_code
 where trader_id in (select desk_id from #desk)
 and blocked_orders.deleted = 0
 group by link_id,security.symbol,side.mnemonic,blocked_orders.security_id,blocked_orders.user_field_1,price.latest
 --order by 1,2,3

--) as links

end





go

if @@error = 0 print 'PROCEDURE: se_get_linked_cash created'
else print 'PROCEDURE: se_get_linked_cash  error on creation'
go