  
if exists (select * from sysobjects where name = 'se_get_groups')
begin
	drop procedure se_get_groups
	print 'PROCEDURE: se_get_groups dropped'
end
go



create procedure [dbo].[se_get_groups]--se_get_groups

as 
 
  declare @MInAccountID numeric(10); 

  
begin	
  
		 create table #dynamic_accounts (account_id numeric(10) not null);
		

		 insert into #dynamic_accounts 
	     select account.account_id
				from se_account_groups
				
		

select @MInAccountID = min(#dynamic_accounts.account_id)    
		from #dynamic_accounts   
		
--begin while  
  
while @MInAccountID is not null    
	begin   
	


 

 
 select @MInAccountID = min(#dynamic_accounts.account_id)    
from #dynamic_accounts   
where #dynamic_accounts.account_id >@MInAccountID   
end  
	
END
go
if @@error = 0 print 'PROCEDURE: se_get_groups created'
else print 'PROCEDURE: se_get_groups error on creation'
go
