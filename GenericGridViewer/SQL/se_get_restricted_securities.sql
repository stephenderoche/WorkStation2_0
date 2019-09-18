if exists (select * from sysobjects where name = 'se_get_restricted_securities')--se_get_restricted_securities 199
       begin
	drop procedure se_get_restricted_securities
	print 'PROCEDURE: se_get_restricted_securities dropped'
end
go

create procedure   [dbo].[se_get_restricted_securities]
(
	@account_id numeric(10)
) 
as
declare @rule_id numeric(10)
begin

select @rule_id = cmpl_rule_id from cmpl_rule where display_name = 'LST-Restricted Securities (All)'

		create table #list_ids
	(
		list_id numeric not null
	);
	insert into #list_ids
		select 
		cmpl_profile_param_value.value as list_id
	from cmpl_profile_rule 
		join cmpl_ruleset_rule on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id 
			and cmpl_ruleset_rule.deleted = 0
		join cmpl_profile_ruleset on cmpl_profile_ruleset.deleted = 0 
			and cmpl_profile_ruleset.cmpl_profile_ruleset_id = cmpl_profile_rule.cmpl_profile_ruleset_id
		join cmpl_profile on cmpl_profile.deleted = 0
			and cmpl_profile.cmpl_profile_id = cmpl_profile_ruleset.cmpl_profile_id
		join cmpl_account_profile on cmpl_account_profile.cmpl_profile_id = cmpl_profile.cmpl_profile_id
		join cmpl_rule on cmpl_rule.deleted = 0
			and cmpl_rule.cmpl_rule_id = cmpl_profile_rule.cmpl_rule_id
		join cmpl_profile_type on cmpl_account_profile.cmpl_profile_type_id = cmpl_profile_type.cmpl_profile_type_id
		join account on account.account_id = cmpl_account_profile.account_id
		join account_level on account.account_level_code = account_level.account_level_code
		join cmpl_profile_param_value on
		cmpl_profile_param_value.cmpl_rule_id = @rule_id
		and cmpl_profile_rule.cmpl_profile_rule_id = cmpl_profile_param_value.cmpl_profile_rule_id
		where cmpl_account_profile.account_id = @account_id

	select 
	list.list_mnemonic,
	list.list_name,
	@account_id as account_id,
	account.short_name,
	security.security_id,
	security.symbol
	 from #list_ids
 
 join list_member on
 list_member.list_id = #list_ids.list_id
 join 
 security on 
 security.security_id = list_member.list_decimal_value
 join list on
 list.list_id = #list_ids.list_id
 join account on
	account.account_id = @account_id

	end
go
if @@error = 0 print 'PROCEDURE: se_get_restricted_securities created'
else print 'PROCEDURE: se_get_restricted_securities error on creation'
go

	
	

