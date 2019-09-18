
declare @account_id numeric(10) = 98
declare @MInAccountID numeric(10); 
declare @MInSecurityID numeric(10); 
declare @payment_frequency numeric(10)
declare @ret_val int;



drop table #accounts
drop table #security
	

create table #accounts (account_id numeric(10) not null);
create table #security(security_id numeric(10) not null, payment_frequency numeric(10) not null)
		 
 
		 insert into #accounts  
		 select child_id
		 from account join account_hierarchy on account.account_id = account_hierarchy.child_id  
		 where account_hierarchy.child_type = 3  
		 and account_hierarchy.parent_id = @account_id 

		 insert into #security  
		 select positions.security_id,
		 security.payment_frequency 
		 from positions
		 join security on 
		 security.security_id = positions.security_id
		 where positions.account_id in (select account_id from #accounts)
		 and security.major_asset_code = 3
		
		 



select @MInAccountID = min(#accounts.account_id)    
		from #accounts   
--begin while  
  
while @MInAccountID is not null    
	begin   

---------------------------------------loop through securities


select @MInSecurityID = min(#accounts.account_id)    
		from #accounts   
--begin securities while  
  
while @MInSecurityID is not null    
	begin 
	
select @payment_frequency = payment_frequency from #security where security_id = @MInSecurityID 

exec calculate_cf_schedule @MInSecurityID,@payment_frequency


 select @MInSecurityID = min(#security.security_id)    
from #security   
where #security.security_id >@MInSecurityID   
end  

--------------------------------------end loop through securities


 select @MInAccountID = min(#accounts.account_id)    
from #accounts    
where #accounts.account_id >@MInAccountID   
end  

--delete from cf_schedule

--select * from cf_schedule
