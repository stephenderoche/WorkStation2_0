
ALTER procedure [dbo].[add_blocked_order_faster]
(@send_order_run_id	numeric(10),
 @current_user   numeric(10)
)
as
	declare @average_price_precision	smallint;
	declare @equivalent_quantity		float;
	declare @market_value				float;
	declare @exchange_rate				float;
	declare @security_exchange_rate		float;
	declare @major_asset_code			smallint;
	declare @listed						bit;
	declare @broker_unlisted_override	bit;
	declare @default_trader_id			smallint;
	declare @default_time_in_force_code tinyint;
	declare @default_note				nvarchar(255);
	declare @major_account_code			tinyint;
	declare @domiciled_flag				tinyint;
	declare @security_price_latest		float;
	declare @security_price_closing		float;  
	declare @security_price_opening		float;
	declare @baseline_price				float;
	declare @index_price				float;
	declare @quantity_rounding			smallint;
	declare @market_value_rounding		smallint;
	declare @account_manager			numeric(10);
	declare @account_regulation_code	smallint;
	declare @ret_val int;
	declare @note_indicator				bit;
	declare @violation_indicator		tinyint;
	declare @tax_lot_market_value		float;
	declare @security_deleted			bit;
	declare @security_symbol			nvarchar(40);
	declare @continue_flag				int;
	declare @cps_add_blocked_order		nvarchar(30);
	declare @cpe_add_blocked_order		nvarchar(30);
	declare @security_level_code		tinyint;
	declare @index_price_latest			float;
	declare @index_price_closing		float;
	declare @index_price_opening		float;
	declare @violation_level			tinyint;
	declare @originated_by_local		smallint;
	declare @originated_time_local		datetime;
	declare @lending_account_id_local	numeric(10);
	declare @quantity_local				float;
	declare @tax_lot_quantity_local		float;
	declare @accrued_income_local		float;
	declare @index_id_local				int;
	declare @index_price_tag_type_local int;
	declare @trader_id_local			numeric(10);
	declare @rowmodctr					int;
	declare @rowcount					int;
	declare @rows_to_update_stats		int;
	declare @use_time_in_force_code		tinyint;
	declare @block_strategy_id			numeric(10);
	declare @is_auto_merge				tinyint;
	declare @is_merge_and_eligible		tinyint;
	declare @force_eligible				tinyint;
	declare @has_manual_settle_date   	tinyint;
	declare @security_deleted_count       int;
	declare @none_major_asset_code_count  int;
	declare @index_major_asset_code_count int;
	declare @zero_security_price_count    int;
	declare @zero_security_price          float;
	declare @account_id_local            numeric(10);
	declare @security_id_local           numeric(10);
	declare @order_id_local              numeric(10);
	declare @equivalent_quantity_local   float;
	declare @market_value_local          float;
	declare @side_code_local             tinyint;
	declare @err_msg                     nvarchar(255);
    declare  @block_id_1 numeric(10) ;
    declare  @order_id_1 numeric(10) ;
    declare  @security_id_1 numeric(10) ;
    declare  @account_id_1 numeric(10) ;
    declare  @side_code_1 tinyint;
    declare  @ticket_type_code_1  tinyint;
    declare  @major_asset_code_1  smallint;
    declare  @average_price_precision_1 smallint;
    declare  @quantity_1  float;
    declare  @market_value_1 float;
    declare  @tax_lot_cost_1 float;
    declare  @tax_lot_quantity_1 float;
    declare  @tax_lot_market_value_1 float;
    declare  @accrued_income_1 float;
	declare  @use_time_in_force_code_1 tinyint;
    declare  @trader_id_1 numeric(10) ;
    declare  @limit_type_code_1 tinyint;
    declare  @limit_price_1_1 float;
    declare  @limit_price_2_1 float;
    declare  @directed_broker_id_1 numeric(10) ;
    declare  @settlement_date_1 datetime;
    declare  @major_account_code_1 tinyint;
    declare  @domiciled_flag_1 bit;
    declare  @violation_indicator_1 bit;
    declare  @note_1 nvarchar(255);
    declare  @baseline_price_1 float;
    declare  @account_manager_1 numeric(10) ;
    declare  @account_regulation_code_1 smallint ;
    declare  @trade_reason_id_1 numeric(10) ;
    declare  @program_id_1 numeric(10) ;
    declare  @index_id_1 numeric(10) ;
    declare  @index_price_1 float ;
    declare  @lending_account_id_1 numeric(10) ;
    declare  @user_field_1_1 float;
    declare  @user_field_2_1 float;
    declare  @user_field_3_1 float;
    declare  @user_field_4_1 float;
    declare  @user_field_5_1 nvarchar(40);
    declare  @user_field_6_1  nvarchar(40);
    declare  @user_field_7_1  nvarchar(40);
    declare  @user_field_8_1  nvarchar(40);
    declare  @manual_accrued_flag_1 tinyint;
    declare  @accrued_days_1 int;
    declare  @strategy_id_1 numeric(10) ;
    declare  @book_id_1 numeric(10) ;
    declare  @link_id_1 numeric(10) ;
    declare  @contingent_id_1 numeric(10) ;
    declare  @originated_by_1  numeric(10) ;
    declare  @originated_time_1 datetime;
    declare  @rte_rule_id_1 numeric(10) ;
    declare  @user_field_9_1 float;
    declare  @user_field_10_1 float;
    declare  @user_field_11_1 float;
    declare  @user_field_12_1 float;
    declare  @user_field_13_1 nvarchar(40);
    declare  @user_field_14_1 nvarchar(40);
    declare  @user_field_15_1 nvarchar(40);
    declare  @user_field_16_1 nvarchar(40);
	declare  @directed_counter_party_id_1 numeric(10) ;
	declare  @tax_lot_relief_method_code_1 tinyint;
	declare  @delivery_date_1 datetime;
	declare  @contract_id_1 numeric(10) ;
	declare  @orig_order_id_1 numeric(10) ;
	declare  @approved_flag_1 tinyint;
	declare  @quantity_estimated_1 float;
	declare  @user_field_17_1  float;
	declare  @user_field_18_1  float;
	declare  @user_field_19_1  float;
	declare  @user_field_20_1  float;
	declare  @user_field_21_1 nvarchar(40);
	declare  @user_field_22_1 nvarchar(40);
	declare  @user_field_23_1 nvarchar(40);
	declare  @user_field_24_1 nvarchar(40);
	declare  @user_field_25_1 float;
	declare  @user_field_26_1  float;
	declare  @user_field_27_1  float;
	declare  @user_field_28_1  float;
	declare  @user_field_29_1 nvarchar(40);
	declare  @user_field_30_1 nvarchar(40);
	declare  @user_field_31_1 nvarchar(40);
	declare  @user_field_32_1 nvarchar(40);
	declare  @swt_assign_unwind_type_1 tinyint ;
	declare  @swt_pay_leg_accrued_intrst_1 float;
	declare  @swt_pay_leg_accrued_days_1 smallint;
	declare  @swt_rec_leg_accrued_intrst_1 float;
	declare  @swt_rec_leg_accrued_days_1 smallint ;
	declare  @fcm_entity_id_1 numeric(10) ;
	declare  @sef_entity_id_1 numeric(10) ;
	declare  @ccp_entity_id_1 numeric(10) ;
	declare  @dco_entity_id_1 numeric(10) ;
	declare  @margin_by_account_1 float ;
	declare  @manual_settlement_date_flag_1  tinyint;
	declare  @trade_date_offset_1 tinyint;
	declare  @trade_date_1 datetime;
	declare  @decision_maker_id_1 numeric;
	declare  @logical_block_id_2 numeric(10);
    declare  @security_id_2      numeric(10);
	declare  @side_code_2        tinyint;
	declare  @ticket_type_code_2 tinyint;
	declare  @trader_id_2        numeric(10);
	declare  @settlement_date_2  datetime;
	declare  @manual_accrued_flag_2 tinyint;
	declare  @accrued_days_2     int;
	declare  @strategy_id_2      numeric(10);
	declare  @link_id_2          numeric(10);
	declare  @package_id_2       numeric(10);
	declare  @contingent_id_2    numeric(10);
	declare  @delivery_date_2   datetime;
	declare  @contract_id_2     numeric(10);
	declare  @swt_assign_unwind_type_2      tinyint;
	declare  @manual_settlement_date_flag_2 tinyint;
	declare  @trade_date_offset_2           tinyint;
	declare  @trade_date_2                  datetime;
	declare  @major_asset_code_2            numeric(10);
	declare  @blocking_code_2               smallint; 
	declare @is_locking_enabled				tinyint;
	declare @is_account_locked				tinyint;
	declare @proforma_locking_type			tinyint;
	declare @validation_ret_val				numeric(10);
	declare @force_new_block				numeric(10);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	select @is_account_locked   = 0;
	select @validation_ret_val  = 0;
    select @proforma_locking_type = 0;
	select @force_new_block =0;


  

	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,2);

