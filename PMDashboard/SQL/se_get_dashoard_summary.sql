  
if exists (select * from sysobjects where name = 'se_get_dashoard_summary')
begin
	drop procedure se_get_dashoard_summary
	print 'PROCEDURE: se_get_dashoard_summary dropped'
end
go

CREATE procedure [dbo].[se_get_dashoard_summary]--se_get_dashoard_summary 199
(       
 @account_id       numeric(10) = 1
 
 )      
as  
declare @order_count numeric(10),
 @Duration float = 0,
 @beta float  = 0,
 @fixed_income_holdings Float,
	@fixed_income_benchmark Float ,
	@equity_holdings Float ,
	@equity_benchmark Float ,
@min_account_id numeric(10),
@TDC float,
@Hiearchy_name varchar(40), 
@model_id numeric(10),
@model_name varchar(40),
@account_name varchar(40),
@manager varchar(100),
@hasMaturity varchar(40),
 @toDate datetime =Getdate() + 7,
 @fromDate datetime = Getdate(),
 @maturityCount numeric(10)

begin 


CREATE TABLE [dbo].[#se_drift_summary](
	[account_id] [numeric](10, 0) NOT NULL,
	[short_name] [nvarchar](40) NOT NULL,
	[model_id] [numeric](10, 0) NOT NULL,
	[model_name] [nvarchar](40) ,
	[sector_drift] [nvarchar](11) NULL,
	[security_drift] [nvarchar](11) NULL,
	[cash] [float] NULL,
	[current_orders] [numeric](10, 0) NULL,
    Beta [float] NULL,
	Duration [float] NULL,
	equity_percent float null,
	equity_model float null,
	debt_percent float null,
	debt_model float null,
	[manager] [nvarchar](40) NULL,
	[has_maturity] [nvarchar](40) NULL
) 


                select @order_count = count(*) from orders where account_id in (select account_id from se_drift_summary where on_hold = 0)--@account_id;

                     create table #t_account ( account_id numeric(10));
                      insert into #t_account  
		  select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;

                     select @min_account_id = min(account_id) from #t_account;

                     while (@min_account_id is not null)
                     begin
                          select @order_count = count(*) from orders where account_id in (@min_account_id) and deleted = 0--@account_id;
						  select @Hiearchy_name = name from hierarchy where hierarchy_id in (select default_hierarchy_id from account where account_id = @min_account_id)
						select @model_id = default_model_id from account where account_id = @min_account_id
						select @manager = Coalesce(name,'NA') from user_info where user_id = (select manager from account where account_id = @min_account_id)
						if( (select
count(*) 
from positions 
join security on
positions.security_id = security.security_id
where  maturity_date is not null
and security.maturity_date between @fromDate and dateadd(dd, 1, @toDate)
and account_id = @min_account_id ) > 0)
begin
set @HasMaturity = 'Y'
end
else
begin
set @HasMaturity = 'N'
end


						if @model_id = -1 
						   begin
						   set @model_name = 'Portfolio Self'
						   end
                        else if @model_id = -2
						   begin
						   set @model_name = 'Sector Self'
						   end
                          else if @model_id = -3
						   begin
						   set @model_name = 'Portfolio Empty'
						   end
						      else if @model_id = -4
						   begin
						   set @model_name = 'Sector Empty'
						   end
						else
						 begin
						 select  @model_name = name from model where model_id = @model_id
						 end
                     select @account_name = short_name  from account where account_id = @min_account_id

			--modified_duration			
  execute se_get_WAVG_analytics 
  @analytic = @Duration output,
  @account_id = @min_account_id,
  @Hierarchy_sector_id = 1,
  @analytic_value = 1,
  @portfolio = 1

  		--beta		
  execute se_get_WAVG_analytics 
  @analytic = @beta output,
  @account_id = @min_account_id,
  @Hierarchy_sector_id = 1,
  @analytic_value = 11,
  @portfolio = 1


  execute se_benchmark_vs_holdings_for_debt_and_equity
@fixed_income_holdings = @fixed_income_holdings output,
@fixed_income_benchmark = @fixed_income_benchmark output,
@equity_holdings = @equity_holdings output,
@equity_benchmark = @equity_benchmark output,
@account_id = @min_account_id,
@hierarchy_name  = '.Default',--@Hiearchy_name,
@userid	 = 198


 

						  
						  execute se_get_cash_percent 
						  @trade_date_cash = @TDC output,
						  @account_id = @min_account_id,
						  @major_asset_code = 0

						
						declare @HD Varchar(40)
						execute se_has_security_drift 
						@has_drift = @HD output,
						@account_id = @min_account_id,
						@topx = 10

						declare @HDSector Varchar(40)
						execute se_has_sector_drift 
						@has_drift = @HDSector output,
						@account_id = @min_account_id
						


						--select * from se_drift_summary
						  insert into #se_drift_summary values(
						  @min_account_id,
						  @account_name,
						  @model_id,
						  @model_name,
						  @HDSector,
						  @HD,
						  @TDC,
						  @order_count,
						  @beta,
						  @Duration,
						  Coalesce(@equity_holdings,0),
						  Coalesce(@equity_benchmark,0),
						  Coalesce(@fixed_income_holdings,0),
						  Coalesce(@fixed_income_benchmark,0),
						  @manager,
						  @HasMaturity)


						 set @HD  = ''
						 set  @HDSector = ''
						 set @TDC = 0
						 set @Duration = 0
						 set @beta = 0
						 set @equity_holdings = 0
						 set @equity_benchmark = 0
						 set @fixed_income_holdings = 0
						 set @fixed_income_benchmark = 0
					

						  
                         select @min_account_id = min(account_id) from #t_account where account_id > @min_account_id;
                                        

                    end; 
            select account.user_field_4,
			#se_drift_summary.*
			from #se_drift_summary 
			join account on
			#se_drift_summary.account_id = account.account_id
			where #se_drift_summary.account_id in (select account_id from #t_account)



end
go
if @@error = 0 print 'PROCEDURE: se_get_dashoard_summary created'
else print 'PROCEDURE: se_get_dashoard_summary error on creation'
go


--update se_drift_summary
--set sector_drift = 'N',
--security_drift = 'N'