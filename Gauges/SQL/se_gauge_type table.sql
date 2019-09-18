if exists (select * from sysobjects where name = 'se_gauge_type')
begin
	drop table se_gauge_type
	print 'TABLE: se_gauge_type dropped'
end
go
create table se_gauge_type
(
	gauge_type_id				numeric(10) identity not null,
	gauge_type                 nvarchar(255) not null
);
go
if @@ERROR = 0 print 'TABLE: se_gauge_type created'
else print 'TABLE: se_gauge_type error on creation'
go

insert into se_gauge_type values('Number of Messages')
insert into se_gauge_type values('Number of Messages not Acknowledged')
insert into se_gauge_type values('Drinks before Nic Loses Time')
select * from se_gauge_type