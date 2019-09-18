
if exists (select * from sysobjects where name = 'se_get_generic_param')
begin
	drop procedure se_get_generic_param
	print 'PROCEDURE: se_get_generic_param dropped'
end
go


create procedure [dbo].[se_get_generic_param]--se_get_generic_param 'All'
(    

 @report                 Varchar(40) = 'All'


 )    
as 

begin	

  if @report  = 'All'
	    begin
		    select @report  = ''
		end
   

select * from se_generic_report where report = @report or @report = ''



END
go
if @@error = 0 print 'PROCEDURE: se_get_generic_param created'
else print 'PROCEDURE: se_get_generic_param error on creation'
go

