if exists (select * from sysobjects where name = 'cps_start_load')
begin
	drop procedure cps_start_load
	print 'PROCEDURE: cps_start_load dropped'
end
go

create procedure [dbo].[cps_start_load]
(
	@continue_flag     bit out,
	@current_user numeric(10)
)
as
	
begin
                        set nocount on;

 DECLARE @ret_val int    
 Select @continue_flag = 1

 	if exists(
		select 1 
		from registry where entry = 'load start'
	) 
	
begin

update registry
set value = FORMAT(GETDATE() , 'MM/dd/yyyy HH:mm:ss')
where entry = 'load start'
end
else

begin
  insert into registry values ('LOAD','load start',FORMAT(GETDATE() , 'MM/dd/yyyy HH:mm:ss') )
end


END

go
if @@error = 0 print 'PROCEDURE: cps_start_load created'
else print 'PROCEDURE: cps_start_load error on creation'
go




