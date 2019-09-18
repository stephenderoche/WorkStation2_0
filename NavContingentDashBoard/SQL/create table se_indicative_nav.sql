drop table se_indicative_nav
create table se_indicative_nav (
account_id numeric(10),
Account_name varchar(40),
Share_class  varchar(40),
Class_Ratio float,
Starting_NAV float,
Income_Daily_Accrual float,
AmortAccret_Daily_Accrual float,
ExpenseDailyAccrualAllocation float,
ClassSpecificDailyExpenseAccruals float,
RealizedGL float,
RealizedCurrGL float,
UnrealizedGL float,
FwdUnrealizedGL float,
Distribution float,
CapitalStock float,
IndTotalNetAssets float,
StartingSharesOutstanding float,
subsReds float,
SharesOutstanding float,
IndNAVperShare float,
ActualNAVperShare float,
Proof float,
Asof Datetime)

insert into se_indicative_nav values(
4,                                    --acount_id
'3367-JHVIT INTL EQTY INDX TR B',  --account_name
'NAV - 3367', --share class
7, --share_class_code
1, --class_ratio
127126004.06, --starting nav
1.78, --'Income_Daily_Accrual'
0.00, --'AmortAccret_Daily_Accrual'
-2577.35, --'ExpenseDailyAccrualAllocation'
-584.97,--'ClassSpecificDailyExpenseAccruals'
0.00, -- 'RealizedGL'
0.00, --'RealizedCurrGL'
836723.77, --'UnrealizedGL',
0.00, --'FwdUnrealizedGL'
0.00, --'Distribution',
0.00, --'CapitalStock',
127959567.29, --'IndTotalNetAssets'
9062742.785,  --'StartingSharesOutstanding'
0.000,--'subsReds',
9062742.79,--'SharesOutstanding',
14.11929813, --'IndNAVperShare',
14.11929813, --'ActualNAVperShare',
0.00000000, -- proof
'08/01/2017') --asof



insert into se_indicative_nav values(
4,                                    --acount_id
'3367-JHVIT INTL EQTY INDX TR B',  --account_name
'Class1 - 3367', --share class
.67181567581,
85405242.33,
1.20,
0.00,
-1731.50,
-584.97,
0.00,
0.00,
562124.15,
0.00,
0.00,
0.00,
85965051.20,
6095984.866,
0.000,
6095984.87,
14.10191349,
14.10191349,
0.00000000,
'08/01/2017'
)

insert into se_indicative_nav values(
4,                                    --acount_id
'3367-JHVIT INTL EQTY INDX TR B',  --account_name
'Class2 - 3367', --share class
.32818432419,
41720761.73,
0.58,
0.00,
-845.85,
0,
0.00,
0.00,
274599.62,
0.00,
0.00,
0.00,
41994516.09,
2966757.919,
0.000,
2966757.92,
14.15501947,
14.15501946,
0.000000005,
'08/01/2017'
)
