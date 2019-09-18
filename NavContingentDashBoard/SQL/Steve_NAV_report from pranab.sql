
delete from share_class_type

insert into share_class_type values(1,'Class 1');
insert into share_class_type values(2,'Class 2');
insert into share_class_type values(3,'Class A');
insert into share_class_type values(4,'Class B');
insert into share_class_type values(5,'Class C');
insert into share_class_type values(6,'Class I');
insert into share_class_type values(7,'NAV');
insert into share_class_type values(8,'Class R1');
insert into share_class_type values(9,'Class R2');
insert into share_class_type values(10,'Class R3');
insert into share_class_type values(11,'Class R4');
insert into share_class_type values(12,'Class R5');
insert into share_class_type values(13,'Class R6');
insert into share_class_type values(14,'Class T');
insert into share_class_type values(15,'1');
insert into share_class_type values(16,'2');
insert into share_class_type values(17,'3');
insert into share_class_type values(18,'NAV');
insert into share_class_type values(19,'A');
insert into share_class_type values(20,'B');
insert into share_class_type values(21,'C');
insert into share_class_type values(22,'ADV');
insert into share_class_type values(23,'R3');
insert into share_class_type values(24,'R4');
insert into share_class_type values(25,'R5');
insert into share_class_type values(26,'I');
insert into share_class_type values(27,'5');
insert into share_class_type values(28,'R1');
insert into share_class_type values(29,'R2');
insert into share_class_type values(30,'I2');
insert into share_class_type values(31,'R6');
insert into share_class_type values(32,'R2');
insert into share_class_type values(33,'R');
insert into share_class_type values(34,'CLASS R GBP ACCUM (hedged)');
insert into share_class_type values(35,'CLASS R EUR ACCUM (hedged)');
insert into share_class_type values(36,'A USD ACCUM');
insert into share_class_type values(37,'C USD ACCUM');
insert into share_class_type values(38,'W USD ACCUM');
insert into share_class_type values(39,'I USD ACCUM');
insert into share_class_type values(40,'A USD DIST');
insert into share_class_type values(41,'C USD DIST');
insert into share_class_type values(42,'W USD DIST');
insert into share_class_type values(43,'W CHF ACCUM');
insert into share_class_type values(44,'W CHF DIST');
insert into share_class_type values(45,'I CHF ACCUM');
insert into share_class_type values(46,'W GBP DIST');
insert into share_class_type values(47,'I GBP ACCUM');
insert into share_class_type values(48,'CLASS R EUR ACCUM (hedged)');
insert into share_class_type values(49,'CLASS R GBP ACCUM (hedged)');
insert into share_class_type values(50,'CLASS R USD ACCUM');
insert into share_class_type values(51,'CLASS SI USD ACCUM');
insert into share_class_type values(52,'CLASS X USD ACCUM');
insert into share_class_type values(53,'CLASS X GBP ACCUM');
insert into share_class_type values(54,'CLASS F USD ACCUM');

select * from share_class_type

drop table se_indicative_nav;
create table se_indicative_nav (
account_id numeric(10),
Account_name varchar(40),
Share_class  varchar(40),
share_class_type_code numeric(10), 
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
7,
1, --class_ratio
127126004.06,
1.78,
0.00,
-2577.35,
-584.97,
0.00,
0.00,
836723.77,
0.00,
0.00,
0.00,
127959567.29,
9062742.785,
0.000,
9062742.79,
14.11929813,
14.11929813,
0.00000000,
'08/01/2017');


insert into se_indicative_nav values(
4,                                    --acount_id
'3367-JHVIT INTL EQTY INDX TR B',  --account_name
'NAV - 3367', --share class
7,
1, --class_ratio
127126004.06,
1.78,
0.00,
-2577.35,
-584.97,
0.00,
0.00,
836723.77,
0.00,
0.00,
0.00,
127959567.29,
9062742.785,
0.000,
9062742.79,
14.11929813,
14.11929813,
0.00000000,
'08/02/2017');

