if exists (select * from sysobjects where name = 'se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays')
begin
	drop procedure se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays
	print 'PROCEDURE: se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays dropped'
end
go
create PROCEDURE [dbo].[se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays] (
	@acctID NUMERIC(10)
	,@isAccountId SMALLINT = 1
	,@ruleSet NUMERIC(10)
	,@rule NUMERIC(10)
	,@userId SMALLINT
	,@sort_order NVARCHAR(40)
	,@show_agg_rule_for_account SMALLINT = 1
	,@description VARCHAR(200)
	)
AS
DECLARE @ret_val INT;
DECLARE @continue_flag INT;
DECLARE @cps_rpx_rule_details_by_acct NVARCHAR(30);
DECLARE @cpe_rpx_rule_details_by_acct NVARCHAR(30);
DECLARE @account_name NVARCHAR(40);
DECLARE @cmpl_profile_rule_id NUMERIC(10);
DECLARE @rule_name NVARCHAR(255);
DECLARE @p_value NVARCHAR(255);
DECLARE @p_value_original NVARCHAR(255);
DECLARE @cmpl_param_clr_type_name NVARCHAR(40);
DECLARE @cmpl_param_type_id NUMERIC(10);
DECLARE @cmpl_rule_param_id NUMERIC(10);

