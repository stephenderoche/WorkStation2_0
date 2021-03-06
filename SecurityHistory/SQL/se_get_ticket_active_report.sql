if exists (select * from sysobjects where name = 'se_get_ticket_active_report')
begin
	drop procedure se_get_ticket_active_report
	print 'PROCEDURE: se_get_ticket_active_report dropped'
end
go


Create procedure se_get_ticket_active_report
(	@trader_id 					numeric(10), 
    @display_currency_id 		numeric(10) = null,
    @include_confirmed_tickets 	tinyint = 1,
    @account_id					numeric(10) = null,
	@isAccountId                  smallint = 1,
    @userId					  	  smallint = 198,          
    @security_id				numeric(10) = -1,
	@issuer_id                  numeric(10) = -1,
	@search                     Varchar(40) = null,
	@m_intAssetCode             numeric(10) = -1
    
) 

as

	declare @ret_val int;

	declare @display_currency_id_local	numeric(10);
	declare @current_user					int;
declare @sacctID						nvarchar(40);

begin

                        set nocount on;

                        declare @ec__errno int;

                        declare @sp_initial_trancount int;

                        declare @sp_trancount int;

	create table #ticket_rprt  	(deleted tinyint null,  		archived						tinyint null,  		closed							tinyint null,  		engaged							tinyint null,  		allocated						tinyint null,  		step_out						tinyint null,  		stepped_out						tinyint null,  
	primary_marked					tinyint null,  		primary_pending					tinyint null,  		primary_confirmed				tinyint null,  		primary_canceled				tinyint null,  		primary_modified				tinyint null,  		secondary_modified				tinyint null,  		secondary_marked				tinyint null
,  		secondary_pending				tinyint null,  		secondary_confirmed				tinyint null,  		secondary_canceled				tinyint null,  		ticket_id						numeric(10) null,  		security_id						numeric(10) null,  		major_asset_code				smallint null, 
 		minor_asset_code				smallint null,  		price							float null,  		principal_factor				float null,  		commission_type_code			smallint null,  		paid_commission_flag			tinyint null,  		market_value_rounding			smallint null,  		original_broker_mnemonic		nvarchar(8) null,  		
		directed_broker_count			smallint null,  		fx_forward_flag					tinyint null,  		broker_comment					nvarchar(40) null,  		exchange_name					nvarchar(40) null,  		exchange_mnemonic				nvarchar(8) null,  		type_description				nvarchar(40) null,  	
		reason_description				nvarchar(40) null,  		mnemonic						nvarchar(8) null,  		trader_name						nvarchar(40) null,  		reviewer_name					nvarchar(40) null,  		primary_sender_name				nvarchar(40) null,  		secondary_sender_name			nvarchar(40) null,  		execution_broker_id				
numeric(10) null,  		created_by						int null,  		reviewer_user_id				int null,  		primary_sender_user_id			int null,  		secondary_sender_user_id		int null,  		commission_reason_code			numeric(10) null,  		exchange_code					numeric(10) null,  		
primary_error_code				smallint null,  		secondary_error_code			smallint null,  		side_code						tinyint null,  		trade_date						datetime null,  		settlement_date					datetime null,  		quantity_executed				float null,  		average_price_executed			float null,  
		commission						float null,  		taxes							float null,  		other_charges					float null,  		local_commission				float null,  		exchange_fee					float null,  		stamp_tax						float null,  		levy							float null,  		other_taxes_fees				float null, 
		 		accrued_income					float null,  		event_counter					int null,  		created_time					datetime null,  		modified_by						smallint null,  		modified_time					datetime null,  		reviewed_by						smallint null,  		reviewed_time					datetime null,  	
					primary_sent_by					smallint null,  		primary_sent_time				datetime null,  		secondary_sent_by				smallint null,  		secondary_sent_time				datetime null,  		block_id						numeric(10) null,  		ticket_type_code				tinyint null,  		broker_ticket_id				numeric(10) null, 
					 		orig_broker_id					numeric(10) null,  		imputed_flag					tinyint null,  		account_short_name				nvarchar(40) null,  		account_name_1					nvarchar(40) null,  		account_name_2					nvarchar(40) null,  		account_user_id_1				nvarchar(40) null,  		account_user_id_2			
	nvarchar(40) null,  		account_user_id_3				nvarchar(40) null,  		account_user_id_4				nvarchar(40) null,  		account_user_id_5				nvarchar(40) null,  		account_user_id_6				nvarchar(40) null,  		account_user_id_7				nvarchar(40) null,  		account_user_id_8
	nvarchar(40) null,  		is_electronic_execution			tinyint null,  		warehousing_flag				tinyint null,  		fx_price_direction				tinyint null,  		settlement_fx_rate				float null,  		commission_rate_type_code		smallint null,  		commission_rate					float
				 null,  		comm_rate_type_mnemonic			nvarchar(8) null,  		ticket_user_field_1				nvarchar(40) null,  		ticket_user_field_2				nvarchar(40) null,  		ticket_user_field_3				nvarchar(40) null,  		ticket_user_field_4				nvarchar(40) null,  		ticket_user_field_5		
		nvarchar(40) null,  		ticket_user_field_6				nvarchar(40) null,  		ticket_user_field_7				nvarchar(40) null,  		ticket_user_field_8				nvarchar(40) null,  		ticket_user_field_9				float null,  		ticket_user_field_10			float null,  		ticket_user_field_11
			float null,  		ticket_user_field_12			float null,  		ticket_user_field_13			float null,  		ticket_user_field_14			float null,  		ticket_user_field_15			float null,  		ticket_user_field_16			float null,  		original_ticket_id				numeric(10) null,  
					trade_time						datetime null,  		commission_calculated_flag		tinyint null,  		exchange_country_code			numeric(10) null,  		exchange_listed_flag			tinyint null,  		comm_fee_sched_id				numeric(10) null,  		commission_schedule_mnemonic	nvarchar(20) null,  
							net_amount						numeric(30, 10) null,  		counterparty_id					numeric(10) null,  		counterparty_short_name			nvarchar(20) null,  		settlement_currency_id          numeric(10) null,  		manual_commission_rate_flag		tinyint null,  		ticket_user_field_17		
								nvarchar(40) null,  		ticket_user_field_18			nvarchar(40) null,  		ticket_user_field_19			nvarchar(40) null,  		ticket_user_field_20			nvarchar(40) null,  		ticket_user_field_21			nvarchar(40) null,  		ticket_user_field_22			nvarchar(40) null,  	
									ticket_user_field_23			nvarchar(40) null,  		ticket_user_field_24			nvarchar(40) null,  		ticket_user_field_25			float null,  		ticket_user_field_26			float null,  		ticket_user_field_27			float null,  		ticket_user_field_28			float null,  	
										ticket_user_field_29			float null,  		ticket_user_field_30			float null,  		ticket_user_field_31			float null,  		ticket_user_field_32			float null,  		swt_assign_unwind_type			tinyint null,  		booking_price					float null,  		traded_price					float null,  	
											contract_id						numeric(10) null,  		trader_note						nvarchar(255) null,  		currency_mnemonic				nvarchar(8) null,  		swap_funded_flag				tinyint null  	);

	create table #blocks_acct  	(  		block_id		numeric(10) null,  		account_id		numeric(10) null  	);

	select @display_currency_id_local = @display_currency_id;

	create table #account  	(  		account_id numeric(10) not null  	);

