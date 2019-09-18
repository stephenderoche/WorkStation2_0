if exists (select * from sysobjects where name = 'se_get_commissions')
begin
       drop procedure se_get_commissions
       print 'PROCEDURE: se_get_commissions dropped'
end
go

--select * from commission_paid
--select * from commission_budget
--select * from account where account_level_code = 1
create procedure  se_get_commissions

--se_get_commissions 199,-1,-1,'01/01/2017','09/01/2018',-1,1,3
--se_get_commissions 199,-1,-1,'01/01/2017','09/01/2018',-1,1,3
--se_get_commissions 199,-1,-1,'01/01/2017','09/01/2018',-1,1,3
			--by broker
--se_get_commissions -1,-1,-1,'01/01/2017','09/01/2018',1,1,15
		--ByBrokerReason
--se_get_commissions -1,-1,-1,'01/01/2017','09/01/2018',1,1,7
--ByAccountBrokerReason
--se_get_commissions 20485,-1,-1,'01/01/2017','09/01/2018',-1,1,3
--ByAccountReason
--se_get_commissions 20485,-1,-1,'01/01/2017','09/01/2018',-1,1,19
(
@account_id numeric(10)= -1,
@broker_id numeric(10)= -1,
@commission_reason_code numeric(10)= -1,
@Start_date datetime,
@end_date datetime,
@Budget_start_date numeric(10) = -1,
@commission_budget_period_code numeric(10) = 1,
@view_type numeric(10)

)
as   
declare @date datetime = GetDate()

begin  

SET NOCOUNT ON;
       
              --SET FMTONLY OFF
create table #account(account_id numeric(10)); -- Store all child level accounts;



  insert into #account(account_id)
         select ahm.child_id
           from account_hierarchy_map ahm
          where parent_id=@account_id
            and child_type=3;




create table #broker_sum(
              broker_id               numeric(10),
              mnemonic                varchar(100),
              commission_reason_code  numeric(10),
              account_id              numeric(10),
              year                    numeric(10),
              month                   numeric(10),
              exec_commission         float,
              supp_commission         float ,
              total_commission        float ,
              budget                  float,
              estimated_exec_commission                 float,
              estimated_supp_commission                   float ,
              estimated_total_commission    float 
       );

       create table #broker_sum_with_budgets(
             broker_id               numeric(10),
             
              commission_reason_code  numeric(10),
              account_id              numeric(10),
              year                    numeric(10),
              month                   numeric(10),
              exec_commission         float,
              supp_commission         float ,
              total_commission        float ,
              budget                  float,
              estimated_exec_commission                 float,
              estimated_supp_commission                   float ,
              estimated_total_commission    float ,
                       grp_id numeric(10)
       );

