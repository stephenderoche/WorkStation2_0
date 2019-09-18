if exists (select * from sysobjects where name = 'se_run_rules')
begin
	drop procedure se_run_rules
	print 'PROCEDURE: se_run_rules dropped'
end
go
create procedure [dbo].[se_run_rules]--exec se_run_rules 2, 9       
(	@account_id                       numeric(10),
    @current_user				  	  smallint
)
as
	
begin
                        set nocount on;
                        declare @ec__errno int;
                        declare @sp_initial_trancount int;
                        declare @sp_trancount int;
						declare @account_name					nvarchar(40);
						declare @isAccountId                  smallint = 1;

	create table #account  	(  		account_id numeric(10) not null  	);

	if @account_id = -1
	begin
		insert into #account (account_id)
		select account.account_id 
		from user_access_map 
		join account 
			on user_access_map.object_id = account.account_id
			and account.deleted = 0
		where user_access_map.user_id = @current_user
			and user_access_map.object_type in (3, 2);
		select @account_name = 'All' ;
	end else begin
		if @isAccountId = 0
		begin
			insert into #account (account_id)
			select 
				workgroup_tree_map.child_id
			from workgroup_tree_map
			join account
				on workgroup_tree_map.child_id = account.account_id
				and account.deleted = 0
				and account.ad_hoc_flag = 0
			where workgroup_tree_map.parent_id = @account_id
				and workgroup_tree_map.child_type in (3, 2);
			select 
				@account_name = workgroup.name
			from workgroup
			where workgroup.workgroup_id = @account_id;
		end else begin
			if @isAccountId = 1
			begin
				insert into #account (account_id)
				select account.account_id
				from account_hierarchy_map
				join account 
					on account_hierarchy_map.child_id = account.account_id
					and account.deleted = 0
				where account_hierarchy_map.parent_id = @account_id
					and account.ad_hoc_flag = 0;
			end else begin
				insert into #account (account_id) values (@account_id);
			end;
			select @account_name = account.short_name 
			from account 
			where account.account_id = @account_id;
		end;
	end;


   ----Run Rules

   --rule id 1
   exec se_rule_Donotreverseatradethatwasplacedwithinthelast @account_id, 1, -1, -1, @current_user, ACCT, 0,'Do not reverse a trade that was placed within the last' 
   --rule id 2 
   exec se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays @account_id, 1, -1, -1, @current_user, ACCT, 0,'Do not sell a security going long term at a >$X gain in the next X days'  
   --rule id 3
   exec se_rule_DNRtradeplacedinlastXdaysunlesspricefluctuatedxpercent @account_id, 1, -1, -1, @current_user, ACCT, 0,'Do not reverse a trade that was placed within the last X days unless the price has fluctuated to the clients favor by >x%'        
   --truncate table se_restricted_security
  --rule id 4
  exec se_rule_DNBSsamesidesymbolinxdaysunlesspricefluctuatedXpercent @account_id, 1, -1, -1, @current_user, ACCT, 0,'DN BS  same side/symbol within x days unless the price has fluctuated to the clients favor by > X'        
   --rule id 5
   exec se_rule_washsale @account_id, 1, -1, -1, @current_user, ACCT, 0,'washSale Pre'  
   
   select * from se_restricted_security
	
end


go
if @@error = 0 print 'PROCEDURE: se_run_rules created'
else print 'PROCEDURE: se_run_rules error on creation'
go

