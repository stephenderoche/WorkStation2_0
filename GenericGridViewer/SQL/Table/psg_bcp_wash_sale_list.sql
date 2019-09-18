/******************************************************************************
* OBJECT 	 		: psg_bcp_wash_sale_list
* PROJECT NAME		: Wash Sale Rule
* DESCRIPTION		: Holds the restricted securities for wash sale check 1
*******************************************************************************/

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'psg_bcp_wash_sale_list')   
begin
	PRINT 'psg_bcp_wash_sale_list exists'
end	
else
begin
	CREATE TABLE psg_bcp_wash_sale_list
	(trade_date     DATETIME    NULL,
	 pfid           VARCHAR(40) NULL,
	 symbol         VARCHAR(16) NULL,
	 cusip          VARCHAR(16) NULL,
	 real_gain_loss FLOAT       NULL)

	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'psg_bcp_wash_sale_list')
	BEGIN
	  PRINT 'psg_bcp_wash_sale_list created'
	END
	else
	begin
		PRINT 'psg_bcp_wash_sale_list error on creation'
	end
	
end	
GO