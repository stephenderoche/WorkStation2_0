if exists (select * from sysobjects where name = 'se_rule_Donotreverseatradethatwasplacedwithinthelast')
begin
	drop procedure se_rule_Donotreverseatradethatwasplacedwithinthelast
	print 'PROCEDURE: se_rule_Donotreverseatradethatwasplacedwithinthelast dropped'
end
go
create procedure [dbo].[se_rule_Donotreverseatradethatwasplacedwithinthelast]
--exec se_rule_Donotreverseatradethatwasplacedwithinthelast 10366, 1, -1, -1, 189, ACCT, 0,'Do not reverse a trade that was placed within the last'        
(@acctID                       numeric(10),
	@isAccountId                  smallint = 1,
	@ruleSet					  numeric(10),
	@rule						  numeric(10),
	@userId					  	  smallint,
	@sort_order                   nvarchar(40),
	@show_agg_rule_for_account	  smallint = 1,
	@description                  varchar(200)
)
as
	declare @ret_val int;
	declare @continue_flag					int;
	declare @cps_rpx_rule_details_by_acct	nvarchar(30);
	declare @cpe_rpx_rule_details_by_acct	nvarchar(30);
	declare @account_name					nvarchar(40);
	declare	@cmpl_profile_rule_id			numeric(10);
	declare @rule_name						nvarchar(255);
	declare @p_value						nvarchar(255);
	declare @p_value_original				nvarchar(255);
	declare @cmpl_param_clr_type_name		nvarchar(40);
	declare @cmpl_param_type_id				numeric(10);
	declare @cmpl_rule_param_id				numeric(10);
begin
                     

