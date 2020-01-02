


if exists (select * from sysobjects where name = 'se_send_proposed')
begin
	drop procedure se_send_proposed
	print 'PROCEDURE: se_send_proposed dropped'
end
go



create procedure [dbo].[se_send_proposed]--se_send_proposed 11576,572427,189
(       
 @account_id       numeric(10),
 @order_id             numeric(10),
 @current_user numeric(10)
)      
as   
       declare 
       @block_id            numeric(10) = null,
       @blocking_code               numeric(10) = null,
       @trade_desk_id             numeric(10) = 20218,
       @send_order_run_id      numeric(10),
       @rte_rule_id          numeric(10) = null, 
       @force_new_block         numeric(10) = null,
       @force_trader_desk      numeric(10) = null,
       @current_exchange_date     datetime = null, 
       @last_order                  tinyint = 1  ,
       @ret_val int,
       @block_id_local                          numeric(10),
       @sp_initial_trancount int,
       @run_id                 numeric(10),
       @total_order            numeric(10),
       @order_index            numeric(10) =1,
       @last_flag              int =0;
begin
       
            -- get last run_id 
                   
                      exec get_send_order_run_id @run_id output;
                      select @last_flag =1 ;
                                       
                      execute @ret_val = create_order_from_prop_intern 
                                     @order_id = @order_id,
                                     @block_id = @block_id,
                                     @current_user = @current_user,
                                     @blocking_code = @blocking_code,
                                     @trade_desk_id = @trade_desk_id,
                                     @rte_rule_id = @rte_rule_id,
                                     @force_new_block = @force_new_block,
                                     @force_trader_desk = @force_trader_desk,
                                     @current_exchange_date = @current_exchange_date,
                                     @last_order = @last_flag,
                                     @send_order_run_id = @run_id 
                                                                                                                ;
                                     if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
                                         begin
                                               if @sp_initial_trancount = 0
                                                              rollback transaction;
                                                      return @ret_val;    
                                               end;
end;

