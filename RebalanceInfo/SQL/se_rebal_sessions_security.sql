if exists (select * from sysobjects where name = 'se_rebal_sessions_security')
begin
	drop procedure se_rebal_sessions_security
	print 'PROCEDURE: se_rebal_sessions_security dropped'
end
go


create procedure  [dbo].[se_rebal_sessions_security] --se_rebal_sessions_security 3

(
       @session_id                       numeric(10)
    
		
)
as
    
	--select * into #Rebal_security from rebal_audit_security where rebal_session_id = @session_id  

	select 
	
	top 10 security.symbol,
	account.short_name,
	rebal_audit_security.*
	from rebal_audit_security
	join security on 
	security.security_id = rebal_audit_security.security_id
	join account on
	account.account_id = rebal_audit_security.account_id
	 where rebal_session_id = @session_id

go
if @@error = 0 print 'PROCEDURE: se_rebal_sessions_security created'
else print 'PROCEDURE: se_rebal_sessions_security error on creation'
go