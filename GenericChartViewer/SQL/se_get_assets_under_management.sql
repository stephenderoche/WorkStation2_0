if exists (select * from sysobjects where name = 'se_get_assets_under_management')
begin
	drop procedure se_get_assets_under_management
	print 'PROCEDURE: se_get_assets_under_management dropped'
end
go



create procedure [dbo].[se_get_assets_under_management] --se_get_assets_under_management 10287,10

(
	@account_id		numeric(10),
    @topx			int = 10
)

as
declare @percent float
declare @myMv float
declare @TotalMV float
begin
  
 ----------------------------------------------------------Work for groups----------------------------------------------------------
   create table #account (account_id numeric(10) not null);
   create table #myassets (percentof_assets float,asset_type varchar(40) not null,market_value numeric(38));
;
 insert into #account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

					

insert
into 
#myassets
select
1,
'My Accounts (Prev Close)' as 'Accounts',
 sum(account.net_asset_value) as MyAccountsMV
  from Account where account.account_id  in (select account_id from #account)
 
insert
into 
#myassets
select
100,
'Firmwide AUM (Prev Close)' as 'Accounts',
 sum(account.net_asset_value) as MyAccountsMV
  from Account where account.deleted = 0

 update #myassets
set market_value =  660980659.00 
where asset_type = 'Firmwide AUM (Prev Close)'


select @myMv = market_value from #myassets where asset_type = 'My Accounts (Prev Close)'
select @TotalMV = market_value from #myassets where asset_type = 'Firmwide AUM (Prev Close)'
Select @percent = @myMv/@TotalMV

update #myassets
set percentof_assets = round(@percent * 100,2)
where asset_type = 'My Accounts (Prev Close)'



select * from #myassets


END


go
if @@error = 0 print 'PROCEDURE: se_get_assets_under_management created'
else print 'PROCEDURE: se_get_assets_under_management error on creation'
go