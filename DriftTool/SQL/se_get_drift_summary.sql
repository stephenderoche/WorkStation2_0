  
if exists (select * from sysobjects where name = 'se_get_drift_summary')
begin
	drop procedure se_get_drift_summary
	print 'PROCEDURE: se_get_drift_summary dropped'
end
go

CREATE procedure [dbo].[se_get_drift_summary]--se_get_drift_summary 20423
(       
 @account_id       numeric(10) = 1
 
 )      
as  
declare @order_count numeric(10),
 @Duration float,
 @Convexity float,
 @fixed_income_holdings Float,
	@fixed_income_benchmark Float ,
	@equity_holdings Float ,
	@equity_benchmark Float ,
@min_account_id numeric(10),
@TDC float,
@Hiearchy_name varchar(40) 
begin 





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


			--modified_duration			
  execute se_get_WAVG_analytics 
  @analytic = @Duration output,
  @account_id = @min_account_id,
  @Hierarchy_sector_id = 1,
  @analytic_value = 1,
  @portfolio = 1

  		--modified_duration			
  execute se_get_WAVG_analytics 
  @analytic = @Convexity output,
  @account_id = @min_account_id,
  @Hierarchy_sector_id = 1,
  @analytic_value = 7,
  @portfolio = 1


  execute se_benchmark_vs_holdings_for_debt_and_equity
@fixed_income_holdings = @fixed_income_holdings output,
@fixed_income_benchmark = @fixed_income_benchmark output,
@equity_holdings = @equity_holdings output,
@equity_benchmark = @equity_benchmark output,
@account_id = @min_account_id,
@hierarchy_name  = @Hiearchy_name,
@userid	 = 198

  ----equity percent
  --  execute se_get_market_value_by_asset_type 
  --@percent = @equityPercent output,
  --@account_id = @min_account_id,
  --@major_asset_code = 1

  --  --debt percent
  --  execute se_get_market_value_by_asset_type 
  --@percent = @debtPercent output,
  --@account_id = @min_account_id,
  --@major_asset_code = 3
 

						  
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
						  update se_drift_summary
						  set current_orders = @order_count,
						  security_drift = @HD,
						  sector_drift = @HDSector,
						  cash = @TDC,
						  Duration = @Duration,
						  Convexity = @Convexity,
						  equity_percent = @equity_holdings,
						  equity_model = @equity_benchmark,
						  debt_percent = @fixed_income_holdings,
						  debt_model = @fixed_income_benchmark
						  where se_drift_summary.account_id = @min_account_id

						   set @HD  = 0
						 set  @HDSector = 0
						 set @TDC = 0
						 set @Duration = 0
						 set @Convexity = 0
						 set @equity_holdings = 0
						 set @equity_benchmark = 0
						 set @fixed_income_holdings = 0
						 set @fixed_income_benchmark = 0

						  
                         select @min_account_id = min(account_id) from #t_account where account_id > @min_account_id;
                                        

                    end; 
select * from se_drift_summary where account_id in (select account_id from #t_account)



end
go
if @@error = 0 print 'PROCEDURE: se_get_drift_summary created'
else print 'PROCEDURE: se_get_drift_summary error on creation'
go


--update se_drift_summary
--set sector_drift = 'N',
--security_drift = 'N'