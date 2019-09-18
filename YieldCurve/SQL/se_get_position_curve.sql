if exists (select * from sysobjects where name = 'se_get_position_curve')
begin
	drop procedure se_get_position_curve
	print 'PROCEDURE: se_get_position_curve dropped'
end
go

create PROCEDURE [dbo].[se_get_position_curve] -- exec se_get_position_curve 98
(   
    @account_id numeric(10)
   
)	    
AS    


begin    

set nocount on

select 
security.symbol,
security.coupon,
round(security_analytics.macaulay_duration ,2) as duration,
round(security_analytics.yield_to_maturity,2) as value,
 positions.quantity*price.latest*security.pricing_factor*security.principal_factor/(currency.exchange_rate) PVOfBond
from positions
join security on
security.security_id = positions.security_id
join security_analytics on
positions.security_id = security_analytics.security_id
join price on
price.security_id = positions.security_id
join currency on
currency.security_id = security.principal_currency_id
where account_id = @account_id 
and security.major_asset_code = 3
order by duration asc




end
go
if @@error = 0 print 'PROCEDURE: se_get_position_curve created'
else print 'PROCEDURE: se_get_position_curve error on creation'
go
