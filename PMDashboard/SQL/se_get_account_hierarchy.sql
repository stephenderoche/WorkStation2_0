if exists (select * from sysobjects where name = 'se_get_account_hierarchy')
begin
	drop procedure se_get_account_hierarchy
	print 'PROCEDURE: se_get_account_hierarchy dropped'
end
go

CREATE procedure [dbo].[se_get_account_hierarchy]--se_get_account_hierarchy 199
(       
 @account_id       numeric(10) = 1
 
 ) 
 as
 declare @hierarchy_id numeric(10) = 52
  declare @account_market_value float
 begin

 select @hierarchy_id = hierarchy_id from hierarchy where name = '.Default'

  execute get_account_market_value   
        @market_value  = @account_market_value output,   
        @account_id   = @account_id,   
        @market_value_type = 7,   
        @currency_id  = 1  

 	select *
	 into #temp
	    from
		(
		select
	    hierarchy_sector.description as ChildName,
		hierarchy_sector_map.security_id,
		hierarchy_sector.hierarchy_sector_id,
		hierarchy_sector.hierarchy_id,
		hierarchy_sector.parent_sector_id
	
		from hierarchy_sector_map 
	join positions on
	positions.security_id = hierarchy_sector_map.security_id
	join hierarchy_sector on
	hierarchy_sector.hierarchy_sector_id = hierarchy_sector_map.hierarchy_sector_id
	join security on
	security.security_id = positions.security_id
	where
		hierarchy_sector_map.hierarchy_id = @hierarchy_id	
		and positions.account_id = @account_id
		)s

	--select * from #temp

select 
hierarchy_sector.description as ParentName,
#temp.ChildName,#temp.hierarchy_id,
#temp.hierarchy_sector_id as child_sector_id,
#temp.parent_sector_id,
#temp.security_id,
security.symbol,
Coalesce(positions.quantity,0) as booked_quantity,
Coalesce(proposed_orders.quantity,0) as proposed_quantity,
Coalesce(orders.quantity,0) as order_quantity,
Coalesce(positions.quantity,0) +Coalesce(proposed_orders.quantity,0)+ Coalesce(orders.quantity,0) as total_quantity,
((Coalesce(positions.quantity,0) +Coalesce(proposed_orders.quantity,0)+ Coalesce(orders.quantity,0))
* coalesce(price.latest, 0.0) * security.principal_factor * pricing_factor / currency.exchange_rate    
       + positions.accrued_income /  currency.exchange_rate)
	     as security_market_value,
@account_market_value as account_market_value


from #temp
join hierarchy_sector on
#temp.parent_sector_id = hierarchy_sector.hierarchy_sector_id
join security on
security.security_id = #temp.security_id
join positions on
positions.security_id = #temp.security_id
left join proposed_orders on
proposed_orders.security_id = #temp.security_id
and proposed_orders.account_id = @account_id
join currency on
currency.security_id = security.principal_currency_id
join price on
price.security_id = security.security_id
left join orders on
orders.security_id = #temp.security_id
and orders.deleted = 0 and orders.account_id = @account_id
where positions.account_id = @account_id
 

end
go
if @@error = 0 print 'PROCEDURE: se_get_account_hierarchy created'
else print 'PROCEDURE: se_get_account_hierarchy error on creation'
go