if @account_id <> -1

	begin

		if @isAccountId <> 0

		begin

			if @isAccountId = 2 

			begin

				insert into #account 

				(

					account_id

				) 

				values (@account_id);

				select 

					@sacctID = account.short_name 

				from account 

				where account.account_id = @account_id;

			end else begin 

				insert into #account 

				(

					account_id

				)

				select

					account.account_id

				from account_hierarchy_map

				join account 

					on account_hierarchy_map.child_id = account.account_id

				where account_hierarchy_map.parent_id = @account_id

					and account.account_level_code <> 3

					and account.deleted = 0

					and account.ad_hoc_flag = 0;

				select 

					@sacctID = account.short_name 

				from account 

				where account.account_id = @account_id;

			end;

		end else begin 

			insert into #account 

			(

				account_id

			)

			select 

				account.account_id

			from workgroup_tree_map

			join account 

				on workgroup_tree_map.child_id = account.account_id

				and account.account_level_code <> 3

				and account.deleted = 0

				and account.ad_hoc_flag = 0			

			where workgroup_tree_map.parent_id = @account_id

				and workgroup_tree_map.child_type in (2, 3);

			select 

				@sacctID = user_info.name 

			from user_info 

			where user_id = @account_id;

		end;

	end else begin

		select @current_user = @userId;

		insert into #account

		(

			account_id

		)

		select

			account.account_id

		from user_access_map

		join account

			on user_access_map.object_id = account.account_id

			and account.deleted = 0

		where user_access_map.user_id = @current_user

			and user_access_map.object_type in (2, 3);

		select @sacctID = 'All';

	end;

			insert into #blocks_acct

			(

				block_id, 

				account_id

			)

			select distinct(blocked_orders.block_id), 

				orders.account_id

			from blocked_orders

			join orders on blocked_orders.block_id = orders.block_id

			            and orders.deleted = 0

			left join account on account.account_id = orders.account_id

			            and account.account_level_code <> 1

			            and account.deleted = 0

			            and account.ad_hoc_flag = 0

			left join account_hierarchy_map on account_hierarchy_map.child_id = account.account_id

			join ticket on ticket.block_id = blocked_orders.block_id

				and ticket.deleted = 0

			join desk_tree_map on desk_tree_map.child_id = blocked_orders.trader_id
			left join security on
			ticket.security_id = security.security_id

			where orders.account_id in (select account_id from #account)  
			and(account.short_name like @search +'%'   or @search = '')
				and (ticket.security_id = @security_id or  @security_id = -1)
				and (security.issuer_id = @issuer_id or @issuer_id = -1) 
				and (security.major_asset_code = @m_intAssetCode or @m_intAssetCode = -1)
				and blocked_orders.deleted = 0
				
			option (keepfixed plan);



	if @display_currency_id_local is null

	begin

		execute @ret_val = get_system_currency  @currency_id = @display_currency_id_local output;

	end;

	insert into #ticket_rprt

	(

		ticket_id,

		security_id,

		major_asset_code,

		minor_asset_code,

		price,

		principal_factor,

		commission_type_code,

		market_value_rounding,

		original_broker_mnemonic,

		directed_broker_count,

		fx_forward_flag,

		type_description,

		paid_commission_flag,

		commission_reason_code,

		exchange_code,

		reviewer_user_id,

		primary_sender_user_id,

		secondary_sender_user_id,

		execution_broker_id,

		created_by,

		archived,

		closed,

		engaged,

		allocated,

		step_out,

		stepped_out,

		primary_marked,

		primary_pending,

		primary_confirmed,

		primary_canceled,

		primary_error_code,

		secondary_marked,

		secondary_pending,

		secondary_confirmed,

		secondary_canceled,

		secondary_error_code,

		side_code,

		trade_date,

		settlement_date,

		quantity_executed,

		average_price_executed,

		commission,

		taxes,

		other_charges,

		local_commission,

		exchange_fee,

		stamp_tax,

		levy,

		other_taxes_fees,

		accrued_income,

		event_counter,

		created_time,

		modified_by,

		modified_time,

		reviewed_by,

		reviewed_time,

		primary_sent_by,

		primary_sent_time,

		secondary_sent_by,

		secondary_sent_time,

		block_id,

		ticket_type_code,

		broker_ticket_id,

		deleted,

		primary_modified,

		secondary_modified,

		orig_broker_id,

		imputed_flag,

		account_short_name,

		account_name_1,

		account_name_2,

		account_user_id_1,

		account_user_id_2,

		account_user_id_3,

		account_user_id_4,

		account_user_id_5,

		account_user_id_6,

		account_user_id_7,

		account_user_id_8,

		is_electronic_execution,

		warehousing_flag,

		fx_price_direction,

		settlement_fx_rate,

		commission_rate_type_code,

		commission_rate,

		ticket_user_field_1,		

		ticket_user_field_2,		

		ticket_user_field_3,		

		ticket_user_field_4,		

		ticket_user_field_5,		

		ticket_user_field_6,		

		ticket_user_field_7,		

		ticket_user_field_8,		

		ticket_user_field_9,		

		ticket_user_field_10,	

		ticket_user_field_11,	

		ticket_user_field_12,	

		ticket_user_field_13,	

		ticket_user_field_14,	

		ticket_user_field_15,	

		ticket_user_field_16,

		ticket_user_field_17,

		ticket_user_field_18,

		ticket_user_field_19,

		ticket_user_field_20,

		ticket_user_field_21,

		ticket_user_field_22,

		ticket_user_field_23,

		ticket_user_field_24,

		ticket_user_field_25,

		ticket_user_field_26,

		ticket_user_field_27,

		ticket_user_field_28,

		ticket_user_field_29,

		ticket_user_field_30,

		ticket_user_field_31,

		ticket_user_field_32,

		original_ticket_id,

		trade_time,

		comm_fee_sched_id,

		commission_schedule_mnemonic,

		commission_calculated_flag,

		counterparty_id,

		swt_assign_unwind_type,

		booking_price,

		traded_price,

		contract_id,

		trader_note,

		currency_mnemonic,

		swap_funded_flag

	)

	select

		ticket_1.ticket_id,

		ticket_1.security_id,

		security.major_asset_code,

		security.minor_asset_code,

		ticket_1.average_price_executed,

		coalesce(debt_ticket.principal_factor, swt_contract.principal_factor, security.principal_factor), 

		ticket_1.commission_type_code,

		currency.market_value_rounding,

		null,

		0,

		0,

		commission_type.description,

		coalesce(1 - commission_type.imputed_flag, 1),

		ticket_1.commission_reason_code,

		ticket_1.exchange_code,

		ticket_1.reviewed_by,

		ticket_1.primary_sent_by,

		ticket_1.secondary_sent_by,

		ticket_1.execution_broker_id,

		ticket_1.created_by,

		ticket_1.archived,

		ticket_1.closed,

		ticket_1.engaged,

		ticket_1.allocated,

		ticket_1.step_out,

		coalesce((select max(convert(tinyint,  ticket_3.step_out))

		        from ticket ticket_3

		        where ticket_1.block_id = ticket_3.block_id and

		              ticket_1.ticket_id = ticket_3.original_ticket_id and

			          ticket_3.deleted = 0 and ticket_3.primary_canceled = 0

			   ), 0),

		ticket_1.primary_marked,

		ticket_1.primary_pending,

		ticket_1.primary_confirmed,

		ticket_1.primary_canceled,

		ticket_1.primary_error_code,

		ticket_1.secondary_marked,

		ticket_1.secondary_pending,

		ticket_1.secondary_confirmed,

		ticket_1.secondary_canceled,

		ticket_1.secondary_error_code,

		ticket_1.side_code,

		ticket_1.trade_date,

		ticket_1.settlement_date,

		ticket_1.quantity_executed,

		ticket_1.average_price_executed,

		ticket_1.commission,

		ticket_1.taxes,

		ticket_1.other_charges,

		ticket_1.local_commission,

		ticket_1.exchange_fee,

		ticket_1.stamp_tax,

		ticket_1.levy,

		ticket_1.other_taxes_fees,

		ticket_1.accrued_income,

		ticket_1.event_counter,

		ticket_1.created_time,

		ticket_1.modified_by,

		ticket_1.modified_time,

		ticket_1.reviewed_by,

		ticket_1.reviewed_time,

		ticket_1.primary_sent_by,

		ticket_1.primary_sent_time,

		ticket_1.secondary_sent_by,

		ticket_1.secondary_sent_time,

		ticket_1.block_id,

		ticket_1.ticket_type_code,

		ticket_1.broker_ticket_id,

		ticket_1.deleted,

		ticket_1.primary_modified,

		ticket_1.secondary_modified,

		ticket_2.execution_broker_id,

		commission_type.imputed_flag,

		account.short_name,

		account.name_1,

		account.name_2,

		account.user_id_1,

		account.user_id_2,

		account.user_id_3,

		account.user_id_4,

		account.user_id_5,

		account.user_id_6,

		account.user_id_7,

		account.user_id_8,

		ticket_1.is_electronic_execution,

		ticket_1.warehousing_flag,

		0,

		ticket_1.settlement_fx_rate,

		1,

		ticket_1.commission,

		ticket_1.user_field_1,		

		ticket_1.user_field_2,		

		ticket_1.user_field_3,		

		ticket_1.user_field_4,		

		ticket_1.user_field_5,		

		ticket_1.user_field_6,		

		ticket_1.user_field_7,		

		ticket_1.user_field_8,		

		ticket_1.user_field_9,		

		ticket_1.user_field_10,	

		ticket_1.user_field_11,	

		ticket_1.user_field_12,	

		ticket_1.user_field_13,	

		ticket_1.user_field_14,	

		ticket_1.user_field_15,	

		ticket_1.user_field_16,

		ticket_1.user_field_17,

		ticket_1.user_field_18,

		ticket_1.user_field_19,

		ticket_1.user_field_20,

		ticket_1.user_field_21,

		ticket_1.user_field_22,

		ticket_1.user_field_23,

		ticket_1.user_field_24,

		ticket_1.user_field_25,

		ticket_1.user_field_26,

		ticket_1.user_field_27,

		ticket_1.user_field_28,

		ticket_1.user_field_29,

		ticket_1.user_field_30,

		ticket_1.user_field_31,

		ticket_1.user_field_32,

		ticket_1.original_ticket_id,

		ticket_1.trade_time,

		ticket_1.comm_fee_sched_id,

		commission_fee_schedule.schedule_mnemonic,

		1,

		ticket_1.counterparty_id,

		ticket_1.swt_assign_unwind_type,

		ticket_1.booking_price,

		ticket_1.traded_price,

		ticket_1.contract_id,

		ticket_1.note,

		currency.mnemonic,

		swp_security.funded_flag

	from

		#blocks_acct

		join ticket ticket_1 on #blocks_acct.block_id = ticket_1.block_id

		left outer join ticket ticket_2 on ticket_1.original_ticket_id = ticket_2.ticket_id

		left outer join debt_ticket on ticket_1.ticket_id = debt_ticket.ticket_id

		left outer join account on #blocks_acct.account_id = account.account_id

		join security on ticket_1.security_id = security.security_id

		left outer join swp_security

			on ticket_1.security_id = swp_security.security_id

		left outer join swt_contract

			on ticket_1.contract_id = swt_contract.contract_id

		join currency on ticket_1.settlement_currency_id = currency.security_id

		left outer join commission_type on ticket_1.commission_type_code = commission_type.commission_type_code

		left outer join commission_fee_schedule on ticket_1.comm_fee_sched_id = commission_fee_schedule.comm_fee_sched_id

	where

		--ticket_1.execution_broker_id = coalesce(@broker_id, ticket_1.execution_broker_id) and

		ticket_1.deleted + 0 = 0		

	option (keepfixed plan);

update 	#ticket_rprt

	set		fx_price_direction = cross_currency.direction

	from 	#ticket_rprt,cross_currency,security

	where	security.principal_currency_id = cross_currency.principal_currency_id and

			security.settlement_currency_id = cross_currency.counter_currency_id and

			security.security_id=#ticket_rprt.security_id

	option (keepfixed plan)

	update #ticket_rprt

	set principal_factor = 1.0

	where #ticket_rprt.major_asset_code = 6

	option (keepfixed plan);

	update #ticket_rprt

	set average_price_executed = 1 / average_price_executed

	where major_asset_code = 6

		and fx_price_direction = 0

		and average_price_executed <> 0.0

	option (keepfixed plan);

update #ticket_rprt

	set commission_rate_type_code = equity_ticket.commission_rate_type_code,

			commission_rate = equity_ticket.commission_rate,

			commission_calculated_flag = equity_ticket.commission_calculated_flag

	from	equity_ticket

	where	#ticket_rprt.ticket_id = equity_ticket.ticket_id

	option (keepfixed plan)

update #ticket_rprt

		set commission_rate_type_code = debt_ticket.commission_rate_type_code,

			commission_rate = debt_ticket.commission_rate,

			commission_calculated_flag = debt_ticket.commission_calculated_flag

	from	debt_ticket

	where	#ticket_rprt.ticket_id = debt_ticket.ticket_id

	option (keepfixed plan)

update #ticket_rprt

	set

		reviewer_name = reviewer.name,

		primary_sender_name = primary_sender.name,

		secondary_sender_name = secondary_sender.name,

		mnemonic = broker.mnemonic,

		trader_name = trader.name,

		exchange_name = exchange.name,

		exchange_mnemonic = exchange.mnemonic,

		reason_description = commission_reason.description,

		broker_comment = oasys_ticket.broker_comment,

		original_broker_mnemonic = broker2.mnemonic,

		comm_rate_type_mnemonic = comm_fee_rate_type.mnemonic,

		exchange_country_code = exchange.country_code,

		exchange_listed_flag = exchange.listed,

		counterparty_short_name = 

		case 

			when #ticket_rprt.major_asset_code = 12  

			then counterparty.mnemonic

			else issuer.short_name

			end

	from

		#ticket_rprt

		left outer join user_info reviewer on #ticket_rprt.reviewer_user_id = reviewer.user_id

		left outer join user_info primary_sender on #ticket_rprt.primary_sender_user_id = primary_sender.user_id

		left outer join user_info secondary_sender on #ticket_rprt.secondary_sender_user_id = secondary_sender.user_id

		left outer join broker on #ticket_rprt.execution_broker_id = broker.broker_id

		left outer join user_info trader on #ticket_rprt.created_by = trader.user_id

		left outer join exchange on #ticket_rprt.exchange_code = exchange.exchange_code

		left outer join commission_reason on #ticket_rprt.commission_reason_code = commission_reason.commission_reason_code

		left outer join comm_fee_rate_type on #ticket_rprt.commission_rate_type_code = comm_fee_rate_type.comm_fee_rate_type_code

		left outer join oasys_ticket on #ticket_rprt.ticket_id = oasys_ticket.ticket_id

		left outer join broker broker2 on #ticket_rprt.orig_broker_id = broker2.broker_id

		left outer join issuer on #ticket_rprt.counterparty_id = issuer.issuer_id

		left outer join counterparty on #ticket_rprt.counterparty_id = counterparty.counterparty_id

	option (keepfixed plan)

	update #ticket_rprt

	set fx_forward_flag = 1

	where exists 

	(

		select 1 

		from fx_forward_map 

		where fx_forward_map.ticket_id = #ticket_rprt.ticket_id

	)

	option (keepfixed plan);

update #ticket_rprt

		set net_amount = 

		case 

			when #ticket_rprt.major_asset_code = 12 

			and #ticket_rprt.minor_asset_code = 101

			and #ticket_rprt.swap_funded_flag = 0 then 0.0

			else 

				round(

					CONVERT(numeric(30, 10),#ticket_rprt.quantity_executed) * 

					CONVERT(numeric(30, 10),#ticket_rprt.price) * 

					CONVERT(numeric(30, 10),security.pricing_factor 

													* #ticket_rprt.principal_factor

													* case 

															when security.major_asset_code <> 5 then 1.0

															when security.contract_size is null then 1.0

															when security.contract_size <= 0.0 then 1.0

															else security.contract_size

															end) + 

					CONVERT(numeric(30, 10),#ticket_rprt.accrued_income) + 

					CONVERT(numeric(30, 10),side.security_sign) * 

						(

							CONVERT(numeric(30, 10),#ticket_rprt.paid_commission_flag) * 

							CONVERT(numeric(30, 10),#ticket_rprt.commission) + 

							CONVERT(numeric(30, 10),#ticket_rprt.taxes) + 

							CONVERT(numeric(30, 10),#ticket_rprt.other_charges) + 

							CONVERT(numeric(30, 10),#ticket_rprt.local_commission) + 

							CONVERT(numeric(30, 10),#ticket_rprt.exchange_fee) + 

							CONVERT(numeric(30, 10),#ticket_rprt.stamp_tax) + 

							CONVERT(numeric(30, 10),#ticket_rprt.levy) + 

							CONVERT(numeric(30, 10),#ticket_rprt.other_taxes_fees)

						), 

					CONVERT(numeric(30, 10),#ticket_rprt.market_value_rounding))

			end

		from

			#ticket_rprt,

			security,

			side

		where

			#ticket_rprt.security_id = security.security_id and

			#ticket_rprt.side_code = side.side_code and

			#ticket_rprt.deleted = 0 and

			((#ticket_rprt.quantity_executed > 0) or 

			 ((#ticket_rprt.quantity_executed <=0 and (#ticket_rprt.allocated = 1 or #ticket_rprt.primary_confirmed = 1)))) and

			(convert(tinyint,  #ticket_rprt.primary_confirmed) in (0, @include_confirmed_tickets) or #ticket_rprt.primary_modified = 1)

	select

		#ticket_rprt.security_id,

		#ticket_rprt.major_asset_code,

		security.minor_asset_code,

		security.name_1

			as security_name_1,

		security.name_2

			as security_name_2,

		security.name_3

			as security_name_3,

		security.symbol,

		security.principal_currency_id

			as principal_curr,

		security.income_currency_id

			as income_curr,

		security.pricing_factor,

		coalesce(swt_contract.principal_factor, security.principal_factor) as principal_factor,

		security.income_factor,

		price.latest

			as price_previous,

		price.asked

			as price_ask,

		price.bid

			as price_bid,

		major_asset.description

			as major_asset_description,

		blocked_orders.quantity_ordered,

		blocked_orders.market_value_ordered,

		blocked_orders.quantity_confirmed,

		blocked_orders.total_quantity_executed,

		case 

			when #ticket_rprt.major_asset_code = 6 

				and #ticket_rprt.fx_price_direction = 0 

				and coalesce(blocked_orders.total_average_price_executed, 0.0) <> 0.0

					then 1/blocked_orders.total_average_price_executed

			else blocked_orders.total_average_price_executed

			end as total_average_price,

		blocked_orders.limit_type_code_ordered

			as block_price_limit_type,

		blocked_orders.best_price_1_ordered

			as block_price_limit_1,

		blocked_orders.best_price_2_ordered

			as block_price_limit_2,

		blocked_orders.settlement_date

			as block_settle_date,

		convert(tinyint,  #ticket_rprt.archived)

			as status_archived,

		0

			as status_currently_archived,

		convert(tinyint,  #ticket_rprt.closed)

			as status_closed,

		convert(tinyint,  #ticket_rprt.engaged)

			as status_engaged,

		convert(tinyint,  #ticket_rprt.allocated)

			as status_allocated,

		convert(tinyint,  #ticket_rprt.step_out)

			as step_out,

		convert(tinyint,  #ticket_rprt.primary_marked)

			as status_primary_marked,

		convert(tinyint,  #ticket_rprt.primary_pending)

			as status_primary_pending,

		convert(tinyint,  #ticket_rprt.primary_confirmed)

			as status_primary_confirmed,

		convert(tinyint,  #ticket_rprt.primary_canceled)

			as status_primary_canceled,

		#ticket_rprt.primary_error_code,

		convert(tinyint,  #ticket_rprt.secondary_marked)

			as status_secondary_marked,

		convert(tinyint,  #ticket_rprt.secondary_pending)

			as status_secondary_pending,

		convert(tinyint,  #ticket_rprt.secondary_confirmed)

			as status_secondary_confirmed,

		convert(tinyint,  #ticket_rprt.secondary_canceled)

			as status_secondary_canceled,

		#ticket_rprt.secondary_error_code,

		#ticket_rprt.ticket_id,

		#ticket_rprt.side_code

			as direction,

		#ticket_rprt.execution_broker_id

			as broker_id,

		convert(tinyint,  #ticket_rprt.stepped_out)

			as stepped_out,

		#ticket_rprt.exchange_code,

		#ticket_rprt.trade_date,

		#ticket_rprt.settlement_date,

		#ticket_rprt.commission_type_code

			as commission_type,

		#ticket_rprt.commission_reason_code

			as commission_reason,

		broker_ticket.quantity_placed,

		broker_ticket.market_value_placed

			as mv_placed,

		broker_ticket.limit_type_code_placed

			as price_limit_type,

		broker_ticket.limit_price_1_placed

			as price_limit_1,

		broker_ticket.limit_price_2_placed

			as price_limit_2,

		#ticket_rprt.quantity_executed,

		#ticket_rprt.average_price_executed

			as average_price,

		null

			as unused_col1,						

		null

			as unused_col2,						

		#ticket_rprt.commission,

		#ticket_rprt.taxes,

		#ticket_rprt.other_charges,

		#ticket_rprt.local_commission

			as commission_local,

		#ticket_rprt.exchange_fee,

		#ticket_rprt.stamp_tax

			as stamp_taxes,

		#ticket_rprt.levy,							  

		#ticket_rprt.other_taxes_fees,

		convert(float, #ticket_rprt.net_amount)

			as net_amount,

  		#ticket_rprt.event_counter,

		#ticket_rprt.created_by

			as creator,

		#ticket_rprt.created_time

			as create_time,

		#ticket_rprt.modified_by

			as modifier,

		#ticket_rprt.modified_time,

		#ticket_rprt.reviewed_by

			as reviewer,

		#ticket_rprt.reviewed_time

			as review_time,

		#ticket_rprt.primary_sent_by

			as primary_sender,

		#ticket_rprt.primary_sent_time

			as primary_send_time,

		#ticket_rprt.secondary_sent_by

			as secondary_sender,

		#ticket_rprt.secondary_sent_time

			as secondary_send_time,

		exchange_mnemonic,

		exchange_name,

		#ticket_rprt.type_description

			as commission_type_name,

		#ticket_rprt.reason_description

			as commission_reason_name,

		#ticket_rprt.mnemonic

			as broker_name,

		trader_name,

		reviewer_name,

		primary_sender_name,

		secondary_sender_name,

		side.mnemonic

			as direction_sh_name,

		@display_currency_id_local

			as display_curr,

		#ticket_rprt.block_id,

		#ticket_rprt.original_broker_mnemonic,

		#ticket_rprt.directed_broker_count

			as directed_broker,

		security.user_id_1,

		security.user_id_2,

		security.user_id_3,

		security.user_id_4,

		security.user_id_5,

		security.user_id_6,

		security.user_id_7,

		security.user_id_8,

		convert(tinyint,  #ticket_rprt.primary_modified)

			as primary_modified_flag,

		convert(tinyint,  #ticket_rprt.secondary_modified)

			as secondary_modified_flag,

		#ticket_rprt.ticket_type_code,

		#ticket_rprt.broker_ticket_id,

		substring(#ticket_rprt.type_description, 1, 1)

			as commission_type_sh_name,

		substring(#ticket_rprt.reason_description, 1, 5)

			as commission_reason_sh_name,

		#ticket_rprt.fx_forward_flag

			as status_fx_forward,

		#ticket_rprt.broker_comment

			as oasys_note,

		#ticket_rprt.imputed_flag

			as imputed_commission_flag,

		account_short_name

			as account_sh_name,

		account_name_1,

		account_name_2,

		account_user_id_1,

		account_user_id_2,

		account_user_id_3,

		account_user_id_4,

		account_user_id_5,

		account_user_id_6,

		account_user_id_7,

		account_user_id_8,

		#ticket_rprt.is_electronic_execution

			as electronic_execution_flag,

		#ticket_rprt.warehousing_flag

			as status_warehoused,

		#ticket_rprt.fx_price_direction,

		#ticket_rprt.settlement_fx_rate,

		#ticket_rprt.commission_rate_type_code,

		#ticket_rprt.comm_rate_type_mnemonic

			as commission_rate_type_mnemonic,

		#ticket_rprt.commission_rate,

		#ticket_rprt.ticket_user_field_1,

		#ticket_rprt.ticket_user_field_2,

		#ticket_rprt.ticket_user_field_3,

		#ticket_rprt.ticket_user_field_4,

		#ticket_rprt.ticket_user_field_5,

		#ticket_rprt.ticket_user_field_6,

		#ticket_rprt.ticket_user_field_7,

		#ticket_rprt.ticket_user_field_8,

		#ticket_rprt.ticket_user_field_9,

		#ticket_rprt.ticket_user_field_10,

		#ticket_rprt.ticket_user_field_11,

		#ticket_rprt.ticket_user_field_12,

		#ticket_rprt.ticket_user_field_13,

		#ticket_rprt.ticket_user_field_14,

		#ticket_rprt.ticket_user_field_15,

		#ticket_rprt.ticket_user_field_16,

		blocked_orders.user_field_1

			as blocked_order_user_field_1,

		blocked_orders.user_field_2

			as blocked_order_user_field_2,

		blocked_orders.user_field_3

			as blocked_order_user_field_3,

		blocked_orders.user_field_4

			as blocked_order_user_field_4,

		coalesce(user_field_list_items_5.description, blocked_orders.user_field_5)

			as blocked_order_user_field_5,

		coalesce(user_field_list_items_6.description, blocked_orders.user_field_6)

			as blocked_order_user_field_6,

		coalesce(user_field_list_items_7.description, blocked_orders.user_field_7)

			as blocked_order_user_field_7,

		coalesce(user_field_list_items_8.description, blocked_orders.user_field_8)

			as blocked_order_user_field_8,

		blocked_orders.user_field_9

			as blocked_order_user_field_9,

		blocked_orders.user_field_10

			as blocked_order_user_field_10,

		blocked_orders.user_field_11

			as blocked_order_user_field_11,

		blocked_orders.user_field_12

			as blocked_order_user_field_12,

		coalesce(user_field_list_items_13.description, blocked_orders.user_field_13)

			as blocked_order_user_field_13,

		coalesce(user_field_list_items_14.description, blocked_orders.user_field_14)

			as blocked_order_user_field_14,

		coalesce(user_field_list_items_15.description, blocked_orders.user_field_15)

			as blocked_order_user_field_15,

		coalesce(user_field_list_items_16.description, blocked_orders.user_field_16)

			as blocked_order_user_field_16,

		#ticket_rprt.original_ticket_id,

		#ticket_rprt.trade_time,

		security.coupon,

		security.maturity_date,

		#ticket_rprt.commission_calculated_flag,

		#ticket_rprt.exchange_country_code,

		#ticket_rprt.exchange_listed_flag,

		security.security_attribute,

		security.par_value

			as security_par_value,

		blocked_orders.strategy_id,

		#ticket_rprt.accrued_income

			as accrued_interest,

		#ticket_rprt.comm_fee_sched_id

			as commission_schedule_id,

		#ticket_rprt.commission_schedule_mnemonic,

		#ticket_rprt.counterparty_id,

		#ticket_rprt.counterparty_short_name,

		security.repo_spread,

		security.repo_investment_rate,

		blocked_orders.user_field_17

			as blocked_order_user_field_17,

		blocked_orders.user_field_18

			as blocked_order_user_field_18,

		blocked_orders.user_field_19

			as blocked_order_user_field_19,

		blocked_orders.user_field_20

			as blocked_order_user_field_20,

		coalesce(user_field_list_items_21.description, blocked_orders.user_field_21)

			as blocked_order_user_field_21,

		coalesce(user_field_list_items_22.description, blocked_orders.user_field_22)

			as blocked_order_user_field_22,

		coalesce(user_field_list_items_23.description, blocked_orders.user_field_23)

			as blocked_order_user_field_23,

		coalesce(user_field_list_items_24.description, blocked_orders.user_field_24)

			as blocked_order_user_field_24,

		blocked_orders.user_field_25

			as blocked_order_user_field_25,

		blocked_orders.user_field_26

			as blocked_order_user_field_26,

		blocked_orders.user_field_27

			as blocked_order_user_field_27,

		blocked_orders.user_field_28

			as blocked_order_user_field_28,

		coalesce(user_field_list_items_29.description, blocked_orders.user_field_29)

			as blocked_order_user_field_29,

		coalesce(user_field_list_items_30.description, blocked_orders.user_field_30)

			as blocked_order_user_field_30,

		coalesce(user_field_list_items_31.description, blocked_orders.user_field_31)

			as blocked_order_user_field_31,

		coalesce(user_field_list_items_32.description, blocked_orders.user_field_32)

			as blocked_order_user_field_32,

		#ticket_rprt.ticket_user_field_17,

		#ticket_rprt.ticket_user_field_18,

		#ticket_rprt.ticket_user_field_19,

		#ticket_rprt.ticket_user_field_20,

		#ticket_rprt.ticket_user_field_21,

		#ticket_rprt.ticket_user_field_22,

		#ticket_rprt.ticket_user_field_23,

		#ticket_rprt.ticket_user_field_24,

		#ticket_rprt.ticket_user_field_25,

		#ticket_rprt.ticket_user_field_26,

		#ticket_rprt.ticket_user_field_27,

		#ticket_rprt.ticket_user_field_28,

		#ticket_rprt.ticket_user_field_29,

		#ticket_rprt.ticket_user_field_30,

		#ticket_rprt.ticket_user_field_31,

		#ticket_rprt.ticket_user_field_32,

		#ticket_rprt.swt_assign_unwind_type,

		#ticket_rprt.booking_price,

		#ticket_rprt.traded_price,

		#ticket_rprt.contract_id,

		#ticket_rprt.trader_note,

		blocked_orders.block_note,

		#ticket_rprt.currency_mnemonic,

		blocked_orders.package_id

			as package_id,

		package.name_1

			as package_name,

		entity.name as contract_ccp_name,

		entity.legal_entity_id as contract_ccp_lei

	from #ticket_rprt

	join broker_ticket 

		on #ticket_rprt.broker_ticket_id = broker_ticket.broker_ticket_id

	join blocked_orders 

		on #ticket_rprt.block_id = blocked_orders.block_id and broker_ticket.block_id = blocked_orders.block_id

	join security 

		on #ticket_rprt.security_id = security.security_id

	left outer join swt_contract

		on blocked_orders.contract_id = swt_contract.contract_id

	left outer join entity

		on swt_contract.central_counterparty_id = entity.entity_id

	left outer join package

		on blocked_orders.package_id = package.package_id

	join major_asset 

		on security.major_asset_code = major_asset.major_asset_code

	join side 

		on #ticket_rprt.side_code = side.side_code and broker_ticket.side_code = side.side_code and blocked_orders.side_code = side.side_code

	left outer join price 

		on #ticket_rprt.security_id = price.security_id

	left outer join 

		(

		    select user_field_list_items.mnemonic, user_field_list_items.description

		    from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 5

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_5

			on blocked_orders.user_field_5 = user_field_list_items_5.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

		    from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 6

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_6

			on blocked_orders.user_field_6 = user_field_list_items_6.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 7

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_7

			on blocked_orders.user_field_7 = user_field_list_items_7.mnemonic

		left outer join (

			select user_field_list_items.mnemonic, user_field_list_items.description

		    from user_field_list_items

			join order_reference_definitions

			    on order_reference_definitions.column_id = 8

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_8

			on blocked_orders.user_field_8 = user_field_list_items_8.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

		    join order_reference_definitions

				on order_reference_definitions.column_id = 13

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_13

			on blocked_orders.user_field_13 = user_field_list_items_13.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

		    join order_reference_definitions

		        on order_reference_definitions.column_id = 14

			    and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_14

			on blocked_orders.user_field_14 = user_field_list_items_14.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

		    join order_reference_definitions

		        on order_reference_definitions.column_id = 15

			    and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_15

			on blocked_orders.user_field_15 = user_field_list_items_15.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 16

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_16

			on blocked_orders.user_field_16 = user_field_list_items_16.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

		    from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 21

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_21

			on blocked_orders.user_field_21 = user_field_list_items_21.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

		    from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 22

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_22

			on blocked_orders.user_field_22 = user_field_list_items_22.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 23

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_23

			on blocked_orders.user_field_23= user_field_list_items_23.mnemonic

		left outer join (

			select user_field_list_items.mnemonic, user_field_list_items.description

		    from user_field_list_items

			join order_reference_definitions

			    on order_reference_definitions.column_id = 24

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_24

			on blocked_orders.user_field_24 = user_field_list_items_24.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

		    join order_reference_definitions

				on order_reference_definitions.column_id = 29

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_29

			on blocked_orders.user_field_29 = user_field_list_items_29.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

		    join order_reference_definitions

		        on order_reference_definitions.column_id = 30

			    and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_30

			on blocked_orders.user_field_30 = user_field_list_items_30.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

		    join order_reference_definitions

		        on order_reference_definitions.column_id = 31

			    and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

		) user_field_list_items_31

			on blocked_orders.user_field_31 = user_field_list_items_31.mnemonic

		left outer join (

		    select user_field_list_items.mnemonic, user_field_list_items.description

			from user_field_list_items

			join order_reference_definitions

				on order_reference_definitions.column_id = 32

				and user_field_list_items.user_field_list_id = order_reference_definitions.user_field_list_id

	    ) user_field_list_items_32

			on blocked_orders.user_field_32 = user_field_list_items_32.mnemonic

	where

		#ticket_rprt.deleted = 0 and

		blocked_orders.deleted = 0 and

		((#ticket_rprt.quantity_executed > 0) or 

		 ((#ticket_rprt.quantity_executed <=0 and

		(#ticket_rprt.allocated = 1 or #ticket_rprt.primary_confirmed = 1)))) and

		(convert(tinyint,  #ticket_rprt.primary_confirmed) in (0, @include_confirmed_tickets) or #ticket_rprt.primary_modified = 1)

	option (force order)		

	;

end
