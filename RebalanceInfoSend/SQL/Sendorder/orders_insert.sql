 
alter trigger [dbo].[orders_insert] on  [dbo].[orders] for insert as
	declare @block_id 						numeric(10);
	declare @order_id						numeric(10);
	declare @ret_val						int;
	declare @message_count		int;
	declare @general_error					int;
	declare @auditing_switch				tinyint;
	declare @security_id					numeric(10);
	declare @side_code						tinyint;
	declare @average_price_precision		smallint;
	declare @average_price_rounding			smallint;
	declare @share_indicator				tinyint;
	declare @block_order_counter			int;
	declare @order_counter_delta			int;
	declare @block_closed_order_counter		int;
	declare @closed_order_counter_delta		int;
	declare @block_held_order_counter		int;
	declare @held_order_counter_delta		int;
	declare @block_quantity_ordered			float;
	declare @quantity_delta					float;
	declare @block_quantity_closed			float;
	declare @quantity_closed_delta			float;
	declare @block_quantity_held			float;
	declare @quantity_held_delta			float;
	declare @block_quantity_confirmed		float;
	declare @quantity_confirmed_delta		float;
	declare @block_market_value_ordered		float;
	declare @market_value_ordered_delta		float;
	declare @block_market_value_closed		float;
	declare @market_value_closed_delta		float;
	declare @block_market_value_held		float;
	declare @market_value_held_delta		float;
	declare @block_market_value_confirmed	float;
	declare @market_value_confirmed_delta	float;
	declare @block_accrued_income_ordered	float;
	declare @accrued_income_ordered_delta	float;
	declare @block_quantity_estimated		float;
	declare @quantity_estimated_delta		float;
	declare @block_event_counter			int;
	declare @block_count					int;
	declare @block_deleted					bit;
	declare @deleted						bit;
	declare @archived						bit;
	declare @block_closed					bit;
	declare @closed							bit;
	declare @block_partially_closed			bit;
	declare @block_partially_closed2		bit;
	declare @block_partially_closed3		bit;
	declare @block_held						bit;
	declare @held							bit;
	declare @block_partially_held			bit;
	declare @block_partially_held2			bit;
	declare @block_partially_held3			bit;
	declare @block_violation_indicator		bit;
	declare @violation_indicator			bit;
	declare @block_split_adjusted			bit;
	declare @split_adjusted					bit;
	declare @block_dividend_adjusted		bit;
	declare @dividend_adjusted				bit;
	declare @block_note_indicator			bit;
	declare @block_domiciled_flag			tinyint;
	declare @domiciled_flag					tinyint;
	declare @domiciled_flag_set				tinyint;
	declare @block_directed_broker_id		numeric(10);
	declare @directed_broker_id				numeric(10);
	declare @directed_broker_id_set			tinyint;
	declare @block_counter_party_id			numeric(10);
	declare @directed_counter_party_id		numeric(10);
	declare @directed_counter_party_id_set	tinyint;
	declare @block_limit_type_code			tinyint;
	declare @limit_type_code_ordered		tinyint;
	declare @limit_type_code_ordered_set	tinyint;
	declare @block_time_in_force_code		tinyint;
	declare @time_in_force_code				tinyint;
	declare @time_in_force_code_set			tinyint;
	declare @block_program_id				numeric(10);
	declare @program_id						numeric(10);
	declare @program_id_set					tinyint;
	declare @block_accrued_days				integer;
	declare @accrued_days					integer;
	declare @block_index_id					numeric(10);
	declare @index_id						numeric(10);
	declare @index_id_set					tinyint;
	declare @block_account_manager			numeric(10);
	declare @account_manager				numeric(10);
	declare @account_manager_set			tinyint;
	declare @block_account_reg_code			smallint;
	declare @account_regulation_code		smallint;
	declare @account_regulation_code_set	tinyint;
	declare @block_trade_reason_id			smallint;
	declare @trade_reason_id				smallint;
	declare @trade_reason_id_set			tinyint;
	declare @block_settlement_date			datetime;
	declare @settlement_date				datetime;
	declare @settlement_date_set			tinyint;
	declare @block_account_id				numeric(10);
	declare @account_id						numeric(10);
	declare @account_id_set					tinyint;
	declare @block_strategy_id				numeric(10);
	declare @strategy_id					numeric(10);
	declare @block_major_account_code		tinyint;
	declare @major_account_code				tinyint;
	declare @major_account_code_set			tinyint;
	declare @block_user_field_1				float;
	declare @user_field_1					float;
	declare @user_field_1_set				tinyint;
	declare @block_user_field_2				float;
	declare @user_field_2					float;
	declare @user_field_2_set				tinyint;
	declare @block_user_field_3				float;
	declare @user_field_3					float;
	declare @user_field_3_set				tinyint;
	declare @block_user_field_4				float;
	declare @user_field_4					float;
	declare @user_field_4_set				tinyint;
	declare @block_user_field_5				nvarchar(40);
	declare @user_field_5					nvarchar(40);
	declare @user_field_5_set				tinyint;
	declare @block_user_field_6				nvarchar(40);
	declare @user_field_6					nvarchar(40);
	declare @user_field_6_set				tinyint;
	declare @block_user_field_7				nvarchar(40);
	declare @user_field_7					nvarchar(40);
	declare @user_field_7_set				tinyint;
	declare @block_user_field_8				nvarchar(40);
	declare @user_field_8					nvarchar(40);
	declare @user_field_8_set				tinyint;
	declare @block_user_field_9				float;
	declare @user_field_9					float;
	declare @user_field_9_set				tinyint;
	declare @block_user_field_10			float;
	declare @user_field_10					float;
	declare @user_field_10_set				tinyint;
	declare @block_user_field_11			float;
	declare @user_field_11					float;
	declare @user_field_11_set				tinyint;
	declare @block_user_field_12			float;
	declare @user_field_12					float;
	declare @user_field_12_set				tinyint;
	declare @block_user_field_13			nvarchar(40);
	declare @user_field_13					nvarchar(40);
	declare @user_field_13_set				tinyint;
	declare @block_user_field_14			nvarchar(40);
	declare @user_field_14					nvarchar(40);
	declare @user_field_14_set				tinyint;
	declare @block_user_field_15			nvarchar(40);
	declare @user_field_15					nvarchar(40);
	declare @user_field_15_set				tinyint;
	declare @block_user_field_16			nvarchar(40);
	declare @user_field_16					nvarchar(40);
	declare @user_field_16_set				tinyint;
	declare @block_approved_flag			tinyint;
	declare @approved_flag					tinyint;
	declare @approved_flag_set				tinyint;
	declare @block_delivery_code			smallint;
	declare @delivery_code					smallint;
	declare @block_best_price_1_ordered		float;
	declare @block_best_price_2_ordered		float;
	declare @block_worst_price_1_ordered	float;
	declare @block_worst_price_2_ordered	float;
	declare @limit_price_1					float;
	declare @limit_price_2					float;
	declare @baseline_price					float;
	declare @note_indicator					tinyint;
	declare @is_locking_enabled				int;
	declare @is_account_locked				int;
	declare @proforma_locking_type			int;
	declare @null_count						int;
	declare @unknown_user_id				numeric(10);
	declare @not_specified_user_id			numeric(10);
	declare @unknown_user_name				nvarchar(128);
	declare @current_user					numeric(10);
	declare @block_recalc					tinyint;
	declare	@tran_count						numeric(10);
	declare @block_user_field_17				float;
	declare @user_field_17					float;
	declare @user_field_17_set				tinyint;
	declare @block_user_field_18				float;
	declare @user_field_18					float;
	declare @user_field_18_set				tinyint;
	declare @block_user_field_19				float;
	declare @user_field_19					float;
	declare @user_field_19_set				tinyint;
	declare @block_user_field_20				float;
	declare @user_field_20					float;
	declare @user_field_20_set				tinyint;
	declare @block_user_field_21				nvarchar(40);
	declare @user_field_21					nvarchar(40);
	declare @user_field_21_set				tinyint;
	declare @block_user_field_22				nvarchar(40);
	declare @user_field_22					nvarchar(40);
	declare @user_field_22_set				tinyint;
	declare @block_user_field_23				nvarchar(40);
	declare @user_field_23					nvarchar(40);
	declare @user_field_23_set				tinyint;
	declare @block_user_field_24				nvarchar(40);
	declare @user_field_24					nvarchar(40);
	declare @user_field_24_set				tinyint;
	declare @block_user_field_25				float;
	declare @user_field_25					float;
	declare @user_field_25_set				tinyint;
	declare @block_user_field_26			float;
	declare @user_field_26					float;
	declare @user_field_26_set				tinyint;
	declare @block_user_field_27			float;
	declare @user_field_27					float;
	declare @user_field_27_set				tinyint;
	declare @block_user_field_28			float;
	declare @user_field_28					float;
	declare @user_field_28_set				tinyint;
	declare @block_user_field_29			nvarchar(40);
	declare @user_field_29					nvarchar(40);
	declare @user_field_29_set				tinyint;
	declare @block_user_field_30			nvarchar(40);
	declare @user_field_30					nvarchar(40);
	declare @user_field_30_set				tinyint;
	declare @block_user_field_31			nvarchar(40);
	declare @user_field_31					nvarchar(40);
	declare @user_field_31_set				tinyint;
	declare @block_user_field_32			nvarchar(40);
	declare @user_field_32					nvarchar(40);
	declare @user_field_32_set				tinyint;
	declare @block_swt_assign_unwind_type	tinyint;
	declare @swt_assign_unwind_type			tinyint;
	declare @swt_assign_unwind_type_set		tinyint;
	declare @block_fcm_entity_id			numeric(10);
	declare @fcm_entity_id					numeric(10);
	declare @user_field_1_mv	float;
	declare @user_field_2_mv	float;
	declare @user_field_3_mv	float;
	declare @user_field_4_mv	float;
	declare @user_field_5_mv 	nvarchar(40);
	declare @user_field_6_mv 	nvarchar(40);
	declare @user_field_7_mv 	nvarchar(40);
	declare @user_field_8_mv 	nvarchar(40);
	declare @user_field_9_mv	float;
	declare @user_field_10_mv	float;
	declare @user_field_11_mv	float;
	declare @user_field_12_mv	float;
	declare @user_field_13_mv 	nvarchar(40);
	declare @user_field_14_mv 	nvarchar(40);
	declare @user_field_15_mv 	nvarchar(40);
	declare @user_field_16_mv 	nvarchar(40);
	declare @user_field_17_mv		float;
	declare @user_field_18_mv		float;
	declare @user_field_19_mv		float;
	declare @user_field_20_mv		float;
	declare @user_field_21_mv 	nvarchar(40);
	declare @user_field_22_mv 	nvarchar(40);
	declare @user_field_23_mv 	nvarchar(40);
	declare @user_field_24_mv 	nvarchar(40);
	declare @user_field_25_mv		float;
	declare @user_field_26_mv	float;
	declare @user_field_27_mv	float;
	declare @user_field_28_mv	float;
	declare @user_field_29_mv 	nvarchar(40);
	declare @user_field_30_mv 	nvarchar(40);
	declare @user_field_31_mv 	nvarchar(40);
	declare @user_field_32_mv 	nvarchar(40);
