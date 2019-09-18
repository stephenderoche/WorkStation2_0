--select * from account where short_name like '%wm%'

if exists (select * from sysobjects where name = 'se_get_account_replacement')
begin
	drop procedure se_get_account_replacement
	print 'PROCEDURE: se_get_account_replacement dropped'
end
go
create PROCEDURE [dbo].[se_get_account_replacement]  --se_get_account_replacement 238
(
	@account_id	numeric(10)
)
AS
declare @hierarchy_id numeric(10)
BEGIN

--select * from country
select @hierarchy_id = hierarchy_id from hierarchy where name = 'by GICS'

	select se_account_replacement.replacement_id,
	se_account_replacement.deleted,
	se_account_replacement.account_id,
	se_account_replacement.buy_list_id,
	case 
	    when se_account_replacement.buy_list_type_id = 0 then (select symbol from security where security_id = se_account_replacement.buy_list_id)
		when se_account_replacement.buy_list_type_id = 1 then (select description from hierarchy_sector where hierarchy_id = @hierarchy_id and hierarchy_sector_id = se_account_replacement.buy_list_id)
	else (select mnemonic from country where country_code = se_account_replacement.buy_list_id)
	end as buy_list_name,
	se_account_replacement.buy_list_type_id,
	case 
	    when se_account_replacement.buy_list_type_id = 0 then 'Security'
		when se_account_replacement.buy_list_type_id = 1 then 'Sector'
	else 'Country'
	end as buy_list_type,
	se_account_replacement.replacement_list_id,
	case 
	    when se_account_replacement.replacement_list_type_id = 0 then (select symbol from security where security_id = se_account_replacement.replacement_list_id)
		when se_account_replacement.replacement_list_type_id = 1 then (select description from hierarchy_sector where hierarchy_id = @hierarchy_id and hierarchy_sector_id = se_account_replacement.replacement_list_id)
	else (select mnemonic from country where country_code = se_account_replacement.replacement_list_id)
	end as replacement_name,
	se_account_replacement.replacement_list_type_id,
		case 
	    when se_account_replacement.replacement_list_type_id = 0 then 'Security'
		when se_account_replacement.replacement_list_type_id = 1 then 'Sector'
	else 'Country'
	end as replacement_type,
	created.user_id as 'created_by_id',
	created.name as 'created_by',
	se_account_replacement.created_time,
	modified.user_id as 'modified_by_id',
	modified.name as 'modified_by',
	se_account_replacement.modified_time
	 from se_account_replacement 
	 join user_info created on
	 created.user_id = se_account_replacement.created_by
	 join user_info modified on
	 modified.user_id = se_account_replacement.modified_by
	 
	 where account_id = @account_id
	 and se_account_replacement.deleted = 0
END

go
if @@error = 0 print 'PROCEDURE: se_get_account_replacement created'
else print 'PROCEDURE: se_get_account_replacement error on creation'
go

