if exists (select * from sysobjects where name = 'se_get_cash_with_output')
begin
	drop procedure se_get_cash_with_output
	print 'PROCEDURE: se_get_cash_with_output dropped'
end
go
create   procedure [dbo].[se_get_cash_with_output] --se_get_cash_with_output 20490
(   
 @account_market_value float output,
 @Cash_amount float output,
 @account_id numeric(10),
 @major_asset_code numeric(10) =0
 )
 
as  
 begin
 
 declare @currency_id numeric(10) 

 declare @Total_cash_offset numeric(10) = 0;
 declare @cash_offest float;
 declare @cash_Order_offest float;
 declare @result_cash float;

 select @currency_id = security_id from currency where mnemonic = 'USD'       
   

  execute get_account_market_value   
        @market_value  = @account_market_value output,   
        @account_id   = @account_id,   
        @market_value_type = 7,   
        @currency_id  = @currency_id  

	
        
        
 if @major_asset_code = 0
    begin 
    
    execute se_get_trade_date_cash 
     @trade_date_cash = @Total_cash_offset output,
     @account_id = @account_id,
     @major_asset_code = @major_asset_code
     print @Total_cash_offset
    end
       
--print @Total_cash_offset
 
---==========================================================================================================================================================

select 
@result_cash = 

--SELECT REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@total_mv),1), '.00','')as value
 Coalesce(round((sum(  
		  (  
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
			 end  
		   )  
		 
		  ) * coalesce(holdings.security_sign, 1.0) 
		  ) ) /1  ,2),0)
		  -- / @account_market_value  *100  ,2),0)
		
		
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
		union all
		select 
		  
		    orders.account_id,
			orders.security_id,
			orders.quantity,
			orders.accrued_income,
			side.security_sign
		from orders
		join side   
		 on orders.side_code = side.side_code 
		join security on
		security.security_id = orders.security_id
		where orders.account_id = @account_id
		and security.major_asset_code = @major_asset_code
		and orders.deleted = 0
		union all
		select 
		    
		    proposed_orders.account_id,
			proposed_orders.security_id,
			proposed_orders.quantity,
			proposed_orders.accrued_income,
			side.security_sign
		from proposed_orders
		join side   
		 on proposed_orders.side_code = side.side_code 
		 join security on
		security.security_id = proposed_orders.security_id
		
		where proposed_orders.account_id = @account_id
		and security.major_asset_code = @major_asset_code
	   
	   ) holdings
	   
join account
	on account.account_id = holdings.account_id
join security 
	on holdings.security_id = security.security_id

join price
    on price.security_id = security.security_id
join currency
    on currency.security_id = security.principal_currency_id   --select * from security

join currency income_currency   
		on security.income_currency_id = income_currency.security_id  
		
where account.account_id = @account_id
 and security.major_asset_code = @major_asset_code

 select @Cash_amount = Coalesce(@result_cash,0) + Coalesce(@Total_cash_offset,0)

 --SELECT  REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@result_cash),1), '.00','')

 return  @account_market_value
 return  Round(@Cash_amount,2)



      
end 

go
if @@error = 0 print 'PROCEDURE: se_get_cash_with_output created'
else print 'PROCEDURE: se_get_cash_with_output error on creation'
go 


