if exists (select * from sysobjects where name = 'se_get_account_id')
begin
	drop procedure se_get_account_id
	print 'PROCEDURE: se_get_account_id dropped'
end
go


create procedure [dbo].[se_get_account_id]--se_get_account_id 'mom1'
(     
 @AccountName      varchar(40)
 )    
as 

   declare @ret_val	int;
   Declare @sub_model_id numeric(10)
   declare @MInAccountID numeric(10); 
begin	
  
select account_id from account where short_name = @AccountName

END

go
if @@error = 0 print 'PROCEDURE: se_get_account_id created'
else print 'PROCEDURE: se_get_account_id error on creation'
go
