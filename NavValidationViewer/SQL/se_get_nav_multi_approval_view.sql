--exec se_get_nav_multi_approval_view @account_id=6,@nav_date='2017-08-04 00:00:00',@control_type_code=-1,@loadhist_definition_id=4,@current_user=41,@focusfail = 0,@focusData = 0

if exists (select * from sysobjects where name = 'se_get_nav_multi_approval_view')
begin
	drop procedure se_get_nav_multi_approval_view
	print 'PROCEDURE: se_get_nav_multi_approval_view dropped'
end
go
create procedure se_get_nav_multi_approval_view
(   @account_id 				numeric(10),
	@nav_date					datetime,
	@control_type_code          numeric(10),
	@loadhist_definition_id		numeric(10),
	@current_user				numeric(10),
	@focusfail                  bit,
	@focusData                  bit
)
as
	declare @ret_val int;
	declare @loadhist_def_name	nvarchar(255);
	declare @asof_time			datetime;
	declare @asof_hour			smallint;
	declare @asof_minute		smallint;
	declare @account_level_code	tinyint;
	declare @nb_childs			smallint;
	declare @child				numeric(10);
	declare @count				tinyint;
	declare @first_comment_data	nvarchar(255);
	declare @second_comment_data	nvarchar(255);
	declare @dq_comment_data	nvarchar(255);
	declare @account_id_local	numeric(10);
	declare @resource_lock_id	numeric(10);
	declare @latest_invocation_id_local numeric(10);
	declare @nav_date_string	nvarchar(255);
	declare @resource_lock_user nvarchar(255);
	declare @result_id_local numeric(10);
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
	select @loadhist_def_name = mnemonic,
		   @asof_hour = asof_hour,
		   @asof_minute = asof_minute
	from loadhist_definition
	where loadhist_definition_id = @loadhist_definition_id;
	select @asof_time = @nav_date;
	select @asof_time = dateadd(hh, @asof_hour, @asof_time);
	select @asof_time = dateadd(mi, @asof_minute, @asof_time);
	select @nav_date_string = convert(nvarchar(10), @nav_date,  101);
	select @account_level_code = account_level_code from account where account_id = @account_id;
	create table #account_invocation     (  	account_id       numeric(10) not      null , 	
	latest_invocation_id           numeric(10)      null , 	
	process_id numeric(10),
	resource_lock_id	numeric(10)      null , 	resource_lock_user	nvarchar(255)      null  	); 
	create table #account_invocation_without_process     (  	account_id       numeric(10) not      null , 	
	latest_invocation_id           numeric(10)      null , 	
	process_id numeric(10),
	resource_lock_id	numeric(10)      null , 	resource_lock_user	nvarchar(255)      null  	); 
	
	create table #account_childs     (  	account_id              numeric(10) not      null , 	account_type			numeric(10) not      null  	);
	create table #rule_result_comment     (  	rule_result_id              numeric(10) not      null , 	rule_first_validation_comment	nvarchar(255) 	null , 	rule_second_validation_comment	nvarchar(255) 	null  	);
	create table #ruleset_result_comment     (  	ruleset_result_id              numeric(10) not      null , 	first_validation_comment	nvarchar(255) 	null , 	second_validation_comment	nvarchar(255) 	null , 	dq_comment_data	nvarchar(255)	null  	);
	if  (@account_level_code = 2)
	begin
		insert into #account_invocation( account_id, latest_invocation_id,process_id, resource_lock_id, resource_lock_user )
		select nav_res_ruleset_review.account_id,
			 max(cmpl_invocation.cmpl_invocation_id)
			 ,nav_account_process.nav_account_process_id
			 , null, null
		from cmpl_invocation
			join nav_res_ruleset_review on cmpl_invocation.cmpl_invocation_id = nav_res_ruleset_review.cmpl_invocation_id
			   and nav_res_ruleset_review.account_id = @account_id
			join nav_account_process on nav_account_process.latest_cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
			   and nav_account_process.asof_time = @asof_time
			   and nav_account_process.account_id = @account_id
		where cmpl_invocation.asof_time = @asof_time
		and cmpl_invocation.test_mode = 0
		and cmpl_invocation.nav_mode = 1
		group by nav_res_ruleset_review.account_id,nav_account_process.nav_account_process_id;

		insert into #account_invocation_without_process( account_id, latest_invocation_id, resource_lock_id, resource_lock_user )
		select nav_res_ruleset_review.account_id,
			 max(cmpl_invocation.cmpl_invocation_id)
			 
			 , null, null
		from cmpl_invocation
			join nav_res_ruleset_review on cmpl_invocation.cmpl_invocation_id = nav_res_ruleset_review.cmpl_invocation_id
			   and nav_res_ruleset_review.account_id = @account_id
		
		where cmpl_invocation.asof_time = @asof_time
		and cmpl_invocation.test_mode = 0
		and cmpl_invocation.nav_mode = 1
		group by nav_res_ruleset_review.account_id;
	end else if @account_level_code = 1 begin
		insert into #account_childs values (@account_id, 2);
		while exists(select account_id from #account_childs where account_type = 2) begin
			select @nb_childs = count(distinct account_id) from #account_childs;
			select @count = 0;
			WHILE(@count < @nb_childs)
			begin
				select @child = min(account_id) from #account_childs where account_type = 2;
				insert into #account_childs (account_id, account_type)
				select child_id, child_type
				from account_hierarchy 
				where parent_id = @child;
				delete from #account_childs where account_id = @child and account_type = 2;
				select @count = @count + 1;
			end;
		end;
		insert into #account_invocation( account_id, latest_invocation_id,process_id )
		select nav_res_ruleset_review.account_id,
			max(cmpl_invocation.cmpl_invocation_id)
			,nav_account_process.nav_account_process_id
		from cmpl_invocation
			join nav_res_ruleset_review on cmpl_invocation.cmpl_invocation_id = nav_res_ruleset_review.cmpl_invocation_id
			join #account_childs on nav_res_ruleset_review.account_id = #account_childs.account_id
			join nav_account_process on nav_account_process.latest_cmpl_invocation_id = cmpl_invocation.cmpl_invocation_id
			   and nav_account_process.asof_time = @asof_time
			   and nav_account_process.account_id = #account_childs.account_id
			
		where cmpl_invocation.asof_time = @asof_time 
			and cmpl_invocation.test_mode = 0 
			and cmpl_invocation.nav_mode = 1
		group by nav_res_ruleset_review.account_id,nav_account_process.nav_account_process_id;
		insert into #account_invocation_without_process( account_id, latest_invocation_id )
		select nav_res_ruleset_review.account_id,
			max(cmpl_invocation.cmpl_invocation_id)
			
		from cmpl_invocation
			join nav_res_ruleset_review on cmpl_invocation.cmpl_invocation_id = nav_res_ruleset_review.cmpl_invocation_id
			join #account_childs on nav_res_ruleset_review.account_id = #account_childs.account_id
		
			
		where cmpl_invocation.asof_time = @asof_time 
			and cmpl_invocation.test_mode = 0 
			and cmpl_invocation.nav_mode = 1
		group by nav_res_ruleset_review.account_id
	end;

		--select * from #account_invocation
		--select * from #account_invocation_without_process


--	select * from #account_invocation
declare cur_comment cursor  for 
	select #account_invocation_without_process.account_id, #account_invocation_without_process.latest_invocation_id
	from  #account_invocation_without_process;
		open cur_comment;
		fetch cur_comment 
		into  @account_id_local, @latest_invocation_id_local;
while ((@@fetch_status <> -1))
		begin
		begin
			if(@latest_invocation_id_local is not null)
			begin
		
				update nav_account_process
				set nav_process_status_code = 8,
					nav_process_action_code = 5,
					modified_by = @current_user
				where account_id = @account_id_local 
				and asof_time = @asof_time
				and nav_process_status_code in (10);
				insert into #rule_result_comment (rule_result_id)
				select distinct nav_res_rule_result.nav_res_rule_result_id
				from  #account_invocation_without_process
				  join nav_res_ruleset_review on nav_res_ruleset_review.cmpl_invocation_id = #account_invocation_without_process.latest_invocation_id 
				  and nav_res_ruleset_review.account_id = #account_invocation_without_process.account_id
				  join nav_res_rule_result on nav_res_ruleset_review.nav_res_ruleset_review_id = nav_res_rule_result.nav_res_ruleset_review_id
				 join cmpl_profile_ruleset on cmpl_profile_ruleset.cmpl_profile_ruleset_id = nav_res_ruleset_review.cmpl_profile_ruleset_id
			
				where  #account_invocation_without_process.latest_invocation_id = @latest_invocation_id_local and #account_invocation_without_process.account_id = @account_id_local;
				declare cur_comment_rule cursor  for 
									select #rule_result_comment.rule_result_id
					from  #rule_result_comment;
						open cur_comment_rule;
						fetch cur_comment_rule 
						into @result_id_local ;
				while ((@@fetch_status <> -1))
						begin
						begin
							select @first_comment_data = '';
							select @first_comment_data = coalesce (@first_comment_data, '') + coalesce(cmpl_long_text_data.data,'') 
							from  cmpl_long_text_data
								join nav_res_rule_result on nav_res_rule_result.first_rev_comment_text_id = cmpl_long_text_data.cmpl_long_text_id
							where  nav_res_rule_result.nav_res_rule_result_id = @result_id_local;
							select @second_comment_data = '';
							select @second_comment_data = coalesce (@second_comment_data, '') + coalesce(cmpl_long_text_data.data,'') 
							from  cmpl_long_text_data
								join nav_res_rule_result on nav_res_rule_result.second_rev_comment_text_id = cmpl_long_text_data.cmpl_long_text_id
							where  nav_res_rule_result.nav_res_rule_result_id = @result_id_local;
							update #rule_result_comment set #rule_result_comment.rule_first_validation_comment = @first_comment_data,
								#rule_result_comment.rule_second_validation_comment = @second_comment_data 
							where #rule_result_comment.rule_result_id = @result_id_local;
					   end;
						fetch cur_comment_rule 
						into @result_id_local;
					end;
						close cur_comment_rule;
				deallocate cur_comment_rule;
				insert into #ruleset_result_comment (ruleset_result_id)
				select distinct nav_res_ruleset_review.nav_res_ruleset_review_id
				from  #account_invocation_without_process
				  join nav_res_ruleset_review on nav_res_ruleset_review.cmpl_invocation_id = #account_invocation_without_process.latest_invocation_id 
				  and nav_res_ruleset_review.account_id = #account_invocation_without_process.account_id
				   join cmpl_profile_ruleset on cmpl_profile_ruleset.cmpl_profile_ruleset_id = nav_res_ruleset_review.cmpl_profile_ruleset_id
			
				where #account_invocation_without_process.latest_invocation_id = @latest_invocation_id_local and #account_invocation_without_process.account_id = @account_id_local;
			declare cur_comment_ruleset cursor  for 
							select #ruleset_result_comment.ruleset_result_id
				from  #ruleset_result_comment;
					open cur_comment_ruleset;
					fetch cur_comment_ruleset 
					into @result_id_local ;
			while ((@@fetch_status <> -1))
					begin
					begin
						select @first_comment_data = '';
						select @first_comment_data = coalesce (@first_comment_data, '') + coalesce(cmpl_long_text_data.data,'') 
						from nav_res_ruleset_review
							join cmpl_long_text_data on cmpl_long_text_data.cmpl_long_text_id = nav_res_ruleset_review.first_rev_comment_text_id
						where nav_res_ruleset_review.nav_res_ruleset_review_id = @result_id_local;
						select @second_comment_data = '';
						select @second_comment_data = coalesce(@second_comment_data, '') + coalesce( cmpl_long_text_data.data, '') 
						from nav_res_ruleset_review
							join cmpl_long_text_data on cmpl_long_text_data.cmpl_long_text_id = nav_res_ruleset_review.second_rev_comment_text_id
						where nav_res_ruleset_review.nav_res_ruleset_review_id = @result_id_local;
						select @dq_comment_data = '';
						select @dq_comment_data = coalesce (@dq_comment_data, '') + coalesce( cmpl_long_text_data.data, '') 
						from   nav_res_ruleset_review
							join cmpl_long_text_data on cmpl_long_text_data.cmpl_long_text_id = nav_res_ruleset_review.dq_rev_comment_text_id
						 where nav_res_ruleset_review.nav_res_ruleset_review_id = @result_id_local;
						update #ruleset_result_comment set first_validation_comment =  @first_comment_data, 
										second_validation_comment =  @second_comment_data,
										dq_comment_data =  @dq_comment_data
						where  #ruleset_result_comment.ruleset_result_id = @result_id_local;
				 end;
					fetch cur_comment_ruleset 
					into @result_id_local;
				end;
					close cur_comment_ruleset;
			deallocate cur_comment_ruleset;
			end;
       end;
		fetch cur_comment 
		into @account_id_local, @latest_invocation_id_local;
    end;
		close cur_comment;
deallocate cur_comment;

--select * from #rule_result_comment
--select * from #ruleset_result_comment
--select * into pkn_account_invocation from #account_invocation;

print @nav_date_string
print @loadhist_def_name

	select  
	 --   nav_account_process.nav_account_process_id,
		--nav_account_process.nav_control_type_code,
		--nav_control_type.name as control_type,
	 --   nav_res_ruleset_review.cmpl_invocation_id, 
		@loadhist_def_name as loadhist_def_name, 
		(select short_name from account where account_id= #account_invocation.account_id) as account_short_name,
		nav_res_ruleset_review.account_id as account_id,
		nav_res_ruleset_review.nav_status_ok,
		cmpl_ruleset.cmpl_ruleset_id,
		nav_res_rule_result.nav_res_rule_result_id,
		nav_res_rule_result.nav_res_ruleset_review_id,
		nav_res_rule_result.cmpl_profile_rule_id,
		cmpl_rule.cmpl_rule_id,
		cmpl_rule.display_name as rule_name, 
		cmpl_rule.comments as rule_comment,
		cmpl_ruleset.name as ruleset_name, 
		cmpl_rule.description as rule_description,	
		nav_res_rule_result.nav_rule_status_code,
		nav_res_rule_result.nav_data_quality_status_code,
		nav_res_rule_result.global_effect,
		nav_res_rule_result.first_rev_comment_text_id as rule_first_rev_comment_id,
		#rule_result_comment.rule_first_validation_comment as rule_first_rev_comment,
		nav_res_rule_result.second_rev_comment_text_id as rule_second_rev_comment_id,
	    #rule_result_comment.rule_second_validation_comment as rule_second_rev_comment,
		nav_res_ruleset_review.first_rev_comment_text_id as ruleset_first_rev_comment_id,
		#ruleset_result_comment.first_validation_comment as ruleset_first_rev_comment,
		nav_res_ruleset_review.second_rev_comment_text_id as ruleset_second_rev_comment_id,
		#ruleset_result_comment.second_validation_comment as ruleset_second_rev_comment,
		nav_res_rule_result.suppress_status,
		nav_rule_status.name as rule_status_name,
		nav_data_quality_status.name as data_quality_status_name,
		#ruleset_result_comment.dq_comment_data  as dq_rev_comment_text,
	

		nav_process_status.name as nav_process_status_name,
		nav_account_process.calc_worst_rule_status,
		nav_account_process.calc_worst_data_qlty_status,
		nav_account_process.nav_control_type_code,
		nav_control_type.name as control_type,
		ui1.name as first_rev_by_name,
		ui2.name as second_rev_by_name,
		ui_dq.name as dq_rev_by_name,
		--(select short_name 
		--from account
		--join account_hierarchy on account_hierarchy.parent_id = account.account_id							  						   
		--where account_hierarchy.child_id = #account_invocation.account_id  
		--and account.account_level_code = 1	
		--and account.account_id <> #account_invocation.account_id) as account_group_name,
		(select   resource_lock_id from resource_lock
			join user_instance on user_instance.user_instance_id = resource_lock.user_instance_id
		where resource_lock.key_id1 = nav_res_ruleset_review.account_id
		and resource_lock.resource_lock_type_code = 2 and resource_lock.key_str1 = @nav_date_string
		union
		select resource_lock_id from resource_lock
			join user_instance_history on user_instance_history.user_instance_id = resource_lock.user_instance_id
		where resource_lock.key_id1 = nav_res_ruleset_review.account_id
		and resource_lock.resource_lock_type_code = 2 and resource_lock.key_str1 = @nav_date_string) as resource_lock_id,
		#account_invocation.resource_lock_user as resource_lock_user,
		convert(bit, (case when (#account_invocation.resource_lock_id is null 
								and #account_invocation.latest_invocation_id is not null) 
							then 1 
							else 0 
							end)) as is_locked
	from nav_res_ruleset_review
		join nav_res_rule_result on nav_res_ruleset_review.nav_res_ruleset_review_id = nav_res_rule_result.nav_res_ruleset_review_id
		join #rule_result_comment on #rule_result_comment.rule_result_id = nav_res_rule_result.nav_res_rule_result_id
		
		join cmpl_profile_rule on nav_res_rule_result.cmpl_profile_rule_id = cmpl_profile_rule.cmpl_profile_rule_id
		join cmpl_profile_ruleset on nav_res_ruleset_review.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
		join cmpl_ruleset on cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
		join cmpl_rule on cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
		--sjd
		join #ruleset_result_comment 
		on #ruleset_result_comment.ruleset_result_id = nav_res_ruleset_review.nav_res_ruleset_review_id
		and nav_res_ruleset_review.cmpl_profile_ruleset_id = cmpl_profile_ruleset.cmpl_profile_ruleset_id
		join nav_rule_status on nav_rule_status.nav_rule_status_code=nav_res_rule_result.nav_rule_status_code
		join nav_data_quality_status on nav_data_quality_status.nav_data_quality_status_code=nav_res_rule_result.nav_data_quality_status_code
		join #account_invocation on #account_invocation.latest_invocation_id= nav_res_ruleset_review.cmpl_invocation_id
		and #account_invocation.account_id=nav_res_ruleset_review.account_id
		left outer join user_info ui1 on nav_res_ruleset_review.first_rev_by = ui1.user_id
		left outer join user_info ui2 on nav_res_ruleset_review.second_rev_by = ui2.user_id
		left outer join user_info ui_dq on nav_res_ruleset_review.dq_rev_by = ui_dq.user_id
		join cmpl_invocation on cmpl_invocation.cmpl_invocation_id = #account_invocation.latest_invocation_id
		join nav_account_process on nav_account_process.nav_account_process_id = #account_invocation.process_id
		   --nav_res_ruleset_review.cmpl_invocation_id = nav_account_process.latest_cmpl_invocation_id 
			and  nav_account_process.account_id= #account_invocation.account_id
			--and nav_account_process.nav_account_process_id = #account_invocation.process_id
		join nav_process_status on nav_account_process.nav_process_status_code = nav_process_status.nav_process_status_code
		join nav_control_type on cmpl_ruleset.nav_control_type = nav_control_type.nav_control_type_code
		and nav_control_type.nav_control_type_code =nav_account_process.nav_control_type_code
		
		where 
		(nav_control_type.nav_control_type_code = @control_type_code or @control_type_code = -1)
		and((@focusfail = 1 and nav_res_rule_result.nav_rule_status_code in (3,4)) or @focusfail = 0)
		and ((@focusData = 1 and nav_res_rule_result.nav_data_quality_status_code in (4)) or @focusData = 0)
		and nav_account_process.account_id in (select #account_invocation.account_id)
		and cmpl_invocation.cmpl_invocation_id = #account_invocation.latest_invocation_id
		and nav_res_ruleset_review.account_id in (select account_id from #account_invocation)
		and #account_invocation.process_id = nav_account_process.nav_account_process_id
	   order by nav_res_ruleset_review.account_id;
end
go
if @@error = 0 print 'PROCEDURE: se_get_nav_multi_approval_view created'
else print 'PROCEDURE: se_get_nav_multi_approval_view error on creation'
go
