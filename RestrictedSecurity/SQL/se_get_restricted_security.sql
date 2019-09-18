if exists (select * from sysobjects where name = 'se_get_restricted_security')
begin
	drop procedure se_get_restricted_security
	print 'PROCEDURE: se_get_restricted_security dropped'
end
go
create procedure [dbo].[se_get_restricted_security] --se_get_restricted_security 125012,9
(     

 @account_id				 numeric(10) = -1,
 @current_user              numeric(10)


 )    
as 

begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
						declare @security_id				 numeric(10) = -1

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

select se_restricted_security.* ,
security.symbol,
0 as position_type_code,
account.short_name,
encumbered_type.mnemonic,
se_restriction_type.restriction_type_description
from se_restricted_security
join security on
security.security_id = se_restricted_security.security_id
join account on 
account.account_id = se_restricted_security.account_id
join encumbered_type on
encumbered_type.encumbered_type_code = se_restricted_security.encumber_type
join se_restriction_type on
se_restriction_type.restriction_type_id = se_restricted_security.restriction_type
where  (se_restricted_security.account_id in (select account_id from #acct) or @account_id =-1)
and (se_restricted_security.security_id = @security_id or @security_id = -1)
group by se_restricted_security.account_id,restriction_description,se_restricted_security.security_id,se_restricted_security.encumber_type,se_restricted_security.isEncumbered,se_restricted_security.exception_date
,se_restricted_security.tax_lot_id,se_restricted_security.restriction_type,se_restricted_security.rule_id,security.symbol,account.short_name,encumbered_type.mnemonic,se_restriction_type.restriction_type_description

	--select * from encumbered_type
end


go
if @@error = 0 print 'PROCEDURE: se_get_restricted_security created'
else print 'PROCEDURE: se_get_restricted_security error on creation'
go

