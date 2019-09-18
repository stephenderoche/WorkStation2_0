  --select * from proposed_orders
if exists (select * from sysobjects where name = 'se_get_bb_data')
begin
	drop procedure se_get_bb_data
	print 'PROCEDURE: se_get_bb_data dropped'
end
go



create procedure [dbo].[se_get_bb_data]  --se_get_bb_data 199
(    
 @account_id numeric(10)
)     

as   



begin    

select
positions.quantity as Quantity, 
se_bb_Data.*

from se_bb_Data
join positions on
positions.account_id = @account_id
and positions.security_id = se_bb_Data.security_id
where positions.account_id = @account_id
and se_bb_Data.security_id > 1


end  


go
if @@error = 0 print 'PROCEDURE: se_get_bb_data created'
else print 'PROCEDURE: se_get_bb_data error on creation'
go