
if exists (select * from sysobjects where name = 'se_update_committment_price')
begin
	drop procedure se_update_committment_price
	print 'PROCEDURE: se_get_tradervspm_orders dropped'
end
go


create procedure [dbo].[se_update_committment_price]--se_update_committment_price 28
(     
 @block_id                numeric(10),
 @price                   float,
 @user_id				  numeric(10)
 )    
as 

update blocked_orders
set user_field_1 = @price,
modified_by = @user_id
where block_id = @block_id



END
go
if @@error = 0 print 'PROCEDURE: se_update_committment_price created'
else print 'PROCEDURE: se_update_committment_price error on creation'
go
