if exists (select * from sysobjects where name = 'se_account_replacement')
begin
	drop table se_account_replacement
	print 'TABLE: se_account_replacement dropped'
end
go
create table se_account_replacement
(
	replacement_id					numeric(10) identity not null,
	deleted				            tinyint not null,
	account_id						numeric(10) not null,
	buy_list_type_id				tinyint not null,
	buy_list_id				        numeric(10) not null,
	replacement_list_type_id		tinyint not null,
	replacement_list_id				numeric(10) not null,
	modified_by						numeric(10) not null,
	modified_time					datetime not null,
	created_by						numeric(10) not null,
	created_time					datetime not null,
	constraint se_account_replacement_pk primary key (replacement_id)
);
go
if @@ERROR = 0 print 'TABLE: se_account_replacement created'
else print 'TABLE: se_account_replacement error on creation'
go

/*

buy list types

0 - security
1 - sector
2 - country

*/

--select * from hierarchy_sector where hierarchy_id = 19
--select * from security where symbol like '%goog%'
--select * from user_info

--truncate table se_account_replacement
--select * from se_account_replacement

insert into se_account_replacement values(0,238,1,535,0,7185,189,getdate(),189,getdate())
insert into se_account_replacement values(0,238,0,3548,0,7185,189,getdate(),189,getdate())
insert into se_account_replacement values(0,238,0,3596,0,3595,189,getdate(),189,getdate())


