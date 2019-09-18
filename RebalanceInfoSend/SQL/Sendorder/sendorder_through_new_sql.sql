
declare
    @order_counter 			int , 
	@account_id 			numeric(10), 
	@position_type_code 	tinyint, 
	@security_id 			numeric(10),
	@send_order_run_id      numeric(10),		
	@pass_warnings			tinyint = 0,
	@strategy_id			numeric(10) = null,
	@current_user			numeric(10),
	@current_exchange_date	datetime = null,	
	@last_order             tinyint = 1,
	@retval                 tinyint;
begin
/*
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =178,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =179,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =180,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =181,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =185,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =187,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =188,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =189,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =198,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =200,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =201,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =202,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =204,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =205,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =206,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =220,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =221,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =223,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =224,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =225,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
*/
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =227,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =228,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =229,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =230,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =231,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =232,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =233,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =235,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =236,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =238,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =239,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =240,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =241,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =242,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =245,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =246,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =193,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =194,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =196,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =208,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =209,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =210,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =213,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =215,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =216,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =248,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =249,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =250,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =251,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =252,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1
exec get_send_order_run_id @send_order_run_id output;
exec create_orders_from_position_faster @order_counter output, @account_id = 125003,@position_type_code =0, @security_id =183,@send_order_run_id =@send_order_run_id, @pass_warnings=0, @strategy_id= null,@current_user=1,@current_exchange_date	=null,	@last_order= 1

end;

