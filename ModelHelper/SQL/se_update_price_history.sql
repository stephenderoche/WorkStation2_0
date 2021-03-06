if exists (select * from sysobjects where name = 'se_update_price_history')
begin
       drop procedure se_update_price_history
       print 'PROCEDURE: se_update_price_history dropped'
end
go

ALTER procedure [dbo].[se_update_price_history]  --se_update_price_journal 162905,5927,'test',189

(  
	@model_id numeric(10) = -1,
	@security_id numeric(10) = -1,
	@entry varchar(100),
	@current_user numeric(10)
	
) 

as

declare @name varchar(40)

begin

                      
	select @name = name from user_info where user_id = @current_user
	

	insert into se_security_journal values(@model_id,@security_id,getdate(),@entry,@name)
 
		

end;
go


if @@error = 0 print 'PROCEDURE: se_update_price_history created'
else print 'PROCEDURE: se_update_price_history error on creation'
go
	
