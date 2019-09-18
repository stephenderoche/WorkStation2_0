/******************************************************************************
* OBJECT 	 		: psg_bcp_wash_sale_list
* PROJECT NAME		: Wash Sale Rule
* DESCRIPTION		: Holds the restricted securities for wash sale check 1
*******************************************************************************/

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'se_wash_sale_list')   
begin
	PRINT 'se_wash_sale_list'
end	
else
begin
	CREATE TABLE se_wash_sale_list
	(trade_date     DATETIME    NULL,
	 pfid           VARCHAR(40) NULL,
	 symbol         VARCHAR(16) NULL,
	 cusip          VARCHAR(16) NULL,
	 real_gain_loss FLOAT       NULL,
	 salePrice FLOAT       NULL)
	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'se_wash_sale_list')
	BEGIN
	  PRINT 'se_wash_sale_list'
	END
	else
	begin
		PRINT 'se_wash_sale_list'
	end
	
end	
GO

select * from se_wash_sale_list

truncate table se_wash_sale_list

insert into se_wash_sale_list values('06/15/2019','Fund01','TSCO.US',null,2000,85)
insert into se_wash_sale_list values('06/15/2019','Fund03','ACIW.US',null,2000,21)
insert into se_wash_sale_list values('06/15/2019','Fund03','AME.US',null,2000,30)
insert into se_wash_sale_list values('06/01/2019','Fund03','BANR.US',null,-2000,52)
insert into se_wash_sale_list values('06/15/2019','Fund02','ABMD.US',null,2000,112.56)
insert into se_wash_sale_list values('06/15/2019','Fund02','ACIW.US',null,2000,21)
insert into se_wash_sale_list values('06/15/2019','Fund02','AME.US',null,2000,30)
insert into se_wash_sale_list values('06/01/2019','Fund02','BANR.US',null,-2000,52)