BEGIN
	/*

Do not sell a security going long term at a >$500 gain in the next 60 days.

*/
	SET NOCOUNT ON;

	DECLARE @ec__errno INT;
	DECLARE @sp_initial_trancount INT;
	DECLARE @sp_trancount INT;

	CREATE TABLE #legal_param_values (
		actual_value NVARCHAR(255) NOT NULL
		,display_value NVARCHAR(255) NOT NULL
		);

	CREATE TABLE #profiles_rpt_asgn_rulebyacct (
		account_id NUMERIC(10) NULL
		,cmpl_profile_id NUMERIC(10) NULL
		,cmpl_profile_type_id NUMERIC(10) NULL
		);

	CREATE TABLE #results_rpt_asgn_rulebyacct (
		account_id NUMERIC(10) NULL
		,cmpl_profile_type_id NUMERIC(10) NULL
		,cmpl_profile_rule_id NUMERIC(10) NULL
		,manager INT NULL
		,manager_name NVARCHAR(40) NULL
		,account_name NVARCHAR(40) NULL
		,severity NVARCHAR(100)
		,display_name NVARCHAR(255) NULL
		,description NVARCHAR(255) NULL
		,run_type NVARCHAR(40) NULL
		,modified_date NVARCHAR(10)
		,modified_by NVARCHAR(40) NULL
		,ruleset_name NVARCHAR(255) NULL
		,profile_type NVARCHAR(100)
		,ancestor_account NVARCHAR(40) NULL
		,profile_comments NVARCHAR(255) NULL
		,ruleset_comments NVARCHAR(255) NULL
		,cmpl_rule_id NUMERIC(10) NULL
		,cmpl_ruleset_rule_id NUMERIC(10) NULL
		);

	CREATE TABLE #result_rpt_asgn_parambyacct (
		cmpl_rule_id NUMERIC(10) NOT NULL
		,display_name NVARCHAR(255) NULL
		,cmpl_param_name NVARCHAR(255) NULL
		,cmpl_param_type_name NVARCHAR(255) NULL
		,cmpl_param_value NVARCHAR(255) NULL
		,cmpl_param_desc NVARCHAR(255) NULL
		,cmpl_param_list_proc NVARCHAR(255) NULL
		,cmpl_param_type_id NUMERIC(10) NOT NULL
		,cmpl_rule_param_id NUMERIC(10) NOT NULL
		,cmpl_profile_rule_id NUMERIC(10) NOT NULL
		,cmpl_ruleset_rule_id NUMERIC(10) NOT NULL
		,account_id NUMERIC(10) NOT NULL
		);

	CREATE TABLE #cmpl_profile_display_value (display_value NVARCHAR(255) NULL);

	
		INSERT INTO #profiles_rpt_asgn_rulebyacct (
			account_id
			,cmpl_profile_id
			,cmpl_profile_type_id
			)
		SELECT DISTINCT ahm_to_descendants.child_id AS account_id
			,cmpl_account_profile.cmpl_profile_id
			,cmpl_account_profile.cmpl_profile_type_id
		FROM account_hierarchy_map ahm_to_descendants
		JOIN account ON ahm_to_descendants.child_id = account.account_id
		JOIN account_hierarchy_map ahm_to_ancestors ON ahm_to_descendants.child_id = ahm_to_ancestors.child_id
		JOIN cmpl_account_profile ON ahm_to_ancestors.parent_id = cmpl_account_profile.account_id
		JOIN cmpl_profile ON cmpl_account_profile.cmpl_profile_id = cmpl_profile.cmpl_profile_id
		WHERE cmpl_profile.deleted = 0
			and   ahm_to_descendants.parent_id= @acctID;
	

	INSERT INTO #results_rpt_asgn_rulebyacct
	SELECT #profiles_rpt_asgn_rulebyacct.account_id
		,#profiles_rpt_asgn_rulebyacct.cmpl_profile_type_id
		,cmpl_profile_rule.cmpl_profile_rule_id
		,account.manager
		,user_info.name
		,account.name_1
		,COALESCE(stat_conv_pre_1.description, stat_conv_pre_2.description) + '/' + NCHAR(13) + COALESCE(stat_conv_post_1.description, stat_conv_post_2.description) AS severity
		,cmpl_rule.display_name
		,cmpl_rule.description
		,CASE 
			WHEN COALESCE(cmpl_profile_rule.run_pre_trade, cmpl_ruleset_rule.run_pre_trade) = 1
				AND COALESCE(cmpl_profile_rule.run_post_trade, cmpl_ruleset_rule.run_post_trade) = 1
				THEN 'Pre/Post'
			WHEN COALESCE(cmpl_profile_rule.run_pre_trade, cmpl_ruleset_rule.run_pre_trade) = 0
				AND COALESCE(cmpl_profile_rule.run_post_trade, cmpl_ruleset_rule.run_post_trade) = 1
				THEN 'Post'
			WHEN COALESCE(cmpl_profile_rule.run_pre_trade, cmpl_ruleset_rule.run_pre_trade) = 1
				AND COALESCE(cmpl_profile_rule.run_post_trade, cmpl_ruleset_rule.run_post_trade) = 0
				THEN 'Pre'
			ELSE 'None'
			END
		,convert(NCHAR(10), cmpl_profile_rule.modified_time, 101)
		,mod_by.name
		,cmpl_ruleset.name AS ruleset_name
		,cmpl_profile_type.name
		,a1.short_name
		,cmpl_profile_rule.comments AS profile_comments
		,cmpl_ruleset_rule.comments AS ruleset_comments
		,cmpl_rule.cmpl_rule_id
		,cmpl_profile_rule.cmpl_ruleset_rule_id
	FROM #profiles_rpt_asgn_rulebyacct
	JOIN cmpl_profile_ruleset ON #profiles_rpt_asgn_rulebyacct.cmpl_profile_id = cmpl_profile_ruleset.cmpl_profile_id
		AND cmpl_profile_ruleset.deleted = 0
	JOIN cmpl_profile_rule ON cmpl_profile_ruleset.cmpl_profile_ruleset_id = cmpl_profile_rule.cmpl_profile_ruleset_id
		AND cmpl_profile_rule.deleted = 0
	JOIN account ON #profiles_rpt_asgn_rulebyacct.account_id = account.account_id
	LEFT OUTER JOIN user_info ON account.manager = user_info.user_id
	JOIN cmpl_ruleset_rule ON cmpl_ruleset_rule.cmpl_ruleset_rule_id = cmpl_profile_rule.cmpl_ruleset_rule_id
	LEFT OUTER JOIN cmpl_status_conversion stat_conv_pre_1 ON cmpl_profile_rule.cmpl_status_conversion_id = stat_conv_pre_1.cmpl_status_conversion_id
	LEFT OUTER JOIN cmpl_status_conversion stat_conv_pre_2 ON cmpl_ruleset_rule.cmpl_status_conversion_id = stat_conv_pre_2.cmpl_status_conversion_id
	LEFT OUTER JOIN cmpl_status_conversion stat_conv_post_1 ON cmpl_profile_rule.cmpl_post_status_conv_id = stat_conv_post_1.cmpl_status_conversion_id
	LEFT OUTER JOIN cmpl_status_conversion stat_conv_post_2 ON cmpl_ruleset_rule.cmpl_post_status_conv_id = stat_conv_post_2.cmpl_status_conversion_id
	JOIN cmpl_rule ON cmpl_profile_rule.cmpl_rule_id = cmpl_rule.cmpl_rule_id
	JOIN cmpl_ruleset ON cmpl_profile_ruleset.cmpl_ruleset_id = cmpl_ruleset.cmpl_ruleset_id
	JOIN user_info mod_by ON mod_by.user_id = cmpl_profile_rule.modified_by
	JOIN cmpl_profile_type ON cmpl_profile_type.cmpl_profile_type_id = #profiles_rpt_asgn_rulebyacct.cmpl_profile_type_id
	JOIN cmpl_account_profile ON cmpl_account_profile.cmpl_account_profile_id = #profiles_rpt_asgn_rulebyacct.cmpl_profile_id
	JOIN account a1 ON a1.account_id = cmpl_account_profile.account_id
	JOIN #account ON account.account_id = #account.account_id
	WHERE (
			cmpl_ruleset.cmpl_ruleset_id = @ruleSet
			OR @ruleSet = - 1
			)
		AND (
			cmpl_rule.cmpl_rule_id = @rule
			OR @rule = - 1
			)
		AND cmpl_rule.description = @description;

	--select * from #results_rpt_asgn_rulebyacct
	INSERT INTO #result_rpt_asgn_parambyacct (
		cmpl_rule_id
		,display_name
		,cmpl_param_name
		,cmpl_param_type_name
		,cmpl_param_value
		,cmpl_param_desc
		,cmpl_param_list_proc
		,cmpl_param_type_id
		,cmpl_rule_param_id
		,cmpl_profile_rule_id
		,cmpl_ruleset_rule_id
		,account_id
		)
	SELECT #assigned_rules.cmpl_rule_id
		,#assigned_rules.display_name
		,cmpl_rule_param.name
		,cmpl_param_type.name
		,CASE 
			WHEN cmpl_param_type_list_attr.cmpl_param_type_id IS NULL
				THEN COALESCE(cmpl_profile_param_value.value, cmpl_rsr_param_value.value, cmpl_rule_param.default_value)
			ELSE list.list_name
			END
		,cmpl_rule_param.description
		,cmpl_param_type_general.legal_value_list_procedure
		,cmpl_rule_param.cmpl_param_type_id
		,cmpl_rule_param.cmpl_rule_param_id
		,#assigned_rules.cmpl_profile_rule_id
		,#assigned_rules.cmpl_ruleset_rule_id
		,#assigned_rules.account_id
	FROM cmpl_rule_param
	JOIN #results_rpt_asgn_rulebyacct #assigned_rules ON #assigned_rules.cmpl_rule_id = cmpl_rule_param.cmpl_rule_id
		AND cmpl_rule_param.deleted = 0
	JOIN cmpl_param_type ON cmpl_param_type.cmpl_param_type_id = cmpl_rule_param.cmpl_param_type_id
	LEFT OUTER JOIN cmpl_profile_param_value ON #assigned_rules.cmpl_profile_rule_id = cmpl_profile_param_value.cmpl_profile_rule_id
		AND #assigned_rules.cmpl_rule_id = cmpl_profile_param_value.cmpl_rule_id
		AND cmpl_rule_param.cmpl_rule_param_id = cmpl_profile_param_value.cmpl_rule_param_id
		AND cmpl_profile_param_value.deleted = 0
	LEFT OUTER JOIN cmpl_rsr_param_value ON #assigned_rules.cmpl_ruleset_rule_id = cmpl_rsr_param_value.cmpl_ruleset_rule_id
		AND #assigned_rules.cmpl_rule_id = cmpl_rsr_param_value.cmpl_rule_id
		AND cmpl_rule_param.cmpl_rule_param_id = cmpl_rsr_param_value.cmpl_rule_param_id
		AND cmpl_rsr_param_value.deleted = 0
	LEFT OUTER JOIN cmpl_param_type_list_attr ON cmpl_param_type_list_attr.cmpl_param_type_id = cmpl_rule_param.cmpl_param_type_id
	LEFT OUTER JOIN list ON convert(NVARCHAR(225), list.list_id) = COALESCE(cmpl_profile_param_value.value, cmpl_rsr_param_value.value, cmpl_rule_param.default_value)
	LEFT OUTER JOIN cmpl_param_type_general ON cmpl_param_type_general.cmpl_param_type_id = cmpl_rule_param.cmpl_param_type_id;

	SELECT @cmpl_profile_rule_id = min(cmpl_profile_rule_id)
	FROM #results_rpt_asgn_rulebyacct;

	WHILE @cmpl_profile_rule_id IS NOT NULL
	BEGIN
		SELECT @rule_name = display_name
		FROM #results_rpt_asgn_rulebyacct
		WHERE cmpl_profile_rule_id = @cmpl_profile_rule_id;

		EXECUTE @ret_val = get_parameterized_rule_name @rule_id = NULL
			,@cmpl_ruleset_rule_id = NULL
			,@cmpl_profile_rule_id = @cmpl_profile_rule_id
			,@rule_name = @rule_name OUTPUT;

		UPDATE #results_rpt_asgn_rulebyacct
		SET display_name = @rule_name
		WHERE cmpl_profile_rule_id = @cmpl_profile_rule_id;

		SELECT @rule_name = description
		FROM #results_rpt_asgn_rulebyacct
		WHERE cmpl_profile_rule_id = @cmpl_profile_rule_id;

		EXECUTE @ret_val = get_parameterized_rule_name @rule_id = NULL
			,@cmpl_ruleset_rule_id = NULL
			,@cmpl_profile_rule_id = @cmpl_profile_rule_id
			,@rule_name = @rule_name OUTPUT;

		UPDATE #results_rpt_asgn_rulebyacct
		SET description = @rule_name
		WHERE cmpl_profile_rule_id = @cmpl_profile_rule_id;

		SELECT @cmpl_profile_rule_id = min(cmpl_profile_rule_id)
		FROM #results_rpt_asgn_rulebyacct
		WHERE cmpl_profile_rule_id > @cmpl_profile_rule_id;
	END;

	SELECT @cmpl_profile_rule_id = min(cmpl_profile_rule_id)
	FROM #result_rpt_asgn_parambyacct;

	WHILE @cmpl_profile_rule_id IS NOT NULL
	BEGIN
		SELECT @cmpl_rule_param_id = min(cmpl_rule_param_id)
		FROM #result_rpt_asgn_parambyacct
		WHERE #result_rpt_asgn_parambyacct.cmpl_profile_rule_id = @cmpl_profile_rule_id;

		WHILE @cmpl_rule_param_id IS NOT NULL
		BEGIN
			SELECT @p_value = cmpl_param_value
				,@p_value_original = cmpl_param_value
				,@cmpl_param_type_id = cmpl_param_type_id
			FROM #result_rpt_asgn_parambyacct
			WHERE #result_rpt_asgn_parambyacct.cmpl_profile_rule_id = @cmpl_profile_rule_id
				AND #result_rpt_asgn_parambyacct.cmpl_rule_param_id = @cmpl_rule_param_id;

			DELETE #legal_param_values;

			EXECUTE @ret_val = get_cmpl_legal_param_values_h @cmpl_param_clr_type_name = @cmpl_param_clr_type_name OUTPUT
				,@cmpl_param_type_id = @cmpl_param_type_id;

			SELECT @p_value = #legal_param_values.display_value
			FROM #legal_param_values
			WHERE #legal_param_values.actual_value = @p_value;

			UPDATE #result_rpt_asgn_parambyacct
			SET cmpl_param_value = @p_value
			WHERE cmpl_profile_rule_id = @cmpl_profile_rule_id
				AND cmpl_param_value = @p_value_original
				AND cmpl_param_type_id = @cmpl_param_type_id;

			SELECT @cmpl_rule_param_id = min(cmpl_rule_param_id)
			FROM #result_rpt_asgn_parambyacct
			WHERE #result_rpt_asgn_parambyacct.cmpl_profile_rule_id = @cmpl_profile_rule_id
				AND #result_rpt_asgn_parambyacct.cmpl_rule_param_id > @cmpl_rule_param_id;
		END;

		SELECT @cmpl_profile_rule_id = min(cmpl_profile_rule_id)
		FROM #result_rpt_asgn_parambyacct
		WHERE cmpl_profile_rule_id > @cmpl_profile_rule_id
			AND cmpl_param_list_proc IS NOT NULL;
	END;

	create table #rule_param_value ( account_id numeric(10), Days numeric(10), Gain Numeric(10));

	insert into #rule_param_value
	select account_id, 
	max(case when cmpl_param_name ='Days' then cmpl_param_value else null end) Days, 
	max(case when cmpl_param_name ='Gain' then cmpl_param_value else null end ) gain  
	from  #result_rpt_asgn_parambyacct
	group by account_id;

	--select * from #rule_param_value

	

	DELETE
	FROM se_restricted_security
	WHERE se_restricted_security.account_id IN (
			SELECT account_id
			FROM #account
			)
		AND se_restricted_security.restriction_type = 1
		AND se_restricted_security.rule_id = 2

   create table #se_restricted_security 
         ( tax_lot_id numeric(10) ,
		   lot_number nvarchar(40),
		    account_id numeric(10), 
		    security_id numeric(10),
			encumber_type numeric(10),
			isEncumbered tinyint,
			exception_date datetime,
			restriction_description nvarchar(1000),
			restriction_type numeric(10),
			rule_id  numeric(10))


