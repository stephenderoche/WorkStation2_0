if exists (select * from sysobjects where name = 'se_get_contibution_ditribution')
begin
	drop procedure se_get_contibution_ditribution
	print 'PROCEDURE: se_get_contibution_ditribution dropped'
end
go

create PROCEDURE [dbo].[se_get_contibution_ditribution] -- se_get_contibution_ditribution 98,2
(   
    @account_id numeric(10),
    @analytic_value numeric(10) = 0    
)	    
AS 
Declare @AMV numeric(10)
Declare @WA float
begin


select  
@AMV = 
sum((positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate))
  from positions
join security_analytics on
security_analytics.security_id = positions.security_id
join price on
price.security_id = positions.security_id
join security on
security.security_id = positions.security_id
join currency on
currency.security_id = security.principal_currency_id
WHERE  account_id = 98
and security.major_asset_code = 3


if (@analytic_value = 1)
  begin

  select  
@WA = 
round(sum(  security_analytics.effective_modified_duration *
(positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate))/
sum(positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate),4)
  from positions
join security_analytics on
security_analytics.security_id = positions.security_id
join price on
price.security_id = positions.security_id
join security on
security.security_id = positions.security_id
join currency on
currency.security_id = security.principal_currency_id
WHERE  account_id = @account_id
and security.major_asset_code = 3

		select  
		top 10
		round(security_analytics.effective_modified_duration * ( positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate/@AMV),4) as Contribution,
		symbol,
		round(security_analytics.effective_modified_duration,4),
		positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate as mv,
		@WA as 'WA_Duration'

		  from positions 
		join security_analytics on
		security_analytics.security_id = positions.security_id
		join price on
		price.security_id = positions.security_id
		join security on
		security.security_id = positions.security_id
		join currency on
		currency.security_id = security.principal_currency_id
		WHERE  account_id = @account_id
		and security.major_asset_code = 3
		order by Contribution desc
  end

       

  else
    begin

	  select  
@WA = 
round(sum(  security_analytics.effective_yield *
(positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate))/
sum(positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate),4)
  from positions
join security_analytics on
security_analytics.security_id = positions.security_id
join price on
price.security_id = positions.security_id
join security on
security.security_id = positions.security_id
join currency on
currency.security_id = security.principal_currency_id
WHERE  account_id = @account_id
and security.major_asset_code = 3

   select  
top 10
		round(security_analytics.effective_yield * ( positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate/@AMV),4) as Contribution,
		symbol,
		round(security_analytics.effective_yield,4),
		positions.quantity * price.latest * security.pricing_factor * security.principal_factor/currency.exchange_rate as mv,
		@WA as 'WA_Yield'
		  from positions 
		join security_analytics on
		security_analytics.security_id = positions.security_id
		join price on
		price.security_id = positions.security_id
		join security on
		security.security_id = positions.security_id
		join currency on
		currency.security_id = security.principal_currency_id
		WHERE  account_id = 98
		and security.major_asset_code = 3
		order by Contribution desc

		end


end

go
if @@error = 0 print 'PROCEDURE: se_get_contibution_ditribution created'
else print 'PROCEDURE: se_get_contibution_ditribution error on creation'
go
