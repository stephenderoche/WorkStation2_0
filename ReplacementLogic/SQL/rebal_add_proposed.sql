USE [LVTS_753]
GO
/****** Object:  StoredProcedure [dbo].[rebal_add_proposed]    Script Date: 4/25/2019 9:13:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER procedure [dbo].[rebal_add_proposed]
(	
    @proposed_orders    	 rebal_add_prop_params readonly,
	@current_user           numeric(10)
)  
as
    declare @cps_add_rebal_prop_order                nvarchar(30);
    declare @cpe_add_rebal_prop_order                nvarchar(30);
    declare @current_time                            datetime;
	declare @ret_val int;
    declare @free_broker_id                          numeric(10);
    declare @auto_routing_id                         smallint;
    declare @continue_flag                           int;
    declare @status_bits                             smallint;
    declare @session_account_id                      numeric(10);
    declare @account_id                              numeric(10);
    declare @position_type_code                      tinyint;  
    declare @security_id                             numeric(10);
    declare @quantity                                float;
    declare @market_value                            float;
    declare @side_code                               tinyint;  
    declare @ticket_type_code                        tinyint;  
    declare @settlement_date                         datetime;
    declare @clear_proposed                          tinyint;  
    declare @accrued_income                          float;
    declare @accrued_days                            int;
    declare @manual_accrued_flag                     tinyint;  
    declare @default_broker_id                       numeric(10);
    declare @rebal_session_id                        numeric(10);
    declare @delivery_date                           datetime;
    declare @trading_desk_id                         numeric(10);
    declare @mf_trade_date_offset                    tinyint;  
    declare @tax_lot_id                              numeric(10);
    declare @tax_lot_relief_on                       tinyint;
    declare @ordered_qty                             float;
    declare @tax_lot_relief_method_code              tinyint;
    declare @client_tax_lot_id                       nvarchar(40);
    declare @lot_number                              nvarchar(40);
    declare @system_currency_id                      numeric(10);
    declare @order_id                                numeric(10);
    declare @tax_lot_qty                             float;
    declare @last_account_id                         numeric(10);
    declare @last_security_id                        numeric(10);
    declare @last_position_type_code                 smallint; 
    declare @remaining_qty                           float;
    declare @send_lot_quantity                       float;
	declare @cps_rebal_insert_po_appr	             nvarchar(30);
	declare @cpe_rebal_insert_po_appr	             nvarchar(30);
    declare @cps_rebal_add_proposed	                 nvarchar(30);
	declare @cpe_rebal_add_proposed                  nvarchar(30);
	declare @rebal_run_id                            numeric(10);
declare @check_cps_pre_hook                      bit;
    declare @check_cpe_pre_hook                      bit;
    declare @check_cps_add_hook                      bit;
    declare @check_cpe_add_hook                      bit;
    declare @check_cps_post_hook                     bit;
    declare @check_cpe_post_hook                     bit;
    declare @called_from_xref                        bit;
	declare @last_in_session						 bit;
    declare @check_cps_pre_hook_tinyint              tinyint;
    declare @check_cpe_pre_hook_tinyint              tinyint;
    declare @called_from_xref_tinyint                tinyint;
    declare @check_cps_post_hook_tinyint             tinyint;
    declare @check_cpe_post_hook_tinyint             tinyint;
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
insert 
	     into rebal_scratch_proposed
		(
		    process_id,
			account_id,
			position_type_code,
			security_id,
			quantity,
			market_value,
			side_code,
			ticket_type_code,
			settlement_date,
			accrued_income,
			manual_accrued_flag,
			accrued_days,
			default_broker_id,
			rebal_session_id,
			delivery_date,
            trading_desk_id,
			clear_proposed,
            session_account_id,
	        status_bits,
	        rebal_run_id,
			mf_trade_date_offset 	
		)
	 select 
	        @@spid,
			account_id,
			position_type_code,
			security_id,
			quantity,
			market_value,
			side_code,
			ticket_type_code,
			settlement_date,
			accrued_income,
			manual_accrued_flag,
			accrued_days,
			default_broker_id,
			rebal_session_id,
			delivery_date,
			trading_desk_id,
	        clear_proposed,
			session_account_id,
			status_bits,
	        rebal_run_id,
			mf_trade_date_offset 
		from  @proposed_orders;
select  @last_in_session  = max(status_bits) &  2 
		   from @proposed_orders;
        select top 1  @rebal_run_id   = rebal_run_id
		   from @proposed_orders;
       if @last_in_session = 0 
       begin
	      return;
	   end;
	create table #rebal_proposed_order  	( 	status_bits					smallint,  	session_account_id			numeric(10),  	account_id					numeric(10),  	position_type_code			tinyint,  	security_id					numeric(10),  	quantity					float,  	market_value				float,  	side_code					tinyint,  	ticket_type_code			tinyint  null,  	settlement_date		    	datetime null,  	clear_proposed				tinyint not null,  	accrued_income				float null,  	accrued_days				int null,  	manual_accrued_flag		    tinyint null,  	default_broker_id			numeric(10) null,  	rebal_session_id			numeric(10) null,  	delivery_date				datetime null,  	trading_desk_id             numeric(10) null,  	mf_trade_date_offset		tinyint null,  	note                        nvarchar(255) null,  	time_in_force_code          tinyint null,  	trader_id                   numeric(10)  null,  	tax_lot_relief_method_code  tinyint  null,  	decision_maker_id			numeric(10) null,  	pre_rebal_continue_flag     tinyint not null,  	post_rebal_continue_flag    tinyint not null  	);
    create table #orders_to_delete  	(  		order_id numeric(10) not null  	);
	create table #accounts_to_reset  	(  		account_id numeric(10) not null  	);	
	create table #affected_ids  	(  		restriction_detail_id		numeric(10) not null  	);



      insert 
	  into #rebal_proposed_order 
	       (
		    status_bits,
            session_account_id,
            account_id,
            position_type_code,
            security_id,
            quantity,
            market_value,
            side_code,
            ticket_type_code,
            settlement_date,
            clear_proposed,
            accrued_income,
            accrued_days,
            manual_accrued_flag,
            default_broker_id,
            rebal_session_id,
            delivery_date,
            trading_desk_id,
            mf_trade_date_offset,
            pre_rebal_continue_flag,
            post_rebal_continue_flag
            )
	 select status_bits,
			session_account_id,
			account_id,
			position_type_code,
			security_id,
			quantity,
			market_value,
			side_code,
			ticket_type_code,
			settlement_date,
			clear_proposed,
			accrued_income,
			accrued_days,
			manual_accrued_flag,
			default_broker_id,
			rebal_session_id,
			delivery_date,
			trading_desk_id,
			mf_trade_date_offset,
			1,
			1
		from rebal_scratch_proposed
       where rebal_run_id = @rebal_run_id;
		Update rebal  
		    set rebal.note                      =  acct.note,
				rebal.time_in_force_code        =  acct.time_in_force_code,
                rebal.trader_id                 =  acct.trader_id,
				rebal.tax_lot_relief_method_code = acct.tax_lot_relief_method_code,
				rebal.decision_maker_id				 = acct.manager
		    from #rebal_proposed_order as rebal inner join account as acct
			    on ( rebal.account_id= acct.account_id);
	 delete rebal_scratch_proposed where rebal_run_id = @rebal_run_id;
	select @continue_flag = 1
select @cps_rebal_add_proposed = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cps_rebal_add_proposed'
	and sysobjects.type = 'P'
if @cps_rebal_add_proposed is not null
begin
	execute @ret_val = cps_rebal_add_proposed
		@continue_flag output, @proposed_orders,
		@current_user
	if (@ret_val != 0 and @ret_val < 60000) or @continue_flag = 0
	begin
		return @ret_val
	end
end

		

	select @current_time = getdate();
	select 	
		@free_broker_id = broker.broker_id 
	from broker 
	where  broker_type_code = 0
	;
	select 	
		@auto_routing_id = desk_id 
	from desk 
	where auto_router = 1
	;
    begin
declare cur_pre_rebal_acct cursor for 
	select account_id, clear_proposed, status_bits
	  From  #rebal_proposed_order
	 where (status_bits &  16 >0)
	   or  (status_bits &  32 >0);
		open cur_pre_rebal_acct;
		fetch cur_pre_rebal_acct 
		 into
		    @account_id,
		    @clear_proposed,
			@status_bits;

		

while ((@@fetch_status <> -1))
		begin
	

		select @check_cps_pre_hook = @status_bits &  16;
	    select @check_cpe_pre_hook = @status_bits &  32;
		begin
		   select @check_cps_pre_hook_tinyint = case
												when @check_cps_pre_hook = 0 then 0
												else 1
												end;
		   select @check_cpe_pre_hook_tinyint = case
												when @check_cpe_pre_hook = 0 then 0
												else 1
												end;
		   execute @ret_val = pre_rebal_proposed_orders 
	 	                                  @account_id = @account_id,
	 	                                  @clear_proposed = @clear_proposed,
	 	                                  @current_user = @current_user,
	 	                                  @check_cps_pre_hook = @check_cps_pre_hook_tinyint,
	 	                                  @check_cpe_pre_hook = @check_cpe_pre_hook_tinyint
	 	                                ;
if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
              begin
                 Update #rebal_proposed_order 
	                Set pre_rebal_continue_flag=0
	              where account_id =@account_id; 
              end;
       end;
		fetch cur_pre_rebal_acct 
	     into  @account_id,
		       @clear_proposed,
		     	@status_bits;
		end;
		close cur_pre_rebal_acct;
deallocate cur_pre_rebal_acct;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
end;  
    begin
declare cur_post_rebal_acct cursor for 
     select account_id, clear_proposed, status_bits
	  From  #rebal_proposed_order
	 where ( pre_rebal_continue_flag != 0 )
	   and  (status_bits &  256 >0
	         or  status_bits &  512 >0);
		open cur_post_rebal_acct;
		fetch cur_post_rebal_acct 
		 into
		    @account_id,
		    @clear_proposed,
			@status_bits;
while ((@@fetch_status <> -1))
		begin
		select @check_cps_post_hook = @status_bits &  256;
	    select @check_cpe_post_hook = @status_bits &  512;
		begin
		   select @check_cps_post_hook_tinyint = case
												when @check_cps_post_hook = 0 then 0
												else 1
												end;
		   select @check_cpe_post_hook_tinyint = case
												when @check_cpe_post_hook = 0 then 0
												else 1
												end;

		   execute @ret_val = post_rebal_proposed_orders 
	 	                                  @account_id = @account_id,
	 	                                  @clear_proposed = @clear_proposed,
	 	                                  @current_user = @current_user,
	 	                                  @check_cps_post_hook = @check_cps_post_hook_tinyint,
	 	                                  @check_cpe_post_hook = @check_cpe_post_hook_tinyint
	 	                                ;
if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
              begin
                 Update #rebal_proposed_order 
	                Set post_rebal_continue_flag=0
	              where account_id =@account_id; 
              end;
       end;
		fetch cur_post_rebal_acct 
		 into
		    @account_id,
		    @clear_proposed,
			@status_bits;
		end;
		close cur_post_rebal_acct;
deallocate cur_post_rebal_acct;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
end;  
	  delete #rebal_proposed_order
	   where (   pre_rebal_continue_flag = 0 
		       or post_rebal_continue_flag = 0  );
		insert into #accounts_to_reset 
			(account_id)
		select distinct account_id 
		  from #rebal_proposed_order;
	select 	
		@clear_proposed = max(clear_proposed),
		@session_account_id = max(session_account_id)
	from #rebal_proposed_order
	where clear_proposed=1
    ;
	if @sp_trancount is null or @sp_initial_trancount is null
                        begin
                            set @sp_trancount = 0;
                            set @sp_initial_trancount = @@trancount;
                        end;
                        if @@TRANCOUNT = 0                      
                            begin transaction;
                        else
                            set @sp_trancount = @sp_trancount + 1;
     if coalesce(@clear_proposed, 0) != 1
	    begin
			execute @ret_val = reset_many_accounts  @current_user = @current_user;
			if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			delete proposed_orders
					from #rebal_proposed_order 
					where #rebal_proposed_order.account_id = proposed_orders.account_id 
						and	#rebal_proposed_order.position_type_code = proposed_orders.position_type_code 
						and	#rebal_proposed_order.security_id = proposed_orders.security_id 
						and proposed_orders.is_pre_executed = 0;
			   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
		end else begin
        execute @ret_val = reset_single_account  @account_id = @session_account_id, @current_user = @current_user;
		if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			delete proposed_orders
					from account_hierarchy_map
					where account_hierarchy_map.child_id = proposed_orders.account_id 
						and account_hierarchy_map.parent_id = @session_account_id
						and proposed_orders.is_pre_executed = 0;
						   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
		end;



	exec se_do_substitute 199

	insert into proposed_orders
	(	
		account_id,
		position_type_code,
		security_id,
		quantity,
		market_value,
		accrued_income,
		side_code,
		ticket_type_code,
		directed_broker_id,
		trader_id,
		time_in_force_code,
		limit_type_code,
		settlement_date,
		compliance_check,
		override_check,
		baseline_price_tag_type,
		index_price_tag_type,
		note,
		tax_lot_relief_method_code,
		lending_account_id,
		created_by,
		created_time,
		modified_by,
		modified_time,
		manual_accrued_flag,
		accrued_days,
		rebal_session_id,
		delivery_date,
		trade_date_offset,
		decision_maker_id
	 )
	select
		account_id,
		position_type_code,
		security_id,
		quantity,
		market_value,
		accrued_income,
		side_code,
		ticket_type_code,
		coalesce(default_broker_id, @free_broker_id),
		coalesce(trading_desk_id, coalesce(trader_id, @auto_routing_id)),
		case
				when mf_trade_date_offset > 0 then 0
				else coalesce(time_in_force_code, 0)
		end,
		0,
		settlement_date,
		null,
		null,
		0,
		0,
		'steve',
		tax_lot_relief_method_code,
		account_id,
		@current_user,
		@current_time,
		@current_user,
		@current_time,
		manual_accrued_flag,
		accrued_days,
		rebal_session_id,
		delivery_date,
		mf_trade_date_offset,
		coalesce(decision_maker_id, @current_user)
	from #rebal_proposed_order 
	where not (quantity = 0.0 and market_value = 0.0);
	   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
	if @sp_trancount = 0                           
                            commit transaction;     
                        else
                            set @sp_trancount = @sp_trancount - 1;
  select @tax_lot_relief_on = coalesce(convert(int,  registry.value), 0)
	from registry
	where
		registry.section = 'TAXLOTRELIEF' and
		registry.entry = 'TAX LOT RELIEF ENABLED';
	if coalesce(@tax_lot_relief_on, 0) != 1
	begin
		return;
	end;
	execute @ret_val = get_system_currency  @currency_id = @system_currency_id output;
	if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
	 create table #rebal_tax_lots  	( 	account_id                           numeric(10),  	security_id                          numeric(10),  	trade_date                           datetime,  	unit_cost                            float,  	position_type_code                   tinyint,  	order_id                             numeric(10),  	market_value                         float,  	ordered_qty                          float,  	side_code                            float,  	tax_lot_relief_method_code           tinyint,  	quantity                             float,  	tax_lot_id                           numeric(10),  	client_tax_lot_id                    nvarchar(40),  	lot_number                           nvarchar(40)  	);
	 insert 
	   into #rebal_tax_lots(    account_id,        security_id,    trade_date,          unit_cost,         position_type_code, 
	                              order_id,       market_value,    ordered_qty,         side_code, tax_lot_relief_method_code,
						       quantity,  tax_lot_id, client_tax_lot_id,                lot_number)
	  select proposed_orders.account_id, proposed_orders.security_id,tax_lot.trade_date, tax_lot.unit_cost,
	         side.position_type_code, proposed_orders.order_id,proposed_orders.market_value,
			  case when side.side_code in ( 3, 5)  then 
			             floor(proposed_orders.market_value / price.latest)  
				else proposed_orders.quantity
			  end  ordered_qty, side.side_code,
			 tax_lot_relief_method.tax_lot_relief_method_code,
			 tax_lot.quantity - coalesce((select	sum(order_tax_lot.quantity - order_tax_lot.quantity_confirmed)
											from order_tax_lot
										   where order_tax_lot.tax_lot_id = tax_lot.tax_lot_id
											 and order_tax_lot.deleted = 0), 0.0) quantity, 
		     tax_lot.tax_lot_id, tax_lot.client_tax_lot_id, tax_lot.lot_number
		from proposed_orders left outer join tax_lot 
		    on (     proposed_orders.account_id = tax_lot.account_id
		         and proposed_orders.security_id = tax_lot.security_id
		         and proposed_orders.position_type_code= tax_lot.position_type_code)
			join side on (  proposed_orders.side_code= side.side_code 
			                and side.buy_indicator = 0 
						    and side.side_code in (2, 3, 4, 5)
							)
			join #rebal_proposed_order on 
			  (   proposed_orders.account_id          = #rebal_proposed_order.account_id
		        and proposed_orders.security_id       = #rebal_proposed_order.security_id
		        and proposed_orders.position_type_code = #rebal_proposed_order.position_type_code)
			 left join tax_lot_relief_method on 
			  ( #rebal_proposed_order.tax_lot_relief_method_code = tax_lot_relief_method.tax_lot_relief_method_code)
			join security on ( proposed_orders.security_id =security.security_id 
			                  and security.major_asset_code <> 0 )
			left join price on ( security.security_id= price.security_id )
		where proposed_orders.is_pre_executed = 0; 
    begin
      if @sp_trancount is null or @sp_initial_trancount is null
                        begin
                            set @sp_trancount = 0;
                            set @sp_initial_trancount = @@trancount;
                        end;
                        if @@TRANCOUNT = 0                      
                            begin transaction;
                        else
                            set @sp_trancount = @sp_trancount + 1;
declare cur_proposed_order cursor for 
	   select  order_id,  account_id,     security_id,         position_type_code,
	       market_value, ordered_qty,       side_code, tax_lot_relief_method_code,
						    quantity,      tax_lot_id,          client_tax_lot_id,           
						  lot_number
		from #rebal_tax_lots
	   where tax_lot_relief_method_code = 3
	 order by account_id, security_id,position_type_code, trade_date desc, unit_cost desc ;
        select @last_account_id          = -1;
        select @last_security_id         = -1;
        select @last_position_type_code  = -1;
		open cur_proposed_order;
		fetch cur_proposed_order 
		 into @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
while ((@@fetch_status <> -1))
		begin
        if (@last_account_id != @account_id ) or ( @last_security_id !=  @security_id ) or (@last_position_type_code != @position_type_code ) 
        begin
		      select @last_account_id          = @account_id;
              select @last_security_id         = @security_id;
              select @last_position_type_code  = @position_type_code;
			  select @remaining_qty  = @ordered_qty; 
		end;
		if @remaining_qty >= @tax_lot_qty 
		begin
		   select @send_lot_quantity = @tax_lot_qty;
		   select @remaining_qty     = @remaining_qty - @tax_lot_qty; 
		end else begin
		   select @send_lot_quantity = @remaining_qty;
		   select @remaining_qty =0;
		end;
		if (@send_lot_quantity > 0) and (@client_tax_lot_id is not null and @lot_number is not null) 
			begin
				execute @ret_val = add_order_tax_lot 
					@tax_lot_id = @tax_lot_id, 
					@order_id = @order_id,
					@quantity = @send_lot_quantity,
					@display_currency_id = @system_currency_id,
					@current_user = @current_user,
					@called_from_front_end = 0 ;
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end;
		fetch cur_proposed_order 
		 into
		      @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
		end;
		close cur_proposed_order;
deallocate cur_proposed_order;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
declare cur_proposed_order cursor for 
	   select  order_id,  account_id,     security_id,         position_type_code,
	       market_value, ordered_qty,       side_code, tax_lot_relief_method_code,
						    quantity,      tax_lot_id,          client_tax_lot_id,           
						  lot_number
		from #rebal_tax_lots
	   where tax_lot_relief_method_code = 4
	 order by account_id, security_id, position_type_code, trade_date asc, unit_cost desc ;
        select @last_account_id          = -1;
        select @last_security_id         = -1;
        select @last_position_type_code  = -1;
		open cur_proposed_order;
		fetch cur_proposed_order 
		 into @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
while ((@@fetch_status <> -1))
		begin
        if (@last_account_id != @account_id ) or ( @last_security_id !=  @security_id ) or (@last_position_type_code != @position_type_code ) 
        begin
		      select @last_account_id          = @account_id;
              select @last_security_id         = @security_id;
              select @last_position_type_code  = @position_type_code;
			  select @remaining_qty  = @ordered_qty;
		end;
		if @remaining_qty >= @tax_lot_qty 
		begin
		   select @send_lot_quantity = @tax_lot_qty;
		   select @remaining_qty     = @remaining_qty - @tax_lot_qty; 
		end else begin
		   select @send_lot_quantity = @remaining_qty;
		   select @remaining_qty =0;
		end;
		if (@send_lot_quantity > 0) and (@client_tax_lot_id is not null and @lot_number is not null) 
			begin
				execute @ret_val = add_order_tax_lot 
					@tax_lot_id = @tax_lot_id, 
					@order_id = @order_id,
					@quantity = @send_lot_quantity,
					@display_currency_id = @system_currency_id,
					@current_user = @current_user,
					@called_from_front_end = 0 ;
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end;
		fetch cur_proposed_order 
		 into
		      @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
		end;
		close cur_proposed_order;
deallocate cur_proposed_order;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
declare cur_proposed_order cursor for 
	   select  order_id,  account_id,     security_id,         position_type_code,
	       market_value, ordered_qty,       side_code, tax_lot_relief_method_code,
						    quantity,      tax_lot_id,          client_tax_lot_id,           
						  lot_number
		from #rebal_tax_lots
	   where (     ( tax_lot_relief_method_code in (5, 6 )
	                 and side_code in (2, 3 ) )
			 or  ( tax_lot_relief_method_code in (2, 7)
	                 and side_code in (4, 5 ) )
			 )
	 order by account_id, security_id, position_type_code, unit_cost asc, trade_date asc;
        select @last_account_id          = -1;
        select @last_security_id         = -1;
        select @last_position_type_code  = -1;
		open cur_proposed_order;
		fetch cur_proposed_order 
		 into @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
while ((@@fetch_status <> -1))
		begin
        if (@last_account_id != @account_id ) or ( @last_security_id !=  @security_id ) or (@last_position_type_code != @position_type_code ) 
        begin
		      select @last_account_id          = @account_id;
              select @last_security_id         = @security_id;
              select @last_position_type_code  = @position_type_code;
			  select @remaining_qty  = @ordered_qty;
		end;
		if @remaining_qty >= @tax_lot_qty 
		begin
		   select @send_lot_quantity = @tax_lot_qty;
		   select @remaining_qty     = @remaining_qty - @tax_lot_qty; 
		end else begin
		   select @send_lot_quantity = @remaining_qty;
		   select @remaining_qty =0;
		end;
		if (@send_lot_quantity > 0) and (@client_tax_lot_id is not null and @lot_number is not null) 
			begin
				execute @ret_val = add_order_tax_lot 
					@tax_lot_id = @tax_lot_id, 
					@order_id = @order_id,
					@quantity = @send_lot_quantity,
					@display_currency_id = @system_currency_id,
					@current_user = @current_user,
					@called_from_front_end = 0 ;
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end;
		fetch cur_proposed_order 
		 into
		      @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
		end;
		close cur_proposed_order;
deallocate cur_proposed_order;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
declare cur_proposed_order cursor for 
	   select  order_id,  account_id,     security_id,         position_type_code,
	       market_value, ordered_qty,       side_code, tax_lot_relief_method_code,
						    quantity,      tax_lot_id,          client_tax_lot_id,           
						  lot_number
		from #rebal_tax_lots
	   where (     ( tax_lot_relief_method_code in (2, 7 )
	                 and side_code in (2, 3 ) )
			 or  ( tax_lot_relief_method_code in (5, 6)
	                 and side_code in (4, 5 ) )
			 )
	 order by account_id, security_id, position_type_code, unit_cost desc, trade_date asc;
        select @last_account_id          = -1;
        select @last_security_id         = -1;
        select @last_position_type_code  = -1;
		open cur_proposed_order;
		fetch cur_proposed_order 
		 into @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
while ((@@fetch_status <> -1))
		begin
        if (@last_account_id != @account_id ) or ( @last_security_id !=  @security_id ) or (@last_position_type_code != @position_type_code ) 
        begin
		      select @last_account_id          = @account_id;
              select @last_security_id         = @security_id;
              select @last_position_type_code  = @position_type_code;
			  select @remaining_qty  = @ordered_qty;
		end;
		if @remaining_qty >= @tax_lot_qty 
		begin
		   select @send_lot_quantity = @tax_lot_qty;
		   select @remaining_qty     = @remaining_qty - @tax_lot_qty; 
		end else begin
		   select @send_lot_quantity = @remaining_qty;
		   select @remaining_qty =0;
		end;
		if (@send_lot_quantity > 0) and (@client_tax_lot_id is not null and @lot_number is not null) 
			begin
				execute @ret_val = add_order_tax_lot 
					@tax_lot_id = @tax_lot_id, 
					@order_id = @order_id,
					@quantity = @send_lot_quantity,
					@display_currency_id = @system_currency_id,
					@current_user = @current_user,
					@called_from_front_end = 0 ;
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end;
		fetch cur_proposed_order 
		 into
		      @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
		end;
		close cur_proposed_order;
deallocate cur_proposed_order;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
declare cur_proposed_order cursor for 
	   select  order_id,  account_id,     security_id,         position_type_code,
	       market_value, ordered_qty,       side_code, tax_lot_relief_method_code,
						    quantity,      tax_lot_id,          client_tax_lot_id,           
						  lot_number
		from #rebal_tax_lots
	   where (     ( tax_lot_relief_method_code in (8 )
	                 and side_code in (2, 3 ) )
			 or  ( tax_lot_relief_method_code in (9)
	                 and side_code in (4, 5 ) )
			 )
	 order by account_id, security_id, position_type_code, unit_cost asc, trade_date desc;
        select @last_account_id          = -1;
        select @last_security_id         = -1;
        select @last_position_type_code  = -1;
		open cur_proposed_order;
		fetch cur_proposed_order 
		 into @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
while ((@@fetch_status <> -1))
		begin
        if (@last_account_id != @account_id ) or ( @last_security_id !=  @security_id ) or (@last_position_type_code != @position_type_code ) 
        begin
		      select @last_account_id          = @account_id;
              select @last_security_id         = @security_id;
              select @last_position_type_code  = @position_type_code;
			  select @remaining_qty  = @ordered_qty;
		end;
		if @remaining_qty >= @tax_lot_qty 
		begin
		   select @send_lot_quantity = @tax_lot_qty;
		   select @remaining_qty     = @remaining_qty - @tax_lot_qty; 
		end else begin
		   select @send_lot_quantity = @remaining_qty;
		   select @remaining_qty =0;
		end;
		if (@send_lot_quantity > 0) and (@client_tax_lot_id is not null and @lot_number is not null) 
			begin
				execute @ret_val = add_order_tax_lot 
					@tax_lot_id = @tax_lot_id, 
					@order_id = @order_id,
					@quantity = @send_lot_quantity,
					@display_currency_id = @system_currency_id,
					@current_user = @current_user,
					@called_from_front_end = 0 ;
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end;
		fetch cur_proposed_order 
		 into
		      @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
		end;
		close cur_proposed_order;
deallocate cur_proposed_order;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
declare cur_proposed_order cursor for 
	   select  order_id,  account_id,     security_id,         position_type_code,
	       market_value, ordered_qty,       side_code, tax_lot_relief_method_code,
						    quantity,      tax_lot_id,          client_tax_lot_id,           
						  lot_number
		from #rebal_tax_lots
	   where (     ( tax_lot_relief_method_code in (9 )
	                 and side_code in (2, 3 ) )
			 or  ( tax_lot_relief_method_code in (8)
	                 and side_code in (4, 5 ) )
			 )
	 order by account_id, security_id, position_type_code, unit_cost desc, trade_date desc;
        select @last_account_id          = -1;
        select @last_security_id         = -1;
        select @last_position_type_code  = -1;
		open cur_proposed_order;
		fetch cur_proposed_order 
		 into @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
while ((@@fetch_status <> -1))
		begin
        if (@last_account_id != @account_id ) or ( @last_security_id !=  @security_id ) or (@last_position_type_code != @position_type_code ) 
        begin
		      select @last_account_id          = @account_id;
              select @last_security_id         = @security_id;
              select @last_position_type_code  = @position_type_code;
			  select @remaining_qty  = @ordered_qty;
		end;
		if @remaining_qty >= @tax_lot_qty 
		begin
		   select @send_lot_quantity = @tax_lot_qty;
		   select @remaining_qty     = @remaining_qty - @tax_lot_qty; 
		end else begin
		   select @send_lot_quantity = @remaining_qty;
		   select @remaining_qty =0;
		end;
		if (@send_lot_quantity > 0) and (@client_tax_lot_id is not null and @lot_number is not null) 
			begin
				execute @ret_val = add_order_tax_lot 
					@tax_lot_id = @tax_lot_id, 
					@order_id = @order_id,
					@quantity = @send_lot_quantity,
					@display_currency_id = @system_currency_id,
					@current_user = @current_user,
					@called_from_front_end = 0 ;
				if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
			end;
		fetch cur_proposed_order 
		 into
		      @order_id, @account_id, @security_id, @position_type_code, 
			  @market_value, @ordered_qty, @side_code, @tax_lot_relief_method_code,
			  @tax_lot_qty,@tax_lot_id,  @client_tax_lot_id, @lot_number;
		end;
		close cur_proposed_order;
deallocate cur_proposed_order;
		   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
   end; 
   if @sp_trancount = 0                           
                            commit transaction;     
                        else
                            set @sp_trancount = @sp_trancount - 1;
	select @cpe_rebal_add_proposed = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cpe_rebal_add_proposed'
	and sysobjects.type = 'P'
if @cpe_rebal_add_proposed is not null
begin
	execute @ret_val = cpe_rebal_add_proposed
		@proposed_orders,
		@current_user
	if (@ret_val != 0 and @ret_val < 60000)
	begin
		return @ret_val
	end
end
	return;
end
