if exists (select * from sysobjects where name = 'se_get_adv_rebal_op')
begin
	drop procedure se_get_adv_rebal_op
	print 'PROCEDURE: se_get_adv_rebal_op dropped'
end
go
create procedure [dbo].[se_get_adv_rebal_op] --se_get_adv_rebal_op 10349,16
(     

 @account_id				 numeric(10),
 @session_id                 numeric(10)

 )    
as 

begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
						declare @security_id				 numeric(10) = -1;
						

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




select 
proposed_orders.security_id,
symbol,
name_1,
count(rebal_session_id) as 'num_proposed_orders',
0 as 'num_orders',
sum(
case when side_code = 0 then
quantity else 0
end) as 'shares_bought',
sum(case when side_code = 0 then
1 else 0
end ) as 'number_buys',
sum(case when side_code = 2 then
quantity else 0
end) as 'shares_sold',

sum(case when side_code = 2 then
1 else 0
end) as 'number_sells'

from proposed_orders

join security on
security.security_id = proposed_orders.security_id
where rebal_session_id =@session_id
group by proposed_orders.security_id,
symbol,
name_1

------orders
union 
select 
orders.security_id,
symbol,
name_1,
0 as 'num_proposed_orders',
count(link_id) as 'num_orders',
sum(
case when side_code = 0 then
quantity else 0
end) as 'shares_bought'
,

sum(case when side_code = 0 then
1 else 0
end ) as 'number_buys',

sum(case when side_code = 2 then
quantity else 0
end) as 'shares_sold',

sum(case when side_code = 2 then
1 else 0
end) as 'number_sells'

from orders

join security on
security.security_id = orders.security_id
where link_id = @session_id and orders.deleted = 0
group by orders.security_id,
symbol,
name_1

--se_get_adv_rebal_op 10349,58

--exec se_rebal_sessions @session_id 

end

go
if @@error = 0 print 'PROCEDURE: se_get_adv_rebal_op created'
else print 'PROCEDURE: se_get_adv_rebal_op on creation'
go