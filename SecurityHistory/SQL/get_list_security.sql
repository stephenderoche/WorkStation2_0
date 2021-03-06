USE [LVTS_753]
GO
/****** Object:  StoredProcedure [dbo].[get_list_security]    Script Date: 7/31/2019 11:17:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER procedure [dbo].[get_list_security]--get_list_security -1
(	@major_asset_code smallint 
) 
as
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	select
		security.security_id,
		security.symbol as mnemonic,
		security.user_id_1,
		security.user_id_2,
		security.user_id_3,
		security.user_id_4,
		security.user_id_5,
		security.user_id_6,
		security.user_id_7,
		security.user_id_8,
		security.user_id_9,  
		security.user_id_10, 
		security.user_id_11, 
		security.user_id_12, 
		security.user_id_13, 
		security.user_id_14, 
		security.user_id_15, 
		security.user_id_16, 
		security.name_1,
		security.debt_type_code
	from security
	where (@major_asset_code = security.major_asset_code  or @major_asset_code = -1)
	and security.deleted = 0
	order by security.symbol;
end
