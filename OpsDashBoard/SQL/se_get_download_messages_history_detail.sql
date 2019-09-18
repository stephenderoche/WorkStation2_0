if exists (select * from sysobjects where name = 'se_get_download_messages_history_detail')
begin
	drop procedure se_get_download_messages_history_detail
	print 'PROCEDURE: se_get_download_messages_history_detail dropped'
end
go
	

create procedure [dbo].[se_get_download_messages_history_detail] --se_get_download_messages_history_detail '3/18/2019 5:57:30 PM','POSITION'

(
@message_time DateTime,
@mnemonic varchar(40)
)
as
begin
     set nocount on;

select record_name,error_text,error_level,@mnemonic as 'load name',
case
     when error_level = 0 then 'Pass'
	  when error_level =1 then 'Warning'
	   when error_level =2 then 'Fail'
	   else 'No File/Not Run'
end as 'Level Mnemonic'
 from download_messages_history
where message_time = @message_time and load_name = @mnemonic
 

end 




go
if @@error = 0 print 'PROCEDURE: se_get_download_messages_history_detail created'
else print 'PROCEDURE: se_get_download_messages_history_detail error on creation'
go




