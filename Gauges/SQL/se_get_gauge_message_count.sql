if exists (select * from sysobjects where name = 'se_get_gauge_message_count')
begin
	drop procedure se_get_gauge_message_count
	print 'PROCEDURE: se_get_gauge_message_count dropped'
end
go

create procedure [dbo].[se_get_gauge_message_count] --se_get_gauge_message_count  'Drinks before Nic Loses Time','All'
(
    @guagetype varchar(40),
	@custodian varchar(40)
)

as
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;

if (@guagetype = 'Number of Messages')
begin

      select count(*) as RecordNumber from message 
	  where header = 1
	  and  (custodian = @custodian or @custodian = 'ALL')
end

if (@guagetype = 'Number of Messages not Acknowledged')
begin

      select count(*) as RecordNumber from message 
	  where header = 1 
	  and (acknowledge = 0 or acknowledge is null)
	  and  (custodian = @custodian or @custodian = 'ALL')
end

if (@guagetype = 'Drinks before Nic Loses Time')
begin

      select 6
end

	
end

go
if @@error = 0 print 'PROCEDURE: se_get_gauge_message_count created'
else print 'PROCEDURE: se_get_gauge_message_count error on creation'
go