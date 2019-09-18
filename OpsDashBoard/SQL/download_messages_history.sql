if exists (select * from sysobjects where name = 'download_messages_history')
begin
	drop table download_messages_history
	print 'TABLE: download_messages_history dropped'
end
go
create table download_messages_history
(
	load_name nvarchar(40) not null,
	load_id numeric(10) not null,
	load_error_code int not null,
	record_name nvarchar(255) null,
	error_text nvarchar(255) not null,
	error_level tinyint not null,
	message_time datetime not null,
	download_messages_history_id numeric(10) identity not null,
	constraint download_messages_history_pk primary key (download_messages_history_id)
);
go
if @@ERROR = 0 print 'TABLE: download_messages_history created'
else print 'TABLE: download_messages_history error on creation'
go


--select * from download_messages_history
--select * from download_messages
--select * from registry where entry = 'load start'

--truncate table download_messages_history

--select * from load_master where mnemonic like '%pos%'
--select * from download_messages_history


--DECLARE @FileName varchar(255) 
--DECLARE @File_Exists int
--SELECT @FileName='C:\Load\Prices\position1.dat'
--EXEC Master..xp_fileexist @FileName, @File_Exists OUT 
--Select @File_Exists 