   /****** Object:  StoredProcedure [dbo].[se_get_trade_compliance_report]    Script Date: 07/18/2014 14:12:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[se_get_price_journal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[se_get_price_journal]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

create procedure se_get_price_journal  --se_get_price_journal 162905,5927

(  
	@model_id numeric(10) = -1,
	@security_id numeric(10)= -1
	
) 

as



begin

                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
		
	

	select * from se_security_journal where se_security_journal.model_id = @model_id and se_security_journal.security_id = @security_id
	order by modified_time desc
 
		

end;
	
