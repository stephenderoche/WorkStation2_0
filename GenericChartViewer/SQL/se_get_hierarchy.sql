if exists (select * from sysobjects where name = 'se_get_hierarchy')
begin
	drop procedure se_get_hierarchy
	print 'PROCEDURE: se_get_hierarchy dropped'
end
go

create procedure [dbo].[se_get_hierarchy]

as
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	select hierarchy_id, name 
		from hierarchy
		order by name asc
		
end

go
if @@error = 0 print 'PROCEDURE: se_get_hierarchy created'
else print 'PROCEDURE: se_get_hierarchy error on creation'
go
