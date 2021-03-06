
ALTER procedure [dbo].[rank_tax_lots]
(
	@account_id				numeric(10), 
	@position_type			numeric(10),
	@security_id			numeric(10),
	@relief_method_code		numeric(10),
	@side_code				numeric(10)
)
as
	declare	@priority int;
	declare	@tax_lot_id	int;
	declare @latest_price float;
	declare @one_year_in_past datetime;
	declare @ret_val int;
	declare @continue_flag		int;
	declare @cps_rank_tax_lots	nvarchar(30);
	declare @cpe_rank_tax_lots	nvarchar(30);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;	
	select @continue_flag = 1
select @cps_rank_tax_lots = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cps_rank_tax_lots'
	and sysobjects.type = 'P'
if @cps_rank_tax_lots is not null
begin
	execute @ret_val = cps_rank_tax_lots
		@continue_flag output, @account_id,
		@position_type,
		@security_id,
		@relief_method_code,
		@side_code
	if (@ret_val != 0 and @ret_val < 60000) or @continue_flag = 0
	begin
		return @ret_val
	end
end
	select @priority = 0;
	select @one_year_in_past = dateadd(yy, -1, getdate());
	if (@relief_method_code = 1)
	begin
		return 0;
	end;	
			----custom
	delete from sell_tax_lots_rank where security_id =@security_id and Account_id = @account_id	

	if exists(select 1 from tax_lot_relief_method where tax_lot_relief_method_code = @relief_method_code) begin
		select @tax_lot_id = -1 ;
		WHILE (@tax_lot_id is not null)
		begin
			select @priority = @priority + 1 ;
			select @tax_lot_id = NULL ;
			 if (@relief_method_code = 2 and @side_code in (2, 3)
              or @relief_method_code = 5 and @side_code in (4, 5))
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by unit_cost desc, trade_date asc;
			end;	
			if (@relief_method_code = 3)
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by trade_date desc, unit_cost desc;
			end;	
			if (@relief_method_code = 4)
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by trade_date asc, unit_cost desc;
			end;	
			if (@relief_method_code = 5 and @side_code in (2, 3)
             or @relief_method_code = 2 and @side_code in (4, 5))
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by unit_cost asc, trade_date asc;
			end;	
			if (@relief_method_code = 6 and @side_code in (2, 3)
             or @relief_method_code = 7 and @side_code in (4, 5))
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
					and datediff(dd,  trade_date,  @one_year_in_past) >= 1
				order by unit_cost asc, trade_date asc;
			end;	
			if (@relief_method_code = 7 and @side_code in (2, 3)
             or @relief_method_code = 6 and @side_code in (4, 5))
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
					and datediff(dd,  trade_date,  @one_year_in_past) >= 1
				order by unit_cost desc, trade_date asc;
			end;	
			if (@relief_method_code = 8 and @side_code in (2, 3)
             or @relief_method_code = 9 and @side_code in (4, 5))
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
					and datediff(dd,  trade_date,  @one_year_in_past) < 1
				order by unit_cost asc, trade_date asc;
			end;	
			if (@relief_method_code = 9 and @side_code in (2, 3)
             or @relief_method_code = 8 and @side_code in (4, 5))
			begin
select top (1)
					@tax_lot_id = tax_lot_id
				from tax_lot
				where account_id = @account_id
					and position_type_code = @position_type
					and security_id = @security_id
					and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
					and datediff(dd,  trade_date,  @one_year_in_past) < 1
			order by unit_cost desc, trade_date asc;
			end;	
			insert into #sell_tax_lots
			( 	tax_lot_id,
				quantity,
				proposed_quantity,
				rank,
				client_tax_lot_id,
				lot_number
			)
			select distinct tax_lot.tax_lot_id, 
				tax_lot.quantity - coalesce((select	sum(order_tax_lot.quantity - order_tax_lot.quantity_confirmed)
													from	order_tax_lot
													where	order_tax_lot.tax_lot_id = tax_lot.tax_lot_id
													and		order_tax_lot.deleted = 0), 0.0), 
				null,
				@priority,
				tax_lot.client_tax_lot_id,
				tax_lot.lot_number
			 from tax_lot
			 where tax_lot.tax_lot_id = @tax_lot_id;
			   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
		end;
		if (@relief_method_code = 6 or @relief_method_code = 7 or
			@relief_method_code = 8 or @relief_method_code = 9)
		begin
			select @tax_lot_id = -1 ;
			WHILE (@tax_lot_id is not null)
			begin
				select @priority = @priority + 1;
				select @tax_lot_id = null ;
				if (@relief_method_code = 6 and @side_code in (2, 3)
				 or @relief_method_code = 7 and @side_code in (4, 5))
				begin
