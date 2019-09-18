drop table se_process_status
create table se_process_status (
account_id numeric(10),
administrator  varchar(40),
target_nav_time  varchar(40),
fund_initialization  numeric(10),
capital_stock  numeric(10),
corp_action  numeric(10),
trades numeric(10),
income numeric(10),
amortization numeric(10),
expenses numeric(10),
income_distribution numeric(10),
UnrealizedGL numeric(10),
nav float,
reviewed numeric(10),
released numeric(10),
asof Datetime)

delete from se_process_status

insert into se_process_status values(199,'Citi','5:40',1,3,2,2,2,2,2,2,2,2,2,2,'02/01/2018')
insert into se_process_status values(208,'Citi','5:40',1,2,2,2,2,2,2,2,2,2,2,2,'02/01/2018')
insert into se_process_status values(209,'Citi','5:40',1,2,2,2,2,2,2,2,2,2,2,2,'02/01/2018')
insert into se_process_status values(210,'Citi','5:40',1,2,2,2,2,2,2,2,2,2,2,2,'02/01/2018')
insert into se_process_status values(211,'Citi','5:40',1,2,2,2,2,2,2,2,2,2,2,2,'02/01/2018')

select * from account where short_name like 'wm%'

select * from se_process_status

create table se_process_status_type (
se_proces_status_type_id numeric(10),
description varchar(40))

insert into se_process_status_type values(1,'Complete')
insert into se_process_status_type values(2,'WIP')
insert into se_process_status_type values(3,'Error')

select * from se_process_status

