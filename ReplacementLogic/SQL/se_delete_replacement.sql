

if exists (select * from sysobjects where name = 'se_delete_replacement')
begin
	drop procedure se_delete_replacement
	print 'PROCEDURE: se_delete_replacement dropped'
end
go
create PROCEDURE [dbo].[se_delete_replacement]  --se_delete_replacement 2,189
(
	@replacement_id	numeric(10),
	@current_user  numeric(10)
)
AS
declare @hierarchy_id numeric(10)
BEGIN

update se_account_replacement 
set deleted = 1,
se_account_replacement.modified_by = @current_user,
se_account_replacement.modified_time = getdate()
where replacement_id = @replacement_id

END

go
if @@error = 0 print 'PROCEDURE: se_delete_replacement created'
else print 'PROCEDURE: se_delete_replacement error on creation'
go

