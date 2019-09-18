

if exists (select * from sysobjects where name = 'se_update_journaling')
begin
       drop procedure se_update_journaling
       print 'PROCEDURE: se_update_journaling dropped'
end
go

create procedure se_update_journaling  --se_update_journaling 34882,'test',189

(  
	@block_id numeric(10) = null,
	@journal_entry varchar(100),
	@user_id numeric(10)
	
) 

as

declare @name varchar(40)

begin

                      
	select @name = name from user_info where user_id = @user_id
	

	insert into se_journaling values(@block_id,getdate(),@journal_entry,@name)
 
		

end;

go


           

if @@error = 0 print 'PROCEDURE: se_update_journaling created'
else print 'PROCEDURE: se_update_journaling error on creation'
go
	
