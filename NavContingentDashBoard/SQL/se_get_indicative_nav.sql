if exists (select * from sysobjects where name = 'se_get_indicative_nav')
begin
	drop procedure se_get_indicative_nav
	print 'PROCEDURE: se_get_indicative_nav dropped'
end
go

create procedure [dbo].[se_get_indicative_nav] --se_get_indicative_nav 199,'08/01/17',7
(	@account_id 	numeric(10),
    @start_date Datetime,
	@share_class_type_code numeric(10) = -1
) 
as
begin
                   
select
account_id ,
Account_name ,
Share_class   as 'Share_class',
share_class_type_code , 
Class_Ratio  as 'Class_Ratio',
Starting_NAV ,
Income_Daily_Accrual  as 'Income_Daily_Accrual',
AmortAccret_Daily_Accrual as  'AmortAccret_Daily_Accrual',
ExpenseDailyAccrualAllocation as 'ExpenseDailyAccrualAllocation',
ClassSpecificDailyExpenseAccruals 'ClassSpecificDailyExpenseAccruals',
RealizedGL as 'RealizedGL',
RealizedCurrGL as 'RealizedCurrGL',
UnrealizedGL 'UnrealizedGL',
FwdUnrealizedGL 'FwdUnrealizedGL',
Distribution 'Distribution',
CapitalStock as 'CapitalStock',
IndTotalNetAssets as 'IndTotalNetAssets',
StartingSharesOutstanding as 'StartingSharesOutstanding',
subsReds 'subsReds',
SharesOutstanding 'SharesOutstanding',
IndNAVperShare as 'IndNAVperShare',
ActualNAVperShare 'ActualNAVperShare',
coalesce(ActualNAVperShare,0) - coalesce(IndNAVperShare,0) as 'Proof',
Asof
from se_indicative_nav
where account_id = @account_id
and convert(datetime, convert(nvarchar(10), se_indicative_nav.Asof, 112), 112) = convert(datetime, convert(nvarchar(10), @start_date, 112), 112)
and (share_class_type_code = @share_class_type_code or  @share_class_type_code = -1)
order by class_Ratio desc	


end
go
if @@error = 0 print 'PROCEDURE: se_get_indicative_nav created'
else print 'PROCEDURE: se_get_indicative_nav error on creation'
go

--truncate table se_indicative_nav
