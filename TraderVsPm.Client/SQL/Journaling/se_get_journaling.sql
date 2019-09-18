   /****** Object:  StoredProcedure [dbo].[se_get_trade_compliance_report]    Script Date: 07/18/2014 14:12:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[se_get_journaling]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[se_get_journaling]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

create procedure se_get_journaling  --se_get_journaling 34882

(  
	@block_id numeric(10) = null
	
) 

as

declare @violations_only_local tinyint;
declare	@ret_val int;
declare	@continue_flag bit;
declare	@cps_get_compliance_report nvarchar(30);
declare	@cpe_get_compliance_report nvarchar(30);

begin

                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
		
	

	Select * from se_journaling where block_id = @block_id
 
		

end;
	
