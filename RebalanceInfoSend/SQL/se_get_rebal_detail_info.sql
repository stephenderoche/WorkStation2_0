

 --select * from rebal_session
if exists (select * from sysobjects where name = 'se_get_rebal_detail_info')
begin
	drop procedure se_get_rebal_detail_info
	print 'PROCEDURE: se_get_rebal_detail_info dropped'
end
go

create procedure  [dbo].[se_get_rebal_detail_info] --se_get_rebal_detail_info -1,10028
(
 @security_id numeric(10) = -1,
 @session_id numeric(10) = -1
)


as
    

	select 
	symbol,
	account.short_name,
	rebal_audit_account.orders_qty_generated as 'Total Account Orders',
	net_cash_change,
	rebal_audit_security.is_encumbered,
	rebal_audit_security.is_minimum,
	rebal_audit_security.is_restricted,
	case 
	    when se_restricted_security.Isencumbered in (0.1) then 1
	 else 0
	end  as is_preprocessed,
	rebal_audit_security.orders_qty_generated,
	rebal_audit_security.orders_mv_generated as ' Order Amount'
    from rebal_audit_security(NOLOCK)

	join account on
	account.account_id = rebal_audit_security.account_id
	join rebal_audit_account(NOLOCK) on
	rebal_audit_account.account_id = rebal_audit_security.account_id
	and rebal_audit_account.rebal_session_id = @session_id
	join security on
	security.security_id = @security_id
	left join se_restricted_security on
	se_restricted_security.security_id = security.security_id

	where (@session_id = -1 or @session_id = rebal_audit_security.rebal_session_id)
	and rebal_audit_security.security_id = @security_id
	

	go
if @@error = 0 print 'PROCEDURE: se_get_rebal_detail_info created'
else print 'PROCEDURE: se_get_rebal_detail_info error on creation'
go

--select * from rebal_audit_account where rebal_audit_account.rebal_session_id = 68
--select * from rebal_audit_security where rebal_audit_security.rebal_session_id = 68