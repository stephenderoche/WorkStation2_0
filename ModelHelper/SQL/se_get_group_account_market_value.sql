if exists (select * from sysobjects where name = 'se_get_group_account_market_value')
begin
	drop procedure se_get_group_account_market_value
	print 'PROCEDURE: se_get_group_account_market_value dropped'
end
go

/*
declare @Account_group_market_value numeric(10)

exec se_get_group_account_market_value @Account_group_market_value = @Account_group_market_value output,@model_id =162905

print @Account_group_market_value



*/


create PROCEDURE [dbo].[se_get_group_account_market_value] 
(   
    @Account_group_market_value float output,
    @model_id  NUMERIC(10) = NULL
)	    
AS    
declare @MInAccountIDChild numeric(10)
declare @account_market_value numeric(10)
begin    

set nocount on
 
	 

	--insert into #child_accounts 
	--select
	--		        account.account_id
	--				from 
	--				account
	--				where default_model_id = @model_id
				   
					    

	  select @MInAccountIDChild = min(#child_accounts.account_id)      
  from #child_accounts while @MInAccountIDChild is not null  
				
				
begin

				  execute get_account_market_value   
				@market_value  = @account_market_value output,   
				@account_id   = @MInAccountIDChild,   
				@market_value_type = 7,   
				@currency_id  = 1    
				
				insert into #AMV values(@MInAccountIDChild,  @account_market_value)

			

 select @MInAccountIDChild = min(#child_accounts.account_id)      
   from #child_accounts     
   where #child_accounts.account_id >@MInAccountIDChild     
  end
 --select * from #AMV

 select @Account_group_market_value = sum(amv)  from #AMV

end	  
/* Display the status of the PROCEDURE creation */



go
if @@error = 0 print 'PROCEDURE: se_get_group_account_market_value created'
else print 'PROCEDURE: se_get_group_account_market_value error on creation'
go
