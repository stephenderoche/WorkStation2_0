  
if exists (select * from sysobjects where name = 'se_get_process_status')
begin
	drop procedure se_get_process_status
	print 'PROCEDURE: se_get_process_status dropped'
end
go

CREATE procedure [dbo].[se_get_process_status]--se_get_process_status 199,'02/01/2018'
(       
 @account_id       numeric(10),
 @date  Datetime
 
 )      
as  
begin 

create table #t_account ( account_id numeric(10));
          
		  insert into #t_account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

select 
se_process_status.account_id,
short_name as fund,
se_process_status.administrator as Administrator,
target_nav_time as 'Target Nav Time',
fund_initialization as 'Fund Initilazation',
capital_stock as 'Cap Stock',
corp_action as 'Corp Actions',
trades as 'Trades',
income as Income,
amortization as 'AmorAcc',
expenses as Expenses,
income_distribution as 'IncDist',
UnrealizedGL as 'UnRealized GL',
nav as NAV,
reviewed as Reviewed,
released as Released,
asof as Date
from
se_process_status
join account on
account.account_id = se_process_status.account_id
where se_process_status.account_id in (select account_id from #t_account)
and convert(datetime, convert(nvarchar(10), asof, 112), 112) between convert(datetime, convert(nvarchar(10), @date, 112), 112) and convert(datetime, convert(nvarchar(10), @date, 112), 112)
		
end
go
if @@error = 0 print 'PROCEDURE: se_get_process_status created'
else print 'PROCEDURE: se_get_process_status_info error on creation'
go
select * from se_process_status