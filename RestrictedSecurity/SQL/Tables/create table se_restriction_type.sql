drop table se_restriction_type
create table se_restriction_type 
([restriction_type_id] [numeric](10, 0) NOT NULL

,restriction_type_description varchar(200)

);

insert into se_restriction_type values(1,'Pre-Processed')
insert into se_restriction_type values(2,'Client Mandate')
insert into se_restriction_type values(3,'Fisher Mandate')

select * from se_restriction_type