if exists (select * from sysobjects where name = 'se_get_model_info')
begin
	drop procedure se_get_model_info
	print 'PROCEDURE: se_get_model_info dropped'
end
go

create PROCEDURE [dbo].[se_get_model_info] --se_get_model_info 162905
(   
    
    @model_id  NUMERIC(10) = NULL
)	    
AS    
Declare @Security_count numeric(10),
        @TopTargetedSecurity varchar(40),
		@bottomTargetedSecurity varchar(40)
begin    
set nocount on

create table #Columns(Attribute Varchar(40),Value varchar(40));


select @Security_count =count(security_id) from model_security where model_id = @model_id

insert into #Columns values('# Securities',@Security_count)
insert into #Columns values('Top Target','0')
insert into #Columns values('Bottom Target','0')

update  #Columns
set #Columns.Value = (
SELECT 
top 1 symbol
FROM  model_security
join security on
security.security_id = model_security.security_id
WHERE  model_id = @model_id and model_security.security_id <>1 
and target in (select max(target) from Model_security where model_id = @model_id and model_security.security_id <>1 )
)
where  #Columns.Attribute = 'Top Target'

update  #Columns
set #Columns.Value = (
SELECT 
top 1 symbol
FROM  model_security
join security on
security.security_id = model_security.security_id
WHERE  model_id = @model_id and model_security.security_id <>1 
and target in (select min(target) from Model_security where model_id = @model_id and model_security.security_id <>1 )
)
where  #Columns.Attribute = 'Bottom Target'



insert into #Columns
select  
'WA Beta',
sum(Coalesce(security_analytics.beta,1) * target) /1 as beta_average
from Model_security
join security_analytics on
security_analytics.security_id = model_security.security_id
WHERE  model_id = @model_id





select * from #Columns
	 
end	  
/* Display the status of the PROCEDURE creation */



go
if @@error = 0 print 'PROCEDURE: se_get_model_info created'
else print 'PROCEDURE: se_get_model_info error on creation'
go