select 
						top (1)
						@tax_lot_id = tax_lot_id
					from tax_lot
					where account_id = @account_id
						and position_type_code = @position_type
						and security_id = @security_id
						and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by unit_cost asc, trade_date asc;
				end;	
				if (@relief_method_code = 7 and @side_code in (2, 3)
				 or @relief_method_code = 6 and @side_code in (4, 5))
				begin
select 
						top (1)
						@tax_lot_id = tax_lot_id
					from tax_lot
					where account_id = @account_id
						and position_type_code = @position_type
						and security_id = @security_id
						and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by unit_cost desc, trade_date asc;
				end;	
				if (@relief_method_code = 8 and @side_code in (2, 3)
				 or @relief_method_code = 9 and @side_code in (4, 5))
				begin
select top (1)
						@tax_lot_id = tax_lot_id
					from tax_lot
					where account_id = @account_id
						and position_type_code = @position_type
						and security_id = @security_id
						and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
				order by unit_cost desc, trade_date asc;
				end;	
				if (@relief_method_code = 9 and @side_code in (2, 3)
				 or @relief_method_code = 8 and @side_code in (4, 5))
				begin
select top (1)
						@tax_lot_id = tax_lot_id
					from tax_lot
					where account_id = @account_id
						and position_type_code = @position_type
						and security_id = @security_id
						and tax_lot_id not in (select tax_lot_id from #sell_tax_lots)
					order by unit_cost asc, trade_date asc;
				end;
				insert into #sell_tax_lots
				( 	tax_lot_id,
					quantity,
					proposed_quantity,
					rank,
					client_tax_lot_id,
					lot_number
				)
				select distinct tax_lot.tax_lot_id, 
					tax_lot.quantity - coalesce((select	sum(order_tax_lot.quantity - order_tax_lot.quantity_confirmed)
														from	order_tax_lot
														where	order_tax_lot.tax_lot_id = tax_lot.tax_lot_id
														and		order_tax_lot.deleted = 0), 0.0), 
					null,
					@priority,
					tax_lot.client_tax_lot_id,
					tax_lot.lot_number
				 from tax_lot
				 where tax_lot.tax_lot_id = @tax_lot_id;
				   select @ec__errno = @@error;
            			if @ec__errno != 0
            			begin
            				if @sp_initial_trancount = 0
            				    rollback transaction;
                          return @ec__errno;
            			end;
			end;
		end;	
		
			----------------------------------------------for taxlot detail to see disposal level-----------------------------------------------
		insert into sell_tax_lots_rank
	(
	tax_lot_id,
	rank,
	security_id,
	account_id
	)
	select
	tax_lot_id,
	rank,
	@security_id,
	@account_id
	from #sell_tax_lots
	end;	
	
	select @cpe_rank_tax_lots = sysobjects.name
from sysobjects
where
	sysobjects.name = 'cpe_rank_tax_lots'
	and sysobjects.type = 'P'
if @cpe_rank_tax_lots is not null
begin
	execute @ret_val = cpe_rank_tax_lots
		@account_id,
		@position_type,
		@security_id,
		@relief_method_code,
		@side_code
	if (@ret_val != 0 and @ret_val < 60000)
	begin
		return @ret_val
	end
end
	return 0;
end
