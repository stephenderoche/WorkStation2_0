  --select * from proposed_orders



if exists (select * from sysobjects where name = 'se_update_bb_data')
begin
	drop procedure se_update_bb_data
	print 'PROCEDURE: se_update_bb_data dropped'
end
go



create procedure [dbo].[se_update_bb_data]  --se_update_bb_data 199
(    
 @account_id numeric(10)
)  
as 
declare @registryValue numeric(10)
declare @file varchar(40)
begin

select @registryValue = value from registry where entry = 'Random'
truncate table se_bb_Data

if (@registryValue = 0)
   Begin 
		 bulk insert se_bb_Data
		 from 'C:\Temp\ensign\BB intergration\bb_analytics_a.csv' 
		 with (formatfile = 'C:\Temp\ensign\BB intergration\bb_data.fmt')
   end
else
    Begin 
		bulk insert se_bb_Data
		from 'C:\Temp\ensign\BB intergration\bb_analytics_b.csv' 
		with (formatfile = 'C:\Temp\ensign\BB intergration\bb_data.fmt')
   end




UPDATE se_bb_Data
SET se_bb_Data.security_id = security.security_id
FROM se_bb_Data
INNER JOIN security 
    ON se_bb_Data.symbol = security.user_id_2

if (@registryValue = 0)
   Begin 
     update registry set value = 1 where entry = 'Random'
   end
else
    Begin 
      update registry set value = 0 where entry = 'Random'
   end



end

go
if @@error = 0 print 'PROCEDURE: se_update_bb_data created'
else print 'PROCEDURE: se_update_bb_data on creation'
go