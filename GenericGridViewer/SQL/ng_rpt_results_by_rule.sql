USE [NAV_753]
GO
/****** Object:  StoredProcedure [dbo].[ng_rpt_results_by_rule]    Script Date: 11/20/2018 10:53:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[ng_rpt_results_by_rule] --exec ng_rpt_results_by_rule  -1,1, 21, '2018-04-01', '2018-04-06', -1, 0, -1, -1  
(
	@account_id				numeric(10) 
	,@is_account smallint
	,@user_id int
	,@asof_time_start			datetime = null
	,@asof_time_end			datetime = null
	,@rule_id numeric(10) = -1
	,@fail_only smallint = 0
	,@nav_status_ok smallint = -1
	,@reviewed smallint = -1
)
as
begin

      set nocount on;

	create table #account (account_id numeric(10), short_name varchar(100))

	select @asof_time_end = @asof_time_start

	if @account_id = -1
		set @account_id = null

	insert into #account (account_id, short_name)

	select distinct a.account_id, a.short_name 
	from account_audit a
		inner join account_hierarchy_map m on m.child_id = a.account_id
	where m.parent_id = IsNull(@account_id, m.parent_id)
		and m.child_type = 3
		and a.is_current = 1
		and a.account_id in (select distinct account_id from position_audit where (convert(datetime, convert(nvarchar(10), asof_time, 112), 112) = convert(datetime, convert(nvarchar(10), @asof_time_start, 112), 112)))

	select distinct r.nav_control_type, p.cmpl_rule_id, cr.display_name as display_name, p.cmpl_profile_rule_id, rs.cmpl_profile_ruleset_id, rs.cmpl_profile_id, m.child_id as account_id, cr.class_name
	into #accounts_profile
	from cmpl_profile cp 
		inner join cmpl_account_profile ap on ap.cmpl_profile_id = cp.cmpl_profile_id 
		inner join account_hierarchy_map m on m.parent_id = ap.account_id
		inner join cmpl_profile_ruleset rs on rs.cmpl_profile_id = cp.cmpl_profile_id and rs.deleted = 0
		inner join cmpl_ruleset r on r.cmpl_ruleset_id = rs.cmpl_ruleset_id
		inner join cmpl_ruleset_rule rt on r.cmpl_ruleset_id = rs.cmpl_ruleset_id
		inner join cmpl_profile_rule p on p.cmpl_profile_ruleset_id = rs.cmpl_profile_ruleset_id and p.cmpl_ruleset_rule_id = rt.cmpl_ruleset_rule_id and p.deleted = 0 
		inner join cmpl_rule cr on cr.cmpl_rule_id = p.cmpl_rule_id and cr.deleted = 0
	where cp.deleted = 0 and m.child_type = 3
		and @rule_id = -1 or cr.cmpl_rule_id = @rule_id

	;with cte_nav_control_type_inv as (
		select n.account_id
			,a.short_name
			,n.nav_control_type_code 
			,MAX(n.latest_cmpl_invocation_id) as cmpl_invocation_id
		from nav_control_type t 
			inner join nav_account_process n on n.nav_control_type_code = t.nav_control_type_code
			inner join #account a on a.account_id = n.account_id
		where t.deleted = 0
			and n.deleted = 0
			and convert(datetime, convert(nvarchar(10), n.asof_time, 112), 112) = convert(datetime, convert(nvarchar(10), @asof_time_start, 112), 112) 
		group by n.account_id
			,n.nav_control_type_code 
			,a.short_name
	)
	select distinct c.account_id
			,c.short_name as account_name
			, c.cmpl_invocation_id
			,case when rv.nav_status_ok = 1 then 'NAV Ok' when rv.nav_status_ok = 0 then 'NAV Not Ok' else '' end as rule_process_status
			,a.display_name as rule_name 
			,case when c.cmpl_invocation_id is null then 'Not Run' when sr.nav_rule_status_code in (3,4) then 'Fail' when sr.nav_rule_status_code = 2 then 'Unknown' else 'Pass' end  as rule_status
			,IsNull(dq.name, '') as data_status
			,IsNull(td3.data, '') + ' ' + IsNull(td1.data, '') as review_comment_1
			,IsNull(td4.data, '') + ' ' + IsNull(td2.data, '') as review_comment_2
			,u1.name as review_by_1
			,u2.name as review_by_2
			,rv.nav_res_ruleset_review_id
			,n.nav_res_rule_result_id
			,n.nav_rule_status_code
			,case when sr.nav_rule_status_code in (3,4) then 1 when sr.nav_rule_status_code = 1 then 0 when sr.nav_rule_status_code = 2 then -1 else -2 end as pass_fail
			,@asof_time_start as asof_time
			,a.class_name
			,a.class_name + ' - ' + a.display_name as full_name
	into #matrix_data
	from cte_nav_control_type_inv c
		inner join #accounts_profile a on a.nav_control_type = c.nav_control_type_code and a.account_id = c.account_id
		left join nav_res_ruleset_review rv on rv.account_id = c.account_id and rv.cmpl_profile_ruleset_id = a.cmpl_profile_ruleset_id and rv.cmpl_invocation_id = c.cmpl_invocation_id
		left join nav_res_rule_result n on n.nav_res_ruleset_review_id = rv.nav_res_ruleset_review_id and n.cmpl_profile_rule_id = a.cmpl_profile_rule_id 
		left join nav_rule_status sr on sr.nav_rule_status_code = n.nav_rule_status_code 
		left join nav_data_quality_status dq on  n.nav_data_quality_status_code = dq.nav_data_quality_status_code
		left join cmpl_long_text_data td3 on td3.cmpl_long_text_id = n.first_rev_comment_text_id
		left join cmpl_long_text_data td4 on td4.cmpl_long_text_id = n.second_rev_comment_text_id
		left join cmpl_long_text_data td1 on td1.cmpl_long_text_id = rv.first_rev_comment_text_id
		left join cmpl_long_text_data td2 on td2.cmpl_long_text_id = rv.second_rev_comment_text_id
		left join user_info u1 on u1.user_id = rv.first_rev_by 
		left join user_info u2 on u2.user_id = rv.second_rev_by 
	where ((@fail_only = 1 and (n.nav_rule_status_code = 3 or  n.nav_rule_status_code = 4)) or (@fail_only = 0))
		and (@nav_status_ok = -1 or (@nav_status_ok in (0, 1) and rv.nav_status_ok = @nav_status_ok) or (@nav_status_ok = 2 and rv.nav_status_ok is null))
		and (@reviewed = -1 or (@reviewed = 0 and (IsNull(td3.data, '') = '' and IsNull(td1.data, '') = '')) or  (@reviewed = 1 and (IsNull(td3.data, '') <> '' or IsNull(td1.data, '') <> '')))

	select * 
	from #matrix_data  		
	order by class_name, account_name





end

