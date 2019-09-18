if exists (select * from sysobjects where name = 'se_get_dow')
begin
	drop procedure se_get_download_messages_history
	print 'PROCEDURE: se_get_download_messages_history dropped'
end
go

create procedure [dbo].[se_get_download_messages_history] --se_get_download_messages_history '3/17/2019 12:00:00 AM',1

(
@date DateTime,
@current bit
)
as
begin
     set nocount on;



if (@current = 0)
begin
Select 
distinct message_time,
mnemonic as 'Load Name',
error_level as 'Error Level',
case
     when error_level = 0 then 'Pass'
	  when error_level =1 then 'Warning'
	   when error_level =2 then 'Fail'
	   else 'No File/Not Run'
end as 'Level Mnemonic',
load_master.sequence

from load_master 
left join download_messages_history on
download_messages_history.load_name = load_master.mnemonic
where enable =1
and message_time between @date and (@date+ 1)

group by message_time,mnemonic,error_level,load_master.sequence
order by mnemonic asc,message_time asc
end
else
begin
Select 
max(message_time) as 'message_time',
mnemonic as 'Load Name',
error_level as 'Error Level',
case
     when error_level = 0 then 'Pass'
	  when error_level =1 then 'Warning'
	   when error_level =2 then 'Fail'
	   else 'No File/Not Run'
end as 'Level Mnemonic',
load_master.sequence
from load_master 
left join download_messages_history on
download_messages_history.load_name = load_master.mnemonic
and FORMAT(message_time , 'MM/dd/yyyy HH:mm:ss') between FORMAT(@date , 'MM/dd/yyyy HH:mm:ss') and FORMAT((@date+1) , 'MM/dd/yyyy HH:mm:ss')
where enable = 1
group by mnemonic,error_level,load_master.sequence


end
 

end 




go
if @@error = 0 print 'PROCEDURE: se_get_download_messages_history created'
else print 'PROCEDURE: se_get_download_messages_history error on creation'
go




