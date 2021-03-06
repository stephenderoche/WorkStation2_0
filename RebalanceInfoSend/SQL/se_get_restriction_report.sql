if exists (select * from sysobjects where name = 'se_get_restriction_report')
begin
	drop procedure se_get_restriction_report
	print 'PROCEDURE: se_get_restriction_report dropped'
end
go
create procedure [dbo].[se_get_restriction_report]
(
	@account_id numeric(10) = null,
	@workgroup_id numeric(10) = null,
	@by_workgroup tinyint = 0, 
	@violations_only tinyint = 0 
) 
as
declare @violations_only_local tinyint;
declare @continue_flag int;
declare @ret_val int;
declare	@cps_get_restriction_report nvarchar(30);
declare	@cpe_get_restriction_report nvarchar(30);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	create table #rest_rpt_account  	(  		account_id numeric(10) null  	);
	create table #rest_rpt_parent  	(  		account_id numeric(10) null      );
	create table #rest_rpt_orders  	(  		order_id				numeric(10) null,  		account_id				numeric(10) null,  		security_id				numeric(10) null,  		cmpl_profile_rule_id    numeric(10) null,  		restriction_detail_id	numeric(10) null,  		quantity				float null,  		side_code				tinyint null,  		override_check			tinyint null,  		restriction_check		tinyint null  	);
	create table #latest_restriction_override  	(  		restriction_override_id	numeric(10) not null  	);
	select @continue_flag = 1
select @cps_get_restriction_report = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cps_get_restriction_report'
	and sysobjects.type = 'P'
if @cps_get_restriction_report is not null
begin
	execute @ret_val = cps_get_restriction_report
		@continue_flag output, @account_id,
		@workgroup_id,
		@by_workgroup,
		@violations_only
	if (@ret_val != 0 and @ret_val < 60000) or @continue_flag = 0
	begin
		return @ret_val
	end
