  --select * from proposed_orders
if exists (select * from sysobjects where name = 'se_get_accounts_on_model')
begin
	drop procedure se_get_accounts_on_model
	print 'PROCEDURE: se_get_accounts_on_model dropped'
end
go



create procedure [dbo].[se_get_accounts_on_model]  --se_get_accounts_on_model 255348
(@model_id numeric(10)
)
     

as 
declare  @MInAccountID numeric(10)
declare @Cash_amount float,
        @account_market float,
		@account_market_value float,
		@cash_percent float,
		@se_get_drift_mv_sum numeric(10),
		@drift_percent float,
		@model_type numeric(10)

create table #accounts (account_id numeric(10) not null,short_name varchar(40),cash_percent float,account_market_value float,hasdrift varchar(10),drift_percent float, drift_mv float,cash_percent_shortfall float,Cash_mv_shortfall numeric(10));

select @model_type = model_type from model where model_id = @model_id

begin    

 insert into #accounts 
select
account.account_id,
account.short_name,
0,
0,
'N',
0,
0,
0,
0
from account
where account.default_model_id = @model_id


select @MInAccountID = min(#accounts.account_id)    
		from #accounts   
		
--begin while  
  
while @MInAccountID is not null    
	begin  
	

  execute se_get_cash_with_output   
        @account_market_value  = @account_market_value output,  
		@Cash_amount =  @Cash_amount output,
        @account_id   = @MInAccountID  

	declare @HD Varchar(40)

	if (@model_type = 0)

	begin
						execute se_has_security_drift 
						@has_drift = @HD output,
						@account_id = @MInAccountID,
						@topx = 10
	end
	else
	begin

		execute se_has_sector_drift 
						@has_drift = @HD output,
						@account_id = @MInAccountID

	end



exec se_get_drift_mv_sum  
@se_get_drift_mv_sum =  @se_get_drift_mv_sum output, 
@account_id  =@MInAccountID,
@model_id = @model_id, 
@account_market_value= @account_market_value

print @se_get_drift_mv_sum

if(@account_market_value = 0)
  begin 
  select @cash_percent = 0
  select @drift_percent = 0
  end
else
  begin
  select @cash_percent = (@Cash_amount/@account_market_value)
  select @drift_percent = (@se_get_drift_mv_sum/@account_market_value)
 
  end
  
      

update #accounts
set #accounts.account_market_value = @account_market_value,
#accounts.cash_percent = @cash_percent,
#accounts.hasdrift  = @HD,
#accounts.drift_percent = @drift_percent,
#accounts.drift_mv   =@se_get_drift_mv_sum
where #accounts.account_id = @MInAccountID

select @MInAccountID = min(#accounts.account_id)    
from #accounts    
where #accounts.account_id >@MInAccountID   
end  

select @model_id as model_id,
* from #accounts

exec se_get_model_info @model_id
end  


go
if @@error = 0 print 'PROCEDURE: se_get_accounts_on_model created'
else print 'PROCEDURE: se_get_accounts_on_model error on creation'
go