
if exists (select * from sysobjects where name = 'se_cmpl_case_statistics')
begin
	drop procedure se_cmpl_case_statistics
	print 'procedure: se_cmpl_case_statistics dropped'
end
go


create procedure  [dbo].[se_cmpl_case_statistics]--exec se_cmpl_case_statistics ''
(
		@account_id numeric(10),
		@user_id numeric(10) =198
       
)
as
       declare @sacctID                                       nvarchar(40);
       declare @resultCount                            int;
       declare @current_user                                  numeric(10);
       declare @ret_val                                       int;
       declare @continue_flag                                 int;
       declare @cps_rpx_cmpl_case_statistics    nvarchar(30);
       declare @cpe_rpx_cmpl_case_statistics    nvarchar(30);
       declare @userId                                          smallint;
	    declare @acctID                       numeric(10)= -1,
       @isAccountId                  smallint = 1,
       @breachOpen                   tinyint = 1,
       @breachAsleep                 tinyint = 1,
       @breachResolved               tinyint = 1,
       @breachClosed                 tinyint = 0,
       @worstSeverityError           tinyint = 1,
       @currentSeverityError         tinyint = 1,
       @activePassive                tinyint = 3,
       @reviewed                     tinyint = 3,
       @atop                         tinyint = 3,
       @manager                      smallint = -1,
       @ruleSet                                   numeric(10)= -1,
       @rule                                      numeric(10)= -1
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;

       
       create table #account      (             account_id numeric(10) not null  );
       create table #mdparams_cmpl_rpt_case_stats 
       (
              parm          nvarchar(50) null, 
              parm_value    tinyint null
       );
       create table #cases_cmpl_rpt_case_stats
       (
              cmpl_case_id               numeric(10) not null,
              created_time               datetime not null,
              current_error_level        tinyint not null,
              date_bucket_min_days int null,
              date_bucket_max_days int null
       );
       create table #casebckt_cmpl_rpt_case_stats
       (
              cmpl_error_level     tinyint not null,
              Today                           int null,
              Days1To5                    int null,
              Days6To10                     int null,
              Days11To20                           int null,
              Days21To30                           int null,
              Days31Plus                  int null,
              Total        int null
       );

       create table #prior_to_pivit
       (
              decription varchar(40),
              Today                           int null,
              Days1To5                      int null,
              Days6To10                     int null,
              Days11To20                           int null,
              Days21To30                           int null,
              Days31Plus                  int null,
              Total        int null
       );

              create table #post_to_pivit
       (
              Time varchar(40),
              warning                           int null,
              fail                       int null
       );
       

      	insert into #account
	select
			        account.account_id
					from account_hierarchy_map
					join account on account_hierarchy_map.child_id = account.account_id
					where account_hierarchy_map.parent_id = @account_id 
						and account.account_level_code = 2
						and account.deleted = 0
						and account.ad_hoc_flag = 0
    if @breachOpen = 1
    begin
        insert into #mdparams_cmpl_rpt_case_stats (parm, parm_value)
        select 'breach_status', cmpl_case_state_id
        from cmpl_case_state
        where name = 'Open';
    end;
    if @breachAsleep = 1
    begin
        insert into #mdparams_cmpl_rpt_case_stats (parm, parm_value)
        select 'breach_status',cmpl_case_state_id
        from cmpl_case_state
        where name = 'Asleep';
    end;
    if @breachResolved = 1
    begin
        insert into #mdparams_cmpl_rpt_case_stats (parm, parm_value)
        select 'breach_status',cmpl_case_state_id
        from cmpl_case_state where name = 'Resolved';
    end;
    if @breachClosed = 1
    begin
        insert into #mdparams_cmpl_rpt_case_stats (parm, parm_value)
        select 'breach_status',cmpl_case_state_id
        from cmpl_case_state where name = 'Closed';
    end;
  insert into #cases_cmpl_rpt_case_stats (cmpl_case_id, created_time, current_error_level)
  select
       cmpl_case.cmpl_case_id,
       min(cmpl_case_action.created_time) as created_time,
       cmpl_case.calc_worst_error_level
  from
       cmpl_case
       join cmpl_case_action 
              on cmpl_case.cmpl_case_id = cmpl_case_action.cmpl_case_id
       join cmpl_case_invocation 
              on cmpl_case.latest_cmpl_case_invocation_id = cmpl_case_invocation.cmpl_case_invocation_id
    join cmpl_profile_rule 
              on cmpl_case.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
       join cmpl_ruleset_rule
              on cmpl_profile_rule.cmpl_ruleset_rule_id = cmpl_ruleset_rule.cmpl_ruleset_rule_id
    join account 
              on cmpl_case.compliance_account_id = account.account_id
    join cmpl_rule 
              on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
    join cmpl_profile_ruleset 
              on cmpl_profile_rule.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
    join cmpl_ruleset 
              on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
    join cmpl_case_state 
              on cmpl_case.cmpl_case_state_id = cmpl_case_state.cmpl_case_state_id
  where
           ((cmpl_case.compliance_account_id in
                        (select account_id
                         from #account)) )
            and cmpl_case_state.cmpl_case_state_id in
                        (select parm_value
                         from #mdparams_cmpl_rpt_case_stats
                         where parm = 'breach_status')
            and cmpl_case.calc_worst_error_level >= @worstSeverityError
            and cmpl_case_invocation.calc_worst_error_level >= @currentSeverityError
            and (cmpl_case.active = @activePassive or @activePassive = 3)
            and (cmpl_case.reviewed = @reviewed or @reviewed = 3)
            and ( COALESCE(cmpl_profile_rule.at_time_of_purchase, cmpl_ruleset_rule.at_time_of_purchase) = @atop or @atop=3)
            and (account.manager = @manager
                or
                 @manager = -1
                )
            and (cmpl_ruleset.cmpl_ruleset_id = @ruleSet or @ruleSet = -1)
            and (cmpl_rule.cmpl_rule_id = @rule or @rule = -1)
  group by
       cmpl_case.cmpl_case_id,
       cmpl_case.calc_worst_error_level;
  insert into #casebckt_cmpl_rpt_case_stats (cmpl_error_level)
  select error_level
  from restriction_severity
  where restriction_severity.error_level <> 0 and restriction_severity.error_level<3;
  update #casebckt_cmpl_rpt_case_stats
       set Today =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) = 0 and #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level),
       Days1To5 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 1 and 5  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level),
       Days6To10 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 6 and 10  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level),
       Days11To20 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 11 and 20  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level),
       Days21To30 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 21 and 30  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level),
       Days31Plus =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) > 30  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level),
       Total =
              (select count(*) from #cases_cmpl_rpt_case_stats where #casebckt_cmpl_rpt_case_stats.cmpl_error_level = #cases_cmpl_rpt_case_stats.current_error_level)
       where cmpl_error_level = 1;
update #casebckt_cmpl_rpt_case_stats
       set Today =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) = 0 and #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level),
       Days1To5 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 1 and 5  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level),
       Days6To10 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 6 and 10  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level),
       Days11To20 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 11 and 20  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level),
       Days21To30 =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) between 21 and 30  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level),
       Days31Plus =
              (select count(*) from #cases_cmpl_rpt_case_stats where datediff(dd,  #cases_cmpl_rpt_case_stats.created_time,  getdate()) > 30  and #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level),
       Total =
              (select count(*) from #cases_cmpl_rpt_case_stats where #casebckt_cmpl_rpt_case_stats.cmpl_error_level <= #cases_cmpl_rpt_case_stats.current_error_level)
       where cmpl_error_level >= 2;
       select @resultCount = count(*)
       from #casebckt_cmpl_rpt_case_stats;

         
         insert into #prior_to_pivit
       
              SELECT
                                         
                     case when cmpl_error_level = 1 then 'Warning'
                     else 'Fail'
                     end as description,
                     Today as 'Today',
                     Days1To5 as '1 -5 Days',
                     Days6To10 as '6 -10 Days',
                     Days11To20 as '10 -20 Days',
                     Days21To30 as '21 -30 Days',
                     Days31Plus as '31 Plus Days',
                     Total as 'Total'
       from #casebckt_cmpl_rpt_case_stats
       join restriction_severity on #casebckt_cmpl_rpt_case_stats.cmpl_error_level = restriction_severity.error_level
       order by cmpl_error_level asc;


	   with unpvt as 
           (select decription,period, cnt
              from ( select decription, Today,Days1To5,Days6To10, Days11To20, Days21To30, Days31Plus, Total 
			          from #prior_to_pivit ) p
                     unpivot ( cnt for period  in ( Today,Days1To5,Days6To10, Days11To20, Days21To30, Days31Plus, Total )
		           )  as unp
            )
       select period, sum(case when decription='Warning' then cnt else 0 end) Warning, 
              sum(case when decription='Fail' then cnt else 0 end) Fail
        From unpvt
       Group by period
	   order by case when period='Today' then 1  
	                 when period='Days1To5'  then  2 
					 when period='Days6To10'  then 3  
					 when period='Days11To20' then 4 
					 when period='Days21To30' then 5
					 when period='Days31Plus' then 6   
					 when period='Total' then 7 end;

end


go
if @@error = 0 print 'PROCEDURE: se_cmpl_case_statistics created'
else print 'PROCEDURE: se_cmpl_case_statistics error on creation'
go