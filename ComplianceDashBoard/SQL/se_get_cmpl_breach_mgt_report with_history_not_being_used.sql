--get_cmpl_breach_details 22,1,2


if exists (select * from sysobjects where name = 'se_get_cmpl_breach_mgt_report')
begin
	drop procedure se_get_cmpl_breach_mgt_report
	print 'PROCEDURE: se_get_cmpl_breach_mgt_report dropped'
end
go

create procedure [dbo].[se_get_cmpl_breach_mgt_report] 
/*
se_get_cmpl_breach_mgt_report 
2, --account_id
1,  --show_closed_breaches
1,  --@show_sleeping_breaches
1,  --@show_reviewed_breaches
1,  --@show_nonreportable_breaches
1,  --@hide_expired_rule_breache
218, --@case_owner_id	
1,   --@show_noaction_breaches
1,   --@show_pass_breaches	
1,  --@show_approved_breaches	
null,  --@start_date	
null,  --@end_date
0,  --@show_asof_for_historical
null, --@asof_cmpl_invocation_id
0,  --@show_to_review_breaches
1,   --@show_to_approved_breaches
1, --@active_fails   
1	--@passive_fails   

*/
(
	@account_id						numeric(10),
	@show_closed_breaches			tinyint = 0,
	@show_sleeping_breaches			tinyint = 0,
	@show_reviewed_breaches			tinyint = 0,
	@show_nonreportable_breaches	tinyint = 0,
	@hide_expired_rule_breaches		tinyint = 0,
	@case_owner_id					numeric(10) = 218,
	@show_noaction_breaches			tinyint = 0,
	@show_pass_breaches				tinyint = 0,
	@show_approved_breaches			tinyint = 1,
	@start_date						datetime = null,
	@end_date						datetime = null,
	@show_asof_for_historical       tinyint = 0,
	@asof_cmpl_invocation_id        numeric(10) = null,
	@show_to_review_breaches	    tinyint = 0,
	@show_to_approved_breaches	    tinyint = 1,
	@active_fails    tinyint = 1,
	@passive_fails    tinyint = 1
)
as
	declare @start_date_local		datetime;
	declare @end_date_local			datetime;
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
if @start_date is null and @asof_cmpl_invocation_id is null
begin
	select
		case 
			when cmpl_case.compliance_account_id = @account_id then 0 
			else 1 
			end as is_child_account,
		cmpl_case.cmpl_case_id as case_id,
		cmpl_case.compliance_account_id as case_account_id,
		cmpl_case.cmpl_profile_rule_id as case_profile_rule_id,
		cmpl_case.cmpl_case_level_id as case_level_id,
		cmpl_case_level.name as case_level,
		cmpl_case.name as case_name,
		cmpl_case.cmpl_case_state_id as case_state_id,
		cmpl_case_state.name as case_state,
		Case
		    when coalesce(cmpl_case_activity.active, cmpl_case.active) = 1 then 'Yes'
			else
			'No'
		end as active,
		
		Case 
		  when cmpl_case.reviewed = 1 then 'Yes'
		  else 'No'
		end as reviewed,
		Case
		   when cmpl_case.reportable = 1 then 'Yes'
		   else 'No'
		end
	    as reportable,
		cmpl_case.active as is_active,
		cmpl_case.reviewed as is_reviewed,
		cmpl_case.reportable as is_reportable,
		cmpl_case.awaken_time as awaken_time,
		cmpl_case.num_case_invocations as num_case_invocations,
		cmpl_case.latest_cmpl_case_invocation_id as latest_cmpl_case_invocation_id,
		cmpl_case.calc_worst_error_level as worst_ever_error_level,
		coalesce(worst_restriction_sev.posttrade_description, 'Unknown') as worst_ever_error_level_desc,
		cmpl_case_invocation.calc_worst_error_level as current_error_level,
		coalesce(current_restriction_sev.posttrade_description, 'Unknown') as current_error_level_desc,
		cmpl_invocation.invoked_time as last_breach_time,
		case_account.account_level_code as case_account_level_code,
		case_account_level.description as case_account_level_desc,
		case_account.short_name as case_account_short_name,
		case_account.name_1 as case_account_name_1,
		case_account.name_2 as case_account_name_2,
		cmpl_profile.name as profile_name,
		cmpl_profile.description as profile_description,
		cmpl_profile.comments as profile_comment,
		cmpl_account_profile.account_id as profile_account_id,
		profile_account.account_level_code as profile_account_level_code,
		profile_account_level.description as profile_account_level_desc,
		profile_account.short_name as profile_account_short_name,
		profile_account.name_1 as profile_account_name_1,
		profile_account.name_2 as profile_account_name_2,
		cmpl_profile_type.name as profile_type,
		coalesce(cmpl_profile_rule.at_time_of_purchase, cmpl_ruleset_rule.at_time_of_purchase) as at_time_of_purchase,
		case cmpl_profile_rule.override_comments
			when 1 then cmpl_profile_rule.comments
			else cmpl_ruleset_rule.comments
			end as rule_assignment_comment,
		cmpl_profile_ruleset.cmpl_profile_ruleset_id as profile_ruleset_id,
		cmpl_ruleset.name as ruleset_name,
		cmpl_ruleset.description as ruleset_description,
		cmpl_ruleset.comments as ruleset_comment,
		cmpl_res_rule_result.display_name as rule_name,
		cmpl_res_rule_result.description as rule_description,
		cmpl_rule.comments as rule_comment,
		cmpl_case_level_override.pre_trade_override as has_pre_trade_override,
		--cmpl_case_user.user_id as case_owner_id,
		COALESCE(user_info.name, workgroup.name) as case_owner_name,
        cmpl_case_user.acknowledged as owner_acknowledged,
		case
			when (COALESCE(cmpl_res_rule_result.pf_rule_effective_end_date, getdate()) >= getdate() 
                    or (cmpl_case.cmpl_case_state_id <> 3
					    and cmpl_case.cmpl_case_state_id <> 2
                        )
                    ) 
                    then 0
			else 1
			end	as is_expired,
		cmpl_case.create_time as create_time,
		cmpl_case.breach_time as breach_time,
		cmpl_case.archive_reference as archive_reference,
		cmpl_case.suppress_noaction_recurrences,
		cmpl_case.closed_noaction,
		cmpl_case.approver as approver_id,
		approver_user_info.name as approver_name,
        cmpl_case.reviewer as reviewer_id,
        reviewer.name as reviewer_name,
		cmpl_case.created_by_internal_rule as created_by_internal_rule,
		cmpl_invocation.cmpl_invocation_id,
		cmpl_invocation.invoked_time,
		cmpl_case.created_by_disclosure_rule as created_by_disclosure_rule,
		cmpl_res_rule_result.nav_date as nav_date,
		cmpl_invocation.asof_time as asof_time,
		cmpl_invocation.uses_audit as uses_audit,
		cmpl_invocation.invocation_comment as invocation_comment,
		cmpl_invocation.invoked_by as invoked_by
	from
		account_hierarchy_map
		join cmpl_case 
			on account_hierarchy_map.child_id = cmpl_case.compliance_account_id
		join account case_account 
			on account_hierarchy_map.child_id = case_account.account_id
		join cmpl_case_level 
			on cmpl_case.cmpl_case_level_id = cmpl_case_level.cmpl_case_level_id
		join cmpl_case_state 
			on cmpl_case.cmpl_case_state_id = cmpl_case_state.cmpl_case_state_id
		join cmpl_case_invocation
			on cmpl_case.latest_cmpl_case_invocation_id = cmpl_case_invocation.cmpl_case_invocation_id
		join cmpl_res_rule_result
			on cmpl_case_invocation.cmpl_res_rule_result_id = cmpl_res_rule_result.cmpl_res_rule_result_id
		join cmpl_invocation 
			on cmpl_res_rule_result.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
		join cmpl_profile_rule 
			on cmpl_case.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
        join cmpl_profile_ruleset
        	on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
        join cmpl_profile 
        	on cmpl_profile_ruleset.cmpl_profile_id = cmpl_profile.cmpl_profile_id
        join cmpl_account_profile 
        	on cmpl_profile.cmpl_profile_id = cmpl_account_profile.cmpl_profile_id
		join account profile_account 
			on cmpl_account_profile.account_id = profile_account.account_id
        join cmpl_profile_type 
        	on cmpl_account_profile.cmpl_profile_type_id = cmpl_profile_type.cmpl_profile_type_id
        join cmpl_ruleset 
        	on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
    	join cmpl_ruleset_rule 
    		on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
    	join cmpl_rule 
    		on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
    	left outer join restriction_severity worst_restriction_sev
			on cmpl_case.calc_worst_error_level = worst_restriction_sev.error_level
		left outer join restriction_severity current_restriction_sev
			on cmpl_case_invocation.calc_worst_error_level = current_restriction_sev.error_level
		join account_level case_account_level
			on case_account.account_level_code = case_account_level.account_level_code
		join account_level profile_account_level
			on profile_account.account_level_code = profile_account_level.account_level_code
		left outer join cmpl_case_level_override
			on cmpl_case.cmpl_case_id = cmpl_case_level_override.cmpl_case_id
		left outer join cmpl_case_user
			on cmpl_case.cmpl_case_id = cmpl_case_user.cmpl_case_id 
		left outer join inbox_message
			on cmpl_case_user.current_inbox_message_id = inbox_message.message_id
		left outer join user_info
			on cmpl_case_user.user_id = user_info.user_id
		left outer join workgroup
			on cmpl_case_user.user_id = workgroup.workgroup_id
		left outer join user_info approver_user_info
			on cmpl_case.approver = approver_user_info.user_id
        left outer join user_info reviewer
            on cmpl_case.reviewer = reviewer.user_id
		left join cmpl_case_activity
			on cmpl_case_activity.cmpl_case_id = cmpl_case.cmpl_case_id
	where
		cmpl_invocation.asof_time is null and -- TODO no as-of runs in standard report, but we'll need a way to get them in asof reports
		account_hierarchy_map.parent_id = @account_id and
		(@show_pass_breaches = 1 or cmpl_case_invocation.calc_worst_error_level > 0 or cmpl_case.calc_worst_error_level > 0) and
		(coalesce(@show_closed_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 3) and
		(coalesce(@show_sleeping_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 1) and
		(coalesce(@show_noaction_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 4) and
		(coalesce(@show_to_review_breaches, 0) = 1 or cmpl_case.reviewed = 1 ) and
		(coalesce(@show_reviewed_breaches, 0) = 1 or cmpl_case.reviewed = 0 ) and
		
		(coalesce(@show_to_approved_breaches, 0) = 1 or cmpl_case.approver is not null) and
		(coalesce(@show_approved_breaches, 0) = 1 or cmpl_case.approver is  null) and
		(coalesce(@show_nonreportable_breaches, 0) = 1 or cmpl_case.reportable <> 0) and
		(
		coalesce(@hide_expired_rule_breaches, 0) = 0 
		or COALESCE(cmpl_res_rule_result.pf_rule_effective_end_date, getdate())
		 >= getdate() or
			(cmpl_case.cmpl_case_state_id <> 3 and cmpl_case.cmpl_case_state_id <> 2) 
		)and
		  (coalesce(@active_fails, 0) = 1 or cmpl_case.active = 0 ) 
		  and(coalesce(@passive_fails, 0) = 1 or cmpl_case.active = 1 )
		
end else begin
	select @start_date_local = convert(datetime, convert(nvarchar(10), @start_date, 112), 112);
	select @end_date_local = convert(datetime, convert(nvarchar(10), @end_date, 112), 112);
	select @end_date_local = dateadd(dd, 1, @end_date_local);
	select
		case 
			when cmpl_case.compliance_account_id = @account_id then 0 
			else 1 
			end as is_child_account,
		cmpl_case.cmpl_case_id as case_id,
		cmpl_case.compliance_account_id as case_account_id,
		cmpl_case.cmpl_profile_rule_id as case_profile_rule_id,
		cmpl_case.cmpl_case_level_id as case_level_id,
		cmpl_case_level.name as case_level,
		cmpl_case.name as case_name,
		cmpl_case.cmpl_case_state_id as case_state_id,
		cmpl_case_state.name as case_state,
		Case
		    when  cmpl_case.active = 1 then 'Yes'
			else
			'No'
		end as active,
		
		Case 
		  when cmpl_case.reviewed = 1 then 'Yes'
		  else 'No'
		end as reviewed,
		Case
		   when cmpl_case.reportable = 1 then 'Yes'
		   else 'No'
		end
	    as reportable,
		cmpl_case.active as is_active,
		cmpl_case.reviewed as is_reviewed,
		cmpl_case.reportable as is_reportable,
		cmpl_case.awaken_time as awaken_time,
		cmpl_case.num_case_invocations as num_case_invocations,
		cmpl_case.latest_cmpl_case_invocation_id as latest_cmpl_case_invocation_id,
		cmpl_case.calc_worst_error_level as worst_ever_error_level,
		coalesce(worst_restriction_sev.posttrade_description, 'Unknown') as worst_ever_error_level_desc,
		cmpl_case_invocation.calc_worst_error_level as current_error_level,
		coalesce(current_restriction_sev.posttrade_description, 'Unknown') as current_error_level_desc,
		cmpl_invocation.invoked_time as last_breach_time,
		case_account.account_level_code as case_account_level_code,
		case_account_level.description as case_account_level_desc,
		case_account.short_name as case_account_short_name,
		case_account.name_1 as case_account_name_1,
		case_account.name_2 as case_account_name_2,
		cmpl_profile.name as profile_name,
		cmpl_profile.description as profile_description,
		cmpl_profile.comments as profile_comment,
		cmpl_account_profile.account_id as profile_account_id,
		profile_account.account_level_code as profile_account_level_code,
		profile_account_level.description as profile_account_level_desc,
		profile_account.short_name as profile_account_short_name,
		profile_account.name_1 as profile_account_name_1,
		profile_account.name_2 as profile_account_name_2,
		cmpl_profile_type.name as profile_type,
		coalesce(cmpl_profile_rule.at_time_of_purchase, cmpl_ruleset_rule.at_time_of_purchase) as at_time_of_purchase,
		case cmpl_profile_rule.override_comments
			when 1 then cmpl_profile_rule.comments
			else cmpl_ruleset_rule.comments
			end as rule_assignment_comment,
		cmpl_profile_ruleset.cmpl_profile_ruleset_id as profile_ruleset_id,
		cmpl_ruleset.name as ruleset_name,
		cmpl_ruleset.description as ruleset_description,
		cmpl_ruleset.comments as ruleset_comment,
		cmpl_res_rule_result.display_name as rule_name,
		cmpl_res_rule_result.description as rule_description,
		cmpl_rule.comments as rule_comment,
		cmpl_case_level_override.pre_trade_override as has_pre_trade_override,
		cmpl_case_user.user_id as case_owner_id,
		COALESCE(user_info.name, workgroup.name) as case_owner_name,
        cmpl_case_user.acknowledged as owner_acknowledged,
		case
			when (COALESCE(cmpl_res_rule_result.pf_rule_effective_end_date, getdate()) >= getdate() 
					or (cmpl_case.cmpl_case_state_id <> 3
							and cmpl_case.cmpl_case_state_id <> 2
						)
					) then 0
			else 1
			end	as is_expired,
		cmpl_case.create_time as create_time,
		cmpl_case.breach_time as breach_time,
		cmpl_case.archive_reference as archive_reference,
		cmpl_case.suppress_noaction_recurrences,
		cmpl_case.closed_noaction,
		cmpl_case.approver as approver_id,
		approver_user_info.name as approver_name,
		case
			when cmpl_case.reviewed <> 0 then cmpl_case.reviewer
			else null 
			end as reviewer_id,
		case
			when cmpl_case.reviewed <> 0 then reviewer_info.name 
			else null
			end as reviewer_name,
		cmpl_case.created_by_internal_rule as created_by_internal_rule,
		cmpl_invocation.cmpl_invocation_id,
		cmpl_invocation.invoked_time,
		cmpl_case.created_by_disclosure_rule as created_by_disclosure_rule,
		cmpl_res_rule_result.nav_date as nav_date,
		cmpl_invocation.asof_time as asof_time,
		cmpl_invocation.uses_audit as uses_audit,
		cmpl_invocation.invocation_comment as invocation_comment,
		cmpl_invocation.invoked_by as invoked_by
	from 
	(
		select 
			account_hierarchy_map.child_id as account_id,
			cmpl_case.cmpl_case_id,
			cmpl_invocation.cmpl_invocation_id
		from account_hierarchy_map
		join cmpl_case
				on account_hierarchy_map.child_id = cmpl_case.compliance_account_id
			join cmpl_case_invocation
					on cmpl_case.cmpl_case_id = cmpl_case_invocation.cmpl_case_id
				join cmpl_res_rule_result
						on cmpl_case_invocation.cmpl_res_rule_result_id = cmpl_res_rule_result.cmpl_res_rule_result_id
					join cmpl_invocation
							on cmpl_res_rule_result.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
		left outer join cmpl_case_user
				on cmpl_case.cmpl_case_id = cmpl_case_user.cmpl_case_id 
		where account_hierarchy_map.parent_id = @account_id 
			and	(
					cmpl_invocation.cmpl_invocation_id = @asof_cmpl_invocation_id
					or
					(
						@asof_cmpl_invocation_id is null 
						and
						(
							(coalesce(cmpl_case.breach_time, @end_date_local) <= @end_date_local and @show_asof_for_historical <> 1 and cmpl_invocation.invoked_time >= @start_date_local and cmpl_invocation.invoked_time <= @end_date_local) 
							or
							(@show_asof_for_historical = 1 and cmpl_invocation.asof_time >= @start_date_local and cmpl_invocation.asof_time <= @end_date_local) 
				) 
					)
				)
			and	(@show_pass_breaches = 1 
				or cmpl_case_invocation.calc_worst_error_level > 0
				or cmpl_case.calc_worst_error_level > 0
				) 
			and	(coalesce(@show_closed_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 3
				) 
			and	(coalesce(@show_sleeping_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 1
				) 
			and	(coalesce(@show_noaction_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 4
				)
			and	(coalesce(@show_to_review_breaches, 0) = 1 or cmpl_case.reviewed = 1 )  
			and	(coalesce(@show_reviewed_breaches, 0) = 1 or cmpl_case.reviewed = 0 ) 
			and (coalesce(@show_to_approved_breaches, 0) = 1 or cmpl_case.approver is not null) 
		    and (coalesce(@show_approved_breaches, 0) = 1 or cmpl_case.approver is  null) 
			and	(coalesce(@show_nonreportable_breaches, 0) = 1 or cmpl_case.reportable <> 0) 
			and cmpl_case.active = 1
	    --  (coalesce(@active_fails, 0) = 1 or cmpl_case.active = 1 ) and
		   --(coalesce(@passive_fails, 0) = 1 or cmpl_case.active = 0 )
			--and	(@case_owner_id is null		
			--	or cmpl_case_user.user_id = @case_owner_id
			--	or cmpl_case_user.user_id in 
			--		(
			--			select workgroup.workgroup_id
			--			from user_access
			--			join workgroup 
			--				on user_access.object_id = workgroup.workgroup_id
			--				and workgroup.deleted = 0
			--			where user_access.user_id = @case_owner_id
			--		)
			--	)
	) cases
		join cmpl_case
				on cases.cmpl_case_id = cmpl_case.cmpl_case_id
			join cmpl_case_level 
					on cmpl_case.cmpl_case_level_id = cmpl_case_level.cmpl_case_level_id
			join cmpl_case_state 
					on cmpl_case.cmpl_case_state_id = cmpl_case_state.cmpl_case_state_id
		join cmpl_invocation
				on cases.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
			join cmpl_res_rule_result
					on cmpl_invocation.cmpl_invocation_id = cmpl_res_rule_result.cmpl_invocation_id
				join cmpl_case_invocation
						on cmpl_res_rule_result.cmpl_res_rule_result_id = cmpl_case_invocation.cmpl_res_rule_result_id
						and cmpl_case.cmpl_case_id = cmpl_case_invocation.cmpl_case_id
					left outer join restriction_severity current_restriction_sev
							on cmpl_case_invocation.calc_worst_error_level = current_restriction_sev.error_level
			join cmpl_profile_rule 
					on cmpl_case.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
				join cmpl_profile_ruleset
	        			on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
					join cmpl_profile 
	        				on cmpl_profile_ruleset.cmpl_profile_id = cmpl_profile.cmpl_profile_id
						join cmpl_account_profile 
	        					on cmpl_profile.cmpl_profile_id = cmpl_account_profile.cmpl_profile_id
							join account profile_account 
									on cmpl_account_profile.account_id = profile_account.account_id
								join cmpl_profile_type 
	        							on cmpl_account_profile.cmpl_profile_type_id = cmpl_profile_type.cmpl_profile_type_id
								join account_level profile_account_level
										on profile_account.account_level_code = profile_account_level.account_level_code
					join cmpl_ruleset 
	        				on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
    			join cmpl_ruleset_rule 
	    				on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
    				join cmpl_rule 
							on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
    		left outer join restriction_severity worst_restriction_sev
					on cmpl_case.calc_worst_error_level = worst_restriction_sev.error_level
			left outer join cmpl_case_level_override
					on cmpl_case.cmpl_case_id = cmpl_case_level_override.cmpl_case_id
			left outer join cmpl_case_user
					on cmpl_case.cmpl_case_id = cmpl_case_user.cmpl_case_id 
				left outer join inbox_message 
						on cmpl_case_user.current_inbox_message_id = inbox_message.message_id
				left outer join user_info
						on cmpl_case_user.user_id = user_info.user_id
				left outer join workgroup
						on cmpl_case_user.user_id = workgroup.workgroup_id
			left outer join user_info approver_user_info
					on cmpl_case.approver = approver_user_info.user_id
			left outer join user_info reviewer_info
					on cmpl_case.reviewer = reviewer_info.user_id
		join account case_account 
				on cases.account_id = case_account.account_id
			join account_level case_account_level
					on case_account.account_level_code = case_account_level.account_level_code
	where ( (@show_asof_for_historical = 0 and cmpl_invocation.asof_time is null) or (@show_asof_for_historical <> 0 and cmpl_invocation.asof_time is not null) )
union
select
		case 
			when cmpl_case_history.compliance_account_id = @account_id then 0 
			else 1 
			end as is_child_account,
		cmpl_case_history.cmpl_case_id as case_id,
		cmpl_case_history.compliance_account_id as case_account_id,
		cmpl_case_history.cmpl_profile_rule_id as case_profile_rule_id,
		cmpl_case_history.cmpl_case_level_id as case_level_id,
		cmpl_case_level.name as case_level,
		cmpl_case_history.name as case_name,
		cmpl_case_history.cmpl_case_state_id as case_state_id,
		cmpl_case_state.name as case_state,
			Case
		    when  cmpl_case_history.active = 1 then 'Yes'
			else
			'No'
		end as active,
		
		Case 
		  when cmpl_case_history.reviewed = 1 then 'Yes'
		  else 'No'
		end as reviewed,
		Case
		   when cmpl_case_history.reportable = 1 then 'Yes'
		   else 'No'
		end
	    as reportable,
		cmpl_case_history.active as is_active,
		cmpl_case_history.reviewed as is_reviewed,
		cmpl_case_history.reportable as is_reportable,
		cmpl_case_history.awaken_time as awaken_time,
		cmpl_case_history.num_case_invocations as num_case_invocations,
		cmpl_case_history.latest_cmpl_case_invocation_id as latest_cmpl_case_invocation_id,
		cmpl_case_history.calc_worst_error_level as worst_ever_error_level,
		coalesce(worst_restriction_sev.posttrade_description, 'Unknown') as worst_ever_error_level_desc,
		cmpl_case_invocation_history.calc_worst_error_level as current_error_level,
		coalesce(current_restriction_sev.posttrade_description, 'Unknown') as current_error_level_desc,
		cmpl_invocation.invoked_time as last_breach_time,
		case_account.account_level_code as case_account_level_code,
		case_account_level.description as case_account_level_desc,
		case_account.short_name as case_account_short_name,
		case_account.name_1 as case_account_name_1,
		case_account.name_2 as case_account_name_2,
		cmpl_profile.name as profile_name,
		cmpl_profile.description as profile_description,
		cmpl_profile.comments as profile_comment,
		cmpl_account_profile.account_id as profile_account_id,
		profile_account.account_level_code as profile_account_level_code,
		profile_account_level.description as profile_account_level_desc,
		profile_account.short_name as profile_account_short_name,
		profile_account.name_1 as profile_account_name_1,
		profile_account.name_2 as profile_account_name_2,
		cmpl_profile_type.name as profile_type,
		coalesce(cmpl_profile_rule.at_time_of_purchase, cmpl_ruleset_rule.at_time_of_purchase) as at_time_of_purchase,
		case cmpl_profile_rule.override_comments
			when 1 then cmpl_profile_rule.comments
			else cmpl_ruleset_rule.comments
			end as rule_assignment_comment,
		cmpl_profile_ruleset.cmpl_profile_ruleset_id as profile_ruleset_id,
		cmpl_ruleset.name as ruleset_name,
		cmpl_ruleset.description as ruleset_description,
		cmpl_ruleset.comments as ruleset_comment,
		cmpl_res_rule_result.display_name as rule_name,
		cmpl_res_rule_result.description as rule_description,
		cmpl_rule.comments as rule_comment,
		cmpl_case_level_override.pre_trade_override as has_pre_trade_override,
		cmpl_case_user.user_id as case_owner_id,
		COALESCE(user_info.name, workgroup.name) as case_owner_name,
        cmpl_case_user.acknowledged as owner_acknowledged,
		case
			when (COALESCE(cmpl_res_rule_result.pf_rule_effective_end_date, getdate()) >= getdate() 
					or (cmpl_case_history.cmpl_case_state_id <> 3
							and cmpl_case_history.cmpl_case_state_id <> 2
						)
					) then 0
			else 1
			end	as is_expired,
		cmpl_case_history.create_time as create_time,
		cmpl_case_history.breach_time as breach_time,
		cmpl_case_history.archive_reference as archive_reference,
		cmpl_case_history.suppress_noaction_recurrences,
		cmpl_case_history.closed_noaction,
		cmpl_case_history.approver as approver_id,
		approver_user_info.name as approver_name,
		case
			when cmpl_case_history.reviewed <> 0 then cmpl_case_history.reviewer
			else null 
			end as reviewer_id,
		case
			when cmpl_case_history.reviewed <> 0 then reviewer_info.name 
			else null
			end as reviewer_name,
		cmpl_case_history.created_by_internal_rule as created_by_internal_rule,
		cmpl_invocation.cmpl_invocation_id,
		cmpl_invocation.invoked_time,
		cmpl_case_history.created_by_disclosure_rule as created_by_disclosure_rule,
		cmpl_res_rule_result.nav_date as nav_date,
		cmpl_invocation.asof_time as asof_time,
		cmpl_invocation.uses_audit as uses_audit,
		cmpl_invocation.invocation_comment as invocation_comment,
		cmpl_invocation.invoked_by as invoked_by
	from 
	(
		select 
			account_hierarchy_map.child_id as account_id,
			cmpl_case_history.cmpl_case_id,
			cmpl_invocation.cmpl_invocation_id
		from account_hierarchy_map
		join cmpl_case_history
				on account_hierarchy_map.child_id = cmpl_case_history.compliance_account_id
			join cmpl_case_invocation_history
					on cmpl_case_history.cmpl_case_id = cmpl_case_invocation_history.cmpl_case_id
				join cmpl_res_rule_result
						on cmpl_case_invocation_history.cmpl_res_rule_result_id = cmpl_res_rule_result.cmpl_res_rule_result_id
					join cmpl_invocation
							on cmpl_res_rule_result.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
		left outer join cmpl_case_user
				on cmpl_case_history.cmpl_case_id = cmpl_case_user.cmpl_case_id 
		where account_hierarchy_map.parent_id = @account_id 
			and	(
					cmpl_invocation.cmpl_invocation_id = @asof_cmpl_invocation_id
					or
					(
						@asof_cmpl_invocation_id is null 
						and
						(
							(coalesce(cmpl_case_history.breach_time, @end_date_local) <= @end_date_local and @show_asof_for_historical <> 1 and cmpl_invocation.invoked_time >= @start_date_local and cmpl_invocation.invoked_time <= @end_date_local) 
							or
							(@show_asof_for_historical = 1 and cmpl_invocation.asof_time >= @start_date_local and cmpl_invocation.asof_time <= @end_date_local) 
						)
					)
				) 
			and	(@show_pass_breaches = 1 
				or cmpl_case_invocation_history.calc_worst_error_level > 0
				or cmpl_case_history.calc_worst_error_level > 0
				) 
			and	(coalesce(@show_closed_breaches, 0) = 1 or cmpl_case_history.cmpl_case_state_id <> 3
				) 
			and	(coalesce(@show_sleeping_breaches, 0) = 1 or cmpl_case_history.cmpl_case_state_id <> 1
				) 
			and	(coalesce(@show_noaction_breaches, 0) = 1 or cmpl_case_history.cmpl_case_state_id <> 4
				) 
		    and	(coalesce(@show_to_review_breaches, 0) = 1 or cmpl_case_history.reviewed = 0)
			and	(coalesce(@show_reviewed_breaches, 0) = 1 or cmpl_case_history.reviewed = 1) 
			and (coalesce(@show_to_approved_breaches, 0) = 1 or cmpl_case_history.approver is not null)
			and (coalesce(@show_approved_breaches, 0) = 1 or cmpl_case_history.approver is  null)
			and	(coalesce(@show_nonreportable_breaches, 0) = 1 or cmpl_case_history.reportable <> 0
				) and
			cmpl_case_history.active = 1
	    --  (coalesce(@active_fails, 0) = 1 or cmpl_case.active = 1 ) and
		   --(coalesce(@passive_fails, 0) = 1 or cmpl_case.active = 0 )
			--and	(@case_owner_id is null		
			--	or cmpl_case_user.user_id = @case_owner_id
			--	or cmpl_case_user.user_id in 
			--		(
			--			select workgroup.workgroup_id
			--			from user_access
			--			join workgroup 
			--				on user_access.object_id = workgroup.workgroup_id
			--				and workgroup.deleted = 0
			--			where user_access.user_id = @case_owner_id
			--		)
			--	)
	) cases
		join cmpl_case_history
				on cases.cmpl_case_id = cmpl_case_history.cmpl_case_id
			join cmpl_case_level 
					on cmpl_case_history.cmpl_case_level_id = cmpl_case_level.cmpl_case_level_id
			join cmpl_case_state 
					on cmpl_case_history.cmpl_case_state_id = cmpl_case_state.cmpl_case_state_id
		join cmpl_invocation
				on cases.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
			join cmpl_res_rule_result
					on cmpl_invocation.cmpl_invocation_id = cmpl_res_rule_result.cmpl_invocation_id
				join cmpl_case_invocation_history
						on cmpl_res_rule_result.cmpl_res_rule_result_id = cmpl_case_invocation_history.cmpl_res_rule_result_id
						and cmpl_case_history.cmpl_case_id = cmpl_case_invocation_history.cmpl_case_id
					left outer join restriction_severity current_restriction_sev
							on cmpl_case_invocation_history.calc_worst_error_level = current_restriction_sev.error_level
			join cmpl_profile_rule 
					on cmpl_case_history.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
				join cmpl_profile_ruleset
	        			on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
					join cmpl_profile 
	        				on cmpl_profile_ruleset.cmpl_profile_id = cmpl_profile.cmpl_profile_id
						join cmpl_account_profile 
	        					on cmpl_profile.cmpl_profile_id = cmpl_account_profile.cmpl_profile_id
							join account profile_account 
									on cmpl_account_profile.account_id = profile_account.account_id
								join cmpl_profile_type 
	        							on cmpl_account_profile.cmpl_profile_type_id = cmpl_profile_type.cmpl_profile_type_id
								join account_level profile_account_level
										on profile_account.account_level_code = profile_account_level.account_level_code
					join cmpl_ruleset 
	        				on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
    			join cmpl_ruleset_rule 
	    				on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
    				join cmpl_rule 
							on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
    		left outer join restriction_severity worst_restriction_sev
					on cmpl_case_history.calc_worst_error_level = worst_restriction_sev.error_level
			left outer join cmpl_case_level_override
					on cmpl_case_history.cmpl_case_id = cmpl_case_level_override.cmpl_case_id
			left outer join cmpl_case_user
					on cmpl_case_history.cmpl_case_id = cmpl_case_user.cmpl_case_id 
				left outer join inbox_message 
						on cmpl_case_user.current_inbox_message_id = inbox_message.message_id
				left outer join user_info
						on cmpl_case_user.user_id = user_info.user_id
				left outer join workgroup
						on cmpl_case_user.user_id = workgroup.workgroup_id
			left outer join user_info approver_user_info
					on cmpl_case_history.approver = approver_user_info.user_id
			left outer join user_info reviewer_info
					on cmpl_case_history.reviewer = reviewer_info.user_id
		join account case_account 
				on cases.account_id = case_account.account_id
			join account_level case_account_level
					on case_account.account_level_code = case_account_level.account_level_code
					where ( (@show_asof_for_historical = 0 and cmpl_invocation.asof_time is null) or (@show_asof_for_historical <> 0 and cmpl_invocation.asof_time is not null) )
union
select
			case 
				when cmpl_case.compliance_account_id = @account_id then 0 
				else 1 
			end 
		as is_child_account,
		cmpl_case.cmpl_case_id as case_id,
		cmpl_case.compliance_account_id as case_account_id,
		cmpl_case.cmpl_profile_rule_id as case_profile_rule_id,
		cmpl_case.cmpl_case_level_id as case_level_id,
		cmpl_case_level.name as case_level,
		cmpl_case.name as case_name,
		cmpl_case.cmpl_case_state_id as case_state_id,
		cmpl_case_state.name as case_state,
		
			Case
		    when coalesce(cmpl_case.active, cmpl_case.active) = 1 then 'Yes'
			else
			'No'
		end as active,
		
		Case 
		  when cmpl_case.reviewed = 1 then 'Yes'
		  else 'No'
		end as reviewed,
		Case
		   when cmpl_case.reportable = 1 then 'Yes'
		   else 'No'
		end
	    as reportable,
		cmpl_case.active as is_active,
		cmpl_case.reviewed as is_reviewed,
		cmpl_case.reportable as is_reportable,
		cmpl_case.awaken_time as awaken_time,
		cmpl_case.num_case_invocations as num_case_invocations,
		cmpl_case.latest_cmpl_case_invocation_id as latest_cmpl_case_invocation_id,
		cmpl_case.calc_worst_error_level as worst_ever_error_level,
		coalesce(worst_restriction_sev.posttrade_description, 'Unknown') as worst_ever_error_level_desc,
		cmpl_case_invocation.calc_worst_error_level as current_error_level,
		coalesce(current_restriction_sev.posttrade_description, 'Unknown') as current_error_level_desc,
		coalesce(cmpl_invocation_history.invoked_time, cmpl_invocation.invoked_time) as last_breach_time,
		case_account.account_level_code as case_account_level_code,
		case_account_level.description as case_account_level_desc,
		case_account.short_name as case_account_short_name,
		case_account.name_1 as case_account_name_1,
		case_account.name_2 as case_account_name_2,
		cmpl_profile.name as profile_name,
		cmpl_profile.description as profile_description,
		cmpl_profile.comments as profile_comment,
		cmpl_account_profile.account_id as profile_account_id,
		profile_account.account_level_code as profile_account_level_code,
		profile_account_level.description as profile_account_level_desc,
		profile_account.short_name as profile_account_short_name,
		profile_account.name_1 as profile_account_name_1,
		profile_account.name_2 as profile_account_name_2,
		cmpl_profile_type.name as profile_type,
		coalesce(cmpl_profile_rule.at_time_of_purchase, cmpl_ruleset_rule.at_time_of_purchase) as at_time_of_purchase,
		case cmpl_profile_rule.override_comments
			when 1 then cmpl_profile_rule.comments
			else cmpl_ruleset_rule.comments
			end as rule_assignment_comment,
		cmpl_profile_ruleset.cmpl_profile_ruleset_id as profile_ruleset_id,
		cmpl_ruleset.name as ruleset_name,
		cmpl_ruleset.description as ruleset_description,
		cmpl_ruleset.comments as ruleset_comment,
		coalesce(cmpl_res_rule_result_history.display_name, cmpl_res_rule_result_history.display_name) as rule_name,
		coalesce(cmpl_res_rule_result_history.description, cmpl_res_rule_result.description) as rule_description,
		cmpl_rule.comments as rule_comment,
		cmpl_case_level_override.pre_trade_override as has_pre_trade_override,
		cmpl_case_user_history.user_id as case_owner_id,
		coalesce(user_info.name, workgroup.name) as case_owner_name,
        cmpl_case_user_history.acknowledged as owner_acknowledged,
			case
				when (coalesce(cmpl_res_rule_result_history.pf_rule_effective_end_date, cmpl_res_rule_result.pf_rule_effective_end_date, getdate()) >= getdate() 
						or (cmpl_case.cmpl_case_state_id <> 3
							and cmpl_case.cmpl_case_state_id <> 2
							)
					) then 0
				else 1
			end as is_expired,
		cmpl_case.create_time as create_time,
		cmpl_case.breach_time as breach_time,
		cmpl_case.archive_reference as archive_reference,
		cmpl_case.suppress_noaction_recurrences,
		cmpl_case.closed_noaction,
		cmpl_case.approver as approver_id,
		approver_user_info.name as approver_name,
		case
			when cmpl_case.reviewed <> 0 then cmpl_case.reviewer
			else null 
			end as reviewer_id,
		case
			when cmpl_case.reviewed <> 0 then reviewer_info.name 
			else null
			end as reviewer_name,
		coalesce(cmpl_case.created_by_internal_rule, 0) as created_by_internal_rule,
		coalesce(cmpl_invocation_history.cmpl_invocation_id, cmpl_invocation.cmpl_invocation_id) as cmpl_invocation_id,
		coalesce(cmpl_invocation_history.invoked_time, cmpl_invocation.invoked_time) as invoked_time,
		coalesce(cmpl_case.created_by_disclosure_rule, 0) as created_by_disclosure_rule,
		coalesce(cmpl_res_rule_result_history.nav_date, cmpl_res_rule_result.nav_date) as nav_date,
		coalesce(cmpl_invocation_history.asof_time, cmpl_invocation.asof_time) as asof_time,
		coalesce(cmpl_invocation_history.uses_audit, cmpl_invocation.uses_audit) as uses_audit,
		coalesce(cmpl_invocation_history.invocation_comment, cmpl_invocation.invocation_comment) as invocation_comment,
		coalesce(cmpl_invocation_history.invoked_by, cmpl_invocation.invoked_by) as invoked_by
	from 
	(
		select 
			account_hierarchy_map.child_id as account_id,
			cmpl_case.cmpl_case_id,
			coalesce(cmpl_invocation.cmpl_invocation_id, cmpl_invocation_history.cmpl_invocation_id) as cmpl_invocation_id
		from account_hierarchy_map
		join cmpl_case
				on account_hierarchy_map.child_id = cmpl_case.compliance_account_id
			join cmpl_case_invocation
					on cmpl_case.cmpl_case_id = cmpl_case_invocation.cmpl_case_id
				left outer join cmpl_res_rule_result
						on cmpl_case_invocation.cmpl_res_rule_result_id = cmpl_res_rule_result.cmpl_res_rule_result_id
				left outer join cmpl_res_rule_result_history
						on cmpl_case_invocation.cmpl_res_rule_result_id = cmpl_res_rule_result_history.cmpl_res_rule_result_id
					left outer join cmpl_invocation
							on cmpl_res_rule_result_history.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
					left outer join cmpl_invocation_history
							on cmpl_res_rule_result_history.cmpl_invocation_id = cmpl_invocation_history.cmpl_invocation_id
		left outer join cmpl_case_user
				on cmpl_case.cmpl_case_id = cmpl_case_user.cmpl_case_id 
		where account_hierarchy_map.parent_id = @account_id 
			and	(
					cmpl_invocation.cmpl_invocation_id = @asof_cmpl_invocation_id
					or
					(
						@asof_cmpl_invocation_id is null 
						and
						(
							(coalesce(cmpl_case.breach_time, @end_date_local) <= @end_date_local and @show_asof_for_historical <> 1 and cmpl_invocation.invoked_time >= @start_date_local and cmpl_invocation.invoked_time <= @end_date_local) 
							or
							(@show_asof_for_historical = 1 and cmpl_invocation.asof_time >= @start_date_local and cmpl_invocation.asof_time <= @end_date_local) 
						)
					)
				) 
			and	(@show_pass_breaches = 1 
				or cmpl_case_invocation.calc_worst_error_level > 0
				or cmpl_case_invocation.calc_worst_error_level > 0
				) 
			and	(coalesce(@show_closed_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 3
				) 
			and	(coalesce(@show_sleeping_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 1
				) 
			and	(coalesce(@show_noaction_breaches, 0) = 1 or cmpl_case.cmpl_case_state_id <> 4
				) 
			and	(coalesce(@show_reviewed_breaches, 0) = 1 or cmpl_case.reviewed = 0
					or (coalesce(@show_approved_breaches, 0) = 1 and cmpl_case.approver is not null)
				) 
			and (coalesce(@show_approved_breaches, 0) = 1 or cmpl_case.approver is null
				)
			and	(coalesce(@show_nonreportable_breaches, 0) = 1 or cmpl_case.reportable <> 0
				) 
				and cmpl_case.active= 1
			--and	(@case_owner_id is null		
			--	or cmpl_case_user.user_id = @case_owner_id
			--	or cmpl_case_user.user_id in 
			--		(
			--			select workgroup.workgroup_id
			--			from user_access
			--			join workgroup 
			--				on user_access.object_id = workgroup.workgroup_id
			--				and workgroup.deleted = 0
			--			where user_access.user_id = @case_owner_id
			--		)
			--	)
	) cases
		join cmpl_case
				on cases.cmpl_case_id = cmpl_case.cmpl_case_id
			join cmpl_case_level 
					on cmpl_case.cmpl_case_level_id = cmpl_case_level.cmpl_case_level_id
			join cmpl_case_state 
					on cmpl_case.cmpl_case_state_id = cmpl_case_state.cmpl_case_state_id
		left outer join cmpl_invocation
				on cases.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
		left outer join cmpl_invocation_history
				on cases.cmpl_invocation_id = cmpl_invocation_history.cmpl_invocation_id
			left outer join cmpl_res_rule_result
					on cmpl_invocation.cmpl_invocation_id = cmpl_res_rule_result.cmpl_invocation_id
			left outer join cmpl_res_rule_result_history
					on cmpl_invocation_history.cmpl_invocation_id = cmpl_res_rule_result_history.cmpl_invocation_id
				join cmpl_case_invocation
						on coalesce(cmpl_res_rule_result.cmpl_res_rule_result_id, cmpl_res_rule_result_history.cmpl_res_rule_result_id) = cmpl_case_invocation.cmpl_res_rule_result_id
						and cmpl_case.cmpl_case_id = cmpl_case_invocation.cmpl_case_id
					left outer join restriction_severity current_restriction_sev
							on cmpl_case_invocation.calc_worst_error_level = current_restriction_sev.error_level
			join cmpl_profile_rule 
					on cmpl_case.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
				join cmpl_profile_ruleset
	        			on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
					join cmpl_profile 
	        				on cmpl_profile_ruleset.cmpl_profile_id = cmpl_profile.cmpl_profile_id
						join cmpl_account_profile 
	        					on cmpl_profile.cmpl_profile_id = cmpl_account_profile.cmpl_profile_id
							join account profile_account 
									on cmpl_account_profile.account_id = profile_account.account_id
								join cmpl_profile_type 
	        							on cmpl_account_profile.cmpl_profile_type_id = cmpl_profile_type.cmpl_profile_type_id
								join account_level profile_account_level
										on profile_account.account_level_code = profile_account_level.account_level_code
					join cmpl_ruleset 
	        				on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
    			join cmpl_ruleset_rule 
	    				on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
    				join cmpl_rule 
							on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
    		left outer join restriction_severity worst_restriction_sev
					on cmpl_case.calc_worst_error_level = worst_restriction_sev.error_level
			left outer join cmpl_case_level_override
					on cmpl_case.cmpl_case_id = cmpl_case_level_override.cmpl_case_id
			left outer join cmpl_case_user_history
					on cmpl_case.cmpl_case_id = cmpl_case_user_history.cmpl_case_id 
				left outer join inbox_message_history
						on cmpl_case_user_history.current_inbox_message_id = inbox_message_history.message_id
				left outer join user_info
						on cmpl_case_user_history.user_id = user_info.user_id
				left outer join workgroup
						on cmpl_case_user_history.user_id = workgroup.workgroup_id
			left outer join user_info approver_user_info
					on cmpl_case.approver = approver_user_info.user_id
			left outer join user_info reviewer_info
					on cmpl_case.reviewer = reviewer_info.user_id
		join account case_account 
				on cases.account_id = case_account.account_id
			join account_level case_account_level
					on case_account.account_level_code = case_account_level.account_level_code
					where ( (@show_asof_for_historical = 0 and cmpl_invocation.asof_time is null) or (@show_asof_for_historical <> 0 and cmpl_invocation.asof_time is not null) )
union 
	select
			case 
				when cmpl_case_history.compliance_account_id = @account_id then 0 
				else 1 
			end 
		as is_child_account,
		cmpl_case_history.cmpl_case_id as case_id,
		cmpl_case_history.compliance_account_id as case_account_id,
		cmpl_case_history.cmpl_profile_rule_id as case_profile_rule_id,
		cmpl_case_history.cmpl_case_level_id as case_level_id,
		cmpl_case_level.name as case_level,
		cmpl_case_history.name as case_name,
		cmpl_case_history.cmpl_case_state_id as case_state_id,
			Case
		    when  cmpl_case_history.active = 1 then 'Yes'
			else
			'No'
		end as active,
		
		Case 
		  when cmpl_case_history.reviewed = 1 then 'Yes'
		  else 'No'
		end as reviewed,
		Case
		   when cmpl_case_history.reportable = 1 then 'Yes'
		   else 'No'
		end
	    as reportable,
		cmpl_case_state.name as case_state,
		cmpl_case_history.active as is_active,
		cmpl_case_history.reviewed as is_reviewed,
		cmpl_case_history.reportable as is_reportable,
		cmpl_case_history.awaken_time as awaken_time,
		cmpl_case_history.num_case_invocations as num_case_invocations,
		cmpl_case_history.latest_cmpl_case_invocation_id as latest_cmpl_case_invocation_id,
		cmpl_case_history.calc_worst_error_level as worst_ever_error_level,
		coalesce(worst_restriction_sev.posttrade_description, 'Unknown') as worst_ever_error_level_desc,
		cmpl_case_invocation_history.calc_worst_error_level as current_error_level,
		coalesce(current_restriction_sev.posttrade_description, 'Unknown') as current_error_level_desc,
		coalesce(cmpl_invocation_history.invoked_time, cmpl_invocation.invoked_time) as last_breach_time,
		case_account.account_level_code as case_account_level_code,
		case_account_level.description as case_account_level_desc,
		case_account.short_name as case_account_short_name,
		case_account.name_1 as case_account_name_1,
		case_account.name_2 as case_account_name_2,
		cmpl_profile.name as profile_name,
		cmpl_profile.description as profile_description,
		cmpl_profile.comments as profile_comment,
		cmpl_account_profile.account_id as profile_account_id,
		profile_account.account_level_code as profile_account_level_code,
		profile_account_level.description as profile_account_level_desc,
		profile_account.short_name as profile_account_short_name,
		profile_account.name_1 as profile_account_name_1,
		profile_account.name_2 as profile_account_name_2,
		cmpl_profile_type.name as profile_type,
		coalesce(cmpl_profile_rule.at_time_of_purchase, cmpl_ruleset_rule.at_time_of_purchase) as at_time_of_purchase,
		case cmpl_profile_rule.override_comments
			when 1 then cmpl_profile_rule.comments
			else cmpl_ruleset_rule.comments
			end as rule_assignment_comment,
		cmpl_profile_ruleset.cmpl_profile_ruleset_id as profile_ruleset_id,
		cmpl_ruleset.name as ruleset_name,
		cmpl_ruleset.description as ruleset_description,
		cmpl_ruleset.comments as ruleset_comment,
		coalesce(cmpl_res_rule_result_history.display_name, cmpl_res_rule_result_history.display_name) as rule_name,
		coalesce(cmpl_res_rule_result_history.description, cmpl_res_rule_result.description) as rule_description,
		cmpl_rule.comments as rule_comment,
		cmpl_case_level_override.pre_trade_override as has_pre_trade_override,
		cmpl_case_user_history.user_id as case_owner_id,
		coalesce(user_info.name, workgroup.name) as case_owner_name,
        cmpl_case_user_history.acknowledged as owner_acknowledged,
			case
				when (coalesce(cmpl_res_rule_result_history.pf_rule_effective_end_date, cmpl_res_rule_result.pf_rule_effective_end_date, getdate()) >= getdate() 
						or (cmpl_case_history.cmpl_case_state_id <> 3
							and cmpl_case_history.cmpl_case_state_id <> 2
							)
					) then 0
				else 1
			end as is_expired,
		cmpl_case_history.create_time as create_time,
		cmpl_case_history.breach_time as breach_time,
		cmpl_case_history.archive_reference as archive_reference,
		cmpl_case_history.suppress_noaction_recurrences,
		cmpl_case_history.closed_noaction,
		cmpl_case_history.approver as approver_id,
		approver_user_info.name as approver_name,
		case
			when cmpl_case_history.reviewed <> 0 then cmpl_case_history.reviewer
			else null 
			end as reviewer_id,
		case
			when cmpl_case_history.reviewed <> 0 then reviewer_info.name 
			else null
			end as reviewer_name,
		coalesce(cmpl_case_history.created_by_internal_rule, 0) as created_by_internal_rule,
		coalesce(cmpl_invocation_history.cmpl_invocation_id, cmpl_invocation.cmpl_invocation_id) as cmpl_invocation_id,
		coalesce(cmpl_invocation_history.invoked_time, cmpl_invocation.invoked_time) as invoked_time,
		coalesce(cmpl_case_history.created_by_disclosure_rule, 0) as created_by_disclosure_rule,
		coalesce(cmpl_res_rule_result_history.nav_date, cmpl_res_rule_result.nav_date) as nav_date,
		coalesce(cmpl_invocation_history.asof_time, cmpl_invocation.asof_time) as asof_time,
		coalesce(cmpl_invocation_history.uses_audit, cmpl_invocation.uses_audit) as uses_audit,
		coalesce(cmpl_invocation_history.invocation_comment, cmpl_invocation.invocation_comment) as invocation_comment,
		coalesce(cmpl_invocation_history.invoked_by, cmpl_invocation.invoked_by) as invoked_by
	from 
	(
		select 
			account_hierarchy_map.child_id as account_id,
			cmpl_case_history.cmpl_case_id,
			coalesce(cmpl_invocation.cmpl_invocation_id, cmpl_invocation_history.cmpl_invocation_id) as cmpl_invocation_id
		from account_hierarchy_map
		join cmpl_case_history
				on account_hierarchy_map.child_id = cmpl_case_history.compliance_account_id
			join cmpl_case_invocation_history
					on cmpl_case_history.cmpl_case_id = cmpl_case_invocation_history.cmpl_case_id
				left outer join cmpl_res_rule_result
						on cmpl_case_invocation_history.cmpl_res_rule_result_id = cmpl_res_rule_result.cmpl_res_rule_result_id
				left outer join cmpl_res_rule_result_history
						on cmpl_case_invocation_history.cmpl_res_rule_result_id = cmpl_res_rule_result_history.cmpl_res_rule_result_id
					left outer join cmpl_invocation
							on coalesce(cmpl_res_rule_result_history.cmpl_invocation_id,cmpl_res_rule_result.cmpl_invocation_id) = cmpl_invocation.cmpl_invocation_id
					left outer join cmpl_invocation_history
							on coalesce(cmpl_res_rule_result_history.cmpl_invocation_id,cmpl_res_rule_result.cmpl_invocation_id) = cmpl_invocation_history.cmpl_invocation_id
		left outer join cmpl_case_user
				on cmpl_case_history.cmpl_case_id = cmpl_case_user.cmpl_case_id 
		where account_hierarchy_map.parent_id = @account_id 
			and (
					cmpl_invocation.cmpl_invocation_id = @asof_cmpl_invocation_id
					or
					(
						@asof_cmpl_invocation_id is null 
						and
						(
							(coalesce(cmpl_case_history.breach_time, @end_date_local) <= @end_date_local
							and @show_asof_for_historical <> 1
							and cmpl_invocation.invoked_time >= @start_date_local
							and cmpl_invocation.invoked_time <= @end_date_local) 
							or
							(@show_asof_for_historical = 1
							and cmpl_invocation.asof_time >= @start_date_local
							and cmpl_invocation.asof_time <= @end_date_local) 
						)
					)
				) 
			and	(@show_pass_breaches = 1 
				or cmpl_case_invocation_history.calc_worst_error_level > 0
				or cmpl_case_invocation_history.calc_worst_error_level > 0
				) 
			and	(coalesce(@show_closed_breaches, 0) = 1 or cmpl_case_history.cmpl_case_state_id <> 3
				) 
			and	(coalesce(@show_sleeping_breaches, 0) = 1 or cmpl_case_history.cmpl_case_state_id <> 1
				) 
			and	(coalesce(@show_noaction_breaches, 0) = 1 or cmpl_case_history.cmpl_case_state_id <> 4
				) 
			and	(coalesce(@show_reviewed_breaches, 0) = 1 or cmpl_case_history.reviewed = 0
					or (coalesce(@show_approved_breaches, 0) = 1 and cmpl_case_history.approver is not null)
				) 
			and (coalesce(@show_approved_breaches, 0) = 1 or cmpl_case_history.approver is null
				)
			and	(coalesce(@show_nonreportable_breaches, 0) = 1 or cmpl_case_history.reportable <> 0
			--	) 
			--and	(@case_owner_id is null		
			--	or cmpl_case_user.user_id = @case_owner_id
			--	or cmpl_case_user.user_id in 
			--		(
			--			select workgroup.workgroup_id
			--			from user_access
			--			join workgroup 
			--				on user_access.object_id = workgroup.workgroup_id
			--				and workgroup.deleted = 0
			--			where user_access.user_id = @case_owner_id
			--		)
			)
	) cases
		join cmpl_case_history
				on cases.cmpl_case_id = cmpl_case_history.cmpl_case_id
			join cmpl_case_level 
					on cmpl_case_history.cmpl_case_level_id = cmpl_case_level.cmpl_case_level_id
			join cmpl_case_state 
					on cmpl_case_history.cmpl_case_state_id = cmpl_case_state.cmpl_case_state_id
		left outer join cmpl_invocation
				on cases.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
		left outer join cmpl_invocation_history
				on cases.cmpl_invocation_id = cmpl_invocation_history.cmpl_invocation_id
			left outer join cmpl_res_rule_result
					on cmpl_invocation.cmpl_invocation_id = cmpl_res_rule_result.cmpl_invocation_id
			left outer join cmpl_res_rule_result_history
					on cmpl_invocation_history.cmpl_invocation_id = cmpl_res_rule_result_history.cmpl_invocation_id
				join cmpl_case_invocation_history
						on coalesce(cmpl_res_rule_result.cmpl_res_rule_result_id, cmpl_res_rule_result_history.cmpl_res_rule_result_id) = cmpl_case_invocation_history.cmpl_res_rule_result_id
						and cmpl_case_history.cmpl_case_id = cmpl_case_invocation_history.cmpl_case_id
					left outer join restriction_severity current_restriction_sev
							on cmpl_case_invocation_history.calc_worst_error_level = current_restriction_sev.error_level
			join cmpl_profile_rule 
					on cmpl_case_history.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
				join cmpl_profile_ruleset
	        			on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
					join cmpl_profile 
	        				on cmpl_profile_ruleset.cmpl_profile_id = cmpl_profile.cmpl_profile_id
						join cmpl_account_profile 
	        					on cmpl_profile.cmpl_profile_id = cmpl_account_profile.cmpl_profile_id
							join account profile_account 
									on cmpl_account_profile.account_id = profile_account.account_id
								join cmpl_profile_type 
	        							on cmpl_account_profile.cmpl_profile_type_id = cmpl_profile_type.cmpl_profile_type_id
								join account_level profile_account_level
										on profile_account.account_level_code = profile_account_level.account_level_code
					join cmpl_ruleset 
	        				on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
    			join cmpl_ruleset_rule 
	    				on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
    				join cmpl_rule 
							on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
    		left outer join restriction_severity worst_restriction_sev
					on cmpl_case_history.calc_worst_error_level = worst_restriction_sev.error_level
			left outer join cmpl_case_level_override
					on cmpl_case_history.cmpl_case_id = cmpl_case_level_override.cmpl_case_id
			left outer join cmpl_case_user_history
					on cmpl_case_history.cmpl_case_id = cmpl_case_user_history.cmpl_case_id 
				left outer join inbox_message_history
						on cmpl_case_user_history.current_inbox_message_id = inbox_message_history.message_id
				left outer join user_info
						on cmpl_case_user_history.user_id = user_info.user_id
				left outer join workgroup
						on cmpl_case_user_history.user_id = workgroup.workgroup_id
			left outer join user_info approver_user_info
					on cmpl_case_history.approver = approver_user_info.user_id
			left outer join user_info reviewer_info
					on cmpl_case_history.reviewer = reviewer_info.user_id
		join account case_account 
				on cases.account_id = case_account.account_id
			join account_level case_account_level
					on case_account.account_level_code = case_account_level.account_level_code
	where (@show_asof_for_historical = 0 and coalesce(cmpl_invocation_history.asof_time, cmpl_invocation.asof_time) is null) 
		or (@show_asof_for_historical <> 0 and coalesce(cmpl_invocation_history.asof_time, cmpl_invocation.asof_time) is not null);
end;
end

go
if @@error = 0 print 'PROCEDURE: se_get_cmpl_breach_mgt_report created'
else print 'PROCEDURE: se_get_cmpl_breach_mgt_report error on creation'
go