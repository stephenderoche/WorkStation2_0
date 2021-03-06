if exists (select * from sysobjects where name = 'se_rebal_pos_exclusion')
begin
	drop procedure se_rebal_pos_exclusion
	print 'PROCEDURE: se_rebal_pos_exclusion dropped'
end
go


create procedure  [dbo].[se_rebal_pos_exclusion] --se_rebal_pos_exclusion 5

(
       @session_id                       numeric(10)
    
		
)
as
    
	select * into #rebal_pos_exclusion from rebal_pos_exclusion where rebal_session_id = @session_id  

	select 
	
	security.symbol,
	account.short_name,
	#rebal_pos_exclusion.*
	from #rebal_pos_exclusion
	join security on 
	security.security_id = #rebal_pos_exclusion.security_id
	join account on
	account.account_id = #rebal_pos_exclusion.account_id
	

	
go
if @@error = 0 print 'PROCEDURE: se_rebal_pos_exclusion created'
else print 'PROCEDURE: se_rebal_pos_exclusion error on creation'
go