if exists (select * from sysobjects where name = 'se_get_gauge_type')
begin
	drop procedure se_get_gauge_type
	print 'procedure: se_get_gauge_type dropped'
end
go

create procedure [dbo].[se_get_gauge_type] --se_get_gauge_type

as
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;

select *  from se_gauge_type

	
end



go
if @@error = 0 print 'PROCEDURE: se_get_gauge_type created'
else print 'PROCEDURE: se_get_gauge_type error on creation'
go

