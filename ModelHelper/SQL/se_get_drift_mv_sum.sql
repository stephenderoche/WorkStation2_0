  --select * from proposed_orders
if exists (select * from sysobjects where name = 'se_get_drift_mv_sum')
begin
	drop procedure se_get_drift_mv_sum
	print 'PROCEDURE: se_get_drift_mv_sum dropped'
end
go

/*
declare  @se_get_drift_mv_sum numeric(10)

exec se_get_drift_mv_sum  
@se_get_drift_mv_sum =  @se_get_drift_mv_sum output, 
@account_id  =20419,
@model_id = 162905, 
@account_market_value= 710010

print @se_get_drift_mv_sum

*/


create  procedure [dbo].[se_get_drift_mv_sum] --se_get_drift_mv_sum 20490
(   
 @se_get_drift_mv_sum numeric(10) = 0 output,
 @account_id numeric(10),
 @Model_id numeric(10),
 @account_market_value float
 
 )
 
as  
create table #model_securities(security_id numeric(10) );

declare  @Model_target float,
         @security_mv float,
		 @Minsecurity_id numeric(10),
		 @total_mv_drift numeric(10),
		 @model_type numeric(10)



begin
 set nocount on;

 insert into #model_securities 
select
positions.security_id
from positions
where positions.account_id = @account_id

select @model_type = model_type from model where model_id = @Model_id


if (@model_type = 1 or @model_type =2)
    begin 

	declare @has_drift float
				exec se_get_sector_drift_mv_sum 
				@has_drift = @has_drift output,
				@account_id = @account_id

				select @se_get_drift_mv_sum = @has_drift
				

	end
else
begin

select  @Minsecurity_id = min(#model_securities.security_id)    
		from #model_securities


		
--begin while  
  
while  @Minsecurity_id is not null    
	begin  

select @Model_target = 0

select @Model_target = target from model_security where model_id = @Model_id and security_id =  @Minsecurity_id



 

exec se_get_security_mv @security_mv = @security_mv output, @account_id  =@account_id, @security_id = @Minsecurity_id,@market_value_type = 4,@account_market_value = @account_market_value

if  ( (coalesce(@security_mv,0) - (coalesce(@Model_target,0)*@account_market_value)) >0)
select @total_mv_drift = Coalesce(@total_mv_drift,0) + (coalesce(@security_mv,0) - (coalesce(@Model_target,0)*@account_market_value))



select @Minsecurity_id = min(#model_securities.security_id)    
from #model_securities   
where #model_securities.security_id >@Minsecurity_id  
end

select @se_get_drift_mv_sum = @total_mv_drift

end --end model_type
return @se_get_drift_mv_sum

	
        
        
 
end  


go
if @@error = 0 print 'PROCEDURE: se_get_drift_mv_sum created'
else print 'PROCEDURE: se_get_drift_mv_sum error on creation'
go

 
