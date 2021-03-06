  --select * from proposed_orders
if exists (select * from sysobjects where name = 'se_get_orders')
begin
	drop procedure se_get_orders
	print 'PROCEDURE: se_get_orders dropped'
end
go

create procedure [dbo].[se_get_orders]--se_get_orders 199
(     
 @account_id       numeric(10)
 
 )    
as 
declare @MINOrderID numeric(10)
declare @security_mv numeric(10) = 0
declare @security_id numeric(10)
declare @order_type varchar(40)
declare @FltGainLoss float
declare @FstGainLoss float
declare @price float

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
*
into #d
from
(
    select 
	account.account_id,
	proposed_orders.order_id,
	security.user_id_1 as Symbol1,
	proposed_orders.user_field_5 as basket,
	country.mnemonic as Country,
	'Proposed Orders' as 'Order Type',
	account.short_name as 'Account Name',
	security.symbol as 'Symbol',
	security.security_id as security_id,
	side.mnemonic as 'BS',
	side.side_code,
	proposed_orders.quantity as Quantity,
	Coalesce(trade_reason.mnemonic,'') as 'Trade Reason',
	proposed_orders.created_time as 'Trade Date',
		case proposed_orders.trade_approval
	     when 0 then 'Pending'
		 when 1 then 'Approved'
		 else
		 'Denied'
	end as Status,
	
	Cast(1.10000000 as float) as 'STGL',
	Cast(1.10000000 as float) as 'LTGL',
	Cast(1.10000000 as float) as 'Tgl',
	Cast(1.10000000 as float) as security_mv,
	price.latest as 'Latest Price'
	
	from proposed_orders
	join account on
	proposed_orders.account_id = account.account_id
	left join trade_reason on
	trade_reason.trade_reason_id = proposed_orders.trade_reason_id
	join side on
	side.side_code = proposed_orders.side_code
	join security on 
	security.security_id = proposed_orders.security_id
	join country on
	country.country_code = security.country_code
	join price on
	price.security_id = security.security_id
	where account.account_id in (select  account_id from #t_account )--= @account_id 

	union all
	select 
	account.account_id,
	orders.order_id,
	security.user_id_1 as Symbol1,
	orders.user_field_5 as basket,
	country.mnemonic as Country,
	'Orders' as 'Order Type',
	account.short_name as 'Account Name',
	security.symbol as 'Symbol',
	security.security_id as security_id,
	side.mnemonic as 'BS',
	side.side_code,
	orders.quantity As Quantity,
	Coalesce(trade_reason.mnemonic,'') as 'Trade Reason',
	orders.created_time as 'Trade Date',
	'Approved' as Status,
	Cast(1.10000000 as float) as 'STGL',
	Cast(1.10000000 as float) as 'LTGL',
	Cast(1.10000000 as float) as 'Tgl',
	Cast(1.10000000 as float) as security_mv,
	price.latest as 'Latest Price'
	from orders
	join account on
	orders.account_id = account.account_id
	left join trade_reason on
	trade_reason.trade_reason_id = orders.trade_reason_id
	join side on
	side.side_code = orders.side_code
	join security on 
	security.security_id = orders.security_id
	join price on
	price.security_id = security.security_id
	join country on
	country.country_code = security.country_code
	where account.account_id in (select  account_id from #t_account )--= @account_id 
	and orders.deleted = 0
	)C

	select @MINOrderID = min(#d.order_id)    
		from #d
  
while @MINOrderID is not null    
	begin  

	select @security_id = #d.security_id from #d where #d.order_id = @MINOrderID
	select @order_type = #d.[Order Type] from #d where #d.order_id = @MINOrderID
	select @price = #d.[Latest Price] from #d where #d.order_id = @MINOrderID
	

	if (@order_type = 'Proposed Orders')
	  begin
	
	   exec se_get_security_mv @security_mv = @security_mv output, @account_id  =@account_id, @security_id = @security_id,@market_value_type = 1

	   exec se_calculate_gain_loss_dashboard @FltGainLoss = @FltGainLoss output,@FstGainLoss = @FstGainLoss output,@price_previous = @price,@account_id= @account_id,
	    @security_id=@security_id,@order_id = @MINOrderID
	  end
	else

	begin
	
	   exec se_get_security_mv @security_mv = @security_mv output, @account_id  =@account_id, @security_id = @security_id,@market_value_type = 2
	  end

	
	update #d
	set #d.STGL = cast(@FstGainLoss as float),
    #d.LTGL = cast(@FltGainLoss as float),
    #d.Tgl = cast(coalesce(@FstGainLoss,0) + coalesce(@FltGainLoss,0)as float),
	#d.security_mv = @security_mv
    where #d.order_id = @MINOrderID


	 select @MINOrderID = min(#d.order_id)    
      from #d    
      where #d.order_id >@MINOrderID   
  end  

select * from #d

--se_get_orders 93

END

go
if @@error = 0 print 'PROCEDURE: se_get_orders created'
else print 'PROCEDURE: se_get_orders error on creation'
go