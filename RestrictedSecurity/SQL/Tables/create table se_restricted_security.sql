

truncate table se_restricted_security
drop table se_restricted_security
create table se_restricted_security 
([security_id] [numeric](10, 0) NOT NULL
,account_id numeric(10)
,encumber_type numeric(10)
,isEncumbered tinyint
,exception_date DateTime
,tax_lot_id varchar(40)
,restriction_description varchar(200),
restriction_type numeric(10)
,rule_id numeric(10)
);

--select * from encumbered_type

insert into encumbered_type values(26,'No Buy','No Buy')
insert into encumbered_type values(27,'No Sell','No Sell')
insert into encumbered_type values(28,'NoBS','No Buy/Sell')


select * from se_restricted_security