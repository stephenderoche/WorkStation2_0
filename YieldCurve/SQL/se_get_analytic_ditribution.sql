if exists (select * from sysobjects where name = 'se_get_analytic_ditribution')
begin
	drop procedure se_get_analytic_ditribution
	print 'PROCEDURE: se_get_analytic_ditribution dropped'
end
go

create PROCEDURE [dbo].[se_get_analytic_ditribution] -- exec se_get_analytic_ditribution 11002,3
(   
    @account_id numeric(10),
    @analytic_value numeric(10) = 0    
)	    
AS 
create table #count (
   s int not null,
   v varchar(40)
);   
begin


--duration
if (@analytic_value =1)
   begin
 insert into #count
    select case 
	          when Coalesce(security_analytics.effective_modified_duration,0) = 0 then 0
              when security_analytics.effective_modified_duration between 0 and 1 then 1
			  when security_analytics.effective_modified_duration between 1.01 and 2 then 2
			  when security_analytics.effective_modified_duration between 2.01 and 3 then 3
			  when security_analytics.effective_modified_duration between 3.01 and 5 then 5
			  when security_analytics.effective_modified_duration between 5.01 and 7 then 7
			  when security_analytics.effective_modified_duration between 7.01 and 10 then 10
			  when security_analytics.effective_modified_duration between 10.01 and 15 then 15
			  when security_analytics.effective_modified_duration between 15.01 and 20 then 20
			  else 30
			  end
		 as num,
		 	 case 
			 when Coalesce(security_analytics.effective_modified_duration,0) = 0 then '0'
              when security_analytics.effective_modified_duration between 0 and 1 then '0 -1'
			  when security_analytics.effective_modified_duration between 1.01 and 2 then '1 -2'
			  when security_analytics.effective_modified_duration between 2.01 and 3 then '2 -3'
			  when security_analytics.effective_modified_duration between 3.01 and 5 then '3 -5'
			  when security_analytics.effective_modified_duration between 5.01 and 7 then '5 -7'
			  when security_analytics.effective_modified_duration between 7.01 and 10 then '7 -10'
			  when security_analytics.effective_modified_duration between 10.01 and 15 then '10 -15'
			  when security_analytics.effective_modified_duration between 15.01 and 20 then '15 -20'
			  else '30'
			  end
		 as value
from positions
join security_analytics on
positions.security_id = security_analytics.security_id
where positions.account_id = @account_id

select count(#count.s)value,
#count.v as description
from #count
group by #count.s,#count.v
end
--rating
else if (@analytic_value = 2)
begin

 insert into #count
   select case 
           
              when quality_rating_rank.rank between 19 and 25 then 1
			  when quality_rating_rank.rank between 10 and 18 then 2
			  when quality_rating_rank.rank between 5 and 9 then 3
			  when quality_rating_rank.rank = 1   then 4
			
			  else 5
			  end
		 as num,
		 	 case 
              when quality_rating_rank.rank between 19 and 25 then 'A'
			  when quality_rating_rank.rank between 10 and 18 then 'B'
			  when quality_rating_rank.rank between 5 and 9 then 'C'
			  when quality_rating_rank.rank = 1   then 'D'
			  else 'NR'
			  end
		 as value
 
from positions
join security on
positions.security_id = security.security_id
join quality_rating on
quality_rating.security_id = security.security_id
and quality_rating.rating_service_code = 1
join quality_rating_rank on
quality_rating_rank.quality_rating_code = quality_rating.quality_rating_code
and quality_rating_rank.rating_service_code = 1
where positions.account_id = @account_id

select count(#count.s)value,
#count.v as description
from #count
group by #count.s,#count.v

end
if (@analytic_value =3)
   begin
 insert into #count
   select case 
              when Coalesce(security_analytics.effective_yield,0) <= 0  then 0
              when security_analytics.effective_yield between .01 and 1 then 1
			  when security_analytics.effective_yield between 1.01 and 2 then 2
			  when security_analytics.effective_yield between 2.01 and 3 then 3
			  when security_analytics.effective_yield between 3.01 and 4 then 4
			  when security_analytics.effective_yield between 4.01 and 5 then 5
			  when security_analytics.effective_yield between 5.01 and 7 then 7
			  when security_analytics.effective_yield between 7.01 and 9 then 8
			 
			
			  else 9
			  end
		 as num,
		 	 case 
			 when security_analytics.effective_yield <= 0  then '0'
              when security_analytics.effective_yield between .01 and 1 then '0 -1'
			  when security_analytics.effective_yield between 1.01 and 2 then '1 - 2'
			  when security_analytics.effective_yield between 2.01 and 3 then '2 - 3'
			  when security_analytics.effective_yield between 3.01 and 4 then '3 - 4'
			  when security_analytics.effective_yield between 4.01 and 5 then '4 - 5'
			  when security_analytics.effective_yield between 5.01 and 7 then '5 - 6'
			  when security_analytics.effective_yield between 7.01 and 9 then '7 - 9'
		
			  else '9+'
			  
			  end
		 as value
from positions
join security_analytics on
positions.security_id = security_analytics.security_id
where positions.account_id = @account_id

select count(#count.s)value,
#count.v as description
from #count
group by #count.s,#count.v
end

end

go
if @@error = 0 print 'PROCEDURE: se_get_analytic_ditribution created'
else print 'PROCEDURE: se_get_analytic_ditribution error on creation'
go
