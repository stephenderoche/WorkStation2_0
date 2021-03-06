  
if exists (select * from sysobjects where name = 'se_update_drift_summary')
begin
	drop procedure se_update_drift_summary
	print 'PROCEDURE: se_update_drift_summary dropped'
end
go

create procedure [dbo].[se_update_drift_summary]--se_update_drift_summary 1
(       
 @account_id       numeric(10)
 
 )      
as  

begin 

update se_drift_summary
set last_rebalance = GetDATe()
where account_id = @account_id
              
end


go
if @@error = 0 print 'PROCEDURE: se_update_drift_summary created'
else print 'PROCEDURE: se_update_drift_summary error on creation'
go
