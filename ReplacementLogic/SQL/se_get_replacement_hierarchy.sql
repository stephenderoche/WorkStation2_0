--select * from account where short_name like '%wm%'

if exists (select * from sysobjects where name = 'se_get_replacement_hierarchy')
begin
	drop procedure se_get_replacement_hierarchy
	print 'PROCEDURE: se_get_replacement_hierarchy dropped'
end
go
create PROCEDURE [dbo].[se_get_replacement_hierarchy]  --se_get_replacement_hierarchy

AS
declare @hierarchy_id numeric(10)
BEGIN


select @hierarchy_id = hierarchy_id from hierarchy where name = 'by GICS'

select hierarchy_sector_id,
description
 from hierarchy_sector
where hierarchy_id = @hierarchy_id
and cash_indicator = 0
order by depth

END

go
if @@error = 0 print 'PROCEDURE: se_get_replacement_hierarchy created'
else print 'PROCEDURE: se_get_replacement_hierarchy error on creation'
go