insert into se_indicative_nav values(
4,                                    --acount_id
'3367-JHVIT INTL EQTY INDX TR B',  --account_name
'NAV - 3367', --share class
7,
1, --class_ratio
127126004.06,
1.78,
0.00,
-2577.35,
-584.97,
0.00,
0.00,
836723.77,
0.00,
0.00,
0.00,
127959567.29,
9062742.785,
0.000,
9062742.79,
14.11929813,
14.11929813,
0.00000000,
'08/03/2017');






select * from  share_class_type_ratio;
create table share_class_type_ratio
( trdate                   datetime,
  account_id               numeric(10),
  parent_share_class_type numeric(10),
  child_share_class_type  numeric(10),
  pct numeric(10,7)
)

drop table share_class_type_ratio;
insert into share_class_type_ratio values ('08/01/2017',4,7,1,.6);
insert into share_class_type_ratio values ('08/01/2017',4,7,2,.4);
insert into share_class_type_ratio values ('08/02/2017',4,7,1,.7);
insert into share_class_type_ratio values ('08/02/2017',4,7,2,.3);
insert into share_class_type_ratio values ('08/03/2017',4,7,1,.5);
insert into share_class_type_ratio values ('08/03/2017',4,7,2,.3);
insert into share_class_type_ratio values ('08/03/2017',4,7,3,.2);

select * from share_class_type;


delete from se_indicative_nav where class_ratio !=1

select * from se_indicative_nav
;

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT Asof,account_id
      ,Account_name
      ,Share_class
      ,Class_Ratio
      ,Starting_NAV
      ,Income_Daily_Accrual
      ,AmortAccret_Daily_Accrual
      ,ExpenseDailyAccrualAllocation
      ,ClassSpecificDailyExpenseAccruals
      ,RealizedGL
      ,RealizedCurrGL
      ,UnrealizedGL
      ,FwdUnrealizedGL
      ,Distribution
      ,CapitalStock
      ,IndTotalNetAssets
      ,StartingSharesOutstanding
      ,subsReds
      ,SharesOutstanding
      ,IndNAVperShare
      ,ActualNAVperShare
      ,Proof
       
  FROM se_indicative_nav
union all
 SELECT Asof,  account_id
      ,Account_name
      ,share_class_type.description 
      ,share_class_type_ratio.pct*100 Class_Ratio
      ,share_class_type_ratio.pct*Starting_NAV
      ,share_class_type_ratio.pct*Income_Daily_Accrual
      ,share_class_type_ratio.pct*AmortAccret_Daily_Accrual
      ,share_class_type_ratio.pct*ExpenseDailyAccrualAllocation
      ,share_class_type_ratio.pct*ClassSpecificDailyExpenseAccruals
      ,share_class_type_ratio.pct*RealizedGL
      ,share_class_type_ratio.pct*RealizedCurrGL
      ,share_class_type_ratio.pct*UnrealizedGL
      ,share_class_type_ratio.pct*FwdUnrealizedGL
      ,share_class_type_ratio.pct*Distribution
      ,share_class_type_ratio.pct*CapitalStock
      ,share_class_type_ratio.pct*IndTotalNetAssets
      ,share_class_type_ratio.pct*StartingSharesOutstanding
      ,share_class_type_ratio.pct*subsReds
      ,SharesOutstanding
      ,share_class_type_ratio.pct*IndNAVperShare
      ,share_class_type_ratio.pct*ActualNAVperShare
      ,share_class_type_ratio.pct*Proof
      
  FROM share_class_type_ratio join share_class_type on (share_class_type_ratio.child_share_class_type = share_class_type.share_class_type_code)
    join se_indicative_nav on ( se_indicative_nav.share_class_type_code = share_class_type_ratio.parent_share_class_type
	                          and se_indicative_nav.Asof =share_class_type_ratio.trdate )

order by 1
