

if exists (select * from sysobjects where name = 'se_get_replacement_country')
begin
	drop procedure se_get_replacement_country
	print 'PROCEDURE: se_get_replacement_country dropped'
end
go
create PROCEDURE [dbo].[se_get_replacement_country]  --se_get_replacement_country

AS

BEGIN


select country_code,
mnemonic
from country


END

go
if @@error = 0 print 'PROCEDURE: se_get_replacement_country created'
else print 'PROCEDURE: se_get_replacement_country error on creation'
go