end
	if @by_workgroup = 1
	begin
   		insert into #rest_rpt_account (account_id)
		select account.account_id
		from 
		    workgroup_tree_map
		join account	
				on account.account_id = workgroup_tree_map.child_id 
					and account.deleted = 0
		where workgroup_tree_map.parent_id = @workgroup_id 
			and	workgroup_tree_map.child_type = 3
		option (keepfixed plan)
		;
	end else if @by_workgroup = 0 and @account_id is not null begin
		insert into #rest_rpt_account (account_id)
		select account_hierarchy_map.child_id
		from account_hierarchy_map, account
		where
			account_hierarchy_map.parent_id = @account_id 
			and	account_hierarchy_map.child_id = account.account_id
			and	account.account_level_code <> 1
			and account.deleted = 0;
	end;
	select @violations_only_local = @violations_only;
	if exists(
		select 1 
		from compliance_control
		where compliance_control.report_violations_only = 1
		) begin 
		select @violations_only_local = 1;
	end;
	insert into #rest_rpt_parent (account_id)
	select distinct account_hierarchy_map.parent_id
	from account_hierarchy_map, account
	where
		account_hierarchy_map.child_id in (select account_id from #rest_rpt_account) and
		account_hierarchy_map.parent_id = account.account_id and
		account.deleted = 0;
	insert into #rest_rpt_orders (order_id, account_id, security_id, cmpl_profile_rule_id)
		select distinct
			proposed_orders.order_id,
			proposed_orders.account_id,
			proposed_orders.security_id, 
			cmpl_res_rule_result.cmpl_profile_rule_id 
		from restriction_detail, restriction_map, proposed_orders, #rest_rpt_account, cmpl_res_rule_result
		where 
		    restriction_detail.restriction_detail_id = restriction_map.restriction_detail_id 
		    and restriction_detail.restriction_id = cmpl_res_rule_result.cmpl_res_rule_result_id
			and	restriction_map.order_id = proposed_orders.order_id 
			and	restriction_detail.error_level >= @violations_only_local 
			and	restriction_detail.account_id = #rest_rpt_account.account_id
			and proposed_orders.is_pre_executed = 0
	union
		select distinct
			proposed_orders.order_id,
			proposed_orders.account_id,
			proposed_orders.security_id,
			cmpl_res_rule_result.cmpl_profile_rule_id
		from restriction_detail, restriction_map, proposed_orders, #rest_rpt_parent , cmpl_res_rule_result
		where 
		    restriction_detail.restriction_detail_id = restriction_map.restriction_detail_id 
		    and restriction_detail.restriction_id = cmpl_res_rule_result.cmpl_res_rule_result_id
			and	restriction_map.order_id = proposed_orders.order_id 
			and	restriction_detail.error_level >= @violations_only_local 
			and	restriction_detail.group_id = #rest_rpt_parent.account_id 
			and	restriction_detail.account_id is null
			and proposed_orders.is_pre_executed = 0
	union
		select distinct
			orders.order_id,
			orders.account_id,
			orders.security_id,
			cmpl_res_rule_result.cmpl_profile_rule_id
		from restriction_detail, restriction_map, orders, #rest_rpt_account, cmpl_res_rule_result
		where
			restriction_detail.restriction_detail_id = restriction_map.restriction_detail_id and
			restriction_detail.restriction_id = cmpl_res_rule_result.cmpl_res_rule_result_id and
			restriction_map.order_id = orders.order_id and
			orders.deleted = 0 and
			restriction_detail.error_level >= @violations_only_local and
			restriction_detail.account_id = #rest_rpt_account.account_id
	union
		select distinct
			orders.order_id,
			orders.account_id,
			orders.security_id,
			cmpl_res_rule_result.cmpl_profile_rule_id
		from restriction_detail, restriction_map, orders, #rest_rpt_parent , cmpl_res_rule_result
		where
			restriction_detail.restriction_detail_id = restriction_map.restriction_detail_id and
			restriction_detail.restriction_id = cmpl_res_rule_result.cmpl_res_rule_result_id and 
			restriction_map.order_id = orders.order_id and
			orders.deleted = 0 and
			restriction_detail.error_level >= @violations_only_local and
			restriction_detail.group_id = #rest_rpt_parent.account_id and
			restriction_detail.account_id is null;
	update #rest_rpt_orders
	set restriction_detail_id =
	(
		select MAX(restriction_detail.restriction_detail_id)
		from restriction_detail
		join  restriction_map on restriction_map.restriction_detail_id = restriction_detail.restriction_detail_id 
		join  cmpl_res_rule_result on  cmpl_res_rule_result.cmpl_res_rule_result_id = restriction_detail.restriction_id
		where 
			restriction_map.order_id = #rest_rpt_orders.order_id and
			restriction_detail.account_id = #rest_rpt_orders.account_id and
			restriction_detail.security_id = #rest_rpt_orders.security_id and
			cmpl_res_rule_result.cmpl_profile_rule_id = #rest_rpt_orders.cmpl_profile_rule_id
	) ;
	insert into #latest_restriction_override 
	(
		restriction_override_id
	) 
	select 
		MAX(restriction_override_id) 
	from restriction_override 
	join #rest_rpt_orders 
		on #rest_rpt_orders.order_id = restriction_override.order_id
	group by restriction_override.order_id;
	select distinct
		restriction_severity.description as severity_description,
		cmpl_account_profile.account_id as group_account_id,	
		#rest_rpt_orders.account_id as single_account_id,
		restriction_detail.restriction_name as restriction_name,
		restriction_detail.low_value as low_value,
		restriction_detail.high_value as high_value,
		restriction_detail.actual_value as actual_value,
		restriction_detail.restriction_status as restriction_status,
		restriction_detail.restriction_status_2 as restriction_status_2,
		cmpl_scope.name as scope_description,
		coalesce(contract.client_contract_id, security.symbol) as symbol,
		restriction_detail.error_level as error_level_code,
		restriction_detail.scope_level as scope_level_code,
		group_name.short_name as group_account_name_1,
		account_name.short_name as single_account_name_1,
		restriction_detail.restriction_id as restriction_id,
		sign(restriction_detail.group_id - restriction_detail.account_id) as group_account_flag,
		coalesce(swp_security.name_1, security.name_1) as security_name_1,
		restriction_map.order_id as order_id,
		restriction_detail.run_by as run_by_id,
		coalesce(user_info.name, '<Unknown>') as run_by_name,
		restriction_detail.run_time as run_time,
		case cmpl_profile_rule.override_comments
			when 0 then cmpl_ruleset_rule.comments
			else cmpl_res_rule_result.profile_rule_comment
			end as rule_assignment_comment,
		restriction_detail.cmpl_res_holding_status_id,
		restriction_override.override_number,
		case 
			when status_code = 0 then restriction_override.reason
			else null
			end as override_reason,
		restriction_user_info.name as override_by,
		restriction_override.override_time,
		coalesce(swp_security.security_id, security.security_id) as security_id,
		coalesce(swp_security.issuer_id, security.issuer_id) as issuer_id,
		coalesce(swp_security.security_level_code, security.security_level_code) as security_level_code,
		coalesce(swp_security.major_asset_code, security.major_asset_code) as major_asset_code,
		coalesce(swp_security.minor_asset_code, security.minor_asset_code) as minor_asset_code
	from 
		#rest_rpt_account
		join #rest_rpt_orders on #rest_rpt_account.account_id = #rest_rpt_orders.account_id
		join account account_name on #rest_rpt_orders.account_id = account_name.account_id
		join security on #rest_rpt_orders.security_id = security.security_id
		join restriction_map 
			on #rest_rpt_orders.restriction_detail_id = restriction_map.restriction_detail_id
		join restriction_detail 
			on  #rest_rpt_orders.restriction_detail_id = restriction_detail.restriction_detail_id
		join restriction_severity on restriction_detail.error_level = restriction_severity.error_level
		join cmpl_scope on restriction_detail.scope_level = cmpl_scope.cmpl_scope_id
		left outer join user_info on restriction_detail.run_by = user_info.user_id
		join cmpl_res_rule_result on restriction_detail.restriction_id = cmpl_res_rule_result.cmpl_res_rule_result_id
		join cmpl_profile_rule on  #rest_rpt_orders.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
		join cmpl_ruleset_rule on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
		join cmpl_profile_ruleset on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
		join cmpl_account_profile on cmpl_profile_ruleset.cmpl_profile_id = cmpl_account_profile.cmpl_profile_id
		join account group_name on cmpl_account_profile.account_id = group_name.account_id
		left outer join restriction_override 
			on restriction_map.order_id = restriction_override.order_id 
			and restriction_override.restriction_override_id in 
				(select restriction_override_id from #latest_restriction_override)
		left outer join user_info restriction_user_info 
			on restriction_user_info.user_id = restriction_override.override_by
		left outer join contract
			on restriction_detail.contract_id = contract.contract_id
		left outer join security swp_security
			on contract.security_id = swp_security.security_id
	order by
		restriction_detail.error_level desc,
		sign(restriction_detail.group_id - restriction_detail.account_id) desc,
		cmpl_account_profile.account_id,
		#rest_rpt_orders.account_id,
		restriction_map.order_id,
		restriction_detail.restriction_id,
		restriction_detail.scope_level;
	select @cpe_get_restriction_report = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cpe_get_restriction_report'
	and sysobjects.type = 'P'
if @cpe_get_restriction_report is not null
begin
	execute @ret_val = cpe_get_restriction_report
		@account_id,
		@workgroup_id,
		@by_workgroup,
		@violations_only
	if (@ret_val != 0 and @ret_val < 60000)
	begin
		return @ret_val
	end
end
end
