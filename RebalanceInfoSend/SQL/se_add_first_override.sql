if exists (select * from sysobjects where name = 'se_add_first_override')
begin
	drop procedure se_add_first_override
	print 'PROCEDURE: se_add_first_override dropped'
end
go
create procedure [dbo].[se_add_first_override]
(	@account_id			numeric(10), 
	@security_id		numeric(10), 
	@reason				nvarchar(255) = null,
	@order_id			numeric(10) = null,
	@current_user		numeric(10),
	@position_type_code tinyint = null
) 
as
	declare @account_id_ptr			numeric(10);
	declare @manager_limit			float;
	declare	@override_by			smallint;
	declare	@override_time			datetime;
	declare	@symbol					nvarchar(40);
	declare	@name_1					nvarchar(40);
	declare	@general_error			int;
	declare	@check_manager_limit	bit;
	declare @ret_val int;
	declare	@continue_flag			bit;
	declare	@cps_add_first_override nvarchar(30);
	declare	@cpe_add_first_override nvarchar(30);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	create table #account  	(  		account_id numeric(10) not null  	);
	select @continue_flag = 1
select @cps_add_first_override = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cps_add_first_override'
	and sysobjects.type = 'P'
if @cps_add_first_override is not null
begin
	execute @ret_val = cps_add_first_override
		@continue_flag output, @account_id,
		@security_id,
		@reason,
		@order_id,
		@current_user,
		@position_type_code
	if (@ret_val != 0 and @ret_val < 60000) or @continue_flag = 0
	begin
		return @ret_val
	end
end
	select @symbol = symbol from security where security_id = @security_id;
	select @override_by = @current_user;
   	select @override_time = getdate();
	select 	@check_manager_limit = compliance_control.check_first_override
	from 	compliance_control;
	insert into #account (account_id)
	select account_hierarchy_map.child_id
	from account_hierarchy_map, account
	where
		account_hierarchy_map.parent_id = @account_id and
		account_hierarchy_map.child_id = account.account_id and
		account.account_level_code <> 1 and
		account.deleted = 0
	option (keepfixed plan);
	execute @ret_val = get_manager_limit  @manager_limit = @manager_limit output, @current_user = @current_user;
	if coalesce(@ret_val, 0) != 0 and coalesce(@ret_val, 0) < 60000
            			begin
            		        if @sp_initial_trancount = 0
                                rollback transaction;
                            return @ret_val;    
            			end;
	if @check_manager_limit = 1
	begin
		if @manager_limit < 0 
		begin 
			raiserror(50299, 10, -1);
			return 50299;
		end;		
	end;
	if exists(
		select 1 from restriction_override, proposed_orders, #account
		where
			proposed_orders.account_id = #account.account_id and
			proposed_orders.security_id = @security_id and
			(@position_type_code is null or proposed_orders.position_type_code = @position_type_code) and
			restriction_override.order_id = proposed_orders.order_id and
			restriction_override.override_number = 1 and
			restriction_override.status_code = 0 and
			proposed_orders.order_id = coalesce(@order_id, proposed_orders.order_id)
	) begin
		select @account_id_ptr = min(#account.account_id)
		from restriction_override, proposed_orders, #account
		where
			proposed_orders.account_id = #account.account_id and
			proposed_orders.security_id = @security_id and
			(@position_type_code is null or proposed_orders.position_type_code = @position_type_code) and
			restriction_override.order_id = proposed_orders.order_id and
			restriction_override.override_number = 1 and
			restriction_override.status_code = 0 and
			proposed_orders.order_id = coalesce(@order_id, proposed_orders.order_id);
		select @name_1 = name_1 from account where account_id = @account_id_ptr;
		raiserror(50211, 10, -1,  @symbol, @name_1);
		return 50211;
	end;
	if @sp_trancount is null or @sp_initial_trancount is null
                        begin
                            set @sp_trancount = 0;
                            set @sp_initial_trancount = @@trancount;
                        end;
                        if @@TRANCOUNT = 0                      
                            begin transaction;
                        else
                            set @sp_trancount = @sp_trancount + 1;
	insert into restriction_override
	(
		order_id,
		override_number,
		status_code,
		compliance_check,
		override_by,
		override_time,
		manager_limit,
		reason
	)
select distinct
		proposed_orders.order_id,
		1,
		0,
		proposed_orders.compliance_check,
		@override_by,
		@override_time,
		@manager_limit,
		@reason
	from proposed_orders, #account
	where
		proposed_orders.account_id = #account.account_id and
		proposed_orders.security_id = @security_id and
		(@position_type_code is null or proposed_orders.position_type_code = @position_type_code) and
		proposed_orders.compliance_check <> 0 and
		proposed_orders.order_id = coalesce(@order_id, proposed_orders.order_id);
	   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
	update override_user
	set 
		acknowledged = 1
	where override_user.order_id in 
		(
			select distinct
				proposed_orders.order_id
			from proposed_orders, #account
			where
				proposed_orders.account_id = #account.account_id and
				proposed_orders.security_id = @security_id and
				(@position_type_code is null or proposed_orders.position_type_code = @position_type_code) and
				proposed_orders.compliance_check <> 0 and
				proposed_orders.order_id = coalesce(@order_id, proposed_orders.order_id)
		) 
		and override_user.user_id = @override_by 
		and	(override_user.acknowledged is null 
			or override_user.acknowledged <> 1
			);
	   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
	if @sp_trancount = 0                           
                            commit transaction;     
                        else
                            set @sp_trancount = @sp_trancount - 1;
	select @cpe_add_first_override = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cpe_add_first_override'
	and sysobjects.type = 'P'
if @cpe_add_first_override is not null
begin
	execute @ret_val = cpe_add_first_override
		@account_id,
		@security_id,
		@reason,
		@order_id,
		@current_user,
		@position_type_code
	if (@ret_val != 0 and @ret_val < 60000)
	begin
		return @ret_val
	end
end
	return 0;
end
