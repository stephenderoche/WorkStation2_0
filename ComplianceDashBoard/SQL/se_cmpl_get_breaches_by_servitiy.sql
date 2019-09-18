if exists (select * from sysobjects where name = 'se_cmpl_get_breaches_by_servitiy')
begin
	drop procedure se_cmpl_get_breaches_by_servitiy
	print 'PROCEDURE: se_cmpl_get_breaches_by_servitiy dropped'
end
go

CREATE PROCEDURE se_cmpl_get_breaches_by_servitiy --se_cmpl_get_breaches_by_servitiy 2
@account_id                   numeric(10),
@user_id						  smallint = 189
AS
BEGIN

with allAccounts (account_id)
as
(
	select child_id from account_hierarchy_map where parent_id = @account_id  and child_id = @account_id 
	union all
	select child_id from account_hierarchy_map 
	inner join allAccounts aa on aa.account_id = parent_id where child_id <> parent_id
)

select cs.name as Name, 
count(distinct c.cmpl_case_id) as [Count] 
from cmpl_case c 
inner join cmpl_case_state cs on c.cmpl_case_state_id = cs.cmpl_case_state_id
inner join cmpl_case_invocation ci on c.cmpl_case_id = ci.cmpl_case_id
inner join cmpl_rule_status cr on cr.cmpl_rule_status_id = ci.calc_worst_error_level
inner join allAccounts a on a.account_id = c.compliance_account_id
where cr.name <> 'Pass' 
--and cr.name <> 'Warning' 
group by cs.name 

END