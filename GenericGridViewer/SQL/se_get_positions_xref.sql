if exists (select * from sysobjects where name = 'se_get_positions_xref')
begin
	drop procedure se_get_positions_xref
	print 'PROCEDURE: se_get_positions_xref dropped'
end
go

create procedure [dbo].[se_get_positions_xref] --se_get_positions_xref 20462,20095
(     
      @account_id numeric(10),    
      @security_id numeric(10) 
	  
)
as
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
create table #acct 
 (
	account_id numeric(10) not null
 );

 insert into #acct 
	select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0

if (@security_id <>-1)
begin

select short_name
,Symbol
,quantity 
from account 
 join positions on
account.account_id = positions.account_id
left outer join security on
security.security_id = positions.security_id
where 
(positions.security_id = @security_id)
 and (account.account_id in (select account_id from #acct) or @account_id = -1)

  union
  
select 
distinct short_name
,Symbol
,0 as quantity 
from account 
 join positions on
account.account_id = positions.account_id
join security on
security.security_id = @security_id
where 
(positions.security_id not in ( @security_id))
--and (Coalesce(positions.quantity,0) !> 0)

 and (account.account_id in (select account_id from #acct) or @account_id = -1)
 and account.account_id not in (select account.account_id

from account 
 join positions on
account.account_id = positions.account_id
left outer join security on
security.security_id = positions.security_id
where 
(positions.security_id = @security_id)
 and (account.account_id in (select account_id from #acct) or @account_id = -1))
 order by quantity desc, short_name asc
	
end
end


go
if @@error = 0 print 'PROCEDURE: se_get_positions_xref created'
else print 'PROCEDURE: se_get_positions_xref error on creation'
go
