if exists (select * from sysobjects where name = 'se_get_bond_change')
begin
	drop procedure se_get_bond_change
	print 'PROCEDURE: se_get_bond_change dropped'
end
go

create PROCEDURE [dbo].[se_get_bond_change] -- exec se_get_bond_change 11002,.25
(   
    @account_id numeric(10),
    @pct_change float = 0    
)	    
AS    

begin 
declare @direction numeric(1) = 1
set nocount on

set @pct_change = @pct_change/100;


select top 100 positions.account_id, security.security_id , security.symbol, security.name_1,  
security.maturity_date,
security_analytics.short_maturity_date,
Coalesce(round(security.coupon,4),0) as coupon, 
Coalesce(round(security_analytics.modified_duration,2),0) as 'Mod. Duration', 
Coalesce(round(security_analytics.effective_modified_duration,2),0) as 'Eff. Mod. Duration', 
Coalesce(round(security_analytics.effective_convexity,2),0) as 'Eff. Convexity',
Coalesce(round(price.latest,4),0) as price,
Coalesce(round(-1*@direction*security_analytics.effective_modified_duration* @pct_change *100,4),0) as '%_change_due_to_Duration', 
Coalesce(round(security_analytics.effective_convexity * @pct_change * @pct_change*100,6),0) as '%_change_due_to_Convexity',
Coalesce(round( (-1*@direction*security_analytics.effective_modified_duration* @pct_change*100 + security_analytics.effective_convexity * @pct_change * @pct_change *100 ),2),0)as Total_ChangeInPct,
Coalesce(round(positions.quantity*price.latest*security.pricing_factor*security.principal_factor/(currency.exchange_rate),2),2) as PVOfBond,
Coalesce(round(		 (1 + (-1*@direction*security_analytics.effective_modified_duration* @pct_change*100 + security_analytics.effective_convexity * @pct_change * @pct_change *100)/100)*
	 ( positions.quantity*price.latest*security.pricing_factor*security.principal_factor/(currency.exchange_rate)),2),2) NewValue
from security, security_analytics, price, positions, currency
where major_asset_code=3
and security.security_id = security_analytics.security_id
and security.security_id= price.security_id
and positions.security_id= security.security_id
and security.principal_currency_id = currency.security_id
and positions.account_id=@account_id
order by PVOfBond desc

end

go
if @@error = 0 print 'PROCEDURE: se_get_bond_change created'
else print 'PROCEDURE: se_get_bond_change error on creation'
go
