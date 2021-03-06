if exists (select * from sysobjects where name = 'create_orders_from_position_faster')
begin
	drop procedure create_orders_from_position_faster
	print 'PROCEDURE: create_orders_from_position_faster dropped'
end
go
create procedure [dbo].[create_orders_from_position_faster]
(	@order_counter 			int output, 
	@account_id 			numeric(10), 
	@position_type_code 	tinyint, 
	@security_id 			numeric(10),
	@send_order_run_id      numeric(10),		
	@pass_warnings			tinyint = 0,
	@strategy_id			numeric(10) = null,
	@current_user			numeric(10),
	@current_exchange_date	datetime = null,	
	@last_order             tinyint = 1
) 
as
	declare @ret_val int;
	declare @passed							tinyint;
	declare @warning						tinyint;
	declare @max_severity					tinyint;
	declare @loop_order_id					numeric(10);
	declare @continue_flag					bit;
	declare @cps_create_orders_from_posit	nvarchar(30);
	declare @cpe_create_orders_from_posit	nvarchar(30);
	declare @block_id						numeric(10);
	declare @local_did_not_send				tinyint;
	declare @auto_desk_id					numeric(10);	
	declare @worst_ret_val				    int;
    declare @number_of_orders               int;
    declare @loop_counter                   int;
	declare @last_order_flag                tinyint;
	declare @override_check					int;
	declare @restriction_level				tinyint;	
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,-1);

	create table #order_ids  	(  		order_id numeric(10) null  	);
	create table #order_scratch      (      order_scratch_id							numeric(10) identity,      block_id                                    numeric(10) NULL,      order_id									numeric(10),      account_id									numeric(10),      security_id                                 numeric(10),      side_code									tinyint NULL,      ticket_type_code							tinyint NULL,      quantity									float NULL,      tax_lot_cost								float NULL,      tax_lot_quantity							float NULL,      accrued_income								float NULL,      time_in_force_code							tinyint NULL,      limit_type_code								tinyint NULL,      limit_price_1								float NULL,      limit_price_2								float NULL,      trader_id                                   numeric(10) NULL,      directed_broker_id                          numeric(10) NULL,      settlement_date								datetime  NULL,      note										nvarchar(255) NULL, 	display_currency_id							numeric(10) NULL,  	originated_by								smallint    NULL,  	originated_time								datetime    NULL,  	baseline_price_tag_type						int    NULL,  	program_id                                  numeric(10) NULL,  	index_id									numeric(10) NULL,  	index_price_tag_type						int    NULL,  	lending_account_id                          numeric(10) NULL,  	user_field_1								float NULL,  	user_field_2								float NULL,  	user_field_3								float NULL,  	user_field_4								float NULL,  	user_field_5								nvarchar(40) NULL,  	user_field_6								nvarchar(40) NULL,  	user_field_7								nvarchar(40) NULL,  	user_field_8								nvarchar(40) NULL,  	trade_reason_id								smallint    NULL,  	compliance_check							tinyint NULL,  	manual_accrued_flag							int    NULL,  	accrued_days								int    NULL,  	strategy_id                                 numeric(10) NULL,  	link_id										numeric(10) NULL,  	contingent_id                               numeric(10) NULL,  	blocking_code                               numeric(10) NULL,  	rte_rule_id                                 numeric(10) NULL,  	user_field_9								float NULL,  	user_field_10								float NULL,  	user_field_11								float NULL,  	user_field_12								float NULL,  	user_field_13								nvarchar(40) NULL,  	user_field_14								nvarchar(40) NULL,  	user_field_15								nvarchar(40) NULL,  	user_field_16								nvarchar(40) NULL,  	directed_counter_party_id                   numeric(10) NULL,  	tax_lot_relief_method_code					tinyint NULL,  	delivery_date								datetime    NULL,  	book_id										numeric(10) NULL,  	force_new_block								numeric(10, 0) NULL,  	contract_id                                 numeric(10) NULL,  	orig_order_id                               numeric(10) NULL,  	approved_flag								tinyint NULL,  	quantity_estimated							float NULL,  	user_field_17								float NULL,  	user_field_18								float NULL,  	user_field_19								float NULL,  	user_field_20								float NULL,  	user_field_21								nvarchar(40) NULL,  	user_field_22								nvarchar(40) NULL,  	user_field_23								nvarchar(40) NULL,  	user_field_24								nvarchar(40) NULL,  	user_field_25								float NULL,  	user_field_26								float NULL,  	user_field_27								float NULL,  	user_field_28								float NULL,  	user_field_29								nvarchar(40) NULL,  	user_field_30								nvarchar(40) NULL,  	user_field_31								nvarchar(40) NULL,  	user_field_32								nvarchar(40) NULL,  	swt_assign_unwind_type						tinyint NULL,  	swt_pay_leg_accrued_interest				float NULL,  	swt_pay_leg_accrued_days					smallint    NULL,  	swt_rec_leg_accrued_interest				float NULL,  	swt_rec_leg_accrued_days					smallint    NULL,  	package_id                                  numeric(10) NULL,  	fcm_entity_id                               numeric(10) NULL,  	sef_entity_id                               numeric(10) NULL,  	ccp_entity_id                               numeric(10) NULL,  	dco_entity_id                               numeric(10) NULL,  	margin_by_account							float NULL,  	manual_settlement_date_flag					tinyint NULL,  	trade_date_offset							tinyint NULL,  	trade_date									datetime    NULL,  	last_order									tinyint NULL,  	major_asset_code							smallint NULL,  	security_symbol								nvarchar(40) NULL,  	security_deleted                            bit NULL,  	security_price_latest                       float NULL,  	security_price_closing                      float NULL,  	security_price_opening                      float NULL,  	security_exchange_rate                      float NULL,  	exchange_rate                               float NULL,  	average_price_precision                     smallint NULL,  	quantity_rounding                           smallint NULL,  	market_value_rounding                       smallint NULL,  	security_level_code                         tinyint NULL,  	default_trader_id                           numeric(10) NULL,  	default_time_in_force_code                  tinyint NULL,  	use_time_in_force_code                      tinyint NULL,  	broker_unlisted_override                    bit NULL,  	default_note                                nvarchar(255) NULL,  	major_account_code                          tinyint NULL,  	domiciled_flag                              tinyint NULL,  	account_manager                             numeric(10) NULL,  	account_regulation_code                     smallint NULL,  	logical_block_id                            numeric(10) NULL,  	market_value                                float NULL,  	tax_lot_market_value                        float NULL,  	equivalent_quantity                         float NULL,  	index_price                                 float NULL,  	note_indicator                              bit NULL,  	baseline_price                              float NULL,  	has_manual_settle_date                      bit NULL,  	violation_indicator                         bit NULL,  	exchange_code                               numeric(10) NULL,  	broker_type_code                            numeric(10) NULL,  	broker_mnemonic                             nvarchar(8) NULL,  	send_order_run_id                           numeric(10),  	decision_maker_id							numeric(10) NULL,
	auto_router tinyint   );
	create  index idx01_order_scratch on #order_scratch(order_id); 
	create  index idx02_order_scratch on #order_scratch(auto_router); 
	create  index idx03_order_scratch on #order_scratch(block_id); 
	create  index idx04_order_scratch on #order_scratch(major_asset_code,blocking_code); 
	select @continue_flag = 1
