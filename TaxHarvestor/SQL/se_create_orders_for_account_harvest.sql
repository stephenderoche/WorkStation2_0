if exists (select * from sysobjects where name = 'se_create_orders_for_account_harvest')
begin
	drop procedure se_create_orders_for_account_harvest
	print 'PROCEDURE: se_create_orders_for_account_harvest dropped'
end
go
--select * from security where 
--select * from orders

create procedure [dbo].[se_create_orders_for_account_harvest]
--exec se_create_orders_for_account_harvest @account_id=199,@security_id=5643,@quantity=100,@SideMnemonic=N'S',@Tax_quantity=100,@Tax_cost=0,@SettmentDate='2014-12-26 00:00:00',@Tax_lot_id=3470
(       
  @account_id           numeric(10),
  @security_id           numeric(10),
  @quantity                  float,
  @SideMnemonic        varchar(40) = 'S',
  @Tax_quantity                float = 100,
  @Tax_cost               float = 0,
  @SettmentDate                datetime = '2016-06-02 00:00:00',
  @Tax_lot_id           numeric(10) = 9441
 )      
as    
declare @trader_id numeric(10)
declare @side_code numeric(10)
declare @ticket_type_code numeric(10)
declare @security varchar(40)
DECLARE @order_id NUMERIC(10)      
DECLARE @link_id NUMERIC(10)      
DECLARE @contingent_id NUMERIC(10)   
DECLARE @ret_val INT;  
begin


select @trader_id = trader_id from account where account_id = @account_id
select @security = symbol from security where security_id = @security_id


---------------------------------------------side---------------------------------------------------------
 SELECT      
        @side_code = side_code      
    FROM      
        side      
    WHERE  mnemonic = RTRIM(LTRIM(UPPER(@SideMnemonic)))      
      
    IF (@side_code IS NULL)      
    BEGIN      
        SELECT      
            @side_code = side_code      
        FROM      
            side      
        WHERE  UPPER(DESCRIPTION) = RTRIM(LTRIM(UPPER(@SideMnemonic)))      
      
        IF (@side_code IS NULL)      
        BEGIN      
            RAISERROR(N'Unknown side %s.',      
                      10,      
                      1,      
                      @SideMnemonic)      
      
           RETURN -1      
        END      
    END      
-----------------------------------------end side---------------------------------------------------------

-----------------------------------------ticket type------------------------------------------------------
 SELECT      
  @ticket_type_code = CASE      
         WHEN m.description = 'EQUITY' THEN (SELECT      
                   ticket_type_code      
                  FROM      
                   ticket_type      
                  WHERE  mnemonic = 'EQU')--select * from ticket_type      
         WHEN m.description = 'DEBT' THEN (SELECT      
                 ticket_type_code      
                FROM      
                 ticket_type      
                WHERE  mnemonic = 'DSEC')      
         WHEN m.description = 'FOREX' THEN (SELECT      
                  ticket_type_code      
                 FROM      
                  ticket_type      
                 WHERE  mnemonic = 'SPT')      
         ELSE (SELECT      
          ticket_type_code      
         FROM      
          ticket_type      
         WHERE  major_asset_code = m.major_asset_code)      
       END      
 FROM      
  security s      
 JOIN   ticket_type t     
  ON s.major_asset_code = t.major_asset_code      
 JOIN   major_asset m      
  ON t.major_asset_code = m.major_asset_code      
 WHERE  s.security_id = @security_id      
      
    IF (@ticket_type_code IS NULL)      
    BEGIN      
        RAISERROR(N'Unknown ticket type code for security %s.',      
                  10,      
                  1,      
           @security)      
      
        RETURN -1      
    END      
-------------------------------------end ticket type------------------------------------------------------

  EXEC @ret_val = ADD_PROPOSED      
        @order_id OUTPUT,--  @order_id numeric(10) output,      
        @link_id OUTPUT,--  @link_id numeric(10) output,      
        @contingent_id OUTPUT,--  @contingent_id numeric(10) output,      
        @account_id,--  @account_id numeric(10),       
        @security_id,--  @security_id numeric(10),       
        @side_code,--  @side_code tinyint,       
        @ticket_type_code,--  @ticket_type_code tinyint,       
        @quantity,--  @quantity float,       
        0,--  @accrued_income float,       
        @Tax_cost,--  @tax_lot_cost float = null,              
        @Tax_quantity,--  @tax_lot_quantity float = null,                       
        NULL,--  @time_in_force_code tinyint = null,      
        NULL,--  @limit_type_code tinyint = null,      
        NULL,--  @limit_price_1 float = null,      
        NULL,--  @limit_price_2 float = null,      
        NULL,--  @trader_id numeric(10) = null,      
        NULL,--  @directed_broker_id numeric(10) = null,      
        @SettmentDate,--  @settlement_date datetime = null,      
        NULL,--  @note varchar(255) = null,      
        NULL,--  @display_currency_id numeric(10) = null,      
        0,--  @baseline_price_tag_type tinyint = 0,      
        NULL,--  @program_id numeric(10) = null,      
        NULL,--  @index_id int = null,      
        0,--  @index_price_tag_type tinyint = 0,      
        NULL,--  @user_field_1 float = null,      
        NULL,--  @user_field_2 float = null,      
        NULL,--  @user_field_3 float = null,      
        NULL,--  @user_field_4 float = null,      
        'TaxHarvest',--  @user_field_5 varchar(40) = null,      
        NULL,--  @user_field_6 varchar(40) = null,      
        NULL,--  @user_field_7 varchar(40) = null,      
        NULL,--  @user_field_8 varchar(40) = null,      
        1,--  @display_error_messages int = 1,      
        NULL,--  @trade_reason_id smallint = null,      
        NULL,--  @manual_accrued_flag tinyint = null,      
        NULL,--  @accrued_days int = null,      
        198, --  @current_user numeric(10),  
		NULL, --strategy_id
		NULL,  --book_id
		Null, --link_flag
		NULL, --contingent_id
		NULL,  -- custodian_id 
		NULL, --deleiver,
		Null,--custody
		NULL,--user_field_9
		NULL,--user_field_10
		NULL,--user_field_11
		NULL,--user_field_12
		NULL,--user_field_13
		NULL,--user_field_14
		NULL,--user_field_15
		NULL,--user_field_16
		NULL,--directed_counter
		null,
		10 ---manual Tax lot releif code

		 exec add_order_tax_lot @Tax_lot_id, @order_id, @quantity , null, 189.00000000, 1


end


go
if @@error = 0 print 'PROCEDURE: se_create_orders_for_account_harvest created'
else print 'PROCEDURE: se_create_orders_for_account_harvest error on creation'
go