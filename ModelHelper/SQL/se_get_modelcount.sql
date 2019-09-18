  --select * from proposed_orders
if exists (select * from sysobjects where name = 'se_get_modelcount')
begin
	drop procedure se_get_modelcount
	print 'PROCEDURE: se_get_modelcount dropped'
end
go



create procedure [dbo].[se_get_modelcount]  

as   
declare @modelCount as numeric(10)


begin    



select
Count(account.default_model_id) as count,
model.name ,
model.model_id
from model
join account on
account.default_model_id = model.model_id
where model.name is not null 
			and model.original_model_id is null
			and model.model_type = 0
			and model.submodel_flag = 0
group by model.name,model.model_id

      
end  


go
if @@error = 0 print 'PROCEDURE: se_get_modelcount created'
else print 'PROCEDURE: se_get_modelcount error on creation'
go