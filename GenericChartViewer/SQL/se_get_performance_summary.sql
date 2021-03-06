  
if exists (select * from sysobjects where name = 'se_get_performance_summary')
begin
	drop procedure se_get_performance_summary
	print 'PROCEDURE: se_get_performance_summary dropped'
end
go

--drop table #ipo_account_info

create procedure [dbo].[se_get_performance_summary]--se_get_performance_summary 20419
(     
 @account_id       numeric(10)
 

 )    
as 

 
  
begin	
  
	select 
	se_performance_summary.account_id,
	se_performance_types.performace_type as 'Period',
	se_performance_summary.account_performace as 'Account Return',
	se_performance_summary.benchmark_performace as 'Benchmark Return'
	from
	se_performance_summary
	join se_performance_types on
	se_performance_types.performace_type_id = se_performance_summary.performace_type_id

	where se_performance_summary.account_id = @account_id

END

go
if @@error = 0 print 'PROCEDURE: se_get_performance_summary created'
else print 'PROCEDURE: se_get_performance_summary error on creation'
go