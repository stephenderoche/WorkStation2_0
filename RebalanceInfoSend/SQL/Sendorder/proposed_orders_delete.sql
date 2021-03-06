 
ALTER trigger  proposed_orders_delete on  [dbo].[proposed_orders] for delete as
	declare	@order_id numeric(10);
	declare @general_error		int;
	declare @locking_type		int;
	declare @ret_val			int;
	declare @message_count		int;
	declare	@tran_count numeric(10);
	declare @null_count			int;
	declare @unknown_user_id	numeric(10);
	declare @unknown_user_name	nvarchar(128);
    declare @contextname                    nvarchar(128);
begin
	declare @rows int
	declare @ec__errno int
	select @rows = @@rowcount
	if @rows = 0
	begin
		return
	end
	select @locking_type = cast(value as int) 
	from registry
	where section = 'REBALANCE' 
		and entry = 'SESSION PROFORMA LOCKING TYPE';
	if @locking_type = 2
	begin
		if exists(
			select 1 
			from deleted 
			join rebal_session
				on deleted.rebal_session_id = rebal_session.rebal_session_id
				and rebal_session.rebal_status_code = 2
			join security
				on deleted.security_id = security.security_id
				and security.minor_asset_code <> 1
				and security.minor_asset_code <> 2
		) begin
			raiserror(50538, 10, -1);
			rollback
			return;
		end;
	end else if @locking_type = 1 begin
		select @null_count = count(*)  													from deleted  													  													where deleted.modified_by is null;  													if @null_count > 0  													begin  														select @unknown_user_id = user_id,  															@unknown_user_name = name  														from user_info (nolock)  														where user_id = -1;  														if @unknown_user_id is null  														begin  															raiserror(50569, 10, -1,  'proposed_orders_delete');  															rollback  															return;  														end else begin  															if @unknown_user_name like '%linedata%'  															begin  																raiserror(60114, 10, -1,  'proposed_orders_delete');  															end;  														end;  													end;
		if exists(
			select 1 
			from deleted 
			join rebal_session
				on deleted.rebal_session_id = rebal_session.rebal_session_id
				and deleted.modified_by <> rebal_session.owner_id
				and rebal_session.rebal_status_code = 2
			join security
				on deleted.security_id = security.security_id
				and security.minor_asset_code <> 1
				and security.minor_asset_code <> 2
		) begin
			raiserror(50538, 10, -1);
			rollback
			return;
		end;
	end;
	exec getcontext @contextname output;
	if @contextname = N'SendOrder'
	begin 
if @rows = 1
	begin
		select @order_id = deleted.order_id 
		from deleted
		;
		if not exists(
			select 1
			from orders 
			where orders.order_id = @order_id
		) begin
			delete order_tax_lot
			where order_tax_lot.order_id = @order_id;
			delete	generic_debt_order
			where	generic_debt_order.order_id = @order_id;
		end; 
	end else begin
delete order_tax_lot
		from deleted
		left outer join orders
			on deleted.order_id = orders.order_id
		where order_tax_lot.order_id = deleted.order_id
			and orders.order_id is null;
		delete generic_debt_order
		from deleted
		left outer join orders
			on deleted.order_id = orders.order_id
		where generic_debt_order.order_id = deleted.order_id
			and orders.order_id is null;
end;	
end;
	delete override_user
	from deleted
	where deleted.order_id = override_user.order_id;
	insert into proposed_orders_d 
	(
		order_id, 
		old_account_id, 
		modified_time
	)
	select 
		deleted.order_id,
		deleted.account_id,
		getdate()
	from deleted
	;
end