select @cps_create_orders_from_posit = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cps_create_orders_from_posit'
	and sysobjects.type = 'P'
if @cps_create_orders_from_posit is not null
begin
	execute @ret_val = cps_create_orders_from_posit
		@continue_flag output, @order_counter output,
		@account_id,
		@position_type_code,
		@security_id,
		@send_order_run_id,
		@pass_warnings,
		@strategy_id,
		@current_user,
		@current_exchange_date,
		@last_order
	if (@ret_val != 0 and @ret_val < 60000) or @continue_flag = 0
	begin
		return @ret_val
	end
end



	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,0);


	select @order_counter = 0;
	select @loop_counter = 0;
	select @passed = 0;
	select @warning = 1;
	select @worst_ret_val = 0;
	if @pass_warnings = 1
	begin
		select @max_severity = @warning;
	end else begin
		select @max_severity = @passed; 
	end;
   insert 
	 into #order_scratch
	(
		block_id 	,
		order_id 	,
		account_id 	,
		security_id 	,
		side_code 	,
		ticket_type_code 	,
		quantity 	,
		tax_lot_cost 	,
		tax_lot_quantity 	,
		accrued_income 	,
		time_in_force_code 	,
		limit_type_code 	,
		limit_price_1 	,
		limit_price_2 	,
		trader_id 	,
		directed_broker_id 	,
		settlement_date 	,
		note 	,
		display_currency_id 	,
		originated_by 	,
		originated_time 	,
		baseline_price_tag_type	,
		program_id 	,
		index_id 	,
		index_price_tag_type 	,
		lending_account_id 	,
		user_field_1 	,
		user_field_2 	,
		user_field_3 	,
		user_field_4 	,
		user_field_5 	,
		user_field_6 	,
		user_field_7 	,
		user_field_8 	,
		trade_reason_id 	,
		compliance_check 	,
		manual_accrued_flag	,
		accrued_days	,
		strategy_id	,
		link_id	,
		contingent_id  	,
		blocking_code	,
		rte_rule_id	,
		user_field_9 	,
		user_field_10 	,
		user_field_11 	,
		user_field_12 	,
		user_field_13 	,
		user_field_14 	,
		user_field_15 	,
		user_field_16 	,
		directed_counter_party_id	,
		tax_lot_relief_method_code	,
		delivery_date	,
		book_id	,
		force_new_block	,
		contract_id	,
		orig_order_id	,
		approved_flag	,
		quantity_estimated	,
		user_field_17 	,
		user_field_18 	,
		user_field_19 	,
		user_field_20 	,
		user_field_21 	,
		user_field_22 	,
		user_field_23 	,
		user_field_24 	,
		user_field_25	,
		user_field_26 	,
		user_field_27 	,
		user_field_28 	,
		user_field_29 	,
		user_field_30 	,
		user_field_31 	,
		user_field_32 	,
		swt_assign_unwind_type	,
		swt_pay_leg_accrued_interest	,
		swt_pay_leg_accrued_days	,
		swt_rec_leg_accrued_interest	,
		swt_rec_leg_accrued_days	,
		package_id	,
		fcm_entity_id	,
		sef_entity_id	,
		ccp_entity_id	,
		dco_entity_id	,
		margin_by_account	,
		manual_settlement_date_flag	,
		trade_date_offset	,
		trade_date	,
		last_order	,
		send_order_run_id ,
		decision_maker_id
	)
	select 
		null	block_id ,
		order_id 	,
		account_id 	,
		security_id 	,
		side_code 	,
		ticket_type_code 	,
		quantity 	,
		tax_lot_cost 	,
		tax_lot_quantity 	,
		accrued_income 	,
		time_in_force_code 	,
		limit_type_code 	,
		limit_price_1 	,
		limit_price_2 	,
		trader_id 	,
		directed_broker_id 	,
		settlement_date 	,
		note 	,
		null display_currency_id 	,
		 proposed_orders.created_by originated_by 	,
		proposed_orders.created_time originated_time 	,
		baseline_price_tag_type	,
		program_id 	,
		index_id 	,
		index_price_tag_type 	,
		lending_account_id 	,
		user_field_1 	,
		user_field_2 	,
		user_field_3 	,
		user_field_4 	,
		user_field_5 	,
		user_field_6 	,
		user_field_7 	,
		user_field_8 	,
		trade_reason_id 	,
		compliance_check 	,
		manual_accrued_flag	,
		accrued_days	,
		strategy_id	,
		rebal_session_id	,--link
		contingent_id  	,
		null blocking_code	,
		rte_rule_id	,
		user_field_9 	,
		user_field_10 	,
		user_field_11 	,
		user_field_12 	,
		user_field_13 	,
		user_field_14 	,
		user_field_15 	,
		user_field_16 	,
		directed_counter_party_id	,
		tax_lot_relief_method_code	,
		delivery_date	,
		book_id	,
		null force_new_block	,
		contract_id	,
		orig_order_id	,
		approved_flag	,
		quantity_estimated	,
		user_field_17 	,
		user_field_18 	,
		user_field_19 	,
		user_field_20 	,
		user_field_21 	,
		user_field_22 	,
		user_field_23 	,
		user_field_24 	,
		user_field_25	,
		user_field_26 	,
		user_field_27 	,
		user_field_28 	,
		user_field_29 	,
		user_field_30 	,
		user_field_31 	,
		user_field_32 	,
		swt_assign_unwind_type	,
		swt_pay_leg_accrued_interest	,
		swt_pay_leg_accrued_days	,
		swt_rec_leg_accrued_interest	,
		swt_rec_leg_accrued_days	,
		null package_id	,
		fcm_entity_id	,
		sef_entity_id	,
		ccp_entity_id	,
		dco_entity_id	,
		margin_by_account	,
		manual_settlement_date_flag	,
		trade_date_offset	,
		null trade_date	,
		1 last_order	,
		@send_order_run_id,
		decision_maker_id
		from
			proposed_orders, account_hierarchy_map
		where account_hierarchy_map.parent_id = @account_id 
			and	account_hierarchy_map.child_id = proposed_orders.account_id 
			and proposed_orders.position_type_code = @position_type_code 
			and proposed_orders.security_id = @security_id 
			and proposed_orders.is_pre_executed = 0;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,1);



 		execute @ret_val = add_blocked_order_faster  
												@send_order_run_id  = @send_order_run_id,
												@current_user = @current_user
												;
	select @cpe_create_orders_from_posit = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cpe_create_orders_from_posit'
	and sysobjects.type = 'P'
if @cpe_create_orders_from_posit is not null
begin
	execute @ret_val = cpe_create_orders_from_posit
		@order_counter output,
		@account_id,
		@position_type_code,
		@security_id,
		@send_order_run_id,
		@pass_warnings,
		@strategy_id,
		@current_user,
		@current_exchange_date,
		@last_order
	if (@ret_val != 0 and @ret_val < 60000)
	begin
		return @ret_val
	end
end
	return @worst_ret_val;
end


go
if @@error = 0 print 'PROCEDURE: create_orders_from_position_faster created'
else print 'PROCEDURE: create_orders_from_position_faster error on creation'
go