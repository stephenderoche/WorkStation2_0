  --select * from proposed_orders
if exists (select * from sysobjects where name = 'psg_get_security_and_price')
begin
	drop procedure psg_get_security_and_price
	print 'PROCEDURE: psg_get_security_and_price dropped'
end
go

Create PROCEDURE [dbo].[psg_get_security_and_price] --psg_get_security_and_price ''
	@symbol	nvarchar(80)
AS
BEGIN
	SELECT
		sec.security_id,
		sec.symbol,
		sec.name_1,
		pr.latest,
		sec.major_asset_code,
		ma.mnemonic,
		sec.principal_currency_id,
		cur.system_currency,
		cur.mnemonic as currency_mnemonic,
		cur.exchange_rate,
		country.name,
		country.country_code
	FROM
		price pr,
		major_asset ma,
		security sec LEFT OUTER JOIN currency cur ON sec.principal_currency_id = cur.security_id
		join country on
		sec.country_code = country.country_code
	WHERE
		((sec.symbol = upper(@symbol)) or (@symbol = ''))
		AND sec.deleted = 0 
		AND pr.security_id = sec.security_id
		AND sec.major_asset_code = ma.major_asset_code		
		and sec.major_asset_code > 0	
END

go
if @@error = 0 print 'PROCEDURE: psg_get_security_and_price created'
else print 'PROCEDURE: psg_get_security_and_price error on creation'
go