  
if exists (select * from sysobjects where name = 'se_get_ipo_data')
begin
	drop procedure se_get_ipo_data
	print 'PROCEDURE: se_get_ipo_data dropped'
end
go

--drop table #ipo_account_info

create procedure [dbo].[se_get_ipo_data]--se_get_ipo_data 163,149,1.5,27.18,28
(     
 @account_id       numeric(10),
 @country_code     numeric(10) = -1,
 @target float = 0,
 @MidPrice float = 0,
 @HighPrice float = 0,
 @currency_id		numeric(10) = 1
 

 )    
as 

  declare @market_value numeric(10)
  declare @market_value_by_country numeric(10)
  declare @default_model_id numeric(10)
  declare @MInAccountID numeric(10)
  declare @country_name varchar(40)
  declare @Incl varchar(40)
 
  
begin	
  
		 create table #child_accounts (account_id numeric(10) not null);
		 create table #ipo_account_info (model varchar(40),account varchar(40),account_id numeric(10),
		 inc tinyint,total_market_vale float,percent_of_aum float,req_target_weight float,
		 target_purchase_weight float);

		 select @country_name = name from country where country_code = @country_code
		
		 insert into #child_accounts 
	     select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0;
		

select @MInAccountID = min(#child_accounts.account_id)    
		from #child_accounts   
		
--begin while  
  
while @MInAccountID is not null    
	begin  
     
   select       
     @default_model_id = default_model_id      
     from account      
    where account_id = @MInAccountID 

exec se_get_account_market_value_by_country @market_value = @market_value output,@account_id = @MInAccountID,@currency_id = @currency_id,@country_code = -1	
exec se_get_account_market_value_by_country @market_value = @market_value_by_country output,@account_id = @MInAccountID,@currency_id = @currency_id,@country_code = @country_code	
print @market_value_by_country

    insert into #ipo_account_info
	select model.name ,
	account.short_name ,
	account.account_id,

	1 ,
	Coalesce(@market_value,0),
	.01,
	case 
	    when @market_value_by_country = 0 or @market_value = 0 then 0
	else
	round(coalesce(@market_value_by_country,1)/coalesce(@market_value,1),4)
	end,
	Coalesce(@market_value_by_country,0)
	from account
	join model on 
	account.default_model_id = model.model_id
	where account.account_id = @MInAccountID







select @MInAccountID = min(#child_accounts.account_id)    
from #child_accounts    
where #child_accounts.account_id >@MInAccountID   
end  


--end while


 select @country_name = 'RegTargetWgt'
 select @Incl = 'inc?'

 	DECLARE @SQLstring nvarchar(700)
DECLARE @ColName varchar(100)
DECLARE @tmvinus varchar(100)
DECLARE @aum varchar(100)
DECLARE @ptmv varchar(100)
DECLARE @pmw varchar(100)
declare @pmvp float

Select @pmvp  = #ipo_account_info.target_purchase_weight * (@target/100) from #ipo_account_info
DECLARE @sem varchar(100)
DECLARE @seh varchar(100)
SET @ColName = @country_name
set  @incl = @Incl
set @tmvinus = 'Total Market Value in USD'
set @aum = 'Percent of Aum'
set @ptmv = 'Porfolio Target MKT Value'
set @pmw = 'Target Purchase Weight MKT Value'
set @sem = 'Share Esitmate of Mid Offer Price'
set @seh = 'Share Esitmate of High Offer Price'


SELECT 
#ipo_account_info.model as Model,
#ipo_account_info.account as Account,
#ipo_account_info.account_id as Account_id,
#ipo_account_info.inc AS Inc, 
#ipo_account_info.total_market_vale AS 'Total Market Value in USD',
#ipo_account_info.percent_of_aum AS 'Percent of Aum',
#ipo_account_info.req_target_weight AS 'RegTargetWgt'  ,
#ipo_account_info.target_purchase_weight AS  'Porfolio Target MKT Value'  ,
#ipo_account_info.target_purchase_weight * (@target/100)  AS  'Target Purchase Weight MKT Value'  ,
case
    @target 
	when 0 then
0  
else
round(#ipo_account_info.target_purchase_weight * (@target/100)/@MidPrice,0)
end AS  'Share Esitmate of Mid Offer Price' ,
case
    @target 
	when 0 then
0  
else
round(#ipo_account_info.target_purchase_weight * (@target/100)/@HighPrice,0)
end AS  'Share Esitmate of High Offer Price'
FROM #ipo_account_info


--SET @SQLstring = 'SELECT 
--#ipo_account_info.model as Model,
--#ipo_account_info.account as Account,
--#ipo_account_info.inc AS Inc, 
--#ipo_account_info.total_market_vale AS [' + @tmvinus + '],
--#ipo_account_info.percent_of_aum AS [' + @aum + '],
--#ipo_account_info.req_target_weight AS [' + @ColName + ']  ,
--#ipo_account_info.target_purchase_weight AS  [' + @ptmv + ']  ,
--#ipo_account_info.target_purchase_weight * ([' + @target + ']/100)  AS  [' + @pmw + ']  ,
--0 AS  [' + @sem + ']  ,
--0 AS  [' + @seh + ']  
--FROM #ipo_account_info'

--EXEC sp_executesql @SQLstring


------------------------------------------------------------------------------
  




 --se_get_ipo_data 93,1,208

END
go
if @@error = 0 print 'PROCEDURE: se_get_ipo_data created'
else print 'PROCEDURE: se_get_ipo_data error on creation'
go
