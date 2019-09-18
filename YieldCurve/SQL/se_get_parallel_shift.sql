if exists (select * from sysobjects where name = 'se_get_parallel_shift')
begin
	drop procedure se_get_parallel_shift
	print 'PROCEDURE: se_get_parallel_shift dropped'
end
go

create PROCEDURE [dbo].[se_get_parallel_shift] -- exec se_get_parallel_shift 'Yield Curve',-25
(   
    @name varchar(40),
	@pct_change  float -- Pct in fraction . e.g, 1 pct = 1; 0.25 pct means 25 basis pt
   
)	    
AS    

create table #se_yield_curve (duration varchar(40), value float);
begin    
declare @direction numeric(1) -- 1 up and -1 down
set nocount on


if(@pct_change < 0)
  set @direction = 1
else
    set @direction = 1
  

 insert into #se_yield_curve 
select '1 Mo',
((one_month * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '3 Mo',
((three_month * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '6 Mo',
((six_month * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '1 Yr',
((one_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '2 Yr',
((two_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '3 Yr',
((three_year * 100) + ((1 * @direction) *(@pct_change)))/100

from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '5 Yr',
((five_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '7 Yr',
((seven_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

 insert into #se_yield_curve 
select '10 Yr',
((ten_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

insert into #se_yield_curve 
select '20 Yr',
((twenty_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name

insert into #se_yield_curve 
select '30 Yr',
((thirty_year * 100) + ((1 * @direction) *(@pct_change)))/100
from se_yield_curve where name = @name


select * from #se_yield_curve




end
go
if @@error = 0 print 'PROCEDURE: se_get_parallel_shift created'
else print 'PROCEDURE: se_get_parallel_shift error on creation'
go
