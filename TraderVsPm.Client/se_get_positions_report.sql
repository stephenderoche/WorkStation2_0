--exec se_get_positions_report @account=N'All',@security=N'YHOO',@issuer=N''

if exists (select * from sysobjects where name = 'se_get_positions_report')  --select * from issuer where short_name like '%gener%'
begin
	drop procedure se_get_positions_report
	print 'PROCEDURE: se_get_positions_report dropped'    

end
go
create procedure se_get_positions_report  
--se_get_positions_report 'ALL','T','','lc'
--se_get_positions_report 'ALL','','AT&T INC',''

(	
    @account					Varchar(40),
	
	@security				Varchar(40) = null,

	@issuer                  Varchar(40) = null,

	@search                 Varchar(40) = null

	
	

) 

as

	declare @ret_val int,
	@settle_date_start			datetime = null,
	@settle_date_end			datetime = null,
	@broker_id					numeric(10) = null,
	@include_confirmed_tickets 	tinyint = 1,
	@isAccountId                  smallint = 1,
    @userId					  	  smallint = 198,
	@trader_id 					numeric(10) = 1,
	@display_currency_id 		numeric(10) = null,
	@account_id            numeric(10) = null,
	@security_id            numeric(10) = null,
	@issuer_id            numeric(10) = null,
	@current_user					int,
    @sacctID						nvarchar(40),
	@display_currency_id_local	numeric(10);

begin

                        set nocount on;

                        declare @ec__errno int;

                        declare @sp_initial_trancount int;

                        declare @sp_trancount int;

-----------------------------------------------------  account ------------------------------------------------------------------
     if @account = 'All'
	    begin
		    select @account_id = -1
		end
     else
	    begin
		   select @account_id = account_id from account where short_name  = @account
		end

-----------------------------------------------------  security ------------------------------------------------------------------

if @security = ''
	    begin
		    select @security_id = -1
		end
     else
	    begin
		   select @security_id = security_id from security where symbol  = @security
		end

-----------------------------------------------------  issuer ------------------------------------------------------------------

if @issuer = ''
	    begin
		    select @issuer_id = -1
		end
     else
	    begin
		   select @issuer_id = issuer_id from issuer where short_name  = @issuer  --select * from issuer where issuer_id = 484
		end

------------------------------------------------------------------------------------------------------------------------


	select @display_currency_id_local = @display_currency_id;


	create table #account  	(  		account_id numeric(10) not null  	);

if @account_id <> -1

	begin

		if @isAccountId <> 0

		begin

			if @isAccountId = 2 

			begin

				insert into #account 

				(

					account_id

				) 

				values (@account_id);

				select 

					@sacctID = account.short_name 

				from account 

				where account.account_id = @account_id;

			end else begin 

				insert into #account 

				(

					account_id

				)

				select

					account.account_id

				from account_hierarchy_map

				join account 

					on account_hierarchy_map.child_id = account.account_id

				where account_hierarchy_map.parent_id = @account_id

					and account.account_level_code <> 3

					and account.deleted = 0

					and account.ad_hoc_flag = 0;

				select 

					@sacctID = account.short_name 

				from account 

				where account.account_id = @account_id;

			end;

		end else begin 

			insert into #account 

			(

				account_id

			)

			select 

				account.account_id

			from workgroup_tree_map

			join account 

				on workgroup_tree_map.child_id = account.account_id

				and account.account_level_code <> 3

				and account.deleted = 0

				and account.ad_hoc_flag = 0			

			where workgroup_tree_map.parent_id = @account_id

				and workgroup_tree_map.child_type in (2, 3);

			select 

				@sacctID = user_info.name 

			from user_info 

			where user_id = @account_id;

		end;

	end else begin

		select @current_user = @userId;

		insert into #account

		(

			account_id

		)

		select

			account.account_id

		from user_access_map

		join account

			on user_access_map.object_id = account.account_id

			and account.deleted = 0

		where user_access_map.user_id = @current_user

			and user_access_map.object_type in (2, 3);

		select @sacctID = 'All';

	end;


	select 
	account.short_name as 'Account Name',
	Quantity,
	user_info.name as Manager,
	security.symbol as Symbol,
	major_asset.mnemonic as 'Major_Asset',
	coalesce(issuer.short_name,'Missing Issuer') as 'Issuer Name'
	from
	positions
	join account on
	account.account_id = positions.account_id
	left join user_info on
	user_info.user_id = account.manager
	left join security on
    positions.security_id = security.security_id
	left join issuer on
	issuer.issuer_id = security.issuer_id
	join major_asset on
	major_asset.major_asset_code = security.major_asset_code
	where 
	positions.account_id in (select account_id from #account)
	and
	 (account.short_name like @search +'%'   or @search = '')
	and 
	(positions.security_id = @security_id or @security_id = -1)
	and 
	(issuer.issuer_id = @issuer_id or @issuer_id = -1)
	
	print @security_id
	--se_get_positions_report 'ALL','','PEPSICO INC'
	--se_get_positions_report 'ALL','HRG',''
	--se_get_positions_report 'ALL','','AT&T INC',''
	print @issuer_id
end
