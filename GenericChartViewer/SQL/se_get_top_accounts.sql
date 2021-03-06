if exists (select * from sysobjects where name = 'se_get_top_accounts')
begin
	drop procedure se_get_top_accounts
	print 'PROCEDURE: se_get_top_accounts dropped'
end
go


Create procedure [dbo].[se_get_top_accounts] --se_get_top_accounts 10287,10

(
	@account_id		numeric(10),
    @topx			int = 10
)

as
begin
  
 ----------------------------------------------------------Work for groups----------------------------------------------------------
   create table #account (account_id numeric(10) not null);
 insert into #account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

					

select 
 top(@topx)
  account.net_asset_value as MV,
  account.short_name as Account
  from Account where account.account_id  in (select account_id from #account)
  order by account.net_asset_value desc
  

END
go
if @@error = 0 print 'PROCEDURE: se_get_top_accounts created'
else print 'PROCEDURE: se_get_top_accounts error on creation'
go