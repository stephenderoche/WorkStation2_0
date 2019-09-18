


create procedure [dbo].[se_create_orders_ipo]--exec se_create_orders_ipo 20095,248,100,'B','Client','Risk Ratio','Desc'
(       
  @security_id           numeric(10),
  @account_id           numeric(10),
  @quantity                  float,
  @SideMnemonic        varchar(40) = 'B'
 
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
        2,--  @ticket_type_code tinyint,       
        @quantity,--  @quantity float,       
        0,--  @accrued_income float,       
        NULL,--  @tax_lot_cost float = null,              
        NULL,--  @tax_lot_quantity float = null,                       
        NULL,--  @time_in_force_code tinyint = null,      
        NULL,--  @limit_type_code tinyint = null,      
        NULL,--  @limit_price_1 float = null,      
        NULL,--  @limit_price_2 float = null,      
        NULL,--  @trader_id numeric(10) = null,      
        -1,--  @directed_broker_id numeric(10) = null,      
        NULL,--  @settlement_date datetime = null,      
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
        NULL,--  @user_field_5 varchar(40) = null,      
        NULL,--  @user_field_6 varchar(40) = null,      
        NULL,--  @user_field_7 varchar(40) = null,      
        NULL,--  @user_field_8 varchar(40) = null,      
        1,--  @display_error_messages int = 1,      
        NULL,--  @trade_reason_id smallint = null,      
        NULL,--  @manual_accrued_flag tinyint = null,      
        NULL,--  @accrued_days int = null,      
        198 --  @current_user numeric(10),  


end

