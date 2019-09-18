
CREATE TABLE [dbo].[se_drift_summary](
	[on_hold] [bit] NOT NULL DEFAULT ((0)),
	[account_id] [numeric](10, 0) NOT NULL,
	[short_name] [nvarchar](40) NOT NULL,
	[model_name] [nvarchar](40) NOT NULL,
	[sector_drift] [nvarchar](11) NULL,
	[security_drift] [nvarchar](11) NULL,
	[cash] [float] NULL,
	[last_rebalance] [datetime] NULL,
	[next_rebalance] [datetime] NULL,
	[current_orders] [numeric](10, 0) NULL,
	[AutoRebalance] [bit] NOT NULL DEFAULT ((0)),
	[Rebalance_frequency] [nvarchar](40) NOT NULL,
	[Duration] [float] NULL,
	[Convexity] [float] NULL,
	[equity_percent] [float] NULL,
	[equity_model] [float] NULL,
	[debt_percent] [float] NULL,
	[debt_model] [float] NULL
) ON [PRIMARY]

GO

create table #all_accounts 
	(
		account_id numeric(10) not null
	);

								
insert into #all_accounts
(
	account_id
)
select account_id 
from account
where deleted = 0
and account.account_level_code = 2



declare @MInAccountID numeric(10);
declare @short_name varchar(40);
declare @model_name varchar(40);


 
 select @MInAccountID= min(#all_accounts.account_id)  
       from #all_accounts  
  

 while @MInAccountID is not null  
 begin 

 select @short_name = short_name from account where account_id = @MInAccountID
  select @model_name = name from model where model_id in (select default_model_id from account where account_id = @MInAccountID)



insert into se_drift_summary values(0,@MInAccountID,@short_name,@model_name,'N','N',0,'07/15/2018','10/15/2018',0,0,'Quarterly',0,0,0,0,0,0)
								
	

select @MInAccountID= min(#all_accounts.account_id)  
from #all_accounts   
 where #all_accounts.account_id >@MInAccountID 
 
 end
 
 drop table #all_accounts