insert into
#se_restricted_security
(tax_lot_id,lot_number, account_id,security_id,encumber_type,isEncumbered,exception_date,restriction_description,restriction_type,rule_id)
select tax_lot_id, tax_lot.lot_number
	    ,#rule_param_value.account_id
		,security.security_id
		,27
		,1
		--,tax_lot.lot_number ---tax id
		,tax_lot.trade_date
		,'Do not sell a security going long term  > $' + CONVERT(varchar(40),#rule_param_value.Gain) + ' and DTL < ' + CONVERT(varchar(40),#rule_param_value.Days)
		,1 
		,2
FROM tax_lot
	JOIN security ON security.security_id = tax_lot.security_id
	JOIN price ON security.security_id = price.security_id
	JOIN currency ON currency.security_id = security.principal_currency_id
	JOIN #account ON #account.account_id = tax_lot.account_id
	JOIN #rule_param_value ON #rule_param_value.account_id = tax_lot.account_id
	WHERE datediff(dd,tax_lot.trade_date, getdate())  between 365 - #rule_param_value.days and 365
		AND ((price.latest - tax_lot.unit_cost) / currency.exchange_rate) * tax_lot.quantity > #rule_param_value.gain


	UPDATE tax_lot
	SET encumbered_quantity = Coalesce(quantity,0),
    encumbered_type_code = 27
	FROM  #se_restricted_security
	where tax_lot.tax_lot_id = #se_restricted_security.tax_lot_id

	INSERT INTO se_restricted_security (
	    tax_lot_id
		,account_id
		,security_id
		,encumber_type
		,isEncumbered
		--,
		,exception_date
		,restriction_description
		,restriction_type
		,rule_id
		)

 select lot_number,account_id,security_id,encumber_type,isEncumbered,exception_date,restriction_description,restriction_type,rule_id
   from #se_restricted_security;


END
	--tax_lot.trade_date  >= (GetDate() - (#result_rpt_asgn_parambyacct.cmpl_param_value +1))


go
if @@error = 0 print 'PROCEDURE: se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays created'
else print 'PROCEDURE: se_rule_Donotsellasecuritygoinglongtermata$XgaininthenextXdays error on creation'
go