create table se_contigent_status 
(    
  account_id numeric(10),
  Asof datetime,
  capstock tinyint,
  expenses tinyint,
  income tinyint,
  nav_calculated tinyint
  
);  

select * from se_indicative_nav

insert into se_contigent_status values(4,'2017-08-01 00:00:00.000',1,1,1,0)
insert into se_contigent_status values(199,'2017-08-01 00:00:00.000',1,1,1,1)