begin
	declare @rows int
	declare @ec__errno int
	select @rows = @@rowcount
	if @rows = 0
	begin
		return
	end

insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
			values( 0, getdate(), getdate(), 0,900);

	--update orders
	--set
	--	quantity_max_approved = case
	--								when inserted.approved_flag = 0 then inserted.quantity_estimated
	--								else inserted.quantity
	--								end,
	--	update_blocked_orders = 0,
	--	modified_by   = inserted.modified_by,
	--	modified_time = getdate()
	--from inserted, security
	--where inserted.order_id = orders.order_id
	-- and orders.security_id = security.security_id
	-- and security.minor_asset_code=12;


	select @not_specified_user_id = null;  															if not update(modified_by)  															begin  																select @not_specified_user_id = user_id,  																	@unknown_user_name = name  																from user_info (nolock)  																where user_id = -1;  																if @not_specified_user_id is null  																begin  																	raiserror(50568, 10, -1,  'orders_insert');  																	rollback  																	return;  																end else begin  																	if @unknown_user_name like '%linedata%'  																	begin  																		raiserror(60113, 10, -1,  'orders_insert');  																	end;  																end;  															end;  															select @null_count = count(*)  															from inserted  															  															where inserted.modified_by is null;  															if @null_count > 0  															begin  																select @unknown_user_id = user_id,  																	@unknown_user_name = name  																from user_info  																where user_id = -1;  																if @unknown_user_id is null  																begin  																	raiserror(50569, 10, -1,  'orders_insert');  																	rollback  																	return;  																end else begin  																	if @unknown_user_name like '%linedata%'  																	begin  																		raiserror(60114, 10, -1,  'orders_insert');  																	end;  																end;  															end;
	select 
		@current_user = coalesce(@not_specified_user_id, max(inserted.modified_by), @unknown_user_id)
	from inserted
	;
	select
		top (1) 
		@user_field_1_mv 	= user_field_1_mv,
		@user_field_2_mv 	= user_field_2_mv,
		@user_field_3_mv 	= user_field_3_mv,
		@user_field_4_mv 	= user_field_4_mv,
		@user_field_5_mv 	= user_field_5_mv,
		@user_field_6_mv 	= user_field_6_mv,
		@user_field_7_mv 	= user_field_7_mv,
		@user_field_8_mv 	= user_field_8_mv,
		@user_field_9_mv 	= user_field_9_mv,
		@user_field_10_mv 	= user_field_10_mv,
		@user_field_11_mv 	= user_field_11_mv,
		@user_field_12_mv 	= user_field_12_mv,
		@user_field_13_mv 	= user_field_13_mv,
		@user_field_14_mv 	= user_field_14_mv,
		@user_field_15_mv 	= user_field_15_mv,
		@user_field_16_mv 	= user_field_16_mv,
		@user_field_17_mv 	= user_field_17_mv,
		@user_field_18_mv 	= user_field_18_mv,
		@user_field_19_mv 	= user_field_19_mv,
		@user_field_20_mv 	= user_field_20_mv,
		@user_field_21_mv 	= user_field_21_mv,
		@user_field_22_mv 	= user_field_22_mv,
		@user_field_23_mv 	= user_field_23_mv,
		@user_field_24_mv 	= user_field_24_mv,
		@user_field_25_mv 	= user_field_25_mv,
		@user_field_26_mv 	= user_field_26_mv,
		@user_field_27_mv 	= user_field_27_mv,
		@user_field_28_mv 	= user_field_28_mv,
		@user_field_29_mv 	= user_field_29_mv,
		@user_field_30_mv 	= user_field_30_mv,
		@user_field_31_mv 	= user_field_31_mv,
		@user_field_32_mv 	= user_field_32_mv
	from blocked_orders_usr_fld_mv
	;


		with orders as
		(
		  select block_id, 
				min(average_price_precision) min_price_precision, 
				min(side_code) side_code,                         
		        count(*) total_order_count, 
				sum( case when closed =1 then 1 else 0 end) closed_order_count, 
				sum( case when held=1 then 1 else 0 end) held_order_count,
				sum( quantity ) quantity_ordered,
				sum( quantity_closed ) quantity_closed,
				sum( case when held =1 then quantity - quantity_confirmed else 0 end) quantity_held,
				sum (quantity_confirmed ) quantity_confirmed,
				sum(quantity_estimated) quantity_estimated,
				sum(market_value) market_value,
				sum(market_value_closed) market_value_closed,
				sum(case when held = 1 then market_value - market_value_confirmed else 0 end) market_value_held,
				sum(market_value_confirmed) market_value_confirmed,
				sum(accrued_income) accrued_income,
				count(distinct directed_broker_id) count_directed_broker_id,
				min(directed_broker_id) min_directed_broker_id,
				count(distinct directed_counter_party_id) count_directed_counter_party_id,
				min(directed_counter_party_id) min_directed_counter_party_id,
				count(distinct limit_type_code) count_limit_type_code,
				min(limit_type_code) min_limit_type_code,
				count(distinct time_in_force_code) count_time_in_force_code,
				min(time_in_force_code) min_time_in_force_code,
				count(distinct program_id) count_program_id,
				min(program_id) min_program_id,
				max(accrued_days) accrued_days,
				count(distinct index_id) count_index_id,
				min(index_id) min_index_id,
				count(distinct account_manager) count_account_manager,
				min(account_manager) min_account_manager,
				count(distinct account_regulation_code) count_account_regulation_code,
				min(account_regulation_code) min_account_regulation_code,
				count(distinct trade_reason_id) count_trade_reason_id,
				min(trade_reason_id) min_trade_reason_id,
				count(distinct settlement_date) count_settlement_date,
				min(settlement_date) min_settlement_date,
				count(distinct account_id) count_account_id,
				min(account_id) min_account_id,
				count(distinct strategy_id) count_strategy_id,
				min(strategy_id) min_strategy_id,
				count(distinct major_account_code) count_major_account_code,
				min(major_account_code) min_major_account_code,
				count(distinct swt_assign_unwind_type) count_swt_assign_unwind_type,
				min(swt_assign_unwind_type) min_swt_assign_unwind_type,
				min(baseline_price) baseline_price,
				max(convert(int, note_indicator)) note_indicator,
				max(convert(int, violation_indicator)) violation_indicator,
				max(convert(int, split_adjusted)) split_adjusted,
				max(convert(int, dividend_adjusted)) dividend_adjusted,
				max(convert(int, domiciled_flag)) domiciled_flag,
				min(convert(int, approved_flag)) approved_flag,  
				min(limit_price_1) min_limit_price_1,
				max(limit_price_1) max_limit_price_1,
				min(limit_price_2) min_limit_price_2,
				max(limit_price_2) max_limit_price_2, 
				0 count_delivery_code,
				0 min_delivery_code,
				--count(distinct order_settlements.delivery_code) count_delivery_code,
				--min(order_settlements.delivery_code) min_delivery_code,
				count(distinct user_field_1) count_user_field_1,
				min(user_field_1) min_user_field_1,
				count(distinct user_field_2) count_user_field_2,
				min(user_field_2) min_user_field_2,
				count(distinct user_field_3) count_user_field_3,
				min(user_field_3) min_user_field_3,
				count(distinct user_field_4) count_user_field_4,
				min(user_field_4) min_user_field_4,
				count(distinct user_field_5) count_user_field_5,
				min(user_field_5) min_user_field_5,
				count(distinct user_field_6) count_user_field_6,
				min(user_field_6) min_user_field_6,
				count(distinct user_field_7) count_user_field_7,
				min(user_field_7) min_user_field_7,
				count(distinct user_field_8) count_user_field_8,
				min(user_field_8) min_user_field_8,
				count(distinct user_field_9) count_user_field_9,
				min(user_field_8) min_user_field_9,
				count(distinct user_field_10) count_user_field_10, min(user_field_10) min_user_field_10,
				count(distinct user_field_11) count_user_field_11, min(user_field_11) min_user_field_11,
				count(distinct user_field_12) count_user_field_12, min(user_field_12) min_user_field_12,
				count(distinct user_field_13) count_user_field_13, min(user_field_13) min_user_field_13,
				count(distinct user_field_14) count_user_field_14, min(user_field_14) min_user_field_14,
				count(distinct user_field_15) count_user_field_15, min(user_field_15)  min_user_field_15,
				count(distinct user_field_16) count_user_field_16, min(user_field_16)  min_user_field_16,
				count(distinct user_field_17) count_user_field_17, min(user_field_17)  min_user_field_17,
				count(distinct user_field_18) count_user_field_18, min(user_field_18)  min_user_field_18,
				count(distinct user_field_19) count_user_field_19, min(user_field_19)  min_user_field_19,
				count(distinct user_field_20) count_user_field_20, min(user_field_20)  min_user_field_20,
				count(distinct user_field_21) count_user_field_21, min(user_field_21)  min_user_field_21,
				count(distinct user_field_22) count_user_field_22, min(user_field_22)  min_user_field_22,
				count(distinct user_field_23) count_user_field_23, min(user_field_23)  min_user_field_23,
				count(distinct user_field_24) count_user_field_24, min(user_field_24)  min_user_field_24,
				count(distinct user_field_25) count_user_field_25, min(user_field_25)  min_user_field_25,
				count(distinct user_field_26) count_user_field_26, min(user_field_26)  min_user_field_26,
				count(distinct user_field_27) count_user_field_27, min(user_field_27)  min_user_field_27,
				count(distinct user_field_28) count_user_field_28, min(user_field_28)  min_user_field_28,
				count(distinct user_field_29) count_user_field_29, min(user_field_29)  min_user_field_29,
				count(distinct user_field_30) count_user_field_30, min(user_field_30)  min_user_field_30,
				count(distinct user_field_31) count_user_field_31, min(user_field_31)  min_user_field_31,
				count(distinct user_field_32) count_user_field_32, min(user_field_32)  min_user_field_32,
				count(distinct fcm_entity_id) count_fcm_entity_id, min(fcm_entity_id)  min_fcm_entity_id
		    from inserted  
		  group by block_id      
		)
		update blocked_orders
		set
			event_counter				= event_counter + 1,
			touched						= 0,
			closed						= case when blocked_orders.quantity_ordered +  orders.quantity_ordered 
													<= blocked_orders.quantity_closed + orders.quantity_closed  then 1 
											 else 0 
										   end,
			partially_closed			= case when (blocked_orders.quantity_ordered +  orders.quantity_ordered > blocked_orders.quantity_closed + orders.quantity_closed )
			                                        and ( blocked_orders.quantity_closed + orders.quantity_closed  ) >0 then 1 else 0 end,
			held						= case when  blocked_orders.quantity_ordered +  orders.quantity_ordered <= blocked_orders.quantity_held + orders.quantity_held then 1 else 0 end,
			partially_held				= case when (blocked_orders.quantity_ordered +  orders.quantity_ordered > blocked_orders.quantity_held + orders.quantity_held )
			                                        and (blocked_orders.quantity_held + orders.quantity_held ) > 0  then 1 else 0 end,
			order_counter				= blocked_orders.order_counter + orders.total_order_count,

			quantity_ordered			= blocked_orders.quantity_ordered + orders.quantity_ordered,
			quantity_closed				= blocked_orders.quantity_closed  + orders.quantity_closed,
			quantity_held				= blocked_orders.quantity_held    + orders.quantity_held,



			quantity_confirmed			= blocked_orders.quantity_confirmed              + orders.quantity_confirmed,
			quantity_estimated			= coalesce(blocked_orders.quantity_estimated, 0) + orders.quantity_estimated,  
			market_value_ordered		= blocked_orders.market_value_ordered       + orders.market_value,
			market_value_closed			= blocked_orders.market_value_closed        + orders.market_value_closed,
			market_value_held			= blocked_orders.market_value_held          + orders.market_value_held,
			market_value_confirmed		= blocked_orders.market_value_confirmed     + orders.market_value_confirmed,
			accrued_income_ordered		= blocked_orders.accrued_income_ordered     + orders.accrued_income,
			directed_broker_id			= case when count_directed_broker_id = 1 and blocked_orders.directed_broker_id = min_directed_broker_id 
													then min_directed_broker_id
											   else NULL
										  end,
			directed_counter_party_id	= case when count_directed_counter_party_id = 1 and blocked_orders.directed_counter_party_id = min_directed_counter_party_id 
													then min_directed_counter_party_id
											   else NULL
										  end,
			limit_type_code_ordered		= case when count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code
													then min_limit_type_code
											   else NULL
										  end,
			time_in_force_code			= case when count_time_in_force_code = 1 and blocked_orders.time_in_force_code = min_time_in_force_code
													then min_time_in_force_code
											   else NULL
										  end,
			program_id					= case when count_program_id = 1 and blocked_orders.program_id = min_program_id
													then min_program_id
											   else NULL
										  end,
			accrued_days				= case when blocked_orders.accrued_days >  orders.accrued_days then blocked_orders.accrued_days
											   else orders.accrued_days
											   end,
			index_id					= case when count_index_id = 1 and blocked_orders.index_id = min_index_id
													then min_index_id
											   else NULL
										  end,
			account_manager				= case when count_account_manager = 1 and blocked_orders.account_manager = min_account_manager
													then min_account_manager
											   else NULL
										  end,
			account_regulation_code		= case when count_account_regulation_code = 1 and blocked_orders.account_regulation_code = min_account_regulation_code
													then min_account_regulation_code
											   else NULL
										  end,
			trade_reason_id				= case when count_trade_reason_id = 1 and blocked_orders.trade_reason_id = min_trade_reason_id
													then min_trade_reason_id
											   else NULL
										  end,
			settlement_date				= case when count_settlement_date = 1 and blocked_orders.settlement_date = min_settlement_date
													then min_settlement_date
											   else NULL
										  end,
			account_id					= case when count_account_id = 1 and blocked_orders.settlement_date = min_account_id
													then min_account_id
											   else NULL
										  end,
			strategy_id					= case when count_strategy_id = 1 and blocked_orders.settlement_date = min_strategy_id
													then min_strategy_id
											   else NULL
										  end,
			major_account_code			= case when count_major_account_code = 1 and blocked_orders.major_account_code = min_major_account_code
													then major_account_code
											   else NULL
										  end,
			swt_assign_unwind_type		= case when count_swt_assign_unwind_type = 1 and blocked_orders.swt_assign_unwind_type = min_swt_assign_unwind_type
													then swt_assign_unwind_type
											   else NULL
										  end,
			baseline_price				= case
												when blocked_orders.order_counter + orders.total_order_count  > 1 then null
												else @baseline_price
										  end,
			note_indicator				= case
												when blocked_orders.note_indicator = 1 or   orders.note_indicator = 1 then 1
												else 0
										  end,
			violation_indicator			= case
												when blocked_orders.violation_indicator = 1 or   orders.violation_indicator = 1 then 1
												else 0
										  end,
			split_adjusted				= case
												when blocked_orders.split_adjusted = 1 or   orders.split_adjusted = 1 then 1
												else 0
										  end,
			dividend_adjusted			= case
												when blocked_orders.dividend_adjusted = 1 or   orders.dividend_adjusted = 1 then 1
												else 0
										  end,
			domiciled_flag				= case
												when blocked_orders.domiciled_flag = 1 or   orders.domiciled_flag = 1 then 1
												else 0
										  end,
			approved_flag				= case
												when blocked_orders.split_adjusted = 1 and  orders.split_adjusted = 1 then 1
												else 0
										  end,
			best_price_1_ordered		= case when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.best_price_1_ordered > min_limit_price_1 then  
																		blocked_orders.best_price_1_ordered
																	else 
																		round(min_limit_price_1 , min_price_precision)
																end
															else  
																case when blocked_orders.best_price_1_ordered > min_limit_price_1 then
																		round(min_limit_price_1 , min_price_precision)
																	 else 
																		blocked_orders.best_price_1_ordered
																 end
										                 end
											  when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code not in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.best_price_1_ordered > min_limit_price_1 then
																		round(min_limit_price_1 , min_price_precision)
																	else 
																		blocked_orders.best_price_1_ordered
																end
															else 
																case when blocked_orders.best_price_1_ordered > min_limit_price_1 then
																		blocked_orders.best_price_1_ordered
																	 else 
																		round(min_limit_price_1 , min_price_precision)
																 end
														end
												end ,
			best_price_2_ordered		= case when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.best_price_2_ordered > min_limit_price_2 then  
																		blocked_orders.best_price_2_ordered
																	else 
																		round(min_limit_price_2 , min_price_precision)
																end
															else  
																case when blocked_orders.best_price_2_ordered > min_limit_price_2 then
																		round(min_limit_price_2 , min_price_precision)
																	 else 
																		blocked_orders.best_price_2_ordered
																 end
										                 end
											  when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code not in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.best_price_2_ordered > min_limit_price_2 then
																		round(min_limit_price_2 , min_price_precision)
																	else 
																		blocked_orders.best_price_2_ordered
																end
															else 
																case when blocked_orders.best_price_2_ordered > min_limit_price_2 then
																		blocked_orders.best_price_2_ordered
																	 else 
																		round(min_limit_price_2 , min_price_precision)
																 end
														end
												end ,
			 worst_price_1_ordered		= case when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.worst_price_1_ordered > max_limit_price_1 then  
																		blocked_orders.worst_price_1_ordered
																	else 
																		round(max_limit_price_1 , min_price_precision)
																end
															else  
																case when blocked_orders.best_price_1_ordered > min_limit_price_1 then
																		round(max_limit_price_1 , min_price_precision)
																	 else 
																		blocked_orders.worst_price_1_ordered
																 end
										                 end
											  when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code not in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.worst_price_1_ordered > max_limit_price_1 then  
																		round(max_limit_price_1 , min_price_precision)
																	else 
																		blocked_orders.worst_price_1_ordered
																end
															else  
																case when blocked_orders.best_price_1_ordered > min_limit_price_1 then
																		blocked_orders.worst_price_1_ordered
																	 else 
																		round(max_limit_price_1 , min_price_precision)
																 end
														end
												end ,
				 worst_price_2_ordered		= case when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.worst_price_2_ordered > max_limit_price_2 then  
																		blocked_orders.worst_price_2_ordered
																	else 
																		round(max_limit_price_2 , min_price_precision)
																end
															else  
																case when blocked_orders.best_price_2_ordered > min_limit_price_2 then
																		round(max_limit_price_2 , min_price_precision)
																	 else 
																		blocked_orders.worst_price_2_ordered
																 end
										                 end
											  when (count_limit_type_code = 1 and blocked_orders.limit_type_code_ordered = min_limit_type_code )
												    and orders.side_code not in (0, 1, 4, 5) 
													then 
														case when limit_type_code_ordered in (6, 1, 4, 5, 7) then
																case when blocked_orders.worst_price_2_ordered > max_limit_price_2 then  
																		round(max_limit_price_2 , min_price_precision)
																	else 
																		blocked_orders.worst_price_2_ordered
																end
															else  
																case when blocked_orders.best_price_2_ordered > min_limit_price_2 then
																		blocked_orders.worst_price_2_ordered
																	 else 
																		round(max_limit_price_2 , min_price_precision)
																 end
														end
												end ,
			delivery_code				= case when count_delivery_code = 1 and blocked_orders.delivery_code = min_delivery_code
													then blocked_orders.delivery_code
											   else -2
										  end,
			update_orders				= 0,
			modified_by					= @current_user,
			user_field_1				= case when count_user_field_1 = 1 and blocked_orders.user_field_1 = min_user_field_1
													then blocked_orders.user_field_1
											   else @user_field_1_mv
										  end, 
			user_field_2				= case when count_user_field_2 = 1 and blocked_orders.user_field_2 = min_user_field_2
													then blocked_orders.user_field_2
											   else @user_field_2_mv
										  end, 
			user_field_3		= case when count_user_field_3 = 1 and blocked_orders.user_field_3 = min_user_field_3 then blocked_orders.user_field_3    else @user_field_3_mv   end,
			user_field_4		= case when count_user_field_4 = 1 and blocked_orders.user_field_4 = min_user_field_4 then blocked_orders.user_field_4    else @user_field_4_mv   end,
			user_field_5		= case when count_user_field_5 = 1 and blocked_orders.user_field_5 = min_user_field_5 then blocked_orders.user_field_5    else @user_field_5_mv   end,
			user_field_6		= case when count_user_field_6 = 1 and blocked_orders.user_field_6 = min_user_field_6 then blocked_orders.user_field_6    else @user_field_6_mv   end,
			user_field_7		= case when count_user_field_7 = 1 and blocked_orders.user_field_7 = min_user_field_7 then blocked_orders.user_field_7    else @user_field_7_mv   end,
			user_field_8		= case when count_user_field_8 = 1 and blocked_orders.user_field_8 = min_user_field_8 then blocked_orders.user_field_8    else @user_field_8_mv   end,
			user_field_9		= case when count_user_field_9 = 1 and blocked_orders.user_field_9 = min_user_field_9 then blocked_orders.user_field_9    else @user_field_9_mv   end,
			user_field_10		= case when count_user_field_10 = 1 and blocked_orders.user_field_10 = min_user_field_10 then blocked_orders.user_field_10    else @user_field_10_mv   end,
			user_field_11		= case when count_user_field_11 = 1 and blocked_orders.user_field_11 = min_user_field_11 then blocked_orders.user_field_11    else @user_field_11_mv   end,
			user_field_12		= case when count_user_field_12 = 1 and blocked_orders.user_field_12 = min_user_field_12 then blocked_orders.user_field_12    else @user_field_12_mv   end,
			user_field_13		= case when count_user_field_13 = 1 and blocked_orders.user_field_13 = min_user_field_13 then blocked_orders.user_field_13    else @user_field_13_mv   end,
			user_field_14		= case when count_user_field_14 = 1 and blocked_orders.user_field_14 = min_user_field_14 then blocked_orders.user_field_14    else @user_field_14_mv   end,
			user_field_15		= case when count_user_field_15 = 1 and blocked_orders.user_field_15 = min_user_field_15 then blocked_orders.user_field_15    else @user_field_15_mv   end,
			user_field_16		= case when count_user_field_16 = 1 and blocked_orders.user_field_16 = min_user_field_16 then blocked_orders.user_field_16    else @user_field_16_mv   end,
			user_field_17		= case when count_user_field_17 = 1 and blocked_orders.user_field_17 = min_user_field_17 then blocked_orders.user_field_17    else @user_field_17_mv   end,
			user_field_18		= case when count_user_field_18 = 1 and blocked_orders.user_field_18 = min_user_field_18 then blocked_orders.user_field_18    else @user_field_18_mv   end,
			user_field_19		= case when count_user_field_19 = 1 and blocked_orders.user_field_19 = min_user_field_19 then blocked_orders.user_field_19    else @user_field_19_mv   end,
			user_field_20		= case when count_user_field_20 = 1 and blocked_orders.user_field_20 = min_user_field_20 then blocked_orders.user_field_20    else @user_field_20_mv   end,
			user_field_21		= case when count_user_field_21 = 1 and blocked_orders.user_field_21 = min_user_field_21 then blocked_orders.user_field_21    else @user_field_21_mv   end,
			user_field_22		= case when count_user_field_22 = 1 and blocked_orders.user_field_22 = min_user_field_22 then blocked_orders.user_field_22    else @user_field_22_mv   end,
			user_field_23		= case when count_user_field_23 = 1 and blocked_orders.user_field_23 = min_user_field_23 then blocked_orders.user_field_23    else @user_field_23_mv   end,
			user_field_24		= case when count_user_field_24 = 1 and blocked_orders.user_field_24 = min_user_field_24 then blocked_orders.user_field_24    else @user_field_24_mv   end,
			user_field_25		= case when count_user_field_25 = 1 and blocked_orders.user_field_25 = min_user_field_25 then blocked_orders.user_field_25    else @user_field_25_mv   end,
			user_field_26		= case when count_user_field_26 = 1 and blocked_orders.user_field_26 = min_user_field_26 then blocked_orders.user_field_26    else @user_field_26_mv   end,
			user_field_27		= case when count_user_field_27 = 1 and blocked_orders.user_field_27 = min_user_field_27 then blocked_orders.user_field_27    else @user_field_27_mv   end,
			user_field_28		= case when count_user_field_28 = 1 and blocked_orders.user_field_28 = min_user_field_28 then blocked_orders.user_field_28    else @user_field_28_mv   end,
			user_field_29		= case when count_user_field_29 = 1 and blocked_orders.user_field_29 = min_user_field_29 then blocked_orders.user_field_29    else @user_field_29_mv   end,
			user_field_30		= case when count_user_field_30 = 1 and blocked_orders.user_field_30 = min_user_field_30 then blocked_orders.user_field_30    else @user_field_30_mv   end,
			user_field_31		= case when count_user_field_31 = 1 and blocked_orders.user_field_31 = min_user_field_31 then blocked_orders.user_field_31    else @user_field_31_mv   end,
			user_field_32		= case when count_user_field_32 = 1 and blocked_orders.user_field_32 = min_user_field_32 then blocked_orders.user_field_32    else @user_field_32_mv   end,
			fcm_entity_id		= case when count_fcm_entity_id = 1 and blocked_orders.fcm_entity_id = min_fcm_entity_id then blocked_orders.fcm_entity_id    else -1   end
		from orders
		where blocked_orders.block_id = orders.block_id;

insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
			values( 0, getdate(), getdate(), 0,901);

		update blocked_orders_total
		set 
			deleted							= blocked_orders.deleted,
			closed							= blocked_orders.closed,
			partially_closed				= blocked_orders.partially_closed,
			held							= blocked_orders.held,
			partially_held					= blocked_orders.partially_held,
			event_counter					= blocked_orders_total.event_counter ,
			order_counter					= blocked_orders.order_counter,
			quantity_ordered				= blocked_orders.quantity_ordered ,
			quantity_closed					= blocked_orders.quantity_closed ,
			quantity_held					= blocked_orders.quantity_held ,
			quantity_confirmed				= blocked_orders.quantity_confirmed ,
			quantity_estimated				= coalesce(blocked_orders.quantity_estimated, 0), 
			market_value_ordered			= blocked_orders.market_value_ordered ,
			market_value_closed				= blocked_orders.market_value_closed ,
			market_value_held				= blocked_orders.market_value_held ,
			market_value_confirmed			= blocked_orders.market_value_confirmed ,
			accrued_income_ordered			= blocked_orders.accrued_income_ordered ,
			directed_broker_id				= blocked_orders.directed_broker_id,
			directed_counter_party_id		= blocked_orders.directed_counter_party_id,
			limit_type_code_ordered			= blocked_orders.limit_type_code_ordered,
			time_in_force_code				= blocked_orders.time_in_force_code,
			program_id						= blocked_orders.program_id,
			accrued_days					= blocked_orders.accrued_days,
			index_id						= blocked_orders.index_id,
			account_manager					= blocked_orders.account_manager,
			account_regulation_code			= blocked_orders.account_regulation_code,
			trade_reason_id					= blocked_orders.trade_reason_id,
			settlement_date					= blocked_orders.settlement_date,
			account_id						= blocked_orders.account_id,
			strategy_id						= blocked_orders.strategy_id,
			major_account_code				= blocked_orders.major_account_code,
			swt_assign_unwind_type			= blocked_orders.swt_assign_unwind_type,
			user_field_1					= blocked_orders.user_field_1,
			user_field_2					= blocked_orders.user_field_2,
			user_field_3					= blocked_orders.user_field_3,
			user_field_4					= blocked_orders.user_field_4,
			user_field_5					= blocked_orders.user_field_5,
			user_field_6					= blocked_orders.user_field_6,
			user_field_7					= blocked_orders.user_field_7,
			user_field_8					= blocked_orders.user_field_8,
			user_field_9					= blocked_orders.user_field_9,
			user_field_10					= blocked_orders.user_field_10,
			user_field_11					= blocked_orders.user_field_11,
			user_field_12					= blocked_orders.user_field_12,
			user_field_13					= blocked_orders.user_field_13,
			user_field_14					= blocked_orders.user_field_14,
			user_field_15					= blocked_orders.user_field_15,
			user_field_16					= blocked_orders.user_field_16,
			baseline_price					= blocked_orders.baseline_price,
			note_indicator					= blocked_orders.note_indicator,
			violation_indicator				= blocked_orders.violation_indicator,
			split_adjusted					= blocked_orders.split_adjusted,
			dividend_adjusted				= blocked_orders.dividend_adjusted,
			domiciled_flag					= blocked_orders.domiciled_flag,
			approved_flag					= blocked_orders.approved_flag,
			best_price_1_ordered			= blocked_orders.best_price_1_ordered,
			best_price_2_ordered			= blocked_orders.best_price_2_ordered, 
			worst_price_1_ordered			= blocked_orders.worst_price_1_ordered, 
			worst_price_2_ordered			= blocked_orders.worst_price_2_ordered,
			delivery_code					= blocked_orders.delivery_code,
			block_recalc					= 0,
			user_field_17					= blocked_orders.user_field_17,
			user_field_18					= blocked_orders.user_field_18,
			user_field_19					= blocked_orders.user_field_19,
			user_field_20					= blocked_orders.user_field_20,
			user_field_21					= blocked_orders.user_field_21,
			user_field_22					= blocked_orders.user_field_22,
			user_field_23					= blocked_orders.user_field_23,
			user_field_24					= blocked_orders.user_field_24,
			user_field_25					= blocked_orders.user_field_25,
			user_field_26					= blocked_orders.user_field_26,
			user_field_27					= blocked_orders.user_field_27,
			user_field_28					= blocked_orders.user_field_28,
			user_field_29					= blocked_orders.user_field_29,
			user_field_30					= blocked_orders.user_field_30,
			user_field_31					= blocked_orders.user_field_31,
			user_field_32					= blocked_orders.user_field_32,
			fcm_entity_id					= blocked_orders.fcm_entity_id
		from blocked_orders
		where blocked_orders_total.block_id = blocked_orders.block_id
		  and blocked_orders.block_id in (select distinct inserted.block_id
		                                    from inserted
											) 

insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
			values( 0, getdate(), getdate(), 0,902);

	update order_tax_lot
		set closed			= inserted.closed,
			held				= inserted.held,
			modified_by		= coalesce(@not_specified_user_id, inserted.modified_by, @unknown_user_id),
			modified_time	= getdate()
	from inserted
	where inserted.order_id = order_tax_lot.order_id
		and ( inserted.closed <> order_tax_lot.closed
		   		or inserted.held <> order_tax_lot.held 
		   	);
	if update(deleted)
	begin
		update order_tax_lot
		   set deleted			= inserted.deleted,
		   	   modified_by		= coalesce(@not_specified_user_id, inserted.modified_by, @unknown_user_id),
			   modified_time	= getdate()
		from inserted
		where inserted.order_id = order_tax_lot.order_id
		   and inserted.deleted <> order_tax_lot.deleted
		   and inserted.deleted = 1;
		update allocations
		   set deleted			= inserted.deleted,
		   	   modified_by		= coalesce(@not_specified_user_id, inserted.modified_by, @unknown_user_id),
			   modified_time	= getdate(),
			   update_orders	= 0
		from inserted
		where inserted.order_id = allocations.order_id
			and allocations.primary_confirmed = 0
			and allocations.primary_pending = 0
			and allocations.primary_canceled = 0
			and inserted.deleted <> allocations.deleted
			and inserted.deleted = 1;
		insert into orders_d 
		(
			order_id, 
			old_account_id, 
			modified_time
		)
		select 
			inserted.order_id,
			inserted.account_id,
			getdate()
		from inserted  
		where inserted.deleted = 1;
	end; 
	select	
		@auditing_switch = enable_auditing
	from trigger_control
	;

insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
			values( 0, getdate(), getdate(), 0,903);
	select @auditing_switch = coalesce(@auditing_switch, 1);
	if @auditing_switch <> 0
	begin
		insert into orders_audit
		(
			deleted,
			archived,
			held,
			closed,
			note_indicator,
			violation_indicator,
			domiciled_flag,
			split_adjusted,
			dividend_adjusted,
			block_id,
			order_id,
			account_id,
			security_id,
			side_code,
			ticket_type_code,
			time_in_force_code,
			average_price_precision,
			quantity,
			market_value,
			tax_lot_cost,
			tax_lot_quantity,
			tax_lot_market_value,
			accrued_income,
			quantity_confirmed,
			market_value_confirmed,
			quantity_closed,
			market_value_closed,
			trader_id,
			directed_broker_id,
			limit_type_code,
			limit_price_1,
			limit_price_2,
			settlement_date,
			major_account_code,
			note,
			program_id,
			accrued_days,
			index_id,
			index_price,
			baseline_price,
			account_manager,
			account_regulation_code,
			trade_reason_id,
			lending_account_id,
			user_field_1,
			user_field_2,
			user_field_3,
			user_field_4,
			user_field_5,
			user_field_6,
			user_field_7,
			user_field_8,
			user_field_9,
			user_field_10,
			user_field_11,
			user_field_12,
			user_field_13,
			user_field_14,
			user_field_15,
			user_field_16,
			created_by,
			created_time,
			originated_by,
			originated_time,
			rte_rule_id,
			action_type_code,
			strategy_id,
			book_id,
			delivery_date,
 			contract_id,
 			orig_order_id,
 			approved_flag,
 			quantity_estimated,
 			quantity_max_approved,
			user_field_17,
			user_field_18,
			user_field_19,
			user_field_20,
			user_field_21,
			user_field_22,
			user_field_23,
			user_field_24,
			user_field_25,
			user_field_26,
			user_field_27,
			user_field_28,
			user_field_29,
			user_field_30,
			user_field_31,
			user_field_32,
			tax_lot_relief_method_code,
			swt_assign_unwind_type,
			swt_pay_leg_accrued_interest,
			swt_pay_leg_accrued_days,
			swt_rec_leg_accrued_interest,
			swt_rec_leg_accrued_days,
			fcm_entity_id,
			sef_entity_id,
			ccp_entity_id,
			dco_entity_id,
			margin_by_account,
			decision_maker_id
			--,
			--trade_date_offset,
			--trade_date
		)
		select
			inserted.deleted,
			inserted.archived,
			inserted.held,
			inserted.closed,
			inserted.note_indicator,
			inserted.violation_indicator,
			inserted.domiciled_flag,
			inserted.split_adjusted,
			inserted.dividend_adjusted,
			inserted.block_id,
			inserted.order_id,
			inserted.account_id,
			inserted.security_id,
			inserted.side_code,
			inserted.ticket_type_code,
			inserted.time_in_force_code,
			inserted.average_price_precision,
			inserted.quantity,
			inserted.market_value,
			inserted.tax_lot_cost,
			inserted.tax_lot_quantity,
			inserted.tax_lot_market_value,
			inserted.accrued_income,
			inserted.quantity_confirmed,
			inserted.market_value_confirmed,
			inserted.quantity_closed,
			inserted.market_value_closed,
			inserted.trader_id,
			inserted.directed_broker_id,
			inserted.limit_type_code,
			inserted.limit_price_1,
			inserted.limit_price_2,
			inserted.settlement_date,
			inserted.major_account_code,
			inserted.note,
			inserted.program_id,
			inserted.accrued_days,
			inserted.index_id,
			inserted.index_price,
			inserted.baseline_price,
			inserted.account_manager,
			inserted.account_regulation_code,
			inserted.trade_reason_id,
			inserted.lending_account_id,
			inserted.user_field_1,
			inserted.user_field_2,
			inserted.user_field_3,
			inserted.user_field_4,
			inserted.user_field_5,
			inserted.user_field_6,
			inserted.user_field_7,
			inserted.user_field_8,
			inserted.user_field_9,
			inserted.user_field_10,
			inserted.user_field_11,
			inserted.user_field_12,
			inserted.user_field_13,
			inserted.user_field_14,
			inserted.user_field_15,
			inserted.user_field_16,
			coalesce(@not_specified_user_id, inserted.modified_by, @unknown_user_id), 
			inserted.modified_time,
			inserted.originated_by,
			inserted.originated_time,
			inserted.rte_rule_id,
			case inserted.deleted
		   		when 0 then 1 
		   		when 1 then 3 
				end as action_type_code,
			inserted.strategy_id,
			inserted.book_id,
			inserted.delivery_date,
 			inserted.contract_id,
 			inserted.orig_order_id,
 			inserted.approved_flag,
 			inserted.quantity_estimated,
 			inserted.quantity_max_approved,
			inserted.user_field_17,
			inserted.user_field_18,
			inserted.user_field_19,
			inserted.user_field_20,
			inserted.user_field_21,
			inserted.user_field_22,
			inserted.user_field_23,
			inserted.user_field_24,
			inserted.user_field_25,
			inserted.user_field_26,
			inserted.user_field_27,
			inserted.user_field_28,
			inserted.user_field_29,
			inserted.user_field_30,
			inserted.user_field_31,
			inserted.user_field_32,
			inserted.tax_lot_relief_method_code,
			inserted.swt_assign_unwind_type,
			inserted.swt_pay_leg_accrued_interest,
			inserted.swt_pay_leg_accrued_days,
			inserted.swt_rec_leg_accrued_interest,
			inserted.swt_rec_leg_accrued_days,
			inserted.fcm_entity_id,
			inserted.sef_entity_id,
			inserted.ccp_entity_id,
			inserted.dco_entity_id,
			inserted.margin_by_account,
			inserted.decision_maker_id
			--,
			--inserted.trade_date_offset,
			--inserted.trade_date
		from inserted  
		where inserted.archived = 0
			and inserted.deleted = 0;
		if @@error != 0
begin
	raiserror(50321, 10, -1)
	rollback transaction
	return
end
	end; 
end
