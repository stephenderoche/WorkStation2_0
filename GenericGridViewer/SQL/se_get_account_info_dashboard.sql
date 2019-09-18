  --select * from proposed_orders
if exists (select * from sysobjects where name = 'se_get_account_info_dashboard')
begin
	drop procedure se_get_account_info_dashboard
	print 'PROCEDURE: se_get_account_info_dashboard dropped'
end
go
create PROCEDURE [dbo].[se_get_account_info_dashboard]  --se_get_account_info_dashboard 10284
(
	@account_id	numeric(10)
)
AS
declare @total_mv numeric(10)
BEGIN
	declare @encumberedMV numeric(10)
	declare @st numeric(10)
	declare @lt numeric(10)
	declare @total numeric(10)
	declare @pst numeric(10)
	declare @plt numeric(10)
	declare @ptotal numeric(10)
	SET NOCOUNT ON;



exec se_calculate_gain_loss_account @pst output, @account_id,1

exec se_calculate_gain_loss_account @plt output, @account_id,2

exec se_calculate_gain_loss_account @ptotal output, @account_id,3



	exec get_account_market_value @total_mv output,@account_id,7,1 

 create table #child_accounts (account_id numeric(10) not null);  
    
	insert into #child_accounts 
	select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0
					    --and account.account_id <> @account_id
    
 

	  select  @encumberedMV =
     
      coalesce(sum((encumbered_positions.encumbered_quantity)    
       * coalesce(price.latest, 0.0) * security.principal_factor * pricing_factor / principal.exchange_rate    
       + (0)    )   ,0)
      from    
            encumbered_positions,     
            security,     
            price,    
            currency principal,    
			currency income    

      where    
            encumbered_positions.security_id = security.security_id and    
            security.security_id = price.security_id and    
            security.principal_currency_id = principal.security_id and    
            coalesce(price.latest, 0.0) * security.principal_factor * pricing_factor <> 0.0 and    
            security.income_currency_id = income.security_id  and  
            encumbered_positions.account_id in (select account_id from #child_accounts)  

		

select @st = sum(user_field_15)
from account where account.account_id in (select account_id from #child_accounts) 
select @lt = sum(user_field_16)
from account where account.account_id in (select account_id from #child_accounts) 

select @total = Coalesce(@st,0) + Coalesce(@lt,0)




  -- Insert statements for procedure here
select 
short_name as 'Account Name', 
user_info.name as 'Manager',
CONVERT(VARCHAR(10),Coalesce(inception_date,getdate()-365),110) as 'Inception Date',
major_account.description as 'Account Type',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@st),1), '.00',''),'0') as 'Short GL',
--Coalesce(@st,'0') as 'Short GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@lt),1), '.00',''),'0') as 'Long GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@total),1), '.00',''),'0') as 'Total GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@pst),1), '.00',''),'0') as 'Orders Short GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@plt),1), '.00',''),'0') as 'Orders Long GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@ptotal),1), '.00',''),'0') as 'Orders Total GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@st + @pst),1), '.00',''),'0') as 'Total Short GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@lt + @plt),1), '.00',''),'0') as 'Total Long GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@total + @ptotal),1), '.00',''),'0') as 'Total Total GL',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@total_mv),1), '.00',''),'0') as 'Account MV',
Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@encumberedMV),1), '.00',''),'0') as 'BTL MV',
@total_mv - @encumberedMV as 'AMVBTL',
account.user_field_12 as 'State',
account.user_field_13 as 'ST Rate',
account.user_field_14 as 'LT Rate'
--Coalesce(REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,@total_mv - @encumberedMV),1), '.00',''),'0') as 'AMVBTL'
from account 
join major_account on
account.major_account_code = major_account.major_account_code
left join user_info on
user_info.user_id = account.manager
where account.account_id in (@account_id)  





END


go
if @@error = 0 print 'PROCEDURE: se_get_account_info_dashboard created'
else print 'PROCEDURE: se_get_account_info_dashboard error on creation'
go