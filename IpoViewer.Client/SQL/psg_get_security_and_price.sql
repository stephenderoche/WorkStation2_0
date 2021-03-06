USE [LVTS_753]
GO
/****** Object:  StoredProcedure [dbo].[psg_get_security_and_price]    Script Date: 10/13/2017 1:43:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[psg_get_security_and_price] --psg_get_security_and_price 'HSBA.GB'
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
		sec.symbol = upper(@symbol) 
		AND sec.deleted = 0 
		AND pr.security_id = sec.security_id
		AND sec.major_asset_code = ma.major_asset_code			
END
