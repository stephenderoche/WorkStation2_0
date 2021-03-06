USE [LVTS_753]
GO
/****** Object:  StoredProcedure [dbo].[cpe_create_order_from_prop1]    Script Date: 4/5/2019 10:27:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
alter procedure [dbo].[cpe_create_order_from_prop]
	

/**************************************************************************************************
* LongView Trading System - NBIM Custom Code
***************************************************************************************************
* NAME             : cps_create_order_from_prop
* TYPE             : custom hook
* CALLED FROM      : create_order_from_proposed
*
* FUNCTIONAL AREAS : 1. updates user_field 21 with block_id for generics
*                  
***************************************************************************************************/	

    @order_id		numeric(10),
	@block_id		numeric(10) = null,
	@current_user   numeric(10),
	@blocking_code	        numeric(10) = null,
	@trade_desk_id       	numeric(10),
	@send_order_run_id      numeric(10),
	@rte_rule_id	        numeric(10) = null,	
	@force_new_block	    numeric(10) = null,
	@force_trader_desk      numeric(10) = null,
	@current_exchange_date	datetime = null, 
	@last_order		        tinyint = 1 ,
	@restriction_level		numeric(10) = 0    

as
set nocount on
 
DECLARE @account_id numeric(10, 0),
        @ret_val int,
        @isGeneric numeric(10),
		@note varchar(40)



    update blocked_orders
	set blocked_orders.user_field_1 = 
	(select Price.latest 
	   from price 
	        where security_id = (select distinct security_id 
	                             from orders 
						         where blocked_orders.block_id= orders.block_id
								 and orders.security_id = price.security_id)
								 ),	
	
	modified_by = 198
	where blocked_orders.send_order_run_id = @send_order_run_id
	and @last_order = 1





return 0
