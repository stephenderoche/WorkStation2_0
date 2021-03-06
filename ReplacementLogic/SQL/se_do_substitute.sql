--select * from account where short_name like '%wm%'

if exists (select * from sysobjects where name = 'se_do_substitute')
begin
	drop procedure se_do_substitute
	print 'PROCEDURE: se_do_substitute dropped'
end
go
create PROCEDURE [dbo].[se_do_substitute]  --se_do_substitute 10350
(
	@account_id	numeric(10)
)
AS
declare @security_id numeric(10)
--create table rebal_proposed_order_steve  	( 	status_bits					smallint,  	session_account_id			numeric(10),  	account_id					numeric(10),  	position_type_code			tinyint,  	security_id					numeric(10),  	quantity					float,  	market_value				float,  	side_code					tinyint,  	ticket_type_code			tinyint  null,  	settlement_date		    	datetime null,  	clear_proposed				tinyint not null,  	accrued_income				float null,  	accrued_days				int null,  	manual_accrued_flag		    tinyint null,  	default_broker_id			numeric(10) null,  	rebal_session_id			numeric(10) null,  	delivery_date				datetime null,  	trading_desk_id             numeric(10) null,  	mf_trade_date_offset		tinyint null,  	note                        nvarchar(255) null,  	time_in_force_code          tinyint null,  	trader_id                   numeric(10)  null,  	tax_lot_relief_method_code  tinyint  null,  	decision_maker_id			numeric(10) null,  	pre_rebal_continue_flag     tinyint not null,  	post_rebal_continue_flag    tinyint not null  	);
truncate table rebal_proposed_order_steve
BEGIN
	update
#rebal_proposed_order
set security_id  =20080.00
from #rebal_proposed_order 
	where not (quantity = 0.0 and market_value = 0.0)
	and security_id = 7689.00

	select @security_id = value from registry where section = 'test'

  insert 
	  into rebal_proposed_order_steve 
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
		from #rebal_proposed_order 
		--select * from rebal_proposed_order_steve 
	--select * from registry where section = 'test'
END

go
if @@error = 0 print 'PROCEDURE: se_do_substitute created'
else print 'PROCEDURE: se_do_substitute error on creation'
go

