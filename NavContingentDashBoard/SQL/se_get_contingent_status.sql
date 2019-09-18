if exists (select * from sysobjects where name = 'se_get_contingent_status')
begin
	drop procedure se_get_contingent_status
	print 'PROCEDURE: se_get_contingent_status dropped'
end
go

create procedure [dbo].[se_get_contingent_status] --se_get_contingent_status '08/01/2017'

(
	@asofDate		DateTime
)
as
select distinct se_indicative_nav.account_id,
  Account_name,
  capstock ,
  expenses ,
  income ,
  nav_calculated
from se_indicative_nav
join se_contigent_status on
se_contigent_status.account_id = se_indicative_nav.account_id
where se_indicative_nav.Asof = @asofDate



go
if @@error = 0 print 'PROCEDURE: se_get_contingent_status created'
else print 'PROCEDURE: se_get_contingent_status error on creation'
go

