
update user_info set password_required=0;
create table order_scratch_pkn      (      order_scratch_id							numeric(10) ,      block_id                                    numeric(10) NULL,      order_id									numeric(10),      account_id									numeric(10),      security_id                                 numeric(10),      side_code									tinyint NULL,      ticket_type_code							tinyint NULL,      quantity									float NULL,      tax_lot_cost								float NULL,      tax_lot_quantity							float NULL,      accrued_income								float NULL,      time_in_force_code							tinyint NULL,      limit_type_code								tinyint NULL,      limit_price_1								float NULL,      limit_price_2								float NULL,      trader_id                                   numeric(10) NULL,      directed_broker_id                          numeric(10) NULL,      settlement_date								datetime  NULL,      note										nvarchar(255) NULL, 	display_currency_id							numeric(10) NULL,  	originated_by								smallint    NULL,  	originated_time								datetime    NULL,  	baseline_price_tag_type						int    NULL,  	program_id                                  numeric(10) NULL,  	index_id									numeric(10) NULL,  	index_price_tag_type						int    NULL,  	lending_account_id                          numeric(10) NULL,  	user_field_1								float NULL,  	user_field_2								float NULL,  	user_field_3								float NULL,  	user_field_4								float NULL,  	user_field_5								nvarchar(40) NULL,  	user_field_6								nvarchar(40) NULL,  	user_field_7								nvarchar(40) NULL,  	user_field_8								nvarchar(40) NULL,  	trade_reason_id								smallint    NULL,  	compliance_check							tinyint NULL,  	manual_accrued_flag							int    NULL,  	accrued_days								int    NULL,  	strategy_id                                 numeric(10) NULL,  	link_id										numeric(10) NULL,  	contingent_id                               numeric(10) NULL,  	blocking_code                               numeric(10) NULL,  	rte_rule_id                                 numeric(10) NULL,  	user_field_9								float NULL,  	user_field_10								float NULL,  	user_field_11								float NULL,  	user_field_12								float NULL,  	user_field_13								nvarchar(40) NULL,  	user_field_14								nvarchar(40) NULL,  	user_field_15								nvarchar(40) NULL,  	user_field_16								nvarchar(40) NULL,  	directed_counter_party_id                   numeric(10) NULL,  	tax_lot_relief_method_code					tinyint NULL,  	delivery_date								datetime    NULL,  	book_id										numeric(10) NULL,  	force_new_block								numeric(10, 0) NULL,  	contract_id                                 numeric(10) NULL,  	orig_order_id                               numeric(10) NULL,  	approved_flag								tinyint NULL,  	quantity_estimated							float NULL,  	user_field_17								float NULL,  	user_field_18								float NULL,  	user_field_19								float NULL,  	user_field_20								float NULL,  	user_field_21								nvarchar(40) NULL,  	user_field_22								nvarchar(40) NULL,  	user_field_23								nvarchar(40) NULL,  	user_field_24								nvarchar(40) NULL,  	user_field_25								float NULL,  	user_field_26								float NULL,  	user_field_27								float NULL,  	user_field_28								float NULL,  	user_field_29								nvarchar(40) NULL,  	user_field_30								nvarchar(40) NULL,  	user_field_31								nvarchar(40) NULL,  	user_field_32								nvarchar(40) NULL,  	swt_assign_unwind_type						tinyint NULL,  	swt_pay_leg_accrued_interest				float NULL,  	swt_pay_leg_accrued_days					smallint    NULL,  	swt_rec_leg_accrued_interest				float NULL,  	swt_rec_leg_accrued_days					smallint    NULL,  	package_id                                  numeric(10) NULL,  	fcm_entity_id                               numeric(10) NULL,  	sef_entity_id                               numeric(10) NULL,  	ccp_entity_id                               numeric(10) NULL,  	dco_entity_id                               numeric(10) NULL,  	margin_by_account							float NULL,  	manual_settlement_date_flag					tinyint NULL,  	trade_date_offset							tinyint NULL,  	trade_date									datetime    NULL,  	last_order									tinyint NULL,  	major_asset_code							smallint NULL,  	security_symbol								nvarchar(40) NULL,  	security_deleted                            bit NULL,  	security_price_latest                       float NULL,  	security_price_closing                      float NULL,  	security_price_opening                      float NULL,  	security_exchange_rate                      float NULL,  	exchange_rate                               float NULL,  	average_price_precision                     smallint NULL,  	quantity_rounding                           smallint NULL,  	market_value_rounding                       smallint NULL,  	security_level_code                         tinyint NULL,  	default_trader_id                           numeric(10) NULL,  	default_time_in_force_code                  tinyint NULL,  	use_time_in_force_code                      tinyint NULL,  	broker_unlisted_override                    bit NULL,  	default_note                                nvarchar(255) NULL,  	major_account_code                          tinyint NULL,  	domiciled_flag                              tinyint NULL,  	account_manager                             numeric(10) NULL,  	account_regulation_code                     smallint NULL,  	logical_block_id                            numeric(10) NULL,  	market_value                                float NULL,  	tax_lot_market_value                        float NULL,  	equivalent_quantity                         float NULL,  	index_price                                 float NULL,  	note_indicator                              bit NULL,  	baseline_price                              float NULL,  	has_manual_settle_date                      bit NULL,  	violation_indicator                         bit NULL,  	exchange_code                               numeric(10) NULL,  	broker_type_code                            numeric(10) NULL,  	broker_mnemonic                             nvarchar(8) NULL,  	send_order_run_id                           numeric(10),  	decision_maker_id							numeric(10) NULL   );
--drop table sendorder_DB_perf;
create table sendorder_DB_perf( send_order_run_id numeric(10), start_time datetime, end_time datetime , rec_count numeric(10), step varchar(10))



/* Add a desk */

USE [lvts_753]
GO

DECLARE	@return_value int,
		@desk_id numeric(10, 0)

EXEC	@return_value = [dbo].[add_desk]
		@desk_id = @desk_id OUTPUT,
		@name = N'Pranab',
		@current_user = 1

SELECT	@desk_id as N'@desk_id'

SELECT	'Return Value' = @return_value

GO

/* Set blocking code for this desk */

insert into trader_blocking_code 
select 17,major_asset_code, 1,0
from major_asset;

/*

*/

update proposed_orders set trader_id=17, modified_by=1

exec sp_updatestats



select * from trader_blocking_code