begin try
	

merge #order_scratch
	using   ( select security.security_id,security.major_asset_code, security.symbol,security.deleted,security.security_level_code,
		            price.latest,price.closing, price.opening, currency.exchange_rate, 
					rounding.average_price_rounding,rounding.quantity_rounding,currency.market_value_rounding,
					exchange.exchange_code
               from security 
					 left outer join price 
									 on security.security_id = price.security_id
					 join currency 
									 on security.principal_currency_id = currency.security_id
					join rounding 
								 	 on security.major_asset_code = rounding.major_asset_code
			       left outer join exchange on exchange.exchange_code = security.exchange_code
			) secr on ( #order_scratch.security_id =  secr.security_id)
	when matched then
		update 
			set
				major_asset_code = secr.major_asset_code,                    
				security_symbol = secr.symbol,
				security_deleted = secr.deleted,
				security_price_latest   = coalesce(secr.latest,0.0),
				security_price_closing  = coalesce(secr.closing,0.0),
				security_price_opening  = coalesce(secr.opening,0.0),
				security_exchange_rate  = secr.exchange_rate,
				exchange_rate           =  coalesce( (select case when currency.exchange_rate = 0 then 1
																  else currency.exchange_rate end
													  from currency
													  where currency.security_id = #order_scratch.display_currency_id),1),
				average_price_precision = secr.average_price_rounding,
				quantity_rounding = secr.quantity_rounding,
				market_value_rounding = secr.market_value_rounding,
				security_level_code   = secr.security_level_code,
				exchange_code         = secr.exchange_code;
	select 
		@security_deleted_count       =  security_deleted_count,
		@none_major_asset_code_count  =  none_major_asset_code_count,
		@index_major_asset_code_count = index_major_asset_code_count,
		@zero_security_price          = zero_security_price
	from (
			select 
				sum(case when security_deleted =1 then 1 else 0 end) security_deleted_count,
				sum(case when major_asset_code = 10  then 1 else 0 end) none_major_asset_code_count,
				sum(case when major_asset_code = 9 then 1 else 0 end) index_major_asset_code_count,
				sum(case when security_price_latest = 0 
							and major_asset_code <> 12 
							and side_code in (1, 3, 5, 7, 7) 
					then 0 
					else 1 
					end)  zero_security_price
			from #order_scratch
		 ) x;
	if @security_deleted_count > 0
	begin
		select @security_symbol = security_symbol
		from (	select security_symbol
				from #order_scratch
				where security_deleted = 1 ) x
		;
		raiserror(50265, 10, -1,  @security_symbol);
		return 50265;
	end;
	if @none_major_asset_code_count > 0  
	begin
		raiserror(50488, 10, -1);
		return 50488;
	end;
	if @index_major_asset_code_count > 0
	begin
		raiserror(50489, 10, -1);
		return 50489;	
	end;
	if @zero_security_price_count >0
	begin
		raiserror(50254, 10, -1);
		return 50254;
	end;
update #order_scratch
	set default_trader_id = account.trader_id,
		default_time_in_force_code = account.time_in_force_code,
		broker_unlisted_override = account.broker_unlisted_override,
		default_note = account.note,
		major_account_code = account.major_account_code,
		domiciled_flag = convert(tinyint,  account.domiciled_flag),
		account_manager = account.manager,
		account_regulation_code = account.account_regulation_code
	from account
	where #order_scratch.account_id = account.account_id;
	select	
		@violation_level = violation_level
	from compliance_control;
	update #order_scratch
	set logical_block_id = order_id, 	       
		quantity = case when side_code in (0, 2, 4, 6, 8) 
						then quantity
						else 0
				   end,
		tax_lot_quantity = case when side_code in (0, 2, 4, 6, 8) 
	                            then tax_lot_quantity
	                            else 0
							end,
		market_value =  case when side_code not in (0, 2, 4, 6, 8) 
							then  quantity / exchange_rate
							else 0
						 end,
		accrued_income =  case when side_code not in (0, 2, 4, 6, 8) 
							then  accrued_income / exchange_rate
	                        else accrued_income
						  end,
		tax_lot_market_value = case when side_code not in (0, 2, 4, 6, 8) 
									then  tax_lot_quantity / exchange_rate
									else 0
							   end,
		equivalent_quantity = case when side_code not in (0, 2, 4, 6, 8)
	                                and  coalesce(security_price_latest ,0) <> 0 
										then  quantity * security_exchange_rate / security_price_latest
									when  side_code not in (0, 2, 4, 6, 8)
										and  coalesce(security_price_latest ,0) = 0 
										then  0
									else  quantity
				              end,
		use_time_in_force_code = coalesce(time_in_force_code, coalesce(default_time_in_force_code, 0)),
		note_indicator         = case when note is not null or default_note is not null 
									then 1
									else 0
								 end,
		baseline_price        = case when baseline_price_tag_type = 0 then coalesce(security_price_latest, 0.0)
									when baseline_price_tag_type = 1 then coalesce(security_price_closing, 0.0)
									else coalesce(security_price_opening, 0.0)
								end,
		decision_maker_id    = coalesce(decision_maker_id, account_manager, @current_user),
		originated_by        = coalesce(originated_by, @current_user),
		originated_time      = coalesce( originated_time,getdate()),
		lending_account_id   = coalesce(lending_account_id,account_id),
		has_manual_settle_date =  case when major_asset_code = 3 then coalesce(manual_settlement_date_flag,0)
									   else 0
								  end,
		violation_indicator = case when compliance_check >= @violation_level then 1
									else 0
							  end;
		update #order_scratch
		   set index_id              = (select programs.index_id
		                                  from programs
						              	where programs.program_id = #order_scratch.program_id
						               ),
			   index_price_tag_type = (select programs.index_price_tag_type
		                                 from programs
							            where programs.program_id = #order_scratch.program_id
						              )
		 where index_id is null;
		 update #order_scratch
		    set index_price = (select case when index_price_tag_type   = 0 then
			                                   price.latest 
										  when  index_price_tag_type  = 1 then
										       price.closing
										  else price.opening
										  end
								  from price
								  where price.security_id= index_id)
           where index_id is not null;

  


merge  #order_scratch
		 using broker
		  on (#order_scratch.directed_broker_id=broker.broker_id)
		  when matched then 
		   update 
		   set broker_type_code =  broker.broker_type_code,
		       broker_mnemonic  =  broker.mnemonic  ; 

		update #order_scratch
		  set auto_router=desk.auto_router
		  from desk
		  where  #order_scratch.trader_id=desk.desk_id;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,3);


    begin
declare cur_find_trader cursor LOCAL STATIC READ_ONLY FORWARD_ONLY for 
     select account_id,security_id,equivalent_quantity,order_id, trader_id , lending_account_id, side_code, quantity, market_value  
	  From  #order_scratch
	  where #order_scratch.auto_router=1;


		open cur_find_trader;
		fetch cur_find_trader 
		 into @account_id_local,@security_id_local,@equivalent_quantity_local,@order_id_local, @trader_id_local,
		      @lending_account_id_local, @side_code_local, @quantity_local, @market_value_local;
while ((@@fetch_status <> -1))
		begin
		begin
		  	 execute @ret_val = find_trader 
										@account_id  =  @account_id_local,
										@security_id =  @security_id_local,
										@quantity    =  @equivalent_quantity_local,
										@order_id    =  @order_id_local,
										@trader_id   =  @trader_id_local output
										;
	        if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
	     Update #order_scratch
	        Set trader_id= @trader_id_local
	      where order_id = @order_id_local; 
       end;
		fetch cur_find_trader 
		 into @account_id_local,@security_id_local,@equivalent_quantity_local,@order_id_local, @trader_id_local,
		      @lending_account_id_local, @side_code_local, @quantity_local, @market_value_local;
    end;
		close cur_find_trader;
deallocate cur_find_trader;
		update #order_scratch
		set #order_scratch.blocking_code = (select trader_blocking_code.blocking_code
		 										  from trader_blocking_code
				 								  where trader_blocking_code.major_asset_code = #order_scratch.major_asset_code 
												   and trader_blocking_code.trader_id = #order_scratch.trader_id)
		where #order_scratch.blocking_code is null;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
end; 

	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,4);

	 if coalesce(@force_new_block,0) !=0 
	 begin 
	    update #order_scratch 
		   set logical_block_id=1;
	 end else begin
	    execute @ret_val = find_block_for_order 
	                                   @current_user = @current_user;
	    if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
	 end;

	 --select * into order_scratch_pkn1 from #order_scratch;

	if @sp_trancount is null or @sp_initial_trancount is null
                        begin
                            set @sp_trancount = 0;
                            set @sp_initial_trancount = @@trancount;
                        end;
                        if @@TRANCOUNT = 0                      
                            begin transaction;
                        else
                            set @sp_trancount = @sp_trancount + 1;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,5);

		Insert 
		  into orders 
			(
				block_id,
				order_id,
				security_id,
				account_id,
				side_code,
				ticket_type_code,
				average_price_precision,
				quantity,
				market_value,
				tax_lot_cost,
				tax_lot_quantity,
				tax_lot_market_value,
				accrued_income,
				time_in_force_code,
				trader_id,
				limit_type_code,
				limit_price_1,
				limit_price_2,
				directed_broker_id,
				settlement_date,
				major_account_code,
				domiciled_flag,
				violation_indicator,
				note,
				baseline_price,
				account_manager,
				account_regulation_code,
				trade_reason_id,
				program_id,
				index_id,
				index_price,
				lending_account_id,
				user_field_1,
				user_field_2,
				user_field_3,
				user_field_4,
				user_field_5,
				user_field_6,
				user_field_7,
				user_field_8,
				manual_accrued_flag,
				accrued_days,
				strategy_id,
				book_id,
				link_id,
				contingent_id,
				originated_by,
				originated_time,
				created_by,
				created_time,
				modified_by,
				modified_time,
				rte_rule_id,
				user_field_9,
				user_field_10,
				user_field_11,
				user_field_12,
				user_field_13,
				user_field_14,
				user_field_15,
				user_field_16,
				directed_counter_party_id,
				tax_lot_relief_method_code,
				delivery_date,
				contract_id,
				orig_order_id,
				approved_flag,
				quantity_estimated,
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
				manual_settlement_date_flag,
				trade_date_offset,
				trade_date,
				send_order_run_id,
				decision_maker_id
			)
	select #order_scratch.block_id,
			#order_scratch.order_id,
			#order_scratch.security_id,
			#order_scratch.account_id,
			#order_scratch.side_code,
			#order_scratch.ticket_type_code,
			#order_scratch.average_price_precision,
			#order_scratch.quantity,
			#order_scratch.market_value,
			#order_scratch.tax_lot_cost,
			#order_scratch.tax_lot_quantity,
			#order_scratch.tax_lot_market_value,
			#order_scratch.accrued_income,
			#order_scratch.use_time_in_force_code,
			coalesce(#order_scratch.trader_id, #order_scratch.default_trader_id) trader_id,
			#order_scratch.limit_type_code,
			#order_scratch.limit_price_1,
			#order_scratch.limit_price_2,
			#order_scratch.directed_broker_id,
			#order_scratch.settlement_date,
			#order_scratch.major_account_code,
			case
				when #order_scratch.domiciled_flag is null then 0
				when #order_scratch.domiciled_flag = 0 then 0
				else 1
			end domiciled_flag,
			#order_scratch.violation_indicator,
			coalesce(#order_scratch.note, #order_scratch.default_note) note,
			#order_scratch.baseline_price,
			#order_scratch.account_manager,
			#order_scratch.account_regulation_code,
			#order_scratch.trade_reason_id,
			#order_scratch.program_id,
			#order_scratch.index_id,
			#order_scratch.index_price,
			#order_scratch.lending_account_id,
			#order_scratch.user_field_1,
			#order_scratch.user_field_2,
			#order_scratch.user_field_3,
			#order_scratch.user_field_4,
			#order_scratch.user_field_5,
			#order_scratch.user_field_6,
			#order_scratch.user_field_7,
			#order_scratch.user_field_8,
			#order_scratch.manual_accrued_flag,
			#order_scratch.accrued_days,
			#order_scratch.strategy_id,
			#order_scratch.book_id,
			#order_scratch.link_id,
			#order_scratch.contingent_id,
			#order_scratch.originated_by,
			#order_scratch.originated_time,
			@current_user,
			getdate(),
			@current_user,
			getdate(),
			#order_scratch.rte_rule_id,
			#order_scratch.user_field_9,
			#order_scratch.user_field_10,
			#order_scratch.user_field_11,
			#order_scratch.user_field_12,
			#order_scratch.user_field_13,
			#order_scratch.user_field_14,
			#order_scratch.user_field_15,
			#order_scratch.user_field_16,
			#order_scratch.directed_counter_party_id,
			#order_scratch.tax_lot_relief_method_code,
			#order_scratch.delivery_date,
			#order_scratch.contract_id,
			#order_scratch.orig_order_id,
			#order_scratch.approved_flag,
			#order_scratch.quantity_estimated,
			#order_scratch.user_field_17,
			#order_scratch.user_field_18,
			#order_scratch.user_field_19,
			#order_scratch.user_field_20,
			#order_scratch.user_field_21,
			#order_scratch.user_field_22,
			#order_scratch.user_field_23,
			#order_scratch.user_field_24,
			#order_scratch.user_field_25,
			#order_scratch.user_field_26,
			#order_scratch.user_field_27,
			#order_scratch.user_field_28,
			#order_scratch.user_field_29,
			#order_scratch.user_field_30,
			#order_scratch.user_field_31,
			#order_scratch.user_field_32,
			#order_scratch.swt_assign_unwind_type,
			#order_scratch.swt_pay_leg_accrued_interest,
			#order_scratch.swt_pay_leg_accrued_days,
			#order_scratch.swt_rec_leg_accrued_interest,
			#order_scratch.swt_rec_leg_accrued_days,
			#order_scratch.fcm_entity_id,
			#order_scratch.sef_entity_id,
			#order_scratch.ccp_entity_id,
			#order_scratch.dco_entity_id,
			#order_scratch.margin_by_account,
			#order_scratch.manual_settlement_date_flag,
			#order_scratch.trade_date_offset,
			#order_scratch.trade_date,
			@send_order_run_id,
			#order_scratch.decision_maker_id
		 from #order_scratch 
		where #order_scratch.block_id is not null;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,6);

			if @major_asset_code_1 = 13
			begin
				execute @ret_val = copy_tba_stips_to_block  @order_id = @order_id_1, @block_id = @block_id_1, @current_user = @current_user;	
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end; 
		select @is_merge_and_eligible = 0;
		if exists(
			select 1
			from registry
			where section = 'ALLOC'
				and entry = 'MERGE AND ELIGIBLE'
				and value = '1'
		) begin
			select @is_merge_and_eligible = 1;
		end;
		 insert into alloc_eligible
			(
				order_id,
				ticket_id,
				is_eligible,
				modified_by,
				modified_time,
				created_by,
				created_time
			)
			select
				#order_scratch.order_id,
				ticket.ticket_id,
				1,
				@current_user,
				getdate(),
				@current_user,
				getdate()
			from ticket join #order_scratch on 
			      (     ticket.block_id = #order_scratch.block_id
				    and ticket.deleted = 0
				   )
			where ( @is_merge_and_eligible = 1
			        or #order_scratch.ticket_type_code in (14, 2, 3)  
				  ) 
			      and not exists 
				    (
						select 1
						from alloc_eligible
						where alloc_eligible.ticket_id = ticket.ticket_id
						  and alloc_eligible.order_id = #order_scratch.order_id
				    );
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
		 update blocked_orders
		    set last_merge_time = getdate(),
				modified_by = @current_user,
				modified_time = getdate()
			where @is_merge_and_eligible = 0
			  and exists ( select 1 
			                 from #order_scratch
							where #order_scratch.block_id = blocked_orders.block_id
							  and #order_scratch.ticket_type_code not in (14, 2, 3)   
							  and #order_scratch.block_id is not null);
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,7);

		insert 
		  into blocked_orders
			(
				security_id,
				deleted,
				closed,
				side_code,
				ticket_type_code,
				trader_id,
				settlement_date,
				manual_accrued_flag,
				accrued_days,
				strategy_id,
				link_id,
				package_id,
				contingent_id,
				created_by,
				created_time,
				modified_by,
				modified_time,
				delivery_date,
				contract_id,
				swt_assign_unwind_type,
				manual_settlement_date_flag,
				trade_date_offset,
				trade_date,
				send_order_run_id,
				logical_block_id,
				major_asset_code,
				blocking_code,
				order_counter

			)
		select DISTINCT 
				#order_scratch.security_id,
				0,
				0,
				#order_scratch.side_code,
				#order_scratch.ticket_type_code,
				#order_scratch.trader_id,
				#order_scratch.settlement_date,
				#order_scratch.manual_accrued_flag,
				#order_scratch.accrued_days,
				#order_scratch.strategy_id,
				#order_scratch.link_id,
				#order_scratch.package_id,
				#order_scratch.contingent_id,
				@current_user,
		        getdate(),
		        @current_user,
		        getdate(),
				#order_scratch.delivery_date,
				#order_scratch.contract_id,
				#order_scratch.swt_assign_unwind_type,
				#order_scratch.manual_settlement_date_flag,
				#order_scratch.trade_date_offset,
				#order_scratch.trade_date,
				#order_scratch.send_order_run_id,
				#order_scratch.logical_block_id,
				#order_scratch.major_asset_code,
				#order_scratch.blocking_code,
				0
			from #order_scratch
			where block_id is null;

	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,8);

			Insert 
			into orders 
			(
				block_id,
				order_id,
				security_id,
				account_id,
				side_code,
				ticket_type_code,
				average_price_precision,
				quantity,
				market_value,
				tax_lot_cost,
				tax_lot_quantity,
				tax_lot_market_value,
				accrued_income,
				time_in_force_code,
				trader_id,
				limit_type_code,
				limit_price_1,
				limit_price_2,
				directed_broker_id,
				settlement_date,
				major_account_code,
				domiciled_flag,
				violation_indicator,
				note,
				baseline_price,
				account_manager,
				account_regulation_code,
				trade_reason_id,
				program_id,
				index_id,
				index_price,
				lending_account_id,
				user_field_1,
				user_field_2,
				user_field_3,
				user_field_4,
				user_field_5,
				user_field_6,
				user_field_7,
				user_field_8,
				manual_accrued_flag,
				accrued_days,
				strategy_id,
				book_id,
				link_id,
				contingent_id,
				originated_by,
				originated_time,
				created_by,
				created_time,
				modified_by,
				modified_time,
				rte_rule_id,
				user_field_9,
				user_field_10,
				user_field_11,
				user_field_12,
				user_field_13,
				user_field_14,
				user_field_15,
				user_field_16,
				directed_counter_party_id,
				tax_lot_relief_method_code,
				delivery_date,
				contract_id,
				orig_order_id,
				approved_flag,
				quantity_estimated,
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
				manual_settlement_date_flag,
				trade_date_offset,
				trade_date,
				send_order_run_id,
				decision_maker_id
			)
			select
				blocked_orders.block_id,
				#order_scratch.order_id,
				#order_scratch.security_id,
				#order_scratch.account_id,
				#order_scratch.side_code,
				#order_scratch.ticket_type_code,
				#order_scratch.average_price_precision,
				#order_scratch.quantity,
				#order_scratch.market_value,
				#order_scratch.tax_lot_cost,
				#order_scratch.tax_lot_quantity,
				#order_scratch.tax_lot_market_value,
				#order_scratch.accrued_income,
				#order_scratch.use_time_in_force_code,
				coalesce(#order_scratch.trader_id, #order_scratch.default_trader_id) trader_id,
				#order_scratch.limit_type_code,
				#order_scratch.limit_price_1,
				#order_scratch.limit_price_2,
				#order_scratch.directed_broker_id,
				#order_scratch.settlement_date,
				#order_scratch.major_account_code,
				case
					when #order_scratch.domiciled_flag is null then 0
					when #order_scratch.domiciled_flag = 0 then 0
					else 1
				end domiciled_flag,
				#order_scratch.violation_indicator,
				coalesce(#order_scratch.note, #order_scratch.default_note) note,
				#order_scratch.baseline_price,
				#order_scratch.account_manager,
				#order_scratch.account_regulation_code,
				#order_scratch.trade_reason_id,
				#order_scratch.program_id,
				#order_scratch.index_id,
				#order_scratch.index_price,
				#order_scratch.lending_account_id,
				#order_scratch.user_field_1,
				#order_scratch.user_field_2,
				#order_scratch.user_field_3,
				#order_scratch.user_field_4,
				#order_scratch.user_field_5,
				#order_scratch.user_field_6,
				#order_scratch.user_field_7,
				#order_scratch.user_field_8,
				#order_scratch.manual_accrued_flag,
				#order_scratch.accrued_days,
				#order_scratch.strategy_id,
				#order_scratch.book_id,
				#order_scratch.link_id,
				#order_scratch.contingent_id,
				#order_scratch.originated_by,
				#order_scratch.originated_time,
				@current_user,
				getdate(),
				@current_user,
				getdate(),
				#order_scratch.rte_rule_id,
				#order_scratch.user_field_9,
				#order_scratch.user_field_10,
				#order_scratch.user_field_11,
				#order_scratch.user_field_12,
				#order_scratch.user_field_13,
				#order_scratch.user_field_14,
				#order_scratch.user_field_15,
				#order_scratch.user_field_16,
				#order_scratch.directed_counter_party_id,
				#order_scratch.tax_lot_relief_method_code,
				#order_scratch.delivery_date,
				#order_scratch.contract_id,
				#order_scratch.orig_order_id,
				#order_scratch.approved_flag,
				#order_scratch.quantity_estimated,
				#order_scratch.user_field_17,
				#order_scratch.user_field_18,
				#order_scratch.user_field_19,
				#order_scratch.user_field_20,
				#order_scratch.user_field_21,
				#order_scratch.user_field_22,
				#order_scratch.user_field_23,
				#order_scratch.user_field_24,
				#order_scratch.user_field_25,
				#order_scratch.user_field_26,
				#order_scratch.user_field_27,
				#order_scratch.user_field_28,
				#order_scratch.user_field_29,
				#order_scratch.user_field_30,
				#order_scratch.user_field_31,
				#order_scratch.user_field_32,
				#order_scratch.swt_assign_unwind_type,
				#order_scratch.swt_pay_leg_accrued_interest,
				#order_scratch.swt_pay_leg_accrued_days,
				#order_scratch.swt_rec_leg_accrued_interest,
				#order_scratch.swt_rec_leg_accrued_days,
				#order_scratch.fcm_entity_id,
				#order_scratch.sef_entity_id,
				#order_scratch.ccp_entity_id,
				#order_scratch.dco_entity_id,
				#order_scratch.margin_by_account,
				#order_scratch.manual_settlement_date_flag,
				#order_scratch.trade_date_offset,
				#order_scratch.trade_date,
				@send_order_run_id,
				#order_scratch.decision_maker_id
			from #order_scratch , blocked_orders 
			where #order_scratch.block_id is null
			  and blocked_orders.send_order_run_id           = @send_order_run_id
			  and #order_scratch.major_asset_code = blocked_orders.major_asset_code
			  and coalesce(#order_scratch.blocking_code,-1) = coalesce(blocked_orders.blocking_code,-1)
			  and #order_scratch.logical_block_id = blocked_orders.logical_block_id;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,9);

			if @major_asset_code_1 = 13
			begin
				execute @ret_val = copy_tba_stips_to_block  @order_id = @order_id_1, @block_id = @block_id_1, @current_user = @current_user;	
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end; 

	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,10);

merge restriction_override
		  using #order_scratch
		     on ( restriction_override.order_id= #order_scratch.order_id )
		  when matched then
			  update set status_code = case when status_code= 0 then 
			                                     1 
									   else status_code
                                       end;

	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,11);

			update cmpl_invocation
			set		
				retained = 1
			from restriction_map
			join #order_scratch 
				 on ( restriction_map.order_id = #order_scratch.order_id)
			join restriction_detail
				on restriction_map.restriction_detail_id = restriction_detail.restriction_detail_id
			join cmpl_res_rule_result
				on restriction_detail.restriction_id = cmpl_res_rule_result.cmpl_res_rule_result_id
			join cmpl_invocation
				on cmpl_res_rule_result.cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
			where cmpl_invocation.retained <> 1;


	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,12);

	--update proposed_orders
	--set rebal_session_id	= null,
	--	modified_by			= @current_user
	-- from #order_scratch
	-- where proposed_orders.order_id = #order_scratch.order_id 
	-- and proposed_orders.rebal_session_id is not null;
			 
	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,13);


	Exec SetContext N'SendOrder';

			delete proposed_orders
			from #order_scratch
			where proposed_orders.order_id = #order_scratch.order_id;

	insert into sendorder_DB_perf( send_order_run_id , start_time, end_time , rec_count, step )
	values(@send_order_run_id, getdate(), getdate(), 0 ,14);

	if @sp_trancount = 0                           
                            commit transaction;     
                        else
                            set @sp_trancount = @sp_trancount - 1;
end try
begin catch 
        if @@trancount != 0
		begin
            rollback transaction;
		end;
		select @err_msg = cast (@@ERROR as nvarchar) + ' - ' + ERROR_MESSAGE();
		raiserror(50693, 10, -1,  @err_msg);
		return 50693;
end catch 
	return @validation_ret_val;
end