-----------------------------------------------------------------------------------Just Broker------------------------------------------------------------------------------------------
if(@Budget_start_date = 1)
begin 
                     insert 
                     into #broker_sum(
                                            broker_id ,
                                            mnemonic ,
                                            commission_reason_code ,
                                            account_id    ,
                                            year          ,
                                            month          ,
                                           exec_commission,
                                           supp_commission,
                                           total_commission,
                                           budget,
                                           estimated_exec_commission,
                                           estimated_supp_commission,
                                           estimated_total_commission)
                           select
                                           b.broker_id ,
                                           b.mnemonic ,
                                            cp.commission_reason_code ,
                                           cp.account_id    ,
                                           cp.year          ,
                                           cp.month 
                             -- ,cast(CONVERT(varchar(40), cp.month) +'/01/'+ CONVERT(varchar(40), cp.year) as datetime) as period
         
                              ,sum(cp.exec_commission)as exec_commission
                              ,sum(cp.supp_commission)as supp_commission
                              ,sum(cp.total_commission)as total_commission
                              ,sum(Coalesce(0,0)) as budget
                              ,sum(cp.estimated_exec_commission)as estimated_exec_commission
                              ,sum(cp.estimated_supp_commission)as estimated_supp_commission
                              ,sum(cp.estimated_total_commission)as estimated_total_commission
          
                     from
                              commission_paid cp
                        left join broker b 
                                           on cp.broker_id = b.broker_id
                                           where   
                                           cp.account_id is  null 
                                           and (cp.broker_id = @broker_id or @broker_id = -1)
                                           and (   cast(CONVERT(varchar(40), cp.month) +'/01/'+ CONVERT(varchar(40), cp.year) as datetime) between @Start_date and @end_date )
                                           and (cp.commission_reason_code = @commission_reason_code or @commission_reason_code = -1)
                                         
                                           
       
                        group by   b.broker_id ,
                                                          b.mnemonic ,
                                                          cp.commission_reason_code ,
                                                          cp.account_id    ,
                                                          cp.year          ,
                                                          cp.month
  end
  else
  ---------------------------------------------------------------------------------not Broker----------------------------------------------------------------------------
  begin
  insert 
                     into #broker_sum(
                                            broker_id ,
                                            mnemonic ,
                                            commission_reason_code ,
                                            account_id    ,
                                            year          ,
                                            month          ,
                                           exec_commission,
                                           supp_commission,
                                           total_commission,
                                           budget,
                                           estimated_exec_commission,
                                           estimated_supp_commission,
                                           estimated_total_commission)
                           select
                                           b.broker_id ,
                                           b.mnemonic ,
                                           cp.commission_reason_code ,
                                           cp.account_id    ,
                                           cp.year          ,
                                           cp.month 
                             -- ,cast(CONVERT(varchar(40), cp.month) +'/01/'+ CONVERT(varchar(40), cp.year) as datetime) as period
         
                              ,sum(cp.exec_commission)as exec_commission
                              ,sum(cp.supp_commission)as supp_commission
                              ,sum(cp.total_commission)as total_commission
                              ,sum(Coalesce(0,0)) as budget
                              ,sum(cp.estimated_exec_commission)as estimated_exec_commission
                              ,sum(cp.estimated_supp_commission)as estimated_supp_commission
                              ,sum(cp.estimated_total_commission)as estimated_total_commission
          
                     from
                              commission_paid cp
                        left join broker b 
                                           on cp.broker_id = b.broker_id
                                           where   
                                       
                                            (cp.broker_id = @broker_id or @broker_id = -1)
                                           and ( @Budget_start_date = -1 and  cast(CONVERT(varchar(40), cp.month) +'/01/'+ CONVERT(varchar(40), cp.year) as datetime) between @Start_date and @end_date )
                                           and (cp.commission_reason_code = @commission_reason_code or @commission_reason_code = -1)
                                           and ( (cp.account_id in (select account_id from #account) and cp.account_id is not null)
                                           or ( @account_id = -1 and cp.account_id is not null )
                                           )
       
                        group by   b.broker_id ,
                                                          b.mnemonic ,
                                                          cp.commission_reason_code ,
                                                          cp.account_id    ,
                                                          cp.year          ,
                                                          cp.month
  end


  -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--Select * from commission_paid
--select * from #broker_sum

insert 
into #broker_sum_with_budgets(
               broker_id ,
                     
               commission_reason_code ,
               account_id    ,
               year          ,
               month          ,
              exec_commission,
              supp_commission,
              total_commission,
              budget,
              estimated_exec_commission,
              estimated_supp_commission,
              estimated_total_commission,
                       grp_id)

select  
                 
                 broker_id ,
                       commission_reason_code ,
                       account_id    ,
                       year          ,
                       month          ,
              sum(exec_commission) ec ,
              sum(supp_commission) sp,
              sum(total_commission) tc,
              sum(0) bg,
              sum(estimated_exec_commission) eec ,
              sum(estimated_supp_commission) esc,
              sum(estimated_total_commission) etc,
                       GROUPING_ID(broker_id ,
                       commission_reason_code ,
                       account_id    ,
                       year          ,
                       month ) grp_id
from #broker_sum
GROUP BY cube (                   broker_id ,
                       commission_reason_code ,
                       account_id    ,
                       year          ,
                       month          )
                                     having GROUPING_ID(broker_id ,
                     commission_reason_code ,
                     account_id    ,
                     year          ,
                     month ) = @view_type or @view_type = -1
order by #broker_sum.broker_id,grp_id



if(@view_type = 15)
begin
update #broker_sum_with_budgets
set #broker_sum_with_budgets.budget = coalesce(commission_budget.budget,0)
from commission_budget
where 
commission_budget.broker_id = #broker_sum_with_budgets.broker_id
end
else if (@view_type = 7)
begin
update #broker_sum_with_budgets
set #broker_sum_with_budgets.budget = coalesce(commission_budget.budget,0)
from commission_budget
where 
commission_budget.broker_id = #broker_sum_with_budgets.broker_id
and (commission_budget.commission_reason_code = #broker_sum_with_budgets.commission_reason_code
     or commission_budget.commission_reason_code = -32767)

end
else if (@view_type = 3)
begin
update #broker_sum_with_budgets
set #broker_sum_with_budgets.budget = coalesce(commission_budget.budget,0)
from commission_budget
where 
commission_budget.broker_id = #broker_sum_with_budgets.broker_id
and commission_budget.account_id = #broker_sum_with_budgets.account_id 
and (commission_budget.commission_reason_code = #broker_sum_with_budgets.commission_reason_code
     or commission_budget.commission_reason_code = -32767)
and commission_budget.account_id is not null

end
else if (@view_type = 19)
begin
update #broker_sum_with_budgets
set #broker_sum_with_budgets.budget = coalesce(commission_budget.budget,0)
from commission_budget
where 
 commission_budget.account_id = #broker_sum_with_budgets.account_id 
and (commission_budget.commission_reason_code = #broker_sum_with_budgets.commission_reason_code
     or commission_budget.commission_reason_code = -32767)
and commission_budget.account_id is not null
end

select
bs.broker_id,
broker.mnemonic,
BS.commission_reason_code,
commission_reason.mnemonic as 'Commission Reason',
bs.account_id,
account.short_name,
bs.month,
bs.year,
bs.exec_commission,
bs.supp_commission,
bs.total_commission,
bs.budget,
case 
    when bs.budget > 0 then
(Coalesce(bs.estimated_total_commission,0) + Coalesce(bs.total_commission,0))/budget 
    else
       0
end as 'PercentBudget',
bs.estimated_exec_commission,
bs.estimated_supp_commission,
bs.estimated_total_commission,
bs.grp_id

from #broker_sum_with_budgets BS
left join Broker on
broker.broker_id = bs.broker_id
left join commission_reason on
commission_reason.commission_reason_code = bs.commission_reason_code
left join account on
account.account_id = bs.account_id

order by short_name,mnemonic,[Commission Reason]

end 

go
if @@error = 0 print 'PROCEDURE: se_get_commissions created'
else print 'PROCEDURE: se_get_commissions error on creation'
go



