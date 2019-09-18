if exists (select * from sysobjects where name = 'se_get_securities_on_model')
begin
	drop procedure se_get_securities_on_model
	print 'PROCEDURE: se_get_securities_on_model dropped'
end
go




creaTE procedure [dbo].[se_get_securities_on_model]  --se_get_securities_on_model 162905,20453,1
(@model_id numeric(10),
 @account_id numeric(10),
 @filter_type numeric(10) = 1
)
     

as 
declare  @Minsecurity_id numeric(10),
        @security_mv numeric(10),
		@account_market_value float,
		@model_target float,
		@Security_holdings_percent float,
		@total_mv_drift numeric(10)
		

create table #positions(security_id numeric(10),symbol varchar(40),target float,holding_percent float, drift_mv numeric(10),drift varchar(40),
purchase_price float,target_price float, current_price float
);


begin    

 execute get_account_market_value   
        @market_value  = @account_market_value output,   
        @account_id   = @account_id,   
        @market_value_type = 7,   
        @currency_id  = 1  



 insert into #positions 
select
positions.security_id,
security.symbol,
0,
0,
0,
'',
Coalesce(se_model_price_history.purchase_price,0),
Coalesce(se_model_price_history.target_price,0),
price.latest
from positions
join security on
positions.security_id = security.security_id
join price on
price.security_id = security.security_id
left join se_model_price_history on
se_model_price_history.security_id = security.security_id
and se_model_price_history.model_id = @model_id
where positions.account_id = @account_id
and security.major_asset_code not in (3,0)


select  @Minsecurity_id = min(#positions.security_id)    
		from #positions
		
--begin while  
  
while @Minsecurity_id is not null    
	begin 
	
	select @model_target = 0; 

	select @model_target = target from model_security where security_id = @Minsecurity_id and model_id = @model_id
	

exec se_get_security_mv @security_mv = @security_mv output, @account_id  =@account_id, @security_id = @Minsecurity_id,@market_value_type = 4,@account_market_value = 1000000

select @total_mv_drift =  (coalesce(@security_mv,0) - (coalesce(@Model_target,0)*@account_market_value))

if(@account_market_value = 0)
  begin 
  select @Security_holdings_percent = 0
  select @total_mv_drift  = 0
  
  end
else
  begin
  
  select @Security_holdings_percent = (@security_mv/@account_market_value)
 select @total_mv_drift =  (coalesce(@security_mv,0) - (coalesce(@Model_target,0)*@account_market_value))
  end
  
update #positions
set #positions.holding_percent = round(Coalesce(@Security_holdings_percent,0),4),
#positions.target = round(Coalesce(@model_target,0),4),
#positions.drift_mv = @total_mv_drift,
#positions.drift = 
case 
 when  @total_mv_drift >= 0 then 'Positive'
else 'Negative'
end 

where #positions.security_id = @Minsecurity_id
      

select @Minsecurity_id = min(#positions.security_id)    
from #positions    
where #positions.security_id >@Minsecurity_id   
end  

if (@filter_type = 1)
   begin
select * from #positions
   end
else if (@filter_type = 2)  --positive cash
   begin
   select * from #positions where #positions.drift_mv > 0
   end
else if (@filter_type = 3)
   begin
       select * from #positions where #positions.drift_mv < 0
   end
else
   begin
        select * from #positions
   end




end  




go
if @@error = 0 print 'PROCEDURE: se_get_securities_on_model created'
else print 'PROCEDURE: se_get_securities_on_model error on creation'
go