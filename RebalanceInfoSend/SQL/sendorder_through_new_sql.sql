if exists (select * from sysobjects where name = 'se_send_orders_faster')
begin
	drop procedure se_send_orders_faster
	print 'PROCEDURE: se_send_orders_faster dropped'
end
go


create procedure  [dbo].[se_send_orders_faster] --se_send_orders_faster 10349,1094
(
 @account_id numeric(10) = -1,
 @security_id numeric(10) 
)

as

declare
    @order_counter 			int , 
	@position_type_code 	tinyint, 
	@send_order_run_id      numeric(10),		
	@pass_warnings			tinyint = 0,
	@strategy_id			numeric(10) = null,
	@current_user			numeric(10),
	@current_exchange_date	datetime = null,	
	@last_order             tinyint = 1,
	@retval                 tinyint;
begin

exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = @account_id ,@position_type_code =0, @security_id =@security_id,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

--select * from desk

end;



