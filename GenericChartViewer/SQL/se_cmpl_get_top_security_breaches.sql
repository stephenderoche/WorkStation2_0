if exists (select * from sysobjects where name = 'se_cmpl_get_top_security_breaches')
begin
	drop procedure se_cmpl_get_top_security_breaches
	print 'PROCEDURE: se_cmpl_get_top_security_breaches dropped'
end
go


create procedure [dbo].[se_cmpl_get_top_security_breaches]  --exec se_cmpl_get_top_security_breaches 199
(
	@account_id                   numeric(10), 
	@userId					      smallint = 189,
	@isAccountId                  smallint = 1,
	@manager                      smallint = -1,
	@ruleSet					  numeric(10)= -1,
	@rule						  numeric(10)=-1,
	@includeWarnings			  tinyint = 0
	
)
as
	declare @sacctID						nvarchar(40);
	declare @resultCount					int;
	declare @errorLevel						int;
	declare @current_user					int;
	declare @includeWarningsLocal			int;
	declare @ret_val						int;
	
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	create table #account  	(  		account_id numeric(10) not null  	);
	create table #results_rpt_pst_top_sec_v 
	(
		security_id			numeric(10) null,
		symbol				nvarchar(40) null,
		name_1				nvarchar(40) null,
		violation_count		int null
	);
	create table #results1_rpt_pst_top_sec_v 
	(
		security_id                 numeric(10),
		symbol                      nvarchar(40),
		name_1                      nvarchar(40),
		cmpl_res_holding_status     numeric(10)
	);

		create table #results1_rpt_sum_top_sec_v 
	(
		
		symbol                      nvarchar(40),
		breachcount                     float,
		sumTotal                       float
	);
	

	select @includeWarningsLocal = @includeWarnings;
	if @includeWarningsLocal = 0
	begin
		select @includeWarningsLocal = @includeWarningsLocal + 1;
		select @errorLevel = 0;
	end else begin
		select @includeWarningsLocal = @includeWarningsLocal - 1;
		select @errorLevel = 1;
	end;
       
	   	insert into #account
	select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0
        
	
	insert into #results_rpt_pst_top_sec_v
	select
		coalesce(security.security_id, swap_security.security_id) as security_id,
		coalesce(security.symbol, contract.client_contract_id) as symbol,
		coalesce(security.name_1, swap_security.name_1) as name_1,
		cmpl_res_holding_status.cmpl_res_rule_result_id
	from #account
	join account 
		on account.account_id = account.account_id
		and (account.manager = @manager or @manager = -1)
	join cmpl_res_rule_result 
		on #account.account_id = cmpl_res_rule_result.account_id	
	join cmpl_invocation 
		on cmpl_res_rule_result.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
	join cmpl_res_account_invocation
		on cmpl_invocation.cmpl_invocation_id = cmpl_res_account_invocation.latest_post_trade_invoc_id
		and cmpl_res_account_invocation.account_id = cmpl_res_rule_result.account_id
	join cmpl_res_holding_status 
		on cmpl_res_rule_result.cmpl_res_rule_result_id = cmpl_res_holding_status.cmpl_res_rule_result_id 
		and cmpl_res_holding_status.error_level > @includeWarningsLocal
	left outer join security 
		on cmpl_res_holding_status.security_id = security.security_id
	left outer join contract
		on cmpl_res_holding_status.contract_id = contract.contract_id
	left outer join security swap_security
		on contract.security_id = swap_security.security_id
	join cmpl_profile_rule 
		on cmpl_res_rule_result.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
		and (cmpl_profile_rule.cmpl_rule_id = @rule or @rule = -1)
	join cmpl_profile_ruleset 
		on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
		join cmpl_case   
      on account.account_id = cmpl_case.compliance_account_id 
	join cmpl_ruleset 
		on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
		and (cmpl_ruleset.cmpl_ruleset_id = @ruleSet or @ruleSet = -1)
			and

  ( (cmpl_case.name not like '%MISSING DATA%') and (cmpl_case.name not like '%CAUTION:%') and 
   (cmpl_case.name not like '%CONFIGURATION ERROR:%') and (cmpl_case.name not like '%INVALID DATA:%') )

  and cmpl_case.calc_worst_error_level = 4
  and cmpl_case.cmpl_case_state_id in (0)
  --select * case_state
	group by
		coalesce(security.security_id, swap_security.security_id), 
		coalesce(security.symbol, contract.client_contract_id),
		coalesce(security.name_1, swap_security.name_1),
		cmpl_res_holding_status.cmpl_res_rule_result_id;
	select @resultCount = count(1)
	from  #results_rpt_pst_top_sec_v;
	
	insert into #results1_rpt_sum_top_sec_v
		SELECT		
		    top(10)
			symbol,
			count(1) as market_value,
			1
			
		from
			#results_rpt_pst_top_sec_v
		group by symbol
		order by market_value desc, symbol asc;

		declare @totlsum as float

		select @totlsum = sum(breachcount) from #results1_rpt_sum_top_sec_v
		--print @totlsum


		update #results1_rpt_sum_top_sec_v
		set sumTotal = Round((breachcount/@totlsum),4)* 100

		select symbol,
		breachcount as market_value,
		sumTotal as pct_acct_total 
		 from #results1_rpt_sum_top_sec_v
		order by sumTotal desc
	;
	
	--exec se_cmpl_post_trade_top_sec_v -1

end




go
if @@error = 0 print 'PROCEDURE: se_cmpl_get_top_security_breaches created'
else print 'PROCEDURE: se_cmpl_get_top_security_breaches error on creation'
go
