if exists (select * from sysobjects where name = 'se_get_restricted_security')
begin
	drop procedure se_get_restricted_security
	print 'PROCEDURE: se_get_restricted_security dropped'
end
go
create procedure [dbo].[se_get_restricted_security] --se_get_restricted_security 10366,1559
(     

 @account_id				 numeric(10) = -1,
 @security_id				 numeric(10) = -1


 )    
as 

begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;

select se_restricted_security.* ,
security.symbol
from se_restricted_security
join security on
security.security_id = se_restricted_security.security_id
where  (se_restricted_security.account_id = @account_id or @account_id =-1)
and (se_restricted_security.security_id = @security_id or @security_id = -1)

	
end


go
if @@error = 0 print 'PROCEDURE: se_get_restricted_security created'
else print 'PROCEDURE: se_get_restricted_security error on creation'
go

