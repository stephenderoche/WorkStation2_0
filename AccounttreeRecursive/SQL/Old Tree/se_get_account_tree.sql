if exists (select * from sysobjects where name = 'se_get_account_tree')
begin
	drop procedure se_get_account_tree
	print 'PROCEDURE: se_get_account_tree dropped'
end
go


create procedure [dbo].[se_get_account_tree]--se_get_account_tree 10287
(	@account_id varchar(40)
) 
as
	declare @model_id numeric(10);
	declare @model_type tinyint;
	declare @hierarchy_id numeric(10)
	declare @hierarchy_sector_id numeric(10)
	declare @ID numeric(10);
	Declare @CoreModel_target float
	Declare @parent_short_name  varchar(40)
	
begin

   create table #parent_accounts (account_id numeric(10) not null,short_name varchar(40));
   create table #child_accounts (account_id numeric(10) not null,account_id1 numeric(10) not null,parent_account_id numeric(10) not null,parent_id numeric(10) not null,Parent_short_name varchar(40), Child_short_name varchar(40));
 insert into #parent_accounts  
		  select
			        account.account_id,
					short_name
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account_hierarchy_map.child_type= 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0
						and account.account_id <> @account_id

--select * from #parent_accounts




---- give sub models unique ids-------------------------------
select @ID = min(#parent_accounts.account_id)    
		from #parent_accounts
		declare @a numeric(10) = 0
while @ID is not null   
	begin  

	select @parent_short_name = #parent_accounts.short_name from #parent_accounts where #parent_accounts.account_id = @id

 insert into #child_accounts  
		  select
			        account.account_id,
					@a,
					@id,
					@a,
				
					@parent_short_name,
					short_name
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @id 
						and account_hierarchy_map.child_type = 3
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

						select @a = @a +1
 select 
	@ID = min(#parent_accounts.account_id)    
from #parent_accounts   
where #parent_accounts.account_id >@ID 

end  

---- give sub models unique ids-------------------------------
 
select * from #child_accounts

end

go
if @@error = 0 print 'PROCEDURE: se_get_account_tree created'
else print 'PROCEDURE: se_get_account_tree error on creation'
go