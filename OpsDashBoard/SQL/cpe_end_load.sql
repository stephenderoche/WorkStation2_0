if exists (select * from sysobjects where name = 'cpe_end_load')
begin
	drop procedure cpe_end_load
	print 'PROCEDURE: cpe_end_load dropped'
end
go

create procedure [dbo].[cpe_end_load]
(
@return_messages tinyint = 1,
@current_user numeric(10) = null
)
as
begin
                        set nocount on;
 DECLARE @ret_val int    
 Declare @startDate dateTime
 declare @Load numeric(10) 
 declare @loadMnemonic varchar(40)
 declare @FileName varchar(255) 
 declare @File_Exists int

 WAITFOR DELAY '00:00:02';

 insert into download_messages_history
	(
		load_name,
		load_id,
		load_error_code,
		record_name,
		error_text,
		error_level,
		message_time
	)
	select
		load_name,
		load_id,
		load_error_code,
		record_name,
		error_text,
		error_level,
		FORMAT(message_time , 'MM/dd/yyyy HH:mm:ss')
	from  download_messages

	select @startDate = value from registry where entry = 'load start'


	select @Load = min(load_master.load_master_id)    
		from load_master   
		where enable = 1
--begin while  
  
while @Load is not null    
	begin   
	select @loadMnemonic = mnemonic from load_master where load_master_id = @Load
	SELECT @FileName=  load_master.filename from  load_master where load_master_id = @Load
      EXEC Master..xp_fileexist @FileName, @File_Exists OUT 

Select @File_Exists 



	if not exists(
		select 1 
		from download_messages_history where load_name = @loadMnemonic
		and message_time > @startDate
		
	) 
	begin
	        if (@File_Exists=1)
			begin
			insert into download_messages_history values
			(
				@loadMnemonic,
				1,
				0,
				'No Errors',
				'No errors',
				0,
				@startDate
			)
			end
			else
			begin

				insert into download_messages_history values
				(
					@loadMnemonic,
					1,
					4,
					'No File',
					'No File',
					4,
					@startDate
				)
	        end

	end
 --if (@File_Exists = 0)
		
	--begin

	--		insert into download_messages_history values
	--		(
	--			@loadMnemonic,
	--			1,
	--			4,
	--			'No File',
	--			'No File',
	--			0,
	--			@startDate
	--		)
	--end

	
	
 select @Load = min(load_master.load_master_id)    
from load_master    
where load_master.load_master_id >@Load  
and enable = 1
end


end 




go
if @@error = 0 print 'PROCEDURE: cpe_end_load created'
else print 'PROCEDURE: cpe_end_load error on creation'
go