--exec se_get_rule_parameters 1 , 1, -1, -1, 189, ACCT, 0,'Do not reverse a trade that was placed within the last' 

                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;



	create table #legal_param_values  	(  		actual_value	nvarchar(255) not null,  		display_value	nvarchar(255) not null  	);
	
	create table #profiles_rpt_asgn_rulebyacct 
	(
		account_id numeric(10)  null,
		cmpl_profile_id numeric(10)  null,
		cmpl_profile_type_id numeric(10)  null
	);
	create table #results_rpt_asgn_rulebyacct 
	(
		account_id					numeric(10)  null,
		cmpl_profile_type_id		numeric(10)  null,
		cmpl_profile_rule_id		numeric(10)  null,
		manager						int null,
		manager_name				nvarchar(40) null,
		account_name				nvarchar(40) null,
		severity					nvarchar(100),
		display_name				nvarchar(255) null,
		description					nvarchar(255) null,
		run_type					nvarchar(40) null,
		modified_date				nvarchar(10),
		modified_by					nvarchar(40) null,
		ruleset_name				nvarchar(255) null,
		profile_type				nvarchar(100),
		ancestor_account			nvarchar(40) null,
        profile_comments            nvarchar(255) null,
        ruleset_comments            nvarchar(255) null,
		cmpl_rule_id				numeric(10) null,
		cmpl_ruleset_rule_id 		numeric(10) null 
	);
	create table #result_rpt_asgn_parambyacct 
	(
		cmpl_rule_id				numeric(10) not null,
		display_name				nvarchar(255) null,
		cmpl_param_name				nvarchar(255) null,
		cmpl_param_type_name		nvarchar(255) null,
		cmpl_param_value			nvarchar(255) null,
		cmpl_param_desc				nvarchar(255) null,
		cmpl_param_list_proc		nvarchar(255) null,
		cmpl_param_type_id			numeric(10) not null,
		cmpl_rule_param_id			numeric(10) not null,
		cmpl_profile_rule_id		numeric(10) not null,
		cmpl_ruleset_rule_id		numeric(10) not null,
		account_id					numeric(10) not null
	);
	create table #cmpl_profile_display_value
	(
		display_value	nvarchar(255) null
	);
	
	if @show_agg_rule_for_account = 0
	begin
		insert into #profiles_rpt_asgn_rulebyacct 
			(
				account_id,
				cmpl_profile_id,
				cmpl_profile_type_id
			)
		select 
			cmpl_account_profile.account_id,
			cmpl_account_profile.cmpl_profile_id,
			cmpl_account_profile.cmpl_profile_type_id
		from account_hierarchy_map
			join cmpl_account_profile on account_hierarchy_map.child_id = cmpl_account_profile.account_id
			join cmpl_profile on cmpl_account_profile.cmpl_profile_id = cmpl_profile.cmpl_profile_id
		where cmpl_account_profile.cmpl_profile_type_id in (1, 3)
			and cmpl_profile.deleted = 0
		and account_hierarchy_map.parent_id = @acctID
		union
		select 
			ahm_to_descendants.child_id as account_id,
			cmpl_account_profile.cmpl_profile_id,
			cmpl_account_profile.cmpl_profile_type_id
		from account_hierarchy_map ahm_to_descendants
			join account on ahm_to_descendants.child_id = account.account_id 
				and account.account_level_code <> 1
			join account_hierarchy_map ahm_to_ancestors on ahm_to_descendants.child_id = ahm_to_ancestors.child_id
			join cmpl_account_profile on ahm_to_ancestors.parent_id = cmpl_account_profile.account_id
			join cmpl_profile on cmpl_account_profile.cmpl_profile_id = cmpl_profile.cmpl_profile_id
		where
		   ahm_to_descendants.parent_id= @acctID
			 and cmpl_account_profile.cmpl_profile_type_id = 2 and
			cmpl_profile.deleted = 0;
	end else begin
		insert into #profiles_rpt_asgn_rulebyacct 
			(
				account_id,
				cmpl_profile_id,
				cmpl_profile_type_id
			)
		select distinct
			ahm_to_descendants.child_id as account_id,
			cmpl_account_profile.cmpl_profile_id,
			cmpl_account_profile.cmpl_profile_type_id
		from account_hierarchy_map ahm_to_descendants
			join account on ahm_to_descendants.child_id = account.account_id
			join account_hierarchy_map ahm_to_ancestors on ahm_to_descendants.child_id = ahm_to_ancestors.child_id
			join cmpl_account_profile on ahm_to_ancestors.parent_id = cmpl_account_profile.account_id
			join cmpl_profile on cmpl_account_profile.cmpl_profile_id = cmpl_profile.cmpl_profile_id
		where cmpl_profile.deleted = 0;
	end;
	insert into #results_rpt_asgn_rulebyacct
	select
		#profiles_rpt_asgn_rulebyacct.account_id,
		#profiles_rpt_asgn_rulebyacct.cmpl_profile_type_id,
		cmpl_profile_rule.cmpl_profile_rule_id,
		account.manager,
		user_info.name,
		account.name_1,
		COALESCE(stat_conv_pre_1.description, stat_conv_pre_2.description) + '/' + nchar(13) + COALESCE(stat_conv_post_1.description, stat_conv_post_2.description) as severity,
		cmpl_rule.display_name,
		cmpl_rule.description,
		case when COALESCE(cmpl_profile_rule.run_pre_trade, cmpl_ruleset_rule.run_pre_trade) = 1 
			and COALESCE(cmpl_profile_rule.run_post_trade, cmpl_ruleset_rule.run_post_trade) = 1 then 'Pre/Post'
		   when COALESCE(cmpl_profile_rule.run_pre_trade, cmpl_ruleset_rule.run_pre_trade) = 0 
			and COALESCE(cmpl_profile_rule.run_post_trade, cmpl_ruleset_rule.run_post_trade) = 1 then 'Post'
		   when COALESCE(cmpl_profile_rule.run_pre_trade, cmpl_ruleset_rule.run_pre_trade) = 1 
			and COALESCE(cmpl_profile_rule.run_post_trade, cmpl_ruleset_rule.run_post_trade)  = 0 then 'Pre'
		   else 'None' end,		  
		convert(nchar(10), cmpl_profile_rule.modified_time, 101),
		mod_by.name,
		cmpl_ruleset.name as ruleset_name,
		cmpl_profile_type.name,	
		a1.short_name,  
		cmpl_profile_rule.comments as profile_comments,
		cmpl_ruleset_rule.comments as ruleset_comments,
		cmpl_rule.cmpl_rule_id,
		cmpl_profile_rule.cmpl_ruleset_rule_id
	 from #profiles_rpt_asgn_rulebyacct
		join cmpl_profile_ruleset on #profiles_rpt_asgn_rulebyacct.cmpl_profile_id = cmpl_profile_ruleset.cmpl_profile_id and cmpl_profile_ruleset.deleted = 0
		join cmpl_profile_rule on cmpl_profile_ruleset.cmpl_profile_ruleset_id = cmpl_profile_rule.cmpl_profile_ruleset_id and cmpl_profile_rule.deleted = 0
		join account on #profiles_rpt_asgn_rulebyacct.account_id = account.account_id
		left outer join user_info on account.manager = user_info.user_id
		join cmpl_ruleset_rule on cmpl_ruleset_rule.cmpl_ruleset_rule_id = cmpl_profile_rule.cmpl_ruleset_rule_id
		left outer join cmpl_status_conversion stat_conv_pre_1 on cmpl_profile_rule.cmpl_status_conversion_id = stat_conv_pre_1.cmpl_status_conversion_id
		left outer join cmpl_status_conversion stat_conv_pre_2 on cmpl_ruleset_rule.cmpl_status_conversion_id = stat_conv_pre_2.cmpl_status_conversion_id 
		left outer join cmpl_status_conversion stat_conv_post_1 on cmpl_profile_rule.cmpl_post_status_conv_id = stat_conv_post_1.cmpl_status_conversion_id 
		left outer join cmpl_status_conversion stat_conv_post_2 on cmpl_ruleset_rule.cmpl_post_status_conv_id = stat_conv_post_2.cmpl_status_conversion_id 
		join cmpl_rule on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
		join cmpl_ruleset on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
		join user_info mod_by on mod_by.user_id = cmpl_profile_rule.modified_by
		join cmpl_profile_type on cmpl_profile_type.cmpl_profile_type_id = #profiles_rpt_asgn_rulebyacct.cmpl_profile_type_id
		join cmpl_account_profile on cmpl_account_profile.cmpl_account_profile_id = #profiles_rpt_asgn_rulebyacct.cmpl_profile_id
		join account a1 on a1.account_id = cmpl_account_profile.account_id
		join #account on account.account_id = #account.account_id
	where (cmpl_ruleset.cmpl_ruleset_id = @ruleSet or @ruleSet = -1)
		and (cmpl_rule.cmpl_rule_id = @rule or @rule = -1)
		and cmpl_rule.description = @description;
		
		--select * from #results_rpt_asgn_rulebyacct

	insert into #result_rpt_asgn_parambyacct
		(cmpl_rule_id,
		display_name,
		cmpl_param_name,
		cmpl_param_type_name,
		cmpl_param_value,
		cmpl_param_desc,
		cmpl_param_list_proc,
		cmpl_param_type_id,
		cmpl_rule_param_id,
		cmpl_profile_rule_id,
		cmpl_ruleset_rule_id,
		account_id
		)
	select 
		#assigned_rules.cmpl_rule_id,
		#assigned_rules.display_name,
		cmpl_rule_param.name,
		cmpl_param_type.name,
		case when cmpl_param_type_list_attr.cmpl_param_type_id is NULL then COALESCE(cmpl_profile_param_value.value, cmpl_rsr_param_value.value, cmpl_rule_param.default_value)
			else list.list_name
		end,
		cmpl_rule_param.description,
		cmpl_param_type_general.legal_value_list_procedure,
		cmpl_rule_param.cmpl_param_type_id,
		cmpl_rule_param.cmpl_rule_param_id,
		#assigned_rules.cmpl_profile_rule_id,
		#assigned_rules.cmpl_ruleset_rule_id,
		#assigned_rules.account_id
	from cmpl_rule_param
		join #results_rpt_asgn_rulebyacct #assigned_rules on #assigned_rules.cmpl_rule_id = cmpl_rule_param.cmpl_rule_id
			and cmpl_rule_param.deleted = 0															
		join cmpl_param_type on cmpl_param_type.cmpl_param_type_id =  cmpl_rule_param.cmpl_param_type_id
		left outer join cmpl_profile_param_value on #assigned_rules.cmpl_profile_rule_id = cmpl_profile_param_value.cmpl_profile_rule_id
			and #assigned_rules.cmpl_rule_id = cmpl_profile_param_value.cmpl_rule_id
			and cmpl_rule_param.cmpl_rule_param_id = cmpl_profile_param_value.cmpl_rule_param_id 
			and cmpl_profile_param_value.deleted = 0 
		left outer join cmpl_rsr_param_value on #assigned_rules.cmpl_ruleset_rule_id = cmpl_rsr_param_value.cmpl_ruleset_rule_id
			and #assigned_rules.cmpl_rule_id = cmpl_rsr_param_value.cmpl_rule_id
			and cmpl_rule_param.cmpl_rule_param_id = cmpl_rsr_param_value.cmpl_rule_param_id
			and cmpl_rsr_param_value.deleted = 0 
		left outer join cmpl_param_type_list_attr on cmpl_param_type_list_attr.cmpl_param_type_id = cmpl_rule_param.cmpl_param_type_id
		left outer join list on convert(nvarchar(225),  list.list_id) = COALESCE(cmpl_profile_param_value.value, cmpl_rsr_param_value.value, cmpl_rule_param.default_value)
		left outer join cmpl_param_type_general on cmpl_param_type_general.cmpl_param_type_id = cmpl_rule_param.cmpl_param_type_id;
	select @cmpl_profile_rule_id = min(cmpl_profile_rule_id) from #results_rpt_asgn_rulebyacct;	
	while @cmpl_profile_rule_id is not null
	begin
		select 
			@rule_name = display_name 
		from #results_rpt_asgn_rulebyacct 
		where cmpl_profile_rule_id = @cmpl_profile_rule_id
			;
		execute @ret_val = get_parameterized_rule_name  @rule_id = null, @cmpl_ruleset_rule_id = null, @cmpl_profile_rule_id = @cmpl_profile_rule_id, @rule_name = @rule_name output;
		update #results_rpt_asgn_rulebyacct 
		set 
			display_name = @rule_name 
		where cmpl_profile_rule_id = @cmpl_profile_rule_id;
		select 
			@rule_name = description 
		from #results_rpt_asgn_rulebyacct 
		where cmpl_profile_rule_id = @cmpl_profile_rule_id
			;
		execute @ret_val = get_parameterized_rule_name  @rule_id = null, @cmpl_ruleset_rule_id = null, @cmpl_profile_rule_id = @cmpl_profile_rule_id, @rule_name = @rule_name output;
		update #results_rpt_asgn_rulebyacct 
		set 
			description = @rule_name 
		where cmpl_profile_rule_id = @cmpl_profile_rule_id;
		select 
			@cmpl_profile_rule_id = min(cmpl_profile_rule_id) 
		from #results_rpt_asgn_rulebyacct 
		where cmpl_profile_rule_id > @cmpl_profile_rule_id;
	end;
	select @cmpl_profile_rule_id = min(cmpl_profile_rule_id)
	from #result_rpt_asgn_parambyacct;
	while @cmpl_profile_rule_id is not null
	begin
		select @cmpl_rule_param_id = min(cmpl_rule_param_id)
		from #result_rpt_asgn_parambyacct
		where #result_rpt_asgn_parambyacct.cmpl_profile_rule_id = @cmpl_profile_rule_id;
		while @cmpl_rule_param_id is not null
		begin
			select 
				@p_value			= cmpl_param_value,
				@p_value_original	= cmpl_param_value,
				@cmpl_param_type_id = cmpl_param_type_id
			from #result_rpt_asgn_parambyacct
			where #result_rpt_asgn_parambyacct.cmpl_profile_rule_id = @cmpl_profile_rule_id
				and #result_rpt_asgn_parambyacct.cmpl_rule_param_id = @cmpl_rule_param_id
			;
			delete #legal_param_values;
			execute @ret_val = get_cmpl_legal_param_values_h  @cmpl_param_clr_type_name = @cmpl_param_clr_type_name output, @cmpl_param_type_id = @cmpl_param_type_id;
			select 
				@p_value = #legal_param_values.display_value
			from #legal_param_values
			where #legal_param_values.actual_value = @p_value
				;
			update #result_rpt_asgn_parambyacct 
			set 
				cmpl_param_value = @p_value
			where cmpl_profile_rule_id = @cmpl_profile_rule_id 
				and cmpl_param_value = @p_value_original 
				and cmpl_param_type_id = @cmpl_param_type_id;
			select @cmpl_rule_param_id = min(cmpl_rule_param_id)
			from #result_rpt_asgn_parambyacct
			where #result_rpt_asgn_parambyacct.cmpl_profile_rule_id = @cmpl_profile_rule_id
				and #result_rpt_asgn_parambyacct.cmpl_rule_param_id > @cmpl_rule_param_id;
		end;
		select @cmpl_profile_rule_id = min(cmpl_profile_rule_id)
		from #result_rpt_asgn_parambyacct
		where cmpl_profile_rule_id > @cmpl_profile_rule_id
			and cmpl_param_list_proc is not null;
	end;

	--select * from #result_rpt_asgn_parambyacct

		create table #rule_param_value ( account_id numeric(10), Days numeric(10));

	insert into #rule_param_value
	select account_id, 
	max(case when cmpl_param_name ='Days' then cmpl_param_value else null end) Days
	from  #result_rpt_asgn_parambyacct
	group by account_id;

