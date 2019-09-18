if exists (select * from sysobjects where name = 'se_get_yield_curve')
begin
	drop procedure se_get_yield_curve
	print 'PROCEDURE: se_get_yield_curve dropped'
end
go

create PROCEDURE [dbo].[se_get_yield_curve] -- exec se_get_yield_curve 'Yield Curve'
(   
    @name varchar(40)
   
)	    
AS    

create table #se_yieild_curve (duration varchar(40), value float);
begin    
set nocount on



 insert into #se_yieild_curve 
 select 
 '1 Mo',

one_month
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '3 Mo',

three_month
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '6 Mo',

six_month
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '1 Yr',

one_year
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '2 Yr',

two_year
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '3 Yr',

three_year
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '5 Yr',

five_year
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '7 Yr',

seven_year
from se_yield_curve where name = @name

 insert into #se_yieild_curve 
select '10 Yr',

ten_year
from se_yield_curve where name = @name

insert into #se_yieild_curve 
select '20 Yr',

twenty_year
from se_yield_curve where name = @name

insert into #se_yieild_curve 
select '30 Yr',

thirty_year
from se_yield_curve where name = @name


select * from #se_yieild_curve




end
go
if @@error = 0 print 'PROCEDURE: se_get_yield_curve created'
else print 'PROCEDURE: se_get_yield_curve error on creation'
go
