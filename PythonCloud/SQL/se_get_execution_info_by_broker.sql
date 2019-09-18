if exists (select * from sysobjects where name = 'se_get_execution_info_by_broker')
begin
	drop procedure se_get_execution_info_by_broker
	print 'PROCEDURE: se_get_execution_info_by_broker dropped'
end
go

create procedure [dbo].[se_get_execution_info_by_broker]--se_get_execution_info_by_broker 40659,116
(       
 @block_id      numeric(10) = -1,
 @broker_id      numeric(10) = -1
 )      
as   
begin


select 
ticket.execution_broker_id,
round(ticket.average_price_executed,4) as 'ticketPrice',
ticket.trade_date as date

from broker_ticket 
 join broker on
 broker.broker_id = broker_ticket.execution_broker_id
 join ticket on
 ticket.broker_ticket_id = broker_ticket.broker_ticket_id
 where broker_ticket.block_id =@block_id
 and (broker_id = @broker_id or @broker_id = -1)








 end
 
go
if @@error = 0 print 'PROCEDURE: se_get_execution_info_by_broker created'
else print 'PROCEDURE: se_get_execution_info_by_broker error on creation'
go