delete from se_restricted_security
where 
 se_restricted_security.account_id in (select account_id from #account )
 and se_restricted_security.restriction_type = 1 
 and se_restricted_security.rule_id = 1
	

--select * into pkn_result_rpt_asgn_parambyacct from #result_rpt_asgn_parambyacct;
--select * into pkn_rule_param_value from #rule_param_value
--select * into pkn_account from #account;

create table #se_restricted_security 
         ( tax_lot_id numeric(10) ,
		   lot_number nvarchar(40),
		    account_id numeric(10), 
		    security_id numeric(10),
			encumber_type numeric(10),
			isEncumbered tinyint,
			exception_date datetime,
			restriction_description nvarchar(1000),
			restriction_type numeric(10),
			rule_id  numeric(10))

insert into
#se_restricted_security
(tax_lot_id,lot_number, account_id,security_id,encumber_type,isEncumbered,exception_date,restriction_description,restriction_type,rule_id)
select tax_lot.tax_lot_id,tax_lot.lot_number,#account.account_id,security.security_id,27,1,tax_lot.trade_date,
'No reverse trades ( No SELL) for '  + #result_rpt_asgn_parambyacct.cmpl_param_value  + ' days'
,1
,1
from tax_lot
join security on
security.security_id = tax_lot.security_id
join price on
security.security_id = price.security_id
join currency on
currency.security_id = security.principal_currency_id
join #result_rpt_asgn_parambyacct on
#result_rpt_asgn_parambyacct.account_id = tax_lot.account_id
join #account on
#account.account_id = tax_lot.account_id
JOIN #rule_param_value ON #rule_param_value.account_id = tax_lot.account_id
where tax_lot.trade_date  >= (GetDate() - (#rule_param_value.Days +1));


update tax_lot
set encumbered_quantity = coalesce(quantity,0),
encumbered_type_code = 27
from #se_restricted_security
where tax_lot.tax_lot_id= #se_restricted_security.tax_lot_id;




insert into
se_restricted_security(tax_lot_id,account_id,security_id,encumber_type,isEncumbered,exception_date,restriction_description,restriction_type,rule_id)
select lot_number,account_id,security_id,encumber_type,isEncumbered,exception_date,restriction_description,restriction_type,rule_id
from #se_restricted_security
 
/*
update tax_lot
set encumbered_quantity = coalesce(quantity,0),
encumbered_type_code = 27
from tax_lot
join security on
security.security_id = tax_lot.security_id
join price on
security.security_id = price.security_id
join currency on
currency.security_id = security.principal_currency_id
join #account on
#account.account_id = tax_lot.account_id
JOIN #rule_param_value ON #rule_param_value.account_id = tax_lot.account_id
and tax_lot.trade_date  >= (GetDate() - (#rule_param_value.Days +1))


insert into
se_restricted_security(tax_lot_id,account_id,security_id,encumber_type,isEncumbered,exception_date,restriction_description,restriction_type,rule_id)
select
distinct(tax_lot.lot_number)
,#account.account_id,
security.security_id,
27,
1,
tax_lot.trade_date,
'No reverse trades ( No SELL) for '  + #result_rpt_asgn_parambyacct.cmpl_param_value  + ' days'
,1
,1
from tax_lot
join security on
security.security_id = tax_lot.security_id
join price on
security.security_id = price.security_id
join currency on
currency.security_id = security.principal_currency_id
join #result_rpt_asgn_parambyacct on
#result_rpt_asgn_parambyacct.account_id = tax_lot.account_id
join #account on
#account.account_id = tax_lot.account_id
JOIN #rule_param_value ON #rule_param_value.account_id = tax_lot.account_id
where tax_lot.trade_date  >= (GetDate() - (#rule_param_value.Days +1))

*/

SELECT *
INTO #ReverseSell
FROM
(
select
account.account_id,
security.security_id,
se_wash_sale_list.symbol,
price.latest,
se_wash_sale_list.salePrice,
se_wash_sale_list.trade_date,
'No reverse trades ( No BUY) for '  + #result_rpt_asgn_parambyacct.cmpl_param_value  + ' days' as direction
from se_wash_sale_list
join security on
se_wash_sale_list.symbol = security.symbol
join account on
account.short_name = se_wash_sale_list.pfid
join #result_rpt_asgn_parambyacct on
#result_rpt_asgn_parambyacct.account_id = account.account_id
JOIN #rule_param_value ON #rule_param_value.account_id = #result_rpt_asgn_parambyacct.account_id
join price on
price.security_id = security.security_id
and se_wash_sale_list.trade_date  >= ((GetDate() - (#rule_param_value.Days +1)))
) test

--select * from se_wash_sale_list

insert into
se_restricted_security(account_id,security_id,encumber_type,isEncumbered,tax_lot_id,exception_date,restriction_description,restriction_type,rule_id)
select
#ReverseSell.account_id,
#ReverseSell.security_id,
26,
0,
'',
#ReverseSell.trade_date,
#ReverseSell.direction
,1
,1
from #ReverseSell

	
end


go
if @@error = 0 print 'PROCEDURE: se_rule_Donotreverseatradethatwasplacedwithinthelast created'
else print 'PROCEDURE: se_rule_Donotreverseatradethatwasplacedwithinthelast error on creation'
go

