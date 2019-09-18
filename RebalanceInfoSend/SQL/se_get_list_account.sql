

if exists (select * from sysobjects where name = 'se_get_list_account')
begin
	drop procedure se_get_list_account
	print 'PROCEDURE: se_get_list_account dropped'
end
go
create procedure [dbo].[se_get_list_account]--se_get_list_account 1,0,9,'wm'
(	
	@isAdmin		tinyint,
	@includeAdHoc	tinyint,
	@current_user 	numeric(10),
	@short_name Varchar(40)
) 
as
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	IF (@isAdmin = 1)
	begin
		select 
			top 25 ( account.account_id)		as	account_id,
			account.short_name			as	short_name
		
		 from
			account
		where
			account.deleted = 0
			and (account.ad_hoc_flag  = 0 or @includeAdHoc = 1)
			and short_name like @short_name + '%'
			order by short_name;
		return;
	end;
	select 
		top 25 ( account.account_id)	as account_id,
		account.short_name			as short_name
		
	 from
		user_access_map,
		account
	where
		user_access_map.user_id = @current_user
		and user_access_map.object_id = account.account_id 
		and user_access_map.object_type in (3, 2)
		and account.deleted = 0
		and (account.ad_hoc_flag  = 0 or @includeAdHoc = 1)
		and short_name like @short_name + '%'
		order by  short_name;
end
