create table se_account_groups (account_id numeric(10) not null,AccountName varchar(40)  null,number numeric(10) null);

Select short_name,* from account where account_level_code = 1 and dynamic_query_id is not null

insert into se_account_groups values(10276,'Cash Short',1)
insert into se_account_groups values(10278,'Cash Heavyt',1)
insert into se_account_groups values(10349,'MY Accounts',1)
select * from se_account_groups