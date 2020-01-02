USE [LVTS_753]
GO
/****** Object:  StoredProcedure [dbo].[se_get_cash_percent]    Script Date: 1/29/2018 2:31:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	declare @TDC float
  execute se_get_cash_percent 
  @trade_date_cash = @TDC output,
  @account_id = 20471,
  @major_asset_code = 0
  
  print @TDC
  
*/

  
ALTER procedure [dbo].[se_get_cash_percent] 
(   
 @trade_date_cash float output ,
 @account_id numeric(10),
 @major_asset_code numeric(10)
 )
as  


 create table #account   (account_id numeric(10) not null   );
 
 declare @cash_offest float;
 declare @cash_Order_offest float;
 declare @Total_cash_offset float; 
 declare @include_proposed bit = 1;
 declare @account_name varchar(40);
 declare @currency_id numeric(10);
 declare @account_market_value numeric(10); 
 declare @Sumcash float;
begin

SET NOCOUNT Off 

  execute get_account_market_value   
        @market_value  = @account_market_value output,   
        @account_id   = @account_id,   
        @market_value_type = 7,   
        @currency_id  = @currency_id  

select @currency_id = security_id from currency where mnemonic = 'USD' 


  execute rpx_populate_t_account    
       @account_id  = @account_id,   
       @is_account_id = 1,   
       @user_id  = 198,   
       @account_name = @account_name output   

    --=============================================================================================================================================        
 ---SJD ADDED for cash offset  
   
 -- DBT select from actual accounts only (Not account groups as they have no positions/orders)  

 --================================================================ order cash offset============================================================
   select @cash_Order_offest =  
     SUM(proceeds)  
     from   
       (  
       select  
		   orders.security_id,  
		   orders.side_code,  
		   sum(orders.quantity) as quantity,  
		   sum(  
			   (      
			  orders.quantity   
			  * price.latest   
			  * side.market_value_sign  
			  * security.pricing_factor  
			  * security.principal_factor  
			  / case when currency.exchange_rate is null then 1  
				  when currency.exchange_rate = 0 then 1  
				  else currency.exchange_rate  
				  end  
			 )  
			 +  
			 (  
			  orders.accrued_income  
			   --=============================================================================================  
			  -- sjd added for interest fi  
			  * side.market_value_sign  
			  --=============================================================================================  
			  / case when income_currency.exchange_rate is null then 1  
				 when income_currency.exchange_rate = 0 then 1  
				 else income_currency.exchange_rate  
				 end  
			  )  + (orders.market_value * side.market_value_sign)
    ) as proceeds
  from #account  
  join orders  
   on #account.account_id = orders.account_id  
   and orders.deleted = 0
  join side  
   on orders.side_code = side.side_code  
  join security  
   on orders.security_id = security.security_id  
  left outer join security_analytics  
   on orders.security_id = security_analytics.security_id  
  join currency   
   on security.principal_currency_id = currency.security_id  
  left outer join currency income_currency   
   on security.income_currency_id = income_currency.security_id  
  join price  
   on orders.security_id = price.security_id  
  group by orders.security_id,  
     orders.side_code  
  )order_to_process  
 


  
 --================================================================ end order cash offset============================================================
 if @include_proposed = 1  
 begin   
    
  select @cash_offest =  
     SUM(proceeds)  
     from   
       (  
       select  
		   proposed_orders.security_id,  
		   proposed_orders.side_code,  
		   sum(coalesce(order_tax_lot.quantity, proposed_orders.quantity)) as quantity,  
		   sum(  
			   (      
			  coalesce(order_tax_lot.quantity, proposed_orders.quantity)   
			  * price.latest   
			  * side.market_value_sign  
			  * security.pricing_factor  
			  * security.principal_factor  
			  / case when currency.exchange_rate is null then 1  
				  when currency.exchange_rate = 0 then 1  
				  else currency.exchange_rate  
				  end  
			 )  
			 +  
			 (  
			  proposed_orders.accrued_income  
			   --=============================================================================================  
			  -- sjd added for interest fi  
			  * side.market_value_sign  
			  --=============================================================================================  
			  / case when income_currency.exchange_rate is null then 1  
				 when income_currency.exchange_rate = 0 then 1  
				 else income_currency.exchange_rate  
				 end  
			  )  + (proposed_orders.market_value * side.market_value_sign)
    ) as proceeds
  from #account  
  join proposed_orders  
   on #account.account_id = proposed_orders.account_id  
  join side  
   on proposed_orders.side_code = side.side_code  
  join security  
   on proposed_orders.security_id = security.security_id  
  left outer join security_analytics  
   on proposed_orders.security_id = security_analytics.security_id  
  join currency   
   on security.principal_currency_id = currency.security_id  
  left outer join currency income_currency   
   on security.income_currency_id = income_currency.security_id  
  join price  
   on proposed_orders.security_id = price.security_id  
  /* if tax lot relief is running, we will be getting our cost data from the tax lot */  
  left outer join order_tax_lot  
   on proposed_orders.order_id = order_tax_lot.order_id  
   and order_tax_lot.deleted = 0  
  left outer join tax_lot  
   on order_tax_lot.tax_lot_id = tax_lot.tax_lot_id  
  /* if tax lot relief is not running, we will be getting our cost data from the position */  
  left outer join positions  
   on proposed_orders.account_id = positions.account_id  
   and proposed_orders.security_id = positions.security_id  
     
  group by proposed_orders.security_id,  
     proposed_orders.side_code  
  )proposed_orders_to_process  
 end  
   
----------------booked cash
declare @booked_cash float
select @booked_cash =  
round(sum((    
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
    end )) * coalesce(holdings.security_sign, 1.0)),2)  
 from     
    (   
  select   
   positions.account_id,  
   positions.security_id,  
   positions.quantity,  
   positions.accrued_income,  
   position_type.security_sign  
   from positions  
  join position_type     
   on positions.position_type_code = position_type.position_type_code    
  where positions.account_id = @account_id  
    ) holdings  
join account  
 on account.account_id = holdings.account_id  
join security   
 on holdings.security_id = security.security_id  
join price  
    on price.security_id = security.security_id  
join currency  
    on currency.security_id = security.principal_currency_id     
join currency income_currency     
  on security.income_currency_id = income_currency.security_id    
where account.account_id = @account_id  
 and security.major_asset_code = 0 

  if @major_asset_code = 0 
    begin
       select @Sumcash = Coalesce(@booked_cash,0) + Coalesce(@cash_offest,0) + Coalesce(@cash_order_offest,0)
	  
	   if(coalesce(@Sumcash,0) = 0) 
	    begin
		  select @trade_date_cash = 0
		end
		else
		begin
	      --select @trade_date_cash = (@Sumcash/@account_market_value)
		select @trade_date_cash =round(
		((@Sumcash/@account_market_value)*100),2)
	   end
	  
    end
  
 
  
 -- sjd end cash offset  
 --============================================================================================================================================       

end

