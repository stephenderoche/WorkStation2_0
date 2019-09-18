if exists (select * from sysobjects where name = 'se_get_execution_info')
begin
	drop procedure se_get_execution_info
	print 'PROCEDURE: se_get_execution_info dropped'
end
go

create procedure [dbo].[se_get_execution_info]--se_get_execution_info 40660
(       
 @block_id      numeric(10) = -1
 )      
as   
begin
select 
@block_id as block_id,
broker.broker_id,
broker.mnemonic as name,
broker_ticket.quantity_executed,
round(broker_ticket.total_average_price_executed,4) as 'AveragePrice'
from broker_ticket 
join broker on
broker.broker_id = broker_ticket.execution_broker_id
where broker_ticket.block_id =@block_id
and broker_ticket.deleted = 0

select 
@block_id as block_id,
broker.broker_id,
broker.mnemonic,
broker_ticket.quantity_executed,
round(broker_ticket.average_price_executed,4) as 'Broker Average Price',
round(broker_ticket.total_average_price_executed,4) as 'Total Broker Average Price',
round(ticket.average_price_executed,4) as 'ticket Average Price',
ticket.quantity_executed as 'Ticket Quantity Executed',
format(ticket.trade_date, 'yyyy-MM-dd') as trade_date,
format(ticket.settlement_date, 'yyyy-MM-dd') as settlement_date
from broker_ticket 
join broker on
broker.broker_id = broker_ticket.execution_broker_id
join ticket on
 ticket.broker_ticket_id = broker_ticket.broker_ticket_id
 where broker_ticket.block_id =@block_id
 and broker_ticket.deleted = 0
 and ticket.deleted = 0


 end
 
go
if @@error = 0 print 'PROCEDURE: se_get_execution_info created'
else print 'PROCEDURE: se_get_execution_info error on creation'
go